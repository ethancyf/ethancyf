IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPMigration_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPMigration_add]
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
-- Author:		Clark YIP
-- Create date: 23 Jun 2009
-- Description:	Insert the SPMigration Table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	 
-- =============================================

CREATE PROCEDURE [dbo].[proc_SPMigration_add]
	@hk_id char(9),	
	@record_status char(1)
	

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
-- =============================================
-- Return results
-- =============================================

EXEC [proc_SymmetricKey_open]

	INSERT INTO SPMigration
				(
				Encrypt_Field1,
				record_status,
				Print_status
				)
	VALUES		(	
				EncryptByKey(KEY_GUID('sym_Key'), @HK_ID),				
				@record_status,
				'N'
				)
				
EXEC [proc_SymmetricKey_close]
				
END
GO

GRANT EXECUTE ON [dbo].[proc_SPMigration_add] TO HCVU
GO
