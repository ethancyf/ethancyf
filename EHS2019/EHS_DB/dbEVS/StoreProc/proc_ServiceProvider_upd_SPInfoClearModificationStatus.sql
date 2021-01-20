IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_SPInfoClearModificationStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_SPInfoClearModificationStatus]
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
-- Create date: 09 Jul 2009
-- Description:	Clear the Service Provider Info in
--				Table "ServiceProvider" UnderModification value
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   29 Sep 2009
-- Description:	    No need to check tsmp for this store proc
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_SPInfoClearModificationStatus]
	@sp_id char(8), 
	@update_by	varchar(20)
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

		UPDATE	ServiceProvider
		Set		UnderModification = Null,
				Update_Dtm = getdate(),
				Update_By = @update_by
		WHERE	SP_ID = @sp_id	

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_SPInfoClearModificationStatus] TO HCVU
GO
