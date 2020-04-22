IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_UserRole_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_UserRole_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		17-07-2008
-- Description:		Get UserRole
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   26 May 2009
-- Description:	    Also get the scheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   13 Jul 2009
-- Description:	    Change the scheme code to MScheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   5 Aug 2009
-- Description:	    Change [MScheme_Code] back to [Scheme_Code]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_UserRole_get]
	@User_ID	varchar(20)
AS BEGIN

	SELECT	[User_ID],
			[Role_Type],
			[Scheme_Code]
	FROM	[UserRole]
	WHERE	[User_ID] = @User_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_UserRole_get] TO HCVU
GO
