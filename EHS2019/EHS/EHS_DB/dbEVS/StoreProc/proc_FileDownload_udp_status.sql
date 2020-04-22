IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileDownload_udp_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileDownload_udp_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Clark Yip
-- Create date:		26 Jun 2008
-- Description:		Update FileDownload Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_FileDownload_udp_status] 
							 @generation_id			char(12)
							,@user_id				varchar(20)
							,@status				char(1)
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

	UPDATE [FileDownload]
	Set [Download_Status] = @status, [Update_by]=@update_by, [Update_Dtm]=@update_dtm
	where [Generation_ID]=@generation_id and [User_ID]=@user_id

END
GO

GRANT EXECUTE ON [dbo].[proc_FileDownload_udp_status] TO HCVU
GO
