IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementGeneratePaymentFile_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementGeneratePaymentFile_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson LAW
-- Modified date:	07 March 2018
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
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
-- Description:		Avoid concurrent update in Generate Payment File. To prevent the following situation:
--					(1) Reimbursement of another scheme is in progress
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 November 2010
-- Description:		Insert queue to SQLJobQueue
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		14 August 2009
-- Description:		Reimbursement Generate Payment File
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 August 2009
-- Description:		Add criteria [ReimbursementAuthTran].[Scheme_Code] = @scheme_code on initializing @voucher_claim, @total_amount, and @rowcount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	08 Sep 2009
-- Description:		Change the calculation on No. of units redeem and total amount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 October 2009
-- Description:		Refine the fields in padding left zero
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	09 Oct 2009
-- Description:		Update the float to bigint
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementGeneratePaymentFile_add] 
	@current_user			varchar(20),
	@reimburse_id			char(15),
	@bank_payment_dtm		datetime,
	@scheme_code			char(10),
	@auto_plan_code			char(1),
	@first_party_acc_no		char(12),
	@payment_code			char(3),
	@first_party_reference	char(12),
	@value_date				char(6),
	@file_name				char(8)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @current_dtm						datetime
	DECLARE @rowcount							bigint
	DECLARE	@voucher_claim						float
	DECLARE @total_amount						bigint
	DECLARE	@cutoff_dtm							datetime
	--DECLARE	@overflow_total_no_instruction		char(7)
	DECLARE @overflow_total_amt_instruction		char(12)	
	DECLARE @Reimbursement_Mode					varchar(2)
	DECLARE @VerifiyCaseAvailable				CHAR(1)
	
-- =============================================
-- Validation 
-- =============================================

--	(1) Reimbursement of another scheme is in progress

	IF (SELECT	COUNT(1)
		FROM	(
			(SELECT Scheme_Code FROM ReimbursementAuthorisation WHERE Reimburse_ID = @reimburse_id AND Authorised_Status = 'P' AND Record_Status = 'A'
			 INTERSECT
			 SELECT Scheme_Code FROM SchemeClaim WHERE Record_Status = 'A' AND Reimbursement_Mode = (
				SELECT Reimbursement_Mode FROM SchemeClaim WHERE Scheme_Code = @scheme_code
			 )
			)
			EXCEPT
			(SELECT Scheme_Code FROM ReimbursementAuthorisation WHERE Reimburse_ID = @reimburse_id AND Authorised_Status = '2' AND Record_Status = 'A'
			 INTERSECT
			 SELECT Scheme_Code FROM SchemeClaim WHERE Record_Status = 'A' AND Reimbursement_Mode = (
				SELECT Reimbursement_Mode FROM SchemeClaim WHERE Scheme_Code = @scheme_code
			 )
			)
			) T
		) <> 0 BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@ERROR
	END


-- =============================================
-- Initialization
-- =============================================
	SELECT @current_dtm = GETDATE()

	SELECT	@voucher_claim = SUM(TD.Unit) 
	FROM	ReimbursementAuthTran R 
			INNER JOIN TransactionDetail TD ON R.Transaction_ID = TD.Transaction_ID
	WHERE	R.Reimburse_ID = @reimburse_id AND R.Scheme_Code = @scheme_code

	SELECT	@total_amount = SUM(TD.Total_Amount) 
	FROM	ReimbursementAuthTran R 
			INNER JOIN TransactionDetail TD ON R.Transaction_ID = TD.Transaction_ID
	WHERE	R.Reimburse_ID = @reimburse_id AND R.Scheme_Code = @scheme_code

	SELECT @cutoff_dtm = Cutoff_Date FROM ReimbursementAuthorisation WHERE Reimburse_ID = @reimburse_id AND Authorised_Status = 'S'	

/*
	SELECT @overflow_total_amt_instruction = ISNULL(REPLICATE('0', 12 - LEN(ISNULL(CONVERT(varchar, CONVERT(integer, @total_amount) * 100), 0))), '')
												+ CONVERT(varchar, CONVERT(integer, @total_amount) * 100)
*/
												
	SELECT @overflow_total_amt_instruction = RIGHT('000000000000' + CONVERT(varchar, @total_amount) + '00', 12)
												
	SELECT @Reimbursement_Mode = Reimbursement_Mode FROM SchemeClaim WHERE Scheme_Code = @scheme_code
												
	--	GET Verification Case Available or not
	SELECT 
		@VerifiyCaseAvailable = Verification_Case_Available 
	FROM 
		ReimbursementAuthorisation 
	WHERE 
		Reimburse_ID = @reimburse_id 
			AND Authorised_Status = 'S' 
				AND Record_Status = 'A'											
-- =============================================
-- Return results
-- =============================================

-- Step 1: Insert into table [BankInDataFile]
	IF @Reimbursement_Mode = '99' BEGIN
		INSERT INTO BankInDataFile
				   ([Reimburse_ID]
				   ,[Record_Seq]
				   ,[FILLER]
				   ,[Second_Party_Identifier]
				   ,[Second_Party_Bank_Acc_Name]
				   ,[Second_Party_Bank_No]
				   ,[Second_Party_Branch_No]
				   ,[Second_Party_Account]
				   ,[Amount]
				   ,[Filler_2]
				   ,[Second_Party_ID_Continuation]
				   ,[Second_Party_Reference]
				   ,[Scheme_Code])  
		SELECT 	
			R.Reimburse_ID AS [Reimburse_ID],
			
			ROW_NUMBER() OVER(ORDER BY T.SP_ID, T.Practice_Display_Seq) AS [Record_Seq], 
			
			REPLICATE(' ', 1) AS [FILLER],
			
			T.SP_ID + ' ' + RIGHT('0' + T.Practice_Display_Seq, 2) + ' ' AS [Second_Party_Identifier], 
			
			LEFT(B.Bank_Acc_Holder, 20) + ISNULL(REPLICATE(' ', 20 - LEN(ISNULL(LEFT(B.Bank_Acc_Holder, 20) ,' '))), '') AS [Second_Party_Bank_Acc_Name],
			
			SUBSTRING(T.Bank_Account_No, 1, 3) AS [Second_Party_Bank_No],
			
			SUBSTRING(T.Bank_Account_No, 5, 3) AS [Second_Party_Branch_No],
			
			SUBSTRING(T.Bank_Account_No, 9, LEN(T.Bank_Account_No))
				+ ISNULL(REPLICATE(' ', 9 - LEN(ISNULL(SUBSTRING(T.Bank_Account_No, 9 , LEN(T.Bank_Account_No)), ' '))), '') AS [Second_Party_Account],
			
			/*
			ISNULL(REPLICATE('0', 10 - LEN(ISNULL(CONVERT(varchar, CONVERT(integer, SUM(T.Claim_Amount)) * 100), 0))), '')
				+ CONVERT(varchar, CONVERT(integer, SUM(T.Claim_Amount)) * 100) AS [Amount],
			*/
			RIGHT('0000000000' + CONVERT(varchar, CONVERT(BIGINT, SUM(T.Claim_Amount))) + '00', 10) AS [Amount],
			
			REPLICATE(' ', 4) AS [Filler_2],
			
			REPLICATE(' ', 6) AS [Second_Party_ID_Continuation],
			
			REPLICATE(' ', 12) AS [Second_Party_Reference],
			
			@scheme_code AS [Scheme_Code]
			
		FROM
			VoucherTransaction T
				INNER JOIN ReimbursementAuthTran R
					ON T.Transaction_ID = R.Transaction_ID
				INNER JOIN BankAccount B
					ON T.SP_ID = B.SP_ID 
						AND T.Practice_Display_Seq = B.SP_Practice_Display_Seq 
			
		WHERE 
			R.Reimburse_ID = @reimburse_id
				AND T.Scheme_Code = @scheme_code
			
		GROUP BY  
			R.Reimburse_ID, T.SP_ID, T.Practice_Display_Seq, T.Bank_Account_No, B.Bank_Acc_Holder
		
		ORDER BY
			T.SP_ID, T.Practice_Display_Seq
			
	END
		
-- Step 2: Insert into table [BankIn]
	SELECT @rowcount = COUNT(1) FROM BankInDataFile WHERE Reimburse_ID = @reimburse_id AND Scheme_Code = @scheme_code

	INSERT INTO BankIn
				([Reimburse_ID]
				,[Submission_Dtm]
				,[Submitted_By]
				,[Transaction_Count]
				,[Vouchers_Count]
				,[Total_Amount]
				,[Value_Date]
				,[Scheme_code]
				,[Record_Status])
	SELECT
				 @reimburse_id
				,@current_dtm
				,@current_user
				,@rowcount
				,@voucher_claim
				,@total_amount
				,@bank_payment_dtm
				,@scheme_code
				,CASE @Reimbursement_Mode
					WHEN '99' THEN 'P'
					WHEN '1' THEN 'N'
					ELSE NULL
				 END AS [Record_Status]

-- Step 3: Insert into table [BankInHeaderFile]
	
	IF @Reimbursement_Mode = '99' BEGIN
		INSERT INTO [BankInHeaderFile]
					([Reimburse_ID]
					,[Auto_Plan_Code]
					,[First_Party_Acc_No]
					,[Payment_Code]
					,[First_Party_Reference]
					,[Value_Date]
					,[Input_Medium]
					,[File_Name]
					,[Total_No_Instruction]
					,[Total_Amt_Instruction]
					,[Overflow_Total_No_Instruction]
					,[Overflow_Total_Amt_Instruction]
					,[FILLER]
					,[Instruction_Source]
					,[Scheme_Code])
		 VALUES
					(@reimburse_id
					,@auto_plan_code
					,@first_party_acc_no
					,@payment_code
					,@first_party_reference
					,@value_date
					,'K'
					,@file_name
					,REPLICATE(' ', 5)
					,REPLICATE(' ', 10)
					,RIGHT('0000000' + CONVERT(varchar, @rowcount), 7)
					,@overflow_total_amt_instruction
					,'  '
					,'1'
					,@scheme_code)
					
	END

-- Step 4: Insert into table [ReimbursementAuthorisation] ([Authorised_Status = 'R')
	INSERT INTO ReimbursementAuthorisation
			   ([Authorised_Dtm]
			   ,[Authorised_Status]
			   ,[Authorised_By]
			   ,[Reimburse_ID]
			   ,[Cutoff_Date]
			   ,[Record_Status]
			   ,[Create_By]
			   ,[Create_Dtm]
			   ,[Update_By]
			   ,[Update_Dtm]
			   ,[Scheme_Code]
			   ,[Verification_Case_Available])
	VALUES
			   (@current_dtm
			   ,'R'
			   ,@current_user
			   ,@reimburse_id
			   ,@cutoff_dtm
			   ,'A'
			   ,@current_user
			   ,@current_dtm
			   ,@current_user
			   ,@current_dtm
			   ,@scheme_code
			   ,@VerifiyCaseAvailable)

-- Step 5: Update table [ReimbursementAuthTran] ([Authorised_Status = 'R')
	UPDATE	ReimbursementAuthTran
	SET		Authorised_Status = 'R'
	WHERE	Reimburse_ID = @reimburse_id
				AND Scheme_Code = @scheme_code

-- Step 6: Insert queue to [SQLJobQueue] so as to patch the VoucherTransaction.Record_Status to 'R' at night
	DECLARE @NextQueueID varchar(10)
	
	SELECT @NextQueueID = CONVERT(varchar, MAX(CONVERT(int, Queue_ID)) + 1)
	FROM SQLJobQueue
	
	SELECT @NextQueueID = RIGHT('0000000000' + @NextQueueID, 10)
	
	IF @NextQueueID IS NULL BEGIN
		SELECT @NextQueueID = '0000000001'
	END

	INSERT INTO SQLJobQueue (
		Queue_ID,
		Job_ID,
		SP,
		SP_Parm,
		Record_Status,
		Request_Dtm,
		Start_Dtm,
		Complete_Dtm
	)
	SELECT
		@NextQueueID,
		Job_ID,
		SP,
		'''' + @reimburse_id + ''', ''' + @scheme_code + '''',
		'A' AS [Record_Status],
		GETDATE() AS [Request_Dtm],
		NULL AS [Start_Dtm],
		NULL AS [Complete_Dtm]
	FROM
		SQLJob
	WHERE
		Job_ID = 'REIMBURSEPATCH'
		
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementGeneratePaymentFile_add] TO HCVU
GO
