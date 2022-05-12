
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLot_search_for_PrivateClinic]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLot_search_for_PrivateClinic];
    END;
GO    
-- =============================================     
-- CR No.:  CREXX-XXX  
-- Author:  Jeremy Chan  
-- Create date:  22 December 2021
-- Description: Search all active Vaccine Lot Mapping records and requests by Clinics
-- =============================================    
--exec proc_COVID19VaccineLot_search_for_PrivateClinic 'SCHEME', 'RVP'

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLot_search_for_PrivateClinic] @service_type VARCHAR(20),
                                                                         @scheme_code     CHAR(10)
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_service_type VARCHAR(20);
		DECLARE @l_scheme_code CHAR(10);

        SET @l_service_type = @service_type;
		SET @l_scheme_code = @scheme_code;

        -- =============================================  
        -- Initialization  
        -- =============================================  
		
		CREATE TABLE #tempBaseScheme (
			Service_Type	VARCHAR(20),
			Scheme_Code     CHAR(10)
		)
       INSERT INTO #tempBaseScheme VALUES (@l_service_type,@l_scheme_code);

        --Find all (Active) COVID19VaccineLotMapping and 
        --(Active Pending Approval COVID19VaccineLotMappingRequest records
        WITH cteCVaccLotMappingRequest
             AS (SELECT VLMR.Request_ID, 
			            VLMR.Centre_ID,
                        VLMR.Vaccine_Lot_No, 
                        VLMR.Record_Status AS New_Record_status, 
                        tb.Scheme_Code, 
						tb.Service_Type,
                        VLMR.Service_Period_From AS New_Service_Period_From, 
                        ISNULL(VLMR.Service_Period_To, VLD.[Expiry_date]) AS New_Service_Period_To, 
                        VLMR.Request_Type, 
                        VLMR.Requested_By, 
                        VLMR.Requested_Dtm, 
                        VLMR.Approved_By, 
                        VLMR.Approved_Dtm
                 FROM COVID19VaccineLotMappingRequest AS VLMR WITH(NOLOCK)
				      INNER JOIN #tempBaseScheme AS tb
				      ON tb.Service_Type = VLMR.Service_Type
                         AND tb.Scheme_Code = VLMR.Scheme_Code
                      INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
                      ON VLMR.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                 --AND VLD.Record_Status = 'A'
                 WHERE VLMR.Record_Status = 'P'
        )
             SELECT ISNULL(VLM.Centre_ID, VLMRSpilt.Centre_ID) AS Centre_ID, 
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
					ISNULL(VLM.Service_Type, VLMRSpilt.Service_Type) AS Service_Type, 
					ISNULL(VLM.Scheme_Code, VLMRSpilt.Scheme_Code) AS Scheme_Code, 
                    VLM.TSMP
             INTO #tempResult
			  FROM #tempBaseScheme AS tbb
                  INNER JOIN COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
                  ON tbb.Service_Type = VLM.Service_Type
                     AND tbb.Scheme_Code = VLM.Scheme_Code
                     AND VLM.Record_Status = 'A'

             --FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
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
                  ON VLMRSpilt.Service_Type = VLM.Service_Type
				     AND VLMRSpilt.Scheme_Code = VLM.Scheme_Code
                     AND VLMRSpilt.Vaccine_Lot_No = VLM.Vaccine_Lot_No


        SELECT VBD.Brand_Name, 
               VBD.Brand_Trade_Name, 
               VLD.[Expiry_date], 
               VLD.Record_Status AS 'LotDetail_Record_Status',
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
			   tr.Service_Type,
			   tr.Scheme_Code,
               tr.TSMP
        FROM #tempResult AS tr
             LEFT OUTER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             ON tr.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                AND VLD.Record_Status = 'A'
             LEFT OUTER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK)
             ON VBD.Brand_ID = VLD.Brand_ID
        ORDER BY VBD.Brand_Name, 
                 tr.Vaccine_Lot_No;


	    IF OBJECT_ID('tempdb..#tempBaseScheme') IS NOT NULL
            BEGIN
                DROP TABLE #tempBaseScheme;
            END;

        IF OBJECT_ID('tempdb..#tempResult') IS NOT NULL
            BEGIN
                DROP TABLE #tempResult;
            END;
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLot_search_for_PrivateClinic] TO HCVU;
GO