IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_vaccine_get_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_vaccine_get_byDocCodeDocID]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Chris YIM
-- Modified date:	24 Aug 2021
-- Description:		Add column [Join_EHRSS], [Contact_No] & [Non_Local_Recovered]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	24 May 2021
-- CR No.:			CRE20-023 (Immu record)
-- Description:		Add column [Clinic_Type]
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
-- Modified by:		Chris YIM
-- Modified date:	05 Feb 2021
-- CR No.:			CRE20-022 (Immu record)
-- Description:		Add column [Vaccine_Brand] & [Vaccine_Lot_No]
--					Add share of benefit under doc. HKIC, EC, HKBC, CCIC, ROP140
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	05 Jan 2021
-- CR No.:			CRE20-XXX (Immu record)
-- Description:	  	Add Column - [VoucherTransaction].[High_Risk]
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
-- Modified by:	    Chris YIM
-- Modified date:	08 Oct 2020
-- CR No.:			
-- Description:	  	Tune
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	07 June 2017
-- CR No.:			CRE16-026-03 (Add PCV13)
-- Description:	  	Add Column - [VoucherTransaction].[High_Risk]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE14-007
-- Modified by:		Lawrence TSANG
-- Modified date:	5 August 2014
-- Description:		Fix VRE display for same doc no. in HKIC and EC
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0026
-- Modified by:		Koala CHENG
-- Modified date:	3 Dec 2013
-- Description:		Fix incorrect recipient demographics retrieval, specify varchar(40) when decrypt
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-001
-- Modified by:		Karl LAM
-- Modified date:	9 Apr 2013
-- Description:		Match the column definition of the result temp table variable @result ([Source],[Available_item_Desc],[Available_item_Desc_Chi]) to real table
--					Grant WSEXT right
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Twinsen Chan
-- Modified date:	4 Jan 2013
-- Description:	    1. Remove SchemeClaim.Scheme_Seq
--					2. Inner join to SubsidizeItem to get the type 'VACCINE'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Koala Cheng
-- Modified date:	27 Oct 2010
-- Description:	    Fix duplicate transaction returned if contain 2 validateted account with same ID (HKIC,HKBC)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:	30 July 2010
-- Description:	    Get the account's demographic
-- =============================================
-- =============================================
-- Author:			Koala Cheng
-- Create date:		30 Jun 2010
-- Description:		Get the TransactionDetail (vaccination only) By 
--					DocCode, Identity For SP/VU Transaction Detail Display
-- =============================================
CREATE PROCEDURE [dbo].[proc_TransactionDetail_vaccine_get_byDocCodeDocID] 
	@Doc_Code			CHAR(20),
	@identity			VARCHAR(20)
WITH RECOMPILE
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

DECLARE @blnOtherDoc_Code tinyint
DECLARE @blnWithVoucherAcct tinyint

DECLARE @OtherDoc_Code table (
	Doc_Code	CHAR(20)
)

DECLARE @tmpVoucherAcct Table
(
	Voucher_Acc_ID	CHAR(15),
	Encrypt_Field2	VARBINARY(200),
	DOB				DATETIME,
	Exact_DOB		CHAR(1),
	Sex				CHAR(1),
	Doc_Code		CHAR(20)
)

DECLARE @tmpTempVoucherAcct Table
(
	Voucher_Acc_ID char(15),
	Encrypt_Field2 varbinary(200),
	DOB datetime,
	Exact_DOB char(1),
	Sex char(1)
)

DECLARE @tmpSpecialAcct Table
(
	Voucher_Acc_ID char(15),
	Encrypt_Field2 varbinary(200),
	DOB datetime,
	Exact_DOB char(1),
	Sex char(1)
)

DECLARE @tmpInvalidAcct Table
(
	Voucher_Acc_ID char(15),
	Encrypt_Field2 varbinary(200),
	DOB datetime,
	Exact_DOB char(1),
	Sex char(1)
)

DECLARE @tmpVoucherTransaction Table
(
	Transaction_ID char(20),
	Encrypt_Field2 varbinary(200),
	DOB datetime,
	Exact_DOB char(1),
	Sex char(1)
)


declare @result table
(
	[Service_Receive_Dtm] datetime,
	[Scheme_Code] char(10),
	[Display_Code] char(25),
	[Scheme_Seq] smallint,
	[Subsidize_Code] char(10),
	[Subsidize_Item_Code] char(10),
	[Available_item_Code]	char(20),
	[Available_item_Desc]	varchar(100),
	[Unit] smallint,
	[Source] char(25),
	[Per_Unit_Value] money,
	[Total_Amount] money,
	[Subsidize_Desc] varchar(100),
	[Subsidize_Desc_Chi] nvarchar(100),
	[Available_item_Desc_Chi]	nvarchar(100),
	[Available_item_Desc_CN]	nvarchar(100),
	[Transaction_ID] char(20),
	[SP_ID] char(8),
	[Practice_Display_Seq] smallint,
	[Practice_Name] nvarchar(100),
	[Practice_Name_Chi] nvarchar(100),
	[Transaction_Dtm] datetime,
	[Ext_Ref_Status] Varchar(10),
	[Encrypt_Field2] varbinary(200),
	[DOB] datetime,
	[Exact_DOB] char(1),
	[Sex] char(1),
	[High_Risk] char(1),
	[Vaccine_Brand] VARCHAR(50),
	[Vaccine_Lot_No] VARCHAR(50)
)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	
SET @blnOtherDoc_Code = 0 
SET @blnWithVoucherAcct = 0 

IF LTRIM(RTRIM(@Doc_Code)) = 'HKIC' 
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKBC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('EC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('CCIC')               
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('ROP140')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'HKBC'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKIC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('EC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('CCIC')               
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('ROP140')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'EC'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKIC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKBC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('CCIC')               
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('ROP140')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'CCIC'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKIC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKBC')   
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('EC')           
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('ROP140')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'ROP140'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKIC')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKBC')   
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('EC')           
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('CCIC')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'VISA'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('PASS')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'HKP'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('PASS') 
END

IF LTRIM(RTRIM(@Doc_Code)) = 'PASS'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKP')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('VISA')
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('OC')
END

IF LTRIM(RTRIM(@Doc_Code)) = 'OC'
BEGIN
	Set @blnOtherDoc_Code = 1
	INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('PASS')
END

-- =============================================
-- Return results
-- =============================================


EXEC [proc_SymmetricKey_open]
	
-- Retrieve VoucherAccount By Identity in different PersonalInformation Tables

	INSERT INTO @tmpVoucherAcct
	SELECT	[Voucher_Acc_ID],
			[Encrypt_Field2],
			[DOB],
			[Exact_DOB],
			[Sex],
			[Doc_Code]
	FROM	[PersonalInformation]
	WHERE	Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
			(
				[Doc_Code] = @Doc_Code
			)

	IF (SELECT COUNT(1) FROM @tmpVoucherAcct)=0
	BEGIN
		INSERT INTO @tmpVoucherAcct
		SELECT	[Voucher_Acc_ID],
				[Encrypt_Field2],
				[DOB],
				[Exact_DOB],
				[Sex],
				[Doc_Code]
		FROM	[PersonalInformation]
		WHERE	Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
				(
					@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code)
				)
	END
	ELSE
		SET @blnWithVoucherAcct = 1
	

	--SELECT * FROM @tmpVoucherAcct

	INSERT INTO @tmpTempVoucherAcct	
	SELECT	TPI.[Voucher_Acc_ID],
			TPI.[Encrypt_Field2],
			TPI.[DOB],
			TPI.[Exact_DOB],
			TPI.[Sex]
	FROM	[TempPersonalInformation] TPI
		INNER JOIN [TempVoucherAccount] TVA
			ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]
	WHERE	
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
			(
				[Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code))
			) 
			AND TVA.[Record_Status] <> 'D'
			
	INSERT INTO @tmpSpecialAcct	
	SELECT	SPI.[Special_Acc_ID],
			SPI.[Encrypt_Field2],
			SPI.[DOB],
			SPI.[Exact_DOB],
			SPI.[Sex]
	FROM	[SpecialPersonalInformation] SPI
		INNER JOIN [SpecialAccount] SVA
			ON SPI.[Special_Acc_ID] = SVA.[Special_Acc_ID]		
	WHERE	
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
			(
				[Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code))
			)
			AND SVA.[Record_Status] <> 'D'
			
	INSERT INTO @tmpInvalidAcct
	SELECT	IPI.[Invalid_Acc_ID],
			IPI.[Encrypt_Field2],
			IPI.[DOB],
			IPI.[Exact_DOB],
			IPI.[Sex]
	FROM	[InvalidPersonalInformation] IPI 
		INNER JOIN [InvalidAccount] IV
			ON IPI.[Invalid_Acc_ID] = IV.[Invalid_Acc_ID]
	WHERE	
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
			(
				[Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code))
			)
			AND IV.[Count_Benefit] = 'Y'
			
EXEC [proc_SymmetricKey_close]

-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)

	INSERT INTO @tmpVoucherTransaction	
	SELECT
		VT.[Transaction_ID],
		tmp.[Encrypt_Field2],
		tmp.[DOB],
		tmp.[Exact_DOB],
		tmp.[Sex]
	FROM
		@tmpVoucherAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH(NOLOCK)
				ON VT.[Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]
					AND VT.[Invalid_Acc_ID] IS NULL
					AND (@blnWithVoucherAcct = 1 OR VT.[Doc_Code]= tmp.[Doc_Code])
	WHERE
		--VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' 
		NOT EXISTS(SELECT 1 FROM [VoucherTransaction] WITH(NOLOCK) WHERE [Record_Status] IN ('I','D') AND VT.Transaction_ID = Transaction_ID)
		AND VT.[Invalid_acc_id] is null
	
	UNION		
	--INSERT INTO @tmpVoucherTransaction
	SELECT
		VT.[Transaction_ID],
		tmp.[Encrypt_Field2],
		tmp.[DOB],
		tmp.[Exact_DOB],
		tmp.[Sex]
	FROM
		@tmpTempVoucherAcct tmp INNER JOIN [VoucherTransaction] VT WITH(NOLOCK)
			ON VT.[Temp_Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]
				AND VT.[Voucher_Acc_ID] = ''
				AND VT.[Special_Acc_ID] IS NULL
	WHERE
		--VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' 
		NOT EXISTS(SELECT 1 FROM [VoucherTransaction] WITH(NOLOCK) WHERE [Record_Status] IN ('I','D') AND VT.Transaction_ID = Transaction_ID)
		AND VT.[Invalid_acc_id] is null
				
	UNION
	--INSERT INTO @tmpVoucherTransaction
	SELECT
		VT.[Transaction_ID],
		tmp.[Encrypt_Field2],
		tmp.[DOB],
		tmp.[Exact_DOB],
		tmp.[Sex]
	FROM
		@tmpSpecialAcct tmp INNER JOIN [VoucherTransaction] VT WITH(NOLOCK)
			ON VT.[Special_Acc_ID] = tmp.[Voucher_Acc_ID]
				AND VT.[Voucher_Acc_ID] = ''
				AND VT.[Invalid_Acc_ID] IS NULL
	WHERE
		--VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' 
		NOT EXISTS(SELECT 1 FROM [VoucherTransaction] WITH(NOLOCK) WHERE [Record_Status] IN ('I','D') AND VT.Transaction_ID = Transaction_ID)
		AND VT.[Invalid_acc_id] is null
	
	UNION
	--INSERT INTO @tmpVoucherTransaction
	SELECT
		VT.[Transaction_ID],
		tmp.[Encrypt_Field2],
		tmp.[DOB],
		tmp.[Exact_DOB],
		tmp.[Sex]
	FROM
		@tmpInvalidAcct tmp INNER JOIN [VoucherTransaction] VT WITH(NOLOCK)
			ON VT.[Invalid_Acc_ID] = tmp.[Voucher_Acc_ID]
	WHERE
		VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'D'   
		
	--SELECT * FROM @tmpVoucherTransaction
				
	INSERT INTO @result
	(
		[Service_Receive_Dtm],
		[Scheme_Code],
		[Scheme_Seq],
		[Subsidize_Code],
		[Subsidize_Item_Code],
		[Available_item_Code],
		[Unit],
		[Source],
		[Per_Unit_Value],
		[Total_Amount],
		[Transaction_ID],
		[SP_ID],
		[Practice_Display_Seq],
		[Practice_Name],
		[Practice_Name_Chi],
		[Transaction_Dtm],
		[Ext_Ref_Status],
		[Encrypt_Field2],
		[DOB],
		[Exact_DOB],
		[Sex],
		[High_Risk],
		[Vaccine_Brand],
		[Vaccine_Lot_No]
	)
	SELECT
		--Distinct
		VT.[Service_Receive_Dtm],
		TD.[Scheme_Code],
		TD.[Scheme_Seq],
		TD.[Subsidize_Code],
		TD.[Subsidize_Item_Code],
		TD.[Available_item_Code],
		TD.[Unit],
		TD.[Scheme_Code],
		TD.[Per_Unit_Value],
		TD.[Total_Amount],
		TD.[Transaction_ID],
		VT.[SP_ID],
		VT.[Practice_Display_Seq],
		P.[Practice_Name],
		P.[Practice_Name_Chi],
		VT.[Transaction_Dtm],
		VT.[Ext_Ref_Status],
		tmp.[Encrypt_Field2],
		tmp.[DOB],
		tmp.[Exact_DOB],
		tmp.[Sex],
		VT.[High_Risk],
		TAF_Brand.AdditionalFieldValueCode AS [Vaccine_Brand],
		TAF_LotNo.AdditionalFieldValueCode AS [Vaccine_Lot_No]
	FROM
		@tmpVoucherTransaction tmp 
			INNER JOIN [TransactionDetail] TD  WITH(NOLOCK)
				ON tmp.[Transaction_ID] = TD.[Transaction_ID]
			INNER JOIN [VoucherTransaction] VT WITH(NOLOCK)
				ON TD.[Transaction_ID] = VT.[Transaction_ID]	
			INNER JOIN [Practice] P
				ON VT.[SP_ID] = P.[SP_ID] AND VT.[Practice_Display_Seq] = P.[Display_Seq]	
			LEFT OUTER JOIN [TransactionAdditionalField] TAF_Brand
				ON VT.[Transaction_ID] = TAF_Brand.Transaction_ID AND TAF_Brand.AdditionalFieldID = 'VaccineBrand'
			LEFT OUTER JOIN [TransactionAdditionalField] TAF_LotNo
				ON VT.[Transaction_ID] = TAF_LotNo.Transaction_ID AND TAF_LotNo.AdditionalFieldID = 'VaccineLotNo'
				
	update @Result
	set [Display_Code] = SC.Display_Code
	from @Result R, SchemeClaim SC
	where R.Scheme_Code = SC.Scheme_Code
	
	update @Result
	set [Source] = SC.Display_Code
	from @Result R, SchemeClaim SC
	where R.[Source] = SC.Scheme_Code
	
	update @Result
	set [Available_item_Desc] = CASE WHEN sid.available_Item_Desc='Injection' THEN NA.[Description] ELSE sid.available_Item_Desc  END,
		[Available_item_Desc_Chi] = CASE WHEN sid.available_Item_Desc='Injection' THEN NA.[Chinese_Description] ELSE sid.available_Item_Desc_Chi END,
		[Available_item_Desc_CN] = CASE WHEN sid.available_Item_Desc='Injection' THEN NA.[CN_Description] ELSE sid.available_Item_Desc_CN END
	from @Result R, subsidizeitemdetails sid, 
		(SELECT [Description], Chinese_Description, CN_Description FROM systemresource WITH (NOLOCK) WHERE ObjectName = 'N/A') AS NA
	where R.[Subsidize_Item_Code] = sid.Subsidize_item_code
	and R.Available_item_Code = sid.Available_item_Code
	
	update @Result
	set [Subsidize_Desc] = s.[Vaccine_Code_Desc],
		[Subsidize_Desc_Chi] = s.[Vaccine_Code_Desc_Chi]
	from @Result R, (SELECT [Vaccine_Code_Source], [Vaccine_Brand_ID_Source], [Vaccine_Code_Desc], [Vaccine_Code_Desc_Chi] FROM VaccineCodeMapping WHERE Source_System='EHS') s
	where REPLACE(R.Scheme_Code + '|' + STR(R.Scheme_Seq) + '|' + R.Subsidize_Code,' ','') = s.[Vaccine_Code_Source] 
		AND (s.[Vaccine_Brand_ID_Source] = '' OR s.[Vaccine_Brand_ID_Source] = R.Vaccine_Brand)


EXEC [proc_SymmetricKey_open]

	SELECT
		CONVERT(VARCHAR(11), R.[Service_Receive_Dtm], 113) AS Service_Receive_Dtm,
		R.[Display_Code],
		R.[Scheme_Code],
		R.[Scheme_Seq],
		R.[Subsidize_Code],
		R.[Subsidize_Item_Code],
		R.[Available_item_Code] AS Available_item_Code,
		R.[Available_item_Desc] AS Available_Item_Desc,
		R.[Available_item_Desc_Chi] AS Available_item_Desc_Chi,
		R.[Available_item_Desc_CN] AS Available_item_Desc_CN,
		R.[Unit],
		R.[Per_Unit_Value],
		R.[Total_Amount],
		R.[Source],
		R.[Subsidize_Desc],
		R.[Subsidize_Desc_Chi],
		R.[Transaction_ID],
		R.[SP_ID],
		R.[Practice_Display_Seq],
		R.[Practice_Name],
		R.[Practice_Name_Chi],
		R.[Transaction_Dtm],
		R.[Ext_Ref_Status],
		'' AS [Remark],
		CONVERT(VARCHAR(100), DecryptByKey(R.[Encrypt_Field2])) AS [Eng_Name],
		R.[DOB],
		R.[Exact_DOB],
		R.[Sex],
		R.[High_Risk],
		R.[Vaccine_Brand],
		R.[Vaccine_Lot_No],
		CBD.Brand_Trade_Name,
		[Brand_Trade_Name_Chi] = 
			CASE 
				WHEN CBD.[Brand_Trade_Name_Chi] IS NULL THEN CBD.[Brand_Trade_Name]
				WHEN CBD.[Brand_Trade_Name_Chi] = '' THEN CBD.[Brand_Trade_Name]
				ELSE CBD.[Brand_Trade_Name_Chi]
			END,
		TAF.[AdditionalFieldValueCode] AS [Clinic_Type],
		TAF_1.[AdditionalFieldValueCode] AS [Join_EHRSS],
		TAF_2.[AdditionalFieldValueCode] AS [Contact_No],
		TAF_3.[AdditionalFieldValueCode] AS [Non_Local_Recovered]
	FROM 
		@result R
			INNER JOIN SubsidizeItem I
				ON R.Subsidize_Item_Code = I.Subsidize_Item_Code
				AND I.Subsidize_Type = 'VACCINE'
			LEFT OUTER JOIN COVID19VaccineBrandDetail CBD
				ON R.[Vaccine_Brand] = CBD.[Brand_ID]
			LEFT OUTER JOIN TransactionAdditionalField TAF WITH(NOLOCK)
				ON R.[Transaction_ID] = TAF.[Transaction_ID]
					AND TAF.[AdditionalFieldID] = 'ClinicType'
			LEFT OUTER JOIN TransactionAdditionalField TAF_1 WITH(NOLOCK)
				ON R.[Transaction_ID] = TAF_1.[Transaction_ID]
					AND TAF_1.[AdditionalFieldID] = 'JoinEHRSS'
			LEFT OUTER JOIN TransactionAdditionalField TAF_2 WITH(NOLOCK)
				ON R.[Transaction_ID] = TAF_2.[Transaction_ID]
					AND TAF_2.[AdditionalFieldID] = 'ContactNo'
			LEFT OUTER JOIN TransactionAdditionalField TAF_3 WITH(NOLOCK)
				ON R.[Transaction_ID] = TAF_3.[Transaction_ID]
					AND TAF_3.[AdditionalFieldID] = 'NonLocalRecovered'
			
	ORDER BY R.[Service_Receive_Dtm]

EXEC [proc_SymmetricKey_close]
	
END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_vaccine_get_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_vaccine_get_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_vaccine_get_byDocCodeDocID] TO WSEXT
GO

