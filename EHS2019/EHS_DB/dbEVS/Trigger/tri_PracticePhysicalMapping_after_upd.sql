IF EXISTS
         (
             SELECT *
             FROM sysobjects
             WHERE type = 'TR'
                   AND name = 'tri_PracticePhysicalMapping_after_upd'
          )
    BEGIN
        DROP TRIGGER [dbo].[tri_PracticePhysicalMapping_after_upd];
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
-- Description:	  Trigger an insert and update statement on PracticePhysicalMapping
-- =========================================================================================

CREATE TRIGGER [dbo].[tri_PracticePhysicalMapping_after_upd] ON [dbo].[PracticePhysicalMapping]
AFTER INSERT, UPDATE
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
                'I', 
                SP_ID, 
                Practice_Display_Seq,
				Practice_Map_ID,
                Create_By, 
                Create_Dtm
         FROM inserted;
     END;