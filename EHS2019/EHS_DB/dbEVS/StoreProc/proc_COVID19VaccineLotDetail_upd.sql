
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotDetail_upd]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_upd];
    END;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-022 (Immu record)
-- Modified by:		 Nichole Ip
-- Modified date:	 10 Mar 2021
-- Description:		 Update the vaccine lot detail on different request 
-- =============================================
-- exec proc_COVID19VaccineLotDetail_update 'LC0000015','E','HAADM1'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_upd] @Vaccine_Lot_No    VARCHAR(20) = NULL, 
                                                          @Action_Type       VARCHAR(2), 
                                                          @user_id           VARCHAR(20) = NULL, 
                                                          @lot_assign_status CHAR(1)     = NULL, 
                                                          @tsmp              TIMESTAMP
AS
    BEGIN

        -- =============================================
        -- Validation 
        -- =============================================
        IF
        (
            SELECT TSMP
            FROM COVID19VaccineLotDetail
            WHERE Vaccine_Lot_No = @Vaccine_Lot_No
        ) != @tsmp
            BEGIN
                RAISERROR('00011', 16, 1);
                RETURN @@error;
            END;

        -- =============================================
        -- Initialization
        -- =============================================
        DECLARE @RecordStatus AS CHAR(1);
        DECLARE @RecordType AS CHAR(1);
        SET @RecordStatus = 'A';
        SET @RecordType = NULL;

        -- =============================================
        -- Return results
        -- =============================================
        ----------------------------------
        --- Approve
        ----------------------------------
        IF @Action_Type = 'A'
            BEGIN
                UPDATE COVID19VaccineLotDetail
                  SET 
                      New_Record_Status = NULL, 
                      Record_status = CASE
                                          WHEN Request_Type = 'R'
                                          THEN 'D'
                                          ELSE 'A'
                                      END, 
                      Request_Type = NULL, 
                      Request_By = NULL, 
                      Request_Dtm = NULL, 
                      Update_by = @user_id, 
                      Update_Dtm = GETDATE(), 
                      Approve_by = @user_id, 
                      Approve_Dtm = GETDATE(),
                      -- [Expiry_Date] = CASE WHEN Request_Type='A' then New_Expiry_Date else [Expiry_Date]  end,
                      Lot_Assign_Status = CASE
                                              WHEN Request_Type = 'A'
                                              THEN New_Lot_Assign_Status
                                              WHEN Request_Type = 'R'
                                              THEN New_Lot_Assign_Status 
                                              ELSE Lot_Assign_Status
                                          END, 
                      New_Expiry_Date = NULL, 
                      New_Lot_Assign_Status = NULL, 
                      Reject_By = NULL, 
                      Reject_Dtm = NULL
                WHERE Vaccine_Lot_No = @Vaccine_Lot_No
                      AND TSMP = @tsmp;
            END;
        ----------------------------------
        --Reject
        ----------------------------------
        IF @Action_Type = 'R'
            BEGIN
                IF EXISTS
                (
                    SELECT Vaccine_Lot_No
                    FROM COVID19VaccineLotDetail
                    WHERE Vaccine_Lot_No = @Vaccine_Lot_No
                          AND Request_Type = 'N'
                )
                    BEGIN
                        DELETE FROM COVID19VaccineLotDetail
                        WHERE Vaccine_Lot_No = @Vaccine_Lot_No;
                    END;
                    ELSE
                    BEGIN
                        UPDATE COVID19VaccineLotDetail
                          SET 
                              Request_Type = NULL, 
                              New_Record_Status = NULL, 
                              Record_status = CASE
                                                  WHEN Request_Type = 'N'
                                                  THEN 'D'
                                                  ELSE 'A'
                                              END, 
                              Request_By = NULL, 
                              Request_Dtm = NULL, 
                              Update_by = @user_id, 
                              Update_Dtm = GETDATE(), 
                              Reject_by = @user_id, 
                              Reject_Dtm = GETDATE(),
                              -- New_Expiry_Date =NULL,
                              New_Lot_Assign_Status = NULL 
                        --Approve_By = NULL, 
                        --Approve_Dtm = NULL
                        WHERE Vaccine_Lot_No = @Vaccine_Lot_No
                              AND TSMP = @tsmp;
                    END;
            END; 
        ----------------------------------
        --- Cancel request
        ----------------------------------
        IF @Action_Type = 'C'
            BEGIN
                IF EXISTS
                (
                    SELECT Vaccine_Lot_No
                    FROM COVID19VaccineLotDetail
                    WHERE Vaccine_Lot_No = @Vaccine_Lot_No
                          AND Request_Type = 'N'
                )
                    BEGIN
                        DELETE FROM COVID19VaccineLotDetail
                        WHERE Vaccine_Lot_No = @Vaccine_Lot_No;
                    END;
                    ELSE
                    BEGIN
                        UPDATE COVID19VaccineLotDetail
                          SET 
                              New_Record_Status = NULL, 
                              Request_Type = NULL, 
                              Record_status = CASE
                                                  WHEN Request_Type = 'N'
                                                  THEN 'D'
                                                  WHEN Request_Type = 'R'
                                                       OR Request_Type = 'A'
                                                  THEN 'A'
                                              END, 
                              Update_by = @user_id, 
                              Update_Dtm = GETDATE(), 
                              Request_by = NULL, 
                              Request_Dtm = NULL,
                              -- New_Expiry_Date = NULL
                              New_Lot_Assign_Status = NULL
                        WHERE Vaccine_Lot_No = @Vaccine_Lot_No
                              AND TSMP = @tsmp;
                    END;
            END;
        ----------------------------------
        --- Edit (Remove)
        ----------------------------------
        IF @Action_Type = 'E'
            BEGIN
                UPDATE COVID19VaccineLotDetail
                  SET 
                      Request_Type = 'R', 
                      New_Record_status = 'P', 
                      New_Lot_Assign_Status = 'U', 
                      Update_by = @user_id, 
                      Update_Dtm = GETDATE(), 
                      Request_by = @user_id, 
                      Request_Dtm = GETDATE()
                WHERE Vaccine_Lot_No = @Vaccine_Lot_No
                      AND TSMP = @tsmp;
            END;
        ----------------------------------
        --- Edit (Update)
        ---------------------------------
        IF @Action_Type = 'U'
            BEGIN
                UPDATE COVID19VaccineLotDetail
                  SET 
                      New_Record_Status = 'P', 
                      Request_Type = 'A', 
                      New_Lot_Assign_Status = @lot_assign_status, 
                      Request_By = @user_id, 
                      Request_Dtm = GETDATE(), 
                      Update_By = @user_id, 
                      Update_Dtm = GETDATE()
                WHERE Vaccine_Lot_No = @vaccine_Lot_No
                      AND TSMP = @tsmp;
            END;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_upd] TO HCVU;
GO