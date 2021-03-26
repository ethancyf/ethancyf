
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotDetail_get_byLotNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_get_byLotNo];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Raiman Chong  
-- Create date:  12 Mar 2021   
-- Description: Get the vaccine lot detail by lot ID
-- =============================================  
--exec proc_COVID19VaccineLotDetail_get_byLotNo '1','A2021010022'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_get_byLotNo] @brand          VARCHAR(10) = NULL, 
                                                                 @vaccine_lot_no VARCHAR(20) = NULL
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_brand VARCHAR(10);
        DECLARE @l_vaccine_lot_no VARCHAR(20);
        SET @l_brand = @brand;
        SET @l_vaccine_lot_no = @vaccine_lot_no;

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
             INNER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK)
             ON VLD.Brand_ID = VBD.Brand_ID
        WHERE VLD.Brand_ID = @l_brand
              AND VLD.[Vaccine_Lot_No] = @l_vaccine_lot_no
              AND VLD.[Record_Status] <> 'D';
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_get_byLotNo] TO HCVU;
GO