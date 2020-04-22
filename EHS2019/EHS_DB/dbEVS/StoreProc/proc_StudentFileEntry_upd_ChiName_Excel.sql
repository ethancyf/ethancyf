IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_ChiName_Excel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_ChiName_Excel]
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
-- Create by:		Chris YIM		
-- Create date:		26 Sep 2019
-- CR No.			CRE19-001
-- Description:		Update Chinese name in SP platform
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_ChiName_Excel]
	@Student_File_ID		VARCHAR(15),
	@Student_Seq			INT,
	@Name_CH_Excel			NVARCHAR(6)
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

	UPDATE 
		StudentFileEntry 
	SET 
		Encrypt_Field4 = EncryptByKey(KEY_GUID('sym_Key'), @Name_CH_Excel)
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_ChiName_Excel] TO HCSP

GO

