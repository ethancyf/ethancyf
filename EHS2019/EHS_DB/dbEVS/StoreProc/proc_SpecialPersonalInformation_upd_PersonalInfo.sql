IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialPersonalInformation_upd_PersonalInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialPersonalInformation_upd_PersonalInfo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
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
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		(1) Change [EC_Reference_No] varchar(15) -> varchar(40)
--					(2) Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 08 Sep 2009
-- Description:	Update SpecialPersonalInformation 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SpecialPersonalInformation_upd_PersonalInfo]
	@Voucher_Acc_ID char(15),
	@DOB datetime,
	@Exact_DOB char(1),
	@Sex char(1),
	@Date_of_Issue datetime,
	--@HKID_Card char(1),
	--@Check_Dtm datetime
	--@Validating char(1)
	--@Record_Status char(1),
	--@Create_Dtm	datetime,
	--@Create_By varchar(20),
	--@Update_Dtm datetime,
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
	@EC_Reference_No_Other_Format char(1),
	--@EC_Date datetime,
	@EC_Age smallint,
	@EC_Date_of_Registration datetime,
	--@NumIdentity varchar(20),
	@Doc_Code char(20),
	@Foreign_Passport_No char(20),
	@Permit_To_Remain_Until datetime,
	@AdoptionPrefixNum char(7),
	@Other_Info varchar(10)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	if (select TSMP from [SpecialPersonalInformation]
		WHERE [Special_Acc_ID] = @Voucher_Acc_ID AND Doc_Code = @Doc_Code) != @TSMP
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
EXEC [proc_SymmetricKey_open]


	UPDATE [SpecialPersonalInformation]
	SET	
		--Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Identity),
		Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name),
		Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chi_Name),
		Encrypt_Field4 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode1),
		Encrypt_Field5 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode2),
		Encrypt_Field6 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode3),
		Encrypt_Field7 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode4),
		Encrypt_Field8 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode5),
		Encrypt_Field9 = EncryptByKey(KEY_GUID('sym_Key'), @CCcode6),
		--Encrypt_Field10 = EncryptByKey(KEY_GUID('sym_Key'), @NumIdentity),
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
		--EC_Date = @EC_Date,
		[EC_Age] = @EC_Age,
		[EC_Date_of_Registration] = @EC_Date_of_Registration,
		[Foreign_passport_no] = @foreign_passport_no,
		[Permit_To_Remain_Until] = @permit_to_remain_until,
		[Other_Info] = @Other_Info,
		[EC_Reference_No_Other_Format] = @EC_Reference_No_Other_Format
	WHERE
		[Special_Acc_ID] = @Voucher_Acc_ID AND [Doc_Code] = @doc_code	


EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_SpecialPersonalInformation_upd_PersonalInfo] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SpecialPersonalInformation_upd_PersonalInfo] TO HCVU
GO
