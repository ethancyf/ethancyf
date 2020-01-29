IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_bySPAspect]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_bySPAspect]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	01 Feb 2018
-- CR No.:			CRE17-012
-- Description:		Add Chinese name search for SP and EHA
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
-- CR No.:		   CRE13-012  
-- Modified by:    Koala CHENG
-- Modified date:  14 Jul 2016  
-- Description:    Claim Transaction Management/Enquiry Revamp
-- ============================================= 

--exec  [dbo].[proc_VoucherTransaction_get_byAny] null,'R',null,null,null,null,'01 Jan 1980 00:00:00','01 Jan 2013 23:59:59',null,'HAADMIN',null,null,null,null,null,null, 1,1,1
--exec  [dbo].[proc_VoucherTransaction_get_byAny] null, null,null,null,null,null,'01 Jan 1980 00:00:00','01 Jan 2013 23:59:59',null,'HAADMIN',null,null,null,null,null,null, 1,1,1
      
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_get_bySPAspect]    
 @sp_id      char(8),  
 @sp_name     varchar(40),  
 @sp_chi_name     nvarchar(6),  
 @sp_hkid     char(9),  
 @bank_acc     varchar(30),  
 @from_date     datetime,  
 @to_date     datetime,  
 @Service_Receive_Dtm_From		datetime,
 @Service_Receive_Dtm_To		datetime,
 @scheme_code    char(10),  
 @status      char(1),  
 @authorised_status   char(1),  
 @Invalidation    char(1),   
 @reimbursement_method char(1),
 @Means_Of_Input		char(1),
 @RCH_code	char(10),
 @user_id     varchar(20), 
 @result_limit_1st_enable BIT, 
 @result_limit_override_enable BIT,
 @override_result_limit BIT 
  
AS BEGIN  
-- =============================================  
-- Declaration  
-- =============================================  
 
CREATE TABLE #TempTransaction(  
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
	Claim_Amount   money,  
	Invalidation   char(1),  
	Invalidation_TSMP  binary(8),
	Manual_Reimburse char(1),
	RCH_code	char(10)
)  

CREATE CLUSTERED INDEX IDX_C_TempTransaction ON #TempTransaction(Transaction_ID)
 
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

DECLARE @Use_Tx_Date AS INT

SET @Use_Tx_Date = 0

IF @from_date IS NOT NULL
	SET @Use_Tx_Date = 1

IF @Service_Receive_Dtm_From IS NOT NULL
	SET @Use_Tx_Date = 2
  
-- =============================================  
-- Retrieve data  
-- =============================================   
  
 OPEN SYMMETRIC KEY sym_Key   
 DECRYPTION BY ASYMMETRIC KEY asym_Key  

-- ---------------------------------------------  
-- Validated Account  
-- ---------------------------------------------  

--IF @sp_id IS NULL AND @sp_name IS NULL AND @sp_hkid IS NULL AND @bank_acc IS NULL
IF @sp_id IS NULL AND @sp_name IS NULL AND @sp_chi_name IS NULL AND @sp_hkid IS NULL AND @bank_acc IS NULL
BEGIN
	INSERT INTO #TempTransaction (  
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
		Claim_Amount,  
		Invalidation,  
		Invalidation_TSMP ,
		Manual_Reimburse,
		RCH_code)  
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
		(
			((@Use_Tx_Date = 1) and VT.Transaction_Dtm between @from_date and @to_date)
			OR
			((@Use_Tx_Date = 2) and VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To)
		)	
		AND (@scheme_code is null or @scheme_code = VT.Scheme_Code)
		AND EXISTS (Select distinct Scheme_Code from UserRole where User_ID = @User_ID AND Scheme_Code = VT.Scheme_Code)
		AND (@Invalidation is null or @Invalidation = isnull(VT.Invalidation,''))
		AND (@sp_id is null or @sp_id = VT.SP_ID)
		AND (@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
		AND (@sp_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
		AND (@sp_chi_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name) = SP.Encrypt_Field3)
		AND (@bank_acc is null or @bank_acc = B.Bank_Account_No)
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
		VT.Invalidation,  
		TI.TSMP,
		MR.Record_Status,
		VT.Manual_Reimburse,  
		TAF.AdditionalFieldValueCode  
END
ELSE
BEGIN   
	INSERT INTO #TempTransaction (  
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
		Claim_Amount,  
		Invalidation,  
		Invalidation_TSMP ,
		Manual_Reimburse,
		RCH_code)  
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
		((@from_date is null and @to_date is null) or (VT.Transaction_Dtm between @from_date and @to_date))
		AND ((@Service_Receive_Dtm_From IS NULL AND @Service_Receive_Dtm_To IS NULL) OR (VT.Service_Receive_Dtm BETWEEN @Service_Receive_Dtm_From AND @Service_Receive_Dtm_To))
		AND (@scheme_code is null or @scheme_code = VT.Scheme_Code)
		AND EXISTS (Select distinct Scheme_Code from UserRole where User_ID = @User_ID AND Scheme_Code = VT.Scheme_Code)
		AND (@Invalidation is null or @Invalidation = isnull(VT.Invalidation,''))
		AND (@sp_id is null or @sp_id = VT.SP_ID)
		AND (@sp_hkid is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid) = SP.Encrypt_Field1)
		AND (@sp_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_name) = SP.Encrypt_Field2)
		AND (@sp_chi_name is null or EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name) = SP.Encrypt_Field3)
		AND (@bank_acc is null or @bank_acc = B.Bank_Account_No)
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
		VT.Invalidation,  
		TI.TSMP,
		MR.Record_Status,
		VT.Manual_Reimburse,  
		TAF.AdditionalFieldValueCode  
END

-- =============================================    
-- Max Row Checking  
-- =============================================  
BEGIN TRY       
	SELECT @rowcount = COUNT(1) FROM #TempTransaction
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
		#TempTransaction T
		INNER JOIN SchemeClaim SC WITH (NOLOCK)
			ON T.Scheme_Code = SC.Scheme_Code
		INNER JOIN VoucherTransaction V WITH (NOLOCK)  
			ON T.Transaction_ID = V.Transaction_ID  
		LEFT JOIN TempVoucherAccount TVA WITH (NOLOCK)  
			ON T.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID  
		LEFT JOIN SpecialAccount SA WITH (NOLOCK)  
			ON T.Special_Acc_ID = SA.Special_Acc_ID  
       
	ORDER BY  
		T.Transaction_Dtm  

	CLOSE SYMMETRIC KEY sym_Key  
   
   	DROP TABLE #TempTransaction 

END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_bySPAspect] TO HCVU
GO

