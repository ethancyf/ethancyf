
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_OutreachList_update]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_OutreachList_update];
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
-- Description:	Update Outreach Home List
-- =============================================
CREATE PROCEDURE [dbo].[proc_OutreachList_update] @Outreach_code     VARCHAR(10), 
                                                  @Type              VARCHAR(5), 
                                                  @Outreach_name_Eng VARCHAR(255), 
                                                  @Outreach_name_Chi NVARCHAR(255), 
                                                  @Address_Eng       VARCHAR(1000), 
                                                  @Address_Chi       NVARCHAR(255), 
                                                  @Record_Status     CHAR(1), 
                                                  @Update_By         VARCHAR(20), 
                                                  @TSMP              TIMESTAMP
AS
    BEGIN
        SET NOCOUNT ON;
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
        UPDATE [dbo].[OutreachList]
          SET 
              [Type] = @Type, 
              [Outreach_name_Eng] = @Outreach_name_Eng, 
              [Outreach_name_Chi] = @Outreach_name_Chi, 
              [Address_Eng] = @Address_Eng, 
              [Address_Chi] = @Address_Chi, 
              [Record_Status] = @Record_Status, 
              [Update_By] = @Update_By, 
              [Update_Dtm] = GETDATE()
        WHERE [Outreach_code] = @Outreach_code
              AND [TSMP] = @TSMP;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_OutreachList_update] TO HCVU;
GO