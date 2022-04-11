IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- CR No.:			CRE20-023-67 (COVID19 - Prefill Personal Information)
-- Author:			Winnie SUEN
-- Create date:		1 Dec 2021
-- Description:		Retrieve Temporary EHS Account By Document & Document Identity & Subsidize item code
-- =============================================

-- [proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize] 'HKIC', ' R5234238', 'C19'
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize]
	@Doc_Code			char(20),
	@identity			varchar(20),
	@Subsidize_Item_Code char(10)	
AS
BEGIN
	SET NOCOUNT ON;
	
-- Performance Issue: Do not Retrieve Temporary Account with Status = 'D'
	
-- =============================================
-- Declaration
-- =============================================

DECLARE @tmpPersonalInformation Table 
(
	Voucher_Acc_ID		char(15),
	DOB					datetime,
	Exact_DOB			char(1),
	Sex					char(1),
	Date_of_Issue		datetime,
	Check_Dtm			datetime,
	Validating			char(1),
	---HKID_Card		char(1),
	Create_By_SmartID	char(1),
	Record_Status		char(1),
	Create_Dtm			datetime,
	Create_By			varchar(20),
	Update_Dtm			datetime,
	Update_By			varchar(20),
	DataEntry_By		varchar(20),	
		
	IdentityNum			varchar(20),
	Eng_Name			varchar(100),
	Chi_Name			nvarchar(20),
	CCcode1				char(5),
	CCcode2				char(5),
	CCcode3				char(5),
	CCcode4				char(5),
	CCcode5				char(5),
	CCcode6				char(5),
	TSMP				varbinary(100),
	EC_Serial_No		varchar(10),
	EC_Reference_No					varchar(40),
	---EC_Date	datetime,
	EC_Age				smallint,
	EC_Date_of_Registration	datetime,
	--Encrypt_Field10		varbinary(100),
	Doc_Code			char(20),
	Foreign_Passport_No	char(20),
	Permit_To_Remain_Until datetime,
	AdoptionPrefixNum	char(7),
	Other_Info			varchar(10),
	EC_Reference_No_Other_Format		char(1),
	Deceased			char(1),
	DOD					datetime,
	Exact_DOD			char(1),
	SmartID_Ver			varchar(5),
	PASS_Issue_Region	varchar(5)
)

DECLARE @tmpVoucherAcct Table
(
	Voucher_Acc_ID char(15)
)

DECLARE @tmpVoucherTransaction Table 
(
	Transaction_ID		char(20),
	Temp_Voucher_Acc_ID char(15),
	Scheme_Code			char(10),
	Subsidize_Item_Code	char(10)
)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	INSERT INTO @tmpVoucherAcct
	SELECT	TPI.[Voucher_Acc_ID]
	FROM	[TempPersonalInformation] TPI WITH(NOLOCK)
		INNER JOIN [TempVoucherAccount] TVA  WITH(NOLOCK)
			ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]
	WHERE	TVA.[Record_Status] <> 'D' AND
			[Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) 				
									
	INSERT INTO @tmpVoucherTransaction	
	SELECT
		VT.[Transaction_ID], VT.[Temp_Voucher_Acc_ID], VT.[Scheme_Code], TD.[Subsidize_Item_Code]
	FROM [VoucherTransaction] VT WITH(NOLOCK)
    INNER JOIN [TransactionDetail] TD WITH(NOLOCK)
		ON VT.Transaction_ID = TD.Transaction_id
	WHERE 
		[Temp_Voucher_Acc_ID] IN 
			(SELECT Voucher_Acc_ID FROM @tmpVoucherAcct)
		AND TD.Subsidize_Item_Code = @Subsidize_Item_Code
		AND VT.Record_Status NOT IN ('I', 'D') AND ISNULL(VT.Invalidation, '') <> 'I'	-- Active Transaction only

	INSERT INTO @tmpPersonalInformation 
	(
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,			
		Date_of_Issue,
		Check_Dtm,
		Validating,
		---HKID_Card,
		Create_By_SmartID,
		Record_Status,	
		Create_Dtm,		
		Create_By,			
		Update_Dtm,			
		Update_By,			
		DataEntry_By,

		IdentityNum,
		Eng_Name,
		Chi_Name,
		CCcode1,
		CCcode2,
		CCcode3,
		CCcode4,
		CCcode5,
		CCcode6,
				
		TSMP,
		EC_Serial_No,
		EC_Reference_No,
		---EC_Date	datetime,
		EC_Age,
		EC_Date_of_Registration,
		--Encrypt_Field10,
		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		AdoptionPrefixNum,
		Other_Info,
		EC_Reference_No_Other_Format,
		Deceased,
		DOD,
		Exact_DOD,
		SmartID_Ver,
		PASS_Issue_Region
	) 
	
	SELECT
		P.[Voucher_Acc_ID],
		P.[DOB],
		P.[Exact_DOB],
		P.[Sex],
		P.[Date_of_Issue],
		--[HKID_Card],
		P.[Check_Dtm],
		P.[Validating],
		P.[Create_By_SmartID],
		P.[Record_Status],
		P.[Create_Dtm],
		P.[Create_By],
		P.[Update_Dtm],
		P.[Update_By],
		P.[DataEntry_By],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])) as [IdentityNum],
		CONVERT(VARCHAR(100), DecryptByKey(P.[Encrypt_Field2])) as [Eng_Name],
		CONVERT(NVARCHAR, DecryptByKey(P.[Encrypt_Field3])) as [Chi_Name],			
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field4])) as [CCcode1],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field5])) as [CCcode2],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field6])) as [CCcode3],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field7])) as [CCcode4],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field8])) as [CCcode5],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field9])) as [CCcode6],
		P.[TSMP],		
		P.[EC_Serial_No],
		P.[EC_Reference_No],
		P.[EC_Age],
		P.[EC_Date_of_Registration],
		P.[Doc_Code],
		P.[Foreign_Passport_No],
		P.[Permit_To_Remain_Until],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field11])) as [AdoptionPrefixNum],
		P.[Other_Info],
		P.[EC_Reference_No_Other_Format],
		P.[Deceased],
		P.[DOD],
		P.[Exact_DOD],
		P.[SmartID_Ver],
		P.[PASS_Issue_Region]
	FROM 
		[TempPersonalInformation] AS P WITH(NOLOCK)
	WHERE
		P.[Voucher_Acc_ID] In (SELECT [Voucher_Acc_ID] FROM @tmpVoucherAcct)		

	EXEC [proc_SymmetricKey_close]

	SELECT
		--[TempVoucherAccount]
		VA.[Voucher_Acc_ID],		
		VA.[Scheme_Code],
		VA.[Validated_Acc_ID],
		VA.[Record_Status],
		VA.[Account_Purpose],
		VA.[Confirm_Dtm],
		VA.[Last_Fail_Validate_Dtm],
		VA.[Create_Dtm],
		VA.[Create_By],
		VA.[Update_Dtm],
		VA.[Update_By],
		VA.[DataEntry_By],
		VA.[TSMP],
		VA.[Original_Acc_ID],
		VA.[Original_Amend_Acc_ID],
		VA.[Create_By_BO],
		
		ISNULL(L.SP_ID, '') as SP_ID,
		ISNULL(L.SP_Practice_Display_Seq, 0) as SP_Practice_Display_Seq,
		
		--TempVoucherAccPendingVerify
		PV.First_Validate_Dtm,
		
		--@tmpVoucherTransaction
		ISNULL(VT.Transaction_ID, '') Transaction_ID,
		VA.[Deceased],
		VA.[SourceApp]
	FROM
		[TempVoucherAccount] VA WITH(NOLOCK)

			INNER JOIN @tmpVoucherTransaction VT 
				ON VA.[Voucher_Acc_ID] = VT.[Temp_Voucher_Acc_ID]
				
			LEFT OUTER JOIN VoucherAccountCreationLog L WITH(NOLOCK) on 
				VA.[Voucher_Acc_ID] = L.[Voucher_Acc_ID]
				
			LEFT OUTER JOIN TempVoucherAccPendingVerify PV WITH(NOLOCK) on 
				VA.[Voucher_Acc_ID] = PV.[Voucher_Acc_ID]

	WHERE
		VA.[Voucher_Acc_ID] IN
			(SELECT Voucher_Acc_ID FROM @tmpVoucherAcct)		
	
	SELECT
		[Voucher_Acc_ID],
		[DOB],
		[Exact_DOB],
		[Sex],
		[Date_of_Issue],
		--[HKID_Card],
		[Check_Dtm],
		[Validating],
		[Record_Status],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By],
		[DataEntry_By],
		[IdentityNum],
		[Eng_Name],
		[Chi_Name],			
		[CCcode1],
		[CCcode2],
		[CCcode3],
		[CCcode4],
		[CCcode5],
		[CCcode6],
		[TSMP],		
		[EC_Serial_No],
		[EC_Reference_No],
		[EC_Age],
		[EC_Date_of_Registration],
		[Doc_Code],
		[Foreign_Passport_No],
		[Permit_To_Remain_Until],
		[AdoptionPrefixNum],
		[Other_Info],
		[Create_By_SmartID],
		[EC_Reference_No_Other_Format],
		[Deceased],
		[DOD],
		[Exact_DOD],
		[SmartID_Ver],
		[PASS_Issue_Region]
	FROM
		@tmpPersonalInformation 
		
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byDocCodeDocIDSubsidize] TO WSEXT
GO