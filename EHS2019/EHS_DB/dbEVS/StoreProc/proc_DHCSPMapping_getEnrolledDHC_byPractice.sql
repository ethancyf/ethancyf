
IF EXISTS
(
    SELECT 1
    FROM Sys.Objects
    WHERE [name] = 'proc_DHCSPMapping_getEnrolledDHC_byPractice'
          AND [type] = 'P'
)
    BEGIN
        DROP PROCEDURE proc_DHCSPMapping_getEnrolledDHC_byPractice;
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
-- Modified date:	07 Jun 2021
-- Description:		1. Extract Enrolled DHC for Practice
-- =============================================

CREATE PROCEDURE [dbo].[proc_DHCSPMapping_getEnrolledDHC_byPractice] @SP_ID CHAR(8) 
--  @Profession_Seq SMALLINT = NULL
AS
    BEGIN
        -- ============================================================
        -- Declaration
        -- ============================================================
        DECLARE @l_SP_ID CHAR(8);
        DECLARE @l_Professional_Seq SMALLINT;
        DECLARE @NSPbySP TABLE
        (Registration_Code     VARCHAR(15),
		 Service_Category_Code CHAR(5),
		 SP_ID                 CHAR(8), 
         Professional_Seq      varchar(2), 
         District_Board        VARCHAR(MAX)
        );
        -- ============================================================
        -- Validation
        -- ============================================================
        -- ============================================================
        -- Initialization
        -- ============================================================
        SET @l_SP_ID = @SP_ID;
        --SET @l_Professional_Seq = @Profession_Seq;
        INSERT INTO @NSPbySP
               SELECT DISTINCT 
                      DSP.Registration_Code, 
                      DSP.Service_Category_Code, 
                      PL.SP_ID, 
                      PL.Professional_Seq, 
                      LTRIM(RTRIM(DB.district_board)) AS District_Board
               FROM DHCSPMapping DSP WITH(NOLOCK)
                    INNER JOIN Professional PL WITH(NOLOCK) ON DSP.Registration_Code = PL.Registration_Code
                                                               AND DSP.Service_Category_Code = PL.Service_Category_Code
                                                               -- AND PL.Record_Status = 'A'
                                                               AND PL.SP_ID = @l_SP_ID
                    --AND (@l_Professional_Seq IS NULL
                    --     OR PL.Professional_Seq = @l_Professional_Seq)
                    INNER JOIN DistrictBoard DB WITH(NOLOCK) ON DB.DHC_District_Code = DSP.District_Code
                    INNER JOIN Practice P WITH(NOLOCK) ON P.SP_ID = PL.SP_ID COLLATE DATABASE_DEFAULT
                                                          AND P.Professional_Seq = PL.Professional_Seq;

        -- ============================================================
        -- Return results
        -- ============================================================

        SELECT Registration_Code, 
               Service_Category_Code, 
               SP_ID, 
               Professional_Seq, 
               STUFF(
        (
            SELECT ', ' + CAST([district_board] AS VARCHAR(MAX))
            FROM @NSPbySP
            WHERE(Registration_Code = TMP.Registration_Code)
                 AND (Service_Category_Code = TMP.Service_Category_Code)
            ORDER BY district_board FOR XML PATH('')
        ), 1, 1, '') AS DistrictName
        FROM @NSPbySP TMP
        GROUP BY Registration_Code, 
                 Service_Category_Code, 
                 SP_ID, 
                 Professional_Seq;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_DHCSPMapping_getEnrolledDHC_byPractice] TO HCVU;
GO