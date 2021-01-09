
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_DPAReport_EHCP_get]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_DPAReport_EHCP_get];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	03 Nov 2020
-- CR No.:			CRE20-015
-- Description:		Fixed bug @ReportDate
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	06 Oct 2020
-- CR No.:			INT20-033
-- Description:		Apply ordering on result
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	17 Aug 2020
-- CR No.:			CRE17-004
-- Description:		New DPAR Report (EHCP Basis)
-- =============================================
-- =============================================
-- Author:			Dickson Law
-- Create date:		21 Feb 2018
-- Description:		New DPAR Report (EHCP Basis)
-- =============================================

CREATE PROCEDURE [dbo].[proc_DPAReport_EHCP_get] @reimburse_id    CHAR(15), 
                                                 @cutoff_Date_str CHAR(11), 
                                                 @scheme_code     CHAR(10)
WITH RECOMPILE
AS
    BEGIN

        -- =============================================
        -- Declaration
        -- =============================================

        DECLARE @Verification_Case_Available CHAR(1);

        CREATE TABLE #Result
        (Seq_No            SMALLINT, 
         SP_ID             CHAR(8), 
         SP_Name           VARCHAR(40), 
         SP_Name_Chi       NVARCHAR(40), 
         Verification_Case CHAR(1), 
         Total_Transaction SMALLINT, 
         Total_Amount      MONEY, 
         Total_Amount_RMB  MONEY,
        );

        DECLARE @EffectiveScheme TABLE
        (Scheme_Code CHAR(10), 
         Scheme_Seq  SMALLINT
        );

        DECLARE @ReimbursementCurrency VARCHAR(10);
        DECLARE @ReportDate DATETIME;
        DECLARE @TotalSP SMALLINT;
        DECLARE @TotalVerCases SMALLINT;
        DECLARE @TotalSPPractice SMALLINT;
        -- =============================================
        -- Validation 
        -- =============================================

        SELECT @ReimbursementCurrency = Reimbursement_Currency
        FROM SchemeClaim
        WHERE Scheme_Code = @scheme_code;

        SELECT @Verification_Case_Available = Verification_Case_Available
        FROM ReimbursementAuthorisation
        WHERE Reimburse_ID = @reimburse_id
              AND Scheme_Code = @scheme_code
              AND Authorised_Status = 'P'
              AND Record_Status = 'A';

        SELECT @ReportDate = ra.Authorised_Dtm
        FROM ReimbursementAuthTran AS rt
             INNER JOIN ReimbursementAuthorisation AS ra
             ON RT.Reimburse_ID = @reimburse_id
                AND RT.Scheme_Code = @scheme_code
                AND RT.Authorised_Status = RA.Authorised_Status
                AND ra.Record_Status = 'A'
        GROUP BY ra.Authorised_Dtm;

        SELECT @TotalSPPractice = SUM(PracticeCount)
        FROM
            (
                SELECT RAT.SP_ID, 
                       COUNT(DISTINCT RAT.Practice_Display_Seq) AS PracticeCount
                FROM ReimbursementAuthTran AS RAT
                WHERE RAT.Reimburse_ID = @reimburse_id
                      AND RAT.Scheme_Code = @scheme_code
                GROUP BY SP_ID
             ) AS T1;
        -- =============================================
        -- Initialization
        -- =============================================
        INSERT INTO @EffectiveScheme
               (Scheme_Code, 
                Scheme_Seq
               )
        SELECT Scheme_Code, 
               MAX(Scheme_Seq)
        FROM SchemeClaim
        WHERE GETDATE() >= Effective_Dtm
        GROUP BY Scheme_Code;

        IF @Verification_Case_Available = 'Y'
            BEGIN
                EXEC [proc_SymmetricKey_open]

                INSERT INTO #Result
                       (Seq_No, 
                        SP_ID, 
                        SP_Name, 
                        SP_Name_Chi, 
                        Verification_Case
                       )
                SELECT RSP.Seq_No, 
                       RSP.SP_ID,
                       CASE @ReimbursementCurrency
                           WHEN 'HKD'
                           THEN CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
                           ELSE NULL
                       END,
                       CASE @ReimbursementCurrency
                           WHEN 'HKDRMB'
                           THEN CASE ISNULL(CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3)), '')
                                    WHEN ''
                                    THEN CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
                                    ELSE CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3))
                                END
                           ELSE NULL
                       END,
                       CASE
                           WHEN Verification_Case IS NULL
                           THEN ''
                           ELSE 'Y'
                       END
                FROM ReimbursementServiceProvider AS RSP
                     INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
                     ON RSP.SP_ID = SP.SP_ID
                WHERE Reimburse_ID = @reimburse_id
                      AND Scheme_Code = @scheme_code
                      AND RSP.Seq_No > 0;

                EXEC [proc_SymmetricKey_close]

                UPDATE #Result
                  SET Total_Transaction = totalTran, 
                      Total_Amount = totalAmount, 
                      Total_Amount_RMB = totalAmountRMB
                FROM
                    (
                        SELECT VT.SP_ID, 
                               COUNT(DISTINCT VT.Transaction_ID) AS totalTran, 
                               SUM(Total_Amount) AS totalAmount, 
                               SUM(Total_Amount_RMB) AS totalAmountRMB
                        FROM ReimbursementAuthTran AS RAT
                             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
                             ON RAT.Transaction_ID = VT.Transaction_ID
                             INNER JOIN TransactionDetail AS TD WITH(NOLOCK)
                             ON RAT.Transaction_ID = TD.Transaction_ID
                        WHERE RAT.Reimburse_ID = @reimburse_id
                              AND RAT.Scheme_Code = @scheme_code
                        GROUP BY VT.SP_ID
                     ) T2
                WHERE #Result.SP_ID = T2.SP_ID;
            END;

        -- =============================================
        -- Return results
        -- =============================================

        SELECT @TotalSP = COUNT(SP_ID)
        FROM #Result;

        SELECT @TotalVerCases = COUNT(SP_ID)
        FROM #Result
        WHERE Verification_Case = 'Y';

        SELECT *
        FROM #Result
        ORDER BY Seq_No;

        SELECT @cutoff_Date_str AS [CutoffDate], 
               @reimburse_id AS [ReimburseID], 
               GETDATE() AS [GenerateDate], 
               @ReportDate AS [ReportDate], 
               @TotalSP AS [TotalSP], 
               @TotalSPPractice AS [TotalSPPractice], 
               @TotalVerCases AS [TotalVerCases], 
               SC.Display_Code AS [SchemeCode], 
               ES.Scheme_Code
        FROM @EffectiveScheme AS ES
             INNER JOIN SchemeClaim AS SC WITH(NOLOCK)
             ON ES.Scheme_Code = SC.Scheme_Code
                AND ES.Scheme_Seq = SC.Scheme_Seq
        WHERE ES.Scheme_Code = @scheme_code;

        DROP TABLE #Result;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_DPAReport_EHCP_get] TO HCVU;
GO