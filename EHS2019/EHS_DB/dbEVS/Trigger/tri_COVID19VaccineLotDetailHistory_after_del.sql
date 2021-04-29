
IF EXISTS
(
    SELECT *
    FROM sysobjects
    WHERE type = 'TR'
          AND name = 'tri_COVID19VaccineLotDetailHistory_after_del'
)
    BEGIN
        DROP TRIGGER [dbo].[tri_COVID19VaccineLotDetailHistory_after_del];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No: CRE20-023
-- Modified by:	 Nichole Ip
-- Modified date: 26 Mar 2021
-- Description:	  Trigger an insert and update statement on COVID19VaccineLotDetailHistory
-- =============================================

CREATE TRIGGER [dbo].[tri_COVID19VaccineLotDetailHistory_after_del] ON [dbo].[COVID19VaccineLotDetail]
AFTER	DELETE
AS
     BEGIN

         SET NOCOUNT ON;

         INSERT INTO COVID19VaccineLotDetailHistory
                (System_Dtm, 
                 Vaccine_Lot_No, 
                 Brand_ID, 
                 [Expiry_Date], 
                 New_Expiry_Date, 
                 Record_Status, 
                 Request_Type, 
                 Request_By, 
                 Request_Dtm, 
                 Approve_By, 
                 Approve_Dtm, 
                 Create_By, 
                 Create_Dtm, 
                 Update_By, 
                 Update_Dtm, 
                 New_Record_Status,
				 Lot_Assign_Status,
				 New_Lot_Assign_Status,
				 Reject_By,
				 Reject_Dtm,
				 Action_Type
                )
         SELECT GETDATE(), 
                Vaccine_Lot_No, 
                 Brand_ID, 
                 [Expiry_Date], 
                 New_Expiry_Date, 
                 Record_Status, 
                 Request_Type, 
                 Request_By, 
                 Request_Dtm, 
                 Approve_By, 
                 Approve_Dtm, 
                 Create_By, 
                 Create_Dtm, 
                 Update_By, 
                 Update_Dtm, 
                 New_Record_Status,
				 Lot_Assign_Status,
				 New_Lot_Assign_Status,
				 Reject_By ,
				 Reject_Dtm ,
				 'Deleted'
         FROM Deleted;
     END;