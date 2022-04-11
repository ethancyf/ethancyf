IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineBrandLotDetail_getAll]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineBrandLotDetail_getAll];
    END;
GO   

-- =============================================  
-- Modification History  
-- Modified by:		Chris YIM 
-- Modified date:   22 Mar 2022
-- CR No.:			CRE20-023-82
-- Description:     Add mapping on brand name for QR Code use
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Nichole Ip
-- Modified date:   01 Apr 2021
-- CR No.:     CRe20-023 
-- Description: Vaccine lot detail contains a field called lot_assign_Status for control the display on lot management
-- =============================================
-- =============================================  
-- Modification History  
-- Created by:  Raiman Chong  
-- Created date: 02 Mar 2021  
-- CR No.:   CRE20-0023  
-- Description: Get all lot and brand detail for batch assign function brand and lot selection
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_COVID19VaccineBrandLotDetail_getAll]  
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
  
 SELECT  
  VLD.[Vaccine_Lot_No],  
  VLD.[Brand_ID],  
  VBD.[Brand_Name],  
  [Brand_Name_Chi] =   
   CASE   
    WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name]  
    WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name]  
    ELSE VBD.[Brand_Name_Chi]  
   END,  
   
  VBD.[Brand_Trade_Name],  
  [Brand_Trade_Name_Chi] =   
   CASE   
    WHEN VBD.[Brand_Trade_Name_Chi] IS NULL THEN VBD.[Brand_Trade_Name]  
    WHEN VBD.[Brand_Trade_Name_Chi] = '' THEN VBD.[Brand_Trade_Name]  
    ELSE VBD.[Brand_Trade_Name_Chi]  
   END,  
  VBD.[Brand_Printout_Name],  
  [Brand_Printout_Name_Chi] =   
   CASE   
    WHEN VBD.[Brand_Printout_Name_Chi] IS NULL THEN VBD.[Brand_Printout_Name]  
    WHEN VBD.[Brand_Printout_Name_Chi] = '' THEN VBD.[Brand_Printout_Name]  
    ELSE VBD.[Brand_Printout_Name_Chi]  
   END,  
  VBD.[Brand_Printout_Vaccine_Code],
  VBD.[HK_Reg_No],  
  VBD.[Manufacturer],  
  VBD.[Vaccination_Window_Min],  
  VBD.[Vaccination_Window_Max],  
  VLD.[Expiry_Date] , 
  VLD.[New_Record_Status],
  VLD.[Record_Status],
  VLD.[Request_Type],
  VLD.[Lot_Assign_Status]
 FROM   
  [COVID19VaccineLotDetail] VLD WITH(NOLOCK)  
   INNER JOIN [COVID19VaccineBrandDetail] VBD WITH(NOLOCK)  
    ON VLD.[Brand_ID] = VBD.[Brand_ID]  
 ORDER BY   
  VLD.[Vaccine_Lot_No]  
  
END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBrandLotDetail_getAll] TO HCVU;
GO


GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBrandLotDetail_getAll] TO HCSP;
GO

