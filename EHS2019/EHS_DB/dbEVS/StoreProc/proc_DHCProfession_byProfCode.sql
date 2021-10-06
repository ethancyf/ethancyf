
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
 

-- =============================================
-- Modification History
-- CR No.:	CRE20-006-2		 
-- Modified by:		Nichole Ip
-- Modified date:	25 Aug 2021
-- Description:		1. proc_DHCProfession_byProfCode is Obsolete
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-006 
-- Modified by:		Nichole Ip
-- Modified date:	21 JUL 2020
-- Description:		1. Check the profession from the DHC 
-- =============================================

--CREATE PROCEDURE [dbo].[proc_DHCProfession_byProfCode]  
--	@ProfCode CHAR(3)

--AS
--    BEGIN
--		-- ============================================================
--        -- Declaration
--        -- ============================================================
--		 -- ============================================================
--        -- Validation
--        -- ============================================================
--        -- ============================================================
--        -- Initialization
--        -- ============================================================ 
--        -- ============================================================
--        -- Return results
--        -- ============================================================

--        SELECT CHARINDEX(@ProfCode, Parm_Value1) AS isProf
--        FROM SystemParameters
--        WHERE Parameter_Name = 'DHC_Profession';
--    END;  
--GO
 
--GRANT EXECUTE ON [dbo].[proc_DHCProfession_byProfCode] TO WSEXT; 
--GO