
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_getActive_BySchemeLotNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_BySchemeLotNo];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Jeremy Chan  
-- Create date:  12 Jan 2022
-- Description: Get the active vaccine lot record from vaccinelotmapping by service type 
-- =============================================    
--exec  proc_COVID19VaccineLotMapping_getActive_BySchemeLotNo 'SCHEME','RVP',''BNT20210423'

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_BySchemeLotNo] @service_type   VARCHAR(20),
                                                                               @scheme_code CHAR(10),
                                                                               @vaccine_lot_no VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_service_type VARCHAR(20);
		DECLARE @l_scheme_code CHAR(10);
        DECLARE @l_vaccine_lot_no VARCHAR(20);

        SET @l_service_type = @service_type;
		SET @l_scheme_code = @scheme_code
        SET @l_vaccine_lot_no = @vaccine_lot_no;
        -- =============================================  
        -- Initialization  
        -- =============================================  

        -- =============================================
        -- Return results
        -- =============================================   
        SELECT VLM.Vaccine_Lot_ID, 
               VLM.Service_Type, 
			   VLM.Scheme_Code,
               VLM.Vaccine_Lot_No,
			   VLM.Record_Status,
			   VLM.Service_Period_From,
			   VLM.Service_Period_To,
			   CASE -- Checking Service_Period_To duplicated case for (read only)
					WHEN VLM.Service_Period_To IS NULL
                            AND VLM.[Vaccine_lot_no] IS NOT NULL
                    THEN VLD.[Expiry_Date]
                    ELSE VLM.[Service_Period_To]
                END AS [Service_Period_To_WithExpiryDate],
			   CASE
                    WHEN Service_Period_To IS NULL
                            AND VLM.[Vaccine_lot_no] IS NOT NULL
                    THEN 'Y'
                    ELSE 'N'
                END AS Use_Of_ExpiryDtm
        FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
             INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             ON VLM.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                AND VLD.Record_Status = 'A'
        WHERE VLM.Record_Status = 'A'
              AND VLM.Service_Type = @l_service_type
			  AND VLM.Scheme_Code = @l_scheme_code
              AND (@l_vaccine_lot_no IS NULL
                   OR @l_vaccine_lot_no = VLM.Vaccine_Lot_No)
              AND ((VLM.Service_Period_To + 1 > GETDATE()
                    AND VLD.[Expiry_date] >= VLM.Service_Period_To)
                   OR VLM.Service_Period_To IS NULL)
              AND VLD.[Expiry_date] + 1 > GETDATE();
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getActive_BySchemeLotNo] TO HCVU;
GO