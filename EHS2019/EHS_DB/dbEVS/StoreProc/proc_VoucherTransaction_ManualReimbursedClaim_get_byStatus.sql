IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus]
GO

/****** Object:  StoredProcedure [dbo].[proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus]    Script Date: 07/13/2010 19:06:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
-- Modified date:	11 Oct 2018
-- CR No.:			CRE17-018-07
-- Description:		Retrieve Temporary Account's Claim
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
-- Modified by:		Derek LEUNG
-- Modified date:	3 Nov 2010
-- Description:		Check status on vouchertransaction table instead of manualreimburse
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	25 Jul 2010
-- Description:		Retrieve record from 'VoucherTransaction' & 
--					'ManualReimbursement'
-- =============================================
-- =============================================
-- Author:	Derek Leung	
-- Create date: 13 July 2010
-- Description: Retrieve VoucherTransaction record	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

--exec proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus 'R', 'HAADMIN',1,1,0

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus] 	
	@status		char(1)	, 
	@user_id    varchar(20),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
	
AS BEGIN
-- =============================================
-- Declaration
-- =============================================

	DECLARE @rowcount INT
	DECLARE @row_cnt_error varchar(max)
	
	DECLARE @UserRoleSchemeCode TABLE (
		Scheme_Code				char(10)
	)
	
	DECLARE @TempTransaction TABLE (
		Transaction_ID			char(20),
		Transaction_Dtm			datetime,
		Encrypt_Field2			varbinary(100),
		Encrypt_Field3			varbinary(100),
		SP_ID					char(8),
		Bank_Account_No			varchar(30),
		Bank_Acc_Display_Seq	smallint,
		Practice_Display_Seq	smallint,
		Practice_Name			nvarchar(200),
		Voucher_Claim			smallint,
		Status					char(1),
		Authorised_Status		char(1),
		Voucher_Acc_ID			char(15),
		Temp_Voucher_Acc_ID		char(15),
		Special_Acc_ID			char(15),
		Invalid_Acc_ID			char(15),
		Scheme_Code				char(10),
		Claim_Amount			money,
		Invalidation			char(1),
		Invalidation_TSMP		binary(8),
		Create_By				char(20),
		Create_Dtm				datetime,
		Service_Receive_Dtm		datetime, 
		Creation_Reason			varchar(255), 
		Override_Reason			varchar(255)
	)

	DECLARE @EffectiveScheme table (
		Scheme_Code				char(10),
		Scheme_Seq				smallint
	)
	
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	
	INSERT INTO @UserRoleSchemeCode (
		Scheme_Code
	)
	SELECT DISTINCT	Scheme_Code
	FROM			UserRole
	WHERE			User_ID = @user_id
	
	INSERT INTO @EffectiveScheme (
		Scheme_Code,
		Scheme_Seq
	)
	SELECT
		Scheme_Code,
		MAX(Scheme_Seq)
	FROM
		SchemeClaim
	--WHERE								-- also get records with inactive scheme
	--	GETDATE() >= Effective_Dtm
	--		AND Record_Status = 'A'
	GROUP BY
		Scheme_Code
	

-- =============================================
-- Retrieve data
-- =============================================	

	EXEC [proc_SymmetricKey_open]
	
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
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Scheme_Code,
		Claim_Amount,
		Invalidation,
		Create_By, 
		Create_Dtm, 
		Service_Receive_Dtm, 
		Creation_Reason, 
		Override_Reason
	)
	SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		T.Transaction_ID,
		T.Transaction_Dtm,
		A.Encrypt_Field2,
		A.Encrypt_Field3,
		T.SP_ID,
		T.Bank_Account_No,
		T.Bank_Acc_Display_Seq,
		T.Practice_Display_Seq,
		P.Practice_Name,
		SUM(TD.Unit),
		T.Voucher_Acc_ID, 
		T.Temp_Voucher_Acc_ID,
		T.Special_Acc_ID,
		T.Invalid_Acc_ID,
		T.Scheme_Code,
		SUM(TD.Total_Amount),
		T.Invalidation,
		T.Create_By,
		T.Create_Dtm, 
		T.Service_Receive_Dtm, 
		MR.Creation_Reason, 
		MR.Override_Reason

	FROM
		VoucherTransaction T 			
			INNER JOIN ServiceProvider A
				ON T.SP_ID = A.SP_ID
			INNER JOIN Practice P
				ON T.SP_ID = P.SP_ID 
					AND T.Practice_Display_Seq = P.Display_Seq
			INNER JOIN Professional PROF
				ON T.SP_ID = PROF.SP_ID
					AND P.Professional_Seq = PROF.Professional_Seq
			INNER JOIN BankAccount B
				ON T.SP_ID = B.SP_ID
					AND T.Practice_Display_Seq = B.SP_Practice_Display_Seq
					AND T.Bank_Acc_Display_Seq = B.Display_Seq
			INNER JOIN PersonalInformation VR
				ON T.Voucher_Acc_ID = VR.Voucher_Acc_ID 
					AND T.Doc_Code = VR.Doc_Code					
					AND ISNULL(T.Voucher_Acc_ID, '') <> ''
					AND ISNULL(T.Invalid_Acc_ID, '') = ''
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID	
			INNER JOIN ManualReimbursement MR
				ON T.Transaction_ID = MR.Transaction_ID
					AND ISNULL(T.Manual_Reimburse,'N') ='Y'			
	WHERE    
		T.Record_Status = @Status
	GROUP BY
		T.Transaction_ID,
		T.Transaction_Dtm,
		A.Encrypt_Field2,
		A.Encrypt_Field3,
		T.SP_ID,
		T.Bank_Account_No,
		T.Bank_Acc_Display_Seq,
		T.Practice_Display_Seq,
		P.Practice_Name,
		T.Record_Status,
		T.Voucher_Acc_ID, 
		T.Temp_Voucher_Acc_ID,
		T.Special_Acc_ID,
		T.Invalid_Acc_ID,
		T.Scheme_Code,
		T.Invalidation,
		T.Create_By, 
		T.Create_Dtm, 
		T.Service_Receive_Dtm, 
		MR.Creation_Reason, 
		MR.Override_Reason,
		T.Manual_Reimburse

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
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Scheme_Code,
		Claim_Amount,
		Invalidation,
		Create_By, 
		Create_Dtm, 
		Service_Receive_Dtm, 
		Creation_Reason, 
		Override_Reason
	)
	SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		T.Transaction_ID,
		T.Transaction_Dtm,
		A.Encrypt_Field2,
		A.Encrypt_Field3,
		T.SP_ID,
		T.Bank_Account_No,
		T.Bank_Acc_Display_Seq,
		T.Practice_Display_Seq,
		P.Practice_Name,
		SUM(TD.Unit),
		T.Voucher_Acc_ID, 
		T.Temp_Voucher_Acc_ID,
		T.Special_Acc_ID,
		T.Invalid_Acc_ID,
		T.Scheme_Code,
		SUM(TD.Total_Amount),
		T.Invalidation,
		T.Create_By,
		T.Create_Dtm, 
		T.Service_Receive_Dtm, 
		MR.Creation_Reason, 
		MR.Override_Reason
	FROM
		VoucherTransaction T 			
			INNER JOIN ServiceProvider A
				ON T.SP_ID = A.SP_ID
			INNER JOIN Practice P
				ON T.SP_ID = P.SP_ID 
					AND T.Practice_Display_Seq = P.Display_Seq
			INNER JOIN Professional PROF
				ON T.SP_ID = PROF.SP_ID
					AND P.Professional_Seq = PROF.Professional_Seq
			INNER JOIN BankAccount B
				ON T.SP_ID = B.SP_ID
					AND T.Practice_Display_Seq = B.SP_Practice_Display_Seq
					AND T.Bank_Acc_Display_Seq = B.Display_Seq
			INNER JOIN TempPersonalInformation TVR
				ON T.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID 
					AND T.Doc_Code = TVR.Doc_Code					
					AND ISNULL(T.Temp_Voucher_Acc_ID, '') <> ''
					AND ISNULL(T.Voucher_Acc_ID, '') = ''
					AND ISNULL(T.Invalid_Acc_ID, '') = ''
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID	
			INNER JOIN ManualReimbursement MR
				ON T.Transaction_ID = MR.Transaction_ID
					AND ISNULL(T.Manual_Reimburse,'N') ='Y'		
	WHERE    
		T.Record_Status = @Status
	GROUP BY
		T.Transaction_ID,
		T.Transaction_Dtm,
		A.Encrypt_Field2,
		A.Encrypt_Field3,
		T.SP_ID,
		T.Bank_Account_No,
		T.Bank_Acc_Display_Seq,
		T.Practice_Display_Seq,
		P.Practice_Name,
		T.Record_Status,
		T.Voucher_Acc_ID, 
		T.Temp_Voucher_Acc_ID,
		T.Special_Acc_ID,
		T.Invalid_Acc_ID,
		T.Scheme_Code,
		T.Invalidation,
		T.Create_By, 
		T.Create_Dtm, 
		T.Service_Receive_Dtm, 
		MR.Creation_Reason, 
		MR.Override_Reason,
		T.Manual_Reimburse

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
			SELECT	@rowcount = COUNT(1)
			FROM	@TempTransaction

		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		CLOSE SYMMETRIC KEY sym_Key  
		RETURN
	END CATCH 

--
	EXEC [proc_SymmetricKey_close]
-- =============================================
-- Return results
-- =============================================

	EXEC [proc_SymmetricKey_open]

	SELECT
		T.Transaction_ID AS [tranNum],
		T.Transaction_Dtm AS [tranDate],
		CONVERT(varchar(100), DecryptByKey(T.Encrypt_Field2)) AS [SPName],
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
		T.Special_Acc_ID,
		T.Invalid_Acc_ID,
		V.TSMP,
		T.Scheme_Code,
		SC.Display_Code,
		T.Claim_Amount AS [totalAmount],
		ISNULL(T.Invalidation, '') AS [Invalidation],
		T.Invalidation_TSMP, 
		T.Create_By, 
		T.Create_Dtm, 
		T.Service_Receive_Dtm, 
		T.Creation_Reason, 
		T.Override_Reason

	FROM
		@TempTransaction T			
			INNER JOIN @EffectiveScheme ES
				ON T.Scheme_Code = ES.Scheme_Code
			INNER JOIN @UserRoleSchemeCode UR
				ON T.Scheme_Code = UR.Scheme_Code
			INNER JOIN SchemeClaim SC
				ON ES.Scheme_Code = SC.Scheme_Code
					AND ES.Scheme_Seq = SC.Scheme_Seq
			INNER JOIN VoucherTransaction V
				ON T.Transaction_ID = V.Transaction_ID
					
	ORDER BY
		T.Transaction_Dtm
		
	EXEC [proc_SymmetricKey_close]
	
END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_ManualReimbursedClaim_get_byStatus] TO HCVU
GO

