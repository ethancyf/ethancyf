IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MessageReader_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MessageReader_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		26 Jun 2008
-- Description:		Insert MessageReader
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_MessageReader_add] 
							 @message_id			char(12)
							,@message_reader		varchar(20)							
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

INSERT INTO [MessageReader]
           ([Message_ID]
           ,[Message_Reader]
           ,[Record_Status]
           ,[Update_By]
           ,[Update_Dtm])
     VALUES
           (@message_id
           ,@message_reader
           ,'U'
           ,@update_by
           ,@update_dtm)
		
END
GO

GRANT EXECUTE ON [dbo].[proc_MessageReader_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_MessageReader_add] TO HCVU
GO
