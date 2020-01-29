IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditLog_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditLog_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[proc_AuditLog_add] @action_time	datetime
						,@end_time	datetime
						,@client_ip	varchar(20)
						,@user_id	varchar(12)
						,@function_code	char(6)
						,@log_id	varchar(10)
						,@action_type	text
                                        
as 

INSERT INTO AuditLog2
           ([System_Dtm]
           ,[Action_Dtm]
           ,[End_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[Function_Code]
           ,[Log_ID]
           ,[Action_Type])
     VALUES
           (getdate()
           ,@action_time
           ,@end_time
           ,@client_ip
           ,@user_id
           ,@function_code
           ,@log_id
           ,@action_type)



GO
