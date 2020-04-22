IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempPersonalInformation_upd_PersonalInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempPersonalInformation_upd_PersonalInfo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
-- CR No.			CRE17-018 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19
-- Modified date:	23 Aug 2018 
-- Description:		Update [Encrypt_Field1] (Doc No.), [Encrypt_Field10] (Num Identity) and [Doc_Code]
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
-- Modified date:	4 May 2010
-- Description:		(1) Change [EC_Reference_No] varchar(15) -> varchar(40)
--					(2) Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Stanley Chan
-- Modified date:	11 March 2010
-- Description:		For HCSP Rectification
--					If Account is modified by smart ID, the field of Create_By_SmartIC will change to "Y"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	28 August 2009
-- Description:		For new Schema
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	27 August 2009
-- Description:		Add the following information in result set
--					doc type, foreign_passport_no, permit_to_remain_until
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 26 May 2008
-- Description:	Update TempPersonalInformation 
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempPersonalInformation_upd_PersonalInfo]
	@Voucher_Acc_ID char(15),
	@DOB datetime,
	@Exact_DOB char(1),
	@Sex char(1),
	@Date_of_Issue datetime,
	@Update_By varchar(20),
	@DataEntry_By varchar(20),
	@Identity varchar(20),
	@Eng_Name varchar(40),
	@Chi_Name nvarchar(12),
	@CCcode1 char(5),
	@CCcode2 char(5),
	@CCcode3 char(5),
	@CCcode4 char(5),
	@CCcode5 char(5),
	@CCcode6 char(5),
	@TSMP timestamp,
	@EC_Serial_No varchar(10),
	@EC_Reference_No varchar(40),
	@EC_Age smallint,
	@EC_Date_of_Registration datetime,
	@NumIdentity varchar(20),
	@Doc_Code char(20),
	@Foreign_Passport_No char(20),
	@Permit_To_Remain_Until datetime,
	@AdoptionPrefixNum char(7),
	@Other_Info varchar(10),
	@Create_By_SmartIC char(1),
	@EC_Reference_No_Other_Format	char(1),
	@SmartID_Ver	varchar(5)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	if (select TSMP from TempPersonalInformation
		WHERE Voucher_Acc_ID = @Voucher_Acc_ID AND Doc_Code = @Doc_Code) != @TSMP
	begin
		Raiserror('00011', 16, 1)
		return @@error
	end

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key


	UPDATE [TempPersonalInformation]
	SET	
		Doc_Code = @Doc_Code,
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Identity),
		Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name),
		Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name),
		Encrypt_Field4 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode1),
		Encrypt_Field5 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode2),
		Encrypt_Field6 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode3),
		Encrypt_Field7 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode4),
		Encrypt_Field8 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode5),
		Encrypt_Field9 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode6),
		Encrypt_Field10 = EncryptByKey(KEY_GUID('sym_Key'), @NumIdentity),
		Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @AdoptionPrefixNum),		
		[DOB] = @DOB,
		[Exact_DOB] = @Exact_DOB,
		[Sex] = @Sex,
		[Date_of_Issue] = @Date_of_Issue,
		[Validating] = 'N',
		[Update_Dtm] = GETDATE(),
		[Update_By] = @Update_By,
		[EC_Serial_No] = @EC_Serial_No,
		[EC_Reference_No] = @EC_Reference_No,
		[EC_Age] = @EC_Age,
		[EC_Date_of_Registration] = @EC_Date_of_Registration,
		[Foreign_passport_no] = @foreign_passport_no,
		[Permit_To_Remain_Until] = @permit_to_remain_until,
		[Create_By_SmartID] = @Create_By_SmartIC,
		[Other_Info] = @Other_Info,
		[EC_Reference_No_Other_Format] = @EC_Reference_No_Other_Format,
		[SmartID_Ver] = @SmartID_Ver
	WHERE
		[Voucher_Acc_ID] = @Voucher_Acc_ID -- AND [Doc_Code] = @doc_code	

	CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformation_upd_PersonalInfo] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempPersonalInformation_upd_PersonalInfo] TO HCVU
GO
