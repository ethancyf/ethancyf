IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempPersonalInformaion_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempPersonalInformaion_add]
GO

SET ANSI_NULLS ON
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
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased], [DOD], [Exact_DOD]
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
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 May 2010
-- Description:		(1) Change [EC_Reference_No] varchar(15) -> varchar(40)
--					(2) Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	23 Dec 2009
-- Description:		Add Create_by_SmartID field
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 05 May 2008
-- Description:	Insert TempPersonalInformation for
--		Temporary Voucher Recipient Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	21 Aug 2009
-- Description:		Oct19 Document Type
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempPersonalInformaion_add] 
	@Voucher_Acc_ID char(15),
	@DOB datetime,
	@Exact_DOB char(1),
	@Sex char(1),
	@Date_of_Issue datetime,
	--@HKID_Card char(1),
	--@Check_Dtm datetime
	--@Validating char(1)
	@Record_Status char(1),
	--@Create_Dtm	datetime,
	@Create_By varchar(20),
	--@Update_Dtm datetime,
	--@Update_By varchar(20),
	@DataEntry_By varchar(20),
	@Identity varchar(20),
	@Eng_Name varchar(100),
	@Chi_Name nvarchar(12),
	@CCcode1 char(5),
	@CCcode2 char(5),
	@CCcode3 char(5),
	@CCcode4 char(5),
	@CCcode5 char(5),
	@CCcode6 char(5),
	@EC_Serial_No varchar(10),
	@EC_Reference_No varchar(40),
	--@EC_Date datetime,
	@EC_Age smallint,
	@EC_Date_of_Registration datetime,
	@NumIdentity varchar(20),
	@Doc_Code char(20),
	@Foreign_Passport_No char(20),
	@Permit_To_Remain_Until datetime,
	@AdoptionPrefixNum	char(7),
	@Other_Info	varchar(10),
	@Create_by_SmartID char(1),
	@EC_Reference_No_Other_Format char(1),
	@Deceased	char(1), 
	@DOD		datetime, 
	@Exact_DOD	char(1),
	@SmartID_Ver	varchar(5)
WITH RECOMPILE
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
		
	INSERT INTO [TempPersonalInformation]
	(
		[Voucher_Acc_ID], [DOB], [Exact_DOB], [Sex], [Date_of_Issue],
		--[HKID_Card], 
		[Check_Dtm], [Validating], [Record_Status], [Create_Dtm],
		[Create_By], [Update_Dtm], [Update_By], [DataEntry_By], 
		[Encrypt_Field1], [Encrypt_Field2], [Encrypt_Field3], [Encrypt_Field4], [Encrypt_Field5],
		[Encrypt_Field6], [Encrypt_Field7], [Encrypt_Field8], [Encrypt_Field9],
		[EC_Serial_No], [EC_Reference_No], --[EC_Date], 
		[EC_Age], [EC_Date_of_Registration],
		[Encrypt_Field10],
		[Doc_Code],	[Foreign_Passport_No], [Permit_To_Remain_Until], 
		[Encrypt_Field11],
		[Other_Info], [Create_By_SmartID],
		[EC_Reference_No_Other_Format],
		[Deceased], 
		[DOD], 
		[Exact_DOD],
		[SmartID_Ver]
	)
	VALUES
	(
		@Voucher_Acc_ID, @DOB, @Exact_DOB, @Sex, @Date_of_Issue,
		--'', 
		NULL, NULL, @Record_Status, GetDate(),
		@Create_By, GetDate(), @Create_By, @DataEntry_By,
		
		EncryptByKey(KEY_GUID('sym_Key'), @Identity),
		EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name),
		EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode1),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode2),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode3),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode4),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode5),
		EncryptByKey(KEY_GUID('sym_Key'), @CCcode6),
		
		@EC_Serial_No, @EC_Reference_No, --NULL, 
		@EC_Age, @EC_Date_of_Registration,
		
		EncryptByKey(KEY_GUID('sym_Key'), @NumIdentity),
		
		@Doc_Code, @Foreign_Passport_No, @Permit_To_Remain_Until,
		
		EncryptByKey(KEY_GUID('sym_Key'), @AdoptionPrefixNum),
		
		@Other_Info, @Create_by_SmartID,
		@EC_Reference_No_Other_Format,
		@Deceased,
		@DOD,
		@Exact_DOD,
		@SmartID_Ver
	)

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformaion_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformaion_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformaion_add] TO WSEXT
Go

