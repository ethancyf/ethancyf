IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_IVRSConnectionMapping_upd_ActionDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_IVRSConnectionMapping_upd_ActionDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Cheung
-- Create date:		25-11-2008
-- Description:		Update Last_Action_Dtm at IVRSConnetionMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_IVRSConnectionMapping_upd_ActionDtm]

@SP_ID char(8),
@CallUniqueID varchar(40)

AS

-- =============================================
-- Declaration
-- =============================================

-- =============================================
-- Initialization
-- =============================================
BEGIN
	Update [dbo].[IVRSConnectionMapping]
	Set 
		Last_Action_Dtm = getdate()
	Where
		SP_ID=@SP_ID and
		CallUniqueID= @CallUniqueID
END
GO

GRANT EXECUTE ON [dbo].[proc_IVRSConnectionMapping_upd_ActionDtm] TO HCSP
GO
