IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0011_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0011_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Winnie SUEN
-- CR No.:			CRE19-006 (DHC)
-- Create date:		26 Aug 2019
-- Description:		New report eHS(S)M0011 - Statistical report of use of vouchers in DHC-related services
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- CR No.:			
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSM0011_Report_get]
	@cutOffDtm datetime = NULL	-- The date to gen report
AS BEGIN

	SET NOCOUNT ON;
	
--	DECLARE @cutoffdtm as datetime
--	set @cutoffdtm='2019-08-27'
-- =============================================
-- Report setting
-- =============================================

	IF @cutOffDtm IS NULL BEGIN
		SET @cutOffDtm = CONVERT(varchar(11), GETDATE(), 106) + ' 00:00'
	END

	DECLARE @ReportDtm datetime
	SET @ReportDtm = DATEADD(day, -1, @cutOffDtm) -- The date report data as at
	

-- =============================================
-- Declaration
-- =============================================

	CREATE TABLE #VoucherTransaction (
		TxCnt					int,
		Scheme_Code				char(10),
		Identity_Num			varbinary(100),
		service_type			char(5),
		Claim_Amount			int,
		Reason_for_visit_L1		smallint default 0,
		Record_Status			char(1),
		Invalidation			char(1)
	)


	-- Seperate a transaction into Several reason
	CREATE TABLE #VT_ReasonForVisit (
		TxCnt					INT,
		Service_Type			CHAR(5),
		Reason_Code				INT
	)


	--Result Table
	DECLARE @ResultTable01 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 nvarchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT '',	
		Result_Value6 varchar(100) DEFAULT '',	
		Result_Value7 varchar(100) DEFAULT '',	
		Result_Value8 varchar(100) DEFAULT '',	
		Result_Value9 varchar(100) DEFAULT '',	
		Result_Value10 varchar(100) DEFAULT '',
		Result_Value11 varchar(100) DEFAULT '',
		Result_Value12 varchar(100) DEFAULT '',
		Result_Value13 varchar(100) DEFAULT '',
		Result_Value14 varchar(100) DEFAULT '',
		Result_Value15 varchar(100) DEFAULT '',
		Result_Value16 varchar(100) DEFAULT '',
		Result_Value17 varchar(100) DEFAULT ''
	)

	DECLARE @ReasonForVisitSummary AS TABLE (
		Service_Type	CHAR(5),
		Reason_Code		INT,
		Tx_Count		INT
	)

	DECLARE @TransactionSummary AS TABLE (
		Service_Type	CHAR(5),
		Tx_Count		INT,
		Claim_Amt		BIGINT,
		NoOfElder		INT
	)

	DECLARE @TempReasonForVisitResult AS TABLE (
		Reason_Code		INT,
		Reason_Desc		VARCHAR(50),
		DHC				INT, 
		DIT				INT,
		SPT				INT,
		POD				INT,
		RMP				INT,
		RCM				INT,
		RDT				INT,
		ROT				INT,
		RPT				INT,
		RMT				INT,
		RRD				INT,
		ENU				INT,
		RNU				INT,
		RCP				INT,
		ROP				INT,
		Total			INT
	)


	DECLARE @Profession AS TABLE (
		Service_Type			char(5)
	)

	DECLARE @Reason AS TABLE
	(
		Reason_Code		INT,
		Reason_Desc		VARCHAR(50)
	)

	DECLARE @DHCBankAccount AS TABLE
	(
		Bank_Account_No	VARCHAR(30)
	)

	DECLARE @DHC_Bank_Account_No AS VARCHAR(1000)

-- =============================================
-- Initialization
-- =============================================

	-- Profession
	INSERT INTO @Profession
	SELECT Service_Category_Code FROM profession
	UNION
	SELECT 'DHC'


	-- Reason for Visit
	INSERT INTO @Reason(Reason_Code, Reason_Desc)
	SELECT DISTINCT Reason_L1_Code, Reason_L1 FROM ReasonForVisitL1
	UNION
	SELECT 5, 'Defer Input'


	SELECT @DHC_Bank_Account_No = Parm_Value1 FROM SystemParameters where Parameter_Name = 'DHC_BankAccountNo'

	INSERT INTO @DHCBankAccount (Bank_Account_No)
	SELECT ITEM FROM func_Split_string(@DHC_Bank_Account_No,'|||')

	-- Only KT district is available now
	DECLARE @District AS VARCHAR(100) = 'Kwai Tsing'

-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Retrieve Voucher transactions
-- ---------------------------------------------
	INSERT INTO #VoucherTransaction (
		TxCnt,
		Scheme_Code,
		identity_num,
		Service_Type,
		Claim_Amount,
		Reason_for_visit_L1,
		Record_Status,
		Invalidation
	)
		SELECT
		COUNT(Transaction_ID), 
		Scheme_Code, 
		identity_num, 
		Service_Type, 
		SUM(Claim_Amount),  
		Reason_for_Visit_L1,
		Record_Status, 
		Invalidation
	FROM (
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Scheme_Code,
			ISNULL(VT.Voucher_Acc_ID, '') AS [Voucher_Acc_ID] ,
			ISNULL(VT.Temp_Voucher_Acc_ID, '') AS [Temp_Voucher_Acc_ID],
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN TP.Encrypt_Field1 ELSE VP.Encrypt_Field1 END AS [Identity_Num],
			CASE WHEN BA.Bank_Account_No IS NULL THEN VT.Service_Type ELSE 'DHC' END AS [Service_Type],
			VT.Claim_Amount,
			CONVERT(smallint, ISNULL(TAF1.AdditionalFieldValueCode, 5)) AS Reason_for_Visit_L1,
			VT.Record_Status,
			VT.Invalidation
		FROM
			 VoucherTransaction  VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
			LEFT JOIN TransactionAdditionalField TAF1
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			LEFT JOIN PersonalInformation VP
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
			LEFT JOIN TempPersonalInformation TP
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
			LEFT JOIN @DHCBankAccount BA
				ON VT.Scheme_Code = 'HCVSDHC' AND VT.Bank_Account_No = BA.Bank_Account_No  --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Using the DHC Bank account)

		WHERE
				(VT.Scheme_Code = 'HCVSDHC' OR (VT.Scheme_Code = 'HCVS' AND ISNULL(VT.DHC_Service,'') = 'Y'))
				AND VT.Transaction_Dtm < @CutOffDtm
	) a
	GROUP BY 
		Scheme_Code, 
		identity_num, 
		Service_Type,
		Reason_for_Visit_L1,
		Record_Status, 
		Invalidation

--------------------------------------------------------------
	DELETE #VoucherTransaction WHERE Record_Status IN (
		SELECT
			Status_Value
		FROM
			StatStatusFilterMapping
		WHERE
			(report_id = 'ALL' OR report_id = 'eHSM0011') 
				AND Table_Name = 'VoucherTransaction'
				AND Status_Name = 'Record_Status' 
				AND ((Effective_Date IS NULL OR Effective_Date <= @CutOffDtm) AND (Expiry_Date IS NULL OR @CutOffDtm < Expiry_Date))
		)


----------------------------------------------------------------
	
	DELETE #VoucherTransaction WHERE (Invalidation IS NOT NULL AND Invalidation IN (
		SELECT
			Status_Value
		FROM
			StatStatusFilterMapping
		WHERE
			(report_id = 'ALL' OR report_id = 'eHSM0011') 
				AND Table_Name = 'VoucherTransaction'
				AND Status_Name = 'Invalidation'
				AND ((Effective_Date IS NULL OR Effective_Date <= @CutOffDtm) AND (Expiry_Date IS NULL OR @CutOffDtm < Expiry_Date)))
			)

-- =============================================
-- Process data for statistics
-- =============================================
--------------------------------------------------------------
-- Reason for Visit (Level 1) | District Health Centre (Core + Satellite) | DIT | POD | SPT | ENU | RCM | RCP | RDT | RMP | RMT | RNU | ROP | ROT | RPT | RRD | Total --
------------------------------------------------------------

-- Prepare data 

	INSERT INTO #VT_ReasonForVisit (TxCnt, service_type, Reason_Code)
	-- Primary
	SELECT 
		SUM(TxCnt) AS cnt, service_type, Reason_for_visit_L1
	FROM 
		#VoucherTransaction 
	GROUP BY 
		service_type, Reason_for_visit_L1


	INSERT INTO @TransactionSummary (Service_Type, Tx_Count, Claim_Amt, NoOfElder)
	SELECT
		P.Service_Type,
		ISNULL(TxCnt, 0),
		ISNULL(Claim_Amount, 0),
		ISNULL(NoOfElder, 0)
	FROM
		@Profession P
		LEFT JOIN
			(SELECT 
				service_type, SUM(TxCnt) AS [TxCnt], SUM(CONVERT(BIGINT, Claim_Amount)) AS [Claim_Amount], COUNT(DISTINCT Identity_num) AS [NoOfElder]
			FROM 
				#VoucherTransaction
			GROUP BY 
				service_type) T 
			ON T.service_type = P.Service_Type


	-- Total
	INSERT INTO @TransactionSummary (Service_Type, Tx_Count, Claim_Amt, NoOfElder)
	SELECT 'Total', SUM(Tx_Count), SUM(Claim_Amt), SUM(NoOfElder)
	FROM @TransactionSummary


------------------------------------------------------------------------------------------
-- Get Data

	INSERT INTO @ReasonForVisitSummary (Service_Type, Reason_Code, Tx_Count)
	SELECT P.Service_Type, R.Reason_Code, ISNULL(VT.TxCnt,0)
	FROM @Profession P
	CROSS JOIN @Reason R
	LEFT JOIN #VT_ReasonForVisit VT 
		ON VT.Reason_Code = R.reason_code AND VT.Service_Type = P.Service_Type


	-- Total
	INSERT INTO @ReasonForVisitSummary (Service_Type, Reason_Code, Tx_Count)
	SELECT 'Total', Reason_Code, SUM(Tx_Count)
	FROM @ReasonForVisitSummary
	GROUP BY Reason_Code

------------------------------------------------------------
-- Result
------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('District: ' + @District)
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))
	
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	VALUES ('', 
			'Under District Health Centre corporate account', '', '', '', 
			'Under individual Enrolled Health Care Provider account', '', '', '', '', '', '', '', '', '', '', 
			'Grand Total'
			)
			
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	VALUES ('','District Health Centre (Core + Satellite)','DIT','POD','SPT','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','Total')

------------------------------------------------------------

	INSERT INTO @ResultTable01 (Result_Value1) VALUES 
	('(a)(i) Cumulative number of voucher claim transactions by principal reason for visit (Level 1):')

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT	[Reason_Desc], 
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DIT] AS VARCHAR(100)) END,			
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([POD] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([SPT] AS VARCHAR(100)) END,						
			[ENU],[RCM],[RCP],[RDT],[RMP],CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
			[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(SELECT		S.Reason_Code, 
				R.Reason_Desc,
				S.Service_Type,						
				ISNULL(S.Tx_Count, 0) AS [Count]
		FROM
			@ReasonForVisitSummary S
			LEFT JOIN @Reason R ON S.Reason_Code = R.Reason_Code
	) SR
	pivot
	(	MAX([Count])
		for Service_Type in ([DHC],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT
	ORDER BY Reason_Code


------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT	'(a)(ii) Cumulative number of voucher claim transactions in total'
			,[DHC],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, Tx_Count
		FROM @TransactionSummary
	) T
	pivot
	(	MAX(Tx_Count)
		for Service_Type in ([DHC],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT	'(b) Cumulative amount of vouchers claimed ($)'
			,[DHC],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, Claim_Amt
		FROM @TransactionSummary
	) T
	pivot
	(	MAX(Claim_Amt)
		for Service_Type in ([DHC],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT	'(c) Cumulative number of elders who have made use of vouchers for each service'
			,[DHC],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, NoOfElder
		FROM  @TransactionSummary
	) T
	pivot
	(	MAX(NoOfElder)
		for Service_Type in ([DHC],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	DECLARE @Overall_NoOfElder INT
	SELECT 	@Overall_NoOfElder = COUNT(DISTINCT Identity_num) FROM #VoucherTransaction

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT 
		'(d) Cumulative number of elders who have ever made use of vouchers for District Health Centre related services ',
		'', '', '', '',
		'', '', '', '', '', '', '', '', '', '', '', 
		@Overall_NoOfElder

------------------------------------------------------------

-- =============================================  
-- Return results  
-- =============================================  
	Declare @strGenDtm varchar(50)    
	SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
	SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)    
	SELECT 'Report Generation Time: ' + @strGenDtm  


-- --------------------------------------------------    
-- To Excel sheet: eHSM0011-01: Report of use of vouchers for District Health Centre related services
-- --------------------------------------------------    
	SELECT	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
		Result_Value16, Result_Value17
	FROM	
		@ResultTable01
	ORDER BY	
		Result_Seq


-- --------------------------------------------------  
-- To Excel sheet:   eHSM0011-Remarks: Remarks  
-- --------------------------------------------------  
	DECLARE @tblRemark AS TABLE (
		Seq	INT identity(1,1),
		Result_Value1 NVARCHAR(MAX),    
		Result_Value2 NVARCHAR(MAX)  
	)

-- Lengend

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(A) Legend', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1.Profession Type Legend', ''


	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT Service_Category_Code, Service_Category_Desc 
	FROM Profession
	ORDER BY Service_Category_Code


	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''


	-- Common Note

	-- eHealth Accounts

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(B) Common Note(s) for the report', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1. eHealth (Subsidies) Accounts:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   a. Number of elders = Count unique HKID',''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	-- Transactions

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2. Transactions:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   c. Exclude voided/deleted transactions', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   d. Under District Health Centre corporate account:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      i. All HCVSDHC claim transactions created under service providers', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT FORMATMESSAGE('      ii. For practice of claim transaction using Bank Account No. (%s) that are counted as "District Health Centre (Core + Satellite)"', REPLACE(@DHC_Bank_Account_No,'|||',',')), ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   e. Under individual Enrolled Health Care Provider account:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      i. All HCVS claim transactions marked ''DHC-related service''', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''


	SELECT Result_Value1, Result_Value2
	FROM @tblRemark
	ORDER BY Seq

-- ---------------------------------------------
-- Drop the temporary tables
-- ---------------------------------------------
	DROP TABLE #vouchertransaction
	DROP TABLE #VT_ReasonForVisit
		
END
GO

GRANT EXECUTE ON proc_EHS_eHSM0011_Report_get TO HCVU
GO


