IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_OverWriteByValidatedAcct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_OverWriteByValidatedAcct]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023-68 (Add Chinese Name)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2021
-- Description:		Change [Name_CH] nvarchar(6) -> nvarchar(12)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023-52
-- Modified by:		Winnie SUEN
-- Modified date:	30 Jun 2021
-- Description:		Extend Eng Name to 82
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
-- Create by:		Chris YIM		
-- Create date:		16 Aug 2019
-- CR No.			CRE19-001
-- Description:		Update Entry from Validated Account in SP platform
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_OverWriteByValidatedAcct]
	@Student_File_ID				VARCHAR(15)	,
	@Student_Seq					INT,
	@Name_EN						VARCHAR(100),
	@Surname_EN						VARCHAR(100),
	@Given_Name_EN					VARCHAR(100),
	@Name_CH						NVARCHAR(12),
	@Name_CH_Excel					NVARCHAR(12),
	@DOB							Datetime,
	@Exact_DOB						CHAR(1),
	@Sex							CHAR(1),
	@Date_of_Issue					Datetime,
	@Permit_To_Remain_Until			Datetime,
	@Foreign_Passport_No			VARCHAR(20),
	@EC_Serial_No					VARCHAR(10),
	@EC_Reference_No				VARCHAR(40),
	@EC_Reference_No_Other_Format	CHAR(1),
	@Update_By						VARCHAR(20),
	@Update_Dtm						Datetime
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
-- Return results
-- =============================================
	EXEC [proc_SymmetricKey_open]

	IF @Name_CH_Excel IS NULL
		BEGIN
			UPDATE 
				StudentFileEntry 
			SET 
				Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Name_EN),
				Encrypt_Field2_1 = EncryptByKey(KEY_GUID('sym_Key'), @Surname_EN), 
				Encrypt_Field2_2 = EncryptByKey(KEY_GUID('sym_Key'), @Given_Name_EN), 
				Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Name_CH), 
				DOB = @DOB,
				Exact_DOB = @Exact_DOB,
				Sex = @Sex,
				Date_of_Issue = @Date_of_Issue,
				Permit_To_Remain_Until = @Permit_To_Remain_Until,
				Foreign_Passport_No = @Foreign_Passport_No,
				EC_Serial_No = @EC_Serial_No,
				EC_Reference_No = @EC_Reference_No,
				EC_Reference_No_Other_Format = @EC_Reference_No_Other_Format,
				Last_Rectify_By = @Update_By,				
				Last_Rectify_Dtm = @Update_Dtm				
			WHERE 
				Student_File_ID = @Student_File_ID 
				AND Student_Seq = @Student_Seq


		END
	ELSE
		BEGIN
			UPDATE 
				StudentFileEntry 
			SET 
				Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Name_EN),
				Encrypt_Field2_1 = EncryptByKey(KEY_GUID('sym_Key'), @Surname_EN), 
				Encrypt_Field2_2 = EncryptByKey(KEY_GUID('sym_Key'), @Given_Name_EN), 
				Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Name_CH), 
				Encrypt_Field4 = EncryptByKey(KEY_GUID('sym_Key'), @Name_CH_Excel), 
				DOB = @DOB,
				Exact_DOB = @Exact_DOB,
				Sex = @Sex,
				Date_of_Issue = @Date_of_Issue,
				Permit_To_Remain_Until = @Permit_To_Remain_Until,
				Foreign_Passport_No = @Foreign_Passport_No,
				EC_Serial_No = @EC_Serial_No,
				EC_Reference_No = @EC_Reference_No,
				EC_Reference_No_Other_Format = @EC_Reference_No_Other_Format,
				Last_Rectify_By = @Update_By,				
				Last_Rectify_Dtm = @Update_Dtm				
			WHERE 
				Student_File_ID = @Student_File_ID 
				AND Student_Seq = @Student_Seq

		END

	EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_OverWriteByValidatedAcct] TO HCSP

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_OverWriteByValidatedAcct] TO HCVU

GO

