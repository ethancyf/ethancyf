IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmission_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmission_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:  Karl LAM  
-- Modified date: 21 Oct 2013  
-- Description:  Change File Name to Varchar(50)  
-- =============================================  
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 May 2008
-- Description:	Insert Professional Submission Record
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalSubmission_add]
	@File_Name varchar(50),
	@Reference_No char(20),
	@Display_Seq int,
	@Registration_code varchar(15),
	@SP_HKID char(9),
	@Surname varchar(40),
	@Other_Name varchar(40)
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

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
-- =============================================
-- Return results
-- =============================================

	INSERT INTO [dbo].[ProfessionalSubmission]
		(
		[File_Name],
		[Reference_No],
		[Display_Seq],
		[Registration_Code],
		[Encrypt_Field1],
		[Encrypt_Field10],
		[Encrypt_Field11]
		--[SP_HKID],
		--[Surname],
		--[Other_Name]
		)
	VALUES
		(
		@File_Name,
		@Reference_No,
		@Display_Seq,
		@Registration_code,
		EncryptByKey(KEY_GUID('sym_Key'), @SP_HKID),
		EncryptByKey(KEY_GUID('sym_Key'), @Surname),
		EncryptByKey(KEY_GUID('sym_Key'), @Other_Name)
		--@SP_HKID,
		--@Surname,
		--@Other_Name
		)

CLOSE SYMMETRIC KEY sym_Key

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmission_add] TO HCVU
GO
