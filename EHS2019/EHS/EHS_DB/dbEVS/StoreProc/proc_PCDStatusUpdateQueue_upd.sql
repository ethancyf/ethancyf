IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PCDStatusUpdateQueue_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_PCDStatusUpdateQueue_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Modification History
-- CR No.:			
-- Modified by:	    
-- Modified date:   
-- Description:		
-- ==========================================================================================
-- ==========================================================================================
-- Author:		Koala CHENG
-- CR No.:		CRE17-016
-- Create Date:	17 Jul 2018
-- Description:	Update record from table - [PCDStatusUpdateQueue]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_PCDStatusUpdateQueue_upd]
	@sp_id				CHAR(8),
	@record_status		CHAR(1)
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

	UPDATE [PCDStatusUpdateQueue]
	SET	[Record_Status] = @record_status,
		[Update_Dtm] = GETDATE()
	WHERE	[SP_ID] = @sp_id

END
GO

GRANT EXECUTE ON [dbo].[proc_PCDStatusUpdateQueue_upd] TO HCVU
GO
