IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StaticData_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StaticData_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	8 Jul 2020
-- CR No.:			CRE19-022 (Inspection Module)
-- Description:		Return [Display_Order]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 30 Apr 2008
-- Description:	Retrieve data from StaticData Table
-- =============================================

CREATE PROCEDURE [dbo].[proc_StaticData_get_cache]
	
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

	SELECT Column_Name, Item_No, Data_Value, Data_Value_Chi, Data_Value_CN, Display_Order
	FROM dbo.StaticData
	WHERE Record_Status = 'A'
	ORDER BY Display_Order
END
GO

GRANT EXECUTE ON [dbo].[proc_StaticData_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_StaticData_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StaticData_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StaticData_get_cache] TO WSEXT
Go