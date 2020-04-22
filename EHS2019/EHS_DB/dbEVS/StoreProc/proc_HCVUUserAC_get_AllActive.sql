IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_get_AllActive]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserAC_get_AllActive]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark YIP
-- Create date:		02-07-2008
-- Description:		Get All Active HCVUUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCVUUserAC_get_AllActive]
as
BEGIN
-- =============================================
-- Return results
-- =============================================
SELECT [User_ID]         
  FROM HCVUUserAC
where Suspended is null


END
GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserAC_get_AllActive] TO HCVU
GO
