
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
-- CR No.:	INT21-0010		 
-- Modified by:		Nichole IP
-- Modified date:	20 Jun 2021
-- Description:		1. Fix DHC district selection issue
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-006 
-- Modified by:		Nichole Ip
-- Modified date:	06 May 2021
-- Description:		1. Refine District Structure for DHC
-- =============================================

CREATE PROCEDURE [dbo].[proc_DistrictBoard_get_bySPID] @SP_ID      CHAR(8), 
                                                       @PracticeNo SMALLINT
AS
    BEGIN
        -- ============================================================
        -- Declaration
        -- ============================================================
        DECLARE @l_SP_ID CHAR(8);
		DECLARE @l_PracticeNo SMALLINT;
        -- ============================================================
        -- Validation
        -- ============================================================
        -- ============================================================
        -- Initialization
        -- ============================================================
        SET @l_SP_ID = @SP_ID;
		SET @l_PracticeNo = @PracticeNo;
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

        SELECT DISTINCT 
               DB.district_board AS DistrictBoard, 
               DB.district_board_chi AS DistrictBoardChi, 
               RTRIM(DB.district_board_shortname_SD) AS DistrictBoardShortname, 
               DB.area_name AS AreaName, 
               DB.area_name_chi AS AreaNameChi, 
               RTRIM(DB.DHC_District_Code) AS DHC_DistrictCode
        FROM DHCSPMapping DSP WITH(NOLOCK)
             INNER JOIN Professional PL WITH(NOLOCK) ON DSP.Registration_Code = PL.Registration_Code
                                                        AND DSP.Service_Category_Code = PL.Service_Category_Code
             INNER JOIN Practice P WITH(NOLOCK) ON P.Professional_Seq = PL.Professional_Seq
                                                   AND P.Display_Seq = @l_PracticeNo
                                                   AND P.SP_ID = PL.SP_ID
             --INNER JOIN CodeMapping CM ON DSP.District_Code = CM.Code_Source
             INNER JOIN DistrictBoard DB WITH(NOLOCK) ON DB.DHC_District_Code = DSP.District_Code
        WHERE PL.SP_id = @l_SP_ID
        ORDER BY DB.district_board;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_bySPID] TO HCSP;
GO
GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_bySPID] TO HCVU;
GO