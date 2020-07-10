IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0011_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0011_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.:			CRE19-021
-- Modified date:	24 Feb 2020
-- Description:		Change logic to consider the practice under the designated MO are working at the DHC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.:			CRD19-043
-- Modified date:		03 Feb 2020
-- Description:		Fix total of "(f) Number of Service Provders who had made voucher claim for DHC-related services"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.:			
-- Modified date:	09 Dec 2019
-- Description:		
--					1. Exclude dummy SP in [SPExceptionList]
--					2. 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.:			
-- Modified date:	27 Nov 2019
-- Description:		Add 2 data
--					1. (e) Number of Service Providers eligible to make voucher claim for DHC-related services 
--					2. (f) Number of Service Provders who had made voucher claim for DHC-related services
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- CR No.:			CRE19-006 (DHC)
-- Create date:		26 Aug 2019
-- Description:		New report eHS(S)M0011 - Statistical report of use of vouchers in DHC-related services
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSM0011_Report_get]
	@request_time datetime = NULL,
	@cutOffDtm datetime = NULL	-- The date to gen report
AS BEGIN

	SET NOCOUNT ON;
	
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

	CREATE TABLE #VoucherTransactionSPBasis (
		TxCnt					int,
		Scheme_Code				char(10),
		SP_ID					char(8),
		Practice_Display_Seq	smallint,
		service_type			char(5),
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
		Result_Value1 nvarchar(1000) DEFAULT '',	
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

	-- For Part (a)(i)
	DECLARE @ReasonForVisitSummary AS TABLE (
		Service_Type	CHAR(5),
		Reason_Code		INT,
		Tx_Count		INT
	)

	-- For Part (a)(ii) - (d) 
	DECLARE @TransactionSummary AS TABLE (
		Service_Type	CHAR(5),
		Tx_Count		INT,
		Claim_Amt		BIGINT,
		NoOfElder		INT
	)

	
	-- For Part (e) Number of Service Providers eligible to make voucher claim for DHC-related services 
	-- Eligible profession
	DECLARE @DHCRelatedServiceEligibleSummary AS TABLE (
		Service_Type	CHAR(5),
		SP_Count		INT
	)

	-- Ineligible profession
	DECLARE @HCVSDHCPracticeIneligibleSummary AS TABLE (
		Service_Type	CHAR(5),
		RegCode_Count	INT
	)

	-- For Part (f) Number of Service Provders who had made voucher claim for DHC-related services
	-- Eligible profession
	DECLARE @TransactionSummarySPBasis AS TABLE (
		Service_Type	CHAR(5),
		SP_Count		INT
	)

	-- Ineligible profession
	DECLARE @TransactionSummaryRegCodeBasis AS TABLE (
		Service_Type	CHAR(5),
		RegCode_Count	INT
	)

	--
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
	
	-- Only KT district is available now
	DECLARE @District AS VARCHAR(100) = 'Kwai Tsing'

	
	-- The designated MO are working at the DHC
	DECLARE @DHC_Core_MOList VARCHAR(100) 

	SELECT @DHC_Core_MOList = COALESCE(@DHC_Core_MOList + ', ', '') + LTRIM( CONVERT(VARCHAR(10), MO_Display_Seq))
	FROM DHCCoreMapping
	GROUP BY MO_Display_Seq
	ORDER BY MO_Display_Seq


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
			CASE WHEN D.MO_Display_Seq IS NOT NULL THEN 'DHC' ELSE VT.Service_Type END AS [Service_Type], --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Under the deaignated MO)
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
			LEFT JOIN Practice P
				ON VT.Practice_Display_Seq = P.Display_Seq AND VT.SP_ID = P.SP_ID
					AND VT.Scheme_Code = 'HCVSDHC'
			LEFT JOIN DHCCoreMapping D
				ON P.SP_ID = D.SP_ID AND P.MO_Display_Seq = D.MO_Display_Seq
					AND D.Record_Status = 'A'
				
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


-- ---------------------------------------------
-- Retrieve Voucher transactions - SP Basis
-- ---------------------------------------------
	INSERT INTO #VoucherTransactionSPBasis (
		TxCnt,
		Scheme_Code,
		SP_ID,
		Practice_Display_Seq,
		Service_Type,
		Record_Status,
		Invalidation
	)
		SELECT
		COUNT(Transaction_ID), 
		Scheme_Code, 
		SP_ID, 
		Practice_Display_Seq,
		Service_Type, 
		Record_Status, 
		Invalidation
	FROM (
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Scheme_Code,
			VT.SP_ID,
			VT.Practice_Display_Seq,
			CASE WHEN D.MO_Display_Seq IS NOT NULL THEN 'DHC' ELSE VT.Service_Type END AS [Service_Type], --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Under the deaignated MO)
			VT.Record_Status,
			VT.Invalidation
		FROM
			 VoucherTransaction  VT
			LEFT JOIN Practice P
				ON VT.Practice_Display_Seq = P.Display_Seq
					AND VT.SP_ID = P.SP_ID
					AND VT.Scheme_Code = 'HCVSDHC'
			LEFT JOIN DHCCoreMapping D
				ON P.SP_ID = D.SP_ID AND P.MO_Display_Seq = D.MO_Display_Seq
					AND D.Record_Status = 'A'
		WHERE
				(VT.Scheme_Code = 'HCVSDHC' OR ((VT.Scheme_Code = 'HCVS' AND ISNULL(VT.DHC_Service,'') = 'Y')))
				AND VT.Transaction_Dtm < @CutOffDtm
	) a
	GROUP BY 
		Scheme_Code, 
		SP_ID, 
		Practice_Display_Seq,
		Service_Type,
		Record_Status, 
		Invalidation

--------------------------------------------------------------
	DELETE #VoucherTransactionSPBasis WHERE Record_Status IN (
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
	
	DELETE #VoucherTransactionSPBasis WHERE (Invalidation IS NOT NULL AND Invalidation IN (
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

--------------------------------------------------------------------------------
-- Transaction Summary

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

--------------------------------------------------------------------------------
-- Transaction Summary - SP Basis (for HCVS only)
	INSERT INTO @TransactionSummarySPBasis (Service_Type, SP_Count)
	SELECT
		P.Service_Type,
		ISNULL(SPCnt, 0)
	FROM
		@Profession P
		LEFT JOIN
			(SELECT 
				service_type, COUNT(DISTINCT SP_ID) AS [SPCnt]
			FROM 
				#VoucherTransactionSPBasis
			WHERE 
				Scheme_Code = 'HCVS'
			GROUP BY 
				service_type) T 
			ON T.service_type = P.Service_Type


	-- Total
	INSERT INTO @TransactionSummarySPBasis (Service_Type, SP_Count)
	SELECT 'Total', SUM(SP_Count)
	FROM @TransactionSummarySPBasis

--------------------------------------------------------------------------------
-- Transaction Summary - RegCode Basis (for HCVSDHC only)
	INSERT INTO @TransactionSummaryRegCodeBasis (Service_Type, RegCode_Count)
	SELECT
		P.Service_Type,
		ISNULL(RegCodeCnt, 0)
	FROM
		@Profession P
		LEFT JOIN
		(
			SELECT service_type, COUNT(DISTINCT PROF.Registration_Code) AS [RegCodeCnt]
			FROM 
				#VoucherTransactionSPBasis  VT
			INNER JOIN
				Practice PT ON PT.SP_ID = VT.SP_ID AND PT.Display_Seq = VT.Practice_Display_Seq
			INNER JOIN
				Professional PROF ON PROF.SP_ID = PT.SP_ID AND PROF.Professional_Seq = PT.Professional_Seq
			WHERE 
				Scheme_Code = 'HCVSDHC'
			GROUP BY service_type	
		) AS T 
		ON T.service_type = P.Service_Type

	-- Total
	INSERT INTO @TransactionSummaryRegCodeBasis (Service_Type, RegCode_Count)
	SELECT 'Total', SUM(RegCode_Count)
	FROM @TransactionSummaryRegCodeBasis

--------------------------------------------------------------------------------
-- DHC related service eligible Summary 
	INSERT INTO @DHCRelatedServiceEligibleSummary (Service_Type, SP_Count)
	SELECT
		P.Service_Type,
		ISNULL(SPCnt, 0)
	FROM
		@Profession P
		LEFT JOIN
			(SELECT 
				P.Service_Category_Code, COUNT(DISTINCT P.SP_ID) AS [SPCnt]
			FROM 
				DHCSPMapping M
				INNER JOIN Professional P
				ON M.Service_Category_Code = P.Service_Category_Code
					AND M.Registration_Code = P.Registration_Code
					AND P.Record_Status = 'A'
			WHERE NOT EXISTS (SELECT SP_ID FROM SPExceptionList WITH (NOLOCK)
							WHERE P.SP_ID = SPExceptionList.SP_ID)
			GROUP BY 
				P.Service_Category_Code) T 
			ON T.Service_Category_Code = P.Service_Type


	-- Total
	INSERT INTO @DHCRelatedServiceEligibleSummary (Service_Type, SP_Count)
	SELECT 'Total',SUM(SP_Count)
	FROM 
		@DHCRelatedServiceEligibleSummary


------------------------------------------------------------------------------------------

-- HCVSDHC practice ineligible Summary
	INSERT INTO @HCVSDHCPracticeIneligibleSummary (Service_Type, RegCode_Count)
	SELECT 
		P.Service_Type,
		COUNT(T.Registration_Code)
	FROM
		@Profession P
		LEFT JOIN 
			(SELECT DISTINCT
				CASE WHEN D.MO_Display_Seq IS NOT NULL THEN 'DHC' ELSE P.Service_Category_Code END AS [Service_Type] --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Under the deaignated MO)
				, P.Registration_Code
			FROM 
				Practice PT
				INNER JOIN Professional P
				ON PT.SP_ID = P.SP_ID
					AND PT.Professional_Seq = P.Professional_Seq
					AND P.Record_Status = 'A'
				INNER JOIN PracticeSchemeInfo PSI
				ON PT.SP_ID = PSI.SP_ID
					AND PT.Display_Seq = PSI.Practice_Display_Seq
					AND PSI.Scheme_Code = 'HCVSDHC'
					AND PSI.Record_Status <> 'D'
				LEFT JOIN DHCCoreMapping D
					ON PT.SP_ID = D.SP_ID AND PT.MO_Display_Seq = D.MO_Display_Seq
						AND D.Record_Status = 'A'

			WHERE NOT EXISTS (SELECT SP_ID FROM SPExceptionList WITH (NOLOCK)
							WHERE P.SP_ID = SPExceptionList.SP_ID)
			) T 
		ON T.Service_Type = P.Service_Type
	GROUP BY P.Service_Type


	-- Total
	INSERT INTO @HCVSDHCPracticeIneligibleSummary (Service_Type, RegCode_Count)
	SELECT 'Total',SUM(RegCode_Count)
	FROM 
		@HCVSDHCPracticeIneligibleSummary


-----------------------------------------------------------------------------------------
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
-- Result 01
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
		'(d) Cumulative number of elders who have ever made use of vouchers for District Health Centre related services (DHC-related services)',
		'', '', '', '',
		'', '', '', '', '', '', '', '', '', '', '', 
		@Overall_NoOfElder

------------------------------------------------------------

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT	'(e) Number of Service Providers eligible to make voucher claim for DHC-related services '
			,[DHC],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT
			Service_Type, [Count]
		FROM
			(	SELECT Service_Type, SP_Count AS [Count]
				FROM @DHCRelatedServiceEligibleSummary
				UNION
				SELECT Service_Type, RegCode_Count AS [Count]
				FROM @HCVSDHCPracticeIneligibleSummary
			) TE
	) T
	pivot
	(	SUM([COUNT])
		for Service_Type in ([DHC],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
	SELECT	'(f) Number of Service Provders who had made voucher claim for DHC-related services'
			,[DHC],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(		
		SELECT 
			Service_Type, [Count]
		FROM
			(	SELECT	Service_Type, SP_Count AS [Count]
				FROM  @TransactionSummarySPBasis
				UNION
				SELECT	Service_Type, RegCode_Count AS [Count]
				FROM  @TransactionSummaryRegCodeBasis
			) TS
	) T
	pivot
	(	SUM([Count])
		for Service_Type in ([DHC],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT



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
	SELECT FORMATMESSAGE('      ii. For practice of claim transaction under the medical organization No ''%s'' are counted as "District Health Centre (Core + Satellite)"', @DHC_Core_MOList), ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   e. Under individual Enrolled Health Care Provider account:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      i. All HCVS claim transactions marked ''DHC-related service''', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	-- Term Definition

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(C) Term Definition', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1. Under District Health Centre corporate account:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      * The DHC operator is enrolled in HCVSDHC as a service provider and assigned an Enrolled Health Care Provider account under eHS(S)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      * DHC operator creates a practice for each non-eligible healthcare service providers to create voucher claims under HCVSDHC', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      * Practices are also divided into 2 types:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '         a. District Health Centre (Core + Satellite)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '            Practices for those non-eligible healthcare service providers who are employed by DHC as staff', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT FORMATMESSAGE('            Practices are marked with medical organization no. ''%s'' (designated for the DHC Centre)', @DHC_Core_MOList), ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '         b. DIT, POD, SPT', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '            Practices for those non-eligible healthcare providers not employed by DHC as staff and provide services under their own clinic', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT FORMATMESSAGE('            Practices are marked with medical organization no. other than %s', @DHC_Core_MOList), ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2. Under individual Enrolled Health Care Provider account', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      * Eligible healthcare service providers enrolled HCVS and joined DHC scheme', ''

	SELECT Result_Value1, Result_Value2
	FROM @tblRemark
	ORDER BY Seq

-- ---------------------------------------------
-- Drop the temporary tables
-- ---------------------------------------------
	DROP TABLE #vouchertransaction
	DROP TABLE #vouchertransactionSPBasis
	DROP TABLE #VT_ReasonForVisit
		
END
GO

GRANT EXECUTE ON proc_EHS_eHSM0011_Report_get TO HCVU
GO
