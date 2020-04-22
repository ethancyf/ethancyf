IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMessage_get_byStatusID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InboxMessage_get_byStatusID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		19 Jun 2008
-- Description:		Get Inbox Records
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	04 Oct 2008
-- Description:		Filter out the inbox messages when the day is over 180 days (parameter)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	4 june 2009
-- Description:		Remove message field
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_InboxMessage_get_byStatusID] @msg_status			char(1)
							,@message_reader		 varchar(20)

as
BEGIN
-- =============================================
-- Declaration
-- =============================================
declare @day as int

SELECT @day=parm_value1 from SystemParameters
WHERE [Parameter_Name] = 'InboxMsgKeepDay' AND [Record_Status] = 'A' AND [Scheme_Code] = 'ALL'

declare @trash_day as int
SELECT @trash_day=parm_value1 from SystemParameters
WHERE [Parameter_Name] = 'TrashKeepDay'AND [Record_Status] = 'A' AND [Scheme_Code] = 'ALL'

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

select m.Subject , mr.[Record_status], mr.[Message_Reader], m.[Create_Dtm], m.message_id
from [Message] m, MessageReader mr
where m.message_id = mr.message_id
AND mr.[Message_Reader] = @message_reader
AND ((@msg_status='D' and mr.[Record_status] = @msg_status and DATEDIFF(dd, mr.[Update_dtm], getdate()) <= @trash_day AND DATEDIFF(dd, m.[Create_Dtm], getdate()) <= @day) 
--AND ((@msg_status='D' and mr.[Record_status] = @msg_status and DATEDIFF(hh, m.[Create_Dtm], getdate()) <= @hr) 
OR (@msg_status<>'D' and mr.[Record_status] in ('R','U') and DATEDIFF(dd, m.[Create_Dtm], getdate()) <= @day))  -- and DATEDIFF(hh, m.[Create_Dtm], getdate()) <= @hr))
order by m.[Create_Dtm] desc
END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_byStatusID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_byStatusID] TO HCVU
GO
