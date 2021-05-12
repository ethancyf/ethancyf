        IF EXISTS (
            SELECT
                *
            FROM
                dbo.sysobjects
            WHERE
                id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0010_Report_get]')
                AND OBJECTPROPERTY(id, N'IsProcedure') = 1
        ) BEGIN DROP PROCEDURE [dbo].[proc_EHS_eHSW0010_Report_get];

        END;

        GO
	-- =============================================
	-- Author:			Raiman Chong
	-- Create date:		12 April 2021
	-- CR No.:			CRE20-023 
	-- Description:		VSS COVID19 Weekly Report
	-- =============================================
	--exec [proc_EHS_eHSW0010_Report_get]
		
	CREATE PROCEDURE [dbo].[proc_EHS_eHSW0010_Report_get] @request_time DATETIME = NULL,-- The reference date to get @CutOff_Dtm. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz 
														  @Cutoff_Dtm DATETIME = NULL -- Inclusive, The Cut Off Date. If defined, it will override the value from the @request_dtm    
		AS BEGIN 
		-- =============================================  
		-- Declaration  
		-- =============================================  
        DECLARE @ReportDtm DATETIME;
		DECLARE @In_Request_Time DATETIME   
		DECLARE @In_CutOff_Dtm DATETIME   

		SET @In_Request_Time = @Request_Time  
		SET @In_CutOff_Dtm = @CutOff_Dtm  

		IF @In_Request_Time IS NULL  
		  SET @In_Request_Time = GETDATE()  

		 -- Ensure the time start from 00:00 (DATETIME compare logic use "<")  
		 IF @In_CutOff_Dtm IS NULL  
		  SET @ReportDtm = CONVERT(VARCHAR, @In_Request_Time, 106)     
		 ELSE  
		  SET @ReportDtm = CONVERT(VARCHAR, DATEADD(d, 1, @In_CutOff_Dtm), 106)-- "106" gives "dd MMM yyyy"    

        -- The date report data as at   
        DECLARE @current_dtm DATETIME;

        DECLARE @PeriodDays INT;

        DECLARE @In_Period_From DATETIME;

        DECLARE @In_Period_To DATETIME;

        -- For Report on means of input for COVID-19 under VSS - Summary
        Declare @In_NumOfSPInvolved Int;

        Declare @In_NumOfTransaction Int;

        Declare @In_NumOfTransaction_SmartID Int;

        Declare @In_NumOfTransaction_Manual Int;

        --For Report on submission of COVID-19 under VSS from 2200 hours to 0800 hours of the following day - Summary
        Declare @In_NumOfSPInvolved_OddHours Int;

        Declare @In_NumOfPracticeInvolved_OddHours Int;

        Declare @In_NumOfTransaction_OddHours Int;

        DECLARE @Tran01 table (
            SP_ID char(8),
            Practice_Display_Seq smallInt,
            Transaction_ID VARCHAR(30),
            Transaction_Dtm datetime,
            MeanOfInput VARCHAR(40),
			Voucher_Acc_ID   char(15),
			Temp_Voucher_Acc_ID char(15),
			Doc_Code char(20),
			Service_Receive_Dtm datetime
        ) 
		
		-- Create Worksheet 2(01a) Report on means of input for COVID-19 under VSS - Summary
        CREATE TABLE #WS02  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(1000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 3(01b) Report on means of input for COVID-19 under VSS - Summary by service provider practice
        CREATE TABLE #WS03  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(1000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT '',
            Col06 VARCHAR(1000) DEFAULT '',
            Col07 VARCHAR(2000) DEFAULT '',
            Col08 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 4(01c) Report on means of input for COVID-19 under VSS - Raw Data
        CREATE TABLE #WS04  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(2000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT '',
            Col06 VARCHAR(1000) DEFAULT '',
            Col07 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 5(02a) Report on submission of COVID-19 under VSS from 2200 hours to 0800 hours of the following day - Summary
        CREATE TABLE #WS05 
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(2000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 6(02b) Report on submission of COVID-19 under VSS from 2200 hours to 0800 hours of the following day - Raw Data
        CREATE TABLE #WS06  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(2000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT '',
            Col06 VARCHAR(1000) DEFAULT '',
            Col07 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 7(03a) Report on COVID19 claims using Passport under VSS
        CREATE TABLE #WS07  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(2000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT '',
            Col06 VARCHAR(1000) DEFAULT '',
            Col07 VARCHAR(1000) DEFAULT '',
            Col08 VARCHAR(1000) DEFAULT '',
            Col09 VARCHAR(1000) DEFAULT '',
            Col10 VARCHAR(1000) DEFAULT '',
            Col11 VARCHAR(1000) DEFAULT '',
            Col12 VARCHAR(1000) DEFAULT '',
            Col13 VARCHAR(1000) DEFAULT '',
            Col14 VARCHAR(1000) DEFAULT '',
            Col15 VARCHAR(1000) DEFAULT '',
            Col16 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 8(03b) Report on COVID19 claims using One Way Permit under VSS
        CREATE TABLE #WS08  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT '',
            Col02 VARCHAR(1000) DEFAULT '',
            Col03 VARCHAR(2000) DEFAULT '',
            Col04 VARCHAR(1000) DEFAULT '',
            Col05 VARCHAR(1000) DEFAULT '',
            Col06 VARCHAR(1000) DEFAULT '',
            Col07 VARCHAR(1000) DEFAULT '',
            Col08 VARCHAR(1000) DEFAULT '',
            Col09 VARCHAR(1000) DEFAULT '',
            Col10 VARCHAR(1000) DEFAULT '',
            Col11 VARCHAR(1000) DEFAULT '',
            Col12 VARCHAR(1000) DEFAULT '',
            Col13 VARCHAR(1000) DEFAULT '',
            Col14 VARCHAR(1000) DEFAULT '',
            Col15 VARCHAR(1000) DEFAULT '',
            Col16 VARCHAR(1000) DEFAULT ''
        );

		-- Create Worksheet 9 Remark
        CREATE TABLE #WS09  
        (
            Seq INT IDENTITY(1, 1),
            Col01 VARCHAR(1000) DEFAULT ''
        );

        -- =============================================
        -- Retrieve data
        -- =============================================
        SET  @current_dtm = @ReportDtm;

        SET @PeriodDays = 7;

        SET @In_Period_From = DATEADD(dd, -1 * @PeriodDays, @current_dtm);

        SET @In_Period_To = @current_dtm;

        --SET @In_Period_From = '2021-03-02'
        --      SET @In_Period_To = '2021-03-28'
        -- @Tran01=============================================
        INSERT INTO
            @Tran01
        SELECT
            VT.SP_ID,
            VT.Practice_Display_Seq,
            vt.transaction_id,
            VT.Transaction_Dtm,
            VT.Create_By_SmartID as MeanOfInput,
			VT.Voucher_Acc_ID,
			VT.Temp_Voucher_Acc_ID,
			vt.Doc_Code,
			VT.Service_Receive_Dtm 
        FROM
            VoucherTransaction VT with (nolock)
        WHERE
            VT.Transaction_Dtm >= @In_Period_From
            AND VT.Transaction_Dtm < @In_Period_To
            AND VT.Scheme_Code = 'VSS'
            AND VT.Category_Code = 'VSSCOVID19'
            AND VT.Record_Status NOT IN ('I', 'D')
            AND ISNULL(VT.Invalidation, '') <> 'I';

        UPDATE
            @Tran01
        SET
            MeanOfInput = (
                CASE
                    MeanOfInput
                    WHEN 'Y' THEN 'Card Reader'
                    WHEN 'N' THEN 'Manual'
                    ELSE MeanOfInput
                END
            ) 
		-- @Tran01=============================================
        -- =============================================
        -- Return result
        -- =============================================
        -- =============================================  
        -- Initialization  
        -- =============================================  
        ------------------------------------------------  
        -- For Excel Sheet (02): -- Create Worksheet 2(01a) Report on means of input for COVID-19 under VSS - Summary
        -- ---------------------------------------------  
        --set Number of service provider involved
        set
            @In_NumOfSPInvolved = (
                select
                    count(distinct sp_id)
                from
                    @Tran01
            ) 
			
		--set Number of Transaction ,Number of Transaction by SmartID,Number of Transaction by Manual  
        select
            @In_NumOfTransaction = count(*),
            @In_NumOfTransaction_SmartID = 
			ISNULL(
					SUM(
						CASE
							WHEN MeanOfInput = 'Card Reader' THEN 1
							ELSE 0
						END
						)
					,0
            ),
            @In_NumOfTransaction_Manual = 
			ISNULL(
					SUM(
						CASE
							WHEN MeanOfInput = 'Manual' THEN 1
							ELSE 0
						END
						)
					,0
			)
        FROM
            @Tran01 --insert @In_NumOfSPInvolved, @In_NumOfTransaction, @In_NumOfTransaction_SmartID, @In_NumOfTransaction_Manual for display
        INSERT INTO
            #WS02  
            (Col01, Col02, Col03)
        values
            (
                'Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111),
                '',
                null
            ),
        (null, null, null),
        (
                '(I)',
                'No.of service provider involved',
                @In_NumOfSPInvolved
            ),
        (null, null, null),
        (
                '(II)',
                'No. of claim transactions',
                @In_NumOfTransaction
            ),
        (
                '',
                'Using card reader',
                @In_NumOfTransaction_SmartID
            ),
        (
                '',
                'Using Manual Input',
                @In_NumOfTransaction_Manual
            );

        ------------------------------------------------  
        -- For Excel Sheet (03): -- Create Worksheet 3(01b) Report on means of input for COVID-19 under VSS - Summary by service provider practice
        -- ---------------------------------------------  
        INSERT INTO
            #WS03 
            (Col01)
        values
            (
                'Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111)
            ),
            ('')
        INSERT INTO
            #WS03 
            (Col04)
        values
            ('Means of Input')
        INSERT INTO
            #WS03  
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05,
                Col06
            )
        VALUES
            (
                'SPID',
                'Practice No.',
                'Total No. of transaction',
                'Card Reader',
                'Manual',
                'Manual input %'
            );

        INSERT INTO
            #WS03  
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05
            ) (
                select
                    sp_id,
                    Practice_Display_Seq,
                    count(*),
                    SUM(
                        CASE
                            WHEN vt.MeanOfInput = 'Card Reader' THEN 1
                            ELSE 0
                        END
                    ) AS [CardReader],
                    SUM(
                        CASE
                            WHEN vt.MeanOfInput = 'Manual' THEN 1
                            ELSE 0
                        END
                    ) AS [Manual]
                FROM
                    @Tran01 VT
                group by
                    sp_id,
                    Practice_Display_Seq
            )
        order by
            sp_id,
            Practice_Display_Seq;

        -- calculate the percentage of manual input
        update #WS03 set Col06 = cast (CONVERT(DECIMAL(5,2),  cast(Col05 as float) / cast(Col03 as float) *100) as varchar)  + '%'
        where
            ISNUMERIC(Col05) = 1
            and ISNUMERIC(Col03) = 1 

	--UPDATE  
	--	#WS03 SET Col06 = convert(varchar , (CASE WHEN Col05 = 0 THEN 0 
	--										ELSE CONVERT(DECIMAL(5,2), 100.0 * Col05 / Col03) END ) + '%';
		
		------------------------------------------------  
        -- For Excel Sheet (04): -- Create Worksheet 4(01c) Report on means of input for COVID-19 under VSS - Raw Data
        -- ---------------------------------------------  
        INSERT INTO
            #WS04 
            (Col01)
        values
            (
                'Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111)
            ),
            ('')
        INSERT INTO
            #WS04  
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05
            )
        VALUES
            (
                'SPID',
                'Practice No.',
                'Transaction ID',
                'Transaction Time',
                'Means of Input'
            );

        INSERT INTO
            #WS04  
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05
            ) (
                select
                    sp_id,
                    Practice_Display_Seq,
                    dbo.func_format_system_number(transaction_id),
                    CONVERT(VARCHAR, transaction_dtm, 20),
                    MeanOfInput
                FROM
                    @Tran01
            )
        order by
            transaction_dtm;

        ------------------------------------------------  
        -- For Excel Sheet (05): -- Create Worksheet 5(02a) Report on submission of COVID-19 under VSS from 2200 hours to 0800 hours of the following day - Summary
        -- ---------------------------------------------  

        (select
                @In_NumOfSPInvolved_OddHours = count(distinct sp_id),
                @In_NumOfPracticeInvolved_OddHours = count(distinct sp_id + ' ' + convert(varchar(3), Practice_Display_Seq)),
                @In_NumOfTransaction_OddHours = count(*)
            from
                @Tran01
            where
                (
				 convert(varchar, Transaction_Dtm, 8) >= '22:00'
                 OR convert(varchar, Transaction_Dtm, 8) < '08:00'
                ));

        INSERT INTO
            #WS05  
            (Col01, Col02, Col03)
        values
            (
                'Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111),
                null,
                null
            ),
			(null, null, null),
			(
                '(I)',
                'No.of service provider involved',
                @In_NumOfSPInvolved_OddHours
            ),
			(null, null, null),
			(
                '(II)',
                'No.of practices involved',
                @In_NumOfPracticeInvolved_OddHours
            ),
			(null, null, null),
			(
                '(III)',
                'No. of claim transactions involved',
                @In_NumOfTransaction_OddHours
            );

        ------------------------------------------------  
        -- For Excel Sheet (06): -- Create Worksheet 6(02b) Report on submission of COVID-19 under VSS from 2200 hours to 0800 hours of the following day - Raw Data
        -- ---------------------------------------------  
        INSERT INTO
            #WS06
            (Col01)
        values
            ('Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111)),
            ('')
        INSERT INTO
            #WS06  
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05
            )
        VALUES
            (
                'SPID',
                'Practice No.',
                'Transaction ID',
                'Transaction Time',
                'Means of Input'
            );

        INSERT INTO
            #WS06  
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05
            ) (
                select
                    SP_ID,
                    Practice_Display_Seq,
                    dbo.func_format_system_number(transaction_id),
                    CONVERT(VARCHAR, transaction_dtm, 20),
                    MeanOfInput
                from
                    @Tran01
                where
                    (
                     convert(varchar, Transaction_Dtm, 8) >= '22:00'
                     OR convert(varchar, Transaction_Dtm, 8) < '08:00'
                    )
            )
        order by
            transaction_dtm;

			------------------------------------------------  
            -- For Excel Sheet (07): -- Create Worksheet 7(03a) Report on COVID19 claims using Passport under VSS
            -- ---------------------------------------------  
        INSERT INTO
            #WS07
            (Col01)
        values
            ('Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111)),
            ('')
        INSERT INTO
            #WS07
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05,
                Col06,
                Col07,
                Col08,
                Col09
            )
        VALUES
            (
                'Transaction ID',
                'Transaction Time',
                'SPID',
                'Practice No.',
                'Service Date',
                'Dose',
                'DOB',
                'DOB Flag',
                'Gender'
            );

        INSERT INTO
            #WS07
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05,
                Col06,
                Col07,
                Col08,
                Col09
            ) (
                SELECT
                    dbo.func_format_system_number(vt.transaction_id),
                    CONVERT(VARCHAR, vt.transaction_dtm, 20),
                    VT.SP_ID,
                    VT.Practice_Display_Seq,
                    CONVERT(VARCHAR, VT.Service_Receive_Dtm, 20),
					sid.Available_Item_Desc,
                    CONVERT(VARCHAR,  isnull(tpi.DOB, VR.DOB), 20),
                    isnull(tpi.Exact_DOB, VR.Exact_DOB),
                    isnull(tpi.sex, VR.sex)
                FROM
                    @Tran01 VT
                    left outer join TransactionDetail TD with (nolock) on VT.Transaction_ID = TD.Transaction_ID
					left outer join SubsidizeItemDetails sid with (nolock) on TD.Subsidize_Item_Code = sid.Subsidize_Item_Code 
																				and td.Available_Item_Code = sid.Available_Item_Code
                    left outer join TempPersonalInformation TPI with (nolock) on VT.Temp_Voucher_Acc_ID = TPI.Voucher_Acc_ID 
																				and (VT.Voucher_Acc_ID ='' or  VT.Voucher_Acc_ID is null)
																				and vt.Doc_Code = TPI.Doc_Code
					left outer join PersonalInformation AS VR WITH(NOLOCK)  
										 ON VT.Voucher_Acc_ID = VR.Voucher_Acc_ID  
											AND VT.Doc_Code = VR.Doc_Code  
											AND VT.Voucher_Acc_ID IS NOT NULL  
											AND VT.Voucher_Acc_ID <> ''  
                WHERE VT.Doc_Code = 'PASS'
            )
        ORDER BY
            transaction_dtm 
			------------------------------------------------  
            -- For Excel Sheet (08): -- Create Worksheet 8(03b) Report on COVID19 claims using One Way Permit under VSS
            -- ---------------------------------------------  
        INSERT INTO
            #WS08
            (Col01)
        values
            (
                'Reporting period: ' + CONVERT(VARCHAR, @In_Period_From, 111) + ' to ' + CONVERT(VARCHAR,  DATEADD(dd, -1, @In_Period_To), 111)
            ),
            ('')
        INSERT INTO
            #WS08
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05,
                Col06,
                Col07,
                Col08,
                Col09
            )
        VALUES
            (
                'Transaction ID',
                'Transaction Time',
                'SPID',
                'Practice No.',
                'Service Date',
                'Dose',
                'DOB',
                'DOB Flag',
                'Gender'
            );

        INSERT INTO
            #WS08
            (
                Col01,
                Col02,
                Col03,
                Col04,
                Col05,
                Col06,
                Col07,
                Col08,
                Col09
            ) (
                SELECT
                    dbo.func_format_system_number(vt.transaction_id),
                    CONVERT(VARCHAR, vt.transaction_dtm, 20),
                    VT.SP_ID,
                    VT.Practice_Display_Seq,
                    CONVERT(VARCHAR, VT.Service_Receive_Dtm, 20),
					sid.Available_Item_Desc,
                    CONVERT(VARCHAR,  isnull(tpi.DOB, VR.DOB), 20),
                    isnull(tpi.Exact_DOB, VR.Exact_DOB),
                    isnull(tpi.sex, VR.sex)
                FROM
                    @Tran01 VT
                    left outer join TransactionDetail TD with (nolock) on VT.Transaction_ID = TD.Transaction_ID
					left outer join SubsidizeItemDetails sid with (nolock) on TD.Subsidize_Item_Code = sid.Subsidize_Item_Code 
																				and td.Available_Item_Code = sid.Available_Item_Code
                    left outer join TempPersonalInformation TPI with (nolock) on VT.Temp_Voucher_Acc_ID = TPI.Voucher_Acc_ID 
																				and (VT.Voucher_Acc_ID ='' or  VT.Voucher_Acc_ID is null)
																				and vt.Doc_Code = TPI.Doc_Code
					left outer join PersonalInformation AS VR WITH(NOLOCK)  
										 ON VT.Voucher_Acc_ID = VR.Voucher_Acc_ID  
											AND VT.Doc_Code = VR.Doc_Code  
											AND VT.Voucher_Acc_ID IS NOT NULL  
											AND VT.Voucher_Acc_ID <> ''  
					WHERE VT.Doc_Code = 'OW'
            ) 
        ORDER BY
            transaction_dtm
			
		-- -----------------------------------------  
        -- Excel worksheet (Remark)  
        -- -----------------------------------------  

		INSERT INTO #WS09 (Col01) values ('(A) Common Note(s) for the report')
		INSERT INTO #WS09 (Col01) values ('   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))')
		INSERT INTO #WS09 (Col01) values ('   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated.')
		INSERT INTO #WS09 (Col01) values ('   c. Exclude voided/deleted transactions.')

		-- =============================================  
        -- Return results  
        -- =============================================   

        SELECT
            'Report Generation Time: ' + CONVERT(VARCHAR(10), @In_Request_Time, 111) + ' ' + CONVERT(VARCHAR(5), @In_Request_Time, 114);

        --The first Sheet
        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, '')
        FROM
            #WS02 WITH(NOLOCK)  
        ORDER BY
            Seq;

        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, ''),
            ISNULL(Col06, ''),
            ISNULL(Col07, ''),
            ISNULL(Col08, '')
        FROM
            #WS03 WITH(NOLOCK)  
        ORDER BY
            Seq;

        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, ''),
            ISNULL(Col06, ''),
            ISNULL(Col07, '')
        FROM
            #WS04 WITH(NOLOCK)  
        ORDER BY
            Seq;

        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, '')
        FROM
            #WS05 WITH(NOLOCK)  
        ORDER BY
            Seq;

        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, ''),
            ISNULL(Col06, ''),
            ISNULL(Col07, '')
        FROM
            #WS06 WITH(NOLOCK)  
        ORDER BY
            Seq;

        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, ''),
            ISNULL(Col06, ''),
            ISNULL(Col07, ''),
            ISNULL(Col08, ''),
            ISNULL(Col09, ''),
            ISNULL(Col10, ''),
            ISNULL(Col11, ''),
            ISNULL(Col12, ''),
            ISNULL(Col13, ''),
            ISNULL(Col14, ''),
            ISNULL(Col15, ''),
            ISNULL(Col16, '')
        FROM
            #WS07 WITH(NOLOCK)  
        ORDER BY
            Seq;

        SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, ''),
            ISNULL(Col02, ''),
            ISNULL(Col03, ''),
            ISNULL(Col04, ''),
            ISNULL(Col05, ''),
            ISNULL(Col06, ''),
            ISNULL(Col07, ''),
            ISNULL(Col08, ''),
            ISNULL(Col09, ''),
            ISNULL(Col10, ''),
            ISNULL(Col11, ''),
            ISNULL(Col12, ''),
            ISNULL(Col13, ''),
            ISNULL(Col14, ''),
            ISNULL(Col15, ''),
            ISNULL(Col16, '')
        FROM
            #WS08 WITH(NOLOCK)  
        ORDER BY
            Seq;

		SELECT
            --ISNULL(_display_seq, ''),   
            ISNULL(Col01, '')
        FROM
            #WS09 WITH(NOLOCK)  
        ORDER BY
            Seq;


        IF OBJECT_ID('tempdb..#WS02') IS NOT NULL BEGIN DROP TABLE #WS02;
        END;

        IF OBJECT_ID('tempdb..#WS03') IS NOT NULL BEGIN DROP TABLE #WS03;
        END;

        IF OBJECT_ID('tempdb..#WS04') IS NOT NULL BEGIN DROP TABLE #WS04;
        END;

        IF OBJECT_ID('tempdb..#WS05') IS NOT NULL BEGIN DROP TABLE #WS05;
        END;

        IF OBJECT_ID('tempdb..#WS06') IS NOT NULL BEGIN DROP TABLE #WS06;
        END;

        IF OBJECT_ID('tempdb..#WS07') IS NOT NULL BEGIN DROP TABLE #WS07;
        END;

        IF OBJECT_ID('tempdb..#WS08') IS NOT NULL BEGIN DROP TABLE #WS08;
        END;

		IF OBJECT_ID('tempdb..#WS09') IS NOT NULL BEGIN DROP TABLE #WS09;
        END;


        END;

        GO
            GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0010_Report_get] TO HCVU;

        GO