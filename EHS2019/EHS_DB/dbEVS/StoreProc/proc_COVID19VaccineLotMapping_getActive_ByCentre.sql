
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_getActive_ByCentre]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_ByCentre]
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Raiman Chong
-- Created date:	17 Mar 2021
-- CR No.:			CRE20-0022
-- Description:		Get Current Time Active Vaccine Lot Mapping record by centre for batch remove drop down list
-- =============================================
-- exec proc_COVID19VaccineLotMapping_getActive_ByCentre 'CVC018'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_ByCentre] @centre_id VARCHAR(10)
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

        DECLARE @In_centre_id VARCHAR(10);
        SET @In_centre_id = @centre_id;

        SELECT VLM.[Vaccine_Lot_No], 
               VBD.[Brand_ID], 
               VBD.[Brand_Name], 
               VBD.[Brand_Trade_Name], 
               VLD.[New_Record_Status], 
               VLD.[Request_Type], 
               VLD.[Record_Status], 
               VLD.[Expiry_Date]
        FROM [COVID19VaccineLotMapping] AS VLM WITH(NOLOCK)
             INNER JOIN [COVID19VaccineLotDetail] AS VLD WITH(NOLOCK)
             ON VLM.[Vaccine_Lot_No] = VLD.[Vaccine_Lot_No]
                AND VLD.Record_Status = 'A'
             INNER JOIN [COVID19VaccineBrandDetail] AS VBD WITH(NOLOCK)
             ON VLD.[Brand_ID] = VBD.[Brand_ID]
        WHERE VLM.[Record_Status] = 'A'
              AND vlm.Centre_ID = @In_centre_id
              AND ((VLM.Service_Period_To + 1 > GETDATE()
                    AND VLD.[Expiry_date] >= VLM.Service_Period_To)
                   OR VLM.Service_Period_To IS NULL)
              AND VLD.[Expiry_date] + 1 > GETDATE()
              AND VLM.[Service_Type] = 'CENTRE'
        GROUP BY VLM.[Vaccine_Lot_No], 
                 VBD.[Brand_ID], 
                 VBD.[Brand_Name], 
                 VBD.[Brand_Trade_Name], 
                 VLD.[New_Record_Status], 
                 VLD.[Request_Type], 
                 VLD.[Record_Status], 
                 VLD.[Expiry_Date]
        ORDER BY VLM.[Vaccine_Lot_No];
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getActive_ByCentre] TO HCSP;

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getActive_ByCentre] TO HCVU;

GO