
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Nichole Ip  
-- Create date:  15 March 2021
-- Description: Get the active vaccine lot record from vaccinelotmapping 
-- =============================================    
--exec  proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo 'CVC018','1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,OR','BNT202100001'
--exec  proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo 'CVC003','13,14,11,12','Y2021MAR03'

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo] @centre_id      VARCHAR(10), 
                                                                               @booth_list     VARCHAR(1000) = NULL, 
                                                                               @vaccine_lot_no VARCHAR(20)   = NULL
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_centre_id VARCHAR(10);
        DECLARE @l_booth_list VARCHAR(1000);
        DECLARE @l_vaccine_lot_no VARCHAR(20);

        SET @l_centre_id = @centre_id;
        SET @l_booth_list = @booth_list;
        SET @l_vaccine_lot_no = @vaccine_lot_no;
        -- =============================================  
        -- Initialization  
        -- =============================================  

        SELECT DISTINCT 
               VCSM.Centre_ID, 
               VCSM.Booth
        INTO #tempBaseBooth
        FROM VaccineCentreSPMapping AS VCSM WITH(NOLOCK)
        WHERE(VCSM.Centre_id = @l_centre_id
              OR @l_centre_id IS NULL)
             AND (@l_booth_list IS NULL
                  OR EXISTS
                           (
                               SELECT 1
                               FROM func_split_string(@l_booth_list, ',') AS booth
                               WHERE vcsm.Booth = booth.Item
                            ));

        -- =============================================
        -- Return results
        -- =============================================   
        SELECT VLM.Vaccine_Lot_ID, 
               VLM.Centre_ID, 
               VLM.Vaccine_Lot_No, 
               booth.Item AS booth
        FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
             OUTER APPLY func_split_string(VLM.Booth, ',') AS booth
             INNER JOIN #tempBaseBooth AS tb
             ON tb.Booth = booth.Item
                AND tb.Centre_ID = VLM.Centre_ID
             INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             ON VLM.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                AND VLD.Record_Status = 'A'
        WHERE VLM.Record_Status = 'A'
              AND VLM.Service_Type = 'CENTRE'
              AND (@l_vaccine_lot_no IS NULL
                   OR @l_vaccine_lot_no = VLM.Vaccine_Lot_No)
              AND ((VLM.Service_Period_To + 1 > GETDATE()
                    AND VLD.[Expiry_date] >= VLM.Service_Period_To)
                   OR VLM.Service_Period_To IS NULL)
              AND VLD.[Expiry_date] + 1 > GETDATE();

        IF OBJECT_ID('tempdb..#tempBaseBooth') IS NOT NULL
            BEGIN
                DROP TABLE #tempBaseBooth;
            END;
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getActive_ByBoothLotNo] TO HCVU;
GO