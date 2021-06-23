
IF EXISTS
(
    SELECT 1
    FROM Sys.Objects
    WHERE [name] = 'proc_DistrictBoard_get_bySPID'
          AND [type] = 'P'
)
    BEGIN
        DROP PROCEDURE proc_DistrictBoard_get_bySPID;
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
-- Modified date:	06 May 2021
-- Description:		1. Refine District Structure for DHC
-- =============================================

CREATE PROCEDURE [dbo].[proc_DistrictBoard_get_bySPID] 
	@SP_ID CHAR(8)
AS
    BEGIN
        -- ============================================================
        -- Declaration
        -- ============================================================
        DECLARE @l_SP_ID CHAR(8);
        -- ============================================================
        -- Validation
        -- ============================================================
        -- ============================================================
        -- Initialization
        -- ============================================================
        SET @l_SP_ID = @SP_ID; 

        -- ============================================================
        -- Return results
        -- ============================================================

        --SELECT Distinct DB.district_board as DistrictBoard, 
        --       DB.district_board_chi as DistrictBoardChi, 
        --       RTRIM(DB.district_board_shortname_SD) as DistrictBoardShortname, 
        --       DB.area_name as AreaName, 
        --       DB.area_name_chi as AreaNameChi
        --FROM DHCSPMapping DSP
        --     INNER JOIN Professional PL ON DSP.Registration_Code = PL.Registration_Code
        --                                   AND DSP.Service_Category_Code = PL.Service_Category_Code
        --     INNER JOIN CodeMapping CM ON DSP.District_Code = CM.Code_Source
        --     INNER JOIN DistrictBoard DB ON DB.district_board_shortname_SD = CM.Code_Target
        --WHERE PL.SP_id = @l_SP_ID
        --ORDER BY DB.district_board;

		SELECT Distinct DB.district_board as DistrictBoard, 
               DB.district_board_chi as DistrictBoardChi, 
               RTRIM(DB.district_board_shortname_SD) as DistrictBoardShortname, 
               DB.area_name as AreaName, 
               DB.area_name_chi as AreaNameChi,
			   RTRIM(DB.DHC_District_Code) as DHC_DistrictCode
        FROM DHCSPMapping DSP  WITH (NOLOCK) 
             INNER JOIN Professional PL  WITH (NOLOCK)  ON DSP.Registration_Code = PL.Registration_Code
                                           AND DSP.Service_Category_Code = PL.Service_Category_Code
             --INNER JOIN CodeMapping CM ON DSP.District_Code = CM.Code_Source
             INNER JOIN DistrictBoard DB WITH (NOLOCK)  ON DB.DHC_District_Code  = DSP.District_Code 
        WHERE PL.SP_id = @l_SP_ID
        ORDER BY DB.district_board;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_bySPID] TO HCSP;
GO

GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_bySPID] TO HCVU;
GO