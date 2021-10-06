IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_byProfRegNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ServiceProvider_get_byProfRegNo];
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
-- Created date:	17 JUL 2020
-- Description:		1. Retrieve Service Provider informaion by Registration code + service category code
-- =============================================
 
CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_byProfRegNo] 
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
        SELECT SP_ID, 
               Service_Category_Code, 
               Registration_Code, 
               EnrolledInEHS
        FROM
        (
            SELECT SP_ID, 
                   Service_Category_Code, 
                   Registration_Code, 
                   EnrolledInEHS,
                   CASE
                       WHEN EnrolledInEHS = 'Y' --SP(A)+Professional(A)+Practice(A)+HCVS Scheme(A)
                       THEN 1
                       ELSE CASE
                                WHEN EnrolledInEHS = 'I' --SP(A/S)+Professional(A/S)+Practice(S/D/NULL)+HCVS Scheme(S/D/NULL)
                                THEN 2
                                ELSE 3
                            END
                   END AS EnrolledResult, 
                   ROW_NUMBER() OVER(
                   ORDER BY CASE
                                WHEN EnrolledInEHS = 'Y'
                                THEN 1
                                ELSE CASE
                                         WHEN EnrolledInEHS = 'I'
                                         THEN 2
                                         ELSE 3
                                     END
                            END) AS RANK
            FROM
            (
                SELECT DISTINCT 
                       PRO.SP_ID, 
                       PRO.Service_Category_Code, 
                       PRO.Registration_Code,
                       CASE
                           WHEN SP.Record_Status = 'D'
                           THEN 'N'
                           ELSE CASE
                                    WHEN(PRO.Record_Status = 'A'
                                         AND PRA.Record_Status = 'A'
                                         AND PSI.Scheme_Code = 'HCVS'
                                         AND PSI.Record_Status = 'A')
                                    THEN 'Y'
                                    ELSE 'I'
                                END
                       END AS EnrolledInEHS
                FROM ServiceProvider SP WITH(NOLOCK) 
                     INNER JOIN Professional PRO  WITH(NOLOCK) ON SP.SP_ID = PRO.SP_ID
                     INNER JOIN Practice PRA  WITH(NOLOCK) ON PRA.SP_ID = PRO.SP_ID
                                                AND PRA.Professional_Seq = PRO.Professional_Seq
                     LEFT JOIN PracticeSchemeInfo PSI  WITH(NOLOCK) ON PSI.SP_ID = PRO.SP_ID
                                                         AND PSI.Practice_Display_Seq = PRA.Display_Seq
                WHERE

                /*PSI.Scheme_Code = 'HCVS' AND*/

                PRO.Registration_Code = @RegNo
                AND PRO.Service_Category_Code = @ProfCode
            ) z
        ) w
        WHERE RANK = 1
        UNION ALL
		-- SP Delisted  
        SELECT TOP 1 '' AS SP_ID, 
                     @ProfCode AS Registration_Code, 
                     @RegNo AS Registration_Code, 
                     'N' AS EnrolledInEHS
        FROM ServiceProvider WITH(NOLOCK) 
        WHERE NOT EXISTS
        (
            SELECT *
            FROM Professional WITH(NOLOCK) 
            WHERE Registration_Code = @RegNo
                  AND Service_Category_Code = @ProfCode
        )
        UNION ALL
		-- SP Not existed
        SELECT TOP 1 SP_ID AS SP_ID, 
                     @ProfCode AS Registration_Code, 
                     @RegNo AS Registration_Code, 
                     'N' AS EnrolledInEHS
        FROM Professional  WITH(NOLOCK) 
        WHERE NOT EXISTS
        (
            SELECT *
            FROM ServiceProvider WITH(NOLOCK) 
            WHERE SP_ID IN
            (
                SELECT SP_ID
                FROM Professional  WITH(NOLOCK) 
                WHERE Registration_Code = @RegNo
                      AND Service_Category_Code = @ProfCode
            )
        );
    END;
GO
 
GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byProfRegNo] TO WSEXT; 
GO