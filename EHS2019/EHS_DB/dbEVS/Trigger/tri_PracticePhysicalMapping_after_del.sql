IF EXISTS
         (
             SELECT *
             FROM sysobjects
             WHERE type = 'TR'
                   AND name = 'tri_PracticePhysicalMapping_after_del'
          )
    BEGIN
        DROP TRIGGER [dbo].[tri_PracticePhysicalMapping_after_del];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =========================================================================================
-- Modification History
-- CR No: CRE20-023
-- Modified by:	 Nichole Ip
-- Modified date: 22 Mar 2021
-- Description:	  Trigger a delete statement on PracticePhysicalMapping
-- =========================================================================================

CREATE TRIGGER [dbo].[tri_PracticePhysicalMapping_after_del] ON [dbo].[PracticePhysicalMapping]
AFTER DELETE
AS
     BEGIN

         SET NOCOUNT ON;

         INSERT INTO PracticePhysicalMappingLOG
                (System_Dtm, 
				 [Action],
                 SP_ID, 
                 Practice_Display_Seq, 
                 Practice_Map_ID, 
                 Create_By, 
                 Create_Dtm
                )
         SELECT GETDATE(), 
                'D', 
                SP_ID, 
                Practice_Display_Seq,
				Practice_Map_ID,
                Create_By, 
                Create_Dtm
         FROM deleted;
     END;