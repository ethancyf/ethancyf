IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotRequest_getByServiceType]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotRequest_getByServiceType];
    END;
GO    

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================     
-- CR No.:  CRE20-022   (Vaccine Lot Management)
-- Author:  Nichole Ip
-- Create date:  Mar 2021
-- Description:  Get the vaccine lot mapping request by centre ID
-- =============================================    
--exec proc_COVID19VaccineLotRequest_get 'CVC003','HAADM1','P',NULL,1,1,1
--exec proc_COVID19VaccineLotRequest_get NULL,'HAADM1','P','R00007',1,1,1

-- CR No.:  CRE20-023   (Vaccine Lot Management)
-- Author:  Jeremy Chan
-- Create date:  Feb 2022
-- Description:  Get the vaccine lot mapping request by Service Type (edited from "proc_COVID19VaccineLotRequest_get")
-- =============================================    
--exec proc_COVID19VaccineLotRequest_getByServiceType '','SCHEME','RVP','HAADM1','P',NULL,1,1,1
--exec proc_COVID19VaccineLotRequest_getByServiceType '','SCHEME','VSS','HAADM1','P',NULL,1,1,1

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotRequest_getByServiceType] 
	@centre_id                    VARCHAR(10)= NULL, 
    @service_type                 VARCHAR(10),
	@scheme_code                  CHAR(10) = NULL,
    @user_id                      VARCHAR(20) = NULL, 
    @record_status                VARCHAR(1)  = NULL, 
    @request_id                   VARCHAR(20) = NULL, 
    @result_limit_1st_enable      BIT, 
    @result_limit_override_enable BIT, 
    @override_result_limit        BIT
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @rowcount INT;
        DECLARE @row_cnt_error VARCHAR(MAX);
        DECLARE @l_centre_id VARCHAR(10);
		DECLARE @l_service_type VARCHAR(10);
		DECLARE @l_scheme_code CHAR(10);
        DECLARE @l_user_id VARCHAR(20);
        DECLARE @delimiter VARCHAR(5);
        DECLARE @l_record_status VARCHAR(1);
        DECLARE @l_request_id VARCHAR(20);
        DECLARE @tblBooth TABLE(Booth VARCHAR(10));

        SET @l_centre_id = @centre_id;
		SET @l_service_type = @service_type;
		SET @l_scheme_code = @scheme_code;
        SET @l_user_id = @user_id;
        SET @l_record_status = @record_status;
        SET @l_request_id = @request_id;
		 
        -- =============================================  
        -- Initialization  
        -- =============================================  
		
		SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable, @result_limit_override_enable)) 
			FilteredVaccine.[Request_ID] AS [Request_ID], 
               FilteredVaccine.[Vaccine_Lot_No] AS [Vaccine_Lot_No], 
               FilteredVaccine.Centre_Name AS [Centre_Name], 
               FilteredVaccine.Booth AS [Booth], 
			   FilteredVaccine.[Service_Type] AS [Service_Type],
			   FilteredVaccine.[Scheme_Code] AS [Scheme_Code],
               VBD.[Brand_Name] AS [BrandName], 
               VLD.[Expiry_Date] AS [Expiry_Date], 
               FilteredVaccine.[Service_Period_From] AS [Request_EffectiveDateFrom], 
               ISNULL(FilteredVaccine.[Service_Period_To], VLD.Expiry_Date) AS [Request_EffectiveDateTo], 
               FilteredVaccine.[Record_status] AS [Record_Status], 
               ISNULL(FilteredVaccine.[Request_Type], '') AS [Request_Type], 
               FilteredVaccine.[Create_By] AS [Create_By], 
               FilteredVaccine.[Create_Dtm] AS [Create_Dtm], 
               FilteredVaccine.[Update_By] AS [Update_By], 
               FilteredVaccine.[Update_dtm] AS [Update_Dtm], 
               FilteredVaccine.[Requested_By] AS [Requested_by], 
               FilteredVaccine.[Requested_Dtm] AS [Requested_Dtm], 
               FilteredVaccine.[Approved_By] AS [Approved_By], 
               FilteredVaccine.[Approved_Dtm] AS [Approved_Dtm], 
               FilteredVaccine.[Rejected_By] AS [Rejected_By], 
               FilteredVaccine.[Rejected_Dtm] AS [Rejected_Dtm], 
               FilteredVaccine.[TSMP] AS [TSMP],
			   CASE
			   WHEN FilteredVaccine.Service_Period_To IS NULL
                        AND FilteredVaccine.Request_Type <> 'R'
                   THEN 'Y'
                   ELSE 'N'
				   END AS Up_To_ExpiryDtm ,
			   VBD.Brand_Trade_Name AS [Brand_Trade_Name]
        INTO #tempResult
		FROM
		(
		 -- =============================================      
        -- Centre
        -- =============================================    
        SELECT 
				VLR.Request_ID, 
                VLR.Vaccine_Lot_No, 
                VC.Centre_Name, 
				VLR.Service_Type,
				NULL AS Scheme_Code,
                VLR.Booth, 
                VLR.Service_Period_From, 
                VLR.Service_Period_To, 
                VLR.Record_status, 
                VLR.Request_Type, 
                VLR.Create_By, 
                VLR.Create_Dtm, 
                VLR.Update_By, 
                VLR.Update_dtm, 
                VLR.Requested_By, 
                VLR.Requested_Dtm, 
                VLR.Approved_By, 
                VLR.Approved_Dtm, 
                VLR.Rejected_By, 
                VLR.Rejected_Dtm, 
                VLR.TSMP

        FROM COVID19VaccineLotMappingRequest AS VLR WITH(NOLOCK)
             INNER JOIN VaccineCentre AS VC WITH(NOLOCK)
             ON VLR.Centre_ID = VC.Centre_ID
             INNER JOIN VaccineCentreVUMapping AS VCM WITH(NOLOCK)
             ON(VCM.User_id = @l_user_id
                OR @user_id IS NULL)
               AND VCM.Centre_ID = VLR.Centre_ID
        WHERE VLR.[Record_Status] = @l_record_status
              AND (VLR.Centre_ID = @l_centre_id
                   OR @l_centre_id IS NULL)
              AND (VLR.Request_id = @l_request_id
                   OR @l_request_id IS NULL)
			  AND VLR.Service_Type = @l_service_type
			  
         -- =============================================      
        -- Clinics    
        -- =============================================    

		UNION


		SELECT VLR.Request_ID, 
               VLR.Vaccine_Lot_No, 
               NULL AS Centre_Name, 
			   VLR.Service_Type,
			   VLR.Scheme_Code,
               VLR.Booth, 
               VLR.Service_Period_From, 
			   VLR.Service_Period_To, 
               VLR.Record_status, 
               VLR.Request_Type, 
               VLR.Create_By, 
               VLR.Create_Dtm, 
               VLR.Update_By, 
               VLR.Update_dtm, 
               VLR.Requested_By, 
               VLR.Requested_Dtm, 
               VLR.Approved_By, 
               VLR.Approved_Dtm, 
               VLR.Rejected_By, 
               VLR.Rejected_Dtm, 
               VLR.TSMP
               
        FROM COVID19VaccineLotMappingRequest AS VLR WITH(NOLOCK)
			 INNER JOIN UserRole AS UR WITH(NOLOCK)
			 ON UR.Scheme_Code = VLR.Scheme_Code
             AND (UR.User_id = @l_user_id
               OR @l_user_id IS NULL)
        WHERE VLR.[Record_Status] = @l_record_status
				   AND VLR.Service_Type = @l_service_type
              AND (VLR.Request_id = @l_request_id
                   OR @l_request_id IS NULL)
		      AND (VLR.Scheme_Code = @l_scheme_code
                   OR @l_scheme_code IS NULL)
		) FilteredVaccine
			INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             ON FilteredVaccine.Vaccine_Lot_No = VLD.Vaccine_Lot_No
             INNER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK)
             ON VBD.Brand_ID = VLD.Brand_ID


        -- =============================================      
        -- Max Row Checking    
        -- =============================================    
        BEGIN TRY
            SELECT @rowcount = COUNT(1)
            FROM #tempResult;
            EXEC proc_CheckFeatureResultRowLimit @row_count = @rowcount, 
                                                 @result_limit_1st_enable = @result_limit_1st_enable, 
                                                 @result_limit_override_enable = @result_limit_override_enable, 
                                                 @override_result_limit = @override_result_limit;
        END TRY
        BEGIN CATCH
            SET @row_cnt_error = ERROR_MESSAGE();
            RAISERROR(@row_cnt_error, 16, 1);
            RETURN;
        END CATCH;


        -- =============================================
        -- Return results
        -- =============================================   

		SELECT * FROM #tempResult;
		DROP TABLE #tempResult;

    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotRequest_getByServiceType] TO HCVU;
GO

