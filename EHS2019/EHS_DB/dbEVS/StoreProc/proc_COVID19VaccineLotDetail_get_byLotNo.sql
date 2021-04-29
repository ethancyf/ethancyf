
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotDetail_get_ByLotNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_get_ByLotNo];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Raiman Chong  
-- Create date:  12 Mar 2021   
-- Description: Get the vaccine lot detail by lot No
-- =============================================  
--exec proc_COVID19VaccineLotDetail_get_ByLotNo '1','A2021010022'
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotDetail_get_ByLotNo] @brand          VARCHAR(10) = NULL, 
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
               VLD.[Brand_ID] AS [Brand_ID], 
               VBD.[Brand_Name] AS [Brand_Name], 
               VBD.[Brand_Trade_Name] AS [Brand_Trade_Name], 
               VLD.[Expiry_date] AS [Expiry_Date], 
               --ISNULL(VLD.[New_Record_status], VLD.[Record_status]) AS [Record_Status],
               --VLD.[Record_status] AS [Record_Status], 
                Record_status = CASE
                                   WHEN VLD.[New_Record_status] IS NOT NULL
                                   THEN VLD.[New_Record_status] --pending (P)
                                   WHEN VLD.[Expiry_date] < CONVERT(DATE, GETDATE())
                                        AND VLD.[Record_status] = 'A' AND VLD.[New_Record_status] IS NULL
                                   THEN 'E' --expired (E)
                                   ELSE VLD.Record_Status -- active/removed (A/D)
                               END, 
			   VLD.[New_Record_status] AS [New_Record_Status], 
               --VLD.[Lot_Assign_status] AS [Lot_Assign_status], 
               Lot_Assign_Status = CASE
                                       WHEN VLD.[Expiry_date] < CONVERT(DATE, GETDATE())
                                            AND VLD.[Record_status] = 'A' AND VLD.[New_Record_status] IS NULL
                                       THEN 'U' --expired [U: Unavailable to assign]
                                       ELSE VLD.[Lot_Assign_Status]
                                   END,
			   VLD.[New_Lot_Assign_status] AS [New_Lot_Assign_status], 
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
             INNER JOIN COVID19VaccineBrandDetail AS VBD WITH(NOLOCK) ON VLD.Brand_ID = VBD.Brand_ID
        WHERE VLD.[Vaccine_Lot_No] = @l_vaccine_lot_no
              AND (VLD.Brand_ID = @l_brand
                   OR @l_brand IS NULL);
        -- AND VLD.[Record_Status] <> 'D';
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotDetail_get_ByLotNo] TO HCVU;
GO