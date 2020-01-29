IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Message_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Message_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		26 Jun 2008
-- Description:		Insert Message
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_Message_add] 
							 @message_id			char(12)
							,@subject				nvarchar(500)
							,@content				ntext
							,@create_by				varchar(20)
							,@create_dtm			datetime

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

INSERT INTO [Message]
           ([Message_ID]
           ,[Subject]
           ,[Message]
           ,[Create_By]
           ,[Create_Dtm])
     VALUES
           (@message_id
           ,@subject
           ,@content
           ,@create_by
           ,@create_dtm)
END
GO

GRANT EXECUTE ON [dbo].[proc_Message_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Message_add] TO HCVU
GO
