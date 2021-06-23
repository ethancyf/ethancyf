IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (COVID19)
-- Modified by:		Winnie SUEN
-- Modified date:	28 May 2021
-- Description:		Add [PASS_Issue_Region]
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
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		(1) Change [EC_Reference_No] varchar(15) -> varchar(40)
--					(2) Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Feb 2010
-- Description:		Add Temp_Voucher_acc_ID
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 20 May 2008
-- Description:	Create a PersonInfoAmendHistory record when 
--				Voucher Account Amendment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	19 Sep 2009
-- Description:		Add Adoption_Prefix_Num, Permit_to_remain_until, doc_code, 
--					Foreign_Passport_No and Other_Info
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_add]
	@Voucher_Acc_ID char(15), 
	@IdentityNum varchar(20),
	@Eng_Name varchar(100),
	@Chi_Name nvarchar(12),
	@CCcode1 char(5),
	@CCcode2 char(5),
	@CCcode3 char(5),
	@CCcode4 char(5),
	@CCcode5 char(5),
	@CCcode6 char(5),
	@DOB datetime,
	@Exact_DOB char(1),
	@Sex char(1),
	@Date_of_Issue datetime,
	@Create_By_SmartID char(1),
	@Update_By char(20),
	@Record_Status char(1),
	@SubmitToVerify char(1),
	@Action_type char(1),
	@EC_Serial_No varchar(10),
	@EC_Reference_No varchar(40),
	@EC_Reference_No_Other_Format char(1),
	@EC_Age smallint,
	@EC_Date_of_Registration datetime,
	@Adoption_Prefix_Num char(7),
	@Permit_To_Remain_Until datetime,
	@Doc_Code char(10),
	@Foreign_Passport_No char(20),
	@other_info varchar(10),
	@temp_Voucher_acc_ID char(15),
	@SmartID_Ver	varchar(5),
	@PASS_Issue_Region	varchar(5)

	
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
-- =============================================
-- Return results
-- =============================================
	EXEC [proc_SymmetricKey_open]


    insert into PersonalInfoAmendHistory
	(
	System_dtm,
	Voucher_Acc_ID,
	Encrypt_Field1,
	Encrypt_Field2,
	Encrypt_Field3,
	Encrypt_Field4,
	Encrypt_Field5,
	Encrypt_Field6,
	Encrypt_Field7,
	Encrypt_Field8,
	Encrypt_Field9,
	DOB,
	Exact_DOB,
	Sex,
	Date_of_Issue,
	Create_By_SmartID,
	Update_By,
	Record_Status,  
	SubmitToVerify,
	Action_type,
	EC_Serial_No,
	EC_Reference_No,
	EC_Age,
	EC_Date_of_Registration,
	Encrypt_Field11,
	Permit_To_Remain_Until,
	Doc_Code,
	Foreign_Passport_No,
	other_info,
	Temp_voucher_Acc_ID,
	EC_Reference_No_Other_Format,
	SmartID_Ver,
	PASS_Issue_Region
	)
	values
	(
	getdate(),
	@Voucher_Acc_ID,
	EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum), 
	EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name),
	EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name),
	EncryptByKey(KEY_GUID('sym_Key'), @CCcode1),
	EncryptByKey(KEY_GUID('sym_Key'), @CCcode2),
	EncryptByKey(KEY_GUID('sym_Key'), @CCcode3),
	EncryptByKey(KEY_GUID('sym_Key'), @CCcode4),
	EncryptByKey(KEY_GUID('sym_Key'), @CCcode5),
	EncryptByKey(KEY_GUID('sym_Key'), @CCcode6),
	@DOB,
	@Exact_DOB,
	@Sex,
	@Date_of_Issue,
	@Create_By_SmartID,
	@Update_By,
	@Record_Status,
	@SubmitToVerify,
	@Action_type,
	@EC_Serial_No,
	@EC_Reference_No,
	@EC_Age,
	@EC_Date_of_Registration,
	EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num),
	@Permit_To_Remain_Until,
	@Doc_Code,
	@Foreign_Passport_No,
	@other_info,
	@temp_Voucher_acc_ID,
	@EC_Reference_No_Other_Format,
	@SmartID_Ver,
	@PASS_Issue_Region
	)

	 EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_add] TO HCVU
GO
