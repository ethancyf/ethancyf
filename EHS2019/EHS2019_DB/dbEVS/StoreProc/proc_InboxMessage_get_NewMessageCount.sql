IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMessage_get_NewMessageCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InboxMessage_get_NewMessageCount]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================  
-- Modification History  
-- CR No.:			I-CRE17-005
-- Modified by:		Koala CHENG
-- Modified date:	15 January 2018
-- Description:		Performance Tuning - Add WITH (NOLOCK)
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Author:			Clark Yip
-- Create date:		19 Jun 2008
-- Description:		Get Inbox Records Count
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   25 Aug 2009
-- Description:	    Add the days filtering
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_InboxMessage_get_NewMessageCount] @message_reader		 varchar(20)

as
BEGIN
-- =============================================
-- Declaration
-- =============================================
DECLARE @Performance_Start_Dtm datetime
SET @Performance_Start_Dtm = GETDATE()

DECLARE @In_Message_Reader varchar(20)
SET @In_Message_Reader = @message_reader

declare @day as int
select @day=parm_value1 from SystemParameters WITH (NOLOCK)
where Parameter_Name = 'InboxMsgKeepDay'and Record_Status = 'A' AND [Scheme_Code] = 'ALL'

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
from [Message] m WITH (NOLOCK), MessageReader mr WITH (NOLOCK)
where m.message_id = mr.message_id
AND mr.[Message_Reader] = @In_Message_Reader
AND mr.[Record_status] = 'U'
AND DATEDIFF(dd, m.[Create_Dtm], getdate()) <= @day

IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
	DECLARE @Performance_End_Dtm datetime
	SET @Performance_End_Dtm = GETDATE()
	
	EXEC proc_SProcPerformance_add 'proc_InboxMessage_get_NewMessageCount',
								   @In_Message_Reader,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
	
END
	
END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_NewMessageCount] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InboxMessage_get_NewMessageCount] TO HCVU
GO
