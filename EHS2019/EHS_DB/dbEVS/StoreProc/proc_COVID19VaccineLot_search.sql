
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLot_search]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLot_search];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022  
-- Author:  Martin Tang  
-- Create date:  10 March 2021
-- Description: Search all active Vaccine Lot Mapping records and requests
-- =============================================    
--exec proc_COVID19VaccineLot_search 'CVC001',NULL,'1,2,3',NULL,NULL,NULL,'HAADM1'

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLot_search] @centre_id      VARCHAR(10), 
                                                        @vaccine_lot_id VARCHAR(20)   = NULL, 
                                                        @booth_list     VARCHAR(1000) = NULL, 
                                                        @vaccine_lot_no VARCHAR(20)   = NULL
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_centre_id VARCHAR(10);
        DECLARE @l_vaccine_lot_id VARCHAR(20);
        DECLARE @l_booth_list VARCHAR(1000);
        DECLARE @l_vaccine_lot_no VARCHAR(20);

        SET @l_centre_id = @centre_id;
        SET @l_vaccine_lot_id = @vaccine_lot_id;
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
             AND (@booth_list IS NULL
                  OR EXISTS
                           (
                               SELECT 1
                               FROM func_split_string(@l_booth_list, ',') AS booth
                               WHERE vcsm.Booth = booth.Item
                            ));

        --Find all (Active) COVID19VaccineLotMapping and 
        --(Active Pending Approval COVID19VaccineLotMappingRequest records
        WITH cteCVaccLotMappingRequest
             AS (SELECT VLMR.Request_ID, 
                        VLMR.Centre_ID, 
                        VLMR.Vaccine_Lot_No, 
                        VLMR.Record_Status AS New_Record_status, 
                        booth.Item AS booth, 
                        VLMR.Service_Period_From AS New_Service_Period_From, 
                        ISNULL(VLMR.Service_Period_To, VLD.[Expiry_date]) AS New_Service_Period_To, 
                        VLMR.Request_Type, 
                        VLMR.Requested_By, 
                        VLMR.Requested_Dtm, 
                        VLMR.Approved_By, 
                        VLMR.Approved_Dtm
                 FROM COVID19VaccineLotMappingRequest AS VLMR WITH(NOLOCK)
                      OUTER APPLY func_split_string(VLMR.Booth, ',') AS booth
                      INNER JOIN #tempBaseBooth AS tb
                      ON tb.Booth = booth.Item
                         AND tb.Centre_ID = VLMR.Centre_ID
                      INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
                      ON VLMR.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                 --AND VLD.Record_Status = 'A'
                 WHERE VLMR.Record_Status = 'P'
        --AND VLMR.Request_Type <> 'R'
        --AND (VLMR.Service_Period_To + 1 > GETDATE()
        --     OR VLMR.Service_Period_To IS NULL)
        )
             SELECT ISNULL(VLM.Centre_ID, VLMRSpilt.Centre_ID) AS Centre_ID, 
                    ISNULL(VLM.booth, VLMRSpilt.booth) AS Booth, 
                    ISNULL(VLM.Vaccine_Lot_No, VLMRSpilt.Vaccine_Lot_No) AS Vaccine_Lot_No, 
                    VLM.Vaccine_Lot_ID, 
                    VLMRSpilt.Request_ID, 
                    VLM.Service_Period_From, 
                    ISNULL(VLM.Service_Period_To, VLD.[Expiry_date]) AS Service_Period_To, 
                    VLMRSpilt.New_Service_Period_From, 
                    VLMRSpilt.New_Service_Period_To, 
                    vlm.Record_Status, 
                    VLMRSpilt.New_Record_status, 
                    ISNULL(VLMRSpilt.New_Record_status, vlm.Record_Status) AS Display_Record_Status, 
                    VLM.Create_By, 
                    VLM.Create_Dtm, 
                    VLM.Update_By, 
                    VLM.Update_dtm, 
                    VLMRSpilt.Request_Type, 
                    VLMRSpilt.Requested_Dtm, 
                    VLMRSpilt.Requested_By, 
                    VLMRSpilt.Approved_By, 
                    VLMRSpilt.Approved_Dtm, 
                    VLM.TSMP
             INTO #tempResult
             FROM #tempBaseBooth AS tbb
                  INNER JOIN COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
                  ON tbb.centre_id = VLM.centre_id
                     AND tbb.Booth = VLM.Booth
                     AND VLM.Record_Status = 'A'
                     AND VLM.Service_Type = 'CENTRE'
                  INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
                  ON VLM.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                     AND VLD.[Expiry_date] + 1 > GETDATE()
                     AND (VLM.Service_Period_To + 1 > GETDATE()
                          OR VLM.Service_Period_To IS NULL)
                     --AND ((VLM.Service_Period_To + 1 > GETDATE()
                     --      AND VLD.[Expiry_date] >= VLM.Service_Period_To)
                     --     OR VLM.Service_Period_To IS NUL
                     AND VLD.Record_Status = 'A'
                  FULL OUTER JOIN cteCVaccLotMappingRequest AS VLMRSpilt WITH(NOLOCK)
                  ON VLMRSpilt.centre_id = VLM.centre_id
                     AND VLMRSpilt.Booth = VLM.Booth
                     AND VLMRSpilt.Vaccine_Lot_No = VLM.Vaccine_Lot_No;

        SELECT VC.Centre_Name, 
               VBD.Brand_Name, 
               VBD.Brand_Trade_Name, 
               VLD.[Expiry_date], 
               VLD.Record_Status AS 'LotDetail_Record_Status', 
               SD.Display_Order AS 'Booth_Order', 
               SD.Data_Value AS 'Booth_name', 
               tbb.Booth, 
               tr.Vaccine_Lot_ID, 
               tr.Vaccine_Lot_No, 
               tr.Request_ID, 
               tr.Service_Period_From, 
               tr.Service_Period_To, 
               tr.New_Service_Period_From, 
               tr.New_Service_Period_To, 
               tr.Request_Type, 
               tr.Record_Status, 
               tr.New_Record_status, 
               tr.Display_Record_Status, 
               tr.Create_By, 
               tr.Create_Dtm, 
               tr.Update_By, 
               tr.Update_dtm, 
               tr.Requested_By, 
               tr.Requested_Dtm, 
               tr.TSMP
        FROM #tempBaseBooth AS tbb
             LEFT OUTER JOIN #tempResult AS tr
             ON tbb.Booth = tr.Booth
                AND tr.Booth = tbb.booth
             LEFT OUTER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             ON tr.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                AND VLD.Record_Status = 'A'
             LEFT OUTER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK)
             ON VBD.Brand_ID = VLD.Brand_ID
             LEFT OUTER JOIN VaccineCentre AS VC WITH(NOLOCK)
             ON tr.Centre_ID = VC.Centre_ID
             LEFT OUTER JOIN StaticData AS SD WITH(NOLOCK)
             ON SD.Item_No = tbb.Booth
                AND SD.Column_Name = 'VaccineCentreBooth'
        ORDER BY SD.Display_Order, 
                 VBD.Brand_Name, 
                 tr.Vaccine_Lot_No;

        IF OBJECT_ID('tempdb..#tempBaseBooth') IS NOT NULL
            BEGIN
                DROP TABLE #tempBaseBooth;
            END;

        IF OBJECT_ID('tempdb..#tempResultWithEditRequest') IS NOT NULL
            BEGIN
                DROP TABLE #tempResultWithEditRequest;
            END;

        IF OBJECT_ID('tempdb..#tempResult') IS NOT NULL
            BEGIN
                DROP TABLE #tempResult;
            END;
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLot_search] TO HCVU;
GO