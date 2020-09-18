IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_OverWriteByValidatedAcct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_OverWriteByValidatedAcct]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Create by:		Winnie SUEN
-- Create date:		10 Sep 2020
-- CR No.			CRE20-003 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Update Entry to Staging from Validated Account
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_OverWriteByValidatedAcct]
	@Student_File_ID				VARCHAR(15)	,
	@Student_Seq					INT,
	@Name_EN						VARCHAR(40),
	@Surname_EN						VARCHAR(40),
	@Given_Name_EN					VARCHAR(40),
	@Name_CH						NVARCHAR(6),
	@Name_CH_Excel					NVARCHAR(6),
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
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	IF @Name_CH_Excel IS NULL
		BEGIN
			UPDATE 
				StudentFileEntryStaging 
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
				StudentFileEntryStaging 
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

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_OverWriteByValidatedAcct] TO HCSP

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_OverWriteByValidatedAcct] TO HCVU

GO

