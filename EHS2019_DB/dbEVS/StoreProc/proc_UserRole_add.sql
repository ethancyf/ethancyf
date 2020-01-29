IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_UserRole_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_UserRole_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		17-07-2008
-- Description:		Add UserRole
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   26 May 2009
-- Description:	    Add the scheme code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   13 Jul 2009
-- Description:	    Change the scheme code to MScheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   11 Aug 2009
-- Description:	    Change back the Mscheme code to Scheme_Code
-- =============================================
CREATE Procedure dbo.proc_UserRole_add
@User_ID		varchar(20)
, @Role_Type		smallint
, @scheme_code		char(10)
, @Create_By		varchar(20)
as

declare @Create_Dtm	as datetime
select @Create_Dtm = getdate()

INSERT INTO [dbo].[UserRole]
           ([User_ID]
           ,[Role_Type]
           ,[Scheme_Code]
           ,[Create_By]
           ,[Create_Dtm]
           ,[Update_By]
           ,[Update_Dtm])
     VALUES
           (@User_ID
           ,@Role_Type
           ,@scheme_code
           ,@Create_By
           ,@Create_Dtm
           ,@Create_By
           ,@Create_Dtm)

GO

GRANT EXECUTE ON [dbo].[proc_UserRole_add] TO HCVU
GO
