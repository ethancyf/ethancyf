IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_BySPIDBankAcctTransID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_BySPIDBankAcctTransID]
GO

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
-- CR No.:			CRE20-0XX (Immu record)
-- Modified by:		Raiman CHONG
-- Modified date:	1 Feb 2021
-- Description:		Add Search Criteria [DocType] AND [DocNo]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-015-12 (Tx Summary)
-- Modified by:		Koala CHENG
-- Modified date:	23 Dec 2020
-- Description:		Get [ConsultAndRegFeeRMB] AND [Subsidize_Code] for SSSCMC scheme
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
-- CR No.:			CRE20-015 (HA Scheme)
-- Modified by:		Winnie SUEN
-- Modified date:	16 Oct 2020
-- Description:		Show [Total_Claim_Amount_RMB] for SSSCMC scheme
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Display "DHC-related Services" in other info
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	26 August 2018
-- CR No.:			CRE17-018
-- Description:		Display "School Code" as additional field
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
-- CR No:			CRE16-026 (Add PCV13)
-- Modified by:		Chris YIM
-- Modified date:	12 Sep 2017
-- Description:		Add item to Other Info : High Risk
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRE16-002 (Revamp VSS)
-- Modified by:		Lawrence TSANG
-- Modified date:	31 August 2016
-- Description:		Add item to Other Info
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRE15-005 (New PIDVSS scheme)
-- Modified by:		Winnie SUEN
-- Modified date:	8 Sep 2015
-- Description:		1. Display Type Of Documentary Proof as additional field
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRE15-004 (TIV and QIV)
-- Modified by:		Winnie SUEN
-- Modified date:	8 Sep 2015
-- Description:		1. Display [Display_Code_For_Claim] instead of [display_code] for [Subsidize_Item_Desc]
--					2. Add Category Label
--					3. Line break for each additional info
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRP15-001 (Fix duplicated record in HCSP after search)
-- Modified by:		Chris YIM
-- Modified date:	11 August 2015
-- Description:		1. Fix the joining with transaction_detail
-- =============================================  


CREATE PROCEDURE [dbo].[proc_VoucherTransaction_get_BySPIDBankAcctTransID]
	@TransactionID	char(20),
	@TranDtmFrom	datetime,
	@TranDtmTo		datetime,
	@SPID			char(8),
	@DataEntry		varchar(20),
	@Practice_Seq	smallint,
	@Record_Status	char(1),
	@Scheme_Code	char(10),
	@Available_HCSP_SubPlatform	char(2),
	@doc_code     char(20),    
	@identity_no1    varchar(20),    
	@Adoption_Prefix_Num char(7)
AS BEGIN
-- =============================================
-- Declaration
-- ============================================= 
	DECLARE @Performance_Start_Dtm datetime
	SET @Performance_Start_Dtm = GETDATE()
	
	DECLARE @In_TransactionID	char(20)
	DECLARE @In_TranDtmFrom		datetime
	DECLARE @In_TranDtmTo		datetime
	DECLARE @In_SPID			char(8)
	DECLARE @In_DataEntry		varchar(20)
	DECLARE @In_Practice_Seq	smallint
	DECLARE @In_Record_Status	char(1)
	DECLARE @In_Scheme_Code		char(10)
	DECLARE @In_Available_HCSP_SubPlatform	char(2)
	DECLARE @In_doc_code     char(20)  
	DECLARE @In_identity_no1    varchar(20)  
	DECLARE @In_Adoption_Prefix_Num char(7)

	SET @In_TransactionID = @TransactionID
	SET @In_TranDtmFrom = @TranDtmFrom
	SET @In_TranDtmTo = @TranDtmTo
	SET @In_SPID = @SPID
	SET @In_DataEntry = @DataEntry
	SET @In_Practice_Seq = @Practice_Seq
	SET @In_Record_Status = @Record_Status
	SET @In_Scheme_Code = @Scheme_Code
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform
	SET @In_doc_code    = @doc_code 
	SET @In_identity_no1    = @identity_no1 
	SET @In_Adoption_Prefix_Num = @Adoption_Prefix_Num 

	DECLARE @In_identity_no2 varchar(20)  

	IF @In_identity_no1 is null  
	BEGIN  
	 set @In_identity_no2  = null  
	END  
	ELSE  
	BEGIN  
	 set @In_identity_no2 = ' ' + @In_identity_no1  
	END 

	DECLARE @maxrow					int
	
	DECLARE @TempTransaction TABLE (
		Transaction_ID				char(20),
		Transaction_Dtm				datetime,
		Total_Claim_Amount			money,
		Total_Claim_Amount_RMB		money,
		Bank_Account_No				varchar(30),
		Encrypt_Field1				varbinary(100),
		Encrypt_Field2				varbinary(100),
		Encrypt_Field3				varbinary(100),
		Encrypt_Field11				varbinary(100),
		Record_Status				char(1),
		DataEntry_By				varchar(20),
		AccountType					char(1),
		SP_ID						char(8),
		Practice_Display_Seq		smallint,
		Practice_Name				nvarchar(100),
		Practice_Name_Chi			nvarchar(100),
		Doc_Code					char(20),
		Scheme_Code					char(10),
		Display_Code				char(10),
		Invalidation				char(1), 
		Manual_Reimburse            char(1),
		Service_Type				char(5), 
		IsUpload					varchar(1),
		Category_Code				varchar(10),
		High_Risk					char(1),
		DHC_Service					char(1),
		Subsidize_Code				char(10),
		ConsultAndRegFeeRMB			Money
	)

	DECLARE @TempTransactionAdditionalField table (
		Transaction_ID				char(20),
		AdditionalFieldID			varchar(20),
		AdditionalFieldValueCode	varchar(50),
		AdditionalFieldValueDesc	nvarchar(255)
	)

	DECLARE @OtherInfo table (
		Transaction_ID				char(20),
		Item_Group_Seq				int,
		Display_Seq					int,
		Content_EN					nvarchar(500),
		Content_TC					nvarchar(500),
		Content_SC					nvarchar(500)
	)
	

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SELECT	@maxrow = Parm_Value1
	FROM	SystemParameters WITH (NOLOCK)
	WHERE	Parameter_Name = 'MaxRowRetrieve' 
				AND Record_Status = 'A'

			
	-- Validated account
	
	INSERT INTO @TempTransaction (
		Transaction_ID,
		Transaction_Dtm,
		Total_Claim_Amount,
		Total_Claim_Amount_RMB,
		Bank_Account_No,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		Record_Status,
		DataEntry_By,
		AccountType,
		SP_ID,
		Practice_Display_Seq,
		Practice_Name,
		Practice_Name_Chi,
		Doc_Code,
		Scheme_Code,
		Invalidation, 
		Manual_Reimburse,
		Service_Type, 
		IsUpload,
		Category_Code,
		High_Risk,
		DHC_Service,
		Subsidize_Code,
		ConsultAndRegFeeRMB
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Claim_Amount,
		TD.Total_Amount_RMB,
		VT.Bank_Account_No,
		PINFO.Encrypt_Field1,
		PINFO.Encrypt_Field2,
		PINFO.Encrypt_Field3,
		PINFO.Encrypt_Field11,
		VT.Record_Status,
		VT.DataEntry_By,
		'V' AS [AccountType],
		VT.SP_ID,
		VT.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		PINFO.Doc_Code,
		VT.Scheme_Code,
		VT.Invalidation, 
		VT.Manual_Reimburse,
		VT.Service_Type, 
		VT.IsUpload,
		VT.Category_Code,
		VT.High_Risk,
		VT.DHC_Service,
		TD.Subsidize_Code,
		TAF.AdditionalFieldValueCode
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
	INNER JOIN PersonalInformation PINFO WITH (NOLOCK) 
		ON VT.Voucher_Acc_ID = PINFO.Voucher_Acc_ID
			AND VT.Doc_Code = PINFO.Doc_Code
	LEFT JOIN ReimbursementAuthTran RAT WITH (NOLOCK)
		ON VT.Transaction_ID  = RAT.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'N'
	LEFT JOIN ManualReimbursement MR WITH (NOLOCK)
		ON VT.Transaction_ID  = MR.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'Y'
	LEFT JOIN SchemeClaim SC WITH (NOLOCK)
		ON VT.Scheme_Code = SC.Scheme_Code
	LEFT JOIN TransactionDetail TD WITH (NOLOCK)  
		ON VT.Transaction_ID = TD.Transaction_ID  AND TD.Total_Amount_RMB IS NOT NULL
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)  
		ON VT.Transaction_ID = TAF.Transaction_ID  AND TAF.AdditionalFieldID = 'ConsultAndRegFeeRMB'

	where isnull(VT.[voucher_acc_id],'') <> ''
	and isnull(VT.[invalid_acc_id],'') = ''
	--and isnull(MR.Record_Status,'R') = 'R'
	AND VT.Record_Status NOT IN ('B', 'D')
	and (@In_TransactionID is null or @In_TransactionID = VT.Transaction_ID)
	and (VT.Transaction_Dtm between @In_TranDtmFrom and @In_TranDtmTo)
	and (@In_SPID = VT.SP_ID)
	and (@In_DataEntry is null or (@In_DataEntry = VT.DataEntry_By AND VT.Record_Status in ('U','P','I')))
	and (@In_Practice_Seq is null or @In_Practice_Seq = VT.Practice_Display_Seq)
	and (@In_Scheme_Code is null or @In_Scheme_Code = VT.Scheme_Code)
	AND (@In_Record_Status IS NULL OR @In_Record_Status = VT.Record_Status)
	AND (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
	AND (@In_identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no1)  
			or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no2))
	 AND (@In_Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_Adoption_Prefix_Num)) 
	  AND (@In_doc_code is null or @doc_code = VT.Doc_Code)  
	IF (SELECT COUNT(1) FROM @TempTransaction) > @maxrow
	BEGIN
		RAISERROR('00009', 16, 1)
		RETURN @@error
	END	
	
	-- Temporary account
	
	INSERT INTO @TempTransaction (
		Transaction_ID,
		Transaction_Dtm,
		Total_Claim_Amount,
		Total_Claim_Amount_RMB,
		Bank_Account_No,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		Record_Status,
		DataEntry_By,
		AccountType,
		SP_ID,
		Practice_Display_Seq,
		Practice_Name,
		Practice_Name_Chi,
		Doc_Code,
		Scheme_Code,
		Invalidation, 
		Manual_Reimburse,
		Service_Type, 
		IsUpload,
		Category_Code,
		High_Risk,
		DHC_Service,
		Subsidize_Code,
		ConsultAndRegFeeRMB
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Claim_Amount,
		TD.Total_Amount_RMB,
		VT.Bank_Account_No,
		PINFO.Encrypt_Field1,
		PINFO.Encrypt_Field2,
		PINFO.Encrypt_Field3,
		PINFO.Encrypt_Field11,
		VT.Record_Status,
		VT.DataEntry_By,
		'T' AS [AccountType],
		VT.SP_ID,
		VT.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		PINFO.Doc_Code,
		VT.Scheme_Code,
		VT.Invalidation, 
		VT.Manual_Reimburse,
		VT.Service_Type, 
		VT.IsUpload,
		VT.Category_Code,
		VT.High_Risk,
		VT.DHC_Service,
		TD.Subsidize_Code,
		TAF.AdditionalFieldValueCode
	FROM VoucherTransaction VT WITH (NOLOCK)
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
	LEFT JOIN SchemeClaim SC WITH (NOLOCK)
		ON VT.Scheme_Code = SC.Scheme_Code
	LEFT JOIN TransactionDetail TD WITH (NOLOCK)  
		ON VT.Transaction_ID = TD.Transaction_ID  AND TD.Total_Amount_RMB IS NOT NULL
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)  
		ON VT.Transaction_ID = TAF.Transaction_ID  AND TAF.AdditionalFieldID = 'ConsultAndRegFeeRMB'

	where isnull(VT.invalid_acc_id,'') = ''
	and isnull(VT.special_acc_id,'') = ''
	and isnull(VT.voucher_acc_id,'') = ''
	and isnull(VT.temp_voucher_acc_id,'') <> ''
	AND VT.Record_Status NOT IN ('B', 'D')
	and (@In_TransactionID is null or @In_TransactionID = VT.Transaction_ID)
	and (VT.Transaction_Dtm between @In_TranDtmFrom and @In_TranDtmTo)
	and (@In_SPID = VT.SP_ID)
	and (@In_DataEntry is null or (@In_DataEntry = VT.DataEntry_By AND VT.Record_Status in ('U','P','I')))
	and (@In_Practice_Seq is null or @In_Practice_Seq = VT.Practice_Display_Seq)
	and (@In_Scheme_Code is null or @In_Scheme_Code = VT.Scheme_Code)
	and (@In_Record_Status is null or		
			(@In_Record_Status <> 'R' and 
				isnull(VT.Manual_Reimburse,'N') = 'N' and 
				@In_Record_Status = VT.Record_Status)
		)
	AND (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
		AND (@In_identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no1)  
			or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no2))
	 AND (@In_Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_Adoption_Prefix_Num)) 
	  AND (@In_doc_code is null or @doc_code = VT.Doc_Code)  
	IF (SELECT COUNT(1) FROM @TempTransaction) > @maxrow
	BEGIN
		RAISERROR('00009', 16, 1)
		RETURN @@error
	END
		
	-- Special account
	
	INSERT INTO @TempTransaction (
		Transaction_ID,
		Transaction_Dtm,
		Total_Claim_Amount,
		Total_Claim_Amount_RMB,
		Bank_Account_No,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		Record_Status,
		DataEntry_By,
		AccountType,
		SP_ID,
		Practice_Display_Seq,
		Practice_Name,
		Practice_Name_Chi,
		Doc_Code,
		Scheme_Code,
		Invalidation, 
		Manual_Reimburse,
		Service_Type, 
		IsUpload,
		Category_Code,
		High_Risk,
		DHC_Service,
		Subsidize_Code,
		ConsultAndRegFeeRMB
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Claim_Amount,
		TD.Total_Amount_RMB,
		VT.Bank_Account_No,
		PINFO.Encrypt_Field1,
		PINFO.Encrypt_Field2,
		PINFO.Encrypt_Field3,
		PINFO.Encrypt_Field11,
		VT.Record_Status,
		VT.DataEntry_By,
		'S' AS [AccountType],
		VT.SP_ID,
		VT.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		VT.Doc_Code,
		VT.Scheme_Code,
		VT.Invalidation, 
		VT.Manual_Reimburse,
		VT.Service_Type, 
		VT.IsUpload,
		VT.Category_Code,
		VT.High_Risk,
		VT.DHC_Service,
		TD.Subsidize_Code,
		TAF.AdditionalFieldValueCode
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
	INNER JOIN SpecialPersonalInformation PINFO WITH (NOLOCK) 
		ON VT.Special_acc_id = PINFO.Special_acc_id
			AND VT.Doc_Code = PINFO.Doc_Code
	LEFT JOIN ReimbursementAuthTran RAT WITH (NOLOCK)
		ON VT.Transaction_ID  = RAT.Transaction_ID
			AND isnull(VT.Manual_Reimburse,'N') = 'N'
	LEFT JOIN SchemeClaim SC WITH (NOLOCK)
		ON VT.Scheme_Code = SC.Scheme_Code
	LEFT JOIN TransactionDetail TD WITH (NOLOCK)  
		ON VT.Transaction_ID = TD.Transaction_ID  AND TD.Total_Amount_RMB IS NOT NULL
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)  
		ON VT.Transaction_ID = TAF.Transaction_ID  AND TAF.AdditionalFieldID = 'ConsultAndRegFeeRMB'

	where isnull(VT.invalid_acc_id,'') = ''
	and isnull(VT.special_acc_id,'') <> ''
	and isnull(VT.voucher_acc_id,'') = ''
	AND VT.Record_Status NOT IN ('B', 'D')
	and (@In_TransactionID is null or @In_TransactionID = VT.Transaction_ID)
	and (VT.Transaction_Dtm between @In_TranDtmFrom and @In_TranDtmTo)
	and (@In_SPID = VT.SP_ID)
	and (@In_DataEntry is null or (@In_DataEntry = VT.DataEntry_By AND VT.Record_Status in ('U','P','I')))
	and (@In_Practice_Seq is null or @In_Practice_Seq = VT.Practice_Display_Seq)
	and (@In_Scheme_Code is null or @In_Scheme_Code = VT.Scheme_Code)
	and (@In_Record_Status is null or
			(@In_Record_Status = 'R' and isnull(RAT.Authorised_Status,'') = 'R') or
			(@In_Record_Status <> 'R' and 
				isnull(VT.Manual_Reimburse,'N') = 'N' and 
				@In_Record_Status = VT.Record_Status and
				isnull(RAT.Authorised_Status,'') <> 'R')
		)
	AND (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
		AND (@In_identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no1)  
			or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no2))
	 AND (@In_Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_Adoption_Prefix_Num)) 
	  AND (@In_doc_code is null or @doc_code = VT.Doc_Code)  
		
	IF (SELECT COUNT(1) FROM @TempTransaction) > @maxrow
	BEGIN
		RAISERROR('00009', 16, 1)
		RETURN @@error
	END
	
	-- Invalid account
	
	INSERT INTO @TempTransaction
	(
		Transaction_ID,
		Transaction_Dtm,
		Total_Claim_Amount,
		Total_Claim_Amount_RMB,
		Bank_Account_No,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field11,
		Record_Status,
		DataEntry_By,
		AccountType,
		SP_ID,
		Practice_Display_Seq,
		Practice_Name,
		Practice_Name_Chi,
		Doc_Code,
		Scheme_Code,
		Invalidation, 
		Manual_Reimburse,
		Service_Type, 
		IsUpload,
		Category_Code,
		High_Risk,
		DHC_Service,
		Subsidize_Code,
		ConsultAndRegFeeRMB
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Claim_Amount,
		TD.Total_Amount_RMB,
		VT.Bank_Account_No,
		PINFO.Encrypt_Field1,
		PINFO.Encrypt_Field2,
		PINFO.Encrypt_Field3,
		PINFO.Encrypt_Field11,
		VT.Record_Status,
		VT.DataEntry_By,
		'I' AS [AccountType],
		VT.SP_ID,
		VT.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		VT.Doc_Code,
		VT.Scheme_Code,
		VT.Invalidation,
		VT.Manual_Reimburse,
		VT.Service_Type, 
		VT.IsUpload,
		VT.Category_Code,
		VT.High_Risk,
		VT.DHC_Service,
		TD.Subsidize_Code,
		TAF.AdditionalFieldValueCode

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
	LEFT JOIN SchemeClaim SC WITH (NOLOCK)
		ON VT.Scheme_Code = SC.Scheme_Code
	LEFT JOIN TransactionDetail TD WITH (NOLOCK)  
		ON VT.Transaction_ID = TD.Transaction_ID AND TD.Total_Amount_RMB IS NOT NULL
	LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)  
		ON VT.Transaction_ID = TAF.Transaction_ID  AND TAF.AdditionalFieldID = 'ConsultAndRegFeeRMB'

	where isnull(VT.invalid_acc_id,'') <> ''
	AND VT.Record_Status NOT IN ('B', 'D')
	and (@In_TransactionID is null or @In_TransactionID = VT.Transaction_ID)
	and (VT.Transaction_Dtm between @In_TranDtmFrom and @In_TranDtmTo)
	and (@In_SPID = VT.SP_ID)
	and (@In_DataEntry is null or @In_DataEntry = VT.DataEntry_By)
	and (@In_Practice_Seq is null or @In_Practice_Seq = VT.Practice_Display_Seq)
	and (@In_Scheme_Code is null or @In_Scheme_Code = VT.Scheme_Code)
	AND (@In_Record_Status IS NULL OR @In_Record_Status = VT.Record_Status)
	AND (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
		AND (@In_identity_no1 is null or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no1)  
			or PINFO.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_identity_no2))
	 AND (@In_Adoption_Prefix_Num is null or PINFO.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @In_Adoption_Prefix_Num)) 
	  AND (@In_doc_code is null or @doc_code = VT.Doc_Code)  

	IF (SELECT COUNT(1) FROM @TempTransaction) > @maxrow
	BEGIN
		RAISERROR('00009', 16, 1)
		RETURN @@error
	END
	
	--

	UPDATE
		@TempTransaction
	SET
		Display_Code = SC.Display_Code
	FROM
		@TempTransaction V
			INNER JOIN SchemeClaim SC
				ON V.Scheme_Code = SC.Scheme_Code


-- ---------------------------------------------------
-- Perpare Other Info
-- ---------------------------------------------------

	INSERT INTO @TempTransactionAdditionalField (
		Transaction_ID,
		AdditionalFieldID,
		AdditionalFieldValueCode,
		AdditionalFieldValueDesc
	)
	SELECT
		T.Transaction_ID,
		TAF.AdditionalFieldID,
		TAF.AdditionalFieldValueCode,
		TAF.AdditionalFieldValueDesc
	FROM
		@TempTransaction T
			INNER JOIN TransactionAdditionalField TAF WITH (NOLOCK)
				ON T.Transaction_ID = TAF.Transaction_ID


	-- ================ Glossary ================
	-- Item_Group_Seq 1: Vaccine and Dose (e.g. QIV-PID 2016/17 (1st Dose))
	-- Item_Group_Seq 11: Category
	-- Item_Group_Seq 21: DocumentaryProof (PID)
	-- Item_Group_Seq 22: DocumentaryProof (DA)
	-- Item_Group_Seq 31: PlaceVaccination (VSS Part 1: non-Others)
	-- Item_Group_Seq 31: PlaceVaccination (VSS Part 2: Others)
	-- Item_Group_Seq 32: PlaceVaccination (ENHVSSO scheme)
	-- Item_Group_Seq 41: DocumentaryProof (PIDVSS scheme)
	-- Item_Group_Seq 51: EHAPP CoPayment
	-- Item_Group_Seq 55: DHC-related Services (HCVS)
	-- Item_Group_Seq 61: CoPaymentFee (HCVS) (with formatting the string to 1,234,567)
	-- Item_Group_Seq 71: CoPaymentFeeRMB (HCVSCHN) (with formatting the string to 1,234,567)
	-- Item_Group_Seq 72: PaymentType (HCVSCHN)
	-- Item_Group_Seq 81: Reason_for_Visit (Header)
	-- Item_Group_Seq 82: Reason_for_Visit
	-- Item_Group_Seq 83: Reason_for_Visit_S1
	-- Item_Group_Seq 84: Reason_for_Visit_S2
	-- Item_Group_Seq 85: Reason_for_Visit_S3
	-- Item_Group_Seq 91: High Risk
	-- Item_Group_Seq 92: School Code
	-- Item_Group_Seq 101: RegistrationFeeRMB (SSSCMC) (with formatting the string to 1,234,567)
	-- Item_Group_Seq 102: CoPaymentFeeRMB (SSSCMC) (with formatting the string to 1,234,567)

	-- ==========================================


	-- Item_Group_Seq 1: Vaccine and Dose (e.g. QIV-PID 2016/17 (1st Dose))

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		1,
		SGC.Display_Seq,
		CASE WHEN TD.Available_Item_Code = 'ONLYDOSE'
			THEN SGC.Display_Code_For_Claim
			ELSE SGC.Display_Code_For_Claim + ' (' + SID.Available_Item_Desc + ')'
		END AS [Content_EN],
		CASE WHEN TD.Available_Item_Code = 'ONLYDOSE'
			THEN SGC.Display_Code_For_Claim
			ELSE SGC.Display_Code_For_Claim + ' (' + SID.Available_Item_Desc_Chi + ')'
		END AS [Content_TC],
		CASE WHEN TD.Available_Item_Code = 'ONLYDOSE'
			THEN SGC.Display_Code_For_Claim
			ELSE SGC.Display_Code_For_Claim + ' (' + SID.Available_Item_Desc_CN + ')'
		END AS [Content_SC]
	FROM
		@TempTransaction T
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON T.Transaction_ID = TD.Transaction_ID
			INNER JOIN SubsidizeItem SI WITH (NOLOCK)
				ON TD.Subsidize_Item_Code = SI.Subsidize_Item_Code
			INNER JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
				ON TD.Scheme_Code = SGC.Scheme_Code
					AND TD.Scheme_Seq = SGC.Scheme_Seq
					AND TD.Subsidize_Code = SGC.Subsidize_Code
			INNER JOIN SubsidizeItemDetails SID WITH (NOLOCK)
				ON TD.Subsidize_Item_Code = SID.Subsidize_Item_Code
					AND TD.Available_Item_Code = SID.Available_Item_Code
	WHERE
		SI.Subsidize_Type = 'VACCINE'


	-- Item_Group_Seq 11: Category

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		11,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'Category'),
			CC.Category_Name
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'Category'),
			CC.Category_Name_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'Category'),
			CC.Category_Name_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN ClaimCategory CC WITH (NOLOCK)
				ON T.Category_Code = CC.Category_Code


	-- Item_Group_Seq 21: DocumentaryProof (PID)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		21,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND T.Category_Code = 'VSSPID'
					AND TAF.AdditionalFieldID = 'DocumentaryProof'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'VSSPID_DOCUMENTARYPROOF'


	-- Item_Group_Seq 22: DocumentaryProof (DA)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		22,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND T.Category_Code = 'VSSDA'
					AND TAF.AdditionalFieldID = 'DocumentaryProof'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'VSSDA_DOCUMENTARYPROOF'


	-- Item_Group_Seq 31: PlaceVaccination (VSS Part 1: non-Others)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		31,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND TAF.AdditionalFieldID = 'PlaceVaccination'
					AND TAF.AdditionalFieldValueCode <> 'OTHERS'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'VSS_PLACEOFVACCINATION'

	-- Item_Group_Seq 31: PlaceVaccination (VSS Part 2: Others)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		31,
		1,
		FORMATMESSAGE('%s: %s - %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value,
			TAF.AdditionalFieldValueDesc
		),
		FORMATMESSAGE('%s: %s - %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value_Chi,
			TAF.AdditionalFieldValueDesc
		),
		FORMATMESSAGE('%s: %s - %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value_CN,
			TAF.AdditionalFieldValueDesc
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND TAF.AdditionalFieldID = 'PlaceVaccination'
					AND TAF.AdditionalFieldValueCode = 'OTHERS'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'VSS_PLACEOFVACCINATION'

	-- Item_Group_Seq 32: PlaceVaccination (ENHVSSO scheme)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		32,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PlaceOfVaccination'),
			SD.Data_Value_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'ENHVSSO'
					AND TAF.AdditionalFieldID = 'PlaceVaccination'
					AND TAF.AdditionalFieldValueCode <> 'OTHERS'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'ENHVSSO_PLACEOFVACCINATION'

	-- Item_Group_Seq 41: DocumentaryProof (PIDVSS scheme)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		41,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'TypeOfDocumentaryProof'),
			SD.Data_Value_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'PIDVSS'
					AND TAF.AdditionalFieldID = 'DocumentaryProof'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'PIDVSS_DOCUMENTARYPROOF'


	-- Item_Group_Seq 51: EHAPP CoPayment

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		51,
		1,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN FORMATMESSAGE('%s: %s',
					(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'EHAPP_CoPayment'),
					SD.Data_Value
				 )
			ELSE FORMATMESSAGE('%s: %s (HCV Amount $%s + $%s)',
					(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'EHAPP_CoPayment'),
					SD.Data_Value,
					TAF2.AdditionalFieldValueCode,
					TAF3.AdditionalFieldValueCode
				 )
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN FORMATMESSAGE('%s: %s',
					(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'EHAPP_CoPayment'),
					SD.Data_Value_Chi
				 )
			ELSE FORMATMESSAGE(N'%s: %s (醫療券金額 $%s + $%s)',
					(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'EHAPP_CoPayment'),
					SD.Data_Value_Chi,
					TAF2.AdditionalFieldValueCode,
					TAF3.AdditionalFieldValueCode
				 )
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN FORMATMESSAGE('%s: %s',
					(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'EHAPP_CoPayment'),
					SD.Data_Value_CN
				 )
			ELSE FORMATMESSAGE(N'%s: %s (医疗券金额 $%s + $%s)',
					(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'EHAPP_CoPayment'),
					SD.Data_Value_CN,
					TAF2.AdditionalFieldValueCode,
					TAF3.AdditionalFieldValueCode
				 )
		END
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'CoPayment'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF1.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'EHAPP_COPAYMENT'
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'HCVAmount'
			LEFT JOIN @TempTransactionAdditionalField TAF3
				ON T.Transaction_ID = TAF3.Transaction_ID
					AND TAF3.AdditionalFieldID = 'NetServiceFee'


	-- Item_Group_Seq 55: DHC-related Services (HCVS)
	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		55,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'DHCRelatedService'),
			SR.[Description]
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'DHCRelatedService'),
			SR.[Chinese_Description]
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'DHCRelatedService'),
			SR.[CN_Description]
		)
	FROM
		@TempTransaction T
		INNER JOIN SystemResource SR
		ON SR.ObjectType = 'Text' AND SR.ObjectName = CASE 
														WHEN T.DHC_Service = 'Y' THEN 'Yes' 
														WHEN T.DHC_Service = 'N' THEN 'No' 
														ELSE 'NA' END
	WHERE
		T.DHC_Service IS NOT NULL


	-- Item_Group_Seq 61: CoPaymentFee (HCVS) (with formatting the string to 1,234,567)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		61,
		1,
		FORMATMESSAGE('%s: $%s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'CoPaymentFee'),
			FORMAT(CONVERT(int, TAF.AdditionalFieldValueCode), 'N0')
		),
		FORMATMESSAGE('%s: $%s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'CoPaymentFee'),
			FORMAT(CONVERT(int, TAF.AdditionalFieldValueCode), 'N0')
		),
		FORMATMESSAGE('%s: $%s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'CoPaymentFee'),
			FORMAT(CONVERT(int, TAF.AdditionalFieldValueCode), 'N0')
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CoPaymentFee'


	-- Item_Group_Seq 71: CoPaymentFeeRMB (HCVSCHN) (with formatting the string to 1,234,567)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		71,
		1,
		FORMATMESSAGE('%s: ¥%s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'CoPaymentFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END
		),
		FORMATMESSAGE('%s: ¥%s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'CoPaymentFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END
		),
		FORMATMESSAGE('%s: ¥%s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'CoPaymentFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CoPaymentFeeRMB'
	WHERE
		T.Scheme_Code = 'HCVSCHN'

	-- Item_Group_Seq 72: PaymentType (HCVSCHN)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		72,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PaymentType'),
			SD.Data_Value
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PaymentType'),
			SD.Data_Value_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'PaymentType'),
			SD.Data_Value_CN
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'PaymentType'
			INNER JOIN StaticData SD WITH (NOLOCK)
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'HCVSCHN_PAYMENTTYPE'
	WHERE
		T.Scheme_Code = 'HCVSCHN'

	-- Item_Group_Seq 81: Reason_for_Visit (Header)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		81,
		1,
		FORMATMESSAGE('%s:',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'ReasonVisit')
		),
		FORMATMESSAGE('%s:',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'ReasonVisit')
		),
		FORMATMESSAGE('%s:',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'ReasonVisit')
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'


	-- Item_Group_Seq 82: Reason_for_Visit

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		82,
		1,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1, R2.Reason_L2)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_Chi
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_Chi, R2.Reason_L2_Chi)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_CN
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_CN, R2.Reason_L2_CN)
		END
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			INNER JOIN ReasonForVisitL1 R1 WITH (NOLOCK)
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'Reason_for_Visit_L2'
			LEFT JOIN ReasonForVisitL2 R2 WITH (NOLOCK)
				ON T.Service_Type = R2.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R2.Reason_L1_Code
					AND TAF2.AdditionalFieldValueCode = R2.Reason_L2_Code


	-- Item_Group_Seq 83: Reason_for_Visit_S1

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		83,
		1,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1, R2.Reason_L2)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_Chi
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_Chi, R2.Reason_L2_Chi)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_CN
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_CN, R2.Reason_L2_CN)
		END
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'ReasonforVisit_S1_L1'
			INNER JOIN ReasonForVisitL1 R1 WITH (NOLOCK)
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S1_L2'
			LEFT JOIN ReasonForVisitL2 R2 WITH (NOLOCK)
				ON T.Service_Type = R2.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R2.Reason_L1_Code
					AND TAF2.AdditionalFieldValueCode = R2.Reason_L2_Code


	-- Item_Group_Seq 84: Reason_for_Visit_S2

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		84,
		1,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1, R2.Reason_L2)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_Chi
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_Chi, R2.Reason_L2_Chi)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_CN
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_CN, R2.Reason_L2_CN)
		END
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'ReasonforVisit_S2_L1'
			INNER JOIN ReasonForVisitL1 R1 WITH (NOLOCK)
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S2_L2'
			LEFT JOIN ReasonForVisitL2 R2 WITH (NOLOCK)
				ON T.Service_Type = R2.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R2.Reason_L1_Code
					AND TAF2.AdditionalFieldValueCode = R2.Reason_L2_Code


	-- Item_Group_Seq 85: Reason_for_Visit_S3

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		85,
		1,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1, R2.Reason_L2)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_Chi
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_Chi, R2.Reason_L2_Chi)
		END,
		CASE WHEN TAF2.AdditionalFieldValueCode IS NULL
			THEN R1.Reason_L1_CN
			ELSE FORMATMESSAGE('%s-%s', R1.Reason_L1_CN, R2.Reason_L2_CN)
		END
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'ReasonforVisit_S3_L1'
			INNER JOIN ReasonForVisitL1 R1 WITH (NOLOCK)
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S3_L2'
			LEFT JOIN ReasonForVisitL2 R2 WITH (NOLOCK)
				ON T.Service_Type = R2.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R2.Reason_L1_Code
					AND TAF2.AdditionalFieldValueCode = R2.Reason_L2_Code

	-- Item_Group_Seq 91: High Risk

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		91,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'RecipientCondition'),
			SD.[Data_Value]
		),
		FORMATMESSAGE('%s: %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'RecipientCondition'),
			SD.[Data_Value_Chi]
		),
		FORMATMESSAGE('%s: %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'RecipientCondition'),
			SD.[Data_Value_CN]
		)
	FROM
		@TempTransaction T
			INNER JOIN (Select 
							[High_Risk] = CASE 
								WHEN Item_No = 'HIGHRISK' THEN 'Y' 
								WHEN Item_No = 'NOHIGHRISK' THEN 'N' 
								ELSE 'NoReference' END
							, * From StaticData WITH (NOLOCK) WHERE [Column_Name] = 'VSS_RECIPIENTCONDITION') SD
				ON T.High_Risk = SD.High_Risk
	WHERE
		T.High_Risk IS NOT NULL

	-- Item_Group_Seq 92: School Code

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		92,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT [Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SchoolCode'),
			TAF.AdditionalFieldValueCode
		),
		FORMATMESSAGE('%s: %s',
			(SELECT [Chinese_Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SchoolCode'),
			TAF.AdditionalFieldValueCode
		),
		FORMATMESSAGE('%s: %s',
			(SELECT [CN_Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SchoolCode'),
			TAF.AdditionalFieldValueCode
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'SchoolCode'
			INNER JOIN (Select * From School WITH (NOLOCK)) SCH
				ON TAF.AdditionalFieldValueCode = SCH.School_Code

	-- Item_Group_Seq 93: School Name

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		93,
		1,
		FORMATMESSAGE('%s: %s',
			(SELECT [Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SchoolName'),
			SCH.Name_Eng
		),
		FORMATMESSAGE('%s: %s',
			(SELECT [Chinese_Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SchoolName'),
			SCH.Name_Chi
		),
		FORMATMESSAGE('%s: %s',
			(SELECT [CN_Description] FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SchoolName'),
			''
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'SchoolCode'
			INNER JOIN (Select * From School WITH (NOLOCK)) SCH
				ON TAF.AdditionalFieldValueCode = SCH.School_Code

	-- Item_Group_Seq 101: RegistrationFeeRMB (SSSCMC) (with formatting the string to 1,234,567)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		101,
		1,
		FORMATMESSAGE('%s: ¥%s %s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SSSCMC_RegistrationFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END,
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = IIF(TD.Subsidize_Code = 'HAS_A','SSSCMC_PatientPaid','SSSCMC_PatientFree')) 
		),
		FORMATMESSAGE('%s: ¥%s %s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SSSCMC_RegistrationFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END,
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = IIF(TD.Subsidize_Code = 'HAS_A','SSSCMC_PatientPaid','SSSCMC_PatientFree'))
		),
		FORMATMESSAGE('%s: ¥%s %s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SSSCMC_RegistrationFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END,
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = IIF(TD.Subsidize_Code = 'HAS_A','SSSCMC_PatientPaid','SSSCMC_PatientFree'))
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'RegistrationFeeRMB'
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON T.Transaction_ID = TD.Transaction_ID
	WHERE
		T.Scheme_Code = 'SSSCMC'

	-- Item_Group_Seq 102: CoPaymentFeeRMB (SSSCMC) (with formatting the string to 1,234,567)

	INSERT INTO @OtherInfo (Transaction_ID, Item_Group_Seq, Display_Seq, Content_EN, Content_TC, Content_SC)
	SELECT
		T.Transaction_ID,
		102,
		1,
		FORMATMESSAGE('%s: ¥%s',
			(SELECT Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SSSCMC_ExtraCoPaymentFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END
		),
		FORMATMESSAGE('%s: ¥%s',
			(SELECT Chinese_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SSSCMC_ExtraCoPaymentFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END
		),
		FORMATMESSAGE('%s: ¥%s',
			(SELECT CN_Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName = 'SSSCMC_ExtraCoPaymentFee'), 
			CASE WHEN AdditionalFieldValueCode = '0' THEN '0' ELSE FORMAT(CONVERT(decimal(9, 2), AdditionalFieldValueCode), 'N2') END
		)
	FROM
		@TempTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CoPaymentFeeRMB'
	WHERE
		T.Scheme_Code = 'SSSCMC'

-- =============================================
-- Return results
-- =============================================
	EXEC [proc_SymmetricKey_open]

	SELECT
		T.Transaction_ID,
		T.Transaction_Dtm,
		T.Total_Claim_Amount,
		T.Total_Claim_Amount_RMB,
		CONVERT(varchar, DecryptByKey(T.Encrypt_Field1)) AS [IDNo], 
		CONVERT(varchar(100), DecryptByKey(T.Encrypt_Field2)) AS [Eng_Name], 
		CONVERT(nvarchar, DecryptByKey(T.Encrypt_Field3)) AS [Chi_Name], 
		CONVERT(varchar, DecryptByKey(T.Encrypt_Field11)) AS [IDNo2], 
		T.DataEntry_By,
		T.AccountType,
		T.Bank_Account_No,
		T.Practice_Name,
		T.Practice_Name_Chi,
		T.Record_Status,
		T.Scheme_Code,
		T.Display_Code,
		T.Doc_Code,
		DT.Doc_Display_Code,
		T.Invalidation, 
		T.Manual_Reimburse, 
		T.IsUpload,
		LEFT(DT.doc_display_code + Space(20), 20)  + convert(varchar, DecryptByKey(T.Encrypt_Field1)) as DocCode_IdentityNum,
		T.Scheme_Code + ' ' + T.Transaction_ID	as SchemeCode_TransactionID,		-- CRE11-024-02
		T.Subsidize_Code, -- Use for determine GP or Waiver 
		T.ConsultAndRegFeeRMB
	FROM
		@TempTransaction T
			INNER JOIN DocType DT WITH (NOLOCK)
				ON T.Doc_Code = DT.Doc_Code
				
	ORDER BY
		T.Transaction_Dtm
	
	EXEC [proc_SymmetricKey_close]

	--

	SELECT
		Transaction_ID,
		Item_Group_Seq,
		Display_Seq,
		Content_EN,
		Content_TC,
		Content_SC
	FROM
		@OtherInfo

	--

	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		DECLARE @Parameter varchar(255)
		SET @Parameter = ISNULL(@In_TransactionID, '') + ',' + ISNULL(CONVERT(varchar, @In_TranDtmFrom, 120), '') + ',' + ISNULL(CONVERT(varchar, @In_TranDtmTo, 120), '') 
						 + ',' + ISNULL(@In_SPID, '') + ',' + ISNULL(@In_DataEntry, '') + ',' + ISNULL(CONVERT(varchar, @In_Practice_Seq), '')
						 + ',' + ISNULL(@In_Record_Status, '') + ',' + ISNULL(@In_Scheme_Code, '') 
		 
		EXEC proc_SProcPerformance_add 'proc_VoucherTransaction_get_BySPIDBankAcctTransID',
									   @Parameter,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END
	
	
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_BySPIDBankAcctTransID] TO HCSP, WSEXT
GO

