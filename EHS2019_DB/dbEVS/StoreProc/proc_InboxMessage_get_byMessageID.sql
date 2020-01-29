IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMessage_get_byMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InboxMessage_get_byMessageID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Paul Yip
-- Create date:		4 Jun 2009
-- Description:		Get message content by Message ID
-- =============================================


CREATE PROCEDURE 	[dbo].[proc_InboxMessage_get_byMessageID] 
							@message_id		 varchar(20)

as
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

select subject, message, message_id
from message 
where message_id = @message_id 
END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_byMessageID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_byMessageID] TO HCVU
GO
