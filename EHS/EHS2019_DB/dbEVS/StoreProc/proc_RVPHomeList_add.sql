IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_RVPHomeList_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
-- =============================================
-- Author:		Twinsen Chan
-- CR No.:		CRE11-013
-- Create date: 19 Oct 2012
-- Description:	Add RVP Home List
-- =============================================
CREATE PROCEDURE [dbo].[proc_RVPHomeList_add]
	@RCH_code		varchar(10),
	@Type			varchar(5),
	@Homename_Eng	varchar(255),
	@Homename_Chi	nvarchar(255),
	@Address_Eng	varchar(1000),
	@Address_Chi	nvarchar(255),
	@Record_Status	char(1),
	@UserID			varchar(20)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[RVPHomeList]
           ([RCH_code]
           ,[Type]
           ,[District]
           ,[Homename_Eng]
           ,[Homename_Chi]
           ,[Address_Eng]
           ,[Address_Chi]
           ,[Record_Status]
           ,[Create_By]
           ,[Create_Dtm]
           ,[Update_By]
           ,[Update_Dtm])
     VALUES
           (@RCH_code
           ,@Type
           ,''
           ,@Homename_Eng
           ,@Homename_Chi
           ,@Address_Eng
           ,@Address_Chi
           ,@Record_Status
           ,@UserID
           ,GETDATE()
           ,@UserID
           ,GETDATE())
END

GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_add] TO HCVU
GO
