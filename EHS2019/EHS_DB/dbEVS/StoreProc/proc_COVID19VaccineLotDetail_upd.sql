
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
-- Description:		 Update the vaccine lot detail status
-- =============================================
-- exec proc_COVID19VaccineLotDetail_update 'LC0000015','E','HAADM1'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_upd] 
@Vaccine_Lot_No VARCHAR(20) = NULL, 
@Action_Type    VARCHAR(2), 
@update_by      VARCHAR(20) = NULL,
@request_by      VARCHAR(20) = NULL,
@tsmp timestamp
AS
    BEGIN

		-- =============================================
		-- Validation 
		-- =============================================
			IF (SELECT TSMP FROM COVID19VaccineLotDetail
				 WHERE Vaccine_Lot_No = @Vaccine_Lot_No) != @tsmp
			BEGIN
				RAISERROR('00011', 16, 1)
				return @@error
			END
			

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
                      New_Record_Status=NULL,
					  Record_status = CASE
                                          WHEN Request_Type = 'R'
                                          THEN 'D'
                                          ELSE 'A'
                                      END, 
                      Request_Type = NULL,  
                      Update_by = @update_by, 
                      Update_Dtm = GETDATE(), 
                      Approve_by = @update_by, 
                      Approve_Dtm = GETDATE()
                WHERE Vaccine_Lot_No = @Vaccine_Lot_No;
            END;
		----------------------------------
		--Reject
		----------------------------------
        IF @Action_Type = 'R'
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
                      Update_by = @update_by, 
                      Update_Dtm = GETDATE(), 
                      Approve_by = @update_by, 
                      Approve_Dtm = GETDATE()
                WHERE Vaccine_Lot_No = @Vaccine_Lot_No;
            END;
		----------------------------------
        --- Cancel request
		----------------------------------
        IF @Action_Type = 'C'
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
                      Update_by = @update_by, 
                      Update_Dtm = GETDATE(), 
                      Request_by = NULL, 
                      Request_Dtm = NULL
                WHERE Vaccine_Lot_No = @Vaccine_Lot_No;
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
                      Update_by = @update_by, 
                      Update_Dtm = GETDATE(), 
                      Request_by = @request_by , 
                      Request_Dtm = GETDATE()
                WHERE Vaccine_Lot_No = @Vaccine_Lot_No;
            END;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_upd] TO HCVU;
GO