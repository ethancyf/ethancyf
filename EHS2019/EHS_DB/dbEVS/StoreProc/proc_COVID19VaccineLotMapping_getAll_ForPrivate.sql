IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_getAll_ForPrivate]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getAll_ForPrivate]
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================  
-- Modification History  
-- Modified by:    Nichole Ip
-- Modified date:   25 Mar 2021
-- CR No.:     CRE20-023 
-- Description:    Handle the vaccine lot removed or pass away record for back office entry
-- =============================================  
 -- =============================================  
-- Modification History  
-- Created by:  Chris YIM  
-- Created date: 16 Dec 2020  
-- CR No.:   CRE20-00XX  
-- Description:  Immu Record  
-- =============================================  
  
-- For Scheme (VSS)  
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getAll_ForPrivate]  
 @SP_ID CHAR(8)  = NULL,  
 @Practice_Display_Seq SMALLINT = NULL 
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
  VLM.[Vaccine_Lot_ID],  
  VLD.[Vaccine_Lot_No],  
  VBD.[Brand_ID],  
  VBD.[Brand_Name],  
  [Brand_Name_Chi] =   
   CASE   
    WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name]  
    WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name]  
    ELSE VBD.[Brand_Name_Chi]  
   END,  
  [Display_Name] = VBD.[Brand_Name] + ' - ' + VLD.[Vaccine_Lot_No],  
  [Display_Name_Chi] =   
   CASE   
    WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name] + ' - ' + VLD.[Vaccine_Lot_No]  
    WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name] + ' - ' + VLD.[Vaccine_Lot_No]  
    ELSE VBD.[Brand_Name_Chi] + ' - ' + VLD.[Vaccine_Lot_No]  
   END,  
  VBD.[Brand_Trade_Name],  
  [Brand_Trade_Name_Chi] =   
   CASE   
    WHEN VBD.[Brand_Trade_Name_Chi] IS NULL THEN VBD.[Brand_Trade_Name]  
    WHEN VBD.[Brand_Trade_Name_Chi] = '' THEN VBD.[Brand_Trade_Name]  
    ELSE VBD.[Brand_Trade_Name_Chi]  
   END,  
  VLM.[SP_ID],  
  VLM.[Practice_Display_Seq],  
  VLM.[Record_Status],  
  VLM.[Service_Period_From],  
  -- VLM.[Service_Period_To],  
  [Service_Period_To] =   
   CASE  
    WHEN VLM.[Service_Period_To] IS NULL THEN VLD.[Expiry_Date]  
    WHEN VLD.[Expiry_Date] < VLM.[Service_Period_To] THEN VLD.[Expiry_Date]  
    ELSE VLM.[Service_Period_To]  
   END,  
  VLD.[Expiry_Date]  
 FROM   
  [COVID19VaccineLotMapping] VLM WITH(NOLOCK)  
   INNER JOIN [COVID19VaccineLotDetail] VLD WITH(NOLOCK)  
    ON VLM.[Vaccine_Lot_No] = VLD.[Vaccine_Lot_No]  
   INNER JOIN [COVID19VaccineBrandDetail] VBD WITH(NOLOCK)  
    ON VLD.[Brand_ID] = VBD.[Brand_ID]  
 WHERE  
  --VLM.Record_Status ='A' and
  VLM.[Service_Type] = 'PRIVATE'  
 ORDER BY   
  VBD.[Brand_Trade_Name],  
  VLM.[Vaccine_Lot_No]  
END  
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getAll_ForPrivate] TO HCSP
GO 

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getAll_ForPrivate] TO HCVU
GO