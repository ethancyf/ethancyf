IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_all]
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
-- Author:		Paul Yip
-- Create date: 14 Dec 2009
-- Description:	Retrieve all service provider information
--				For PPIePR token serial no. retrieval schedule job
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_all] 
AS
BEGIN
	SET NOCOUNT ON;
EXEC [proc_SymmetricKey_open]

	select SP_ID, 
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID, 
			Record_status 
	from serviceprovider


	
EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_all] TO HCVU
GO
