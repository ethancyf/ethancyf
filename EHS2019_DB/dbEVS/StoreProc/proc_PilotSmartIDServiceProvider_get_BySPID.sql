IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PilotSmartIDServiceProvider_get_BySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PilotSmartIDServiceProvider_get_BySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 2 Feb 2010
-- Description:	Retrieve all active service provider's practices
--				which is able to use Smart IC related function in HCSP 
--				platform before 1st August 2010.
-- =============================================
-- =============================================
-- Modification History
-- Modified by:
-- Modified date:
-- Description:
-- =============================================


CREATE PROCEDURE [dbo].[proc_PilotSmartIDServiceProvider_get_BySPID]
	@SP_ID	char(8)

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
	select	Practice_Display_Seq 
	from	PilotSmartIDServiceProvider
	where	SP_ID = @sp_id and
			Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_PilotSmartIDServiceProvider_get_BySPID] TO HCSP
GO
