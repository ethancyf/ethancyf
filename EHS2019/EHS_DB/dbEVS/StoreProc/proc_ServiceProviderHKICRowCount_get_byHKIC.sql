IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC]
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
-- Author:		Kathy LEE
-- Create date: 19 June 2008
-- Description:	Search whether any exisitng HKIC No. in
--				Table "ServiceProvider" and Table "ServiceProviderStaging"
--				by using HKIC No.
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC]
	@sp_hkid char(9)
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
--DECLARE	@returnVal int
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

	SELECT (SELECT COUNT(1)
	FROM	ServiceProvider SP
	WHERE	SP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid)
			and Record_Status <> 'D')
	+
	(SELECT Count(1)
	FROM	ServiceProviderStaging SPS
	WHERE	SPS.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid))

	--SELECT	@returnVal

	--RETURN	@returnVal
EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC] TO HCVU
GO
