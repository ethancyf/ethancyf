IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

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
CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderHKICRowCount_get_byHKIC] TO HCVU
GO
