  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ImmdOutstandingInboxMessage_get_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ImmdOutstandingInboxMessage_get_all]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul Yip
-- Create date: 21 July 2010
-- Description:	Retrieve list of SPID which have Immd Outstanding Inbox message
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdOutstandingInboxMessage_get_all]
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


SELECT
	System_dtm,
	SP_ID
FROM [dbo].[ImmdOutstandingInboxMessage]



END

GO

GRANT EXECUTE ON [dbo].[proc_ImmdOutstandingInboxMessage_get_all] TO HCVU
GO
