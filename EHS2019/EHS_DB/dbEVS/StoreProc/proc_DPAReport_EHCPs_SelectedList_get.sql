
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_DPAReport_EHCPs_SelectedList_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_DPAReport_EHCPs_SelectedList_get];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Oct 2020
-- CR No.:			CRE20-015 (HA Scheme)
-- Description:		Enlarge size of [Scheme_Claim].[Scheme_Desc] from 100 -> 200
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	17 Aug 2020
-- CR No.:			CRE17-004
-- Description:		New DPAR Report (EHCP Basis)
--					1. Add "New Selected"
--					2. Add summary 
-- =============================================
-- =============================================
-- Author:			Dickson Law
-- Create date:		21 Feb 2018
-- Description:		New DPAR Report (EHCP Basis)
-- =============================================

CREATE PROCEDURE [dbo].[proc_DPAReport_EHCPs_SelectedList_get] @reimburse_id    CHAR(15), 
                                                               @cutoff_Date_str CHAR(11), 
                                                               @scheme_code     CHAR(10)
WITH RECOMPILE
AS
    BEGIN
        -- =============================================
        -- Declaration
        -- =============================================

        DECLARE @tempContent TABLE
        (Result_Value1 VARCHAR(100), 
         Result_Value2 VARCHAR(100)
        );

        DECLARE @tempLegend TABLE
        (Parameter VARCHAR(1000), 
         Value     VARCHAR(1000)
        );

        CREATE TABLE #ResultEHCP
        (SP_Seq_No         INT, 
         SP_ID             CHAR(8), 
         SP_Name           VARCHAR(40), 
         SP_Name_Chi       NVARCHAR(40), 
         Total_Transaction SMALLINT, 
         Total_Amount      MONEY, 
         Total_Amount_RMB  MONEY, 
         Verification_Case VARCHAR(1)
        );

        -- Create Worksheet 01 Result Table  
        CREATE TABLE #ws01
        (Seq_No         INT IDENTITY(1, 1), 
         _result_value1 VARCHAR(1000) DEFAULT '', 
         _result_value2 VARCHAR(1000) DEFAULT '', 
         _result_value3 VARCHAR(1000) DEFAULT '', 
         _result_value4 VARCHAR(1000) DEFAULT '', 
         _result_value5 VARCHAR(1000) DEFAULT '', 
         _result_value6 VARCHAR(1000) DEFAULT ''
        );

        DECLARE @strGenDtm VARCHAR(50);
        DECLARE @ReimbursementCurrency VARCHAR(10);

        --Reimbursement
        DECLARE @TotalSP INT;
        DECLARE @TotalSPPractice INT;
        DECLARE @TotalTransaction INT;
        DECLARE @TotalAmount MONEY;
        --EHCPs Selected for Checking
        DECLARE @TotalSelectedSP INT;
        DECLARE @TotalSelectedTransaction INT;
        DECLARE @TotalSelectedAmount MONEY;
        DECLARE @ReportDate DATETIME;

        DECLARE @strReportDate VARCHAR(50);
        DECLARE @strStatusOriginal CHAR(1);
        DECLARE @strStatusNewSelected CHAR(1);

        DECLARE @In_reimburse_id CHAR(15)= @reimburse_id;
        DECLARE @In_cutoff_Date_str CHAR(11)= @cutoff_Date_str;
        DECLARE @In_scheme_code CHAR(10)= @scheme_code;

        -- =============================================
        -- Validation 
        -- =============================================
        -- =============================================
        -- Initialization
        -- =============================================

        SELECT @ReimbursementCurrency = Reimbursement_Currency
        FROM SchemeClaim
        WHERE Scheme_Code = @In_scheme_code;
        SET @strStatusOriginal = 'O';
        SET @strStatusNewSelected = 'N';
        -----------------------------------    
        -- Result Table 1: Content    
        -----------------------------------   

        SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108);
        SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm) - 3);
        INSERT INTO @tempContent
               (Result_Value1, 
                Result_Value2
               )
        VALUES
              ('Report Generation Time:' + @strGenDtm, 
               ''
              );

        -----------------------------------    
        -- Result Table 2: Verification List    
        -----------------------------------   
        --result table
        OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key;

        -- Insert this reimbursement exist SP
        INSERT INTO #ResultEHCP
               (SP_Seq_No, 
                SP_ID, 
                SP_Name, 
                SP_Name_Chi, 
                Verification_Case
               )
        SELECT RSP.Seq_No, 
               RSP.SP_ID,
               CASE @ReimbursementCurrency
                   WHEN 'HKD'
                   THEN CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
                   ELSE NULL
               END,
               CASE @ReimbursementCurrency
                   WHEN 'HKDRMB'
                   THEN CASE ISNULL(CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3)), '')
                            WHEN ''
                            THEN CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
                            ELSE CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3))
                        END
                   ELSE NULL
               END, 
               rsp.Verification_Case
        FROM ReimbursementServiceProvider AS RSP
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON RSP.SP_ID = SP.SP_ID
        WHERE Reimburse_ID = @In_reimburse_id
              AND Scheme_Code = @In_scheme_code;
        --AND Verification_Case = 'Y'              
        CLOSE SYMMETRIC KEY sym_Key;

        -- Insert this reimbursement exist SP total tran and amount
        UPDATE #ResultEHCP
          SET Total_Transaction = totalTran, 
              Total_Amount = totalAmount, 
              Total_Amount_RMB = totalAmountRMB
        FROM
        (
            SELECT VT.SP_ID, 
                   COUNT(DISTINCT VT.Transaction_ID) AS totalTran, 
                   SUM(Total_Amount) AS totalAmount, 
                   SUM(Total_Amount_RMB) AS totalAmountRMB
            FROM ReimbursementAuthTran AS RAT
                 INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
                 ON RAT.Transaction_ID = VT.Transaction_ID
                 INNER JOIN TransactionDetail AS TD WITH(NOLOCK)
                 ON RAT.Transaction_ID = TD.Transaction_ID
            WHERE RAT.Reimburse_ID = @In_reimburse_id
                  AND RAT.Scheme_Code = @In_scheme_code
            GROUP BY VT.SP_ID
        ) T2
        WHERE #ResultEHCP.SP_ID = T2.SP_ID;

        SELECT @ReportDate = Create_Dtm
        FROM ReimbursementAuthorisation
        WHERE Reimburse_ID = @In_reimburse_id
              AND Scheme_Code = @In_scheme_code;

        SET @strReportDate = CONVERT(VARCHAR(11), @ReportDate, 111) + ' ' + CONVERT(VARCHAR(20), @ReportDate, 108);
        SET @strReportDate = LEFT(@strReportDate, LEN(@strReportDate) - 3);

        --The Summary
        INSERT INTO #ws01(_result_value1)
        VALUES('Reimbuserment ID: ' + @In_reimburse_id);
        INSERT INTO #ws01(_result_value1)
        VALUES('Scheme: ' + @In_scheme_code);
        INSERT INTO #ws01(_result_value1)
        VALUES('Cutoff Date: ' + FORMAT(CONVERT(DATETIME, @In_cutoff_Date_str, 111), 'dd MMM yyyy', 'en-US'));
        INSERT INTO #ws01(_result_value1)
        VALUES('Search and Hold Date: ' + @strReportDate);
        INSERT INTO #ws01(_result_value1)
        VALUES('Reimbursement:');

        SELECT @TotalSP = COUNT(DISTINCT SP_ID)
        FROM #ResultEHCP AS r;

        INSERT INTO #ws01(_result_value2)
        VALUES('Total No. of Service Provider ID: ' + CONVERT(VARCHAR(20), @TotalSP));

        SELECT @TotalSPPractice = SUM(PracticeCount)
        FROM
        (
            SELECT RAT.SP_ID, 
                   COUNT(DISTINCT RAT.Practice_Display_Seq) AS PracticeCount
            FROM ReimbursementAuthTran AS RAT
            WHERE RAT.Reimburse_ID = @In_reimburse_id
                  AND RAT.Scheme_Code = @In_scheme_code
            GROUP BY SP_ID
        ) AS T1;

        INSERT INTO #ws01(_result_value2)
        VALUES('Total No. of Service Provider ID (Practice number): ' + CONVERT(VARCHAR(20), @TotalSPPractice));

        SELECT @TotalTransaction = SUM(Total_Transaction)
        FROM #ResultEHCP;

        INSERT INTO #ws01(_result_value2)
        VALUES('Total No. of Transactions: ' + CONVERT(VARCHAR(20), @TotalTransaction));

        SELECT @TotalAmount = SUM(Total_Amount)
        FROM #ResultEHCP;

        INSERT INTO #ws01(_result_value2)
        VALUES('Total Amount Claimed ($): ' + replace((CONVERT(NVARCHAR(20), @TotalAmount, 1)), '.00', ''));

        INSERT INTO #ws01(_result_value1)
        VALUES('EHCPs Selected for Checking:');

        SELECT @TotalSelectedSP = COUNT(DISTINCT SP_ID)
        FROM #ResultEHCP
        WHERE Verification_Case = @strStatusOriginal
              OR Verification_Case = @strStatusNewSelected;
        INSERT INTO #ws01(_result_value2)
        VALUES('Total No. of EHCPs: ' + CONVERT(VARCHAR(20), @TotalSelectedSP));

        SELECT @TotalSelectedTransaction = SUM(Total_Transaction)
        FROM #ResultEHCP
        WHERE Verification_Case = @strStatusOriginal
              OR Verification_Case = @strStatusNewSelected;

        INSERT INTO #ws01(_result_value2)
        VALUES('Total No. of Transactions: ' + CONVERT(VARCHAR(20), @TotalSelectedTransaction));

        SELECT @TotalSelectedAmount = SUM(Total_Amount)
        FROM #ResultEHCP
        WHERE Verification_Case = @strStatusOriginal
              OR Verification_Case = @strStatusNewSelected;

        INSERT INTO #ws01(_result_value2)
        VALUES('Total Amount Claimed ($):  ' + replace((CONVERT(NVARCHAR(20), @TotalSelectedAmount, 1)), '.00', ''));

        INSERT INTO #ws01(_result_value1)
        VALUES('');

        --The list of EHCPs selected fot reimbursement checking
        INSERT INTO #ws01
               (_result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4, 
                _result_value5, 
                _result_value6
               )
        VALUES
              ('No.', 
               'Service Provider ID', 
               'Service Provider Name', 
               'No. of Transaction', 
               'Amount Claimed ($)', 
               'New Selected'
              );

        INSERT INTO #ws01
               (_result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4, 
                _result_value5, 
                _result_value6
               )
        SELECT SP_Seq_No, 
               SP_ID, 
               SP_Name, 
               Total_Transaction, 
               Total_Amount,
               CASE
                   WHEN Verification_Case = @strStatusNewSelected
                   THEN 'Y'
                   ELSE ''
               END
        FROM #ResultEHCP
        WHERE Verification_Case = @strStatusOriginal
              OR Verification_Case = @strStatusNewSelected
        ORDER BY SP_Seq_No;

        --Scheme Legend
        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        VALUES
              ('Scheme Legend:', 
               ''
              );
        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        SELECT RTRIM(display_Code), 
               scheme_desc
        FROM SchemeClaim
        GROUP BY display_Code, 
                 scheme_desc, 
                 display_seq
        ORDER BY display_seq;

        -- =============================================        
        -- Return results        
        -- =============================================  
        -----------------------------------    
        --  Result Table 1: Content
        -----------------------------------  
        SELECT Result_Value1, 
               Result_Value2
        FROM @tempContent;

        -----------------------------------    
        -- Result Table 2: Verification List    
        -----------------------------------   
        SELECT _result_value1, 
               _result_value2, 
               _result_value3, 
               _result_value4, 
               _result_value5, 
               _result_value6
        FROM #ws01
        ORDER BY Seq_No;

        -----------------------------------    
        -- Result Table 3: Legend    
        -----------------------------------      

        SELECT Parameter AS [Legend], 
               Value
        FROM @tempLegend;

        -- =============================================        
        -- House Keeping        
        -- =============================================   
        DROP TABLE #ResultEHCP;
        DROP TABLE #ws01;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_DPAReport_EHCPs_SelectedList_get] TO HCVU;
GO