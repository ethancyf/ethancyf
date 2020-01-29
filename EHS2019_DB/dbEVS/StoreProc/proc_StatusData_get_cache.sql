IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StatusData_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StatusData_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Author:		Clark Yip
-- Create date: 28 April 2008
-- Description:	Retrieve the status to cache
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_StatusData_get_cache]
	
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

	SELECT  ENUM_CLASS, COLUMN_NAME, STATUS_VALUE, STATUS_DESCRIPTION, RECORD_STATUS, EXPIRY_DTM, STATUS_DESCRIPTION_CHI, Status_Description_CN, DISPLAY_ORDER
	FROM	dbo.StatusData
	WHERE	Record_status='A' and 
	(Expiry_dtm is null OR (Expiry_dtm is not null AND Expiry_dtm <= {fn now()}))
	ORDER BY COLUMN_NAME ASC, DISPLAY_ORDER ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_StatusData_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StatusData_get_cache] TO HCVU
GO
