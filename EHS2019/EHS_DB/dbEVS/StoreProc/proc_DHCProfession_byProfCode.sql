
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_DHCProfession_byProfCode]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_DHCProfession_byProfCode];
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
-- Modified date:	21 JUL 2020
-- Description:		1. Check the profession from the DHC 
-- =============================================

CREATE PROCEDURE [dbo].[proc_DHCProfession_byProfCode]  
	@ProfCode CHAR(3)

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

        SELECT CHARINDEX(@ProfCode, Parm_Value1) AS isProf
        FROM SystemParameters
        WHERE Parameter_Name = 'DHC_Profession';
    END;  
GO
 
GRANT EXECUTE ON [dbo].[proc_DHCProfession_byProfCode] TO WSEXT; 
GO