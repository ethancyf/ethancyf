
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMappingRequestSummary_get_byRequestID]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMappingRequestSummary_get_byRequestID];
    END;
GO

-- =============================================       
-- CR No.:  CRE20-022     
-- Author:  Nichole Ip
-- Create date:  10 Mar 2021     
-- Description:  Get Vaccine Lot management request list
-- =============================================      
-- exec proc_COVID19VaccineLotMappingSummary_get 'R202100222' 
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMappingRequestSummary_get_byRequestID] @request_id VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;  
        -- =============================================    
        -- Declaration    
        -- =============================================    
        DECLARE @tblBooth TABLE(Booth VARCHAR(10));
        DECLARE @booth VARCHAR(1000);
        DECLARE @delimiter VARCHAR(10);

        -- =============================================    
        -- Initialization    
        -- =============================================    
        --- =============================================  
        --- Prepare the booth temp table   
        --- =============================================  
        SELECT @booth = booth
        FROM COVID19VaccineLotMappingRequest
        WHERE Request_id = @request_id;

        SET @delimiter = ',';
        INSERT INTO @tblBooth
        SELECT *
        FROM func_split_string(@booth, @delimiter);

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
                        vld.Expiry_Date, 
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
             SELECT VMR.[Request_ID], 
                    VMR.[Vaccine_Lot_No], 
                    VMR.[Service_Type], 
                    VMR.[Centre_ID], 
                    TTB.[Booth] AS [Booth_No], 
                    SD.[Data_Value] AS [Booth_Name], 
                    VMR.[Service_Period_From] AS [Request_Service_Period_From], 
                    VMR.[Service_Period_To] AS [Request_Service_Period_To], 
                    VMR.[Request_Type], 
                    VMR.[Record_Status], 
                    VLM.[Service_Period_From] AS [Mapping_Service_Period_From], 
                    ISNULL(VLM.[Service_Period_To], VLM.[Expiry_Date]) AS [Mapping_Service_Period_To],
                    CASE
                        WHEN VLM.[Service_Period_To] IS NULL
                             AND VLM.[Vaccine_lot_no] IS NOT NULL
                        THEN 'Y'
                        ELSE 'N'
                    END AS Use_Of_ExpiryDtm
             FROM COVID19VaccineLotMappingRequest AS VMR WITH(NOLOCK)
                  INNER JOIN @tblBooth AS TTB
                  ON 1 = 1
                  LEFT JOIN StaticData AS SD WITH(NOLOCK)
                  ON SD.Item_No = TTB.Booth
                     AND SD.Column_Name = 'VaccineCentreBooth'
                  LEFT JOIN cteCVaccLotMappingDetail AS VLM
                  ON VMR.vaccine_lot_no = VLM.Vaccine_Lot_No
                     AND VLM.centre_id = VMR.Centre_id
                     AND VLM.booth = ttb.Booth
                     AND VLM.Record_Status = 'A'
                     AND VLM.Service_Type = 'CENTRE'
                     AND VLM.[Expiry_date] + 1 > GETDATE()
                     AND ((VLM.Service_Period_To + 1 > GETDATE()
                           AND VLM.[Expiry_date] >= VLM.Service_Period_To)
                          OR VLM.Service_Period_To IS NULL)

             --LEFT JOIN COVID19VaccineLotMapping VLM ON VMR.vaccine_lot_no = VLM.vaccine_lot_no  
             --                                          AND VMR.centre_id = VLM.centre_id  
             --                                          AND TTB.Booth = VLM.Booth  
             --   AND VLM.Record_Status ='A'
             --   AND VLM.Service_Type = 'CENTRE'
             --AND VLM.[Expiry_date] + 1 > GETDATE()
             WHERE VMR.request_id = @request_id
             ORDER BY SD.Display_Order;
    END;   
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMappingRequestSummary_get_byRequestID] TO HCVU;
GO