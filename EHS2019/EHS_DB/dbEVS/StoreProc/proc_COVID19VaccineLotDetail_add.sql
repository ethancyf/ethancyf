
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotDetail_add]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_add];
    END;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================  
-- Modification History  
-- Modified by:   
-- Modified date:   
-- Description:   
-- =============================================  
-- =============================================
-- Modification History
-- Created by:		Nichole Ip
-- Created date:	11 Mar 2021
-- CR No.:			CRE20-0022
-- Description:		Add the vaccine lot detail
-- =============================================
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_add] @vaccine_Lot_No    VARCHAR(20), 
                                                          @brand_ID          VARCHAR(10), 
                                                          @expiry_Date       DATETIME, 
                                                          @record_Status     CHAR(1), 
                                                          @new_Record_Status CHAR(1), 
                                                          @lot_assign_status CHAR(1), 
                                                          @request_Type      VARCHAR(20), 
                                                          @create_By         VARCHAR(20) = NULL, 
                                                          @update_By         VARCHAR(20) = NULL, 
                                                          @request_By        VARCHAR(20) = NULL, 
                                                          @tsmp              TIMESTAMP   = NULL
AS
    BEGIN
        SET NOCOUNT ON;
        -- =============================================
        -- Declaration
        -- =============================================
        -- =============================================
        -- Validation 
        -- =============================================
        --(
        --    SELECT TSMP
        --    FROM COVID19VaccineLotDetail
        --    WHERE Vaccine_Lot_No = @Vaccine_Lot_No
        --) != @tsmp
        --OR @tsmp IS NOT NULL)
        --    BEGIN
        --        RAISERROR('00011', 16, 1);
        --        RETURN @@error;
        --    END;
        -- =============================================
        -- Initialization
        -- =============================================
        -- =============================================
        -- Return results
        -- =============================================
        IF(@tsmp IS NOT NULL)
            BEGIN
                IF EXISTS
                (
                    SELECT TOP 1 *
                    FROM COVID19VaccineLotDetail
                    WHERE Vaccine_Lot_No = @vaccine_lot_no
                          AND tsmp = @tsmp
                )
                    BEGIN
                        UPDATE COVID19VaccineLotDetail
                          SET 
                              Brand_ID = @brand_ID,
							  [Expiry_Date] = @expiry_date,
							  Record_status = @record_Status,  
							  New_Record_Status = @new_Record_Status, 
                              Lot_Assign_Status = @lot_assign_status,
							  Request_Type = @request_Type, 						                              
                              Request_By = @request_By, 
                              Request_Dtm = GETDATE(), 
                              Update_By = @update_By, 
                              Update_Dtm = GETDATE(), 
                              Approve_By = NULL, 
                              Approve_Dtm = NULL, 
                              Reject_By = NULL, 
                              Reject_Dtm = NULL
                        WHERE Vaccine_Lot_No = @vaccine_Lot_No
                              AND TSMP = @tsmp;
                    END;
                    ELSE
                    BEGIN
                        RAISERROR('00011', 16, 1);
                        RETURN @@error;
                    END;
            END;
            ELSE
            BEGIN
                INSERT INTO [dbo].[COVID19VaccineLotDetail]
                ([Vaccine_Lot_No], 
                 [Brand_ID], 
                 [Expiry_Date], 
                 [Record_Status], 
                 [New_Record_Status], 
                 [Lot_Assign_Status], 
                 [Request_Type], 
                 [Create_By], 
                 [Create_Dtm], 
                 [Request_By], 
                 [Request_Dtm], 
                 [Update_By], 
                 [Update_Dtm]
                )
                VALUES
                (@vaccine_Lot_No, 
                 @brand_ID, 
                 @expiry_Date, 
                 @record_Status, 
                 @new_Record_Status, 
                 @lot_assign_status, 
                 @request_Type, 
                 @create_By, 
                 GETDATE(), 
                 @request_By, 
                 GETDATE(), 
                 @update_By, 
                 GETDATE()
                );
            END;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_add] TO HCVU;
GO