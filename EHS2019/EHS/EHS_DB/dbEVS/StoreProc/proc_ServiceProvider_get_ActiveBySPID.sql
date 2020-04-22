IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_ActiveBySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_ActiveBySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Clark YIP
-- Create date: 23 Jan 2009
-- Description:	Get the SP if it's status is active
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_ActiveBySPID]
	@SP_ID			char(8)
AS
BEGIN
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
	SELECT count(1)
	FROM ServiceProvider
	where 
	sp_id=@SP_ID and Record_Status='A'
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_ActiveBySPID] TO HCSP
GO
