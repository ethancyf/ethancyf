
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementFiles_get_byReimbID_SSSCMC]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [proc_ReimbursementFiles_get_byReimbID_SSSCMC];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	7 Dec 2020
-- CR No.			CRE20-015 (HA Scheme)
-- Description:		Add [Sub-Specialities] in [02] worksheet
-- =============================================
-- =============================================        
-- Author:   Martin Tang     
-- CR No.:		CRE20-015 special Support Scheme
-- Create date:  10 Nov 2020          
-- Description:  REF: proc_ReimbursementFiles_get_byReimbI
-- =============================================  
-- ============================================= 
--EXEC [dbo].[proc_ReimbursementFiles_get_byReimbID_SSSCMC] @reimburse_id = 'R202000213', 
--                                                          @cutoff_Date_str = '06 Nov 2020 23:59', 
--                                                          @scheme_code = 'SSSCMC', 
--                                                          @file_id = 'PreAuthorizationCheck';

CREATE PROCEDURE [dbo].[proc_ReimbursementFiles_get_byReimbID_SSSCMC] @reimburse_id    CHAR(15), 
                                                                      @cutoff_Date_str CHAR(11), 
                                                                      @scheme_code     CHAR(10), 
                                                                      @file_id         VARCHAR(50)
AS
    BEGIN

        -- =============================================
        -- Declaration
        -- =============================================               
        DECLARE @display_code CHAR(25);
        DECLARE @strGenDtm VARCHAR(50);
        DECLARE @intSummaryFirstRow SMALLINT= 17;
        DECLARE @intRowCount INT;

        DECLARE @tempLegend TABLE
        (Parameter NVARCHAR(1000), 
         Value     NVARCHAR(1000)
        );

        DECLARE @tempContent TABLE
        (Result_Value1 NVARCHAR(100), 
         Result_Value2 NVARCHAR(100)
        );

        DECLARE @tempSummary TABLE
        (Parameter NVARCHAR(100), 
         Value     VARCHAR(100)
        );

        DECLARE @WS01 AS TABLE
        (Seq   INT IDENTITY(1, 1), 
         Col01 VARCHAR(100) DEFAULT '', 
         Col02 NVARCHAR(100) DEFAULT '', 
         Col03 NVARCHAR(100) DEFAULT '', 
         Col04 NVARCHAR(100) DEFAULT '', 
         Col05 VARCHAR(100) DEFAULT '', 
         Col06 VARCHAR(100) DEFAULT '', 
         Col07 VARCHAR(100) DEFAULT '', 
         Col08 VARCHAR(100) DEFAULT '', 
         Col09 VARCHAR(100) DEFAULT ''
        );

        CREATE TABLE #temp01
        (SP_ID_Practice       VARCHAR(100), 
         SP_Name              VARCHAR(40), 
         SP_Name_Chi          NVARCHAR(40), 
         Practice_Name        NVARCHAR(100), 
         Practice_Name_Chi    NVARCHAR(100), 
         Total_Amount_RMB     MONEY, 
         Total_SupportFee     MONEY, 
         total_reduction_fee  MONEY, 
         Scheme_Code          CHAR(10), 
         Practice_Display_Seq SMALLINT, 
         SP_ID                CHAR(8), 
         Total_Transaction    SMALLINT
        );

        CREATE TABLE #temp02
        (SP_ID_Practice       VARCHAR(100), 
         SP_Name              VARCHAR(40), 
         SP_Name_Chi          NVARCHAR(40), 
         Practice_Name        NVARCHAR(100), 
         Practice_Name_Chi    NVARCHAR(100),
		 SubSpecialities_Code VARCHAR(10),
		 SubSpecialities_Name_CN  NVARCHAR(100),
         Transaction_ID       CHAR(20), 
         Total_Amount_RMB     MONEY, 
         Total_SupportFee     MONEY, 
         total_reduction_fee  MONEY, 
         Transaction_Dtm      DATETIME, 
         Display_Code         CHAR(25), 
         SP_ID                CHAR(8), 
         practice_display_seq SMALLINT
        );

        -- =============================================
        -- Initialization
        -- =============================================
        -----------------------------------    
        -- initial Transaction Pool
        -----------------------------------       

        OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key;

        SELECT CAST(VT.SP_ID AS VARCHAR) + ' (' + CAST(VT.practice_display_seq AS VARCHAR) + ')' AS [SP_ID_Practice], 
               CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.[Encrypt_Field2])) AS [SP_Name], 
               CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.[Encrypt_Field3])) AS [SP_Name_Chi], 
               P.Practice_Name, 
               P.Practice_Name_Chi, 
               RAT.Transaction_ID, 
               TD.Unit, 
               TD.Total_Amount_RMB, 
               TD.Per_Unit_Value, 
               TD.ExchangeRate_Value, 
               VT.Transaction_Dtm, 
               SC.Display_Code, 
               TD.Subsidize_Code, 
               VT.SP_ID, 
               RSP.Seq_No, 
               RSP.Verification_Case, 
               VT.practice_display_seq, 
               TD.Scheme_Seq, 
               ISNULL(CAST(TAFS.AdditionalFieldValueCode AS MONEY), 0) AS 'Total_SupportFee',
			   SSM.SubSpecialities_Code,
			   SSM.Name_CN AS [SubSpecialities_Name_CN]
        INTO #Results
        FROM ReimbursementAuthTran AS RAT
             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
             ON RAT.Transaction_ID = VT.Transaction_ID
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON VT.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P WITH(NOLOCK)
             ON VT.SP_ID = P.SP_ID
                AND VT.Practice_Display_Seq = P.display_seq
             INNER JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON RAT.Transaction_ID = TD.Transaction_ID
             INNER JOIN SchemeClaim AS SC WITH(NOLOCK)
             ON RAT.Scheme_Code = SC.Scheme_Code
             LEFT JOIN ReimbursementServiceProvider AS RSP WITH(NOLOCK)
             ON VT.SP_ID = RSP.SP_ID
                AND RAT.Reimburse_ID = RSP.Reimburse_ID
                AND RAT.Scheme_Code = RSP.Scheme_Code
             LEFT OUTER JOIN TransactionAdditionalField AS TAFS WITH(NOLOCK)
             ON vt.Transaction_ID = TAFS.Transaction_ID
                AND TAFS.AdditionalFieldID = 'TotalSupportFee'
             LEFT OUTER JOIN TransactionAdditionalField AS TAF2 WITH(NOLOCK)
             ON vt.Transaction_ID = TAF2.Transaction_ID
                AND TAF2.AdditionalFieldID = 'SubSpecialities'
             LEFT OUTER JOIN HAServiceSubSpecialitiesMapping AS SSM WITH(NOLOCK)
             ON TAF2.AdditionalFieldValueCode = SSM.SubSpecialities_Code
				
        WHERE RAT.Reimburse_ID = @reimburse_id
              AND RAT.Scheme_Code = @scheme_code
        ORDER BY VT.SP_ID ASC, 
                 VT.practice_display_seq ASC, 
                 RAT.Transaction_ID ASC, 
                 VT.transaction_dtm ASC;
        CLOSE SYMMETRIC KEY sym_Key;

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
              (N'报告生成时间: ' + @strGenDtm, 
               ''
               );

        -----------------------------------    
        -- Result Table 2: Summary   
        -----------------------------------  

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              (N'报告生成日期', 
               @strGenDtm
               );

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              (N'付款截数日期', 
               @cutoff_Date_str
               );

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              (N'偿还编号', 
               RTRIM(LTRIM(@reimburse_id))
               );
        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
               );
        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
               );

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT N'服务提供者号码总数（执业处所数目）', 
               COUNT(DISTINCT SP_ID_Practice)
        FROM #Results;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT N'总交易数目', 
               COUNT(DISTINCT Transaction_ID)
        FROM #Results;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT N'从特别支援计划户口扣除的总金额 (¥)', 
               ISNULL(SUM(Total_Amount_RMB), 0)
        FROM #Results;
        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT N'由特别支援计划支付的減免总费用 (¥)', 
               ISNULL(SUM(Total_SupportFee), 0) - ISNULL(SUM(Total_Amount_RMB), 0)
        FROM #Results;
        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT N'由特别支援计划承担的总金额 (¥)', 
               ISNULL(SUM(Total_SupportFee), 0)
        FROM #Results;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
               );

        SELECT @display_code = SC.Display_Code
        FROM SchemeClaim AS SC WITH(NOLOCK)
        WHERE SC.Scheme_Code = @scheme_code;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              (N'计划', 
               RTRIM(LTRIM(@display_code))
               );

        -----------------------------------    
        -- Result Table 3:  Sheet 01
        -----------------------------------
        INSERT INTO #temp01
               (SP_ID_Practice, 
                SP_Name, 
                SP_Name_Chi, 
                Practice_Name, 
                Practice_Name_Chi, 
                Total_Amount_RMB, 
                Scheme_Code, 
                practice_display_seq, 
                SP_ID, 
                Total_SupportFee, 
                total_reduction_fee, 
                Total_Transaction
               )
        SELECT SP_ID_Practice, 
               SP_Name, 
               SP_Name_Chi, 
               Practice_Name, 
               Practice_Name_Chi, 
               SUM(Total_Amount_RMB), 
               Display_Code, 
               practice_display_seq, 
               SP_ID, 
               SUM(Total_SupportFee), 
               ISNULL(SUM(Total_SupportFee), 0) - ISNULL(SUM(Total_Amount_RMB), 0), 
               COUNT(DISTINCT Transaction_ID)
        FROM #Results
        GROUP BY SP_ID_Practice, 
                 SP_Name, 
                 SP_Name_Chi, 
                 Practice_Name, 
                 Practice_Name_Chi, 
                 Display_Code, 
                 practice_display_seq, 
                 SP_ID;

        -----------------------------------    
        -- Result Table 4:  Sheet 02 
        -----------------------------------     

        INSERT INTO #temp02
               (SP_ID_Practice, 
                SP_Name, 
                SP_Name_Chi, 
                Practice_Name, 
                Practice_Name_Chi, 
				SubSpecialities_Code,
				SubSpecialities_Name_CN,
                Transaction_ID, 
                Total_Amount_RMB, 
                Total_SupportFee, 
                total_reduction_fee, 
                Transaction_Dtm, 
                Display_Code, 
                SP_ID, 
                practice_display_seq
               )
        SELECT SP_ID_Practice, 
               SP_Name, 
               SP_Name_Chi, 
               Practice_Name, 
               Practice_Name_Chi,
			   SubSpecialities_Code,
			   SubSpecialities_Name_CN,
               Transaction_ID, 
               Total_Amount_RMB, 
               Total_SupportFee, 
               ISNULL(Total_SupportFee, 0) - ISNULL(Total_Amount_RMB, 0), 
               Transaction_Dtm, 
               Display_Code, 
               SP_ID, 
               practice_display_seq
        FROM #Results AS IT;

        -----------------------------------    
        -- Result Table 5: Legend    
        -----------------------------------      

        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        VALUES
              (N'计划简写:', 
               ''
               );
        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        SELECT RTRIM(display_Code), 
               Scheme_Desc_CN
        FROM SchemeClaim
        WHERE Scheme_Code = @scheme_code;

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
        -- Result Table 2:  Summary
        -----------------------------------  
        SELECT Parameter AS [Summary], 
               Value AS [ ]
        FROM @tempSummary;
        -----------------------------------    
        -- Result Table 3:  sheet 01
        -----------------------------------    
        INSERT INTO @WS01
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09
               )
        SELECT ROW_NUMBER() OVER(
               ORDER BY SP_ID, 
                        Practice_Display_Seq), 
               SP_ID_Practice,
               CASE ISNULL(Practice_Name_Chi, '')
                   WHEN ''
                   THEN Practice_name
                   ELSE Practice_Name_Chi
               END AS [Practice Name],
               CASE ISNULL(SP_Name_Chi, '')
                   WHEN ''
                   THEN SP_Name
                   ELSE SP_Name_Chi
               END AS [Service Provider Name], 
               Total_Transaction, 
               Total_Amount_RMB, 
               total_reduction_fee, 
               Total_SupportFee, 
               RTRIM(Scheme_Code) AS Scheme_Code
        FROM #temp01
        ORDER BY SP_ID ASC, 
                 practice_display_seq ASC;

        SELECT @intRowCount = COUNT(*)
        FROM @WS01;

        WHILE @intRowCount < @intSummaryFirstRow
            BEGIN
                SET @intRowCount = @intRowCount + 1;
                INSERT INTO @WS01(Col01)
                VALUES('');
            END;

        INSERT INTO @WS01
               (Col03, 
                Col04
               )
        SELECT N'服务提供者号码总数（执业处所数目）', 
               COUNT(DISTINCT SP_ID_Practice)
        FROM #Results;

        INSERT INTO @WS01
               (Col03, 
                Col04
               )
        SELECT N'总交易数目', 
               COUNT(DISTINCT Transaction_ID)
        FROM #Results;

        INSERT INTO @WS01
               (Col03, 
                Col04
               )
        SELECT N'从特别支援计划户口扣除的总金额 (¥)', 
               ISNULL(SUM(Total_Amount_RMB), 0)
        FROM #Results;

        INSERT INTO @WS01
               (Col03, 
                Col04
               )
        SELECT N'由特别支援计划支付的減免总费用 (¥)', 
               ISNULL(SUM(Total_SupportFee), 0) - ISNULL(SUM(Total_Amount_RMB), 0)
        FROM #Results;

        INSERT INTO @WS01
               (Col03, 
                Col04
               )
        SELECT N'由特别支援计划承担的总金额 (¥)', 
               ISNULL(SUM(Total_SupportFee), 0)
        FROM #Results;

        SELECT Col01, 
               Col02, 
               Col03, 
               Col04, 
               Col05, 
               Col06, 
               Col07, 
               Col08, 
               Col09
        FROM @WS01
        ORDER BY Seq;

        -----------------------------------    
        -- Result Table 4:  sheet 02
        -----------------------------------  

        SELECT ROW_NUMBER() OVER(
               ORDER BY SP_ID, 
                        Practice_Display_Seq, 
                        Transaction_dtm) AS rownum, 
               SP_ID_Practice AS [Service Provider ID (Practice Number)], 
               Practice_Name_Chi AS [Practice Name], 
			   SubSpecialities_Name_CN AS [SubSpecialities Name],
               SP_Name_Chi AS [Service Provider Name], 
               transaction_id AS [Transaction ID], 
               Total_Amount_RMB, 
               total_reduction_fee, 
               Total_SupportFee, 
               transaction_dtm AS [Transaction Date], 
               RTRIM(Display_Code) AS [Scheme]
        FROM #temp02
        ORDER BY SP_ID ASC, 
                 Practice_Display_Seq ASC, 
                 Transaction_dtm ASC;

        -----------------------------------    
        -- Result Table 5:  sheet 03 (hard coding sheet)
        -----------------------------------  

        SELECT '';

        -----------------------------------    
        -- Result Table 6: Legend    
        -----------------------------------      
        SELECT Parameter AS [Legend], 
               Value AS [ ]
        FROM @tempLegend;
        -- =============================================        
        -- House Keeping        
        -- =============================================     
        DROP TABLE #Results;
        DROP TABLE #temp01;
        DROP TABLE #temp02;
    END;

GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementFiles_get_byReimbID_SSSCMC] TO HCVU;
GO