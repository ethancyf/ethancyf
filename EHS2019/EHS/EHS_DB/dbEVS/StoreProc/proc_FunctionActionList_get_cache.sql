 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FunctionActionList_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FunctionActionList_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- Modified by:	Koala Cheng
-- Modified date:	12 Jan 2011
-- Description:	CRE11-004 Audit Log Enhancement.
-- =============================================

CREATE  procedure [dbo].[proc_FunctionActionList_get_cache]
AS

-- =============================================
-- Return results
-- =============================================

SELECT [Function_Code]
      ,[Log_ID]
      ,[Description]
      ,[Record_Status]
      ,[Is_Log_EHA_Info]
      ,[Is_Log_EHA_DocInfo]
      ,[Is_Log_SPID]
      ,[Is_Log_SPHKIC]
FROM 
		[dbo].[FunctionActionList]
--WHERE
--	[Record_Status] = 'A'
	
GO

GRANT EXECUTE ON [dbo].[proc_FunctionActionList_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_FunctionActionList_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_FunctionActionList_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_FunctionActionList_get_cache] TO WSINT
GO

GRANT EXECUTE ON [dbo].[proc_FunctionActionList_get_cache] TO WSEXT
GO