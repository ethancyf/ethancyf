IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOTransition_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOTransition_del]
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
-- Create date: 19 Jun 2009
-- Description:	Delete the MOTransition Table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	 
-- =============================================

CREATE PROCEDURE [dbo].[proc_MOTransition_del]
	@hk_id char(9),
	@display_seq smallint

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

	Delete from MOTransition
	Where Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HK_ID) and Display_seq=@display_seq				
				
EXEC [proc_SymmetricKey_close]
				
END
GO

GRANT EXECUTE ON [dbo].[proc_MOTransition_del] TO HCVU
GO
