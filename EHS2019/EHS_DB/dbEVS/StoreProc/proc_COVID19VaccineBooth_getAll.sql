
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineBooth_getAll]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineBooth_getAll]
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- Created by:		
-- Created date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Raiman
-- Created date:	11 Mar 2021
-- CR No.:			CRE20-0022
-- Description:		Get the vaccine lot booth list
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19VaccineBooth_getAll]
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

        SELECT VCSP.[Centre_ID], 
               VCSP.[Booth], 
               VC.[Centre_Name], 
               sd.Data_Value, 
               sd.Display_Order
        FROM [VaccineCentreSPMapping] AS VCSP WITH(NOLOCK)
             INNER JOIN [VaccineCentre] AS VC WITH(NOLOCK)
             ON VCSP.[Centre_ID] = VC.[Centre_ID]
             LEFT OUTER JOIN StaticData AS sd WITH(NOLOCK)
             ON sd.[Column_Name] = 'VaccineCentreBooth'
                AND vcsp.[booth] = sd.[Item_No]
        GROUP BY VCSP.[Centre_ID], 
                 VC.[Centre_Name], 
                 vcsp.[booth], 
                 sd.Data_Value, 
                 sd.Display_Order
        ORDER BY VCSP.[Centre_ID], 
                 sd.Display_Order;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBooth_getAll] TO HCSP;

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBooth_getAll] TO HCVU;

GO