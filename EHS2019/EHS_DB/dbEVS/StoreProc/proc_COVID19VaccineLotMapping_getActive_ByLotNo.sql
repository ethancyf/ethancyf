
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_getActive_ByLotNo]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_ByLotNo];
    END;
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE21-003 (To handle Centre ID with prefix 'DH')
-- Modified by:		Winnie SUEN
-- Modified date:	28 Sep 2021
-- Description:		Consider Centre ID with prefix "DH0" as 'DH CLINIC'
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023-59 
-- Modified by:		Winnie SUEN
-- Modified date:	28 Sep 2021
-- Description:		Handle New Prefix of Centre Code 'Q'
-- =============================================
-- =============================================  
-- Author:  Nichole Ip 
-- CR No.:  CRE20 - 023
-- Create date: 26 Mar 2021  
-- Description:  Get the active vaccine lot from lot mapping and vaccine lot request
-- =============================================  
--  
CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getActive_ByLotNo] @VaccineLotNo VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;  
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @Vaccine_Lot_ID CHAR(10);
        DECLARE @l_VaccineLotNo VARCHAR(20)= @VaccineLotNo;
        -- =============================================  
        -- Validation   
        -- =============================================  
        -- =============================================  
        -- Initialization  
        -- =============================================  
        -- =============================================  
        -- Return results  
        -- =============================================  

        SELECT DISTINCT 
               vaccine_lot_no, 
               service_type, 
               centre_id, 
               centre_name,
               CASE
					WHEN Centre_ID IS NULL OR service_type <> 'CENTRE' THEN '' 
					WHEN LEFT(centre_id, 3) = 'DH0' THEN 'DH CLINIC'
					ELSE 'CENTRE'
               END AS centre_service_type
        FROM
        (
            SELECT VLM.Vaccine_Lot_No, 
                   VLM.Service_Type, 
                   VLM.Centre_ID, 
                   VC.Centre_Name
            FROM COVID19VaccineLotMapping AS VLM WITH(NOLOCK)
                 INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK) ON VLD.Vaccine_Lot_No = VLM.Vaccine_Lot_No
                 LEFT JOIN VaccineCentre AS VC WITH(NOLOCK) ON VLM.Centre_ID = VC.Centre_ID
            WHERE VLM.Vaccine_Lot_No = @l_VaccineLotNo
                  AND VLM.Record_Status = 'A' -- not count on record has removed
                  AND ISNULL(VLM.Service_Period_To, VLD.Expiry_Date) >= CONVERT(date, GETDATE()) -- lot no asigned to any booth and still effective
            UNION ALL
            SELECT VLR.Vaccine_Lot_No, 
                   VLR.Service_Type, 
                   VLR.Centre_ID, 
                   VC.Centre_Name
            FROM COVID19VaccineLotMappingRequest AS VLR WITH(NOLOCK)
			INNER JOIN COVID19VaccineLotDetail AS VLD WITH(NOLOCK) ON VLD.Vaccine_Lot_No = VLR.Vaccine_Lot_No
                 LEFT JOIN VaccineCentre AS VC WITH(NOLOCK) ON VC.Centre_ID = VLR.Centre_ID
            WHERE VLR.Record_Status = 'P' -- pending  request
                  AND VLR.Vaccine_Lot_No = @l_VaccineLotNo
                 --AND ISNULL(VLR.Service_Period_To, VLD.Expiry_Date) >= CONVERT(date, GETDATE()) -- if null get the expirydate from detail
        ) AS tempSummary
        ORDER BY centre_name;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getActive_ByLotNo] TO HCVU;
GO