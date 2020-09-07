IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTranList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthTranList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	18 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Support reimbursed transaction with temp voucher account
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:		Koala CHENG
-- Modified date:	15 January 2018
-- CR No.:			I-CRE17-005
-- Description:		Performance Tuning
-- 					1. Add WITH (NOLOCK)
-- ============================================= 
-- =============================================  
-- Modification History  
-- CR No:			CRE11-024-02 HCVS Pilot Extension Part 2
-- Modified by:		Tony FUNG
-- Modified date:	18 November 2011  
-- Description:		Added SchemeCode_TransactionID to result set
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRE11-024-02 HCVS Pilot Extension Part 2
-- Modified by:		Tony FUNG
-- Modified date:	11 November 2011  
-- Description:		Added DocCode_IdentityNum to result set
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 May 2010
-- Description:		(1) Fix the INNER JOIN on InvalidPersonalInformation
--					(2) Retrieve [Invalidation]
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Get the list of ReimbursementAuthTran by Reimburse_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark Yip
-- Modified date:	17 Dec 2008
-- Description:		1. Total amount will be calculated based on the Claim_Amount field
--					2. Join the VoucherAccount to make the query match the index
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 August 2009
-- Description:		Inner join [SchemeClaim] and [DocType]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 September 2009
-- Description:		Remove criteria [Scheme_Code] while inner joining [VoucherAccount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Pre-calculate the total unit and total amount into the temporary table @TransactionGroupByTransactionID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	14 Sep 2009
-- Description:		Update the default sorting order to transaction_dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 September 2009
-- Description:		Add criteria Doc_Code on inner joining PersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 September 2009
-- Description:		Add @Scheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 September 2009
-- Description:		Remove the field [Doc_Code] in temporary table @TransactionGroupByTransactionID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 September 2009
-- Description:		Retrieve [PersonalInformation].[Encrypt_Field11]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	24 September 2009
-- Description:		Handle Special account and invalid account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 October 2009
-- Description:		Handle expired scheme
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReimbursementAuthTranList_get]
	@SP_ID					char(8),
	@Practice_Display_Seq	smallint,
	@Reimburse_ID			varchar(20),
	@Scheme_Code			char(10)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @TransactionGroupByTransactionID table (
		Transaction_ID			char(20),
		TotalUnit				int,
		TotalAmount				money,
		Authorised_Cutoff_Dtm	datetime
	)
	
	DECLARE @TranList table (
		Transaction_ID	char(20),
		Transaction_Dtm	datetime,
		Service_Receive_Dtm	datetime,
		Scheme_Code	char(10),
		Doc_Code char(10),
		Encrypt_Field1 varbinary(100),
		Encrypt_Field2  varbinary(100),
		Encrypt_Field3  varbinary(100),
		Encrypt_Field11 varbinary(100),
		TotalUnit int,
		TotalAmount money,
		Invalidation	char(1)
	)
	
	DECLARE @EffectiveScheme table (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @TransactionGroupByTransactionID (
		Transaction_ID,
		TotalUnit,
		TotalAmount,
		Authorised_Cutoff_Dtm
	)
	SELECT
		VT.Transaction_ID,
		SUM(TD.Unit) AS [TotalUnit],
		SUM(TD.Total_Amount) AS [TotalAmount],
		MIN(RAT.Authorised_Cutoff_Dtm) AS [Authorised_Cutoff_Dtm]
		
	FROM
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN ReimbursementAuthTran RAT WITH (NOLOCK)
				ON VT.Transaction_ID = RAT.Transaction_ID
	
	WHERE
		VT.SP_ID = @SP_ID
			AND VT.Practice_Display_Seq = @Practice_Display_Seq
			AND RAT.Reimburse_ID = @Reimburse_ID
			AND VT.Scheme_Code = @Scheme_Code
			
	GROUP BY
		VT.Transaction_ID
		
		
	INSERT INTO @EffectiveScheme (
		Scheme_Code,
		Scheme_Seq
	)
	SELECT
		Scheme_Code,
		MAX(Scheme_Seq)
	FROM
		SchemeClaim WITH (NOLOCK)
	WHERE
		GETDATE() >= Effective_Dtm
	GROUP BY
		Scheme_Code
-- =============================================
-- Return results
-- =============================================

	-- Validated Account
	Insert into @TranList
	(
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Scheme_Code,
		Doc_Code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		TotalUnit,
		TotalAmount,
		Invalidation
	)
	SELECT
		T.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Scheme_Code,
		P.Doc_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.Encrypt_Field11,
		T.TotalUnit AS [Total_Unit],
		T.TotalAmount AS [Total_Amount],
		VT.Invalidation
		
	FROM
		@TransactionGroupByTransactionID T
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
				ON T.Transaction_ID = VT.Transaction_ID
			INNER JOIN VoucherAccount VA WITH (NOLOCK)
				ON VT.Voucher_Acc_ID = VA.Voucher_Acc_ID
			INNER JOIN PersonalInformation P WITH (NOLOCK)
				ON VA.Voucher_Acc_ID = P.Voucher_Acc_ID
					AND VT.Doc_Code = P.Doc_Code
		
	WHERE VT.Voucher_Acc_ID <> ''
	
	-- Temp Account
	Insert into @TranList
	(
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Scheme_Code,
		Doc_Code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		TotalUnit,
		TotalAmount,
		Invalidation
	)
	SELECT
		T.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Scheme_Code,
		P.Doc_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.Encrypt_Field11,
		T.TotalUnit AS [Total_Unit],
		T.TotalAmount AS [Total_Amount],
		VT.Invalidation
		
	FROM
		@TransactionGroupByTransactionID T
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
				ON T.Transaction_ID = VT.Transaction_ID
			INNER JOIN TempVoucherAccount VA WITH (NOLOCK)
				ON VT.Temp_Voucher_Acc_ID = VA.Voucher_Acc_ID
			INNER JOIN TempPersonalInformation P WITH (NOLOCK)
				ON VA.Voucher_Acc_ID = P.Voucher_Acc_ID
					AND VT.Doc_Code = P.Doc_Code
		
	WHERE VT.Voucher_Acc_ID = '' AND ISNULL(VT.Special_Acc_ID,'')='' AND VT.Invalid_Acc_ID is NULL

	-- Special Account
	Insert into @TranList
	(
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Scheme_Code,
		Doc_Code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		TotalUnit,
		TotalAmount,
		Invalidation
	)
	SELECT
		T.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Scheme_Code,
		P.Doc_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.Encrypt_Field11,
		T.TotalUnit AS [Total_Unit],
		T.TotalAmount AS [Total_Amount],
		VT.Invalidation
		
	FROM
		@TransactionGroupByTransactionID T
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
				ON T.Transaction_ID = VT.Transaction_ID
			INNER JOIN SpecialAccount VA WITH (NOLOCK)
				ON VT.Special_Acc_ID = VA.Special_Acc_ID
			INNER JOIN SpecialPersonalInformation P WITH (NOLOCK)
				ON VA.Special_Acc_ID = P.Special_Acc_ID
					AND VT.Doc_Code = P.Doc_Code
		
	WHERE VT.Voucher_Acc_ID = '' AND VT.Special_Acc_ID is not null AND VT.Invalid_Acc_ID is NULL
	
	-- Invalid Account
	Insert into @TranList
	(
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Scheme_Code,
		Doc_Code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		TotalUnit,
		TotalAmount,
		Invalidation
	)
	SELECT
		T.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Scheme_Code,
		P.Doc_Code,
		P.Encrypt_Field1,
		P.Encrypt_Field2,
		P.Encrypt_Field3,
		P.Encrypt_Field11,
		T.TotalUnit AS [Total_Unit],
		T.TotalAmount AS [Total_Amount],
		VT.Invalidation
		
	FROM
		@TransactionGroupByTransactionID T
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
				ON T.Transaction_ID = VT.Transaction_ID
			INNER JOIN InvalidAccount VA WITH (NOLOCK)
				ON VT.Invalid_Acc_ID = VA.Invalid_Acc_ID
			INNER JOIN InvalidPersonalInformation P WITH (NOLOCK)
				ON VA.Invalid_Acc_ID = P.Invalid_Acc_ID
		
	WHERE VT.Voucher_Acc_ID = '' AND VT.Invalid_Acc_ID is not NULL
	
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	SELECT
		T.Transaction_ID,
		T.Transaction_Dtm,
		T.Service_Receive_Dtm,
		T.Scheme_Code,
		SC.Display_Code,
		T.Doc_Code,
		DT.Doc_Display_Code,
		CONVERT(char, DecryptByKey(T.Encrypt_Field1)) AS [IDNo],
		CONVERT(varchar(40), DecryptByKey(T.Encrypt_Field2)) AS [Eng_Name],
		CONVERT(nvarchar, DecryptByKey(T.Encrypt_Field3)) AS [Chi_Name],
		CONVERT(char, DecryptByKey(T.Encrypt_Field11)) AS [IDNo2],
		T.TotalUnit AS [Total_Unit],
		T.TotalAmount AS [Total_Amount],
		T.Invalidation,
		LEFT(DT.doc_display_code + Space(20), 20)  + convert(varchar, DecryptByKey(T.Encrypt_Field1)) as DocCode_IdentityNum,
		T.Scheme_Code + ' ' + T.Transaction_ID	as SchemeCode_TransactionID		-- CRE11-024-02
	FROM @TranList T
		INNER JOIN @EffectiveScheme ES
			ON T.Scheme_Code = ES.Scheme_Code
		INNER JOIN SchemeClaim SC WITH (NOLOCK)
			ON ES.Scheme_Code = SC.Scheme_Code
				AND ES.Scheme_Seq = SC.Scheme_Seq
		INNER JOIN DocType DT WITH (NOLOCK)
			ON T.Doc_Code = DT.Doc_Code	
	
	ORDER BY
		T.Transaction_Dtm
	
	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTranList_get] TO HCSP
GO
