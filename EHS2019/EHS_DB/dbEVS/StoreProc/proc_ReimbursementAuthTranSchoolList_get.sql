
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTranSchoolList_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ReimbursementAuthTranSchoolList_get];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:			Martin Tang
-- Create date:		09 Sep 2020
-- CR No.:			CRE20-003
-- Description:		Break Down of Monthly Statement by Schools
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementAuthTranSchoolList_get] @sp_id                CHAR(8), 
                                                                  @practice_display_seq SMALLINT, 
                                                                  @reimburse_id         CHAR(15), 
                                                                  @scheme_code          CHAR(10)
AS
    BEGIN

        -- =============================================
        -- Declaration
        -- =============================================
        DECLARE @In_sp_id CHAR(8)= @sp_id;
        DECLARE @In_practice_display_seq SMALLINT= @practice_display_seq;
        DECLARE @In_reimburse_id CHAR(15)= @reimburse_id;
        DECLARE @In_scheme_code CHAR(10)= @scheme_code;

        -- =============================================
        -- Initialization
        -- =============================================
        -- =============================================
        -- Return results
        -- =============================================

        SELECT s.School_Code, 
               rat.Scheme_Code, 
               s.Name_Chi AS SchoolName_Chi, 
               s.Name_Eng AS SchoolName_Eng, 
               COUNT(vt.Transaction_ID) AS No_of_Transaction, 
               CONVERT(INT, SUM(td.Total_Amount)) AS Total_Amount
        FROM ReimbursementAuthTran AS rat
             INNER JOIN VoucherTransaction AS vt
             ON rat.Transaction_ID = vt.Transaction_ID
                AND vt.SP_ID = @In_sp_id
                AND vt.Practice_Display_Seq = @In_practice_display_seq
             INNER JOIN TransactionAdditionalField AS taf
             ON vt.Transaction_ID = taf.Transaction_ID
                AND taf.AdditionalFieldID = 'SchoolCode'
             INNER JOIN School AS s
             ON s.School_Code = taf.AdditionalFieldValueCode
             INNER JOIN TransactionDetail AS td
             ON vt.Transaction_ID = td.Transaction_ID
        WHERE rat.Reimburse_ID = @In_reimburse_id
              AND rat.Scheme_Code = @In_scheme_code
        GROUP BY s.School_Code, 
                 s.Name_Chi, 
                 s.Name_Eng, 
                 rat.Scheme_Code;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTranSchoolList_get] TO HCSP;
GO