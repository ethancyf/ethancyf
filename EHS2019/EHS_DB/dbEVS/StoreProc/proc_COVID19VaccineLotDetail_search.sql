
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotDetail_search]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_search];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022 (Vaccine Lot Creation)   
-- Author:  Nichole Ip
-- Create date:  12 Mar 2021   
-- Description: Get the vaccine lot detail by several criteria
-- =============================================  
--exec proc_COVID19VaccineLotDetail_search NULL,'A2021010022',NULL,NULL,NULL,0,0,0
-- exec proc_COVID19VaccineLotDetail_search NULL,NULL,NULL,NULL,NULL,0,0,0
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_search] @brand                        VARCHAR(10) = NULL, 
                                                             @vaccine_lot_no               VARCHAR(20) = NULL, 
                                                             @expiry_date_from             DATETIME    = NULL, 
                                                             @expiry_date_to               DATETIME    = NULL, 
                                                             @record_status                VARCHAR(1)  = NULL, 
                                                             @result_limit_1st_enable      BIT, 
                                                             @result_limit_override_enable BIT, 
                                                             @override_result_limit        BIT
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_brand VARCHAR(10);
        DECLARE @l_vaccine_lot_no VARCHAR(20);
        DECLARE @l_expiry_date_from DATETIME;
        DECLARE @l_expiry_date_to DATETIME;
        DECLARE @l_record_status VARCHAR(1);
        SET @l_brand = @brand;
        SET @l_vaccine_lot_no = @vaccine_lot_no;
        SET @l_expiry_date_from = @expiry_date_from;
        SET @l_expiry_date_to = @expiry_date_to;
        SET @l_record_status = @record_status;
        -- =============================================  
        -- Initialization  
        -- =============================================  
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT VLD.[Vaccine_Lot_No] AS [Vaccine_Lot_No], 
               VLD.[Brand_ID] AS [Brand_ID], 
               VBD.[Brand_Name] AS [Brand_Name], 
               VLD.[Expiry_date] AS [Expiry_Date], 
               VBD.[Brand_Trade_Name] AS [Brand_Trade_Name], 
               -- ISNULL(VLD.[New_Record_status], VLD.[Record_status]) AS [Record_Status], 
               Record_status = CASE
                                   WHEN VLD.[New_Record_status] IS NOT NULL
                                   THEN VLD.[New_Record_status] --pending (P)
                                   WHEN VLD.[Expiry_date] < CONVERT(DATE, GETDATE())
                                        AND VLD.[Record_status] = 'A' AND VLD.[New_Record_status] IS NULL
                                   THEN 'E' --expired (E)
                                   ELSE VLD.Record_Status -- active/removed (A/D)
                               END, 
               VLD.[New_Record_status] AS [New_Record_Status], 
               VLD.[Create_By] AS [Create_By], 
               VLD.[Create_Dtm] AS [Create_Dtm], 
               VLD.[Update_By] AS [Update_By], 
               VLD.[Update_dtm] AS [Update_Dtm], 
               VLD.[TSMP] AS [TSMP], 
               ISNULL(VLD.[Request_Type], '') AS [Request_Type], 
               VLD.[New_Expiry_date] AS [New_Expiry_date], 
               VLD.[Request_By] AS [Request_by], 
               VLD.[Request_Dtm] AS [Request_Dtm], 
               VLD.[Approve_By] AS [Approve_By], 
               VLD.[Approve_Dtm] AS [Approve_Dtm], 
               --VLD.[Lot_Assign_Status] AS [Lot_Assign_Status], 
               Lot_Assign_Status = CASE
                                       WHEN VLD.[Expiry_date] < CONVERT(DATE, GETDATE())
                                            AND VLD.[Record_status] = 'A' AND VLD.[New_Record_status] IS NULL
                                       THEN 'U' --expired set lot assign status to [U: Unavailable to assign]
                                       ELSE VLD.[Lot_Assign_Status]
                                   END, 
               VLD.[New_Lot_Assign_Status] AS [New_Lot_Assign_Status]
        FROM COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             INNER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK) ON VLD.Brand_ID = VBD.Brand_ID
        WHERE(VLD.Brand_ID = @l_brand
              OR @l_brand IS NULL)
             AND (VLD.[Vaccine_Lot_No] LIKE '%' + @l_vaccine_lot_no + '%'
                  OR @l_vaccine_lot_no IS NULL)
             AND ((@l_expiry_date_from IS NULL
                   AND @l_expiry_date_to IS NULL)
                  OR VLD.[Expiry_Date] BETWEEN @l_expiry_date_from AND @l_expiry_date_to)
             AND (
				--(@l_record_status IS NULL AND record_status <> 'D') -- any record
				(@l_record_status IS NULL ) -- any record
                  OR (@l_record_status = 'P' AND new_record_status = 'P') --pending record
                  OR (@l_record_status = 'A' AND record_status = 'A' AND New_Record_Status IS NULL AND expiry_date >= CONVERT(DATE, GETDATE())) -- active record without expired
                  OR (@l_record_status = 'E' AND record_status = 'A' AND New_Record_Status IS NULL AND expiry_date < CONVERT(DATE, GETDATE()))  -- expired record
                  OR (@l_record_status = 'D' AND record_status = 'D') --removed record
             )
        ORDER BY VBD.[Brand_Trade_Name], 
                 VLD.[Expiry_date], 
                 VLD.[Vaccine_Lot_No];
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_search] TO HCVU;
GO