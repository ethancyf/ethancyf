IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_VaccineCentre_getAll]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_VaccineCentre_getAll];
    END;
GO   

-- =============================================  
-- Modification History  
-- Modified by:		Chris YIM
-- Modified date:   06 Apr 2022
-- CR No.:			CRE20-023-86		
-- Description:		Add column "Centre_Service_Type"
-- =============================================   
-- =============================================  
-- Modification History  
-- Created by:		Raiman Chong  
-- Created date:	29 Mar 2021  
-- CR No.:			CRE20-0023  
-- Description:		Get all Vaccine Centre for HCVU User Maintenance Function
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_VaccineCentre_getAll]  
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
 VC.Centre_ID,
 VC.Centre_Name,
 VC.Centre_Name_Chi,
 VC.Centre_Service_Type
 FROM   
  [VaccineCentre] VC WITH(NOLOCK)   
 ORDER BY   
  VC.[Centre_ID]  
  
END;
GO

GRANT EXECUTE ON [dbo].[proc_VaccineCentre_getAll] TO HCVU;
GO


GRANT EXECUTE ON [dbo].[proc_VaccineCentre_getAll] TO HCSP;
GO
