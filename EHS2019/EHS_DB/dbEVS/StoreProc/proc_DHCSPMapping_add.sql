IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_DHCSPMapping_add]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    DROP PROCEDURE [dbo].[proc_DHCSPMapping_add];
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
-- Modified date:	16 JUL 2020
-- Description:		1. To add eHS(S) service provider as a DHC NSP in eHS(S), it supports 
--                     to upload all district NSP or particular district NSP
-- =============================================
 
CREATE PROCEDURE [dbo].[proc_DHCSPMapping_add] @ProfCode         VARCHAR(20), 
                                               @ProgRegNo        VARCHAR(20), 
                                               @ProfDistrictCode VARCHAR(5)
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
       
        INSERT INTO DHCSPMapping
					(
					 [Service_Category_Code], 
					 [Registration_Code], 
					 [District_Code], 
					 create_DTM
					)
			VALUES  (
					 @ProfCode, 
					 @ProgRegNo, 
					 @ProfDistrictCode, 
					 GETDATE()
					);
    END;
GO
 
GRANT EXECUTE ON [dbo].[proc_DHCSPMapping_add] TO WSEXT; 
GO