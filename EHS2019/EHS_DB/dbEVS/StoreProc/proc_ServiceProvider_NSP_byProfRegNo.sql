IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_NSP_byProfRegNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ServiceProvider_NSP_byProfRegNo];
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
-- Modified date:	20 JUL 2020
-- Description:		1. Retrieve NSP by Registration code + service category code
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_NSP_byProfRegNo] 
	@ProfCode VARCHAR(20), 
	@RegNo    VARCHAR(20)
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
        SELECT *
        FROM DHCSPMapping
        WHERE Service_Category_Code = @ProfCode
              AND Registration_Code = @RegNo;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_ServiceProvider_NSP_byProfRegNo] TO WSEXT; 
GO