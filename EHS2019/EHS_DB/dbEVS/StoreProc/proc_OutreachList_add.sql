
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_OutreachList_add]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_OutreachList_add];
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
-- Description:	Add Outreach List
-- =============================================
CREATE PROCEDURE [dbo].[proc_OutreachList_add] @Outreach_code     VARCHAR(10), 
                                               @Type              VARCHAR(5), 
                                               @Outreach_name_Eng VARCHAR(255), 
                                               @Outreach_name_Chi NVARCHAR(255), 
                                               @Address_Eng       VARCHAR(1000), 
                                               @Address_Chi       NVARCHAR(255), 
                                               @Record_Status     CHAR(1), 
                                               @UserID            VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;

        INSERT INTO [dbo].[OutreachList]
               ([Outreach_code], 
                [Type], 
                [Outreach_Name_Eng], 
                [Outreach_Name_Chi], 
                [Address_Eng], 
                [Address_Chi], 
                [Record_Status], 
                [Create_By], 
                [Create_Dtm], 
                [Update_By], 
                [Update_Dtm]
               )
        VALUES
              (@Outreach_code, 
               @Type, 
               @Outreach_name_Eng, 
               @Outreach_name_Chi, 
               @Address_Eng, 
               @Address_Chi, 
               @Record_Status, 
               @UserID, 
               GETDATE(), 
               @UserID, 
               GETDATE()
               );
    END;

GO

GRANT EXECUTE ON [dbo].[proc_OutreachList_add] TO HCVU;
GO