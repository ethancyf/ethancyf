
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_byAny]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_byAny];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	22 Oct 2020
-- CR No.			CRE20-015 (HA Scheme)
-- Description:		Add column - Claim Amount RMB
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	10 Sep 2020
-- CR No.			CRE20-003
-- Description:		Add School Code (RCH_code->SchoolOrRCH_code)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	05 Oct 2016
-- CR No.			CRE13-012
-- Description:		RCH Code sorting - add column RCH Code
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   INT13-0009  
-- Modified by:    Koala CHENG
-- Modified date:  05 Apr 2013  
-- Description:    Remove Scheme_Seq column
-- ============================================= 
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================    
-- Modification History    
-- CR No.: CRE12-008-02    
-- Modified by:    Twinsen CHAN  
-- Modified date:  27 Nov 2012  
-- Description:    Remove Scheme_Seq from join with table "SchemeClaim"  
-- =============================================    
-- =============================================    
-- Modification History  
-- CR No.:		   CRP12-007
-- Modified by:    Koala CHENG
-- Modified date:  07 Jan 2013
-- Description:    Performance Tuning
-- ============================================= 
-- =============================================  
-- Modification History  
-- Modified by:    Lawrence TSANG
-- Modified date:  29 May 2011
-- Description:    Add parameters: @Service_Receive_Dtm_From, @Service_Receive_Dtm_To
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:    Lawrence TSANG
-- Modified date:  7 November 2010
-- Description:    Add [Means_Of_Input]
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:    Lawrence TSANG
-- Modified date:  3 November 2010
-- Description:    Handle new [VoucherTransaction].[Record_Status]:
--						B: Pending Approval
--						R: Reimbursed
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	31 August 2010
-- Description:		Only join with table "InvalidPersonalInformation"
--					using InvalidAccID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	17 August 2010
-- Description:		Reconstruct the logic on retrieving transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	27 July 2010
-- Description:		Correctly handle "Removed (Back Office)" and "Pending Approval" status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	24 July 2010
-- Description:		Join with "ManualReimburse" for validate a/c
--					to get all the outside payment claim
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	19 July 2010
-- Description:		Add eHealth Account ID in search criteria
--					If the Status is Reimbursed (R), also get Manual Reimbursed (M) transaction
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 4 May 2010  
-- Description:  Fix the where clause on Status  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 24 March 2010  
-- Description:  Add Invalidation  
-- =============================================  
-- =============================================  
-- Author:   Clark Yip  
-- Create date:  29 Apr 2008  
-- Description:  Get Claim Tran (ych480:Encryption)  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   08 Dec 2008  
-- Description:     1. Add to select the total amount. Total amount will be calculated based on the Claim_Amount field  
--     2. Change the checking on encrypted field (compare the encrypted value rather than the decrypted value)  
--     3. Add back the missing join with PersonalInformation table and remove the distinct  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark YIP  
-- Modified date:   19 Aug 2009  
-- Description:     Adopt to new reimbursement schema  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 1 September 2009  
-- Description:  Add temporary table @TempTransaction so that the logic can be handled in a centralized place  
--     (But in final result the temporary table needs to join VoucherTransaction again to get the TSMP as timestamp cannot be inserted)  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 1 September 2009  
-- Description:  Add @user_id  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 1 September 2009  
-- Description:  Select [SchemeClaim].[Display_Code] in final result  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark YIP  
-- Modified date:   07 Sep 2009  
-- Description:     Inner join the TransactionDetail to get the total unit and total amount  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 14 September 2009  
-- Description:  Retrieve information about Validated Account, Temporary Account, Special Account, and Invalid Account  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 16 September 2009  
-- Description:  Allow search by Document Code and Document No  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 17 September 2009  
-- Description:  Allow NULL on @from_date and @to_date  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 18 September 2009  
-- Description:  Retrieve [TempVoucherAccount].[TSMP] and [SpecialAccount].[TSMP]  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Clark YIP  
-- Modified date: 24 September 2009  
-- Description:  Refine the filtering criteria to handle different account combinations:  
--     1. Temp + Special + Validated  
--     2. Temp + Special + Invalid  
--     3. Temp + Validated  
--     4. Temp + Special  
--     5. Temp only  
--     6. Validated only  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Clark YIP  
-- Modified date: 25 Sept 2009  
-- Description:  Change the filter of effective schemeClaim using transaction_Dtm  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Clark YIP  
-- Modified date: 28 Sept 2009  
-- Description:  Sync the join criteria with VU CTM  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 30 September 2009  
-- Description:  Remove the column [VoucherTransaction].[Reason_For_Visit_L1] and [VoucherTransaction].[Reason_For_Visit_L2]  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 2 October 2009  
-- Description:  Fix the logic on Record_Status in validated account  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 5 October 2009  
-- Description:  Handle expired scheme  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 8 October 2009  
-- Description:  Check the no. of rows after retrieving data from each type of account (Validated / Temporary / Special / Invalid)  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Clark YIP  
-- Modified date: 13 Oct 2009  
-- Description:  Remove some unused fields  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:    
-- Modified date:   
-- Description:    
-- =============================================  
--exec  [dbo].[proc_VoucherTransaction_get_byAny] null,'R',null,null,null,null,null,null,'01 Jan 1980 00:00:00','01 Jan 2013 23:59:59',null,'HAADMIN',null,null,null,null,null,null,null,null,null, 1,1,1
--exec  [dbo].[proc_VoucherTransaction_get_byAny] null, null,null,null,null,null,null,null,'01 Jan 1980 00:00:00','01 Jan 2013 23:59:59',null,'HAADMIN',null,null,null,null,null,null,null,null,null, 1,1,1

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_get_byAny] 
	@transaction_id               CHAR(20), 
    @status                       CHAR(1), 
    @authorised_status            CHAR(1), 
    @sp_id                        CHAR(8), 
    @sp_name                      VARCHAR(100), 
    @sp_hkid                      CHAR(9), 
    @bank_acc                     VARCHAR(30), 
    @service_type                 CHAR(5), 
    @from_date                    DATETIME, 
    @to_date                      DATETIME, 
    @scheme_code                  CHAR(10), 
    @user_id                      VARCHAR(20), 
    @doc_code                     CHAR(20), 
    @identity_no1                 VARCHAR(20), 
    @Adoption_Prefix_Num          CHAR(7), 
    @Invalidation                 CHAR(1), 
    @voucher_acc_id               VARCHAR(15), 
    @reimbursement_method         CHAR(1), 
    @Means_Of_Input               CHAR(1), 
    @Service_Receive_Dtm_From     DATETIME, 
    @Service_Receive_Dtm_To       DATETIME, 
    @SchoolOrRCH_code             CHAR(50), 
    @result_limit_1st_enable      BIT, 
    @result_limit_override_enable BIT, 
    @override_result_limit        BIT
AS
    BEGIN  
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @TempTransaction TABLE
        (Transaction_ID       CHAR(20), 
         Transaction_Dtm      DATETIME, 
         Encrypt_Field2       VARBINARY(100), 
         Encrypt_Field3       VARBINARY(100), 
         SP_ID                CHAR(8), 
         Bank_Account_No      VARCHAR(30), 
         Bank_Acc_Display_Seq SMALLINT, 
         Practice_Display_Seq SMALLINT, 
         Practice_Name        NVARCHAR(200), 
         Voucher_Claim        SMALLINT, 
         STATUS               CHAR(1), 
         Authorised_Status    CHAR(1), 
         Voucher_Acc_ID       CHAR(15), 
         Temp_Voucher_Acc_ID  CHAR(15), 
         Special_Acc_ID       CHAR(15), 
         Invalid_Acc_ID       CHAR(15), 
         Scheme_Code          CHAR(10),
         --  Scheme_Seq	int,
         Claim_Amount         MONEY, 
         Claim_Amount_RMB     MONEY, 
         Invalidation         CHAR(1), 
         Invalidation_TSMP    BINARY(8), 
         Manual_Reimburse     CHAR(1), 
         SchoolOrRCH_code     CHAR(50)
        );

        DECLARE @identity_no2 VARCHAR(20);

        -- =============================================  
        -- Validation   
        -- =============================================  
        -- =============================================  
        -- Initialization  
        -- =============================================  
        DECLARE @rowcount INT;
        DECLARE @row_cnt_error VARCHAR(MAX);
        DECLARE @errCode_lower CHAR(5);
        DECLARE @errCode_upper CHAR(5);
        SET @errCode_lower = '00009';
        SET @errCode_upper = '00017';

        IF @identity_no1 IS NULL
            BEGIN
                SET @identity_no2 = NULL;
            END;
            ELSE
            BEGIN
                SET @identity_no2 = ' ' + @identity_no1;
            END;

        -- =============================================  
        -- Retrieve data  
        -- =============================================   

        EXEC [proc_SymmetricKey_open]

        -- ---------------------------------------------  
        -- Validated Account  
        -- ---------------------------------------------  

        INSERT INTO @TempTransaction
               (Transaction_ID, 
                Transaction_Dtm, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                SP_ID, 
                Bank_Account_No, 
                Bank_Acc_Display_Seq, 
                Practice_Display_Seq, 
                Practice_Name, 
                Voucher_Claim, 
                STATUS, 
                Authorised_Status, 
                Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Special_Acc_ID, 
                Invalid_Acc_ID, 
                Scheme_Code,
                --  Scheme_Seq,
                Claim_Amount, 
                Claim_Amount_RMB, 
                Invalidation, 
                Invalidation_TSMP, 
                Manual_Reimburse, 
                SchoolOrRCH_code
               )
        SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable, @result_limit_override_enable)) VT.Transaction_ID, 
                                                                                                       VT.Transaction_Dtm, 
                                                                                                       SP.Encrypt_Field2, 
                                                                                                       SP.Encrypt_Field3, 
                                                                                                       VT.SP_ID, 
                                                                                                       VT.Bank_Account_No, 
                                                                                                       VT.Bank_Acc_Display_Seq, 
                                                                                                       VT.Practice_Display_Seq, 
                                                                                                       P.Practice_Name, 
                                                                                                       SUM(TD.Unit), 
                                                                                                       VT.Record_Status,
                                                                                                       CASE
                                                                                                           WHEN ISNULL(RAT.Authorised_Status, '') =
                                                                                                           'R'
                                                                                                                OR VT.Record_Status = 'R'
                                                                                                           THEN 'G'
                                                                                                           ELSE ISNULL(RAT.Authorised_Status, '')
                                                                                                       END AS [Authorised_Status], 
                                                                                                       VT.Voucher_Acc_ID, 
                                                                                                       VT.Temp_Voucher_Acc_ID, 
                                                                                                       VT.Special_Acc_ID, 
                                                                                                       VT.Invalid_Acc_ID, 
                                                                                                       VT.Scheme_Code,
                                                                                                       --  TD.Scheme_Seq,
                                                                                                       SUM(TD.Total_Amount), 
                                                                                                       SUM(TD.Total_Amount_RMB), 
                                                                                                       VT.Invalidation, 
                                                                                                       TI.TSMP AS [Invalidation_TSMP], 
                                                                                                       VT.Manual_Reimburse, 
                                                                                                       COALESCE(TAF.AdditionalFieldValueCode, TAFSC.
                                                                                                       AdditionalFieldValueCode) AS
                                                                                                       [SchoolOrRCH_Code]
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON VT.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P WITH(NOLOCK)
             ON VT.SP_ID = P.SP_ID
                AND VT.Practice_display_seq = P.Display_seq
             INNER JOIN BankAccount AS B WITH(NOLOCK)
             ON P.SP_ID = B.SP_ID
                AND P.Display_seq = B.SP_Practice_Display_Seq
                AND VT.Bank_Acc_Display_Seq = B.Display_Seq
             INNER JOIN Professional AS PF WITH(NOLOCK)
             ON P.SP_ID = PF.SP_ID
                AND P.Professional_Seq = PF.Professional_Seq
             INNER JOIN PersonalInformation AS PINFO WITH(NOLOCK)
             ON VT.Voucher_Acc_ID = PINFO.Voucher_Acc_ID
                AND VT.Doc_Code = PINFO.Doc_Code
             LEFT JOIN ReimbursementAuthTran AS RAT WITH(NOLOCK)
             ON VT.Transaction_ID = RAT.Transaction_ID
                AND ISNULL(VT.Manual_Reimburse, 'N') = 'N'
             LEFT JOIN ManualReimbursement AS MR WITH(NOLOCK)
             ON VT.Transaction_ID = MR.Transaction_ID
                AND ISNULL(VT.Manual_Reimburse, 'N') = 'Y'
             LEFT JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON VT.Transaction_ID = TD.Transaction_ID
                AND VT.Scheme_Code = TD.Scheme_Code
             LEFT JOIN TransactionInvalidation AS TI WITH(NOLOCK)
             ON VT.Transaction_ID = TI.Transaction_ID
             LEFT JOIN TransactionAdditionalField AS TAF WITH(NOLOCK)
             ON VT.Transaction_ID = TAF.Transaction_ID
                AND TAF.AdditionalFieldID = 'RHCCode'
             LEFT JOIN TransactionAdditionalField AS TAFSC WITH(NOLOCK)
             ON VT.Transaction_ID = TAFSC.Transaction_ID
                AND TAFSC.AdditionalFieldID = 'SchoolCode'
        WHERE VT.Voucher_Acc_ID <> ''
              AND VT.Invalid_Acc_ID IS NULL
              AND (@transaction_id IS NULL
                   OR @transaction_id = VT.Transaction_ID)
              AND ((@from_date IS NULL
                    AND @to_date IS NULL)
                   OR (VT.Transaction_Dtm BETWEEN @from_date AND @to_date))
              AND ((@Service_Receive_Dtm_From IS NULL
                    AND @Service_Receive_Dtm_To IS NULL)
                   OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
              AND (@service_type IS NULL
                   OR @service_type = VT.Service_Type)
              AND (@scheme_code IS NULL
                   OR @scheme_code = VT.Scheme_Code)
              AND VT.Scheme_Code IN
        (
            SELECT DISTINCT 
                   Scheme_Code
            FROM UserRole
            WHERE User_ID = @User_ID
        )
              AND (@doc_code IS NULL
                   OR @doc_code = VT.Doc_Code)
              AND (@voucher_acc_id IS NULL
                   OR @voucher_acc_id = ISNULL(VT.Voucher_Acc_ID, ''))
              AND (@Invalidation IS NULL
                   OR @Invalidation = ISNULL(VT.Invalidation, ''))
              AND (@sp_id IS NULL
                   OR @sp_id = VT.SP_ID)
              AND (@sp_hkid IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
              AND (@sp_name IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
              AND (@bank_acc IS NULL
                   OR @bank_acc = B.Bank_Account_No)
              AND (@identity_no1 IS NULL
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no1)
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no2))
              AND (@Adoption_Prefix_Num IS NULL
                   OR PINFO.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
              AND (@status IS NULL
                   OR @status = VT.Record_Status)
              AND (@authorised_status IS NULL
                   OR (@authorised_status <> 'N'
                       AND @authorised_status <> 'G'
                       AND @authorised_status = RAT.Authorised_Status)
                   OR (@authorised_status = 'N'
                       AND RAT.Authorised_Status IS NULL)
                   OR (@authorised_status = 'G'
                       AND (VT.Record_Status = 'R'
                            OR RAT.Authorised_Status = 'R')))
              AND (@reimbursement_method IS NULL
                   OR (@reimbursement_method = 'Y'
                       AND ISNULL(VT.Manual_Reimburse, 'N') = 'Y')
                   OR (@reimbursement_method = 'N'
                       AND ISNULL(VT.Manual_Reimburse, 'N') = 'N'))
              AND (@Means_Of_Input IS NULL
                   OR (@Means_Of_Input = 'M'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
                   OR (@Means_Of_Input = 'C'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y'))
              AND (@SchoolOrRCH_code IS NULL
                   OR @SchoolOrRCH_code = TAF.AdditionalFieldValueCode
                   OR @SchoolOrRCH_code = TAFSC.AdditionalFieldValueCode)
        GROUP BY VT.Transaction_ID, 
                 VT.Transaction_Dtm, 
                 SP.Encrypt_Field2, 
                 SP.Encrypt_Field3, 
                 VT.SP_ID, 
                 VT.Bank_Account_No, 
                 VT.Bank_Acc_Display_Seq, 
                 VT.Practice_Display_Seq, 
                 P.Practice_Name, 
                 VT.Record_Status, 
                 RAT.Authorised_Status, 
                 VT.Voucher_Acc_ID, 
                 VT.Temp_Voucher_Acc_ID, 
                 VT.Special_Acc_ID, 
                 VT.Invalid_Acc_ID, 
                 VT.Scheme_Code,  
                 --  TD.Scheme_Seq,
                 VT.Invalidation, 
                 TI.TSMP, 
                 MR.Record_Status, 
                 VT.Manual_Reimburse, 
                 TAF.AdditionalFieldValueCode, 
                 TAFSC.AdditionalFieldValueCode;

        -- =============================================    
        -- Max Row Checking  
        -- =============================================  
        BEGIN TRY
            SELECT @rowcount = COUNT(1)
            FROM @TempTransaction;
            EXEC proc_CheckFeatureResultRowLimit @row_count = @rowcount, 
                                                 @result_limit_1st_enable = @result_limit_1st_enable, 
                                                 @result_limit_override_enable = @result_limit_override_enable, 
                                                 @override_result_limit = @override_result_limit;
        END TRY
        BEGIN CATCH
            SET @row_cnt_error = ERROR_MESSAGE();

            IF(ISNULL(@row_cnt_error, '') <> ''
               AND @row_cnt_error <> @errCode_lower)
              OR (@result_limit_override_enable = 0
                  AND @row_cnt_error = @errCode_lower)
                BEGIN
                    --throw error if upper limit reached (error = @errCode_upper)
                    --if upper limit is not enabled, throw error if lower limit is reached
                    --if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
                    RAISERROR(@row_cnt_error, 16, 1);
                    EXEC [proc_SymmetricKey_close]
                    RETURN;
                END;
        END CATCH;

        -- ---------------------------------------------  
        -- Temporary Account  
        -- ---------------------------------------------  

        INSERT INTO @TempTransaction
               (Transaction_ID, 
                Transaction_Dtm, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                SP_ID, 
                Bank_Account_No, 
                Bank_Acc_Display_Seq, 
                Practice_Display_Seq, 
                Practice_Name, 
                Voucher_Claim, 
                STATUS, 
                Authorised_Status, 
                Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Special_Acc_ID, 
                Invalid_Acc_ID, 
                Scheme_Code,
                --  Scheme_Seq,
                Claim_Amount, 
                Claim_Amount_RMB, 
                Invalidation, 
                Invalidation_TSMP, 
                Manual_Reimburse, 
                SchoolOrRCH_code
               )
        SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable, @result_limit_override_enable)) VT.Transaction_ID, 
                                                                                                       VT.Transaction_Dtm, 
                                                                                                       SP.Encrypt_Field2, 
                                                                                                       SP.Encrypt_Field3, 
                                                                                                       VT.SP_ID, 
                                                                                                       VT.Bank_Account_No, 
                                                                                                       VT.Bank_Acc_Display_Seq, 
                                                                                                       VT.Practice_Display_Seq, 
                                                                                                       P.Practice_Name, 
                                                                                                       SUM(TD.Unit), 
                                                                                                       VT.Record_Status, 
                                                                                                       NULL AS [Authorised_Status], 
                                                                                                       VT.Voucher_Acc_ID, 
                                                                                                       VT.Temp_Voucher_Acc_ID, 
                                                                                                       VT.Special_Acc_ID, 
                                                                                                       VT.Invalid_Acc_ID, 
                                                                                                       VT.Scheme_Code,  
                                                                                                       --  TD.Scheme_Seq,
                                                                                                       SUM(TD.Total_Amount), 
                                                                                                       SUM(TD.Total_Amount_RMB), 
                                                                                                       VT.Invalidation, 
                                                                                                       TI.TSMP AS [Invalidation_TSMP], 
                                                                                                       VT.Manual_Reimburse, 
                                                                                                       COALESCE(TAF.AdditionalFieldValueCode, TAFSC.
                                                                                                       AdditionalFieldValueCode) AS
                                                                                                       [SchoolOrRCH_Code]
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON VT.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P WITH(NOLOCK)
             ON VT.SP_ID = P.SP_ID
                AND VT.Practice_display_seq = P.Display_seq
             INNER JOIN BankAccount AS B WITH(NOLOCK)
             ON P.SP_ID = B.SP_ID
                AND P.Display_seq = B.SP_Practice_Display_Seq
                AND VT.Bank_Acc_Display_Seq = B.Display_Seq
             INNER JOIN Professional AS PF WITH(NOLOCK)
             ON P.SP_ID = PF.SP_ID
                AND P.Professional_Seq = PF.Professional_Seq
             INNER JOIN TempPersonalInformation AS PINFO WITH(NOLOCK)
             ON VT.Temp_Voucher_Acc_ID = PINFO.Voucher_Acc_ID
                AND VT.Doc_Code = PINFO.Doc_Code
             LEFT JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON VT.Transaction_ID = TD.Transaction_ID
                AND VT.Scheme_Code = TD.Scheme_Code
             LEFT JOIN TransactionInvalidation AS TI WITH(NOLOCK)
             ON VT.Transaction_ID = TI.Transaction_ID
             LEFT JOIN TransactionAdditionalField AS TAF WITH(NOLOCK)
             ON VT.Transaction_ID = TAF.Transaction_ID
                AND TAF.AdditionalFieldID = 'RHCCode'
             LEFT JOIN TransactionAdditionalField AS TAFSC WITH(NOLOCK)
             ON VT.Transaction_ID = TAFSC.Transaction_ID
                AND TAFSC.AdditionalFieldID = 'SchoolCode'
        WHERE ISNULL(VT.invalid_acc_id, '') = ''
              AND ISNULL(VT.special_acc_id, '') = ''
              AND ISNULL(VT.voucher_acc_id, '') = ''
              AND ISNULL(VT.temp_voucher_acc_id, '') <> ''
              AND (@transaction_id IS NULL
                   OR @transaction_id = VT.Transaction_ID)
              AND ((@from_date IS NULL
                    AND @to_date IS NULL)
                   OR (VT.Transaction_Dtm BETWEEN @from_date AND @to_date))
              AND ((@Service_Receive_Dtm_From IS NULL
                    AND @Service_Receive_Dtm_To IS NULL)
                   OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
              AND (@service_type IS NULL
                   OR @service_type = VT.Service_Type)
              AND (@scheme_code IS NULL
                   OR @scheme_code = VT.Scheme_Code)
              AND VT.Scheme_Code IN
        (
            SELECT DISTINCT 
                   Scheme_Code
            FROM UserRole
            WHERE User_ID = @User_ID
        )
              AND (@doc_code IS NULL
                   OR @doc_code = VT.Doc_Code)
              AND @voucher_acc_id IS NULL
              AND (@Invalidation IS NULL
                   OR @Invalidation = ISNULL(VT.Invalidation, ''))
              AND (@sp_id IS NULL
                   OR @sp_id = VT.SP_ID)
              AND (@sp_hkid IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
              AND (@sp_name IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
              AND (@bank_acc IS NULL
                   OR @bank_acc = B.Bank_Account_No)
              AND (@identity_no1 IS NULL
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no1)
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no2))
              AND (@Adoption_Prefix_Num IS NULL
                   OR PINFO.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
              AND (@status IS NULL
                   OR @status = VT.Record_Status)
              AND (@authorised_status IS NULL
                   OR @authorised_status = 'N')
              AND (@reimbursement_method IS NULL)
              AND (@Means_Of_Input IS NULL
                   OR (@Means_Of_Input = 'M'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
                   OR (@Means_Of_Input = 'C'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y'))
              AND (@SchoolOrRCH_code IS NULL
                   OR @SchoolOrRCH_code = TAF.AdditionalFieldValueCode
                   OR @SchoolOrRCH_code = TAFSC.AdditionalFieldValueCode)
        GROUP BY VT.Transaction_ID, 
                 VT.Transaction_Dtm, 
                 SP.Encrypt_Field2, 
                 SP.Encrypt_Field3, 
                 VT.SP_ID, 
                 VT.Bank_Account_No, 
                 VT.Bank_Acc_Display_Seq, 
                 VT.Practice_Display_Seq, 
                 P.Practice_Name, 
                 VT.Record_Status, 
                 VT.Voucher_Acc_ID, 
                 VT.Temp_Voucher_Acc_ID, 
                 VT.Special_Acc_ID, 
                 VT.Invalid_Acc_ID, 
                 VT.Scheme_Code,
                 --  TD.Scheme_Seq,
                 VT.Invalidation, 
                 TI.TSMP, 
                 VT.Manual_Reimburse, 
                 TAF.AdditionalFieldValueCode, 
                 TAFSC.AdditionalFieldValueCode;

        -- =============================================    
        -- Max Row Checking  
        -- =============================================  
        BEGIN TRY
            SELECT @rowcount = COUNT(1)
            FROM @TempTransaction;
            -- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
            EXEC proc_CheckFeatureResultRowLimit @row_count = @rowcount, 
                                                 @result_limit_1st_enable = @result_limit_1st_enable, 
                                                 @result_limit_override_enable = @result_limit_override_enable, 
                                                 @override_result_limit = @override_result_limit;
        END TRY
        BEGIN CATCH
            SET @row_cnt_error = ERROR_MESSAGE();

            IF(ISNULL(@row_cnt_error, '') <> ''
               AND @row_cnt_error <> @errCode_lower)
              OR (@result_limit_override_enable = 0
                  AND @row_cnt_error = @errCode_lower)
                BEGIN
                    --throw error if upper limit reached (error = @errCode_upper)
                    --if upper limit is not enabled, throw error if lower limit is reached
                    --if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
                    RAISERROR(@row_cnt_error, 16, 1);
                    EXEC [proc_SymmetricKey_close]
                    RETURN;
                END;
        END CATCH;

        -- ---------------------------------------------  
        -- Special Account  
        -- ---------------------------------------------  

        INSERT INTO @TempTransaction
               (Transaction_ID, 
                Transaction_Dtm, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                SP_ID, 
                Bank_Account_No, 
                Bank_Acc_Display_Seq, 
                Practice_Display_Seq, 
                Practice_Name, 
                Voucher_Claim, 
                STATUS, 
                Authorised_Status, 
                Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Special_Acc_ID, 
                Invalid_Acc_ID, 
                Scheme_Code,
                --  Scheme_Seq,  
                Claim_Amount, 
                Claim_Amount_RMB, 
                Invalidation, 
                Invalidation_TSMP, 
                Manual_Reimburse, 
                SchoolOrRCH_code
               )
        SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable, @result_limit_override_enable)) VT.Transaction_ID, 
                                                                                                       VT.Transaction_Dtm, 
                                                                                                       SP.Encrypt_Field2, 
                                                                                                       SP.Encrypt_Field3, 
                                                                                                       VT.SP_ID, 
                                                                                                       VT.Bank_Account_No, 
                                                                                                       VT.Bank_Acc_Display_Seq, 
                                                                                                       VT.Practice_Display_Seq, 
                                                                                                       P.Practice_Name, 
                                                                                                       SUM(TD.Unit), 
                                                                                                       VT.Record_Status,
                                                                                                       CASE
                                                                                                           WHEN ISNULL(RAT.Authorised_Status, '') =
                                                                                                           'R'
                                                                                                                OR VT.Record_Status = 'R'
                                                                                                           THEN 'G'
                                                                                                           ELSE ISNULL(RAT.Authorised_Status, '')
                                                                                                       END AS [Authorised_Status], 
                                                                                                       VT.Voucher_Acc_ID, 
                                                                                                       VT.Temp_Voucher_Acc_ID, 
                                                                                                       VT.Special_Acc_ID, 
                                                                                                       VT.Invalid_Acc_ID, 
                                                                                                       VT.Scheme_Code,  
                                                                                                       --  TD.Scheme_Seq,
                                                                                                       SUM(TD.Total_Amount), 
                                                                                                       SUM(TD.Total_Amount_RMB), 
                                                                                                       VT.Invalidation, 
                                                                                                       TI.TSMP AS [Invalidation_TSMP], 
                                                                                                       VT.Manual_Reimburse, 
                                                                                                       COALESCE(TAF.AdditionalFieldValueCode, TAFSC.
                                                                                                       AdditionalFieldValueCode) AS
                                                                                                       [SchoolOrRCH_Code]
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON VT.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P WITH(NOLOCK)
             ON VT.SP_ID = P.SP_ID
                AND VT.Practice_display_seq = P.Display_seq
             INNER JOIN BankAccount AS B WITH(NOLOCK)
             ON P.SP_ID = B.SP_ID
                AND P.Display_seq = B.SP_Practice_Display_Seq
                AND VT.Bank_Acc_Display_Seq = B.Display_Seq
             INNER JOIN Professional AS PF WITH(NOLOCK)
             ON P.SP_ID = PF.SP_ID
                AND P.Professional_Seq = PF.Professional_Seq
             INNER JOIN SpecialPersonalInformation AS PINFO WITH(NOLOCK)
             ON VT.Special_acc_id = PINFO.Special_acc_id
                AND VT.Doc_Code = PINFO.Doc_Code
             LEFT JOIN ReimbursementAuthTran AS RAT WITH(NOLOCK)
             ON VT.Transaction_ID = RAT.Transaction_ID
                AND ISNULL(VT.Manual_Reimburse, 'N') = 'N'
             LEFT JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON VT.Transaction_ID = TD.Transaction_ID
                AND VT.Scheme_Code = TD.Scheme_Code
             LEFT JOIN TransactionInvalidation AS TI WITH(NOLOCK)
             ON VT.Transaction_ID = TI.Transaction_ID
             LEFT JOIN TransactionAdditionalField AS TAF WITH(NOLOCK)
             ON VT.Transaction_ID = TAF.Transaction_ID
                AND TAF.AdditionalFieldID = 'RHCCode'
             LEFT JOIN TransactionAdditionalField AS TAFSC WITH(NOLOCK)
             ON VT.Transaction_ID = TAFSC.Transaction_ID
                AND TAFSC.AdditionalFieldID = 'SchoolCode'
        WHERE ISNULL(VT.invalid_acc_id, '') = ''
              AND ISNULL(VT.special_acc_id, '') <> ''
              AND ISNULL(VT.voucher_acc_id, '') = ''
              AND (@transaction_id IS NULL
                   OR @transaction_id = VT.Transaction_ID)
              AND ((@from_date IS NULL
                    AND @to_date IS NULL)
                   OR (VT.Transaction_Dtm BETWEEN @from_date AND @to_date))
              AND ((@Service_Receive_Dtm_From IS NULL
                    AND @Service_Receive_Dtm_To IS NULL)
                   OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
              AND (@service_type IS NULL
                   OR @service_type = VT.Service_Type)
              AND (@scheme_code IS NULL
                   OR @scheme_code = VT.Scheme_Code)
              AND VT.Scheme_Code IN
        (
            SELECT DISTINCT 
                   Scheme_Code
            FROM UserRole
            WHERE User_ID = @User_ID
        )
              AND (@doc_code IS NULL
                   OR @doc_code = VT.Doc_Code)
              AND @voucher_acc_id IS NULL
              AND (@Invalidation IS NULL
                   OR @Invalidation = ISNULL(VT.Invalidation, ''))
              AND (@sp_id IS NULL
                   OR @sp_id = VT.SP_ID)
              AND (@sp_hkid IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
              AND (@sp_name IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
              AND (@bank_acc IS NULL
                   OR @bank_acc = B.Bank_Account_No)
              AND (@identity_no1 IS NULL
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no1)
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no2))
              AND (@Adoption_Prefix_Num IS NULL
                   OR PINFO.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
              AND (@status IS NULL
                   OR @status = VT.Record_Status)
              AND (@authorised_status IS NULL
                   OR (@authorised_status <> 'N'
                       AND @authorised_status <> 'G'
                       AND @authorised_status = RAT.Authorised_Status)
                   OR (@authorised_status = 'N'
                       AND RAT.Authorised_Status IS NULL)
                   OR (@authorised_status = 'G'
                       AND (VT.Record_Status = 'R'
                            OR RAT.Authorised_Status = 'R')))
              AND (@reimbursement_method IS NULL
                   OR (@reimbursement_method = 'N'
                       AND ISNULL(VT.Manual_Reimburse, 'N') = 'N'))
              AND (@Means_Of_Input IS NULL
                   OR (@Means_Of_Input = 'M'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
                   OR (@Means_Of_Input = 'C'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y'))
              AND (@SchoolOrRCH_code IS NULL
                   OR @SchoolOrRCH_code = TAF.AdditionalFieldValueCode
                   OR @SchoolOrRCH_code = TAFSC.AdditionalFieldValueCode)
        GROUP BY VT.Transaction_ID, 
                 VT.Transaction_Dtm, 
                 SP.Encrypt_Field2, 
                 SP.Encrypt_Field3, 
                 VT.SP_ID, 
                 VT.Bank_Account_No, 
                 VT.Bank_Acc_Display_Seq, 
                 VT.Practice_Display_Seq, 
                 P.Practice_Name, 
                 VT.Record_Status, 
                 RAT.Authorised_Status, 
                 VT.Voucher_Acc_ID, 
                 VT.Temp_Voucher_Acc_ID, 
                 VT.Special_Acc_ID, 
                 VT.Invalid_Acc_ID, 
                 VT.Scheme_Code,  
                 --  TD.Scheme_Seq,
                 VT.Invalidation, 
                 TI.TSMP, 
                 VT.Manual_Reimburse, 
                 TAF.AdditionalFieldValueCode, 
                 TAFSC.AdditionalFieldValueCode;

        -- =============================================    
        -- Max Row Checking  
        -- =============================================  
        BEGIN TRY
            SELECT @rowcount = COUNT(1)
            FROM @TempTransaction;
            -- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
            EXEC proc_CheckFeatureResultRowLimit @row_count = @rowcount, 
                                                 @result_limit_1st_enable = @result_limit_1st_enable, 
                                                 @result_limit_override_enable = @result_limit_override_enable, 
                                                 @override_result_limit = @override_result_limit;
        END TRY
        BEGIN CATCH
            SET @row_cnt_error = ERROR_MESSAGE();

            IF(ISNULL(@row_cnt_error, '') <> ''
               AND @row_cnt_error <> @errCode_lower)
              OR (@result_limit_override_enable = 0
                  AND @row_cnt_error = @errCode_lower)
                BEGIN
                    --throw error if upper limit reached (error = @errCode_upper)
                    --if upper limit is not enabled, throw error if lower limit is reached
                    --if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
                    RAISERROR(@row_cnt_error, 16, 1);
                    EXEC [proc_SymmetricKey_close]
                    RETURN;
                END;
        END CATCH;

        -- ---------------------------------------------  
        -- Invalid Account  
        -- ---------------------------------------------  

        INSERT INTO @TempTransaction
               (Transaction_ID, 
                Transaction_Dtm, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                SP_ID, 
                Bank_Account_No, 
                Bank_Acc_Display_Seq, 
                Practice_Display_Seq, 
                Practice_Name, 
                Voucher_Claim, 
                STATUS, 
                Authorised_Status, 
                Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Special_Acc_ID, 
                Invalid_Acc_ID, 
                Scheme_Code,  
                --  Scheme_Seq,
                Claim_Amount, 
                Claim_Amount_RMB, 
                Invalidation, 
                Invalidation_TSMP, 
                Manual_Reimburse, 
                SchoolOrRCH_code
               )
        SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable, @result_limit_override_enable)) VT.Transaction_ID, 
                                                                                                       VT.Transaction_Dtm, 
                                                                                                       SP.Encrypt_Field2, 
                                                                                                       SP.Encrypt_Field3, 
                                                                                                       VT.SP_ID, 
                                                                                                       VT.Bank_Account_No, 
                                                                                                       VT.Bank_Acc_Display_Seq, 
                                                                                                       VT.Practice_Display_Seq, 
                                                                                                       P.Practice_Name, 
                                                                                                       SUM(TD.Unit), 
                                                                                                       VT.Record_Status,
                                                                                                       CASE
                                                                                                           WHEN ISNULL(RAT.Authorised_Status, '') =
                                                                                                           'R'
                                                                                                                OR VT.Record_Status = 'R'
                                                                                                           THEN 'G'
                                                                                                           ELSE ISNULL(RAT.Authorised_Status, '')
                                                                                                       END AS [Authorised_Status], 
                                                                                                       VT.Voucher_Acc_ID, 
                                                                                                       VT.Temp_Voucher_Acc_ID, 
                                                                                                       VT.Special_Acc_ID, 
                                                                                                       VT.Invalid_Acc_ID, 
                                                                                                       VT.Scheme_Code,  
                                                                                                       --  TD.Scheme_Seq,
                                                                                                       SUM(TD.Total_Amount), 
                                                                                                       SUM(TD.Total_Amount_RMB), 
                                                                                                       VT.Invalidation, 
                                                                                                       TI.TSMP AS [Invalidation_TSMP], 
                                                                                                       VT.Manual_Reimburse, 
                                                                                                       COALESCE(TAF.AdditionalFieldValueCode, TAFSC.
                                                                                                       AdditionalFieldValueCode) AS
                                                                                                       [SchoolOrRCH_Code]
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON VT.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P WITH(NOLOCK)
             ON VT.SP_ID = P.SP_ID
                AND VT.Practice_display_seq = P.Display_seq
             INNER JOIN BankAccount AS B WITH(NOLOCK)
             ON P.SP_ID = B.SP_ID
                AND P.Display_seq = B.SP_Practice_Display_Seq
                AND VT.Bank_Acc_Display_Seq = B.Display_Seq
             INNER JOIN Professional AS PF WITH(NOLOCK)
             ON P.SP_ID = PF.SP_ID
                AND P.Professional_Seq = PF.Professional_Seq
             INNER JOIN InvalidPersonalInformation AS PINFO WITH(NOLOCK)
             ON VT.invalid_acc_id = PINFO.invalid_acc_id
             LEFT JOIN ReimbursementAuthTran AS RAT WITH(NOLOCK)
             ON VT.Transaction_ID = RAT.Transaction_ID
                AND ISNULL(VT.Manual_Reimburse, 'N') = 'N'
             LEFT JOIN ManualReimbursement AS MR WITH(NOLOCK)
             ON VT.Transaction_ID = MR.Transaction_ID
                AND ISNULL(VT.Manual_Reimburse, 'N') = 'Y'
             LEFT JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON VT.Transaction_ID = TD.Transaction_ID
                AND VT.Scheme_Code = TD.Scheme_Code
             LEFT JOIN TransactionInvalidation AS TI WITH(NOLOCK)
             ON VT.Transaction_ID = TI.Transaction_ID
             LEFT JOIN TransactionAdditionalField AS TAF WITH(NOLOCK)
             ON VT.Transaction_ID = TAF.Transaction_ID
                AND TAF.AdditionalFieldID = 'RHCCode'
             LEFT JOIN TransactionAdditionalField AS TAFSC WITH(NOLOCK)
             ON VT.Transaction_ID = TAFSC.Transaction_ID
                AND TAFSC.AdditionalFieldID = 'SchoolCode'
        WHERE ISNULL(VT.invalid_acc_id, '') <> ''
              AND (@transaction_id IS NULL
                   OR @transaction_id = VT.Transaction_ID)
              AND ((@from_date IS NULL
                    AND @to_date IS NULL)
                   OR (VT.Transaction_Dtm BETWEEN @from_date AND @to_date))
              AND ((@Service_Receive_Dtm_From IS NULL
                    AND @Service_Receive_Dtm_To IS NULL)
                   OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
              AND (@service_type IS NULL
                   OR @service_type = VT.Service_Type)
              AND (@scheme_code IS NULL
                   OR @scheme_code = VT.Scheme_Code)
              AND VT.Scheme_Code IN
        (
            SELECT DISTINCT 
                   Scheme_Code
            FROM UserRole
            WHERE User_ID = @User_ID
        )
              AND (@doc_code IS NULL
                   OR @doc_code = VT.Doc_Code)
              AND @voucher_acc_id IS NULL
              AND (@Invalidation IS NULL
                   OR @Invalidation = ISNULL(VT.Invalidation, ''))
              AND (@sp_id IS NULL
                   OR @sp_id = VT.SP_ID)
              AND (@sp_hkid IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
              AND (@sp_name IS NULL
                   OR ENCRYPTBYKEY(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
              AND (@bank_acc IS NULL
                   OR @bank_acc = B.Bank_Account_No)
              AND (@identity_no1 IS NULL
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no1)
                   OR PINFO.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @identity_no2))
              AND (@Adoption_Prefix_Num IS NULL
                   OR PINFO.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
              AND (@status IS NULL
                   OR @status = VT.Record_Status)
              AND (@authorised_status IS NULL
                   OR (@authorised_status <> 'N'
                       AND @authorised_status <> 'G'
                       AND @authorised_status = RAT.Authorised_Status)
                   OR (@authorised_status = 'N'
                       AND RAT.Authorised_Status IS NULL)
                   OR (@authorised_status = 'G'
                       AND (VT.Record_Status = 'R'
                            OR RAT.Authorised_Status = 'R')))
              AND (@reimbursement_method IS NULL
                   OR (@reimbursement_method = 'Y'
                       AND ISNULL(VT.Manual_Reimburse, 'N') = 'Y')
                   OR (@reimbursement_method = 'N'
                       AND ISNULL(VT.Manual_Reimburse, 'N') = 'N'))
              AND (@Means_Of_Input IS NULL
                   OR (@Means_Of_Input = 'M'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
                   OR (@Means_Of_Input = 'C'
                       AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y'))
              AND (@SchoolOrRCH_code IS NULL
                   OR @SchoolOrRCH_code = TAF.AdditionalFieldValueCode
                   OR @SchoolOrRCH_code = TAFSC.AdditionalFieldValueCode)
        GROUP BY VT.Transaction_ID, 
                 VT.Transaction_Dtm, 
                 SP.Encrypt_Field2, 
                 SP.Encrypt_Field3, 
                 VT.SP_ID, 
                 VT.Bank_Account_No, 
                 VT.Bank_Acc_Display_Seq, 
                 VT.Practice_Display_Seq, 
                 P.Practice_Name, 
                 VT.Record_Status, 
                 RAT.Authorised_Status, 
                 VT.Voucher_Acc_ID, 
                 VT.Temp_Voucher_Acc_ID, 
                 VT.Special_Acc_ID, 
                 VT.Invalid_Acc_ID, 
                 VT.Scheme_Code,  
                 --  TD.Scheme_Seq,
                 VT.Invalidation, 
                 TI.TSMP, 
                 VT.Manual_Reimburse, 
                 MR.Record_status, 
                 TAF.AdditionalFieldValueCode, 
                 TAFSC.AdditionalFieldValueCode;

        -- =============================================    
        -- Max Row Checking  
        -- =============================================  
        BEGIN TRY
            SELECT @rowcount = COUNT(1)
            FROM @TempTransaction;
            -- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
            EXEC proc_CheckFeatureResultRowLimit @row_count = @rowcount, 
                                                 @result_limit_1st_enable = @result_limit_1st_enable, 
                                                 @result_limit_override_enable = @result_limit_override_enable, 
                                                 @override_result_limit = @override_result_limit;
        END TRY
        BEGIN CATCH
            SET @row_cnt_error = ERROR_MESSAGE();

            IF(ISNULL(@row_cnt_error, '') <> ''
               AND @row_cnt_error <> @errCode_lower)
              OR (@result_limit_override_enable = 0
                  AND @row_cnt_error = @errCode_lower)
                BEGIN
                    --throw error if upper limit reached (error = @errCode_upper)
                    --if upper limit is not enabled, throw error if lower limit is reached
                    --if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
                    RAISERROR(@row_cnt_error, 16, 1);
                    EXEC [proc_SymmetricKey_close]
                    RETURN;
                END;
        END CATCH;

        -- =============================================    
        -- Throw out error if lower limit is reached 
        -- =============================================  
        IF ISNULL(@row_cnt_error, '') = @errCode_lower
            BEGIN
                RAISERROR(@row_cnt_error, 16, 1);
                EXEC [proc_SymmetricKey_close]
                RETURN;
            END;

        -- =============================================  
        -- Return results  
        -- =============================================  

        SELECT T.Transaction_ID AS [tranNum], 
               T.Transaction_Dtm AS [tranDate], 
               CONVERT(VARCHAR(100), DECRYPTBYKEY(T.Encrypt_Field2)) AS [SPName], 
               CONVERT(NVARCHAR, DECRYPTBYKEY(T.Encrypt_Field3)) AS [SPChiName], 
               T.SP_ID AS [SPID], 
               T.Bank_Account_No AS [BankAccountNo], 
               T.Bank_Acc_Display_Seq AS [BankAccountID], 
               T.Practice_Display_Seq AS [practiceid], 
               T.Practice_Name AS [PracticeName], 
               T.Voucher_Claim AS [voucherRedeem], 
               T.STATUS AS [status], 
               T.Authorised_Status AS [Authorised_status], 
               T.Voucher_Acc_ID, 
               T.Temp_Voucher_Acc_ID, 
               TVA.TSMP AS [Temp_Voucher_Acc_TSMP], 
               T.Special_Acc_ID, 
               SA.TSMP AS [Special_Acc_TSMP], 
               T.Invalid_Acc_ID, 
               V.TSMP, 
               T.Scheme_Code, 
               SC.Display_Code, 
               T.Claim_Amount AS [totalAmount], 
               T.Claim_Amount_RMB AS [totalAmountRMB], 
               ISNULL(T.Invalidation, '') AS [Invalidation], 
               T.Invalidation_TSMP, 
               ISNULL(T.Manual_Reimburse, 'N') AS [Manual_Reimburse],
               CASE ISNULL(V.Create_By_SmartID, 'N')
                   WHEN 'Y'
                   THEN 'C'
                   WHEN 'N'
                   THEN 'M'
               END AS [Means_Of_Input], 
               T.SchoolOrRCH_code
        FROM @TempTransaction AS T
             INNER JOIN SchemeClaim AS SC WITH(NOLOCK)
             ON T.Scheme_Code = SC.Scheme_Code
             --AND T.Scheme_Seq = SC.Scheme_Seq
             INNER JOIN VoucherTransaction AS V WITH(NOLOCK)
             ON T.Transaction_ID = V.Transaction_ID
             LEFT JOIN TempVoucherAccount AS TVA WITH(NOLOCK)
             ON T.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID
             LEFT JOIN SpecialAccount AS SA WITH(NOLOCK)
             ON T.Special_Acc_ID = SA.Special_Acc_ID
        ORDER BY T.Transaction_Dtm;

        EXEC [proc_SymmetricKey_close]
    END;  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byAny] TO HCVU;
GO