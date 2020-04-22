IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_upd_ForcePwdChange]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserAC_upd_ForcePwdChange]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Clark Yip
-- Create date:		08-12-2008
-- Description:		Update Force_Pwd_Change of HCVUUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCVUUserAC_upd_ForcePwdChange]
@User_ID	char(20), @Force_Pwd_Change char(1)
as


-- =============================================
-- Declaration
-- =============================================
select @User_ID = ltrim(rtrim(@User_ID))

-- =============================================
-- Update Transcation
-- =============================================
begin	
	update HCVUUserAC
	set Force_Pwd_Change=@Force_Pwd_Change		
	where User_ID = @User_ID
end


GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserAC_upd_ForcePwdChange] TO HCVU
GO
