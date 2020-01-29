IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenSNSPID_get_By_HKID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenSNSPID_get_By_HKID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 10 July 2008
-- Description:	Get SPID and Token Serial Number by HKID
-- =============================================
CREATE PROCEDURE [dbo].[proc_TokenSNSPID_get_By_HKID]
	@HKID as char(9)
AS
BEGIN


OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
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
    SELECT T.Token_Serial_No TokenSN, SP.SP_ID UserID, T.Project ProjectCode
	from ServiceProvider SP, Token T
	where SP.SP_ID = T.[User_ID]
	and SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'),upper(@HKID))
	
CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_TokenSNSPID_get_By_HKID] TO HCVU
GO
