
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_get_ByLotId]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_get_ByLotId];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   (Vaccine Lot management)
-- Author:  Martin Tang
-- Create date:   10 March 2021
-- Description: Get the vaccine lot mapping By Lot Id
-- =============================================  
-- =============================================  
-- CR No.:  CRE20-023   (Vaccine Lot management)
-- Author:  Jeremy Chan
-- Create date:   04 January 2022
-- Description: Add service type in vaccine lot mapping By Lot Id
-- =============================================  
-- exec proc_COVID19VaccineLotMapping_get_ByLotId 'DC0000032'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_get_ByLotId] @Vaccine_Lot_ID VARCHAR(20),
                                                                   @Service_Type VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;
        -- =============================================  
        -- Declaration  
        -- =============================================  
		DECLARE @l_service_type VARCHAR(20)
		DECLARE @l_vaccine_lot_id VARCHAR(20);
		
		SET @l_service_type = @Service_Type;
		SET @l_vaccine_lot_id = @Vaccine_Lot_ID;
        -- =============================================  
        -- Initialization  
        -- =============================================
IF @l_service_type = 'CENTRE'
BEGIN
        SELECT VLR.Vaccine_Lot_No, 
               VLR.Centre_ID, 
               booth.Item AS Booth, 
               Approved_By, 
               Approved_Dtm, 
               Rejected_By, 
               Rejected_Dtm
        INTO #tempMappingRequest
        FROM COVID19VaccineLotMappingRequest AS VLR WITH(NOLOCK)
             OUTER APPLY func_split_string(VLR.Booth, ',') AS booth
        WHERE vlr.Request_Type <> 'R'
              AND Record_Status = 'A'
			  AND Service_Type = @l_service_type;
        -- =============================================
        -- Return results
        -- =============================================   
        WITH cteTempMappingRequest
             AS (SELECT *, 
                        ROW_NUMBER() OVER(PARTITION BY Vaccine_Lot_No, 
                                                       Centre_ID, 
                                                       Booth
                        ORDER BY Approved_Dtm desc) AS rt
                 FROM #tempMappingRequest)
             SELECT VLM.Vaccine_Lot_ID, 
                    VLM.Vaccine_Lot_No, 
					VLM.Service_Type,
					VLM.Scheme_Code,
                    sd.Data_Value AS booth, 
                    sd.Item_No AS Booth_id, 
                    VLM.Service_Period_From, 
                    VLM.Service_Period_To, 
                    VLM.Record_Status, 
                    VLM.Create_By, 
                    VLM.Create_Dtm, 
                    VLM.Update_By, 
                    VLM.Update_Dtm, 
                    VLMR.Approved_By, 
                    VLMR.Approved_Dtm, 
                    VLD.[Expiry_date], 
                    VLM.Centre_ID, 
                    VC.Centre_Name, 
                    VBD.Brand_Name, 
                    VBD.Brand_Trade_Name, 
                    VLM.TSMP, 
                    VLM.Record_Status AS 'New_Record_Status', 
                    NULL AS 'New_Service_Period_From', 
                    NULL AS 'New_Service_Period_To', 
                    NULL AS 'Request_Type', 
                    NULL AS 'Requested_By', 
                    NULL AS 'Requested_Dtm',
                    CASE
                        WHEN VLM.Service_Period_To IS NULL
                        THEN 'Y'
                        ELSE 'N'
                    END AS Up_To_ExpireDtm
             FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
                  INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
                  ON VLM.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                  INNER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK)
                  ON VBD.Brand_ID = VLD.Brand_ID
                  LEFT JOIN VaccineCentre AS VC WITH(NOLOCK)
                  ON VLM.Centre_ID = VC.Centre_ID
                  LEFT JOIN StaticData AS SD WITH(NOLOCK)
                  ON SD.Item_No = VLM.Booth
                     AND SD.Column_Name = 'VaccineCentreBooth'
                  LEFT JOIN cteTempMappingRequest AS VLMR
                  ON VLMR.Vaccine_Lot_No = VLM.Vaccine_Lot_No
                     AND VLMR.Centre_ID = VLM.Centre_ID
                     AND VLMR.Booth = VLM.Booth
                     AND VLMR.rt = 1
             WHERE VLM.Vaccine_Lot_ID = @l_vaccine_lot_id
			 AND VLM.Service_Type = @l_service_type;
	END
	ELSE IF @l_service_type = 'SCHEME'
		BEGIN
		SELECT Vaccine_Lot_No, 
               Service_Type, 
               Scheme_Code, 
               Approved_By, 
               Approved_Dtm, 
               Rejected_By, 
               Rejected_Dtm
        INTO #tempMappingSchemeRequest
        FROM COVID19VaccineLotMappingRequest AS VLR WITH(NOLOCK)
        WHERE vlr.Request_Type <> 'R'
              AND Record_Status = 'A'
			  AND Service_Type = @l_service_type;
        -- =============================================
        -- Return results
        -- =============================================   
        WITH cteTempMappingRequest
             AS (SELECT *, 
                        ROW_NUMBER() OVER(PARTITION BY Vaccine_Lot_No, 
                                                       Service_Type, 
                                                       Scheme_Code
                        ORDER BY Approved_Dtm desc) AS rt
                 FROM #tempMappingSchemeRequest)
             SELECT VLM.Vaccine_Lot_ID, 
                    VLM.Vaccine_Lot_No, 
					VLM.Service_Type,
					VLM.Scheme_Code,
                    NULL AS booth, 
                    NULL AS Booth_id, 
                    VLM.Service_Period_From, 
                    VLM.Service_Period_To, 
                    VLM.Record_Status, 
                    VLM.Create_By, 
                    VLM.Create_Dtm, 
                    VLM.Update_By, 
                    VLM.Update_Dtm, 
                    VLMR.Approved_By, 
                    VLMR.Approved_Dtm, 
                    VLD.[Expiry_date], 
                    VLM.Centre_ID, 
                    NULL AS Centre_Name, 
                    VBD.Brand_Name, 
                    VBD.Brand_Trade_Name, 
                    VLM.TSMP, 
                    VLM.Record_Status AS 'New_Record_Status', 
                    NULL AS 'New_Service_Period_From', 
                    NULL AS 'New_Service_Period_To', 
                    NULL AS 'Request_Type', 
                    NULL AS 'Requested_By', 
                    NULL AS 'Requested_Dtm',
                    CASE
                        WHEN VLM.Service_Period_To IS NULL
                        THEN 'Y'
                        ELSE 'N'
                    END AS Up_To_ExpireDtm
             FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
                  INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
                  ON VLM.Vaccine_Lot_No = VLD.Vaccine_Lot_No
                  INNER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK)
                  ON VBD.Brand_ID = VLD.Brand_ID
                  LEFT JOIN cteTempMappingRequest AS VLMR
                  ON VLMR.Vaccine_Lot_No = VLM.Vaccine_Lot_No
                     AND VLMR.Service_Type = VLM.Service_Type
                     AND VLMR.Scheme_Code = VLM.Scheme_Code
                     AND VLMR.rt = 1
             WHERE VLM.Vaccine_Lot_ID = @l_vaccine_lot_id
			 AND VLM.Service_Type = @l_service_type;
		END
		 END
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_get_ByLotId] TO HCVU;
GO