
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMappingRequest_add]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMappingRequest_add];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Author: Martin Tang
-- CR No.: CRE20-022 
-- Create date:  05 Feb 2021
-- Description: Add COVID19VaccineLotMappingRequest List  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by: Jeremy Chan	  
-- Modified date: 05 Jan 2022  
-- Description: Add Service_Type & Scheme_Code
-- =============================================  
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMappingRequest_add] @Request_ID          VARCHAR(10), 
                                                                  @vaccine_Lot_No      VARCHAR(20), 
                                                                  @service_Type        VARCHAR(20), 
																  @scheme_Code         CHAR(10),
                                                                  @centre_ID           VARCHAR(10), 
                                                                  @booth_list          VARCHAR(1000), 
                                                                  @service_Period_From DATETIME = null, 
                                                                  @service_Period_To   DATETIME = null, 
                                                                  @request_Type        VARCHAR(20), 
                                                                  @record_Status       CHAR(1), 
                                                                  @request_By          VARCHAR(20), 
                                                                  @create_By           VARCHAR(20), 
                                                                  @update_By           VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;
        INSERT INTO [dbo].[COVID19VaccineLotMappingRequest]
               (Request_ID, 
                Vaccine_Lot_No, 
                Service_Type, 
				Scheme_Code,
                Centre_ID, 
                Booth, 
                RCH_Code, 
                SP_ID, 
                Practice_Display_Seq, 
                Service_Period_From, 
                Service_Period_To, 
                Request_Type, 
                Record_status, 
                Requested_By, 
                Requested_Dtm, 
                Create_By, 
                Create_Dtm, 
                Update_By, 
                Update_dtm
               )
        VALUES
              (@Request_ID, 
               @vaccine_Lot_No, 
               @service_Type, 
			   @scheme_Code,
               @centre_ID, 
               @booth_list, 
               NULL, 
               NULL, 
               NULL, 
               @service_Period_From, 
               @service_Period_To, 
               @request_Type, 
               @record_Status, 
               @request_By, 
               GETDATE(), 
               @create_By, 
               GETDATE(), 
               @update_By, 
               GETDATE()
               );
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMappingRequest_add] TO HCVU;
GO