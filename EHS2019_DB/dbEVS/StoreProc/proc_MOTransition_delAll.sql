IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOTransition_delAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOTransition_delAll]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 22 Jun 2009
-- Description:	Delete All the MOTransition Table by HKID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	 
-- =============================================

CREATE PROCEDURE [dbo].[proc_MOTransition_delAll]
	@hk_id char(9)
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

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	Delete from MOTransition
	Where Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)				
				
CLOSE SYMMETRIC KEY sym_Key
				
END
GO

GRANT EXECUTE ON [dbo].[proc_MOTransition_delAll] TO HCVU
GO
