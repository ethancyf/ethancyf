IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHSAccountTSMP_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHSAccountTSMP_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Karl LAM
-- Modified date:	15 Aug 2013
-- Description:	Grant execute permission to HCVU
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 09 Oct 2009
-- Description:	Update EHSAccountTSMP while Adding Transaction
-- =============================================


CREATE PROCEDURE [proc_EHSAccountTSMP_upd]
	@Doc_Code char(20),
	@identity varchar(20),
	@update_by varchar(20),
	@TSMP timestamp
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Validation 
-- =============================================

	IF (SELECT [TSMP] FROM [EHSAccountTSMP] WHERE [Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) ) ! = @TSMP
	BEGIN
		Raiserror('00011', 16, 1)
		RETURN @@error
	END	
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE [EHSAccountTSMP]
	SET [Update_By] = @update_by, [Update_Dtm] = GetDate()
	WHERE [Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity)
	
CLOSE SYMMETRIC KEY sym_Key
		
END
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_upd] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_upd] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_upd] TO WSEXT
GO