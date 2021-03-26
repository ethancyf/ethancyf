
IF EXISTS
         (
             SELECT *
             FROM sysobjects
             WHERE type = 'TR'
                   AND name = 'tri_COVID19VaccineLotMapping_after_upd'
          )
    BEGIN
        DROP TRIGGER [dbo].[tri_COVID19VaccineLotMapping_after_upd];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No: CRE20-023
-- Modified by:	 Nichole Ip
-- Modified date: 05 Mar 2021
-- Description:	  Trigger an insert and update statement on COVID19VaccineLotMapping
-- =============================================

CREATE TRIGGER [dbo].[tri_COVID19VaccineLotMapping_after_upd] ON [dbo].[COVID19VaccineLotMapping]
AFTER INSERT, UPDATE
AS
     BEGIN

         SET NOCOUNT ON;

         INSERT INTO COVID19VaccineLotMappingHistory
                (System_Dtm, 
                 Vaccine_Lot_ID, 
                 Vaccine_Lot_No, 
                 Service_Type, 
                 Centre_ID, 
                 Booth, 
                 RCH_Code, 
                 SP_ID, 
                 Practice_Display_Seq, 
                 Service_Period_From, 
                 Service_Period_To, 
                 Record_Status, 
                 Create_By, 
                 Create_Dtm, 
                 Update_By, 
                 Update_Dtm
                )
         SELECT GETDATE(), 
                Vaccine_Lot_ID, 
                Vaccine_Lot_No, 
                Service_Type, 
                Centre_ID, 
                Booth, 
                RCH_Code, 
                SP_ID, 
                Practice_Display_Seq, 
                Service_Period_From, 
                Service_Period_To, 
                Record_Status, 
                Create_By, 
                Create_Dtm, 
                Update_By, 
                Update_Dtm
         FROM inserted;
     END;