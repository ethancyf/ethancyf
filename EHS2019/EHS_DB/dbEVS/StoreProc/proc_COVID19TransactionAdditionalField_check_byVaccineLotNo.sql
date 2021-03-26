
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19TransactionAdditionalField_check_byVaccineLotNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19TransactionAdditionalField_check_byVaccineLotNo];
    END;
GO

-- =============================================  
-- Author:  Nichole Ip 
-- CR No.:  CRE20 - 022 
-- Create date: 02 Mar 2021  
-- Description: Check If the VaccineLotNo is being used in any TransactionAdditionalField  
-- =============================================  
CREATE PROCEDURE [dbo].[proc_COVID19TransactionAdditionalField_check_byVaccineLotNo] 
@VaccineLotNo VARCHAR(20)
AS
    BEGIN
        SET NOCOUNT ON;  
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @Transaction_ID CHAR(20);

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

        SELECT TOP 1 @Transaction_ID = Transaction_ID
        FROM TransactionAdditionalField WITH(NOLOCK)
        WHERE AdditionalFieldID = 'VaccineLotNO'
              AND AdditionalFieldValueCode = @l_VaccineLotNo;

        IF @Transaction_ID IS NULL
            BEGIN
                SELECT 0;
            END;
            ELSE
            BEGIN
                SELECT 1;
            END;
    END;


		GO
GRANT EXECUTE ON [dbo].[proc_COVID19TransactionAdditionalField_check_byVaccineLotNo] TO HCVU;
GO