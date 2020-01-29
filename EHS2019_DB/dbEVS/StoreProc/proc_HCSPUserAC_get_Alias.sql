IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_Alias]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_Alias]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark YIP
-- Create date:		10-06-2008
-- Description:		Get HCSPUserAC Alias
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSPUserAC_get_Alias]
	@alias varchar(20)
AS
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
	SELECT count(1)
	From HCSPUserAC
	where [Alias_Account] = @alias OR [SP_ID] = @alias
			
END  
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_Alias] TO HCSP
GO
