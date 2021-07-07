IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionConfirm_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionConfirm_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
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
-- CR No.:			CRE20-015 (HA Scheme)
-- Modified by:		Winnie SUEN
-- Modified date:	16 Oct 2020
-- Description:		Show [Total_Claim_Amount_RMB] for SSSCMC scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	26 August 2018
-- CR No.:			CRE17-018
-- Description:		Display "Place of Vaccination" as additional field
-- =============================================
-- =============================================  
-- Modification History  
-- CR No:			CRE16-026 (Add PCV13)
-- Modified by:		Chris YIM
-- Modified date:	22 Sep 2017
-- Description:		Add item to Other Info : High Risk
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			INT16-0032 Long term fix for HCSP Record Confirmation (empty Other Info))
-- Modified by:		Winnie SUEN
-- Modified date:	3 Jan 2017
-- Description:		Long term fix for empty Other Info
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			INT16-0015 (Fix Record Confirmation for Incomplete HCVS)
-- Modified by:		Lawrence TSANG
-- Modified date:	28 October 2016
-- Description:		Fast fix for Other Info
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
-- =============================================  
-- Modification History  
-- CR No:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	17 Feb 2015
-- Description:		1. Handle currency with RMB
--					2. Add Input Parameter "Available_HCSP_SubPlatform"
-- =============================================  

CREATE Procedure [dbo].[proc_VoucherTransactionConfirm_get]  
 @SP_ID      char(8),  
 @Practice_Display_Seq  smallint,  
 @DataEntry_By    varchar(20),  
 @Transaction_Dtm   datetime,  
 @SchemeCode     char(10),
 @Transaction_ID	char(20),
 @IncludeIncompleteClaim char(1)='N',
 @Available_HCSP_SubPlatform	char(2)
AS BEGIN

-- =============================================  
-- Declaration  
-- =============================================  
DECLARE @Performance_Start_Dtm datetime
SET @Performance_Start_Dtm = GETDATE()

DECLARE @In_SP_ID					char(8)
DECLARE @In_Practice_Display_Seq	smallint
DECLARE @In_DataEntry_By			varchar(20)
DECLARE @In_Transaction_Dtm			datetime
DECLARE @In_SchemeCode				char(10)
DECLARE @In_Transaction_ID			char(20)
DECLARE @In_IncludeIncompleteClaim	char(1)
DECLARE @In_Available_HCSP_SubPlatform	char(2)
SET @In_SP_ID = @SP_ID
SET @In_Practice_Display_Seq = @Practice_Display_Seq
SET @In_DataEntry_By = @DataEntry_By
SET @In_Transaction_Dtm = @Transaction_Dtm
SET @In_SchemeCode = @SchemeCode
SET @In_Transaction_ID = @Transaction_ID
SET @In_IncludeIncompleteClaim = @IncludeIncompleteClaim
SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

create table #VoucherAccTransaction (  
  Transaction_ID  char(20) collate database_default,  
  Transaction_Dtm datetime,  
  Voucher_Acc_ID char(15),  
  Temp_Voucher_Acc_ID char(15),  
  Encrypt_Field1 varbinary(100),  
  Encrypt_Field2 varbinary(200),  
  Encrypt_Field3 varbinary(100),  
  total_unit  smallint,  
  Claim_Amount money,  
  Claim_Amount_RMB money,  
  DataEntry_By  varchar(20),  
  Practice_Name  nvarchar(100),  
  Practice_Name_Chi  nvarchar(100),  
  Bank_Account_No varchar(30),  
  Record_Status char(1),  
  TSMP binary(8),  
  Voucher_Acc_TSMP binary(8),  
  EC_Serial_No varchar(10),  
  EC_Reference_No varchar(40),  
  EC_Age smallint,  
  EC_Date_of_Registration datetime,  
  Scheme_Code char(25) collate database_default,  
  --Doc_Type char(25),  
  Doc_Code char(20),  
  DOB    datetime,  
  Exact_DOB  char(1),  
  Date_Of_Issue datetime,  
  Encrypt_Field11 varbinary(100),  
  Service_Type char(5),  
  original_amend_acc_id char(15),  
  original_TSMP binary(8),  
  Validated_Acc_ID char(15),  
  original_DOI datetime,  
  original_DOB datetime,  
  original_Exact_DOB char(1),  
  Send_To_ImmD char(1),   
  IsUpload varchar(1),
  Category_Code varchar(10),
  High_Risk char(1)
)  
  
CREATE INDEX IX_VAT on #VoucherAccTransaction (Transaction_ID)  
  
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
-- Initization  
-- =============================================  
  
  
select @In_Transaction_Dtm = dateadd(day, 1, @In_Transaction_Dtm)  
  
  
declare @IsUpload as char(1)

SET @IsUpload = 'N'
IF @In_DataEntry_By = 'Upload'  BEGIN
	SET @IsUpload = 'Y'
	Set @In_DataEntry_By = null
END

  
-- scenario A + C  
  
insert into #VoucherAccTransaction  
(  
  Transaction_ID,  
  Transaction_Dtm,  
  Voucher_Acc_ID,  
  Temp_Voucher_Acc_ID,  
  Encrypt_Field1,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  Claim_Amount,
  Claim_Amount_RMB,
  DataEntry_By,  
  Practice_Name,  
  Practice_Name_Chi,  
  Bank_Account_No,  
  Record_Status,  
  TSMP,  
  Voucher_Acc_TSMP,  
  EC_Serial_No,  
  EC_Reference_No,  
  EC_Age,  
  EC_Date_of_Registration,  
  Scheme_Code,  
  --Doc_Type,  
  Doc_Code,  
  DOB,  
  Exact_DOB,  
  Date_Of_Issue,  
  Encrypt_Field11,  
  Service_Type,   
  IsUpload,
  Category_Code,
  High_Risk
)  
  
select v.Transaction_ID,  
  v.Transaction_Dtm,  
  v.Voucher_Acc_ID,  
  '',  
  p.Encrypt_Field1,  
  p.Encrypt_Field2,  
  p.Encrypt_Field3,  
  v.Claim_Amount,
  td.Total_Amount_RMB,  
  v.DataEntry_By,  
  pr.Practice_Name,  
  pr.Practice_Name_Chi,  
  v.Bank_Account_No,  
  v.Record_Status,  
  v.tsmp,  
  null,  
  p.EC_Serial_No,  
  p.EC_Reference_No,  
  p.EC_Age,  
  p.EC_Date_of_Registration,  
  v.scheme_code,  
  --dt.doc_display_code,  
  p.doc_code,  
  p.DOB,  
  p.Exact_DOB,  
  p.Date_Of_Issue,  
  p.Encrypt_Field11,  
  v.Service_Type,   
  v.IsUpload,
  v.Category_Code,
  v.High_Risk
from VoucherTransaction v
	LEFT JOIN TransactionDetail TD  
		ON v.Transaction_ID = TD.Transaction_ID AND TD.Total_Amount_RMB IS NOT NULL
	LEFT JOIN SchemeClaim SC
		ON v.Scheme_Code = SC.Scheme_Code,
  PersonalInformation p,  
  Practice pr--, doctype dt  
where v.SP_ID = @In_SP_ID  
and (@In_Practice_Display_Seq is null or v.Practice_Display_Seq = @In_Practice_Display_Seq)  
and (@In_DataEntry_By is null or v.DataEntry_By = @In_DataEntry_By)  
and ((@IsUpload = 'N' or @IsUpload is null) or v.IsUpload  = @IsUpload)
and (@In_SchemeCode is null or v.scheme_code = @In_SchemeCode)
and v.SP_ID = pr.SP_ID  
and v.Voucher_Acc_ID = p.Voucher_Acc_ID  
and v.doc_code = p.doc_code  
and v.Practice_Display_Seq = pr.Display_Seq  
and v.Transaction_Dtm < @In_Transaction_Dtm  
--and v.Record_Status = 'P'								-- CRE11-024-02
and ((@In_IncludeIncompleteClaim='N' and v.Record_Status='P') or
	(@In_IncludeIncompleteClaim='Y' and (v.Record_Status='P' or v.Record_Status='U')))		-- CRE11-024-02
and (@In_Transaction_ID IS NULL OR v.Transaction_ID = @In_Transaction_ID)
--and v.doc_code = dt.doc_code  
and (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
  
-- scenario B  
  
insert into #VoucherAccTransaction  
(  
  Transaction_ID,  
  Transaction_Dtm,  
  Voucher_Acc_ID,  
  Temp_Voucher_Acc_ID,  
  Encrypt_Field1,  
  Encrypt_Field2,  
  Encrypt_Field3,  
  Claim_Amount,
  Claim_Amount_RMB,  
  DataEntry_By,  
  Practice_Name,  
  Practice_Name_Chi,  
  Bank_Account_No,  
  Record_Status,  
  TSMP,  
  Voucher_Acc_TSMP,  
  EC_Serial_No,  
  EC_Reference_No,  
  EC_Age,  
  EC_Date_of_Registration,  
  Scheme_Code,  
  --Doc_Type,  
  Doc_Code,  
  DOB,  
  Exact_DOB,  
  Date_Of_Issue,  
  Encrypt_Field11,  
  Service_Type,  
  original_amend_acc_id,  
  Validated_Acc_ID,   
  IsUpload,
  Category_Code,
  High_Risk
)  
select v.Transaction_ID,  
  v.Transaction_Dtm,  
  '',  
  v.Temp_Voucher_Acc_ID,  
  p.Encrypt_Field1,  
  p.Encrypt_Field2,  
  p.Encrypt_Field3,  
  v.Claim_Amount,
  td.Total_Amount_RMB,  
  v.DataEntry_By,  
  pr.Practice_Name,  
  pr.Practice_Name_Chi,  
  v.Bank_Account_No,  
  v.Record_Status,  
  v.tsmp,  
  t.tsmp,  
  p.EC_Serial_No,  
  p.EC_Reference_No,  
  p.EC_Age,  
  p.EC_Date_of_Registration,  
  v.scheme_code,  
  --dt.doc_display_code,  
  p.doc_code,  
  p.DOB,  
  p.Exact_DOB,  
  p.Date_Of_Issue,  
  p.Encrypt_Field11,  
  v.Service_Type,  
  t.original_amend_acc_id,  
  t.Validated_Acc_ID,   
  v.IsUpload,
  v.Category_Code,
  v.High_Risk
from VoucherTransaction v
	LEFT JOIN TransactionDetail TD  
		ON v.Transaction_ID = TD.Transaction_ID  AND TD.Total_Amount_RMB IS NOT NULL
	LEFT JOIN SchemeClaim SC
		ON v.Scheme_Code = SC.Scheme_Code,
  TempPersonalInformation p,  
  Practice pr,  
  TempVoucherAccount t--,doctype dt  
where v.SP_ID = @In_SP_ID  
and (@In_Practice_Display_Seq is null or v.Practice_Display_Seq = @In_Practice_Display_Seq)  
and (@In_DataEntry_By is null or v.DataEntry_By = @In_DataEntry_By)  
and ((@IsUpload = 'N' or @IsUpload is null) or v.IsUpload  = @IsUpload)
and (@In_SchemeCode is null or v.scheme_code = @In_SchemeCode)  -- CRE11-024-02
and v.SP_ID = pr.SP_ID  
and v.Temp_Voucher_Acc_ID = t.Voucher_Acc_ID  
and v.Temp_Voucher_Acc_ID = p.Voucher_Acc_ID  
and v.Practice_Display_Seq = pr.Display_Seq  
and v.Transaction_Dtm < @In_Transaction_Dtm  
and v.Voucher_Acc_ID = ''  
--and v.Record_Status = 'P'					-- CRE11-024-02
and ((@In_IncludeIncompleteClaim='N' and v.Record_Status='P') or
	(@In_IncludeIncompleteClaim='Y' and (v.Record_Status='P' or v.Record_Status='U')))		-- CRE11-024-02
and (@In_Transaction_ID IS NULL OR v.Transaction_ID = @In_Transaction_ID)
--and v.doc_code = dt.doc_code  
and (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
  

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
		#VoucherAccTransaction T
			INNER JOIN TransactionAdditionalField TAF
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
	-- Item_Group_Seq 61: CoPaymentFee (HCVS) (with formatting the string to 1,234,567)
	-- Item_Group_Seq 71: CoPaymentFeeRMB (HCVSCHN) (with formatting the string to 1,234,567)
	-- Item_Group_Seq 72: PaymentType (HCVSCHN)
	-- Item_Group_Seq 81: Reason_for_Visit (Header)
	-- Item_Group_Seq 82: Reason_for_Visit
	-- Item_Group_Seq 83: Reason_for_Visit_S1
	-- Item_Group_Seq 84: Reason_for_Visit_S2
	-- Item_Group_Seq 85: Reason_for_Visit_S3
	-- Item_Group_Seq 91: High Risk
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
		#VoucherAccTransaction T
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
			INNER JOIN SubsidizeItem SI
				ON TD.Subsidize_Item_Code = SI.Subsidize_Item_Code
			INNER JOIN SubsidizeGroupClaim SGC
				ON TD.Scheme_Code = SGC.Scheme_Code
					AND TD.Scheme_Seq = SGC.Scheme_Seq
					AND TD.Subsidize_Code = SGC.Subsidize_Code
			INNER JOIN SubsidizeItemDetails SID
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
		#VoucherAccTransaction T
			INNER JOIN ClaimCategory CC
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND T.Category_Code = 'VSSPID'
					AND TAF.AdditionalFieldID = 'DocumentaryProof'
			INNER JOIN StaticData SD
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND T.Category_Code = 'VSSDA'
					AND TAF.AdditionalFieldID = 'DocumentaryProof'
			INNER JOIN StaticData SD
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND TAF.AdditionalFieldID = 'PlaceVaccination'
					AND TAF.AdditionalFieldValueCode <> 'OTHERS'
			INNER JOIN StaticData SD
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'VSS'
					AND TAF.AdditionalFieldID = 'PlaceVaccination'
					AND TAF.AdditionalFieldValueCode = 'OTHERS'
			INNER JOIN StaticData SD
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
		#VoucherAccTransaction T
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND T.Scheme_Code = 'PIDVSS'
					AND TAF.AdditionalFieldID = 'DocumentaryProof'
			INNER JOIN StaticData SD
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'CoPayment'
			INNER JOIN StaticData SD
				ON TAF1.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'EHAPP_COPAYMENT'
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'HCVAmount'
			LEFT JOIN @TempTransactionAdditionalField TAF3
				ON T.Transaction_ID = TAF3.Transaction_ID
					AND TAF3.AdditionalFieldID = 'NetServiceFee'


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
		#VoucherAccTransaction T
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
		#VoucherAccTransaction T
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'PaymentType'
			INNER JOIN StaticData SD
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
		#VoucherAccTransaction T
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			INNER JOIN ReasonForVisitL1 R1
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'Reason_for_Visit_L2'
			LEFT JOIN ReasonForVisitL2 R2
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'ReasonforVisit_S1_L1'
			INNER JOIN ReasonForVisitL1 R1
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S1_L2'
			LEFT JOIN ReasonForVisitL2 R2
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'ReasonforVisit_S2_L1'
			INNER JOIN ReasonForVisitL1 R1
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S2_L2'
			LEFT JOIN ReasonForVisitL2 R2
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF1
				ON T.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'ReasonforVisit_S3_L1'
			INNER JOIN ReasonForVisitL1 R1
				ON T.Service_Type = R1.Professional_Code
					AND TAF1.AdditionalFieldValueCode = R1.Reason_L1_Code
			LEFT JOIN @TempTransactionAdditionalField TAF2
				ON T.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S3_L2'
			LEFT JOIN ReasonForVisitL2 R2
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
		#VoucherAccTransaction T
			INNER JOIN (Select 
							[High_Risk] = CASE 
								WHEN Item_No = 'HIGHRISK' THEN 'Y' 
								WHEN Item_No = 'NOHIGHRISK' THEN 'N' 
								ELSE 'NoReference' END
							, * From StaticData WHERE [Column_Name] = 'VSS_RECIPIENTCONDITION') SD
				ON T.High_Risk = SD.High_Risk
	WHERE
		T.High_Risk IS NOT NULL


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
		#VoucherAccTransaction T
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
		#VoucherAccTransaction T
			INNER JOIN @TempTransactionAdditionalField TAF
				ON T.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CoPaymentFeeRMB'
	WHERE
		T.Scheme_Code = 'SSSCMC'
  
update #VoucherAccTransaction  
set total_unit = temp.total_unit  
from #VoucherAccTransaction tt,   
(select t.transaction_id, sum(td.unit) as total_unit from TransactionDetail td, #VoucherAccTransaction t  
where t.transaction_id = td.transaction_id collate database_default  
group by t.transaction_id) as temp  
where temp.transaction_id = tt.transaction_id collate database_default
  
  
update #VoucherAccTransaction  
set original_TSMP = t.TSMP,  
 original_DOI = p.date_of_issue,  
 original_DOB = p.DOB,  
 original_Exact_DOB = p.exact_dob  
from #VoucherAccTransaction vt, tempvoucheraccount t, temppersonalinformation p  
where vt.original_amend_acc_id = t.voucher_acc_id collate database_default  
and t.voucher_acc_id = p.voucher_acc_id  
and vt.original_amend_acc_id is not null  
and t.account_purpose = 'O'  
  
update #VoucherAccTransaction  
set Send_To_ImmD = 'X'  
where original_amend_acc_id is null  
  
update #VoucherAccTransaction  
set Send_To_ImmD = 'N'  
where Date_Of_Issue = Original_DOI  
and DOB = Original_DOB  
and Exact_DOB = original_Exact_DOB  
and Original_DOI is not null  
and Original_DOB is not null  
and original_Exact_DOB is not null  
and original_amend_acc_id is not null  
  
update #VoucherAccTransaction  
set Send_To_ImmD = 'Y'  
where Send_To_ImmD is null  
  
  
-- =============================================  
-- Return results  
-- =============================================  
EXEC [proc_SymmetricKey_open]
  
select v.Transaction_ID,  
  v.Transaction_Dtm,  
  v.Voucher_Acc_ID,  
  v.Temp_Voucher_Acc_ID,  
  convert(varchar, DecryptByKey(v.Encrypt_Field1)) as IdentityNum,  
  convert(varchar(100), DecryptByKey(v.Encrypt_Field2)) as Eng_Name,  
  convert(nvarchar, DecryptByKey(v.Encrypt_Field3)) as Chi_Name,  
  --v.IdentityNum,  
  --v.Eng_Name,  
  --v.Chi_Name,  
  v.Total_Unit,  
  v.Claim_Amount Total_Amount,  
  v.Claim_Amount_RMB Total_Amount_RMB,  
  v.DataEntry_By,  
  v.Practice_Name,  
  v.Practice_Name_Chi,  
  v.Bank_Account_No,  
  v.Record_Status,  
  v.TSMP,  
  v.Voucher_Acc_TSMP,  
  v.EC_Serial_No,  
  v.EC_Reference_No,  
  v.EC_Age,  
  v.EC_Date_of_Registration,  
  SC.Display_Code AS [Scheme],
  dt.doc_display_code as [Doc_Type],  
  v.Doc_Code,  
  convert(char(7), DecryptByKey(v.Encrypt_Field11)) as Adoption_Prefix_Num,  
  v.original_amend_acc_id,  
  v.original_TSMP,  
  v.Validated_Acc_ID,  
  v.Send_To_ImmD,   
  v.IsUpload, 
  LEFT(dt.doc_display_code + Space(20), 20)  + convert(varchar, DecryptByKey(v.Encrypt_Field1)) as DocCode_IdentityNum,
  SC.Display_Code + ' ' + v.Transaction_ID	as SchemeCode_TransactionID		-- CRE11-024-02
	FROM
		#VoucherAccTransaction v
			INNER JOIN DocType dt
				ON v.doc_code = dt.doc_code
			INNER JOIN SchemeClaim SC
				ON V.Scheme_Code = SC.Scheme_Code
order by Transaction_Dtm asc  

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
	SET @Parameter = ISNULL(@In_SP_ID, '') + ',' + ISNULL(CONVERT(varchar, @In_Practice_Display_Seq), '') + ',' + ISNULL(@In_DataEntry_By, '')
					 + ',' + ISNULL(CONVERT(varchar, @In_Transaction_Dtm, 120), '') + ',' + ISNULL(@In_SchemeCode, '') + ',' + ISNULL(@In_Transaction_ID, '')
					 + ',' + ISNULL(@In_IncludeIncompleteClaim, '')
	
	EXEC proc_SProcPerformance_add 'proc_VoucherTransactionConfirm_get',
								   @Parameter,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
END


END
GO  

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionConfirm_get] TO HCSP, HCVU, WSEXT
GO

