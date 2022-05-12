
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Martin Tang  
-- Create date:  15 March 2021
-- Description: Get all pending approval records in COVID19VaccineLotMappingRequest
-- =============================================    
-- CR No.:  CRE20-023   
-- Author:  Jeremy Chan  
-- Edited date:  15 FEb 2022
-- Description: Get all pending approval records in COVID19VaccineLotMappingRequest by centre or scheme
-- =============================================  
--exec proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo 'CVC003','1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,OR','BNT202100001'
--exec proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo 'CVC003','13,14,15,16','BNT202100001'

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo] @centre_id      VARCHAR(10)  = NULL,
                                                                                  @booth_list     VARCHAR(1000) = NULL, 
                                                                                  @vaccine_lot_no VARCHAR(20)   = NULL,
																				  @service_type   VARCHAR(20)   = NULL,
																				  @scheme_code    CHAR(10)   = NULL
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_centre_id VARCHAR(10);
        DECLARE @l_booth_list VARCHAR(1000);
        DECLARE @l_vaccine_lot_no VARCHAR(20);
		DECLARE @l_service_type VARCHAR(20);
		DECLARE @l_scheme_code CHAR(10);

        SET @l_centre_id = @centre_id;
        SET @l_booth_list = @booth_list;
        SET @l_vaccine_lot_no = @vaccine_lot_no;
		SET @l_service_type = @service_type;
		SET @l_scheme_code = @scheme_code;
        -- =============================================  
        -- Initialization  
        -- =============================================  
IF @l_service_type = 'CENTRE'
BEGIN
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

        -- =============================================
        -- Return results
        -- =============================================   
        --Find all pending approval records in COVID19VaccineLotMappingRequest
        SELECT VLMR.Request_ID, 
               VLMR.Centre_ID, 
               VLMR.Vaccine_Lot_No, 
               booth.Item AS booth
        FROM COVID19VaccineLotMappingRequest AS VLMR WITH(NOLOCK)
             OUTER APPLY func_split_string(VLMR.Booth, ',') AS booth
             INNER JOIN #tempBaseBooth AS tb
             ON tb.Booth = booth.Item
                AND tb.Centre_ID = VLMR.Centre_ID
             INNER JOIN StaticData AS SD WITH(NOLOCK)
             ON SD.Item_No = booth.Item
                AND SD.Column_Name = 'VaccineCentreBooth'
        WHERE VLMR.Record_Status = 'P'
              AND VLMR.Service_Type = @l_service_type
              AND (@l_vaccine_lot_no IS NULL
                   OR @l_vaccine_lot_no = VLMR.Vaccine_Lot_No)
        ORDER BY sd.Display_Order ASC;
		END


		ELSE IF @l_service_type = 'SCHEME'
		BEGIN
		--CREATE TABLE #tempBaseScheme (
		--	Service_Type	VARCHAR(20),
		--	Scheme_Code     CHAR(10)
		--)
       --INSERT INTO #tempBaseScheme VALUES (@l_service_type,@l_scheme_code);
		--  SELECT DISTINCT 
        --       VCSM.Centre_ID, 
        --       VCSM.Booth
        --INTO #tempBaseBooth
        --FROM VaccineCentreSPMapping AS VCSM WITH(NOLOCK)
        --WHERE(VCSM.Centre_id = @l_centre_id
           --   OR @l_centre_id IS NULL)
             --AND (@booth_list IS NULL
              --    OR EXISTS
                         --  (
                            --   SELECT 1
                           --    FROM func_split_string(@l_booth_list, ',') AS booth
                          --     WHERE vcsm.Booth = booth.Item
                          --  ));

        -- =============================================
        -- Return results
        -- =============================================   
        --Find all pending approval records in COVID19VaccineLotMappingRequest
        SELECT VLMR.Request_ID, 
               VLMR.Scheme_Code, 
			   VLMR.Service_Type,
               VLMR.Vaccine_Lot_No
               --booth.Item AS booth
        FROM COVID19VaccineLotMappingRequest AS VLMR WITH(NOLOCK)
             --OUTER APPLY func_split_string(VLMR.Booth, ',') AS booth
             --INNER JOIN #tempBaseScheme AS tb
             --ON tb.Scheme_Code = booth.Item
              --  AND tb.Centre_ID = VLMR.Centre_ID
             --INNER JOIN StaticData AS SD WITH(NOLOCK)
             --ON SD.Item_No = booth.Item
              --  AND SD.Column_Name = 'VaccineCentreBooth'
        WHERE VLMR.Record_Status = 'P'
              AND VLMR.Service_Type = @l_service_type
              AND (@l_vaccine_lot_no IS NULL
                   OR @l_vaccine_lot_no = VLMR.Vaccine_Lot_No)
			  AND VLMR.Scheme_Code = @l_scheme_code
		END
        IF OBJECT_ID('tempdb..#tempBaseBooth') IS NOT NULL
            BEGIN
                DROP TABLE #tempBaseBooth;
            END;
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMappingPendingApproval_get_byLotNo] TO HCVU;
GO