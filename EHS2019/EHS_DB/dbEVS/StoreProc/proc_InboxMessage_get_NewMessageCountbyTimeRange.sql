IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMessage_get_NewMessageCountbyTimeRange]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InboxMessage_get_NewMessageCountbyTimeRange]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		19 Jun 2008
-- Description:		Get Inbox Records Count within the specified time range
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================


CREATE PROCEDURE 	[dbo].[proc_InboxMessage_get_NewMessageCountbyTimeRange] @message_reader		 varchar(20)
								, @last_check_dtm datetime
								, @current_dtm	datetime

as
BEGIN
-- =============================================
-- Declaration
-- =============================================

declare @hr as int
SELECT @hr=parm_value1 from [SystemParameters]
WHERE [Parameter_Name] = 'ActiveInboxIntrayHour' AND [Record_Status] = 'A' AND [Scheme_Code] = 'ALL'
		
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

select count(1)
from [Message] m, MessageReader mr
where m.message_id = mr.message_id
AND mr.[Message_Reader] = @message_reader
AND mr.[Record_status] = 'U'
--AND DATEDIFF(hh, m.[Create_Dtm], getdate()) <= @hr
AND m.[Create_Dtm] Between @last_check_dtm AND @current_dtm

END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_NewMessageCountbyTimeRange] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_NewMessageCountbyTimeRange] TO HCVU
GO
