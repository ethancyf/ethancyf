
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
-- CR No.:  CRE20-022 (Vaccine Lot Management)   
-- Author:  Nichole Ip
-- Create date:  12 Mar 2021   
-- Description: Get the vaccine lot detail by brand, lot no
-- =============================================  
--exec proc_COVID19VaccineLotDetailList_get NULL,'A2021010022',NULL,NULL,NULL,0,0,0
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_search] @brand                        VARCHAR(10) = NULL, 
                                                              @vaccine_lot_no               VARCHAR(20)  = NULL, 
                                                              @expiry_date_from             DATETIME  = NULL, 
                                                              @expiry_date_to               DATETIME  = NULL, 
                                                              @new_record_status            VARCHAR(1)  = NULL,
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
        DECLARE @l_new_record_status VARCHAR(1);
        SET @l_brand = @brand;
        SET @l_vaccine_lot_no = @vaccine_lot_no;
        SET @l_expiry_date_from = @expiry_date_from;
        SET @l_expiry_date_to = @expiry_date_to;
        SET @l_new_record_status = @new_record_status;

        -- =============================================  
        -- Initialization  
        -- =============================================  
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT VLD.[Vaccine_Lot_No] AS [Vaccine_Lot_No], 
               VLD.[Brand_ID] AS [BrandID], 
               VBD.[Brand_Name] AS [BrandName], 
               VLD.[Expiry_date] AS [ExpiryDate], 
               ISNULL(VLD.[New_Record_status], VLD.[Record_status]) AS [Record_Status], 
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
               VLD.[Approve_Dtm] AS [Approve_Dtm]
        FROM COVID19VaccineLotDetail AS VLD WITH(NOLOCK)
             INNER JOIN COVID19VaccineBrandDetail AS VBD  WITH(NOLOCK) ON VLD.Brand_ID = VBD.Brand_ID
        WHERE(VLD.Brand_ID = @l_brand
              OR @l_brand IS NULL)
             AND (VLD.[Vaccine_Lot_No] LIKE '%' + @l_vaccine_lot_no + '%'
                  OR @l_vaccine_lot_no IS NULL)
             AND ((@l_expiry_date_from IS NULL
                   AND @l_expiry_date_to IS NULL)
                  OR VLD.[Expiry_Date] BETWEEN @l_expiry_date_from AND @l_expiry_date_to)
             AND VLD.[Record_Status] <> 'D'
             AND ((VLD.[New_Record_status] = @l_new_record_status and @l_new_record_status = 'P')
				  Or (@l_new_record_status = 'A' and VLD.New_Record_Status is null)
                  OR @l_new_record_status IS NULL);
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_search] TO HCVU;
GO