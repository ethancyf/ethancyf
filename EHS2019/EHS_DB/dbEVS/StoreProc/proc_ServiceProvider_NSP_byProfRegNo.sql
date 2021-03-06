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
-- CR No.:			CRE20-006-2 
-- Modified by:		Nichole Ip
-- Modified date:	25 Aug 2021
-- Description:		1. Add with(nolock) on table
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-006 
-- Created by:		Nichole Ip
-- Created date:	20 JUL 2020
-- Description:		1. Retrieve NSP by Registration code + service category code
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_NSP_byProfRegNo] 
	@ProfCode VARCHAR(20), 
	@RegNo    VARCHAR(20)
AS
    BEGIN
		SET NOCOUNT ON;
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
        SELECT Service_Category_Code,Registration_Code,District_Code
        FROM DHCSPMapping  WITH(NOLOCK) 
        WHERE Service_Category_Code = @ProfCode
              AND Registration_Code = @RegNo;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_ServiceProvider_NSP_byProfRegNo] TO WSEXT; 
GO