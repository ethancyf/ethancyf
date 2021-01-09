
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0008_Report_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_EHS_eHSW0008_Report_get];
    END;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	30 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:     Koala CHENG  
-- Modified date:   21 Dec 2020
-- CR No.:			INT20-0064 Fix eHSW0008 HAD filtering)
-- Description:     1. [01-Summary] Fix HAD and DHC filtering with SP + practice no.
--					2. Add WITH(NOLOCK)
-- =============================================  
-- =============================================
-- Author:			Raiman Chong
-- Create date: 	05 Nov 2020
-- CR. No			CRE20-014-02 (Gov SIV 2020_21)
-- Description:		Report_Using_Government_Supplied_Vaccine
-- =============================================
-- exec [proc_EHS_eHSW0008_Report_get]

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0008_Report_get] @request_time DATETIME = NULL, -- The reference date to get @CutOff_Dtm. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)  
                                                      @Cutoff_Dtm   DATETIME = NULL -- Inclusive, The Cut Off Date. If defined, it will override the value from the @request_dtm  
AS
    BEGIN  
        -- =============================================  
        -- Report setting  
        -- =============================================  

        IF @request_time IS NULL
            BEGIN
                SET @request_time = GETDATE()
            END;

        IF @Cutoff_Dtm IS NULL
            BEGIN
                SET @Cutoff_Dtm = CONVERT(VARCHAR(11), @request_time, 106);
            END;

        DECLARE @ReportDtm DATETIME;
        SET @ReportDtm = DATEADD(day, -1, @Cutoff_Dtm); -- The date report data as at  
        -- =============================================  
        -- Declaration  
        -- =============================================  
        -- =============================================  
        -- Declaration For #WS02 Summary
        -- =============================================  

        DECLARE @Scheme_Code AS VARCHAR(10);
        DECLARE @current_scheme_Seq AS INT;
        DECLARE @Current_Scheme_desc VARCHAR(20);
        DECLARE @Category_Code VARCHAR(10);
        DECLARE @Category_Seq INT;
        DECLARE @QIV_Subsidize_Code VARCHAR(10);

        DECLARE @LAIV_Subsidize_Code VARCHAR(10);
        DECLARE @Current_Season_Start_Dtm DATETIME;
        DECLARE @result1 INT, @result2 INT, @result3 INT, @result4 INT, @result5 INT, @result6 INT, @result7 INT, @result8 INT, @result9 INT,
        @result10 INT, @result11 INT, @result12 INT, @result13 INT, @result14 INT, @result15 INT, @result16 INT, @result17 INT, @result18 INT,
        @result19 INT, @result20 INT, @result21 INT, @result22 INT, @result23 INT, @result24 INT, @result25 INT, @result26 INT, @result27 INT,
        @result28 INT, @result29 INT, @result30 INT, @result31 INT;
        DECLARE @Report_ID AS VARCHAR(30);

        DECLARE @SIV_ByCategory AS TABLE -- SIV for current season  
        (Display_Seq            INT, 
         Subsidize_Code         VARCHAR(10), 
         Display_Code_For_Claim VARCHAR(25), 
         Category_Code          VARCHAR(10), 
         Category_Seq           INT
        );

        --move back later
        IF OBJECT_ID('tempdb..#WS02') IS NOT NULL
            BEGIN
                DROP TABLE #WS02;
            END;
        IF OBJECT_ID('tempdb..#WS03') IS NOT NULL
            BEGIN
                DROP TABLE #WS03;
            END;
        IF OBJECT_ID('tempdb..#WS04') IS NOT NULL
            BEGIN
                DROP TABLE #WS04;
            END;
        IF OBJECT_ID('tempdb..#WS05') IS NOT NULL
            BEGIN
                DROP TABLE #WS05;
            END;

        CREATE TABLE #WS02
        (_display_seq    TINYINT, 
         _result_value1  VARCHAR(200) DEFAULT '', 
         _result_value2  VARCHAR(100) DEFAULT '', 
         _result_value3  VARCHAR(100) DEFAULT '', 
         _result_value4  VARCHAR(100) DEFAULT '', 
         _result_value5  VARCHAR(100) DEFAULT '', 
         _result_value6  VARCHAR(100) DEFAULT '', 
         _result_value7  VARCHAR(100) DEFAULT '', 
         _result_value8  VARCHAR(100) DEFAULT '', 
         _result_value9  VARCHAR(100) DEFAULT '', 
         _result_value10 VARCHAR(100) DEFAULT '', 
         _result_value11 VARCHAR(100) DEFAULT '', 
         _result_value12 VARCHAR(100) DEFAULT '', 
         _result_value13 VARCHAR(100) DEFAULT '', 
         _result_value14 VARCHAR(100) DEFAULT '', 
         _result_value15 VARCHAR(100) DEFAULT '', 
         _result_value16 VARCHAR(100) DEFAULT '', 
         _result_value17 VARCHAR(100) DEFAULT '', 
         _result_value18 VARCHAR(100) DEFAULT '', 
         _result_value19 VARCHAR(100) DEFAULT '', 
         _result_value20 VARCHAR(100) DEFAULT '', 
         _result_value21 VARCHAR(100) DEFAULT '', 
         _result_value22 VARCHAR(100) DEFAULT '', 
         _result_value23 VARCHAR(100) DEFAULT '', 
         _result_value24 VARCHAR(100) DEFAULT '', 
         _result_value25 VARCHAR(100) DEFAULT '', 
         _result_value26 VARCHAR(100) DEFAULT '', 
         _result_value27 VARCHAR(100) DEFAULT '', 
         _result_value28 VARCHAR(100) DEFAULT '', 
         _result_value29 VARCHAR(100) DEFAULT '', 
         _result_value30 VARCHAR(100) DEFAULT '', 
         _result_value31 VARCHAR(100) DEFAULT '', 
         _result_value32 VARCHAR(100) DEFAULT ''
        );

        CREATE TABLE #temp_VSS  -- Transaction with SIV and PV  
        (voucher_acc_id      CHAR(15), 
         temp_voucher_acc_id CHAR(15), 
         Transaction_ID      VARCHAR(20), 
         Service_Receive_Dtm DATETIME, 
         DOB                 DATETIME, 
         DOB_Adjust          DATETIME, 
         Exact_DOB           CHAR(1), 
         Dose                CHAR(20), 
         SP_ID               CHAR(8), 
		 Practice_Display_Seq SMALLINT,
         Subsidize_Code      VARCHAR(10), 
         Subsidize_Item_Code CHAR(10), 
         identity_num        VARCHAR(20), 
         IsWithVoucher       TINYINT, 
         IsSIV               TINYINT, 
         IsPV                TINYINT, 
         IsPV13              TINYINT, 
         IsMMR               TINYINT, 
         IsCurrentSeason     TINYINT
        );
        CREATE TABLE #temp_HCVS
        (voucher_acc_id      CHAR(15), 
         temp_voucher_acc_id CHAR(15), 
         transaction_id      VARCHAR(20), 
         Reason1             CHAR(1), 
         Reason2             CHAR(1), 
         identity_num        VARCHAR(20), 
         dob                 DATETIME, 
         service_receive_dtm DATETIME, 
         SP_ID               CHAR(8)
        );

        CREATE TABLE #account
        (voucher_acc_id      CHAR(15), 
         temp_voucher_acc_id CHAR(15), 
         identity_num        VARCHAR(20), 
         doc_code            CHAR(10), 
         dob                 DATETIME
        );

        DECLARE @TempTransactionStatusFilter AS TABLE
        (Status_Name  CHAR(50), 
         Status_Value CHAR(10)
        );
        INSERT INTO @TempTransactionStatusFilter
               (Status_Name, 
                Status_Value
               )
        SELECT Status_Name, 
               Status_Value
        FROM StatStatusFilterMapping WITH(NOLOCK)
        WHERE(Report_id = 'ALL'
              OR Report_id = @Report_ID)
             AND Table_Name = 'VoucherTransaction'
             AND (Effective_Date IS NULL
                  OR Effective_Date <= @Cutoff_Dtm)
             AND (Expiry_Date IS NULL
                  OR Expiry_Date >= @Cutoff_Dtm);

        -- =============================================  
        -- Declaration For #WS02 Summary end
        -- =============================================  
        -- =============================================  
        -- Declaration For #WS03 Raw Data
        -- =============================================  
        DECLARE @system_Dtm AS DATETIME;
        DECLARE @Report_Dtm DATETIME;
        DECLARE @Date_Range TINYINT;

        DECLARE @Str_NA VARCHAR(10);

        DECLARE @TRANSACTION TABLE
        (sp_id                CHAR(8), 
         transaction_id       CHAR(20), 
         transaction_dtm      DATETIME, 
         service_receive_dtm  DATETIME, 
         subsidize_item_code  VARCHAR(10), 
         dose                 CHAR(20), 
         scheme_seq           INT, 
         per_unit_value       INT, 
         voucher_acc_id       CHAR(15), 
         temp_voucher_acc_id  CHAR(15), 
         special_acc_id       CHAR(15), 
         invalid_acc_id       CHAR(15), 
         doc_code             CHAR(20), 
         transaction_status   CHAR(1), 
         reimbursement_status CHAR(1), 
         create_by_smartid    CHAR(1), 
         row                  INT, 
         vaccine              VARCHAR(100), 
         category_code        VARCHAR(10), 
         hkic_symbol          CHAR(1)
        );

        DECLARE @ACCOUNT TABLE
        (sp_id                CHAR(8), 
         transaction_id       CHAR(20), 
         transaction_dtm      DATETIME, 
         service_receive_dtm  DATETIME, 
         dose                 CHAR(20), 
         dob                  DATETIME, 
         exact_dob            CHAR(1), 
         sex                  CHAR(1), 
         doc_code             CHAR(20), 
         transaction_status   CHAR(1), 
         reimbursement_status CHAR(1), 
         row                  INT, 
         vaccine              VARCHAR(100), 
         category_code        VARCHAR(10)
        );

        CREATE TABLE #WS03
        (result_seq     INT, 
         result_value1  VARCHAR(200), 
         result_value2  VARCHAR(200), 
         result_value3  VARCHAR(200), 
         result_value4  VARCHAR(200), 
         result_value5  VARCHAR(200), 
         result_value6  VARCHAR(200), 
         result_value7  VARCHAR(200), 
         result_value8  VARCHAR(200), 
         result_value9  VARCHAR(200), 
         result_value10 VARCHAR(200), 
         result_value11 VARCHAR(200), 
         result_value12 VARCHAR(200), 
         result_value13 VARCHAR(200), 
         result_value14 VARCHAR(200), 
         result_value15 VARCHAR(200), 
         result_value16 VARCHAR(200)
        );

        -- =============================================  
        -- Declaration For #WS03 Raw Data end
        -- =============================================  
        -- =============================================  
        -- Declaration For #WS04 HAD 
        -- =============================================  

        CREATE TABLE #WS04
        (result_seq    INT IDENTITY(1, 1), 
         result_value1 VARCHAR(200), 
         result_value2 VARCHAR(200),
        );

        -- =============================================  
        -- Declaration For #WS04 HAD end
        -- =============================================  
        -- =============================================  
        -- Declaration For #WS05 Remark
        -- =============================================  

        CREATE TABLE #Remark
        (result_value1 VARCHAR(200), 
         result_value2 VARCHAR(200),
        );

        -- =============================================  
        -- Declaration For #WS05 Remark end
        -- =============================================  
        -- =============================================  
        -- Validation   
        -- =============================================  
        -- =============================================  
        -- Validation for #WS02
        -- =============================================  
        SET @Scheme_Code = 'VSS';
        SET @current_scheme_Seq = 5;
        SET @Report_ID = 'eHSW0008';

        -- Get all SIV of VSS with current season  
        INSERT INTO @SIV_ByCategory
               (Display_Seq, 
                Subsidize_Code, 
                Display_Code_For_Claim, 
                Category_Code, 
                Category_Seq
               )
        SELECT ROW_NUMBER() OVER(
               ORDER BY S.Display_Seq), 
               S.Subsidize_Code, 
               SGC.Display_Code_For_Claim, 
               SCA.Category_Code, 
               CC.Display_Seq
        FROM [SubsidizeGroupClaim] AS SGC WITH(NOLOCK)
             INNER JOIN [Subsidize] AS S WITH(NOLOCK)
             ON SGC.[Subsidize_Code] = S.[Subsidize_Code]
             INNER JOIN [SubsidizeGroupCategory] AS SCA WITH(NOLOCK)
             ON SCA.[Subsidize_Code] = S.[Subsidize_Code]
             INNER JOIN [ClaimCategory] AS CC WITH(NOLOCK)
             ON CC.Category_Code = SCA.Category_Code
        WHERE SGC.[Scheme_Code] = @Scheme_Code
              AND (S.[Subsidize_Item_Code] = 'SIV'
                   AND SGC.[Scheme_Seq] = @current_scheme_Seq
                   AND SGC.Subsidize_Code LIKE '%G')
        ORDER BY CC.Display_Seq, 
                 SGC.Display_Seq;

        SELECT @Current_Season_Start_Dtm = MIN(SG.Claim_Period_From)
        FROM SubsidizeGroupClaim AS SG WITH(NOLOCK)
             INNER JOIN SubsidizeGroupClaimItemDetails AS SGD WITH(NOLOCK)
             ON SG.Scheme_Code = SGD.Scheme_Code
                AND SG.Scheme_Seq = SGD.Scheme_Seq
                AND SG.Subsidize_Code = SGD.Subsidize_Code
        WHERE SG.Scheme_Code = @Scheme_Code
              AND SG.Scheme_Seq = @current_scheme_Seq;

        INSERT INTO #account
               (voucher_acc_id, 
                temp_voucher_acc_id, 
                identity_num, 
                doc_code, 
                dob
               )
        (
            SELECT p.voucher_acc_id, 
                   NULL, 
                   CONVERT(VARCHAR, DECRYPTBYKEY(p.Encrypt_Field1)), 
                   p.doc_code, 
                   p.dob
            FROM voucheraccount AS va WITH(NOLOCK), 
                 personalinformation AS p WITH(NOLOCK)
            WHERE va.voucher_acc_id = p.voucher_acc_id
                  AND va.create_dtm < @Cutoff_Dtm
        );

        INSERT INTO #account
               (voucher_acc_id, 
                temp_voucher_acc_id, 
                identity_num, 
                doc_code, 
                dob
               )
        (
            SELECT NULL, 
                   p.voucher_acc_id, 
                   CONVERT(VARCHAR, DECRYPTBYKEY(p.Encrypt_Field1)), 
                   p.doc_code, 
                   p.dob
            FROM tempvoucheraccount AS va WITH(NOLOCK), 
                 temppersonalinformation AS p WITH(NOLOCK)
            WHERE va.voucher_acc_id = p.voucher_acc_id
                  AND va.create_dtm < @Cutoff_Dtm
        );

        INSERT INTO #temp_VSS
               (voucher_acc_id, 
                temp_voucher_acc_id, 
                Transaction_ID, 
                Service_Receive_Dtm, 
                DOB, 
                DOB_Adjust, 
                Exact_DOB, 
                Dose, 
                SP_ID, 
				Practice_Display_Seq,
                Subsidize_Code, 
                Subsidize_Item_Code, 
                IsSIV, 
                IsCurrentSeason
               )
        SELECT vt.Voucher_Acc_ID, 
               vt.Temp_Voucher_Acc_ID, 
               VT.Transaction_ID, 
               VT.Service_Receive_Dtm, 
               VR.DOB, 
               VR.DOB, 
               VR.Exact_DOB, 
               D.Available_Item_Code, 
               VT.SP_ID, 
			   VT.Practice_Display_Seq,
               D.Subsidize_Code, 
               D.Subsidize_Item_Code,
               CASE
                   WHEN Subsidize_Item_Code = 'SIV'
                   THEN 1
                   ELSE 0
               END,
               CASE
                   WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm
                   THEN 1
                   ELSE 0
               END
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN TransactionDetail AS D WITH(NOLOCK)
             ON VT.Transaction_ID = D.Transaction_ID
             INNER JOIN PersonalInformation AS VR WITH(NOLOCK)
             ON VT.Voucher_Acc_ID = VR.Voucher_Acc_ID
                AND VT.Doc_Code = VR.Doc_Code
                AND VT.Voucher_Acc_ID IS NOT NULL
                AND VT.Voucher_Acc_ID <> ''
             INNER JOIN VoucherAccount AS A WITH(NOLOCK)
             ON VT.Voucher_Acc_ID = A.Voucher_Acc_ID
        WHERE VT.Scheme_Code = @Scheme_Code
              AND VT.Transaction_Dtm <= @Cutoff_Dtm
              AND (D.scheme_seq = @current_scheme_Seq
                   AND D.Subsidize_Code IN
        (
            SELECT Subsidize_Code
            FROM @SIV_ByCategory
        ))
              AND VT.record_status NOT IN
        (
            SELECT Status_Value
            FROM @TempTransactionStatusFilter
            WHERE Status_Name = 'Record_Status'
        )
              AND (vt.Invalidation IS NULL
                   OR vt.Invalidation NOT IN
        (
            SELECT Status_Value
            FROM @TempTransactionStatusFilter
            WHERE Status_Name = 'Invalidation'
        ));

        -- ---------------------------------------------  
        -- Temp  
        -- ---------------------------------------------  
        INSERT INTO #temp_VSS
               (voucher_acc_id, 
                temp_voucher_acc_id, 
                Transaction_ID, 
                Service_Receive_Dtm, 
                DOB, 
                DOB_Adjust, 
                Exact_DOB, 
                Dose, 
                SP_ID, 
				Practice_Display_Seq,
                Subsidize_Code, 
                Subsidize_Item_Code, 
                IsSIV, 
                IsCurrentSeason
               )
        SELECT vt.Voucher_Acc_ID, 
               vt.Temp_Voucher_Acc_ID, 
               VT.Transaction_ID, 
               VT.Service_Receive_Dtm, 
               TVR.DOB, 
               TVR.DOB, 
               TVR.Exact_DOB, 
               D.Available_Item_Code, 
               VT.SP_ID, 
			   VT.Practice_Display_Seq,
               D.Subsidize_Code, 
               D.Subsidize_Item_Code,
               CASE
                   WHEN Subsidize_Item_Code = 'SIV'
                   THEN 1
                   ELSE 0
               END,
               CASE
                   WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm
                   THEN 1
                   ELSE 0
               END
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN TransactionDetail AS D WITH(NOLOCK)
             ON VT.Transaction_ID = D.Transaction_ID
             INNER JOIN TempPersonalInformation AS TVR WITH(NOLOCK)
             ON VT.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID
                AND (VT.Voucher_Acc_ID = ''
                     OR VT.Voucher_Acc_ID IS NULL)
                AND VT.Special_Acc_ID IS NULL
                AND VT.Invalid_Acc_ID IS NULL
                AND VT.Temp_Voucher_Acc_ID <> ''
                AND VT.Temp_Voucher_Acc_ID IS NOT NULL
                AND VT.Doc_Code = TVR.Doc_Code
             INNER JOIN TempVoucherAccount AS A WITH(NOLOCK)
             ON VT.Temp_Voucher_Acc_ID = A.Voucher_Acc_ID
        WHERE VT.Scheme_Code = @Scheme_Code
              AND VT.Transaction_Dtm <= @Cutoff_Dtm
              AND (D.scheme_seq = @current_scheme_Seq
                   AND D.Subsidize_Code IN
        (
            SELECT Subsidize_Code
            FROM @SIV_ByCategory
        ))
              AND VT.record_status NOT IN
        (
            SELECT Status_Value
            FROM @TempTransactionStatusFilter
            WHERE Status_Name = 'Record_Status'
        )
              AND (vt.Invalidation IS NULL
                   OR vt.Invalidation NOT IN
        (
            SELECT Status_Value
            FROM @TempTransactionStatusFilter
            WHERE Status_Name = 'Invalidation'
        ));

        -- ---------------------------------------------  
        -- Special  
        -- ---------------------------------------------  
        INSERT INTO #temp_VSS
               (Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Transaction_ID, 
                Service_Receive_Dtm, 
                DOB, 
                DOB_Adjust, 
                Exact_DOB, 
                Dose, 
                SP_ID, 
				Practice_Display_Seq,
                Subsidize_Code, 
                Subsidize_Item_Code, 
                IsSIV, 
                IsCurrentSeason
               )
        SELECT vt.Voucher_Acc_ID, 
               vt.Temp_Voucher_Acc_ID, 
               VT.Transaction_ID, 
               VT.Service_Receive_Dtm, 
               TVR.DOB, 
               TVR.DOB, 
               TVR.Exact_DOB, 
               D.Available_Item_Code, 
               VT.SP_ID, 
			   VT.Practice_Display_Seq,
               D.Subsidize_Code, 
               D.Subsidize_Item_Code,
               CASE
                   WHEN Subsidize_Item_Code = 'SIV'
                   THEN 1
                   ELSE 0
               END,
               CASE
                   WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm
                   THEN 1
                   ELSE 0
               END
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN TransactionDetail AS D WITH(NOLOCK)
             ON VT.Transaction_ID = D.Transaction_ID
             INNER JOIN SpecialPersonalInformation AS TVR WITH(NOLOCK)
             ON VT.Special_Acc_ID = TVR.Special_Acc_ID
                AND VT.Special_Acc_ID IS NOT NULL
                AND (VT.Voucher_Acc_ID IS NULL
                     OR VT.Voucher_Acc_ID = '')
                AND VT.Invalid_Acc_ID IS NULL
                AND VT.Doc_Code = TVR.Doc_Code
             INNER JOIN SpecialAccount AS A WITH(NOLOCK)
             ON VT.Special_Acc_ID = A.Special_Acc_ID
        WHERE VT.Scheme_Code = @Scheme_Code
              AND VT.Transaction_Dtm <= @Cutoff_Dtm
              AND (D.scheme_seq = @current_scheme_Seq
                   AND D.Subsidize_Code IN
        (
            SELECT Subsidize_Code
            FROM @SIV_ByCategory
        ))
              AND VT.record_status NOT IN
        (
            SELECT Status_Value
            FROM @TempTransactionStatusFilter
            WHERE Status_Name = 'Record_Status'
        )
              AND (vt.Invalidation IS NULL
                   OR vt.Invalidation NOT IN
        (
            SELECT Status_Value
            FROM @TempTransactionStatusFilter
            WHERE Status_Name = 'Invalidation'
        ));
        UPDATE #temp_HCVS
          SET identity_num = a.identity_num, 
              dob = a.dob
        FROM #account a, #temp_HCVS v
        WHERE ISNULL(v.voucher_acc_id, '') = ''
              AND ISNULL(a.voucher_acc_id, '') = ''
              AND v.temp_voucher_acc_id = a.temp_voucher_acc_id;
        UPDATE #temp_HCVS
          SET identity_num = a.identity_num, 
              dob = a.dob
        FROM #account a, #temp_HCVS v
        WHERE ISNULL(v.voucher_acc_id, '') <> ''
              AND ISNULL(a.voucher_acc_id, '') <> ''
              AND v.voucher_acc_id = a.voucher_acc_id;

        --  
        -- ---------------------------------------------  
        -- Patch data  
        -- ---------------------------------------------  
        UPDATE #temp_VSS
          SET identity_num = a.identity_num, 
              dob = a.dob
        FROM #account a, #temp_VSS vss
        WHERE ISNULL(vss.voucher_acc_id, '') = ''
              AND ISNULL(a.voucher_acc_id, '') = ''
              AND vss.temp_voucher_acc_id = a.temp_voucher_acc_id;
        UPDATE #temp_VSS
          SET identity_num = a.identity_num, 
              dob = a.dob
        FROM #account a, #temp_VSS vss
        WHERE ISNULL(vss.voucher_acc_id, '') <> ''
              AND ISNULL(a.voucher_acc_id, '') <> ''
              AND vss.voucher_acc_id = a.voucher_acc_id;
        UPDATE #temp_VSS
          SET IsWithVoucher = 1
        FROM #temp_VSS vss, #temp_HCVS hcvs
        WHERE vss.Service_Receive_Dtm = hcvs.service_receive_dtm
              AND vss.identity_num = hcvs.identity_num
              AND (hcvs.Reason1 = 'Y'
                   AND hcvs.Reason2 = 'Y')
              AND vss.SP_ID = hcvs.SP_ID;
        UPDATE #temp_VSS
          SET DOB = CONVERT(VARCHAR, YEAR(DOB)) + '-' + CONVERT(VARCHAR, MONTH(DOB)) + '-' + CONVERT(VARCHAR, DAY(DATEADD(d, -DAY(DATEADD(m, 1, DOB)),
          DATEADD(m, 1, DOB))))
        WHERE Exact_DOB IN('M', 'U');
        UPDATE #temp_VSS
          SET DOB_Adjust = DOB;
        UPDATE #temp_VSS
          SET DOB_Adjust = DATEADD(yyyy, 1, DOB)
        WHERE MONTH(DOB) > MONTH(Service_receive_dtm)
              OR (MONTH(DOB) = MONTH(Service_receive_dtm)
                  AND DAY(DOB) > DAY(Service_receive_dtm));

        -- =============================================  
        --  Validation for #WS02 end
        -- ============================================= 
        -- =============================================  
        -- Initialization  
        -- =============================================  
        -- =============================================  
        -- Initialization for #WS02
        -- =============================================  

        SELECT @Current_scheme_desc = Season_Desc
        FROM VaccineSeason WITH(NOLOCK)
        WHERE Scheme_Code = @Scheme_Code
              AND Scheme_Seq = @current_scheme_Seq
              AND Subsidize_Item_Code = 'SIV';

        -----------------------------------------  
        -- (iii) Seasonal Influenza Vaccination  
        -----------------------------------------  
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1
               )
        VALUES
              (1, 
               'Reporting period: as at ' + CONVERT(VARCHAR, DATEADD(dd, -1, @Cutoff_Dtm), 111)
              );

        INSERT INTO #WS02(_display_seq)
        VALUES(2);

        INSERT INTO #WS02
               (_display_seq, 
                _result_value1
               )
        VALUES
              (3, 
               REPLACE('(i) Seasonal Influenza Vaccination ([DATE]) - All Service Providers', '[DATE]', @current_scheme_desc)
              );
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4
               )
        VALUES
              (4, 
               '', 
               '', 
               '', 
               'No. of SP involved'
              );

        -- SIV --   
        DECLARE @Row AS INT= 5;
        DECLARE Category_Cursor CURSOR
        FOR SELECT DISTINCT 
                   Category_Code, 
                   Category_Seq
            FROM @SIV_ByCategory
            ORDER BY Category_Seq;
        OPEN Category_Cursor;
        FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq;
        WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @QIV_Subsidize_Code = '';

                SET @LAIV_Subsidize_Code = '';
                SELECT @QIV_Subsidize_Code = Subsidize_Code
                FROM @SIV_ByCategory
                WHERE Category_Code = @Category_Code
                      AND Subsidize_Code LIKE '%QIVG';

                SELECT @LAIV_Subsidize_Code = Subsidize_Code
                FROM @SIV_ByCategory
                WHERE Category_Code = @Category_Code
                      AND Subsidize_Code LIKE '%LAIVG';

                -- Header --      
                INSERT INTO #WS02
                       (_display_seq, 
                        _result_value1, 
                        _result_value2, 
                        _result_value3
                       )
                VALUES
                      (@Row, 
                       '', 
                       '', 
                       'Sub-total'
                      );
                UPDATE #WS02
                  SET _result_value1 = (
                      SELECT Display_Code_For_claim
                      FROM @SIV_ByCategory
                      WHERE Subsidize_Code = @QIV_Subsidize_Code)
                WHERE _display_seq = @Row;
                UPDATE #WS02
                  SET _result_value2 = (
                      SELECT Display_Code_For_claim
                      FROM @SIV_ByCategory
                      WHERE Subsidize_Code = @LAIV_Subsidize_Code)
                WHERE _display_seq = @Row;

                -- QIV --    
                SET @result1 = (
                SELECT COUNT(1)
                FROM #temp_VSS
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND Subsidize_Code = @QIV_Subsidize_Code);    
                -- LAIV --  
                SET @result2 = (
                SELECT COUNT(1)
                FROM #temp_VSS
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND Subsidize_Code = @LAIV_Subsidize_Code);

                SET @result2 = CASE
                                   WHEN(
                               SELECT Display_Code_For_claim
                               FROM @SIV_ByCategory
                               WHERE Subsidize_Code = @LAIV_Subsidize_Code) IS NULL
                                   THEN NULL
                                   ELSE(
                                   SELECT COUNT(1)
                                   FROM #temp_VSS
                                   WHERE IsSIV = 1
                                         AND IsCurrentSeason = 1
                                         AND Subsidize_Code = @LAIV_Subsidize_Code)
                               END;

                -- Sub-total  
                SET @result4 = @result1 + ISNULL(@result2, 0);    
                -- No. of SP Involved  
                SET @result5 = (
                SELECT COUNT(DISTINCT SP_ID)
                FROM #temp_VSS
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND (Subsidize_Code IN(@QIV_Subsidize_Code, @LAIV_Subsidize_Code))
                OR (@LAIV_Subsidize_Code IS NULL
                    OR Subsidize_Code = @LAIV_Subsidize_Code));

                INSERT INTO #WS02
                       (_display_seq, 
                        _result_value1, 
                        _result_value2, 
                        _result_value3, 
                        _result_value4
                       )
                VALUES
                      (@Row, 
                       @result1, 
                       @result2, 
                       @result4, 
                       @result5
                      );
                SET @Row = @Row + 1;
                INSERT INTO #WS02(_display_seq)
                VALUES(@Row);
                SET @Row = @Row + 1;
                FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq;
            END;
        CLOSE Category_Cursor;
        DEALLOCATE Category_Cursor;

        -- Total  
        -- start from display_seq = 15  
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4
               )
        VALUES
              (17, 
               'QIV Total', 
               'LAIV Total', 
               'Total', 
               'Total No. of SP involved'
              );
        INSERT INTO #WS02(_display_seq)
        VALUES(18);
        UPDATE #WS02
          SET _result_value1 = (
              SELECT COUNT(1)
              FROM #temp_VSS
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND Subsidize_Code LIKE '%QIVG')
        WHERE _display_seq = 18;
        UPDATE #WS02
          SET _result_value2 = (
              SELECT COUNT(1)
              FROM #temp_VSS
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND Subsidize_Code LIKE '%LAIVG')
        WHERE _display_seq = 18;

        UPDATE #WS02
          SET _result_value3 = (
              SELECT COUNT(1)
              FROM #temp_VSS
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND (Subsidize_Code LIKE '%QIVG'
                         OR Subsidize_Code LIKE '%LAIVG'))
        WHERE _display_seq = 18;
        UPDATE #WS02
          SET _result_value4 = (
              SELECT COUNT(DISTINCT SP_ID)
              FROM #temp_VSS
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1)
        WHERE _display_seq = 18;
        INSERT INTO #WS02(_display_seq)
        VALUES(19);
        INSERT INTO #WS02(_display_seq)
        VALUES(20);

        --==================================== NGO doctors and coordinated by Home Affairs Department (HAD) only =================================================-
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1
               )
        VALUES
              (21, 
               REPLACE('(ii) Seasonal Influenza Vaccination ([DATE]) - NGO doctors and coordinated by Home Affairs Department (HAD) only', '[DATE]',
               @current_scheme_desc)
              );
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4
               )
        VALUES
              (22, 
               '', 
               '', 
               '', 
               'No. of SP involved'
              );

        -- SIV --   
        SET @Row = @Row + 6;
        DECLARE Category_Cursor CURSOR
        FOR SELECT DISTINCT 
                   Category_Code, 
                   Category_Seq
            FROM @SIV_ByCategory
            ORDER BY Category_Seq;
        OPEN Category_Cursor;
        FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq;
        WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @QIV_Subsidize_Code = '';

                SET @LAIV_Subsidize_Code = '';
                SELECT @QIV_Subsidize_Code = Subsidize_Code
                FROM @SIV_ByCategory
                WHERE Category_Code = @Category_Code
                      AND Subsidize_Code LIKE '%QIVG';

                SELECT @LAIV_Subsidize_Code = Subsidize_Code
                FROM @SIV_ByCategory
                WHERE Category_Code = @Category_Code
                      AND Subsidize_Code LIKE '%LAIVG';

                -- Header --      
                INSERT INTO #WS02
                       (_display_seq, 
                        _result_value1, 
                        _result_value2, 
                        _result_value3
                       )
                VALUES
                      (@Row, 
                       '', 
                       '', 
                       'Sub-total'
                      );
                UPDATE #WS02
                  SET _result_value1 = (
                      SELECT Display_Code_For_claim
                      FROM @SIV_ByCategory
                      WHERE Subsidize_Code = @QIV_Subsidize_Code)
                WHERE _display_seq = @Row;
                UPDATE #WS02
                  SET _result_value2 = (
                      SELECT Display_Code_For_claim
                      FROM @SIV_ByCategory
                      WHERE Subsidize_Code = @LAIV_Subsidize_Code)
                WHERE _display_seq = @Row;

                -- QIV --    
                SET @result1 = (
                SELECT COUNT(1)
                FROM #temp_VSS ts
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND Subsidize_Code = @QIV_Subsidize_Code
                      AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'A'
                )));    
                -- LAIV --  
                SET @result2 = (
                SELECT COUNT(1)
                FROM #temp_VSS ts
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND Subsidize_Code = @LAIV_Subsidize_Code
                      AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'A'
                )));

                SET @result2 = CASE
                                   WHEN(
                               SELECT Display_Code_For_claim
                               FROM @SIV_ByCategory
                               WHERE Subsidize_Code = @LAIV_Subsidize_Code) IS NULL
                                   THEN NULL
                                   ELSE(
                                   SELECT COUNT(1)
                                   FROM #temp_VSS ts
                                   WHERE IsSIV = 1
                                         AND IsCurrentSeason = 1
                                         AND Subsidize_Code = @LAIV_Subsidize_Code
                                         AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'A'
                )))
                               END;

                -- Sub-total  
                SET @result4 = @result1 + ISNULL(@result2, 0);    
                -- No. of SP Involved  
                SET @result5 = (
                SELECT COUNT(DISTINCT SP_ID)
                FROM #temp_VSS ts
                WHERE(IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND (Subsidize_Code IN(@QIV_Subsidize_Code, @LAIV_Subsidize_Code))
                OR (@LAIV_Subsidize_Code IS NULL
                    OR Subsidize_Code = @LAIV_Subsidize_Code))
                AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'A'
                )));

                INSERT INTO #WS02
                       (_display_seq, 
                        _result_value1, 
                        _result_value2, 
                        _result_value3, 
                        _result_value4
                       )
                VALUES
                      (@Row, 
                       @result1, 
                       @result2, 
                       @result4, 
                       @result5
                      );
                SET @Row = @Row + 1;
                INSERT INTO #WS02(_display_seq)
                VALUES(@Row);
                SET @Row = @Row + 1;
                FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq;
            END;
        CLOSE Category_Cursor;
        DEALLOCATE Category_Cursor;

        -- Total  
        -- start from display_seq = 15  
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4
               )
        VALUES
              (35, 
               'QIV Total', 
               'LAIV Total', 
               'Total', 
               'Total No. of SP involved'
              );
        INSERT INTO #WS02(_display_seq)
        VALUES(36);
        UPDATE #WS02
          SET _result_value1 = (
              SELECT COUNT(1)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND Subsidize_Code LIKE '%QIVG'
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
			      AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'A'
        )))
        WHERE _display_seq = 36;
        UPDATE #WS02
          SET _result_value2 = (
              SELECT COUNT(1)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND Subsidize_Code LIKE '%LAIVG'
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
			      AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'A'
        )))
        WHERE _display_seq = 36;

        UPDATE #WS02
          SET _result_value3 = (
              SELECT COUNT(1)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND (Subsidize_Code LIKE '%QIVG'
                         OR Subsidize_Code LIKE '%LAIVG')
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
			      AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'A'
        )))
        WHERE _display_seq = 36;
        UPDATE #WS02
          SET _result_value4 = (
              SELECT COUNT(DISTINCT SP_ID)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
			      AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'A'
        )))
        WHERE _display_seq = 36;
        INSERT INTO #WS02(_display_seq)
        VALUES(37);
        INSERT INTO #WS02(_display_seq)
        VALUES(38);

        --==================================== NGO doctors and coordinated by Home Affairs Department (HAD) only =================================================-
        --====================================  K&T DHC VSS doctors only =================================================-
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1
               )
        VALUES
              (39, 
               REPLACE('(iii) Seasonal Influenza Vaccination ([DATE]) - K&T DHC VSS doctors only', '[DATE]', @current_scheme_desc)
              );
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4
               )
        VALUES
              (40, 
               '', 
               '', 
               '', 
               'No. of SP involved'
              );

        -- SIV --   
        SET @Row = @Row + 6;
        DECLARE Category_Cursor CURSOR
        FOR SELECT DISTINCT 
                   Category_Code, 
                   Category_Seq
            FROM @SIV_ByCategory
            ORDER BY Category_Seq;
        OPEN Category_Cursor;
        FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq;
        WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @QIV_Subsidize_Code = '';

                SET @LAIV_Subsidize_Code = '';
                SELECT @QIV_Subsidize_Code = Subsidize_Code
                FROM @SIV_ByCategory
                WHERE Category_Code = @Category_Code
                      AND Subsidize_Code LIKE '%QIVG';

                SELECT @LAIV_Subsidize_Code = Subsidize_Code
                FROM @SIV_ByCategory
                WHERE Category_Code = @Category_Code
                      AND Subsidize_Code LIKE '%LAIVG';

                -- Header --      
                INSERT INTO #WS02
                       (_display_seq, 
                        _result_value1, 
                        _result_value2, 
                        _result_value3
                       )
                VALUES
                      (@Row, 
                       '', 
                       '', 
                       'Sub-total'
                      );
                UPDATE #WS02
                  SET _result_value1 = (
                      SELECT Display_Code_For_claim
                      FROM @SIV_ByCategory
                      WHERE Subsidize_Code = @QIV_Subsidize_Code)
                WHERE _display_seq = @Row;
                UPDATE #WS02
                  SET _result_value2 = (
                      SELECT Display_Code_For_claim
                      FROM @SIV_ByCategory
                      WHERE Subsidize_Code = @LAIV_Subsidize_Code)
                WHERE _display_seq = @Row;

                -- QIV --    
                SET @result1 = (
                SELECT COUNT(1)
                FROM #temp_VSS ts
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND Subsidize_Code = @QIV_Subsidize_Code
                      AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'B'
                )));    
                -- LAIV --  
                SET @result2 = (
                SELECT COUNT(1)
                FROM #temp_VSS ts
                WHERE IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND Subsidize_Code = @LAIV_Subsidize_Code
                      AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'B'
                )));

                SET @result2 = CASE
                                   WHEN(
                               SELECT Display_Code_For_claim
                               FROM @SIV_ByCategory
                               WHERE Subsidize_Code = @LAIV_Subsidize_Code) IS NULL
                                   THEN NULL
                                   ELSE(
                                   SELECT COUNT(1)
                                   FROM #temp_VSS ts
                                   WHERE IsSIV = 1
                                         AND IsCurrentSeason = 1
                                         AND Subsidize_Code = @LAIV_Subsidize_Code
                                         AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'B'
                )))
                               END;

                -- Sub-total  
                SET @result4 = @result1 + ISNULL(@result2, 0);    
                -- No. of SP Involved  
                SET @result5 = (
                SELECT COUNT(DISTINCT SP_ID)
                FROM #temp_VSS ts
                WHERE(IsSIV = 1
                      AND IsCurrentSeason = 1
                      AND (Subsidize_Code IN(@QIV_Subsidize_Code, @LAIV_Subsidize_Code))
                OR (@LAIV_Subsidize_Code IS NULL
                    OR Subsidize_Code = @LAIV_Subsidize_Code))
                AND (EXISTS
                (
                    SELECT 1
                    FROM GovSIVHAD AS gs WITH(NOLOCK)
                    WHERE gs.SP_ID = ts.SP_ID
						  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                          AND gs.[GROUP] LIKE 'B'
                )));

                INSERT INTO #WS02
                       (_display_seq, 
                        _result_value1, 
                        _result_value2, 
                        _result_value3, 
                        _result_value4
                       )
                VALUES
                      (@Row, 
                       @result1, 
                       @result2, 
                       @result4, 
                       @result5
                      );
                SET @Row = @Row + 1;
                INSERT INTO #WS02(_display_seq)
                VALUES(@Row);
                SET @Row = @Row + 1;
                FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq;
            END;
        CLOSE Category_Cursor;
        DEALLOCATE Category_Cursor;

        -- Total  
        -- start from display_seq = 15  
        INSERT INTO #WS02
               (_display_seq, 
                _result_value1, 
                _result_value2, 
                _result_value3, 
                _result_value4
               )
        VALUES
              (54, 
               'QIV Total', 
               'LAIV Total', 
               'Total', 
               'Total No. of SP involved'
              );
        INSERT INTO #WS02(_display_seq)
        VALUES(55);
        UPDATE #WS02
          SET _result_value1 = (
              SELECT COUNT(1)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND Subsidize_Code LIKE '%QIVG'
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
			      AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'B'
        )))
        WHERE _display_seq = 55;
        UPDATE #WS02
          SET _result_value2 = (
              SELECT COUNT(1)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND Subsidize_Code LIKE '%LAIVG'
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
				  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'B'
        )))
        WHERE _display_seq = 55;

        UPDATE #WS02
          SET _result_value3 = (
              SELECT COUNT(1)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND (Subsidize_Code LIKE '%QIVG'
                         OR Subsidize_Code LIKE '%LAIVG')
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
				  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'B'
        )))
        WHERE _display_seq = 55;
        UPDATE #WS02
          SET _result_value4 = (
              SELECT COUNT(DISTINCT SP_ID)
              FROM #temp_VSS ts
              WHERE IsSIV = 1
                    AND IsCurrentSeason = 1
                    AND (EXISTS
        (
            SELECT 1
            FROM GovSIVHAD AS gs WITH(NOLOCK)
            WHERE gs.SP_ID = ts.SP_ID
				  AND gs.Practice_Display_Seq = ts.Practice_Display_Seq
                  AND gs.[GROUP] LIKE 'B'
        )))
        WHERE _display_seq = 55;
        INSERT INTO #WS02(_display_seq)
        VALUES(56);
        INSERT INTO #WS02(_display_seq)
        VALUES(57);

        --====================================  K&T DHC VSS doctors only =================================================-
        --========================================= #WS2 table finish =============================================-
        -- =============================================  
        -- Initialization for #WS02 end
        -- =============================================  
        -- =============================================  
        -- Initialization for #WS03
        -- =============================================  

        EXEC [proc_SymmetricKey_open]
        SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm);
        SET @system_Dtm = GETDATE();
        SET @Date_Range = 7;
        SET @Scheme_Code = 'VSS';

        -- --------------------------------------------- 
        -- Transactions 
        -- --------------------------------------------- 
        INSERT INTO @Transaction
               (sp_id, 
                transaction_id, 
                transaction_dtm, 
                service_receive_dtm, 
                subsidize_item_code, 
                dose, 
                scheme_seq, 
                per_unit_value, 
                voucher_acc_id, 
                temp_voucher_acc_id, 
                special_acc_id, 
                invalid_acc_id, 
                doc_code, 
                transaction_status, 
                reimbursement_status, 
                create_by_smartid, 
                row, 
                vaccine, 
                category_code, 
                hkic_symbol
               )
        SELECT vt.sp_id, 
               vt.transaction_id, 
               vt.transaction_dtm, 
               vt.service_receive_dtm, 
               td.subsidize_item_code, 
               td.available_item_code AS [Dose], 
               td.scheme_seq, 
               td.per_unit_value, 
               ISNULL(vt.voucher_acc_id, ''), 
               ISNULL(vt.temp_voucher_acc_id, ''), 
               ISNULL(vt.special_acc_id, ''), 
               ISNULL(vt.invalid_acc_id, ''), 
               vt.doc_code, 
               vt.record_status AS [Transaction_Status], 
               NULL AS [Reimbursement_Status], 
               vt.create_by_smartid, 
               10 + ROW_NUMBER() OVER(
               ORDER BY vt.transaction_dtm), 
               sgc.display_code_for_claim AS [Vaccine], 
               vt.category_code, 
               vt.hkic_symbol
        FROM vouchertransaction AS VT WITH(NOLOCK)
             INNER JOIN transactiondetail AS td WITH(NOLOCK)
             ON vt.transaction_id = td.transaction_id
                AND vt.scheme_code = @Scheme_Code
                AND vt.scheme_code = td.scheme_code
             LEFT JOIN subsidizegroupclaim AS SGC WITH(NOLOCK)
             ON td.scheme_code = sgc.scheme_code
                AND td.scheme_seq = sgc.scheme_seq
                AND td.subsidize_code = sgc.subsidize_code
        WHERE vt.scheme_code = @Scheme_Code
              AND td.subsidize_code LIKE '%G'
              AND vt.transaction_dtm <= @Cutoff_Dtm
              AND vt.transaction_dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)
              AND vt.record_status NOT IN
        (
            SELECT status_value
            FROM statstatusfiltermapping WITH(NOLOCK)
            WHERE(report_id = 'ALL'
                  OR report_id = @Report_ID)
                 AND table_name = 'VoucherTransaction'
                 AND status_name = 'Record_Status'
                 AND ((effective_date IS NULL
                       OR effective_date <= @cutoff_dtm)
                      AND (expiry_date IS NULL
                           OR expiry_date >= @cutoff_dtm))
        )
              AND (vt.invalidation IS NULL
                   OR vt.invalidation NOT IN
        (
            SELECT status_value
            FROM statstatusfiltermapping WITH(NOLOCK)
            WHERE(report_id = 'ALL'
                  OR report_id = @Report_ID)
                 AND table_name = 'VoucherTransaction'
                 AND status_name = 'Invalidation'
                 AND ((effective_date IS NULL
                       OR effective_date <= @cutoff_dtm)
                      AND (expiry_date IS NULL
                           OR expiry_date >= @cutoff_dtm))
        ))
        ORDER BY vt.transaction_dtm; 
        -- --------------------------------------------- 
        -- Patch the Reimbursement_Status 
        -- for transaction created in payment outside eHS 
        -- --------------------------------------------- 
        UPDATE @Transaction
          SET reimbursement_status = 'R'
        WHERE transaction_status = 'R'; 
        -- --------------------------------------------- 
        -- Patch the Reimbursement_Status 
        -- --------------------------------------------- 
        UPDATE @Transaction
          SET reimbursement_status = CASE rat.authorised_status
                                         WHEN 'R'
                                         THEN 'G'
                                         ELSE rat.authorised_status
                                     END
        FROM @Transaction VT
             INNER JOIN reimbursementauthtran RAT WITH(NOLOCK)
             ON vt.transaction_id = rat.transaction_id
        WHERE vt.transaction_status = 'A'; 
        -- --------------------------------------------- 
        -- Patch the Transaction_Status 
        -- --------------------------------------------- 
        UPDATE @Transaction
          SET transaction_status = 'R'
        WHERE reimbursement_status = 'G'; 
        -- --------------------------------------------- 
        -- Validated accounts 
        -- --------------------------------------------- 
        INSERT INTO @Account
               (sp_id, 
                transaction_id, 
                transaction_dtm, 
                service_receive_dtm, 
                dose, 
                dob, 
                exact_dob, 
                sex, 
                doc_code, 
                transaction_status, 
                reimbursement_status, 
                row, 
                vaccine, 
                category_code
               )
        SELECT vt.sp_id, 
               vt.transaction_id, 
               vt.transaction_dtm, 
               vt.service_receive_dtm, 
               vt.dose, 
               vp.dob, 
               vp.exact_dob, 
               vp.sex, 
               vt.doc_code, 
               vt.transaction_status, 
               vt.reimbursement_status, 
               vt.row, 
               vt.vaccine, 
               vt.category_code
        FROM @Transaction AS VT
             INNER JOIN personalinformation AS VP WITH(NOLOCK)
             ON vt.voucher_acc_id = vp.voucher_acc_id
                AND vt.doc_code = vp.doc_code
        WHERE vt.voucher_acc_id <> ''; 
        -- --------------------------------------------- 
        -- Temporary accounts 
        -- --------------------------------------------- 
        INSERT INTO @Account
               (sp_id, 
                transaction_id, 
                transaction_dtm, 
                service_receive_dtm, 
                dose, 
                dob, 
                exact_dob, 
                sex, 
                doc_code, 
                transaction_status, 
                reimbursement_status, 
                row, 
                vaccine, 
                category_code
               )
        SELECT vt.sp_id, 
               vt.transaction_id, 
               vt.transaction_dtm, 
               vt.service_receive_dtm, 
               vt.dose, 
               tp.dob, 
               tp.exact_dob, 
               tp.sex, 
               vt.doc_code, 
               vt.transaction_status, 
               vt.reimbursement_status, 
               vt.row, 
               vt.vaccine, 
               vt.category_code
        FROM @Transaction AS VT
             INNER JOIN temppersonalinformation AS TP WITH(NOLOCK)
             ON vt.temp_voucher_acc_id = tp.voucher_acc_id
        WHERE vt.voucher_acc_id = ''
              AND vt.temp_voucher_acc_id <> ''
              AND vt.special_acc_id = ''; 
        -- --------------------------------------------- 
        -- Special accounts 
        -- --------------------------------------------- 
        INSERT INTO @Account
               (sp_id, 
                transaction_id, 
                transaction_dtm, 
                service_receive_dtm, 
                dose, 
                dob, 
                exact_dob, 
                sex, 
                doc_code, 
                transaction_status, 
                reimbursement_status, 
                row, 
                vaccine, 
                category_code
               )
        SELECT vt.sp_id, 
               vt.transaction_id, 
               vt.transaction_dtm, 
               vt.service_receive_dtm, 
               vt.dose, 
               sp.dob, 
               sp.exact_dob, 
               sp.sex, 
               vt.doc_code, 
               vt.transaction_status, 
               vt.reimbursement_status, 
               vt.row, 
               vt.vaccine, 
               vt.category_code
        FROM @Transaction AS VT
             INNER JOIN specialpersonalinformation AS SP WITH(NOLOCK)
             ON vt.special_acc_id = sp.special_acc_id
        WHERE vt.voucher_acc_id = ''
              AND vt.special_acc_id <> ''
              AND vt.invalid_acc_id = ''; 
        -- ============================================= 
        -- Process data 
        -- ============================================= 
        -- --------------------------------------------- 
        -- Build frame 
        -- --------------------------------------------- 
        DECLARE @Display_Text_RecepientCondition VARCHAR(100);
        SELECT @Display_Text_RecepientCondition = description
        FROM systemresource WITH(NOLOCK)
        WHERE objecttype = 'Text'
              AND objectname = 'RecipientCondition';

        INSERT INTO #WS03
               (result_seq, 
                result_value1
               )
        VALUES
              (2, 
               'Reporting period: the week ending ' + CONVERT(VARCHAR, DATEADD(dd, -1, @Cutoff_Dtm), 111)
              );
        INSERT INTO #WS03(result_seq)
        VALUES(3);
        INSERT INTO #WS03
               (result_seq, 
                result_value1, 
                result_value2, 
                result_value3, 
                result_value4, 
                result_value5, 
                result_value6, 
                result_value7, 
                result_value8, 
                result_value9, 
                result_value10, 
                result_value11, 
                result_value12, 
                result_value13, 
                result_value14, 
                result_value15
               )
        VALUES
              (4, 
               'Transaction ID', 
               'Transaction Time', 
               'SPID', 
               'Service Date', 
               'Category', 
               'Subsidy', 
               'Dose', 
               'DOB', 
               'DOB Flag', 
               'Gender', 
               'Doc Type', 
               'HKIC Symbol', 
               'Transaction Status', 
               'Reimbursement Status', 
               'Means of Input'
              ); 
        -- --------------------------------------------- 
        -- Build data 
        -- --------------------------------------------- 
        INSERT INTO #WS03
               (result_seq, 
                result_value1, 
                result_value2, 
                result_value3, 
                result_value4, 
                result_value5, 
                result_value6, 
                result_value7, 
                result_value8, 
                result_value9, 
                result_value10, 
                result_value11, 
                result_value12, 
                result_value13, 
                result_value14, 
                result_value15
               )
        SELECT a.row, 
               dbo.Func_format_system_number(a.transaction_id), 
               CONVERT(VARCHAR, t.transaction_dtm, 20), 
               t.sp_id, 
               Format(t.service_receive_dtm, 'yyyy/MM/dd'), 
               cc.category_name AS [Category], 
               t.vaccine AS 'Subsidy',
               CASE a.dose
                   WHEN 'ONLYDOSE'
                   THEN 'Only Dose'
                   ELSE sid.available_item_desc
               END AS [Dose], 
               Format(a.dob, 'yyyy/MM/dd'), 
               a.exact_dob, 
               a.sex, 
               a.doc_code,
               CASE
                   WHEN ISNULL(sd3.status_description, '') = ''
                   THEN @Str_NA
                   ELSE sd3.status_description
               END, 
               sd1.status_description, 
               ISNULL(sd2.status_description, ''),
               CASE
                   WHEN t.create_by_smartid = 'Y'
                   THEN 'Card Reader'
                   ELSE 'Manual'
               END AS create_by_smartid
        FROM @Account AS A
             INNER JOIN @Transaction AS T
             ON a.transaction_id = t.transaction_id
                AND a.vaccine = t.vaccine
             INNER JOIN subsidizeitemdetails AS SID WITH(NOLOCK)
             ON a.dose = sid.available_item_code
                AND sid.subsidize_item_code = t.subsidize_item_code
             INNER JOIN statusdata AS SD1 WITH(NOLOCK)
             ON a.transaction_status = sd1.status_value
                AND sd1.enum_class = 'ClaimTransStatus'
             INNER JOIN claimcategory AS CC WITH(NOLOCK)
             ON a.category_code = cc.category_code
             LEFT JOIN statusdata AS SD2 WITH(NOLOCK)
             ON a.reimbursement_status = sd2.status_value
                AND sd2.enum_class = 'ReimbursementStatus'
             LEFT JOIN statusdata AS SD3 WITH(NOLOCK)
             ON t.hkic_symbol = sd3.status_value
                AND sd3.enum_class = 'HKICSymbol';

        EXEC [proc_SymmetricKey_close]

        -- =============================================  
        -- Initialization for #WS03 end
        -- =============================================  
        -- =============================================  
        -- Initialization for #WS04 end
        -- =============================================  
 
		INSERT INTO #WS04
               (result_value1, 
                result_value2
               ) values ('(i) NGO doctors and coordinated by Home Affairs Department (HAD)','');
		INSERT INTO #WS04
               (result_value1, 
                result_value2
               ) values ('','');
		INSERT INTO #WS04
               (result_value1, 
                result_value2
               ) values ('SPID','Practice No.');

        INSERT INTO #WS04
               (result_value1, 
                result_value2
               )
        SELECT 
               SP_ID, 
               Practice_Display_Seq
        FROM GovSIVHAD WITH(NOLOCK) where [GROUP] like 'A';

		INSERT INTO #WS04
               (
                result_value1, 
                result_value2
               ) values ('','');
		
		INSERT INTO #WS04
               (
                result_value1, 
                result_value2
               ) values ('(ii) K&T DHC VSS doctors','');
		INSERT INTO #WS04
               (
                result_value1, 
                result_value2
               ) values ('','')
		INSERT INTO #WS04
               ( 
                result_value1, 
                result_value2
               ) values ('SPID','Practice No.');

        INSERT INTO #WS04
               (
                result_value1, 
                result_value2
               )
        SELECT  
               SP_ID, 
               Practice_Display_Seq
        FROM GovSIVHAD WITH(NOLOCK) where [GROUP] like 'B';

        -- =============================================  
        -- Initialization for #WS04 end
        -- =============================================  
        -- ============================================= 
        -- Return result 
        -- ============================================= 
        -- =============================================  
        -- Return results  
        -- =============================================  
        -- --------------------------------------------------    
        -- Content Page  WS01  Content
        -- --------------------------------------------------  

        DECLARE @ContentTable TABLE
        (Display_Seq INT IDENTITY(1, 1), 
         Value1      VARCHAR(100), 
         Value2      VARCHAR(100)
        );
        DECLARE @strGenDtm VARCHAR(50);
        DECLARE @schemeDate DATETIME;
        SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108);
        SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm) - 3);
        SET @schemeDate = CONVERT(VARCHAR(11), DATEADD(dd, -1, @strGenDtm), 111);

        EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'VSS', 
                                                                @schemeDate;

        INSERT INTO @ContentTable(Value1)
        SELECT 'Report Generation Time: ' + @strGenDtm;
        SELECT ISNULL(Value1, ''), 
               ISNULL(Value2, '')
        FROM @ContentTable
        ORDER BY Display_Seq;

        -- --------------------------------------------------    
        -- Content Page  WS02  -Summary
        -- --------------------------------------------------  
        SELECT 
        --ISNULL(_display_seq, ''), 
        ISNULL(_result_value1, ''), 
        ISNULL(_result_value2, ''), 
        ISNULL(_result_value3, ''), 
        ISNULL(_result_value4, ''), 
        ISNULL(_result_value5, ''), 
        ISNULL(_result_value6, ''), 
        ISNULL(_result_value7, ''), 
        ISNULL(_result_value8, ''), 
        ISNULL(_result_value9, ''), 
        ISNULL(_result_value10, ''), 
        ISNULL(_result_value11, ''), 
        ISNULL(_result_value12, ''), 
        ISNULL(_result_value13, ''), 
        ISNULL(_result_value14, ''), 
        ISNULL(_result_value15, ''), 
        ISNULL(_result_value16, ''), 
        ISNULL(_result_value17, ''), 
        ISNULL(_result_value18, ''), 
        ISNULL(_result_value19, ''), 
        ISNULL(_result_value20, ''), 
        ISNULL(_result_value21, ''), 
        ISNULL(_result_value22, ''), 
        ISNULL(_result_value23, ''), 
        ISNULL(_result_value24, ''), 
        ISNULL(_result_value25, ''), 
        ISNULL(_result_value26, ''), 
        ISNULL(_result_value27, ''), 
        ISNULL(_result_value28, '')
        FROM #WS02 WITH(NOLOCK)
        ORDER BY _display_seq;

        -- --------------------------------------------------    
        -- Content Page  WS03  -Raw Data
        -- --------------------------------------------------  
        --ORDER BY Display_Seq;
        SELECT result_value1, 
               result_value2, 
               result_value3, 
               result_value4, 
               result_value5, 
               result_value6, 
               result_value7, 
               result_value8, 
               result_value9, 
               result_value10, 
               result_value11, 
               result_value12, 
               result_value13, 
               result_value14, 
               result_value15, 
               result_value16
        FROM #WS03
        ORDER BY result_seq;

        -- --------------------------------------------------    
        -- Content Page  WS04  -HAD
        -- --------------------------------------------------  
        --      --ORDER BY Display_Seq;

        SELECT result_value1, 
               result_value2
        FROM #WS04
        ORDER BY result_seq;

        -- --------------------------------------------------    
        -- Content Page  WS05  Remark
        -- --------------------------------------------------  

        EXEC [proc_EHS_VaccineRemark_Stat_Read];
    END;  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0008_Report_get] TO HCVU;
GO