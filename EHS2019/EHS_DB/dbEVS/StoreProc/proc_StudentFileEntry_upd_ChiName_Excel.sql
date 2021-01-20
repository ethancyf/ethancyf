IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_ChiName_Excel]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_ChiName_Excel]
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
-- Modified by:		Chris YIM	
-- Modified date:	26 Sep 2019
-- CR No.			CRE20-003
-- Description:		Grant HCVU right
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
	EXEC [proc_SymmetricKey_open]

	UPDATE 
		StudentFileEntry 
	SET 
		Encrypt_Field4 = EncryptByKey(KEY_GUID('sym_Key'), @Name_CH_Excel)
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq

	EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_ChiName_Excel] TO HCSP

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_ChiName_Excel] TO HCVU

GO

