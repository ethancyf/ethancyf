IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PreFillPersonalInformaion_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PreFillPersonalInformaion_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	30 Dec 2015
-- Description:		Change [Chi_Name] nvarchar(6) -> nvarchar(12)
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Kathy LEE
-- Modified date: 	6 Dec 2010
-- Description:		Remove inserting 'EC_Date'
-- =============================================
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 11 Sep 2009
-- Description:	Insert PreFillPersonalInformation 
-- =============================================

CREATE PROCEDURE [dbo].[proc_PreFillPersonalInformaion_add] 
	@Pre_Fill_Consent_ID char(15),
	@Doc_Code char(20),
	@DOB datetime,
	@Exact_DOB char(1),
	@Sex char(1),
	@Date_of_Issue datetime,
--	@EC_Serial_No varchar(10),
--	@EC_Reference_No varchar(15),
--	@EC_Age smallint,
--	@EC_Date_of_Registration datetime,
	@Foreign_Passport_No char(20),
	@Permit_To_Remain_Until datetime,
	@Other_Info	varchar(10),
--	@DataEntry_By varchar(20),
	@Identity varchar(20),
	@Eng_Name varchar(40),
	@Chi_Name nvarchar(12),
	@CCcode1 char(5),
	@CCcode2 char(5),
	@CCcode3 char(5),
	@CCcode4 char(5),
	@CCcode5 char(5),
	@CCcode6 char(5),
	@NumIdentity varchar(20),
	@AdoptionPrefixNum	char(7)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
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
		
	INSERT INTO [PreFillPersonalInformation]
	(
		[Pre_Fill_Consent_ID], [DOB], [Exact_DOB], [Sex], [Date_of_Issue],
		[Create_Dtm],
		--[DataEntry_By], 
		[Encrypt_Field1], [Encrypt_Field2], [Encrypt_Field3], [Encrypt_Field4], [Encrypt_Field5],
		[Encrypt_Field6], [Encrypt_Field7], [Encrypt_Field8], [Encrypt_Field9],
		[EC_Serial_No], [EC_Reference_No], [EC_Age], [EC_Date_of_Registration],
		[Encrypt_Field10],
		[Doc_Code],	[Foreign_Passport_No], [Permit_To_Remain_Until], 
		[Encrypt_Field11],
		[Other_Info]
	)
	VALUES
	(
		@Pre_Fill_Consent_ID, @DOB, @Exact_DOB, @Sex, @Date_of_Issue,
		GetDate(),
		--'',
		
		EncryptByKey(KEY_GUID('sym_Key'), @Identity),
		EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name),
		EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode1),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode2),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode3),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode4),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode5),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode6),
		
		NULL, NULL, NULL, NULL, 
		
		EncryptByKey(KEY_GUID('sym_Key'), @NumIdentity),
		
		@Doc_Code, @Foreign_Passport_No, @Permit_To_Remain_Until,
		
		EncryptByKey(KEY_GUID('sym_Key'), @AdoptionPrefixNum),
		
		@Other_Info
	)

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_PreFillPersonalInformaion_add] TO HCPUBLIC
GO
