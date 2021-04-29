IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineBrandDetail_getAll]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineBrandDetail_getAll];
    END;
GO   

-- =============================================  
-- Modification History  
-- Modified by:  Nichole Ip
-- Modified date:   08 Apr 2021
-- CR No.:     CRE20-023
-- Description:   this store proc no longer to use
-- =============================================  
-- =============================================  
-- Modification History  
-- Created by:  Raiman Chong  
-- Created date: 02 Mar 2021  
-- CR No.:   CRE20-0023  
-- Description:  Immu Record  
-- =============================================  
  
--CREATE PROCEDURE [dbo].[proc_COVID19VaccineBrandDetail_getAll]  
--AS  
--BEGIN  
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
  
 --SELECT  
 -- VBD.[Brand_ID],  
 -- VBD.[Brand_Name],  
 -- [Brand_Name_Chi] =   
 --  CASE   
 --   WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name]  
 --   WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name]  
 --   ELSE VBD.[Brand_Name_Chi]  
 --  END,  
   
 -- VBD.[Brand_Trade_Name],  
 -- [Brand_Trade_Name_Chi] =   
 --  CASE   
 --   WHEN VBD.[Brand_Trade_Name_Chi] IS NULL THEN VBD.[Brand_Trade_Name]  
 --   WHEN VBD.[Brand_Trade_Name_Chi] = '' THEN VBD.[Brand_Trade_Name]  
 --   ELSE VBD.[Brand_Trade_Name_Chi]  
 --  END,  
 -- VBD.[Brand_Printout_Name],  
 -- [Brand_Printout_Name_Chi] =   
 --  CASE   
 --   WHEN VBD.[Brand_Printout_Name_Chi] IS NULL THEN VBD.[Brand_Printout_Name]  
 --   WHEN VBD.[Brand_Printout_Name_Chi] = '' THEN VBD.[Brand_Printout_Name]  
 --   ELSE VBD.[Brand_Printout_Name_Chi]  
 --  END,  
 -- VBD.[HK_Reg_No],  
 -- VBD.[Manufacturer],  
 -- VBD.[Vaccination_Window_Min],  
 -- VBD.[Vaccination_Window_Max]
 --FROM   
 --[COVID19VaccineBrandDetail] VBD WITH(NOLOCK)  

 --ORDER BY   
 -- VBD.[Brand_ID]  

--END;
--GO

--GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBrandDetail_getAll] TO HCVU;
--GO


--GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBrandDetail_getAll] TO HCSP;
--GO
