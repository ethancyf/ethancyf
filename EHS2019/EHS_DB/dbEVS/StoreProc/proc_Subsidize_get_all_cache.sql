
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_Subsidize_get_all_cache]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_Subsidize_get_all_cache];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:			Martin Tang
-- Create date:		07 Dec 2020
-- CR No.:			CRE20-015-10
-- Description:		Special Support Scheme (Performance Turning) Ref: proc_SubsidizeItem_get_bySubsidizeCode
-- =============================================
CREATE PROCEDURE [dbo].[proc_Subsidize_get_all_cache]
AS
    BEGIN
        -- =============================================
        -- Declaration
        -- ============================================= 
        -- =============================================
        -- Validation 
        -- =============================================
        -- =============================================
        -- Initialization
        -- =============================================
        -- =============================================
        -- Return results
        -- =============================================

        SELECT s.Subsidize_code, 
               s.subsidize_item_code, 
               s.display_code, 
               s.Display_Seq, 
               s.record_status AS Subsidize_Record_Status, 
               s.Legend_Desc AS [subsidize_item_desc], 
               s.Legend_Desc_Chi AS [subsidize_item_desc_chi], 
               s.Legend_Desc_CN AS [subsidize_item_desc_cn], 
               i.subsidize_type, 
               i.record_status AS SubsidizeItem_Record_Status, 
               s.Create_by AS Subsidize_Create_by, 
               s.Create_dtm AS Subsidize_Create_dtm, 
               s.Update_by AS Subsidize_Update_By, 
               s.Update_dtm AS Subsidize_Update_Dtm, 
               i.Create_by AS SubsidizeItem_Create_by, 
               i.Create_dtm AS SubsidizeItem_Create_Dtm, 
               i.Update_by AS SubsidizeItem_Update_by, 
               i.Update_dtm AS SubsidizeItem_Update_dtm
        FROM Subsidize AS s
             INNER JOIN SubsidizeItem AS i
             ON s.subsidize_item_code = i.subsidize_item_code
        ORDER BY s.Display_Seq;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_Subsidize_get_all_cache] TO HCSP, HCVU, HCPUBLIC, WSINT;
GO