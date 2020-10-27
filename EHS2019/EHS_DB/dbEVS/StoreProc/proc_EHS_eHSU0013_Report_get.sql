IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0013_Report_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    DROP PROCEDURE [dbo].[proc_EHS_eHSU0013_Report_get];
GO

/****** Object:  StoredProcedure [dbo].[proc_EHS_eHSU0013_Report_get]    Script Date: 18/08/2020 16:40:48 ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:			Nichole Ip
-- Create date:		18 August 2020
-- CR No.:			CRE20-009
-- Description:		Generate Report for CRE20-009
-- =============================================
--[dbo].[proc_EHS_eHSU0013_Report_get] '','T','2020-01-01 00:00:00','2020-06-30 00:00:00'

Create PROCEDURE [dbo].[proc_EHS_eHSU0013_Report_get] @request_time DATETIME = NULL, 
                                                     @period_type  CHAR(1), 
                                                     @From_Date    DATETIME = NULL, 
                                                     @To_Date      DATETIME = NULL
AS
    BEGIN
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @current_dtm DATETIME;
        DECLARE @DisplayVisitPeriod VARCHAR(60);
        DECLARE @seq INT;
        DECLARE @In_Period_From DATETIME= @From_Date;
        DECLARE @In_Period_To DATETIME= @To_Date;
        DECLARE @DateTypeFN CHAR(30);
		DECLARE @Current_Scheme_desc VARCHAR(20)

        -- Create Worksheet 02 Criteria Table  
        CREATE TABLE #WS01 
        (Seq   INT, 
         Col01 VARCHAR(100) DEFAULT '', 
         Col02 VARCHAR(100) DEFAULT ''
        );

        -- Create Worksheet 03 Result Table  
       CREATE TABLE  #WS02 
        (Seq   INT IDENTITY(1, 1), 
         Col01 VARCHAR(100) DEFAULT '', 
         Col02 VARCHAR(100) DEFAULT '', 
         Col03 VARCHAR(100) DEFAULT '', 
         Col04 VARCHAR(100) DEFAULT '', 
         Col05 VARCHAR(200) DEFAULT '', 
         Col06 VARCHAR(100) DEFAULT '', 
         Col07 VARCHAR(100) DEFAULT '', 
         Col08 VARCHAR(100) DEFAULT '', 
         Col09 VARCHAR(100) DEFAULT '', 
         Col10 VARCHAR(100) DEFAULT '', 
         Col11 VARCHAR(100) DEFAULT '', 
         Col12 VARCHAR(100) DEFAULT '', 
         Col13 VARCHAR(100) DEFAULT ''
        );

        -- Create Worksheet 04 Remark  
        CREATE TABLE  #WS03
        (Seq   INT, 
         Col01 NVARCHAR(MAX) DEFAULT '', 
         Col02 NVARCHAR(MAX) DEFAULT ''
        );

        -- =============================================  
        -- Initialization  
        -- =============================================  
        -- Sheet 2 Criteria
        --Set Date type full name
        IF @period_type = 'T'
            BEGIN
                SET @DateTypeFN = 'Transaction Date';
            END;
        IF @period_type = 'S'
            BEGIN
                SET @DateTypeFN = 'Service Date';
            END;
        SET @DisplayVisitPeriod = CONVERT(CHAR(10), @In_Period_From, 111) + ' to ' + CONVERT(CHAR(10), @In_Period_To, 111);
        SET @current_dtm = GETDATE();

        --Sheet 3 result table
        SELECT dbo.func_format_system_number(vt.Transaction_ID) AS transaction_id, 
               CONVERT(CHAR(20), vt.Transaction_Dtm, 120) AS Transaction_DTm, 
               vt.SP_ID, 
               CONVERT(CHAR(10), vt.Service_Receive_Dtm, 111) AS Service_Receive_Dtm, 
               sd.Data_Value  AS Type_Of_Documentary_Proof,

        --   display 'QIV-DA 2020/21 '  
               rtrim(sz.Display_Code) + ' ' + vs.Season_Desc AS subsidy, 
               'Only Dose' AS Available_Item_Code,
               CASE
                   WHEN ISNULL(vt.voucher_acc_id, '') = ''
                   THEN CONVERT(CHAR(10), tpi.dob, 111)
                   ELSE CONVERT(CHAR(10), pi.DOB, 111)
               END AS dob,
               CASE
                   WHEN ISNULL(vt.voucher_acc_id, '') = ''
                   THEN tpi.exact_dob
                   ELSE pi.Exact_DOB
               END AS DOB_Flag,
               CASE
                   WHEN ISNULL(vt.voucher_acc_id, '') = ''
                   THEN tpi.sex
                   ELSE pi.sex
               END AS gender,
               CASE
                   WHEN ISNULL(vt.voucher_acc_id, '') = ''
                   THEN tpi.Doc_Code
                   ELSE pi.Doc_Code
               END AS doc_type, 
               vt.HKIC_Symbol AS hkic_Symbol, 
               ec.Status_Description
        INTO #Results
        FROM VoucherTransaction vt WITH(NOLOCK)
             INNER JOIN TransactionAdditionalField taf WITH(NOLOCK) ON vt.Transaction_id = taf.Transaction_ID
             INNER JOIN TransactionDetail td WITH(NOLOCK) ON td.Transaction_ID = vt.Transaction_ID
             INNER JOIN
        (
            SELECT *
            FROM StatusData WITH(NOLOCK)
            WHERE Enum_Class = 'ClaimTransStatus'
        ) ec ON ec.Status_Value = vt.Record_Status
             INNER JOIN Subsidize sz WITH(NOLOCK) ON sz.Subsidize_Code = td.Subsidize_Code
			 INNER JOIN ClaimCategory cc with(NOLOCK) on cc.Category_Code = vt.Category_Code
             INNER JOIN VaccineSeason vs WITH(NOLOCK) ON vs.scheme_seq = td.scheme_seq
                                                         AND vs.subsidize_item_Code = td.Subsidize_Item_Code
                                                         AND vs.scheme_code = td.scheme_code
			 INNER JOIN StaticData sd WITH(NOLOCK) on sd.Item_No = taf.AdditionalFieldValueCode
			 and sd.Column_Name ='VSSDA_DOCUMENTARYPROOF'
             LEFT JOIN TempPersonalInformation tpi WITH(NOLOCK) ON vt.Temp_Voucher_Acc_ID = tpi.Voucher_Acc_ID
             LEFT JOIN PersonalInformation pi WITH(NOLOCK) ON vt.Voucher_Acc_ID = pi.Voucher_Acc_ID
        WHERE vt.scheme_code = 'VSS'
              ---  AND vt.Transaction_ID = 'TG20818000000223'
              AND taf.AdditionalFieldValueCode in ( 'CSSA_CERT','ANNEX_PAGE')

              ------- vt.Transaction_Dtm between @In_Period_From AND @In_Period_To  ;
              AND ((@period_type = 'S'
                    ---- AND vt.Service_Receive_Dtm BETWEEN @In_Period_From AND @In_Period_To)
                    AND vt.Service_Receive_Dtm >= @In_Period_From
                    AND vt.Service_Receive_Dtm < @In_Period_To + 1)
                   OR (@period_type = 'T'
                       ----    AND vt.Transaction_Dtm BETWEEN @In_Period_From AND @In_Period_To))
                       AND vt.Transaction_Dtm >= @In_Period_From
                       AND vt.Transaction_Dtm < @In_Period_To + 1))
					   AND vt.Record_Status NOT IN ('I', 'D')

        ORDER BY vt.transaction_id;

		----------------------------
        ---Sheet 4 Remark
		---------------------------
		--- Identity Document Type
		SET @Seq = 0;
		INSERT INTO #WS03
		SELECT @Seq, '(A) Legend', NULL

		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '1. Identity Document Type', NULL

		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
        SELECT @Seq as seq,Doc_Code, 
              Doc_Name
        FROM DOCTYPE
        ORDER BY Display_Seq;

		---Subsidy
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03 SELECT @Seq, NULL, NULL

		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '2. Subsidy', NULL

		SET @Seq = @Seq + 1;
		INSERT INTO 
			#WS03
		SELECT DISTINCT 
			@Seq, Display_Code_For_Claim, Legend_Desc_For_Claim
		FROM 
			SubsidizeGroupClaim a
				INNER JOIN Subsidize b 
					ON a.Subsidize_Code = b.Subsidize_Code
				INNER JOIN Subsidizeitem c 
					ON b.subsidize_item_Code = c.Subsidize_Item_Code AND c.Subsidize_Type = 'VACCINE'
		WHERE a.scheme_code='VSS' and a.Subsidize_Code in ('VDAQIV','VDATIV')
		ORDER BY 
			Display_Code_For_Claim

		---DOB Flag
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03 SELECT @Seq, NULL, NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '3. DOB Flag', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'D', 'Exact date DD/MM/YYYY'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'M', 'MM/YYYY'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'Y', 'Only year YYYY'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'A', 'Exemption Certificate: Date of registration + age'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'R', 'Exemption Certificate: Reported year of birth'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'T', 'Exemption Certificate: Exact date DD/MM/YYYY on travel document' + '   ' + 'HKBC: Exact date DD/MM/YYYY for DOB in word'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'U', 'Exemption Certificate: MM/YYYY on travel document' + '   ' + 'HKBC: MM/YYYY for DOB in word'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'V', 'Exemption Certificate: Only year YYYY on travel document' + '   ' + 'HKBC: Only year YYYY for DOB in word'
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, NULL, NULL   

	 
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '(B) Common Note(s) for the report', NULL

		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '1. Transactions:', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'a. VSS claim transactions under category "Persons receiving Disability Allowance / standard rate of "100% disabled" or "requiring constant attendance" under CSSA"', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'b. Used one of below documentary proof', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '   - Certificate of CSSA Recipients (for Medical Waivers)', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, '   - The new Annex page of ¡§Notification of Successful Application¡¨/ ¡§Notification of Revision of Assistance¡¨ ', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'c. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'd. Exclude those reimbursed transactions with invalidation status marked as Invalidated.', NULL
		SET @Seq = @Seq + 1;
		INSERT INTO #WS03
		SELECT @Seq, 'e. Exclude voided/deleted transactions.', NULL
        ------------------------------------------------  
        -- For Excel Sheet (02): Criteria 
        -----------------------------------------------  

        SET @seq = 0;
        INSERT INTO #WS01
        (Seq, 
         Col01, 
         Col02
        )
        VALUES
        (@seq, 
         'Type of Date', 
         @DateTypeFN
        );
        SET @seq = @seq + 1;
        INSERT INTO #WS01
        (Seq, 
         Col01, 
         Col02
        )
        VALUES
        (@seq, 
         'Date', 
         @DisplayVisitPeriod
        );

        ------------------------------------------------  
        -- For Excel Sheet (03): 01-Claims 
        -----------------------------------------------  
        INSERT INTO #WS02(Col01)
        VALUES(RTRIM(@DateTypeFN) + ' Period: ' + @DisplayVisitPeriod);

        ------------------------------------
        --- Transactions:
        ------------------------------------

        INSERT INTO #WS02(Col01)
               SELECT 'Total No. of Transactions: ' + LTRIM(CONVERT(CHAR(2), COUNT(DISTINCT Transaction_ID)))
               FROM #Results;
        INSERT INTO #WS02(Col01)
        VALUES('');
        INSERT INTO #WS02(Col01)
        VALUES('');
        INSERT INTO #WS02
        (Col01, 
         Col02, 
         Col03, 
         Col04, 
         Col05, 
         Col06, 
         Col07, 
         Col08, 
         Col09, 
         Col10, 
         Col11, 
         Col12, 
         Col13
        )
        VALUES
        ('Transaction ID', 
         'Transaction Time', 
         'SPID', 
         'Service Date', 
         'Type Of Documentary Proof', 
         'Subsidy', 
         'Dose', 
         'DOB', 
         'DOB Flag', 
         'Gender', 
         'Doc Type', 
         'HKIC Symbol', 
         'Transaction Status'
        );
        INSERT INTO #WS02
        (Col01, 
         Col02, 
         Col03, 
         Col04, 
         Col05, 
         Col06, 
         Col07, 
         Col08, 
         Col09, 
         Col10, 
         Col11, 
         Col12, 
         Col13
        )
               SELECT *
               FROM #Results;

		-------------------
		---Sheet 4 Remark
		-------------------
		

		-- SET @seq = @seq + 1;
		-- INSERT INTO #WS03
  --      (Seq, 
  --       Col01, 
  --       Col02
  --      )
		--select * from #Results01

		
        -- =============================================  
        -- Return results  
        -- =============================================   
        ---Sheet1
        SELECT 'Report Generation Time: ' + CONVERT(VARCHAR(10), @current_dtm, 111) + ' ' + CONVERT(VARCHAR(5), @current_dtm, 114); --The first Sheet
        --Sheet2
        SELECT v.Col01, 
               v.Col02
        FROM #WS01 AS v
        ORDER BY seq;

        --Sheet3
        SELECT w.Col01, 
               w.Col02, 
               w.Col03, 
               w.Col04, 
               w.Col05, 
               w.Col06, 
               w.Col07, 
               w.Col08, 
               w.Col09, 
               w.Col10, 
               w.Col11, 
               w.Col12, 
               w.Col13
        FROM #WS02 AS w
        ORDER BY w.Seq;

		 --Sheet4
        SELECT  w3.Col01, 
               w3.Col02 
        FROM #WS03 AS w3
		 ORDER BY w3.Seq;

        IF OBJECT_ID('tempdb..#Results') IS NOT NULL
            DROP TABLE #Results;
		 IF OBJECT_ID('tempdb..#Results01') IS NOT NULL
            DROP TABLE #Results01;
	 
    END;
GO
GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0013_Report_get] TO HCVU;
GO