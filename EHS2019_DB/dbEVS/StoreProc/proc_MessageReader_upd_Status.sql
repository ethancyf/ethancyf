IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MessageReader_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MessageReader_upd_Status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark Yip
-- Create date:		19 Jun 2008
-- Description:		Update Message Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_MessageReader_upd_Status] 
							 @message_id			char(12)
							,@message_reader		varchar(20)
							,@record_status			char(1)
							,@update_by				varchar(20)
							,@update_dtm			datetime

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

UPDATE MessageReader
SET	[Record_Status]=@record_status, [Update_by]=@update_by, [Update_dtm]=@update_dtm
where [message_id] = @message_id
AND [Message_Reader] = @message_reader
		
END
GO

GRANT EXECUTE ON [dbo].[proc_MessageReader_upd_Status] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_MessageReader_upd_Status] TO HCVU
GO
