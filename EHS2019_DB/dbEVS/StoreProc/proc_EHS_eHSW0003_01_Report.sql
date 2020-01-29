IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0003_01_Report]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSW0003_01_Report]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Winnie SUEN
-- Modified date:	12 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	Handle DH connection status ([DH_Vaccine_Ref])
-- =============================================
---- =============================================
---- Author:			Marco CHOI
---- Create date:		14 Sep 2017
---- Description:		PCV13 Weekly Statistic Report
---- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0003_01_Report] 
	@Report_Dtm			DATETIME = NULL

--DECLARE @Report_Dtm			DATETIME = NULL
--SET @Report_Dtm = '2017-09-18'

AS BEGIN
-- ===================================
-- Declaration
-- ===================================

	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, @Report_Dtm, 106)
	END ELSE BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END
	
	DECLARE @SchemeDate Datetime
	SET @SchemeDate = DATEADD(dd, -1, @Report_Dtm)

	--Varaiable
	DECLARE @VSS_current_scheme_Seq					INT
	DECLARE @RVP_current_scheme_Seq					INT
	DECLARE @VSS_Start_Dtm							DATETIME
	DECLARE @RVP_Start_Dtm							DATETIME
	DECLARE @VSS_PCV13_Start_Dtm					DATETIME
	DECLARE @RVP_PCV13_Start_Dtm					DATETIME
	DECLARE @VSS_IV_Current_Season_Start_Dtm		DATETIME
	DECLARE @RVP_IV_Current_Season_Start_Dtm		DATETIME
	
	EXEC @VSS_current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'VSS', @SchemeDate
	EXEC @RVP_current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'RVP', @SchemeDate
	--	
	SELECT SG.Scheme_Code,
		SG.Scheme_Seq,
		SGD.Subsidize_Item_Code,
		SG.Claim_Period_From
	INTO #SubsidyDateTT
	FROM SubsidizeGroupClaim  SG
	INNER JOIN SubsidizeGroupClaimItemDetails SGD
	ON SG.Scheme_Code = SGD.Scheme_Code
	AND SG.Scheme_Seq = SGD.Scheme_Seq
	AND SG.Subsidize_Code = SGD.Subsidize_Code
	WHERE (SG.Scheme_Code = 'RVP' OR SG.Scheme_Code = 'VSS')

	SELECT @VSS_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'VSS' AND Scheme_Seq = '1' AND Subsidize_Item_Code='PV' 
	SELECT @RVP_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = '1' AND Subsidize_Item_Code='PV' 	
	SELECT @VSS_PCV13_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'VSS' AND Scheme_Seq = '1' AND Subsidize_Item_Code='PV13' 
	SELECT @RVP_PCV13_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = '1' AND Subsidize_Item_Code='PV13' 
	SELECT @VSS_IV_Current_Season_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'VSS' AND Scheme_Seq = @VSS_current_scheme_Seq AND Subsidize_Item_Code='SIV' 
	SELECT @RVP_IV_Current_Season_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = @RVP_current_scheme_Seq AND Subsidize_Item_Code='SIV' 
	
	
	
	--Label Text
	DECLARE @HighRisk							VARCHAR(100)	
	DECLARE @NonHighRisk						VARCHAR(100)	
	DECLARE @VSS_current_scheme_desc			VARCHAR(20)
	DECLARE @RVP_current_scheme_desc			VARCHAR(20)
	DECLARE @DisplayText_HighRisk				VARCHAR(200)
	DECLARE @DisplayText_NonHighRisk			VARCHAR(200)
	DECLARE @DisplayText_HavePrevious_PCV13		VARCHAR(200)
	DECLARE @DisplayText_NoPrevious_PCV13		VARCHAR(200)
	DECLARE @DisplayText_HavePrevious_23vPPV	VARCHAR(200)
	DECLARE @DisplayText_NoPrevious_23vPPV		VARCHAR(200)
	DECLARE @DisplayText_NoConnectionCMS		VARCHAR(200)
	DECLARE @DisplayText_NoConnectionCIMS		VARCHAR(200)
	DECLARE @DisplayText_NoConnectionAll		VARCHAR(200)
	DECLARE @DisplayText_Stat_HighRisk			VARCHAR(200)
	DECLARE @DisplayText_Stat_PCV13				VARCHAR(200)
	DECLARE @DisplayText_Stat_23vPPV			VARCHAR(200)
	DECLARE @DisplayText_Vacc_Before_PCV13		VARCHAR(200)
	DECLARE @DisplayText_Vacc_After_PCV13		VARCHAR(200)
	
	SELECT @HighRisk = Data_Value FROM StaticData WHERE Column_Name='VSS_RECIPIENTCONDITION' AND Item_No='HIGHRISK'
	SELECT @NonHighRisk = Data_Value FROM StaticData WHERE Column_Name='VSS_RECIPIENTCONDITION' AND Item_No='NOHIGHRISK'
	SELECT @VSS_current_scheme_desc = Season_Desc FROM VaccineSeason WHERE Scheme_Code = 'VSS' AND Scheme_Seq = @VSS_current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
	SELECT @RVP_current_scheme_desc = Season_Desc FROM VaccineSeason WHERE Scheme_Code = 'RVP' AND Scheme_Seq = @RVP_current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
	SET @DisplayText_HighRisk				= '  ' + @HighRisk + ' Condition'
	SET @DisplayText_NonHighRisk			= '  ' + @NonHighRisk +' Condition'
	SET @DisplayText_HavePrevious_PCV13		= '  Have previous PCV13 vaccination'
	SET @DisplayText_HavePrevious_23vPPV	= '  Have previous 23vPPV vaccination'
	SET @DisplayText_NoPrevious_PCV13		= '  No previous PCV13 Vaccination'
	SET @DisplayText_NoPrevious_23vPPV		= '  No previous 23vPPV Vaccination'
	SET @DisplayText_NoConnectionCMS		= '    (Failed to connect HA)'
	SET @DisplayText_NoConnectionCIMS		= '    (Failed to connect DH)'
	SET @DisplayText_NoConnectionAll		= '    (Failed to connect HA and DH)'
	SET @DisplayText_Stat_HighRisk			= 'Statistics on High-Risk Condition (Note 1)'
	SET @DisplayText_Stat_PCV13				= 'Statistics on previous PCV13 vaccination (Note 2)'
	SET @DisplayText_Stat_23vPPV			= 'Statistics on previous 23vPPV vaccination (Note 2)'
	SET @DisplayText_Vacc_Before_PCV13		= 'Vaccination before commencement of PCV13 (with service date before [DATE])'
	SET @DisplayText_Vacc_After_PCV13		= 'Vaccination after commencement of PCV13 (service date start from [DATE])'
	--

	-- EHS_Vaccine_Ref, HA_Vaccine_Ref, DH_Vaccine_Ref
	-- Value, 1st Char= 23vPPV, 2nd Char = PCV13
	DECLARE @CMS_Pos_23vPPV INT
	DECLARE @CMS_Pos_PCV13 INT
	SET @CMS_Pos_23vPPV = 1
	SET @CMS_Pos_PCV13 = 2
	--

	CREATE TABLE #Result (
		Display_Seq				INT IDENTITY(1,1),
		Value1					varchar(200) DEFAULT NULL,
		Value2					varchar(200) DEFAULT NULL,
		Value3					varchar(200) DEFAULT NULL,
		Value4					varchar(200) DEFAULT NULL,
		Value5					varchar(200) DEFAULT NULL
	)
		
-- ===================================
-- Gather result
-- ===================================

	SELECT 
		Transaction_ID,
		Transaction_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Scheme_Code,
		Service_Receive_Dtm,
		High_Risk,
		ISNULL(EHS_Vaccine_Ref, '') EHS_Vaccine_Ref,
		ISNULL(HA_Vaccine_Ref, 'NN') HA_Vaccine_Ref,	-- Consider as no vaccine record
		ISNULL(Ext_Ref_Status, '') HA_Vaccine_Ref_Status,
		ISNULL(DH_Vaccine_Ref, 'NN') DH_Vaccine_Ref,	-- Consider as no vaccine record
		ISNULL(DH_Vaccine_Ref_Status, '') DH_Vaccine_Ref_Status,
		Subsidize_Item_Code,
		CASE WHEN (Service_Receive_Dtm >= @VSS_IV_Current_Season_Start_Dtm) THEN 'Y' ELSE 'N' END AS 'Current_Season'
	INTO #VT_VSS
	FROM #VT
	WHERE Scheme_Code='VSS' 

	SELECT 
		VT.Transaction_ID,
		Transaction_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		VT.Scheme_Code,
		Service_Receive_Dtm,
		High_Risk,
		ISNULL(EHS_Vaccine_Ref, '') EHS_Vaccine_Ref,
		ISNULL(HA_Vaccine_Ref, 'NN') HA_Vaccine_Ref,	-- Consider as no vaccine record
		ISNULL(Ext_Ref_Status, '') HA_Vaccine_Ref_Status,
		ISNULL(DH_Vaccine_Ref, 'NN') DH_Vaccine_Ref,	-- Consider as no vaccine record
		ISNULL(DH_Vaccine_Ref_Status, '') DH_Vaccine_Ref_Status,
		VT.Subsidize_Item_Code,
		CASE WHEN (Service_Receive_Dtm >= @RVP_IV_Current_Season_Start_Dtm) THEN 'Y' ELSE 'N' END AS 'Current_Season',		
		HL.Type AS [RCH_Type] 
	INTO #VT_RVP
	FROM #VT VT
	INNER JOIN TransactionAdditionalField TAF2        
		ON VT.Transaction_ID = TAF2.Transaction_ID        
		AND TAF2.AdditionalFieldID = 'RHCCode'        
	INNER JOIN RVPHomeList HL        
		ON TAF2.AdditionalFieldValueCode = HL.RCH_Code  
	WHERE VT.Scheme_Code='RVP' 
		
	INSERT INTO #Result (Value1) SELECT 'Reporting period: as at ' + FORMAT(@SchemeDate, 'yyyy/MM/dd')
	INSERT INTO #Result (Value1) SELECT ''

	-- ====== VSS part ========
	INSERT INTO #Result (Value1) SELECT REPLACE('(1) VSS (start from [DATE])', '[DATE]', FORMAT(@VSS_Start_Dtm, 'dd MMM yyyy')) 
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	 
	SELECT '', '', '', 'Cumulative', @VSS_current_scheme_desc
	
	-- ****** 23vPPV ******
	INSERT INTO #Result (Value1, Value2) SELECT '(i)', '23vPPV'
		
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT 'a.', 
		REPLACE(@DisplayText_Vacc_Before_PCV13, '[DATE]', FORMAT(@VSS_PCV13_Start_Dtm, 'dd MMM yyyy')),
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm < @VSS_PCV13_Start_Dtm
				
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT 'b.', 
		REPLACE(@DisplayText_Vacc_After_PCV13, '[DATE]', FORMAT(@VSS_PCV13_Start_Dtm, 'dd MMM yyyy')),
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
		
	INSERT INTO #Result (Value1, Value2)	SELECT '', 'b.1 ' + @DisplayText_Stat_HighRisk

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_HighRisk,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND High_Risk IS NOT NULL
	AND ISNULL(High_Risk, '') ='Y'

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NonHighRisk,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND High_Risk IS NOT NULL
	AND ISNULL(High_Risk, '') ='N'
		
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND High_Risk IS NOT NULL					

	INSERT INTO #Result (Value1, Value2)	SELECT '', 'b.2 ' + @DisplayText_Stat_PCV13
	
	-- No PCV13 (Include vaccine connect fail)
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoPrevious_PCV13,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 
	AND (HA_Vaccine_Ref = 'CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N')
	AND (DH_Vaccine_Ref = 'CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N')


	-- No PCV13 and HA connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionCMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 	
	AND HA_Vaccine_Ref = 'CC'
	AND SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N'

	-- No PCV13 and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionCIMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 	
	AND SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N'
	AND DH_Vaccine_Ref = 'CC'

	-- No PCV13 and both HA and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionAll,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 	
	AND HA_Vaccine_Ref = 'CC'
	AND DH_Vaccine_Ref = 'CC'


	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_HavePrevious_PCV13,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'Y'
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'Y'
		)

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'Total (a+b)',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV'

	INSERT INTO #Result (Value1) SELECT ''
	
	-- ****** PCV13 ******
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	 
	SELECT '', '', '', 'Cumulative', @VSS_current_scheme_desc
	INSERT INTO #Result (Value1, Value2) SELECT '(ii)', REPLACE('PCV13 (start from [DATE])', '[DATE]', FORMAT(@VSS_PCV13_Start_Dtm, 'dd MMM yyyy'))

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT 'a.', 
		REPLACE(@DisplayText_Vacc_After_PCV13, '[DATE]', FORMAT(@VSS_PCV13_Start_Dtm, 'dd MMM yyyy')),
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code = 'PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm

	INSERT INTO #Result (Value1, Value2)	SELECT '', 'a.1 ' + @DisplayText_Stat_HighRisk
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_HighRisk,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND High_Risk IS NOT NULL
	AND ISNULL(High_Risk, '') ='Y'

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NonHighRisk,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND High_Risk IS NOT NULL
	AND ISNULL(High_Risk, '') ='N'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND High_Risk IS NOT NULL				

	INSERT INTO #Result (Value1, Value2)	SELECT '', 'a.2 ' + @DisplayText_Stat_23vPPV

	-- No 23vPPV (Include vaccine connect fail) 
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoPrevious_23vPPV,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND (HA_Vaccine_Ref='CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND (DH_Vaccine_Ref='CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')

	-- No 23vPPV and HA connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionCMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND HA_Vaccine_Ref='CC' 
	AND SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N'

	-- No 23vPPV and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionCIMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N'
	AND DH_Vaccine_Ref='CC'

	-- No 23vPPV and both HA and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionAll,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND HA_Vaccine_Ref='CC'
	AND DH_Vaccine_Ref='CC'


	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_HavePrevious_23vPPV,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y' 
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		)

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)			
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm
		
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)			
	SELECT '', 
		'',
		'Total (a)',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_VSS
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @VSS_PCV13_Start_Dtm

	INSERT INTO #Result (Value1) SELECT ''

	-- ========================
	
	-- ====== RVP part ========
	INSERT INTO #Result (Value1) SELECT REPLACE('(2) RVP (start from [DATE])', '[DATE]', FORMAT(@RVP_Start_Dtm, 'dd MMM yyyy')) 
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5) SELECT '', '', '', 'Cumulative', @RVP_current_scheme_desc
	
	-- ****** 23vPPV ******
	INSERT INTO #Result (Value1, Value2) SELECT '(i)', '23vPPV'
		
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT 'a.', 
		REPLACE(@DisplayText_Vacc_Before_PCV13, '[DATE]', FORMAT(@RVP_PCV13_Start_Dtm, 'dd MMM yyyy')),
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm < @RVP_PCV13_Start_Dtm
				
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT 'b.', 
		REPLACE(@DisplayText_Vacc_After_PCV13, '[DATE]', FORMAT(@RVP_PCV13_Start_Dtm, 'dd MMM yyyy')),
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
		
	INSERT INTO #Result (Value1, Value2)	SELECT '', 	'b.1 ' + @DisplayText_Stat_PCV13
	
	-- No PCV13
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_NoPrevious_PCV13,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 
	AND (HA_Vaccine_Ref = 'CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N')
	AND (DH_Vaccine_Ref = 'CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N')

	-- No PCV13 and HA connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionCMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 
	AND HA_Vaccine_Ref='CC' 
	AND SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N'

	-- No PCV13 and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionCIMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 
	AND SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N'
	AND DH_Vaccine_Ref='CC'

	-- No PCV13 and both HA and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)		
	SELECT '', 
		@DisplayText_NoConnectionAll,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'N' 
	AND HA_Vaccine_Ref='CC' 
	AND DH_Vaccine_Ref='CC' 

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		@DisplayText_HavePrevious_PCV13,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'Y' 
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_PCV13, 1) = 'Y'
		)

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'Total (a+b)',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV'

	INSERT INTO #Result (Value1) SELECT ''

	-- ****** PCV13 ******
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	SELECT '', '', '', 'Cumulative', @RVP_current_scheme_desc
	INSERT INTO #Result (Value1, Value2)	SELECT '(ii)', REPLACE('PCV13 (start from [DATE])', '[DATE]', FORMAT(@RVP_PCV13_Start_Dtm, 'dd MMM yyyy'))

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT 'a.', 
		REPLACE(@DisplayText_Vacc_After_PCV13, '[DATE]', FORMAT(@RVP_PCV13_Start_Dtm, 'dd MMM yyyy')),
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm

	INSERT INTO #Result (Value1, Value2)	SELECT '', 'a.1 ' + @DisplayText_Stat_23vPPV
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'RCHD',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND RCH_Type ='D'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'RCHE',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND RCH_Type ='E'
		
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'IPID',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND RCH_Type ='I'
		
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND (RCH_Type ='D' OR RCH_Type ='E' OR RCH_Type ='I')
	
	INSERT INTO #Result (Value1, Value2)	SELECT '', 	@DisplayText_NoPrevious_23vPPV

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'RCHD',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND (HA_Vaccine_Ref='CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND (DH_Vaccine_Ref='CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND RCH_Type ='D'

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'RCHE',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND (HA_Vaccine_Ref='CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND (DH_Vaccine_Ref='CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND RCH_Type ='E'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'IPID',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND (HA_Vaccine_Ref='CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND (DH_Vaccine_Ref='CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND RCH_Type ='I'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND (HA_Vaccine_Ref='CC' OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND (DH_Vaccine_Ref='CC' OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N')
	AND (RCH_Type ='D' OR RCH_Type ='E' OR RCH_Type ='I')
	
	-- No 23vPPV and HA connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_NoConnectionCMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND HA_Vaccine_Ref='CC' 
	AND SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N'

	-- No 23vPPV and DH connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_NoConnectionCIMS,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N'
	AND DH_Vaccine_Ref = 'CC'

	-- No 23vPPV and all connect fail
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		@DisplayText_NoConnectionAll,
		'',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'N' 
	AND HA_Vaccine_Ref = 'CC' 
	AND DH_Vaccine_Ref = 'CC' 

	INSERT INTO #Result (Value1, Value2)	SELECT '', @DisplayText_HavePrevious_23vPPV
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'RCHD',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y' 
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		)
	AND RCH_Type ='D'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'RCHE',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y' 
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		)
	AND RCH_Type ='E'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'IPID',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y' 
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		)
	AND RCH_Type ='I'
	
	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'Total',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm
	AND (SUBSTRING(EHS_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y' 
		OR SUBSTRING(HA_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		OR SUBSTRING(DH_Vaccine_Ref, @CMS_Pos_23vPPV, 1) = 'Y'
		)
	AND (RCH_Type ='D' OR RCH_Type ='E' OR RCH_Type ='I')

	INSERT INTO #Result (Value1, Value2, Value3, Value4, Value5)	
	SELECT '', 
		'',
		'Total (a)',
		ISNULL(SUM(1), 0),
		ISNULL(SUM(CASE WHEN Current_Season = 'Y' THEN 1 ELSE 0 END ), 0)
	FROM #VT_RVP
	WHERE Subsidize_Item_Code='PV13'
	AND Service_Receive_Dtm >= @RVP_PCV13_Start_Dtm

	INSERT INTO #Result (Value1) SELECT ''

	-- ========================
	
	-- ====== Remark ==========	
	INSERT INTO #Result (Value1) SELECT 'Remark:'
	INSERT INTO #Result (Value1, Value2) SELECT 'a.', @VSS_current_scheme_desc + ' VSS start from ' + FORMAT(@VSS_IV_Current_Season_Start_Dtm, 'dd MMM yyyy')
	INSERT INTO #Result (Value1, Value2) SELECT 'b.', @RVP_current_scheme_desc + ' RVP start from ' + FORMAT(@RVP_IV_Current_Season_Start_Dtm, 'dd MMM yyyy')
	INSERT INTO #Result (Value1, Value2) SELECT 'Note 1: ', 'Capturing of recipient''s conditions start from ' + FORMAT(@VSS_PCV13_Start_Dtm, 'dd MMM yyyy')
	INSERT INTO #Result (Value1, Value2) SELECT 'Note 2: ', 'Including those who have already received vaccination at eHS(S)/HA/DH and as at when the claim was made'
	-- ========================	

	SELECT	
		ISNULL(Value1, ''), 
		ISNULL(Value2, ''), 
		ISNULL(Value3, ''), 
		ISNULL(Value4, ''), 
		ISNULL(Value5, '')
	FROM #Result 
	ORDER By Display_Seq ASC
	
	--
	DROP TABLE #SubsidyDateTT
	--DROP TABLE #VT
	DROP TABLE #VT_VSS
	DROP TABLE #VT_RVP
	DROP TABLE #Result

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0003_01_Report] TO HCVU
GO

