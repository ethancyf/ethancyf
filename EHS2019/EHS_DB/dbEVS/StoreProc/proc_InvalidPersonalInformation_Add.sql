IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InvalidPersonalInformation_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InvalidPersonalInformation_Add]
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
-- Description:		Change [Encrypt_Field3] nvarchar(6) -> nvarchar(12)
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		25 March 2010
-- Description:		Add invalid personal information
-- =============================================

CREATE PROCEDURE [dbo].[proc_InvalidPersonalInformation_Add]
	@Invalid_Acc_ID				char(15),
	@Doc_Code					char(20),
	@DOB						datetime,
	@Exact_DOB					char(1),
	@Sex						char(1),
	@Date_of_Issue				datetime,
	@Create_By_SmartID			char(1),
	@EC_Serial_No				varchar(10),
	@EC_Reference_No			varchar(15),
	@EC_Age						smallint,
	@EC_Date_of_Registration	datetime,
	@Foreign_Passport_No		varchar(40),
	@Permit_To_Remain_Until		datetime,
	@Record_Status				char(1),
	@Other_Info					varchar(10),
	@Create_Dtm					datetime,
	@Create_By					varchar(20),
	@Update_Dtm					datetime,
	@Update_By					varchar(20),
	@DataEntry_By				varchar(20),
	@Encrypt_Field1				varchar(20),	-- Identity No.
	@Encrypt_Field2				varchar(40),	-- English Name
	@Encrypt_Field3				nvarchar(12),	-- Chinese Name
	@Encrypt_Field4				char(5),		-- CC Code 1
	@Encrypt_Field5				char(5),		-- CC Code 2
	@Encrypt_Field6				char(5),		-- CC Code 3
	@Encrypt_Field7				char(5),		-- CC Code 4
	@Encrypt_Field8				char(5),		-- CC Code 5
	@Encrypt_Field9				char(5),		-- CC Code 6
	@Encrypt_Field10			varchar(20),	-- No. part of the Identity No.
	@Encrypt_Field11			char(7),		-- Adoption Cert. prefix
	@SmartID_Ver				varchar(5)

AS BEGIN

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
-- Process data
-- =============================================
	EXEC [proc_SymmetricKey_open]

	INSERT INTO InvalidPersonalInformation (
		Invalid_Acc_ID,
		Doc_Code,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Create_By_SmartID,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Record_Status,
		Other_Info,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Encrypt_Field10,
		Encrypt_Field11,
		SmartID_Ver
	) VALUES (
		@Invalid_Acc_ID,
		@Doc_Code,
		@DOB,
		@Exact_DOB,
		@Sex,
		@Date_of_Issue,
		@Create_By_SmartID,
		@EC_Serial_No,
		@EC_Reference_No,
		@EC_Age,
		@EC_Date_of_Registration,
		@Foreign_Passport_No,
		@Permit_To_Remain_Until,
		@Record_Status,
		@Other_Info,
		@Create_Dtm,
		@Create_By,
		@Update_Dtm,
		@Update_By,
		@DataEntry_By,
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field1),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field2),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field3),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field4),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field5),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field6),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field7),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field8),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field9),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field10),
		EncryptByKey(KEY_GUID('sym_Key'), @Encrypt_Field11),
		@SmartID_Ver
	)
	
	EXEC [proc_SymmetricKey_close]
	

END
GO

GRANT EXECUTE ON [dbo].[proc_InvalidPersonalInformation_Add] TO HCVU
GO
