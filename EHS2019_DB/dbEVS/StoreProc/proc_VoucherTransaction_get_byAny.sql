IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_byAny]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_byAny]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
 @transaction_id    char(20),  
 @status      char(1),  
 @authorised_status   char(1),  
 @sp_id      char(8),  
 @sp_name     varchar(40),  
 @sp_hkid     char(9),  
 @bank_acc     varchar(30),  
 @service_type     char(5),  
 @from_date     datetime,  
 @to_date     datetime,  
 @scheme_code    char(10),  
 @user_id     varchar(20),  
 @doc_code     char(20),  
 @identity_no1    varchar(20),  
 @Adoption_Prefix_Num char(7),
 @Invalidation    char(1),   
 @voucher_acc_id    varchar(15),
 @reimbursement_method char(1),
 @Means_Of_Input		char(1),
 @Service_Receive_Dtm_From		datetime,
 @Service_Receive_Dtm_To		datetime,
 @RCH_code	char(10),
 @result_limit_1st_enable BIT, 
 @result_limit_override_enable BIT,
 @override_result_limit BIT 
  
AS BEGIN  
-- =============================================  
-- Declaration  
-- =============================================  
 

 DECLARE @TempTransaction TABLE (  
  Transaction_ID   char(20),  
  Transaction_Dtm   datetime,  
  Encrypt_Field2   varbinary(100),  
  Encrypt_Field3   varbinary(100),  
  SP_ID     char(8),  
  Bank_Account_No   varchar(30),  
  Bank_Acc_Display_Seq smallint,  
  Practice_Display_Seq smallint,  
  Practice_Name   nvarchar(200),  
  Voucher_Claim   smallint,  
  Status     char(1),  
  Authorised_Status  char(1),  
  Voucher_Acc_ID   char(15),  
  Temp_Voucher_Acc_ID  char(15),  
  Special_Acc_ID   char(15),  
  Invalid_Acc_ID   char(15),  
  Scheme_Code    char(10),
--  Scheme_Seq	int,
  Claim_Amount   money,  
  Invalidation   char(1),  
  Invalidation_TSMP  binary(8),
  Manual_Reimburse char(1),
  RCH_code	char(10)
 )  
 
 DECLARE @identity_no2 varchar(20)
  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
 DECLARE @rowcount int  
 DECLARE @row_cnt_error varchar(max)
 DECLARE @errCode_lower char(5) 
 DECLARE @errCode_upper char(5) 
 SET @errCode_lower = '00009'
 SET @errCode_upper = '00017'
 
IF @identity_no1 is null
BEGIN
	set @identity_no2  = null
END
ELSE
BEGIN
	set @identity_no2 = ' ' + @identity_no1
END

  
-- =============================================  
-- Retrieve data  
-- =============================================   
  
 OPEN SYMMETRIC KEY sym_Key   
 DECRYPTION BY ASYMMETRIC KEY asym_Key  


-- ---------------------------------------------  
-- Validated Account  
-- ---------------------------------------------  
   
 INSERT INTO @TempTransaction (  
  Transaction_ID,  
  Transaction_Dtm,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  SP_ID,  
  Bank_Account_No,  
  Bank_Acc_Display_Seq,  
  Practice_Display_Seq,  
  Practice_Name,  
  Voucher_Claim,  
  Status,  
  Authorised_Status,  
  Voucher_Acc_ID,  
  Temp_Voucher_Acc_ID,  
  Special_Acc_ID,  
  Invalid_Acc_ID,  
  Scheme_Code,
--  Scheme_Seq,
  Claim_Amount,  
  Invalidation,  
  Invalidation_TSMP ,
  Manual_Reimburse,
  RCH_code
 )  
 SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	VT.Transaction_ID,  
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
	CASE WHEN
		ISNULL(RAT.Authorised_Status, '') = 'R' OR VT.Record_Status = 'R' THEN 'G' 
		ELSE ISNULL(RAT.Authorised_Status, '')
	END AS [Authorised_Status],
	VT.Voucher_Acc_ID,   
	VT.Temp_Voucher_Acc_ID,  
	VT.Special_Acc_ID,  
	VT.Invalid_Acc_ID,  
	VT.Scheme_Code,
	--  TD.Scheme_Seq,
	SUM(TD.Total_Amount),  
	VT.Invalidation,  
	TI.TSMP AS [Invalidation_TSMP],
	VT.Manual_Reimburse,
	TAF.AdditionalFieldValueCode AS [RCH_Code]   
 
 FROM VoucherTransaction VT	WITH (NOLOCK)
	INNER JOIN ServiceProvider SP	WITH (NOLOCK)
		ON VT.SP_ID = SP.SP_ID
	INNER JOIN Practice P	WITH (NOLOCK)
		ON VT.SP_ID  = P.SP_ID
			AND VT.Practice_display_seq = P.Display_seq
	INNER JOIN BankAccount B	WITH (NOLOCK)
		ON P.SP_ID = B.SP_ID
			AND P.Display_seq = B.SP_Practice_Display_Seq
			AND VT.Bank_Acc_Display_Seq = B.Display_Seq
	INNER JOIN Professional PF	WITH (NOLOCK)
		ON P.SP_ID = PF.SP_ID
			AND P.Professional_Seq = PF.Professional_Seq
	INNER JOIN PersonalInformation PINFO 	WITH (NOLOCK)
		ON VT.Voucher_Acc_ID = PINFO.Voucher_Acc_ID
			AND VT.Doc_Code = PINFO.Doc_Code	
	LEFT JOIN ReimbursementAuthTran RAT	WITH (NOLOCK)
		ON VT.Transaction_ID  = RAT.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'N'
	LEFT JOIN ManualReimbursement MR	WITH (NOLOCK)
		ON VT.Transaction_ID  = MR.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'Y'
	LEFT JOIN TransactionDetail TD	WITH (NOLOCK)
		ON VT.Transaction_ID = TD.Transaction_ID
			AND VT.Scheme_Code = TD.Scheme_Code
	LEFT JOIN TransactionInvalidation TI	WITH (NOLOCK)
		ON VT.Transaction_ID = TI.Transaction_ID
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)
		ON VT.Transaction_ID = TAF.Transaction_ID AND TAF.AdditionalFieldID = 'RHCCode'
		
 WHERE
	VT.Voucher_Acc_ID <> ''
	AND VT.Invalid_Acc_ID IS NULL
	and (@transaction_id is null or @transaction_id = VT.Transaction_ID)
	and ((@from_date is null and @to_date is null) or (VT.Transaction_Dtm between @from_date and @to_date))
	AND ((@Service_Receive_Dtm_From IS NULL AND @Service_Receive_Dtm_To IS NULL) OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
	and (@service_type is null or @service_type = VT.Service_Type)
	and (@scheme_code is null or @scheme_code = VT.Scheme_Code)
	and VT.Scheme_Code in (Select distinct Scheme_Code from UserRole where User_ID = @User_ID)
	and (@doc_code is null or @doc_code = VT.Doc_Code)
	and (@voucher_acc_id is null or @voucher_acc_id = isnull(VT.Voucher_Acc_ID,''))
	and (@Invalidation is null or @Invalidation = isnull(VT.Invalidation,''))
	and (@sp_id is null or @sp_id = VT.SP_ID)
	and (@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
	and (@sp_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
	and (@bank_acc is null or @bank_acc = B.Bank_Account_No)
	and (@identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no1)
		or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no2))
	and (@Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	AND (@status IS NULL OR @status = VT.Record_Status)

	AND (@authorised_status IS NULL
			OR (@authorised_status <> 'N' AND @authorised_status <> 'G' AND @authorised_status = RAT.Authorised_Status)
			OR (@authorised_status = 'N' AND RAT.Authorised_Status IS NULL)
			OR (@authorised_status = 'G' AND (VT.Record_Status = 'R' OR RAT.Authorised_Status = 'R'))
		)
	
	AND (@reimbursement_method IS NULL
			OR (@reimbursement_method = 'Y' AND ISNULL(VT.Manual_Reimburse, 'N') = 'Y')
			OR (@reimbursement_method = 'N' AND ISNULL(VT.Manual_Reimburse, 'N') = 'N')
		)
	AND (@Means_Of_Input IS NULL
			OR (@Means_Of_Input = 'M' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
			OR (@Means_Of_Input = 'C' AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y')
		)
	AND (@RCH_code IS NULL OR @RCH_code = TAF.AdditionalFieldValueCode)
	
 GROUP BY  
  VT.Transaction_ID,  
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
  TAF.AdditionalFieldValueCode  

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
	SELECT @rowcount = COUNT(1) FROM @TempTransaction
	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
	
	IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
		(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
		BEGIN
			--throw error if upper limit reached (error = @errCode_upper)
			--if upper limit is not enabled, throw error if lower limit is reached
			--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
END CATCH  
 
-- ---------------------------------------------  
-- Temporary Account  
-- ---------------------------------------------  
   
 INSERT INTO @TempTransaction (  
  Transaction_ID,  
  Transaction_Dtm,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  SP_ID,  
  Bank_Account_No,  
  Bank_Acc_Display_Seq,  
  Practice_Display_Seq,  
  Practice_Name,  
  Voucher_Claim,  
  Status,  
  Authorised_Status,  
  Voucher_Acc_ID,  
  Temp_Voucher_Acc_ID,  
  Special_Acc_ID,  
  Invalid_Acc_ID,  
  Scheme_Code,
--  Scheme_Seq,
  Claim_Amount,  
  Invalidation,  
  Invalidation_TSMP,
  Manual_Reimburse,
  RCH_code  
 )  
 SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	VT.Transaction_ID,  
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
	VT.Invalidation,  
	TI.TSMP AS [Invalidation_TSMP],
	VT.Manual_Reimburse,
	TAF.AdditionalFieldValueCode AS [RCH_Code]   
 
 FROM VoucherTransaction VT	WITH (NOLOCK)
	INNER JOIN ServiceProvider SP WITH (NOLOCK)
		ON VT.SP_ID = SP.SP_ID
	INNER JOIN Practice P WITH (NOLOCK)
		ON VT.SP_ID  = P.SP_ID
			AND VT.Practice_display_seq = P.Display_seq
	INNER JOIN BankAccount B WITH (NOLOCK)
		ON P.SP_ID = B.SP_ID
			AND P.Display_seq = B.SP_Practice_Display_Seq
			AND VT.Bank_Acc_Display_Seq = B.Display_Seq
	INNER JOIN Professional PF WITH (NOLOCK)
		ON P.SP_ID = PF.SP_ID
			AND P.Professional_Seq = PF.Professional_Seq
	INNER JOIN TempPersonalInformation PINFO WITH (NOLOCK)
		ON VT.Temp_Voucher_Acc_ID = PINFO.Voucher_Acc_ID
			AND VT.Doc_Code = PINFO.Doc_Code
	LEFT JOIN TransactionDetail TD	WITH (NOLOCK)
		ON VT.Transaction_ID = TD.Transaction_ID
			AND VT.Scheme_Code = TD.Scheme_Code
	LEFT JOIN TransactionInvalidation TI	WITH (NOLOCK)
		ON VT.Transaction_ID = TI.Transaction_ID
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)
		ON VT.Transaction_ID = TAF.Transaction_ID AND TAF.AdditionalFieldID = 'RHCCode'

	where isnull(VT.invalid_acc_id,'') = ''
	and isnull(VT.special_acc_id,'') = ''
	and isnull(VT.voucher_acc_id,'') = ''
	and isnull(VT.temp_voucher_acc_id,'') <> ''
	and (@transaction_id is null or @transaction_id = VT.Transaction_ID)
	and ((@from_date is null and @to_date is null) or (VT.Transaction_Dtm between @from_date and @to_date))
	AND ((@Service_Receive_Dtm_From IS NULL AND @Service_Receive_Dtm_To IS NULL) OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
	and (@service_type is null or @service_type = VT.Service_Type)
	and (@scheme_code is null or @scheme_code = VT.Scheme_Code)
	and VT.Scheme_Code in (Select distinct Scheme_Code from UserRole where User_ID = @User_ID)
	and (@doc_code is null or @doc_code = VT.Doc_Code)
	and @voucher_acc_id is null
	and (@Invalidation is null or @Invalidation = isnull(VT.Invalidation,''))
	and (@sp_id is null or @sp_id = VT.SP_ID)
	and (@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
	and (@sp_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
	and (@bank_acc is null or @bank_acc = B.Bank_Account_No)
	and (@identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no1)
		or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no2))
	and (@Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	AND (@status IS NULL OR @status = VT.Record_Status)
	and (@authorised_status is null or @authorised_status = 'N')
	and (@reimbursement_method is null)  
	AND (@Means_Of_Input IS NULL
			OR (@Means_Of_Input = 'M' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
			OR (@Means_Of_Input = 'C' AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y')
		)
	AND (@RCH_code IS NULL OR @RCH_code = TAF.AdditionalFieldValueCode)
		
 GROUP BY  
  VT.Transaction_ID,  
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
  TAF.AdditionalFieldValueCode   

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
	SELECT @rowcount = COUNT(1) FROM @TempTransaction
	-- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
	
	IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
		(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
		BEGIN
			--throw error if upper limit reached (error = @errCode_upper)
			--if upper limit is not enabled, throw error if lower limit is reached
			--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
END CATCH  
   
   
-- ---------------------------------------------  
-- Special Account  
-- ---------------------------------------------  
  
 INSERT INTO @TempTransaction (  
  Transaction_ID,  
  Transaction_Dtm,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  SP_ID,  
  Bank_Account_No,  
  Bank_Acc_Display_Seq,  
  Practice_Display_Seq,  
  Practice_Name,  
  Voucher_Claim,  
  Status,  
  Authorised_Status,  
  Voucher_Acc_ID,  
  Temp_Voucher_Acc_ID,  
  Special_Acc_ID,  
  Invalid_Acc_ID,  
  Scheme_Code,
--  Scheme_Seq,  
  Claim_Amount,  
  Invalidation,  
  Invalidation_TSMP,
  Manual_Reimburse,
  RCH_code  
 )  
 SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	VT.Transaction_ID,  
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
	CASE WHEN
		ISNULL(RAT.Authorised_Status, '') = 'R' OR VT.Record_Status = 'R' THEN 'G' 
		ELSE ISNULL(RAT.Authorised_Status, '')
	END AS [Authorised_Status],
	VT.Voucher_Acc_ID,   
	VT.Temp_Voucher_Acc_ID,  
	VT.Special_Acc_ID,  
	VT.Invalid_Acc_ID,  
	VT.Scheme_Code,  
	--  TD.Scheme_Seq,
	SUM(TD.Total_Amount),  
	VT.Invalidation,  
	TI.TSMP AS [Invalidation_TSMP],
	VT.Manual_Reimburse,
	TAF.AdditionalFieldValueCode AS [RCH_Code]   
 
from VoucherTransaction VT WITH (NOLOCK)
	INNER JOIN ServiceProvider SP WITH (NOLOCK)
		ON VT.SP_ID = SP.SP_ID
	INNER JOIN Practice P WITH (NOLOCK)
		ON VT.SP_ID  = P.SP_ID
			AND VT.Practice_display_seq = P.Display_seq
	INNER JOIN BankAccount B WITH (NOLOCK)
		ON P.SP_ID = B.SP_ID
			AND P.Display_seq = B.SP_Practice_Display_Seq
			AND VT.Bank_Acc_Display_Seq = B.Display_Seq
	INNER JOIN Professional PF WITH (NOLOCK)
		ON P.SP_ID = PF.SP_ID
			AND P.Professional_Seq = PF.Professional_Seq
	INNER JOIN SpecialPersonalInformation PINFO  WITH (NOLOCK)
		ON VT.Special_acc_id = PINFO.Special_acc_id
			AND VT.Doc_Code = PINFO.Doc_Code
	LEFT JOIN ReimbursementAuthTran RAT WITH (NOLOCK)
		ON VT.Transaction_ID  = RAT.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'N'
	LEFT JOIN TransactionDetail TD WITH (NOLOCK)
		ON VT.Transaction_ID = TD.Transaction_ID
			AND VT.Scheme_Code = TD.Scheme_Code
	LEFT JOIN TransactionInvalidation TI WITH (NOLOCK)
		ON VT.Transaction_ID = TI.Transaction_ID
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)
		ON VT.Transaction_ID = TAF.Transaction_ID AND TAF.AdditionalFieldID = 'RHCCode'

	where isnull(VT.invalid_acc_id,'') = ''
	and isnull(VT.special_acc_id,'') <> ''
	and isnull(VT.voucher_acc_id,'') = ''
	and (@transaction_id is null or @transaction_id = VT.Transaction_ID)
	and ((@from_date is null and @to_date is null) or (VT.Transaction_Dtm between @from_date and @to_date))
	AND ((@Service_Receive_Dtm_From IS NULL AND @Service_Receive_Dtm_To IS NULL) OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
	and (@service_type is null or @service_type = VT.Service_Type)
	and (@scheme_code is null or @scheme_code = VT.Scheme_Code)
	and VT.Scheme_Code in (Select distinct Scheme_Code from UserRole where User_ID = @User_ID)
	and (@doc_code is null or @doc_code = VT.Doc_Code)
	and @voucher_acc_id is null
	and (@Invalidation is null or @Invalidation = isnull(VT.Invalidation,''))
	and (@sp_id is null or @sp_id = VT.SP_ID)
	and (@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
	and (@sp_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
	and (@bank_acc is null or @bank_acc = B.Bank_Account_No)
	and (@identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no1)
		or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no2))
	and (@Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	AND (@status IS NULL OR @status = VT.Record_Status)
	AND (@authorised_status IS NULL
			OR (@authorised_status <> 'N' AND @authorised_status <> 'G' AND @authorised_status = RAT.Authorised_Status)
			OR (@authorised_status = 'N' AND RAT.Authorised_Status IS NULL)
			OR (@authorised_status = 'G' AND (VT.Record_Status = 'R' OR RAT.Authorised_Status = 'R'))
		)
	and (@reimbursement_method is null or
			(@reimbursement_method = 'N' and isnull(VT.Manual_Reimburse,'N') = 'N')
		)
	AND (@Means_Of_Input IS NULL
			OR (@Means_Of_Input = 'M' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
			OR (@Means_Of_Input = 'C' AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y')
		)
	AND (@RCH_code IS NULL OR @RCH_code = TAF.AdditionalFieldValueCode)

 GROUP BY  
  VT.Transaction_ID,  
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
  TAF.AdditionalFieldValueCode   

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
	SELECT @rowcount = COUNT(1) FROM @TempTransaction
	-- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
	
	IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
		(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
		BEGIN
			--throw error if upper limit reached (error = @errCode_upper)
			--if upper limit is not enabled, throw error if lower limit is reached
			--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
END CATCH  
  
   
-- ---------------------------------------------  
-- Invalid Account  
-- ---------------------------------------------  
  
 INSERT INTO @TempTransaction (  
  Transaction_ID,  
  Transaction_Dtm,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  SP_ID,  
  Bank_Account_No,  
  Bank_Acc_Display_Seq,  
  Practice_Display_Seq,  
  Practice_Name,  
  Voucher_Claim,  
  Status,  
  Authorised_Status,  
  Voucher_Acc_ID,  
  Temp_Voucher_Acc_ID,  
  Special_Acc_ID,  
  Invalid_Acc_ID,  
  Scheme_Code,  
--  Scheme_Seq,
  Claim_Amount,  
  Invalidation,  
  Invalidation_TSMP,
  Manual_Reimburse,
  RCH_code  
 )  
 SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable))
	VT.Transaction_ID,  
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
	CASE WHEN
	ISNULL(RAT.Authorised_Status, '') = 'R' OR VT.Record_Status = 'R' THEN 'G' 
	ELSE ISNULL(RAT.Authorised_Status, '')
	END AS [Authorised_Status],
	VT.Voucher_Acc_ID,   
	VT.Temp_Voucher_Acc_ID,  
	VT.Special_Acc_ID,  
	VT.Invalid_Acc_ID,  
	VT.Scheme_Code,  
	--  TD.Scheme_Seq,
	SUM(TD.Total_Amount),  
	VT.Invalidation,  
	TI.TSMP AS [Invalidation_TSMP],
	VT.Manual_Reimburse,
	TAF.AdditionalFieldValueCode AS [RCH_Code]   
 
 from VoucherTransaction VT WITH (NOLOCK)
	INNER JOIN ServiceProvider SP WITH (NOLOCK)
		ON VT.SP_ID = SP.SP_ID
	INNER JOIN Practice P WITH (NOLOCK)
		ON VT.SP_ID  = P.SP_ID
			AND VT.Practice_display_seq = P.Display_seq
	INNER JOIN BankAccount B WITH (NOLOCK)
		ON P.SP_ID = B.SP_ID
			AND P.Display_seq = B.SP_Practice_Display_Seq
			AND VT.Bank_Acc_Display_Seq = B.Display_Seq
	INNER JOIN Professional PF WITH (NOLOCK)
		ON P.SP_ID = PF.SP_ID
			AND P.Professional_Seq = PF.Professional_Seq
	INNER JOIN InvalidPersonalInformation PINFO WITH (NOLOCK) 
		ON VT.invalid_acc_id = PINFO.invalid_acc_id
	LEFT JOIN ReimbursementAuthTran RAT WITH (NOLOCK)
		ON VT.Transaction_ID  = RAT.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'N'
	LEFT JOIN ManualReimbursement MR WITH (NOLOCK)
		ON VT.Transaction_ID  = MR.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'Y'
	LEFT JOIN TransactionDetail TD WITH (NOLOCK)
		ON VT.Transaction_ID = TD.Transaction_ID
			AND VT.Scheme_Code = TD.Scheme_Code
	LEFT JOIN TransactionInvalidation TI WITH (NOLOCK)
		ON VT.Transaction_ID = TI.Transaction_ID
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)
		ON VT.Transaction_ID = TAF.Transaction_ID AND TAF.AdditionalFieldID = 'RHCCode'

	where isnull(VT.invalid_acc_id,'') <> ''
	and (@transaction_id is null or @transaction_id = VT.Transaction_ID)
	and ((@from_date is null and @to_date is null) or (VT.Transaction_Dtm between @from_date and @to_date))
	AND ((@Service_Receive_Dtm_From IS NULL AND @Service_Receive_Dtm_To IS NULL) OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
	and (@service_type is null or @service_type = VT.Service_Type)
	and (@scheme_code is null or @scheme_code = VT.Scheme_Code)
	and VT.Scheme_Code in (Select distinct Scheme_Code from UserRole where User_ID = @User_ID)
	and (@doc_code is null or @doc_code = VT.Doc_Code)
	and @voucher_acc_id is null
	and (@Invalidation is null or @Invalidation = isnull(VT.Invalidation,''))
	and (@sp_id is null or @sp_id = VT.SP_ID)
	and (@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
	and (@sp_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
	and (@bank_acc is null or @bank_acc = B.Bank_Account_No)
	and (@identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no1)
		or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity_no2))
	and (@Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
	AND (@status IS NULL OR @status = VT.Record_Status)
	AND (@authorised_status IS NULL
			OR (@authorised_status <> 'N' AND @authorised_status <> 'G' AND @authorised_status = RAT.Authorised_Status)
			OR (@authorised_status = 'N' AND RAT.Authorised_Status IS NULL)
			OR (@authorised_status = 'G' AND (VT.Record_Status = 'R' OR RAT.Authorised_Status = 'R'))
		)
	AND (@reimbursement_method IS NULL
			OR (@reimbursement_method = 'Y' AND ISNULL(VT.Manual_Reimburse, 'N') = 'Y')
			OR (@reimbursement_method = 'N' AND ISNULL(VT.Manual_Reimburse, 'N') = 'N')
		)
	AND (@Means_Of_Input IS NULL
			OR (@Means_Of_Input = 'M' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N')
			OR (@Means_Of_Input = 'C' AND ISNULL(VT.Create_By_SmartID, 'N') = 'Y')
		)
	AND (@RCH_code IS NULL OR @RCH_code = TAF.AdditionalFieldValueCode)

 GROUP BY  
  VT.Transaction_ID,  
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
  TAF.AdditionalFieldValueCode

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
	SELECT @rowcount = COUNT(1) FROM @TempTransaction
	-- SQL exception is caught by [catch] block if row limit is reached. No result will be selected out
	EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
END TRY

BEGIN CATCH    	    
	SET @row_cnt_error = ERROR_MESSAGE()    
	
	IF (isnull(@row_cnt_error,'') <> '' AND @row_cnt_error <> @errCode_lower) or
		(@result_limit_override_enable = 0 AND @row_cnt_error = @errCode_lower )
		BEGIN
			--throw error if upper limit reached (error = @errCode_upper)
			--if upper limit is not enabled, throw error if lower limit is reached
			--if the error is not related to upper / lower limit, there must be sth wrong in the try block, throw the error immediately
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END
END CATCH   


-- =============================================    
-- Throw out error if lower limit is reached 
-- =============================================  
IF isnull(@row_cnt_error,'') = @errCode_lower 
		BEGIN		
			RAISERROR (@row_cnt_error,16,1)    
			CLOSE SYMMETRIC KEY sym_Key  
			RETURN
		END

-- =============================================  
-- Return results  
-- =============================================  
  
 SELECT  
	T.Transaction_ID AS [tranNum],  
	T.Transaction_Dtm AS [tranDate],  
	CONVERT(varchar(40), DecryptByKey(T.Encrypt_Field2)) AS [SPName],  
	CONVERT(nvarchar, DecryptByKey(T.Encrypt_Field3)) AS [SPChiName],  
	T.SP_ID AS [SPID],  
	T.Bank_Account_No AS [BankAccountNo],  
	T.Bank_Acc_Display_Seq AS [BankAccountID],  
	T.Practice_Display_Seq AS [practiceid],  
	T.Practice_Name AS [PracticeName],  
	T.Voucher_Claim AS [voucherRedeem],   
	T.Status AS [status],  
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
	ISNULL(T.Invalidation, '') AS [Invalidation],  
	T.Invalidation_TSMP,
	isnull(T.Manual_Reimburse,'N') AS [Manual_Reimburse],
	CASE ISNULL(V.Create_By_SmartID, 'N')
	WHEN 'Y' THEN 'C'
	WHEN 'N' THEN 'M'
	END AS [Means_Of_Input],
	CASE WHEN
		LTRIM(RTRIM(T.Scheme_Code)) = 'RVP' THEN T.RCH_code
		ELSE NULL
	END AS [RCH_Code]   
 
 FROM  
  @TempTransaction T
  INNER JOIN SchemeClaim SC WITH (NOLOCK)
	ON T.Scheme_Code = SC.Scheme_Code
		--AND T.Scheme_Seq = SC.Scheme_Seq
   INNER JOIN VoucherTransaction V WITH (NOLOCK)  
	ON T.Transaction_ID = V.Transaction_ID  
   LEFT JOIN TempVoucherAccount TVA WITH (NOLOCK)  
	ON T.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID  
   LEFT JOIN SpecialAccount SA WITH (NOLOCK)  
	ON T.Special_Acc_ID = SA.Special_Acc_ID  
       
 ORDER BY  
  T.Transaction_Dtm  

 CLOSE SYMMETRIC KEY sym_Key  
   
END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byAny] TO HCVU
GO

