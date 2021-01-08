
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementHold_upd]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ReimbursementHold_upd];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Oct 2020
-- CR No.:			CRE20-015-02 (Special Support Scheme)
-- Description:		Clear [Verification_Case] for scheme [SSSCMC]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	17 Aug 2020
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
--					1. Random select 10% of SP for Verify Case
--					2. classific the original and new selected cases
--					3. The vaule of Verification_Case should be null in HCVSCHN or HCVSDHC
--                  4. remove 'regenerateList'
--					5.1. SQL Tuning
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson LAW
-- Modified date:	07 March 2018
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
--					1. Add column [SP_ID],[Practice_Display_Seq],[Service_type] to [ReimbursementAuthTran] 
--					2. Add column [Verification_Case_Available] for table [ReimbursementAuthorisation] (AS 'Y') 
--					3. Random select 10% of SP for Verify Case (Each Reimbursement and each Scheme)(Step 3)
--					4. Add insert to [ReimbursementServiceProvider] (Step 4)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	26 February 2018
-- CR No.:			I-CRE17-007
-- Description:		Performance Tuning
--					1. Store Transaction ID in temp table first rather than direct insert to [ReimbursementAuthTran] (Reduce [VoucherTransaction] lock time])
--					2. Then insert Transaction ID from temp table to [ReimbursementAuthTran]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 June 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 June 2015
-- CR No.:			INT15-0005
-- Description:		Avoid concurrent update in First Authorization. To prevent the following 2 situations:
--					(1) The current Cutoff Date has been reset
--					(2) The current Reimbursement has already been completed
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		13 Aug 2009
-- Description:		Reimbursement Hold action
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 September 2009
-- Description:		Add criterion while inserting into [ReimbursementAuthTran]: [VoucherTransaction].[Transaction_ID] does not exist in [ReimbursementAuthTran].[Transaction_ID]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 September 2009
-- Description:		Remove column [VoucherTransaction].[Authorised_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	07 Oct 2009
-- Description:		Refine the sql
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementHold_upd] @cutoff_dtm   DATETIME, 
                                                    @current_user VARCHAR(20), 
                                                    @scheme_code  CHAR(10), 
                                                    @reimburse_id CHAR(15)
WITH RECOMPILE
AS
    BEGIN

        -- =============================================
        -- Declaration
        -- =============================================
        CREATE TABLE #Transaction
        (Transaction_ID       CHAR(20), 
         SP_ID                CHAR(8), 
         Practice_Display_Seq SMALLINT, 
         Service_Type         CHAR(5)
        );

        DECLARE @tempReimbursementSP TABLE
        (Reimburse_ID      VARCHAR(10), 
         Seq_No            SMALLINT, 
         SP_ID             CHAR(8), 
         Scheme_Code       CHAR(10), 
         Verification_Case CHAR(1), 
         Create_Dtm        DATETIME, 
         Create_By         VARCHAR(20)
        );

        DECLARE @VerifiyCaseAvailable CHAR(1);
        DECLARE @TotalSP SMALLINT;
        DECLARE @RandomPercent FLOAT;
        DECLARE @NumOfVerifyCase SMALLINT;
        DECLARE @MinNumOfVerifyCase SMALLINT;
        DECLARE @ExistNumOfVerifyCase SMALLINT;
        DECLARE @SchemeHCVSDHC CHAR(10);
        DECLARE @SchemeHCVSCHN CHAR(10);
		DECLARE @SchemeSSSCMC CHAR(10);
        DECLARE @ClearVerificationCase CHAR(1);
		
        DECLARE @strStatus CHAR(1);
        DECLARE @strStatusOriginal CHAR(1);
        DECLARE @strStatusNewSelected CHAR(1);

        DECLARE @In_cutoff_dtm DATETIME= @cutoff_dtm;
        DECLARE @In_current_user VARCHAR(20)= @current_user;
        DECLARE @In_scheme_code CHAR(10)= @scheme_code;
        DECLARE @In_reimburse_id CHAR(15)= @reimburse_id;

        -- =============================================
        -- Validation 
        -- =============================================
        --	(1) The current Cutoff Date has been reset

        IF
        (
            SELECT COUNT(1)
            FROM ReimbursementAuthorisation
            WHERE Reimburse_ID = @In_reimburse_id
                  AND Authorised_Status = 'S'
                  AND Record_Status = 'A'
        ) = 0
            BEGIN
                RAISERROR('00011', 16, 1);
                RETURN @@ERROR;
            END;

        --	(2) The current Reimbursement has already been completed

        IF
        (
            SELECT COUNT(1)
            FROM ReimbursementAuthorisation
            WHERE Reimburse_ID = @In_reimburse_id
                  AND Authorised_Status = 'R'
                  AND Record_Status = 'A'
                  AND Scheme_Code IN
            (
                SELECT Scheme_Code
                FROM SchemeClaim
                WHERE Record_Status = 'A'
                      AND Reimbursement_Mode =
                (
                    SELECT Reimbursement_Mode
                    FROM SchemeClaim
                    WHERE Scheme_Code = @In_scheme_code
                )
            )
        ) <> 0
            BEGIN
                RAISERROR('00011', 16, 1);
                RETURN @@ERROR;
            END;
        --	(3) GET Verification Case Available or not
        SELECT @VerifiyCaseAvailable = Verification_Case_Available
        FROM ReimbursementAuthorisation
        WHERE Reimburse_ID = @In_reimburse_id
              AND Authorised_Status = 'S'
              AND Record_Status = 'A';

        -- =============================================
        -- Initialization
        -- =============================================

        SET @SchemeHCVSDHC = 'HCVSDHC';
        SET @SchemeHCVSCHN = 'HCVSCHN';
        SET @SchemeSSSCMC = 'SSSCMC';

        SET @MinNumOfVerifyCase = 10;
        SET @RandomPercent = 0.1;
        SET @strStatusOriginal = 'O';
        SET @strStatusNewSelected = 'N';

		-- The value of 'Verification_Case' is always null in HCVSCHN or HCVSDHC or SSSCMC
		IF @In_scheme_code IN (@SchemeHCVSDHC, @SchemeHCVSCHN, @SchemeSSSCMC)
			SET @ClearVerificationCase = 'Y';
		ELSE
			SET @ClearVerificationCase = 'N';

        -- =============================================
        -- Return results
        -- =============================================
        -- Step 1: Insert transaction id into temp table
        INSERT INTO #Transaction
               (Transaction_ID, 
                SP_ID, 
                Practice_Display_Seq, 
                Service_Type
               )
        SELECT VT.Transaction_ID AS [Transaction_ID], 
               VT.SP_ID AS [SP_ID], 
               VT.Practice_Display_Seq AS [Practice_Display_Seq], 
               VT.Service_Type AS [Service_Type]
        FROM VoucherTransaction AS VT
             LEFT OUTER JOIN ReimbursementAuthTran AS RA
             ON VT.Transaction_ID = RA.Transaction_ID
        WHERE VT.Record_Status = 'A'
              AND VT.Confirmed_Dtm <= @In_cutoff_dtm
              AND VT.Scheme_Code = @In_scheme_code
              AND RA.Transaction_ID IS NULL;

        -- Step 2: Insert into [ReimbursementAuthTran]
        INSERT INTO [ReimbursementAuthTran]
               (Transaction_ID, 
                Scheme_Code, 
                Reimburse_ID, 
                Authorised_Status, 
                Authorised_Cutoff_Dtm, 
                Authorised_Cutoff_By, 
                SP_ID, 
                Practice_Display_Seq, 
                Service_type
               )
        SELECT VT.Transaction_ID, 
               @In_scheme_code, 
               @In_reimburse_id, 
               'P', 
               @In_cutoff_dtm, 
               @In_current_user, 
               VT.SP_ID, 
               VT.Practice_Display_Seq, 
               VT.Service_Type
        FROM #Transaction AS VT;

        -- Step 3: Insert into [ReimbursementAuthorisation]
        INSERT INTO [ReimbursementAuthorisation]
               ([Authorised_Dtm], 
                [Authorised_Status], 
                [Scheme_Code], 
                [Authorised_By], 
                [Record_Status], 
                [Reimburse_ID], 
                [Cutoff_Date], 
                [Create_By], 
                [Create_Dtm], 
                [Update_By], 
                [Update_Dtm], 
                [Verification_Case_Available]
               )
        VALUES
              (GETDATE(), 
               'P', 
               @In_scheme_code, 
               @In_current_user, 
               'A', 
               @In_reimburse_id, 
               @In_cutoff_dtm, 
               @In_current_user, 
               GETDATE(), 
               @In_current_user, 
               GETDATE(), 
               @VerifiyCaseAvailable
              );

        -- Step 4: Calculate the number of cases for Verification
        SELECT @TotalSP = COUNT(DISTINCT SP_ID)
        FROM ReimbursementAuthTran
        WHERE Reimburse_ID = @In_reimburse_id
              AND Scheme_Code = @In_scheme_code;

        SELECT @NumOfVerifyCase = CEILING(@TotalSP * @RandomPercent);
        IF @NumOfVerifyCase < @MinNumOfVerifyCase
            BEGIN
                SET @NumOfVerifyCase = @MinNumOfVerifyCase;
            END;

        -- Step 5: Insert into [ReimbursementServiceProvider]        
        -- Insert a new list of selected ECHP to [ReimbursementServiceProvider]
        -- Compare the existing list of selected ECHP and Distinct SP in reimbursement, and update [ReimbursementServiceProvider]
        --classific the original and new selected cases
        --If users search and hold the record for the first time, the status is 'original' otherwise is 'new selected'.
        IF EXISTS
        (
            SELECT 1
            FROM ReimbursementServiceProvider
            WHERE Reimburse_ID = @In_reimburse_id
                  AND Scheme_Code = @In_scheme_code
        )
            BEGIN
                SET @strStatus = @strStatusNewSelected;
            END;
            ELSE
            BEGIN
                SET @strStatus = @strStatusOriginal;
            END;

        INSERT INTO @tempReimbursementSP
        SELECT Reimburse_ID, 
               Seq_No, 
               SP_ID, 
               Scheme_Code,
               CASE
                   WHEN Verification_Case IS NOT NULL
                   THEN @strStatusOriginal
                   ELSE NULL
               END, 
               GETDATE() AS Create_Dtm, 
               @In_current_user AS Create_By
        FROM
        (
            SELECT T1.Reimburse_ID AS Reimburse_ID, 
                   ROW_NUMBER() OVER(PARTITION BY T1.Reimburse_ID, 
                                                  T1.Scheme_Code
                   ORDER BY T1.SP_ID) AS Seq_No, 
                   T1.SP_ID AS SP_ID, 
                   T1.Scheme_Code AS Scheme_Code, 
                   RSP.Verification_Case
            FROM
            (
                SELECT DISTINCT 
                       SP_ID, 
                       Reimburse_ID, 
                       Scheme_Code
                FROM ReimbursementAuthTran
                WHERE Reimburse_ID = @In_reimburse_id
                      AND Scheme_Code = @In_scheme_code
            ) AS T1
            LEFT JOIN ReimbursementServiceProvider AS RSP
            ON RSP.Reimburse_ID = T1.Reimburse_ID
               AND RSP.SP_ID = T1.SP_ID
               AND RSP.Scheme_Code = T1.Scheme_Code
        ) AS T2;

        SELECT @ExistNumOfVerifyCase = COUNT(1)
        FROM @tempReimbursementSP
        WHERE Verification_Case = @strStatusOriginal;

        IF(@NumOfVerifyCase > @ExistNumOfVerifyCase)
            BEGIN
                SET @NumOfVerifyCase = @NumOfVerifyCase - @ExistNumOfVerifyCase;

                UPDATE RSP
                  SET Verification_Case = @strStatus
                FROM
                (
                    SELECT TOP (@NumOfVerifyCase) *
                    FROM @tempReimbursementSP
                    WHERE Reimburse_ID = @In_reimburse_id
                          AND Scheme_Code = @In_scheme_code
                          AND Verification_Case IS NULL
                    ORDER BY NEWID()
                ) AS RSP;
            END;

        DELETE FROM ReimbursementServiceProvider
        WHERE Reimburse_ID = @In_reimburse_id
              AND Scheme_Code = @In_scheme_code;

        INSERT INTO ReimbursementServiceProvider
               (Reimburse_ID, 
                Seq_No, 
                SP_ID, 
                Scheme_Code, 
                Verification_Case, 
                Create_dtm, 
                Create_By
               )
        SELECT Reimburse_ID, 
               Seq_No, 
               SP_ID, 
               Scheme_Code, 
               Verification_Case, 
               Create_dtm, 
               Create_By
        FROM @tempReimbursementSP AS TRSP
        WHERE TRSP.Reimburse_ID = @In_reimburse_id
              AND TRSP.Scheme_Code = @In_scheme_code;

        -- Set the value of 'Verification_Case' to null
        IF @ClearVerificationCase = 'Y'
            BEGIN
                UPDATE ReimbursementServiceProvider
                  SET Verification_Case = NULL
                WHERE Reimburse_ID = @In_reimburse_id
                      AND Scheme_Code = @In_scheme_code;
            END;

        -- =============================================
        -- House Keeping
        -- =============================================
        DROP TABLE #Transaction;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementHold_upd] TO HCVU;
GO