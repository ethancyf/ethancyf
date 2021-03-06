IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenSNSPID_get_By_HKID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenSNSPID_get_By_HKID]
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
-- Author:		Timothy LEUNG
-- Create date: 10 July 2008
-- Description:	Get SPID and Token Serial Number by HKID
-- =============================================
CREATE PROCEDURE [dbo].[proc_TokenSNSPID_get_By_HKID]
	@HKID as char(9)
AS
BEGIN


EXEC [proc_SymmetricKey_open]
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
	
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_TokenSNSPID_get_By_HKID] TO HCVU
GO
