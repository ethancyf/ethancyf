
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeItem_get_all_cache]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_SubsidizeItem_get_all_cache];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:			Martin Tang
-- Create date:		09 Sep 2020
-- CR No.:			CRE20-003 
-- Description:		Break Down of Monthly Statement by Schools
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeItem_get_all_cache]
AS
    BEGIN

        -- =============================================
        -- Declaration
        -- =============================================
        -- =============================================
        -- Initialization
        -- =============================================
        -- =============================================
        -- Return results
        -- =============================================

        SELECT si.Subsidize_Item_Code, 
               si.Subsidize_item_Display_Code, 
               si.Subsidize_Type, 
               si.Display_Seq
        FROM SubsidizeItem AS si
        ORDER BY si.Display_Seq;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItem_get_all_cache] TO HCVU;
GO