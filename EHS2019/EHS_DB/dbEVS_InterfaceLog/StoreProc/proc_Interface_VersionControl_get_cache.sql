  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Interface_VersionControl_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Interface_VersionControl_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy TSE
-- Create date: 22 Sep 2011
-- CR No.:		CRE11-024-02 (Enhancement on HCVS Extension Part 2)
-- Description:	Retrieve data from VersionControl Table
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_Interface_VersionControl_get_cache]
	
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

	SELECT Logical_Name, Physical_Name, Description, Effective_Dtm
	FROM dbo.VersionControl
	ORDER BY Logical_Name ASC, Effective_Dtm DESC
END

GO

GRANT EXECUTE ON [dbo].[proc_Interface_VersionControl_get_cache] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_Interface_VersionControl_get_cache] TO HCSP
GO