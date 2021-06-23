IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_Valid_byProfRegNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ServiceProvider_Valid_byProfRegNo];
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
-- Description:		1. Pass the Registration code + service category code to check the validation of SP
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_Valid_byProfRegNo] 
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
        FROM ServiceProvider SP WITH(NOLOCK) 
             INNER JOIN Professional PF  WITH(NOLOCK) ON SP.SP_ID = PF.SP_ID
        WHERE SP.Record_status = 'A'
              AND PF.Record_Status = 'A'
              AND Service_Category_Code = @ProfCode
              AND Registration_Code = @RegNo;
    END;
GO
 
GRANT EXECUTE ON [dbo].[proc_ServiceProvider_Valid_byProfRegNo] TO WSEXT; 
GO