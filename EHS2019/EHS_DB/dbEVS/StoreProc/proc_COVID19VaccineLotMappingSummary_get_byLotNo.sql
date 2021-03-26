
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMappingSummary_get_byLotNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMappingSummary_get_byLotNo];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Nichole Ip
-- Create date: 10 Mar 2021   
-- Description: Get the vaccine lot mapping detail by booth list for displaying vaccine lot history gridview in confirm page and
--				checking the duplicated lot record for batch assign function in vaccine lot management 
-- =============================================    
-- exec proc_COVID19VaccineLotMappingSummary_get_byLotNo 'BNT202100001','1,2,8','CVC001'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMappingSummary_get_byLotNo] @vaccine_lot_no VARCHAR(20)   = NULL, 
                                                                          @booth_list     VARCHAR(1000) = NULL, 
                                                                          @centre_id      VARCHAR(10)   = NULL
AS
    BEGIN
        SET NOCOUNT ON;
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @tblBooth TABLE
        (Booth          VARCHAR(10), 
         vaccine_lot_no VARCHAR(20), 
         Centre_id      VARCHAR(10)
        );
        DECLARE @booth VARCHAR(1000);
        DECLARE @delimiter CHAR(1);

        -- =============================================  
        -- Initialization  
        -- =============================================  
        SET @booth = @booth_list;
        SET @delimiter = ',';
        INSERT INTO @tblBooth
        SELECT *, 
               @vaccine_lot_no AS vaccine_lot_no, 
               @centre_id AS centre_id
        FROM func_split_string(@booth, @delimiter);

        --   select * from  @tblBooth

        WITH cteCVaccLotMappingDetail
             AS (SELECT VLM.[Vaccine_Lot_ID], 
                        VLM.[Service_Type], 
                        VLM.[Service_Period_From], 
                        VLM.[Service_Period_To], 
                        VLM.[Record_Status], 
                        VLM.[Create_By], 
                        VLM.[Create_Dtm], 
                        VLM.[Update_By], 
                        VLM.[Update_Dtm], 
                        vld.[Expiry_Date], 
                        VLM.[TSMP], 
                        VLM.[Vaccine_lot_no], 
                        vlm.Centre_ID, 
                        vlm.booth 

                 FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
                      INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
                      ON VLD.Vaccine_Lot_No = VLM.Vaccine_Lot_No)

             -- =============================================
             -- Return results
             -- =============================================   
             SELECT sd.Data_Value AS booth, 
                    sd.Item_No AS Booth_id, 
                    TTB.[Vaccine_Lot_No], 
                    TTB.[Centre_ID], 
                    VLM.[Vaccine_Lot_ID], 
                    VLM.[Service_Type], 
                    VLM.[Service_Period_From], 
                    VLM.[Service_Period_To], 
					CASE -- Checking Service_Period_To duplicated case for (read only)
                        WHEN VLM.Service_Period_To IS NULL
                             AND VLM.[Vaccine_lot_no] IS NOT NULL
                        THEN VLM.[Expiry_Date]
                        ELSE VLM.[Service_Period_To]
                    END AS [Service_Period_To_WithExpiryDate],
                    VLM.[Record_Status], 
                    VLM.[Create_By], 
                    VLM.[Create_Dtm], 
                    VLM.[Update_By], 
                    VLM.[Update_Dtm], 
                    VLM.[TSMP],
                    CASE
                        WHEN Service_Period_To IS NULL
                             AND VLM.[Vaccine_lot_no] IS NOT NULL
                        THEN 'Y'
                        ELSE 'N'
                    END AS Use_Of_ExpiryDtm
             FROM @tblBooth AS TTB
                  INNER JOIN StaticData AS SD WITH(NOLOCK)
                  ON SD.Item_No = TTB.Booth
                     AND SD.Column_Name = 'VaccineCentreBooth'
                  LEFT JOIN cteCVaccLotMappingDetail AS VLM
                  ON ttb.vaccine_lot_no = VLM.Vaccine_Lot_No
                     AND VLM.centre_id = ttb.centre_id
                     AND VLM.booth = ttb.Booth
                     AND VLM.Record_Status = 'A'
                     AND VLM.Service_Type = 'CENTRE'
                     AND VLM.[Expiry_date] + 1 > GETDATE()
                     AND (VLM.[Expiry_date] >= VLM.Service_Period_To
                          OR VLM.Service_Period_To IS NULL)	  
				order by sd.Display_Order asc;

     
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMappingSummary_get_byLotNo] TO HCVU;
GO