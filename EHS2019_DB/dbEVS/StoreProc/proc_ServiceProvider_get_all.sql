IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_all]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul Yip
-- Create date: 14 Dec 2009
-- Description:	Retrieve all service provider information
--				For PPIePR token serial no. retrieval schedule job
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_all] 
AS
BEGIN
	SET NOCOUNT ON;
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	select SP_ID, 
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID, 
			Record_status 
	from serviceprovider


	
CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_all] TO HCVU
GO
