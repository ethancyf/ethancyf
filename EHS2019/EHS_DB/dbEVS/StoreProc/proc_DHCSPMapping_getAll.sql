
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_DHCSPMapping_getAll]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_DHCSPMapping_getAll];
    END;
GO

 SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			 
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-006 
-- Modified by:		Nichole Ip
-- Modified date:	03 May 2021
-- Description:		1. Retrieve DHC Mapping informaion 
-- =============================================

CREATE PROCEDURE [dbo].[proc_DHCSPMapping_getAll] 

AS
    BEGIN
  		-- ============================================================
        -- Declaration
        -- ============================================================
		 -- ============================================================
        -- Validation
        -- ============================================================
        -- ============================================================
        -- Initialization
        -- ============================================================ 
        -- ============================================================
        -- Return results
        -- ============================================================
        SELECT	Service_Category_Code,
				Registration_Code ,
				District_Code  
        FROM	DHCSPMapping

    END;
GO
 
GRANT EXECUTE ON [dbo].[proc_DHCSPMapping_getAll] TO WSEXT; 
GO