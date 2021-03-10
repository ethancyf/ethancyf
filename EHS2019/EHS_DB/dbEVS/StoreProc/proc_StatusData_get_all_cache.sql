IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StatusData_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StatusData_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:		
-- CR No.:				
-- Description:			
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	6 March 2021
-- CR No.:			CRE20-023-09
-- Description:		Immu Record
-- =============================================
CREATE PROCEDURE [dbo].[proc_StatusData_get_all_cache]
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

	SELECT  
		ENUM_CLASS, 
		COLUMN_NAME, 
		STATUS_VALUE, 
		STATUS_DESCRIPTION, 
		RECORD_STATUS, 
		Effective_Dtm, 
		EXPIRY_DTM, 
		STATUS_DESCRIPTION_CHI, 
		Status_Description_CN, 
		DISPLAY_ORDER
	FROM	
		StatusData
	ORDER BY 
		COLUMN_NAME ASC, 
		DISPLAY_ORDER ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_StatusData_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StatusData_get_all_cache] TO HCVU
GO
