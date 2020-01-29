IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileDownload_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileDownload_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		26 Jun 2008
-- Description:		Insert FileDownload
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_FileDownload_add] 
							 @generation_id			char(12)
							,@user_id				varchar(20)
							
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
if (select count(1) from [FileDownload] 
	where [Generation_ID] = @generation_id AND [User_ID] = @user_id) = 0
begin
	INSERT INTO [FileDownload]
			   ([Generation_ID]
			   ,[User_ID]
			   ,[Download_Status]
			   )
		 VALUES
			   (@generation_id
			   ,@user_id
			   ,'N'
			   )
end

END
GO

GRANT EXECUTE ON [dbo].[proc_FileDownload_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_FileDownload_add] TO HCSP
GO

