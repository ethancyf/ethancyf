IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ImmdOutstandingInboxMessage_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ImmdOutstandingInboxMessage_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul Yip
-- Create date: 21 July 2010
-- Description:	keep track of IMMD outstanding inbox message
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdOutstandingInboxMessage_add]
	@SP_ID as char(8)
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

INSERT INTO [dbo].[ImmdOutstandingInboxMessage]
(
	[System_Dtm], [SP_ID]
)
VALUES
(
	GetDate(), @SP_ID
)
END

GO

GRANT EXECUTE ON [dbo].[proc_ImmdOutstandingInboxMessage_add] TO HCVU
GO



