
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_update]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_update];
    END;
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-022 (Immu record)
-- Modified by:		Nichole Ip
-- Modified date:	20 Feb 2021
-- Description:		from the vaccine lot request to update COVID19VaccineLotMapping record
-- =============================================
-- exec proc_COVID19VaccineLotMapping_update 'LC0000015','E','HAADM1'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_update] @request_id  VARCHAR(20) = NULL, 
                                                              @action_type VARCHAR(2), 
                                                              @user_id     VARCHAR(20) = NULL, 
                                                              @tsmp        TIMESTAMP
AS
    BEGIN
        BEGIN
            SET NOCOUNT ON;   
            -- =============================================
            -- Validation 
            -- =============================================
            IF
            (
                SELECT TSMP
                FROM COVID19VaccineLotMappingRequest
                WHERE Request_ID = @request_id
            ) != @tsmp
                BEGIN
                    RAISERROR('00011', 16, 1);
                    RETURN @@error;
                END;

            -- =============================================
            -- Declaration 
            -- =============================================
            DECLARE @RequestTypeRemove AS CHAR(1)= 'R';
            DECLARE @RequestTypeNew AS CHAR(1)= 'N';
            DECLARE @RequestTypeAmend AS CHAR(1)= 'A';
            DECLARE @RecordStatusActive AS CHAR(1)= 'A';
            DECLARE @RecordStatusPendingApproval AS CHAR(1)= 'P';
            DECLARE @RecordStatusRemoved AS CHAR(1)= 'D';
            DECLARE @l_request_id VARCHAR(20);
            DECLARE @centre_id AS VARCHAR(10);
            DECLARE @vaccine_lot_no AS VARCHAR(20);
            DECLARE @booth_no AS VARCHAR(1000);
            DECLARE @count INT;
            DECLARE @countRow INT;
            DECLARE @request_type AS VARCHAR(1);
            DECLARE @New_Lot_ID CHAR(20);
            DECLARE @Profile_ID CHAR(10);
            DECLARE @requested_by VARCHAR(20);

            -- =============================================  
            -- Initialization  
            -- =============================================  
            --DECLARE @delimiter VARCHAR(10);
            SET @l_request_id = @request_id;
            SET @Profile_ID = 'VLM';
            SELECT ROW_NUMBER() OVER(
                   ORDER BY VLMR.Booth_no) AS id, 
                   VLMR.Booth_no, 
                   VLMR.Centre_id, 
                   VLMR.Vaccine_lot_no, 
                   VLMR.Request_Type, 
                   VLMR.Requested_By
            INTO #tempMappingRequest
            FROM
            (
                SELECT Booth.Item AS [Booth_no], 
                       Request_id, 
                       Centre_id, 
                       Vaccine_lot_no, 
                       Request_Type, 
                       Requested_By
                FROM COVID19VaccineLotMappingRequest AS VMR
                     OUTER APPLY func_split_string(VMR.Booth, ',') AS Booth
                WHERE Request_Id = @request_id
            ) AS VLMR;
            SELECT *
            FROM #tempMappingRequest;
            SELECT TOP 1 @request_type = Request_type
            FROM #tempMappingRequest;
            SET @count = 1;
            SELECT @countRow = COUNT(*)
            FROM #tempMappingRequest;

            -- =============================================
            -- Return results
            -- =============================================
            ----------------------------------
            --- Approve
            ----------------------------------
            IF @action_type = 'A'
                BEGIN --if(1)
                    WHILE @count <= @countRow
                        BEGIN -- while start
                            SELECT @centre_id = centre_id, 
                                   @vaccine_lot_no = vaccine_lot_no, 
                                   @booth_no = booth_no, 
                                   @requested_by = requested_by
                            FROM #tempMappingRequest
                            WHERE id = @count;
                            SET @count = @count + 1;
                            IF EXISTS
                            (
                                SELECT TOP 1 *
                                FROM COVID19VaccineLotMapping
                                WHERE Centre_ID = @centre_id
                                      AND Vaccine_Lot_No = @vaccine_lot_no
                                      AND Booth = @booth_no 
                            --AND Record_Status ='A'
                            )
                                BEGIN -- if (exists)
                                    IF @request_type = 'A'
                                       OR @request_type = 'N'
                                        BEGIN --if(3)
                                            -- new assign
                                            -- update Lotmapping
                                            UPDATE COVID19VaccineLotMapping
                                              SET Service_Period_From = lmr.Service_Period_From, 
                                                  Service_Period_To = lmr.Service_Period_To, 
                                                  Update_By = @user_id, 
                                                  Update_Dtm = GETDATE(), 
                                                  Record_Status = 'A'
                                            FROM COVID19VaccineLotMapping VLM
                                                 INNER JOIN COVID19VaccineLotMappingRequest lmr
                                                 ON lmr.Request_id = @request_id
                                            WHERE VLM.Centre_id = @centre_id
                                                  AND VLM.Vaccine_Lot_no = @vaccine_lot_no
                                                  AND VLM.booth = @booth_no;
                                        END;--if(3)
                                        ELSE
                                        BEGIN --if(3)
                                            -- remove
                                            UPDATE COVID19VaccineLotMapping
                                              SET Record_status = 'D', 
                                                  Update_By = @user_id, 
                                                  Update_Dtm = GETDATE()
                                            FROM COVID19VaccineLotMapping VLM
                                                 INNER JOIN COVID19VaccineLotMappingRequest lmr
                                                 ON lmr.Request_id = @request_id
                                            WHERE VLM.Centre_id = @centre_id
                                                  AND VLM.Vaccine_Lot_no = @vaccine_lot_no
                                                  AND VLM.booth = @booth_no;
                                        END;--if(3)
                                    --update LotmappingRequest
                                    UPDATE COVID19VaccineLotMappingRequest
                                      SET Approved_by = @user_id, 
                                          Approved_Dtm = GETDATE(), 
                                          Record_status = 'A'
                                    WHERE Request_Id = @request_id;
                                END; --if (exists)
                                ELSE
                                BEGIN -- if (not exists)

                                    IF @request_type <> 'R'

                                    --- if the request type is not Remove 
                                        BEGIN
                                            -- not exist on the mapping table
                                            -- get the latest Lot ID
                                            BEGIN TRANSACTION;
                                            EXEC proc_SystemProfile_get_byProfileID_withOutputParm @Profile_ID, 
                                                                                                   @New_Lot_ID OUTPUT;
                                            COMMIT TRANSACTION;
                                            --SELECT @New_Lot_ID = 'LC' + CONVERT(CHAR(8), GETDATE(), 112) + RIGHT('0000000000' + RTRIM(LTRIM(
                                            --@New_Lot_ID)), 5);

											Select  @New_Lot_ID ='LC' + convert(char(8),getdate(),112) + RIGHT('0000000000' + RTRIM(LTRIM(@New_Lot_ID)), 5);

                                            --- insert into the Mapping table
                                            INSERT INTO [dbo].[COVID19VaccineLotMapping]
                                                   ([Vaccine_Lot_ID], 
                                                    [Vaccine_Lot_No], 
                                                    [Service_Type], 
                                                    [Centre_ID], 
                                                    [Booth], 
                                                    [Service_Period_From], 
                                                    [Service_Period_To], 
                                                    [Record_status], 
                                                    --[Lot_status], --remove it later
                                                    [Create_By], 
                                                    [Create_Dtm], 
                                                    [Update_By], 
                                                    [Update_dtm]
                                                   )
                                            SELECT @New_Lot_ID, 
                                                   Vaccine_Lot_No, 
                                                   Service_Type, 
                                                   Centre_ID, 
                                                   @booth_no, 
                                                   Service_Period_From, 
                                                   Service_Period_To, 
                                                   'A', 
                                                   --'A', 
                                                   @requested_by, 
                                                   GETDATE(), 
                                                   @user_id, 
                                                   GETDATE()
                                            FROM COVID19VaccineLotMappingRequest
                                            WHERE Request_ID = @request_id;
                                            ---update the request table

                                            UPDATE COVID19VaccineLotMappingRequest
                                              SET Approved_by = @user_id, 
                                                  Approved_Dtm = GETDATE(), 
                                                  Record_status = 'A'
                                            WHERE Request_Id = @request_id;
                                        END;
                                        ELSE
                                        BEGIN
                                            ---update the request table

                                            UPDATE COVID19VaccineLotMappingRequest
                                              SET Approved_by = @user_id, 
                                                  Approved_Dtm = GETDATE(), 
                                                  Record_status = 'A'
                                            WHERE Request_Id = @request_id;
                                        END;
                                END; -- if (not exists)
                        END;
                END; -- while end
        END;--if(1)
        ----------------------------------
        --- Reject
        ----------------------------------
        IF @action_type = 'R'
            BEGIN
                UPDATE COVID19VaccineLotMappingRequest
                  SET Rejected_by = @user_id, 
                      Rejected_Dtm = GETDATE(), 
                      Record_status = 'A'
                WHERE Request_Id = @request_id;
            END;
        DROP TABLE #tempMappingRequest;
        END;
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_update] TO HCVU;
GO