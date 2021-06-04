
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_OutreachList_get_byORInfo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_OutreachList_get_byORInfo];
    END;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
-- =============================================
-- Author:		Martin Tang
-- CR No.:		CRE20-023
-- Create date: 20 May 2021
-- Description:	Search Outreach record
-- =============================================
--exec proc_OutreachList_get_byORInfo null,null, 's',null,1,1,0

CREATE PROCEDURE [dbo].[proc_OutreachList_get_byORInfo] @Outreach_code                VARCHAR(10), 
                                                        @Outreach_name                NVARCHAR(255), 
                                                        @Outreach_addr                NVARCHAR(255), 
                                                        @Outreach_stat                NVARCHAR(1), 
                                                        @result_limit_1st_enable      BIT, 
                                                        @result_limit_override_enable BIT, 
                                                        @override_result_limit        BIT
AS
    BEGIN
        SET NOCOUNT ON;

        -- =============================================
        -- Declaration
        -- =============================================
        DECLARE @rowcount INT;
        DECLARE @row_cnt_error VARCHAR(MAX);

        -- =============================================
        -- Initialization
        -- =============================================
        SET @Outreach_name = '%' + @Outreach_name + '%';
        SET @Outreach_addr = '%' + @Outreach_addr + '%';
        SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable, @result_limit_override_enable)) [Outreach_code], 
                                                                                                       [Type],
																									   [Outreach_Name_Eng], 
                                                                                                       [Outreach_Name_Chi], 
                                                                                                       [Address_Eng], 
                                                                                                       [Address_Chi], 
                                                                                                       [Record_Status], 
                                                                                                       [Create_By], 
                                                                                                       [Create_Dtm], 
                                                                                                       [Update_By], 
                                                                                                       [Update_Dtm], 
                                                                                                       [TSMP]
        INTO #tempOutreachList
        FROM OutreachList
        WHERE([Outreach_code] = @Outreach_code
              OR @Outreach_code IS NULL)
             AND ([Outreach_Name_Eng] LIKE @Outreach_name
                  OR [Outreach_Name_Chi] LIKE @Outreach_name)
             AND ([Address_Eng] LIKE @Outreach_addr
                  OR [Address_Chi] LIKE @Outreach_addr)
             AND ([Record_Status] = @Outreach_stat
                  OR @Outreach_stat IS NULL);

        -- =============================================    
        -- Max Row Checking  
        -- =============================================  
        BEGIN TRY
            SELECT @rowcount = COUNT(1)
            FROM #tempOutreachList;
            EXEC proc_CheckFeatureResultRowLimit 
                 @row_count = @rowcount, 
                 @result_limit_1st_enable = @result_limit_1st_enable, 
                 @result_limit_override_enable = @result_limit_override_enable, 
                 @override_result_limit = @override_result_limit;
        END TRY
        BEGIN CATCH
            SET @row_cnt_error = ERROR_MESSAGE();
            RAISERROR(@row_cnt_error, 16, 1);
            RETURN;
        END CATCH;

        -- =============================================  
        -- Return results  
        -- =============================================  
        SELECT [Outreach_code], 
               [Type], 
               [Outreach_Name_Eng], 
               [Outreach_Name_Chi], 
               [Address_Eng], 
               [Address_Chi], 
               [Record_Status], 
               [Create_By], 
               [Create_Dtm], 
               [Update_By], 
               [Update_Dtm], 
               [TSMP]
        FROM #tempOutreachList;
        DROP TABLE #tempOutreachList;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_OutreachList_get_byORInfo] TO HCVU;
GO