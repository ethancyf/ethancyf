
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_delisted_byProfRegNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ServiceProvider_delisted_byProfRegNo];
    END;
GO

/****** Object:  StoredProcedure [dbo].[proc_ServiceProvider_delisted_byProfRegNo]    Script Date: 20/07/2020 15:32:04 ******/

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
-- Description:		1. Check any Professional registration no. is suspended/delisted in eHS(S)
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_delisted_byProfRegNo] 
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
        
        SELECT SP_ID
        FROM DHCSPMapping DHCM WITH(NOLOCK) 
             INNER JOIN Professional PRO  WITH(NOLOCK) ON DHCM.Registration_Code = PRO.Registration_Code
                                            AND DHCM.Service_Category_Code = PRO.Service_Category_Code
        WHERE PRO.Record_Status IN('D', 'S')
             AND DHCM.Registration_Code = @RegNo
             AND DHCM.Service_Category_Code = @ProfCode;
    END;
GO
 
GRANT EXECUTE ON [dbo].[proc_ServiceProvider_delisted_byProfRegNo] TO WSEXT; 
GO