
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_OutreachList_delete_byCode]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_OutreachList_delete_byCode];
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
-- Description:	Delete Outreach record
-- =============================================
CREATE PROCEDURE [dbo].[proc_OutreachList_delete_byCode] @Outreach_code VARCHAR(10), 
                                                         @TSMP          TIMESTAMP
AS
    BEGIN
        SET NOCOUNT ON;
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

        IF
          (
              SELECT TSMP
              FROM [dbo].[OutreachList]
              WHERE Outreach_code = @Outreach_code
           ) <> @TSMP
            BEGIN
                RAISERROR('00011', 16, 1);
                RETURN @@error;
            END;

        DELETE FROM OutreachList
        WHERE Outreach_code = @Outreach_code
              AND TSMP = @TSMP;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_OutreachList_delete_byCode] TO HCVU;
GO