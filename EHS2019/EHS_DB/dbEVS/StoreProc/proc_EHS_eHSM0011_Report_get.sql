IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0011_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0011_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Nichole IP
-- CR No.:			CRE20-006
-- Modified date:	24 May 2021
-- Description:		
--     1) Add sub report for use of vouchers for District Health Centre related services (All) 
--     2) Add sub report for use of vouchers for District Health Centre related services (Sham Shui Po)
--     3) District Health Centre (Core + Satellite) separate into DIT, POD & SPT
--     4) Change the remark wording
-- =============================================
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
	
	declare @debug as char(1) = 'N'
-- =============================================
-- Declaration
-- =============================================
	CREATE TABLE #VoucherTransaction (
		TxCnt					int,
		Scheme_Code				char(10),
		Identity_Num			varbinary(100),
		service_type			char(10),
		Claim_Amount			int,
		Reason_for_visit_L1		smallint default 0,
		Record_Status			char(1),
		Invalidation			char(1),
		DistrictCode			varchar(50)
	)

	CREATE TABLE #VoucherTransactionSPBasis (
		TxCnt					int,
		Scheme_Code				char(10),
		SP_ID					char(8),
		Practice_Display_Seq	smallint,
		service_type			char(10),
		Record_Status			char(1),
		Invalidation			char(1),
		DistrictCode			varchar(50)
	)

	-- Seperate a transaction into Several reason
	CREATE TABLE #VT_ReasonForVisit (
		TxCnt					INT,
		Service_Type			CHAR(10),
		Reason_Code				INT,
		DistrictCode			varchar(50)
	)

	-- Temporary District
    DECLARE @tempDistrict AS TABLE(District_Code CHAR(3));


	--Result Table
	--Total
	DECLARE @ResultTable00 AS TABLE (
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
		Result_Value17 varchar(100) DEFAULT '',
		Result_Value18 varchar(100) DEFAULT '',
		Result_Value19 varchar(100) DEFAULT ''
	)

	-- Kwai Tsing
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
		Result_Value17 varchar(100) DEFAULT '',
		Result_Value18 varchar(100) DEFAULT '',
		Result_Value19 varchar(100) DEFAULT ''
	)

	-- Sham Shui Po
	DECLARE @ResultTable02 AS TABLE (
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
		Result_Value17 varchar(100) DEFAULT '',
		Result_Value18 varchar(100) DEFAULT '',
		Result_Value19 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable03 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 nvarchar(1000) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 nvarchar(200) DEFAULT '',
		Result_Value5 nvarchar(200) DEFAULT ''
	)

	-- For Part (a)(i)
	DECLARE @ReasonForVisitSummary AS TABLE (
		Service_Type	CHAR(10),
		Reason_Code		INT,
		Tx_Count		INT,
		DistrictCode			varchar(50)
	)

	-- For Part (a)(ii) - (d) 
	DECLARE @TransactionSummary AS TABLE (
		Service_Type	CHAR(10),
		Tx_Count		INT,
		Claim_Amt		BIGINT,
		NoOfElder		INT,
		DistrictCode			varchar(50)
	)

	
	-- For Part (e) Number of Service Providers eligible to make voucher claim for DHC-related services 
	-- Eligible profession
	DECLARE @DHCRelatedServiceEligibleSummary AS TABLE (
		Service_Type	CHAR(10),
		SP_Count		INT,
		DistrictCode			varchar(50)
	)

	-- Ineligible profession
	DECLARE @HCVSDHCPracticeIneligibleSummary AS TABLE (
		Service_Type	CHAR(10),
		RegCode_Count	INT,
		DistrictCode			varchar(50)
	)

	-- For Part (f) Number of Service Provders who had made voucher claim for DHC-related services
	-- Eligible profession
	DECLARE @TransactionSummarySPBasis AS TABLE (
		Service_Type	CHAR(10),
		SP_Count		INT,
		DistrictCode			varchar(50)
	)

	-- Ineligible profession
	DECLARE @TransactionSummaryRegCodeBasis AS TABLE (
		Service_Type	CHAR(10),
		RegCode_Count	INT,
		DistrictCode			varchar(50)
	)

	--
	--DECLARE @TempReasonForVisitResult AS TABLE (
	--	Reason_Code		INT,
	--	Reason_Desc		VARCHAR(50),
	--	DHC				INT, 
	--	DIT				INT,
	--	SPT				INT,
	--	POD				INT,
	--	RMP				INT,
	--	RCM				INT,
	--	RDT				INT,
	--	ROT				INT,
	--	RPT				INT,
	--	RMT				INT,
	--	RRD				INT,
	--	ENU				INT,
	--	RNU				INT,
	--	RCP				INT,
	--	ROP				INT,
	--	Total			INT,
	--	DistrictCode			varchar(50)
	--)


	DECLARE @Profession AS TABLE (
		Service_Type			char(10)
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
	UNION ALL
	SELECT 'DHC-DIT'
	UNION ALL
	SELECT 'DHC-POD'
	UNION ALL
	SELECT 'DHC-SPT'


	-- Reason for Visit
	INSERT INTO @Reason(Reason_Code, Reason_Desc)
	SELECT DISTINCT Reason_L1_Code, Reason_L1 FROM ReasonForVisitL1
	UNION
	SELECT 5, 'Defer Input'
	
	-- Only Kwai Tsing and sham shui po district is available now
	DECLARE @District_Name1 AS VARCHAR(100) = 'Kwai Tsing'
	DECLARE @District_Name2 AS VARCHAR(100) = 'Sham Shui Po'

	DECLARE @District_Code1 AS VARCHAR(100) = 'KC'
	DECLARE @District_Code2 AS VARCHAR(100) = 'SSP'
	
	INSERT INTO @tempDistrict values (@District_Code1)
	INSERT INTO @tempDistrict values (@District_Code2)

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
		Invalidation,
		DistrictCode 
	)
		SELECT
		COUNT(Transaction_ID), 
		Scheme_Code, 
		identity_num, 
		Service_Type, 
		SUM(Claim_Amount),  
		Reason_for_Visit_L1,
		Record_Status, 
		Invalidation,
		DistrictCode
	FROM (
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Scheme_Code,
			ISNULL(VT.Voucher_Acc_ID, '') AS [Voucher_Acc_ID] ,
			ISNULL(VT.Temp_Voucher_Acc_ID, '') AS [Temp_Voucher_Acc_ID],
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN TP.Encrypt_Field1 ELSE VP.Encrypt_Field1 END AS [Identity_Num],
			CASE WHEN D.MO_Display_Seq IS NOT NULL THEN 'DHC-' + VT.Service_Type ELSE VT.Service_Type END AS [Service_Type], --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Under the deaignated MO)
			VT.Claim_Amount,
			CONVERT(smallint, ISNULL(TAF1.AdditionalFieldValueCode, 5)) AS Reason_for_Visit_L1,
			VT.Record_Status,
			VT.Invalidation, --TAF.AdditionalFieldValueCode as DistrictCode
			--CASE WHEN ISNULL(TAF.AdditionalFieldValueCode,'') ='' then DB.district_board_shortname_SD   else TAF.AdditionalFieldValueCode end as DistrictCode
			DistrictCode= CASE WHEN  ISNULL(TAF.AdditionalFieldValueCode,'') <> '' THEN 
			   TAF.AdditionalFieldValueCode 
			ELSE 
				CASE WHEN db.district_board IS NULL THEN
					(SELECT TOP 1 DBD.DHC_District_Code FROM DHCCoreMapping DM INNER JOIN DistrictBoard DBD ON DBD.district_board_SD = DM.District_Board and DM.SP_ID = VT.SP_ID ORDER BY DM.MO_Display_seq)
				ELSE
					DB.DHC_District_Code 
				END
			END
		FROM
			 VoucherTransaction  VT  WITH (NOLOCK)
			INNER JOIN TransactionDetail TD  WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			-- new add  --
			LEFT JOIN TransactionAdditionalField TAF WITH (NOLOCK)
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'DHCDistrictCode'
			-- new add  --
			LEFT JOIN TransactionAdditionalField TAF1  WITH (NOLOCK)
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			LEFT JOIN PersonalInformation VP  WITH (NOLOCK)
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID AND VT.Doc_Code =VP.Doc_Code 
			LEFT JOIN TempPersonalInformation TP  WITH (NOLOCK)
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID AND VT.Doc_Code =TP.Doc_Code 
			LEFT JOIN Practice P  WITH (NOLOCK)
				ON VT.Practice_Display_Seq = P.Display_Seq AND VT.SP_ID = P.SP_ID
					AND VT.Scheme_Code = 'HCVSDHC'
			LEFT JOIN DHCCoreMapping D  WITH (NOLOCK)
				ON P.SP_ID = D.SP_ID AND P.MO_Display_Seq = D.MO_Display_Seq
					AND D.Record_Status = 'A'
			-- new add --
			LEFT JOIN DistrictBoard DB  WITH (NOLOCK) on DB.district_board_SD = D.District_Board 
			-- new add --
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
		Invalidation,
		DistrictCode

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

	IF @debug='Y' 
	BEGIN 
		SELECT '#VoucherTransaction'
		SELECT * FROM #VoucherTransaction

	END;
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
		Invalidation,
		DistrictCode
	)
		SELECT
		COUNT(Transaction_ID), 
		Scheme_Code, 
		SP_ID, 
		Practice_Display_Seq,
		Service_Type, 
		Record_Status, 
		Invalidation,
		DistrictCode
	FROM (
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Scheme_Code,
			VT.SP_ID,
			VT.Practice_Display_Seq,
			CASE WHEN D.MO_Display_Seq IS NOT NULL THEN 'DHC-' + VT.Service_Type ELSE VT.Service_Type END AS [Service_Type], --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Under the deaignated MO)
			VT.Record_Status,
			VT.Invalidation, --TAF.AdditionalFieldValueCode as DistrictCode
			DistrictCode= CASE WHEN  ISNULL(TAF.AdditionalFieldValueCode,'') <> '' THEN 
			   TAF.AdditionalFieldValueCode 
			ELSE 
				CASE WHEN db.district_board IS NULL THEN
					(SELECT TOP 1 DBD.DHC_District_Code FROM DHCCoreMapping DM INNER JOIN DistrictBoard DBD ON DBD.district_board_SD = DM.District_Board and DM.SP_ID = VT.SP_ID  ORDER BY DM.MO_Display_seq)
				ELSE
					db.DHC_District_Code
				END
			END
		FROM
			 VoucherTransaction  VT  WITH (NOLOCK)
			 	-- new add  --
			LEFT JOIN TransactionAdditionalField TAF  WITH (NOLOCK)
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'DHCDistrictCode'
			-- new add  --
			LEFT JOIN Practice P  WITH (NOLOCK)
				ON VT.Practice_Display_Seq = P.Display_Seq
					AND VT.SP_ID = P.SP_ID
					AND VT.Scheme_Code = 'HCVSDHC'
			LEFT JOIN DHCCoreMapping D  WITH (NOLOCK)
				ON P.SP_ID = D.SP_ID AND P.MO_Display_Seq = D.MO_Display_Seq
					AND D.Record_Status = 'A'
			-- new add --
			LEFT JOIN DistrictBoard DB  WITH (NOLOCK) on DB.district_board_SD = D.District_Board 
			-- new add --
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
		Invalidation, DistrictCode

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

	IF @debug='Y' 
	BEGIN 
		SELECT '##VoucherTransactionSPBasis'
		SELECT * FROM #VoucherTransactionSPBasis

	END;
-- =============================================
-- Process data for statistics
-- =============================================
--------------------------------------------------------------
-- Reason for Visit (Level 1) | District Health Centre (Core + Satellite) DHC-DIT | DHC-POD | DHC-SPT | DIT | POD | SPT | ENU | RCM | RCP | RDT | RMP | RMT | RNU | ROP | ROT | RPT | RRD | Total --
------------------------------------------------------------

-- Prepare data 

	INSERT INTO #VT_ReasonForVisit (TxCnt, service_type, Reason_Code,DistrictCode)
	-- Primary
	SELECT 
		SUM(TxCnt) AS cnt, service_type, Reason_for_visit_L1, DistrictCode 
	FROM 
		#VoucherTransaction 
	GROUP BY 
		service_type, Reason_for_visit_L1, DistrictCode 

	IF @debug='Y' 
	BEGIN 
		SELECT '#VT_ReasonForVisit'
		SELECT * FROM #VT_ReasonForVisit

	END;
--------------------------------------------------------------------------------
-- Transaction Summary

	INSERT INTO @TransactionSummary (Service_Type, Tx_Count, Claim_Amt, NoOfElder,DistrictCode)
	SELECT
		P.Service_Type,
		ISNULL(TxCnt, 0),
		ISNULL(Claim_Amount, 0),
		ISNULL(NoOfElder, 0),
		TD.District_Code  
	FROM
		@Profession P CROSS JOIN @tempDistrict TD
		LEFT JOIN
			(SELECT 
				service_type, SUM(TxCnt) AS [TxCnt], SUM(CONVERT(BIGINT, Claim_Amount)) AS [Claim_Amount], COUNT(DISTINCT Identity_num) AS [NoOfElder],DistrictCode 
			FROM 
				#VoucherTransaction
			GROUP BY 
				service_type,DistrictCode ) T 
			ON T.service_type = P.Service_Type
			AND TD.District_Code = T.DistrictCode 

	-- Total
	INSERT INTO @TransactionSummary (Service_Type, Tx_Count, Claim_Amt, NoOfElder,DistrictCode)
	SELECT 'Total', SUM(Tx_Count), SUM(Claim_Amt), SUM(NoOfElder),DistrictCode
	FROM @TransactionSummary
	Group by DistrictCode --new add

	IF @debug='Y' 
	BEGIN 
		SELECT '#@TransactionSummary'
		SELECT * FROM @TransactionSummary

	END;
--------------------------------------------------------------------------------
-- Transaction Summary - SP Basis (for HCVS only)
	INSERT INTO @TransactionSummarySPBasis (Service_Type, SP_Count,DistrictCode )
	SELECT
		P.Service_Type,
		ISNULL(SPCnt, 0),
		TD.District_Code 
	FROM
		@Profession P CROSS JOIN @tempDistrict TD
		LEFT JOIN
			(SELECT 
				service_type, COUNT(DISTINCT SP_ID) AS [SPCnt],DistrictCode
			FROM 
				#VoucherTransactionSPBasis
			WHERE 
				Scheme_Code = 'HCVS'  
			GROUP BY 
				service_type,DistrictCode ) T 
			ON T.service_type = P.Service_Type
			AND TD.District_Code = T.DistrictCode 
	 

	-- Total
	INSERT INTO @TransactionSummarySPBasis (Service_Type, SP_Count,DistrictCode)
	SELECT 'Total', SUM(SP_Count),DistrictCode
	FROM @TransactionSummarySPBasis
	group by DistrictCode -- new add

	IF @debug='Y' 
	BEGIN 
		SELECT '@TransactionSummarySPBasis'
		SELECT * FROM @TransactionSummarySPBasis

	END;
--------------------------------------------------------------------------------
-- Transaction Summary - RegCode Basis (for HCVSDHC only)
	INSERT INTO @TransactionSummaryRegCodeBasis (Service_Type, RegCode_Count,DistrictCode)
	SELECT
		P.Service_Type,
		ISNULL(RegCodeCnt, 0),TD.District_Code 
	FROM
		@Profession P  CROSS JOIN @tempDistrict TD
		LEFT JOIN
		(
			SELECT service_type, COUNT(DISTINCT PROF.Registration_Code) AS [RegCodeCnt],DistrictCode
			FROM 
				#VoucherTransactionSPBasis  VT
			INNER JOIN
				Practice PT  WITH (NOLOCK) ON PT.SP_ID = VT.SP_ID AND PT.Display_Seq = VT.Practice_Display_Seq
			INNER JOIN
				Professional PROF  WITH (NOLOCK) ON PROF.SP_ID = PT.SP_ID AND PROF.Professional_Seq = PT.Professional_Seq
			WHERE 
				Scheme_Code = 'HCVSDHC'  
			GROUP BY service_type,DistrictCode
		) AS T
		ON T.service_type = P.Service_Type
		AND TD.District_Code = T.DistrictCode 
	 
	-- Total
	INSERT INTO @TransactionSummaryRegCodeBasis (Service_Type, RegCode_Count,DistrictCode)
	SELECT 'Total', SUM(RegCode_Count),DistrictCode
	FROM @TransactionSummaryRegCodeBasis
	GROUP by DistrictCode --new add


	IF @debug='Y' 
	BEGIN 
		SELECT '@@TransactionSummaryRegCodeBasis'
		SELECT * FROM @TransactionSummaryRegCodeBasis

	END;
--------------------------------------------------------------------------------
-- DHC related service eligible Summary 
	INSERT INTO @DHCRelatedServiceEligibleSummary (Service_Type, SP_Count,DistrictCode)
	SELECT
		P.Service_Type,
		ISNULL(SPCnt, 0), TD.District_Code 
	FROM
		@Profession P  CROSS JOIN @tempDistrict TD
		LEFT JOIN
			(SELECT 
				P.Service_Category_Code, COUNT(DISTINCT P.SP_ID) AS [SPCnt],District_Code 
			FROM 
				DHCSPMapping M  WITH (NOLOCK)
				INNER JOIN Professional P  WITH (NOLOCK)
				ON M.Service_Category_Code = P.Service_Category_Code
					AND M.Registration_Code = P.Registration_Code
					AND P.Record_Status = 'A'
			WHERE NOT EXISTS (SELECT SP_ID FROM SPExceptionList WITH (NOLOCK)
							WHERE P.SP_ID = SPExceptionList.SP_ID)
							 
			GROUP BY 
				P.Service_Category_Code,M.District_Code ) T 
			ON T.Service_Category_Code = P.Service_Type
			--AND TD.District_Code = CASE WHEN T.District_Code ='KC' THEN 'KTS' ELSE T.District_Code END
			AND TD.District_Code =   T.District_Code  

	-- Total
	INSERT INTO @DHCRelatedServiceEligibleSummary (Service_Type, SP_Count,DistrictCode)
	SELECT 'Total',SUM(SP_Count),DistrictCode
	FROM 
		@DHCRelatedServiceEligibleSummary
		GROUP BY DistrictCode -- new add

	IF @debug='Y' 
	BEGIN 
		SELECT '@DHCRelatedServiceEligibleSummary'
		SELECT * FROM @DHCRelatedServiceEligibleSummary

	END;
------------------------------------------------------------------------------------------

-- HCVSDHC practice ineligible Summary
	INSERT INTO @HCVSDHCPracticeIneligibleSummary (Service_Type, RegCode_Count,DistrictCode)
	SELECT 
		P.Service_Type,
		COUNT(T.Registration_Code),TD.District_Code 
	FROM
		@Profession P  CROSS JOIN @tempDistrict TD
		LEFT JOIN 
			(SELECT DISTINCT
				CASE WHEN D.MO_Display_Seq IS NOT NULL THEN 'DHC-' +  P.Service_Category_Code ELSE P.Service_Category_Code END AS [Service_Type] --DHC (Core + Satellite) = HCVSDHC Scheme + Working at DHC Centre (Under the deaignated MO)
				, P.Registration_Code,
				CASE WHEN DB.district_board_shortname_SD IS NOT NULL THEN
					DB.DHC_District_Code 
				ELSE
				(SELECT TOP 1 DBD.DHC_District_Code  FROM DHCCoreMapping DM INNER JOIN DistrictBoard DBD ON DBD.district_board_SD = DM.District_Board and DM.SP_ID = PT.SP_ID )
				END AS District_Board
			FROM 
				Practice PT  WITH (NOLOCK)
				INNER JOIN Professional P  WITH (NOLOCK)
				ON PT.SP_ID = P.SP_ID
					AND PT.Professional_Seq = P.Professional_Seq
					AND P.Record_Status = 'A'
				INNER JOIN PracticeSchemeInfo PSI  WITH (NOLOCK)
				ON PT.SP_ID = PSI.SP_ID
					AND PT.Display_Seq = PSI.Practice_Display_Seq
					AND PSI.Scheme_Code = 'HCVSDHC'
					AND PSI.Record_Status <> 'D'
				LEFT JOIN DHCCoreMapping D  WITH (NOLOCK)
					ON PT.SP_ID = D.SP_ID AND PT.MO_Display_Seq = D.MO_Display_Seq /****/
						AND D.Record_Status = 'A'
				-- new add --
				LEFT JOIN DistrictBoard DB  WITH (NOLOCK) on DB.district_board_SD = D.District_Board 
				-- new add --
			WHERE NOT EXISTS (SELECT SP_ID FROM SPExceptionList WITH (NOLOCK)
							WHERE P.SP_ID = SPExceptionList.SP_ID)
 
			) T 
		ON T.Service_Type = P.Service_Type AND T.District_Board =TD.District_Code 
	GROUP BY P.Service_Type,TD.District_Code 
	 

	-- Total
	INSERT INTO @HCVSDHCPracticeIneligibleSummary (Service_Type, RegCode_Count,DistrictCode)
	SELECT 'Total',SUM(RegCode_Count),DistrictCode
	FROM 
		@HCVSDHCPracticeIneligibleSummary
	group by DistrictCode -- new add

	IF @debug='Y' 
	BEGIN 
		SELECT '@HCVSDHCPracticeIneligibleSummary'
		SELECT * FROM @HCVSDHCPracticeIneligibleSummary

	END;
-----------------------------------------------------------------------------------------
-- Get Data

	INSERT INTO @ReasonForVisitSummary (Service_Type, Reason_Code, Tx_Count,DistrictCode)
	SELECT P.Service_Type, R.Reason_Code, ISNULL(VT.TxCnt,0),TD.District_Code  
	FROM @Profession P  CROSS JOIN @tempDistrict TD
	CROSS JOIN @Reason R
	LEFT JOIN #VT_ReasonForVisit VT 
		ON VT.Reason_Code = R.reason_code AND VT.Service_Type = P.Service_Type
		 and VT.DistrictCode =TD.District_Code  --add
	 

	-- Total
	INSERT INTO @ReasonForVisitSummary (Service_Type, Reason_Code, Tx_Count,DistrictCode)
	SELECT 'Total', Reason_Code, SUM(Tx_Count),DistrictCode
	FROM @ReasonForVisitSummary
	GROUP BY Reason_Code,DistrictCode
	
	IF @debug='Y' 
	BEGIN 
		SELECT '@ReasonForVisitSummary'
		SELECT * FROM @ReasonForVisitSummary

	END;

------------------------------------------------------------
-- Result 00
------------------------------------------------------------
	INSERT INTO @ResultTable00 (Result_Value1) VALUES ('District: ' + @District_Name1 + ',' + @District_Name2  )
	INSERT INTO @ResultTable00 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))
	
	INSERT INTO @ResultTable00 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable00 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	VALUES ('', 
			'Under District Health Centre corporate account', '', '', '', '', '', 
			'Under individual Enrolled Health Care Provider account', '', '', '', '', '', '', '', '', '', '', 
			'Grand Total'
			)
			
	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2)
	VALUES ('','District Health Centre (Core + Satellite)')

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	VALUES ('','DIT','POD','SPT','DIT','POD','SPT','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','')

------------------------------------------------------------

	INSERT INTO @ResultTable00 (Result_Value1) VALUES 
	('(a)(i) Cumulative number of voucher claim transactions by principal reason for visit (Level 1):')

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT	[Reason_Desc], 
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-DIT] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-POD] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-SPT] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DIT] AS VARCHAR(100)) END,			
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([POD] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([SPT] AS VARCHAR(100)) END,						
			[ENU],[RCM],[RCP],[RDT],[RMP],CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
			[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(SELECT		S.Reason_Code, 
				R.Reason_Desc,
				S.Service_Type,						
				SUM(ISNULL(S.Tx_Count, 0)) AS [Count]
		FROM
			@ReasonForVisitSummary S
			LEFT JOIN @Reason R ON S.Reason_Code = R.Reason_Code
		GROUP BY S.Reason_Code, 
				R.Reason_Desc,
				S.Service_Type
	) SR
	pivot
	(	MAX([Count])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT
	ORDER BY Reason_Code


------------------------------------------------------------
	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT	'(a)(ii) Cumulative number of voucher claim transactions in total'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, SUM(Tx_Count) AS Tx_Count
		FROM @TransactionSummary
		GROUP BY Service_Type 
	) T
	pivot
	(	MAX(Tx_Count)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT	'(b) Cumulative amount of vouchers claimed ($)'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, SUM(Claim_Amt) AS Claim_Amt
		FROM @TransactionSummary
		GROUP BY Service_Type  
	) T
	pivot
	(	MAX(Claim_Amt)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT	'(c) Cumulative number of elders who have made use of vouchers for each service'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, SUM(NoOfElder) AS NoOfElder
		FROM (SELECT	Service_Type, DistrictCode, SUM(NoOfElder) AS NoOfElder
		FROM  @TransactionSummary
		GROUP BY Service_Type , DistrictCode) TS
		GROUP BY Service_Type
	) T
	pivot
	(	MAX(NoOfElder)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	DECLARE @Overall_NoOfElder INT
	SELECT 	@Overall_NoOfElder = SUM(NoofElder) FROM 
	(SELECT  COUNT(DISTINCT Identity_num) as NoofElder FROM #VoucherTransaction WHERE DistrictCode =@District_Code1 
	UNION ALL
	SELECT  COUNT(DISTINCT Identity_num) as NoofElder FROM #VoucherTransaction WHERE DistrictCode =@District_Code2) TVT


	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT 
		'(d) Cumulative number of elders who have ever made use of vouchers for District Health Centre related services (DHC-related services)',
		'', '', '', '',
		'', '', '', '', '', '', '', '', '', '', '', '', '', 
		@Overall_NoOfElder

------------------------------------------------------------

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT	'(e) Number of Service Providers eligible to make voucher claim for DHC-related services '
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT
			Service_Type, sum([Count]) as [Count]
		FROM
			(	SELECT Service_Type,DistrictCode, SUM(SP_Count) AS [Count]
				FROM @DHCRelatedServiceEligibleSummary 
				GROUP BY Service_Type ,DistrictCode
				UNION ALL
				SELECT Service_Type,DistrictCode, SUM(RegCode_Count) AS [Count]
				FROM @HCVSDHCPracticeIneligibleSummary
				GROUP BY Service_Type ,DistrictCode
			) TE
			GROUP BY Service_Type
	) T
	pivot
	(	SUM([COUNT])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17,Result_Value18,Result_Value19)
	SELECT	'(f) Number of Service Provders who had made voucher claim for DHC-related services'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(		
		SELECT 
			Service_Type, SUM([Count]) AS [Count]
		FROM
			(	SELECT	Service_Type,DistrictCode, SUM(SP_Count) AS [Count]
				FROM  @TransactionSummarySPBasis
				GROUP BY Service_Type ,DistrictCode
				UNION ALL
				SELECT	Service_Type,DistrictCode, SUM(RegCode_Count) AS [Count]
				FROM  @TransactionSummaryRegCodeBasis
				GROUP BY Service_Type ,DistrictCode
			) TS
			GROUP BY Service_Type 
	) T
	pivot
	(	SUM([Count])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2)
	SELECT '', ''
	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2)
	SELECT 'Abbreviation', ''

	INSERT INTO @ResultTable00 (Result_Value1, Result_Value2)
	SELECT Service_Category_Code, Service_Category_Desc 
	FROM Profession
	ORDER BY Service_Category_Code
------------------------------------------------------------
-- Result 01
------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('District: ' + @District_Name1 )
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))
	
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	VALUES ('', 
			'Under District Health Centre corporate account', '', '', '', '', '',
			'Under individual Enrolled Health Care Provider account', '', '', '', '', '', '', '', '', '', '', 
			'Grand Total'
			)
			
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2)
	VALUES ('','District Health Centre (Core + Satellite)' )

			
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	VALUES ('','DIT','POD','SPT','DIT','POD','SPT','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','')

------------------------------------------------------------

	INSERT INTO @ResultTable01 (Result_Value1) VALUES 
	('(a)(i) Cumulative number of voucher claim transactions by principal reason for visit (Level 1):')

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	[Reason_Desc], 
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-DIT] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-POD] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-SPT] AS VARCHAR(100)) END,
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
		WHERE DistrictCode = @District_Code1 
	) SR
	pivot
	(	MAX([Count])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT
	ORDER BY Reason_Code


------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(a)(ii) Cumulative number of voucher claim transactions in total'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, Tx_Count
		FROM @TransactionSummary
		WHERE DistrictCode = @District_Code1 
	) T
	pivot
	(	MAX(Tx_Count)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(b) Cumulative amount of vouchers claimed ($)'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, Claim_Amt
		FROM @TransactionSummary
		WHERE DistrictCode = @District_Code1 
	) T
	pivot
	(	MAX(Claim_Amt)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(c) Cumulative number of elders who have made use of vouchers for each service'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, NoOfElder
		FROM  @TransactionSummary
		WHERE DistrictCode = @District_Code1 
	) T
	pivot
	(	MAX(NoOfElder)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	
	SELECT 	@Overall_NoOfElder = COUNT(DISTINCT Identity_num) FROM #VoucherTransaction WHERE DistrictCode = @District_Code1 

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT 
		'(d) Cumulative number of elders who have ever made use of vouchers for District Health Centre related services (DHC-related services)',
		'', '', '', '',
		'', '', '', '', '', '', '', '', '', '', '', '', '',
		@Overall_NoOfElder

------------------------------------------------------------

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(e) Number of Service Providers eligible to make voucher claim for DHC-related services '
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT
			Service_Type, [Count]
		FROM
			(	SELECT Service_Type, SP_Count AS [Count]
				FROM @DHCRelatedServiceEligibleSummary
				WHERE DistrictCode = @District_Code1 
				UNION ALL
				SELECT Service_Type, RegCode_Count AS [Count]
				FROM @HCVSDHCPracticeIneligibleSummary
				WHERE DistrictCode = @District_Code1 
			) TE
	) T
	pivot
	(	SUM([COUNT])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(f) Number of Service Provders who had made voucher claim for DHC-related services'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(		
		SELECT 
			Service_Type, [Count]
		FROM
			(	SELECT	Service_Type, SP_Count AS [Count]
				FROM  @TransactionSummarySPBasis
				WHERE DistrictCode = @District_Code1 
				UNION ALL
				SELECT	Service_Type, RegCode_Count AS [Count]
				FROM  @TransactionSummaryRegCodeBasis
				WHERE DistrictCode = @District_Code1 
			) TS
	) T
	pivot
	(	SUM([Count])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2)
	SELECT '', ''
	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2)
	SELECT 'Abbreviation', ''

	INSERT INTO @ResultTable01 (Result_Value1, Result_Value2)
	SELECT Service_Category_Code, Service_Category_Desc 
	FROM Profession
	ORDER BY Service_Category_Code
------------------------------------------------------------
-- Result 02
------------------------------------------------------------
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('District: ' + @District_Name2 )
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))
	
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	VALUES ('', 
			'Under District Health Centre corporate account', '', '', '',  '', '',
			'Under individual Enrolled Health Care Provider account', '', '', '', '', '', '', '', '', '', '', 
			'Grand Total'
			)
 
			
	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2 )
	VALUES ('','District Health Centre (Core + Satellite)')


	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	VALUES ('','DIT','POD','SPT','DIT','POD','SPT','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','')

------------------------------------------------------------

	INSERT INTO @ResultTable02 (Result_Value1) VALUES 
	('(a)(i) Cumulative number of voucher claim transactions by principal reason for visit (Level 1):')

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
								Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	[Reason_Desc], 
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-DIT] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-POD] AS VARCHAR(100)) END,
			CASE WHEN Reason_Code = 5 THEN 'N/A' ELSE CAST([DHC-SPT] AS VARCHAR(100)) END,
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
		WHERE DistrictCode = @District_Code2 
	) SR
	pivot
	(	MAX([Count])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT
	ORDER BY Reason_Code


------------------------------------------------------------
	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(a)(ii) Cumulative number of voucher claim transactions in total'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, Tx_Count
		FROM @TransactionSummary
		WHERE DistrictCode = @District_Code2 
	) T
	pivot
	(	MAX(Tx_Count)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(b) Cumulative amount of vouchers claimed ($)'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, Claim_Amt
		FROM @TransactionSummary
		WHERE DistrictCode = @District_Code2 
	) T
	pivot
	(	MAX(Claim_Amt)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(c) Cumulative number of elders who have made use of vouchers for each service'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT	Service_Type, NoOfElder
		FROM  @TransactionSummary
		WHERE DistrictCode = @District_Code2 
	) T
	pivot
	(	MAX(NoOfElder)
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------
 	SELECT 	@Overall_NoOfElder = COUNT(DISTINCT Identity_num) FROM #VoucherTransaction WHERE DistrictCode = @District_Code2 

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT 
		'(d) Cumulative number of elders who have ever made use of vouchers for District Health Centre related services (DHC-related services)',
		'', '', '', '',
		'', '', '', '', '', '', '', '', '', '', '', '', '', 
		@Overall_NoOfElder

------------------------------------------------------------

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(e) Number of Service Providers eligible to make voucher claim for DHC-related services '
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(	SELECT
			Service_Type, [Count]
		FROM
			(	SELECT Service_Type, SP_Count AS [Count]  
				FROM @DHCRelatedServiceEligibleSummary
				WHERE DistrictCode = @District_Code2 
				UNION ALL
				SELECT Service_Type, RegCode_Count AS [Count]  
				FROM @HCVSDHCPracticeIneligibleSummary
				WHERE DistrictCode = @District_Code2
			) TE
			 
	) T
	pivot
	(	SUM([COUNT])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

------------------------------------------------------------

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT	'(f) Number of Service Provders who had made voucher claim for DHC-related services'
			,[DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT]
			,[ENU],[RCM],[RCP],[RDT],[RMP],[RMT]
			,[RNU],[ROP],[ROT],[RPT],[RRD],[Total]
	FROM
	(		
		SELECT 
			Service_Type, [Count]
		FROM
			(	SELECT	Service_Type, SP_Count AS [Count]
				FROM  @TransactionSummarySPBasis
				WHERE DistrictCode = @District_Code2 
				UNION ALL
				SELECT	Service_Type, RegCode_Count AS [Count]
				FROM  @TransactionSummaryRegCodeBasis
				WHERE DistrictCode = @District_Code2 
			) TS
	) T
	pivot
	(	SUM([Count])
		for Service_Type in ([DHC-DIT],[DHC-POD],[DHC-SPT],[DIT],[POD],[SPT],[ENU],[RCM],[RCP],[RDT],[RMP],[RMT],[RNU],[ROP],[ROT],[RPT],[RRD],[Total])
	) PT

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2)
	SELECT '', ''
	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2)
	SELECT 'Abbreviation', ''

	INSERT INTO @ResultTable02 (Result_Value1, Result_Value2)
	SELECT Service_Category_Code, Service_Category_Desc 
	FROM Profession
	ORDER BY Service_Category_Code
	-------------------------------------------------------
	 
	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT DCM.District_Board ,DCM.SP_ID,DCM.MO_Display_Seq ,MO.MO_Eng_Name ,MO.MO_Chi_Name    
	FROM DHCCoreMapping DCM INNER JOIN MedicalOrganization MO
	on DCM.SP_ID = MO.SP_ID and MO.Display_Seq  = DCM.MO_Display_Seq AND DCM.Record_Status = 'A'
	ORDER By DCM.District_Board, DCM.SP_ID,DCM.MO_Display_Seq
-- =============================================  
-- Return results  
-- =============================================  
	Declare @strGenDtm varchar(50)    
	SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
	SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)    
	SELECT 'Report Generation Time: ' + @strGenDtm  

-- --------------------------------------------------    
-- To Excel sheet: eHSM0011-01: Report of use of vouchers for District Health Centre related services (Kwai Tsing + Sham Shui Po)
-- --------------------------------------------------    
	SELECT	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
		Result_Value16, Result_Value17, Result_Value18, Result_Value19
	FROM	
		@ResultTable00
	ORDER BY	
		Result_Seq

-- --------------------------------------------------    
-- To Excel sheet: eHSM0011-01: Report of use of vouchers for District Health Centre related services (Kwai Tsing)
-- --------------------------------------------------    
	SELECT	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
		Result_Value16, Result_Value17, Result_Value18, Result_Value19
	FROM	
		@ResultTable01
	ORDER BY	
		Result_Seq

-- --------------------------------------------------    
-- To Excel sheet: eHSM0011-02: Report of use of vouchers for District Health Centre related services (Sham Shui Po)
-- --------------------------------------------------    
	SELECT	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
		Result_Value16, Result_Value17, Result_Value18, Result_Value19
	FROM	
		@ResultTable02
	ORDER BY	
		Result_Seq

-- --------------------------------------------------    
-- To Excel sheet: eHSM0011-03: Report of DHC Centre List
-- --------------------------------------------------    
	SELECT	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5 
	FROM	
		@ResultTable03
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
	SELECT  '      ii. For practice of claim transaction under the specified medical organization are counted as "District Health Centre (Core + Satellite)"', ''

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
	SELECT '            Practices are marked with specified medical organization (designated for the DHC Centre)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '            Number of Service Providers are counted by Professional Registration No. under practice', ''


	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '         b. DIT, POD, SPT', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '            Practices for those non-eligible healthcare providers not employed by DHC as staff and provide services under their own clinic', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '            Practices are marked with medical organization no. other than that specified for the DHC Centre', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '            Number of Service Providers are counted by Professional Registration No. under practice', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2. Under individual Enrolled Health Care Provider account', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      * Eligible healthcare service providers enrolled HCVS and joined DHC scheme', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      * Number of Service Providers are counted by Service Provider ID', ''

		INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '3. Calculation on Sub Report for all districts (eHS(S)M0011-01)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      Summation from all DHC district values', ''


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
