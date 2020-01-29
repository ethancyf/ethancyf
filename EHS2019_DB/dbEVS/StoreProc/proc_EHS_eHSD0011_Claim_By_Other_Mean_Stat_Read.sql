IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0011_Claim_By_Other_Mean_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0011_Claim_By_Other_Mean_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Change table from [_IVRSClaimDurationSummary] to [RpteHSD0011_07_IVRSClaimDuration]
--					Performance Tuning
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Helen Lam
-- Modified date:	16 Nov 2011
-- Description:	(1)Change the exiting worksheets data from IVRS to include all interface system (i.e. IVRS, HKMA IH, HKDA IH) 
--				(2)Add a new summary worksheet to show the total number of transactions and total number of SP that using upload claim through interface systems and named it as ．Summary・
--				(3)Add a new worksheet to show the number of transaction by each scheme through interface systems and named it as ・04-Tx by Scheme・

-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	7 October 2011
-- Description:		(1) Added 'ROP' for the new profession of optometrist
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 April 2011
-- Description:		(1) Refine the layout
--					(2) Add total
--					(3) Include all transactions regardless the transaction status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 December 2010
-- Description:		Fix the date comparison
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Oct 2008
-- Description:	Statistics for getting voucher claim by using IVRS
--				1. No. of SP using IVRS to claim by Profession
--				2. No. of Transaction by Profession (IVRS)
--				3. No. of Voucher Claimed by Profession (IVRS)
--				4. Get related data to temp table about claim duration (_IVRSClaimDurationSummary)
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	15 Oct 2009
-- Description:		Update using new Audit Log entry
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

create PROCEDURE [dbo].[proc_EHS_eHSD0011_Claim_By_Other_Mean_Stat_Read]
@Report_Dtm		datetime = NULL
AS 
BEGIN

	SET NOCOUNT ON;
	
	declare @start_dtm varchar(50)
	declare @end_dtm varchar(50)
	declare @temp_dtm varchar(50)
	declare @no_of_days int
	select @no_of_days=14

	IF @Report_Dtm IS NOT NULL 
	BEGIN
		SELECT @end_dtm = CONVERT(varchar, DATEADD(dd, 1, @Report_Dtm), 106)
		
	END 
	ELSE 
	BEGIN
		SELECT @end_dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
		set @Report_Dtm=getdate()
	END
	
	
	select @start_dtm  =  CONVERT(VARCHAR(11), dateadd(day, -(@no_of_days), @end_dtm), 106)
	
--	select @start_dtm  =  CONVERT(VARCHAR(11), dateadd(day, -(@no_of_days), GETDATE()), 106)
--	select @end_dtm  =  CONVERT(VARCHAR(11), GETDATE(), 106)
--	

	if DATEDIFF(dd, @start_dtm, '01 Jan 2009') > 0
	begin
		select @start_dtm = '01 Jan 2009'
	end
	--add new interface variable
	declare @appsource varchar(20) -- added CRE11-022
	-- # of SP by Professional
	declare @sp_ENU int
	declare @sp_RCM int
	declare @sp_RCP int
	declare @sp_RDT int
	declare @sp_RMP int
	declare @sp_RMT int
	declare @sp_RNU int
	declare @sp_ROP int				-- added CRE11-024-02
	declare @sp_ROT int
	declare @sp_RPT int
	declare @sp_RRD int
	declare @sp_Total int

	-- # of Transaction by Professional
	declare @t_ENU int
	declare @t_RCM int
	declare @t_RCP int
	declare @t_RDT int
	declare @t_RMP int
	declare @t_RMT int
	declare @t_RNU int
	declare @t_ROP int				-- added CRE11-024-02
	declare @t_ROT int
	declare @t_RPT int
	declare @t_RRD int
	declare @t_Total int

	-- # of Voucher Claimed by Professional
	declare @c_ENU int
	declare @c_RCM int
	declare @c_RCP int
	declare @c_RDT int
	declare @c_RMP int
	declare @c_RMT int
	declare @c_RNU int
	declare @c_ROP int				-- added CRE11-024-02
	declare @c_ROT int
	declare @c_RPT int
	declare @c_RRD int
	declare @c_Total int

	-- # of Transaction by Scheme added CRE11-022
	declare @s_HCVS int
	declare @s_CIVSS int
	declare @s_EVSS int
	declare @s_RVP int
	declare @s_HSIVSS int
	declare @s_Total int

	declare	@claim_max char(8)
	declare	@claim_min char(8)
	declare	@claim_avg char(8)
	declare	@claim_T int

	set @claim_max='N/A'
	set @claim_min='N/A'
	set @claim_avg='N/A'
	
	CREATE TABLE #TxAll
	(
		SP_ID				CHAR(8),
		Service_Type		CHAR(5),
		Scheme_Code			CHAR(10),
		TxCount				INT,
		AppSource			VARCHAR(10),
		DataEntry_By		VARCHAR(20),
		Manual_Reimburse	CHAR(1)
	)

	CREATE TABLE #TxDaily
	(
		Transaction_Dtm		DATE,
		SP_ID				CHAR(8),
		Service_Type		CHAR(5),
		Scheme_Code			CHAR(10),
		TxCount				INT,
		Amount				MONEY,
		AppSource			VARCHAR(10),
		DataEntry_By		VARCHAR(20),
		Manual_Reimburse	CHAR(1)
	)

	-- Summary information , added CRE11-022
	create table #Summary
	(
		appsource	varchar(20),
		dummyspace	varchar(20) default ' ' ,		
		tol_no_trans varchar(20) default ' ',
		tol_no_SP  varchar(20) default ' '
	)
	-- # of SP by Profession
	create table #SP
	(
		report_type char(1),	 --'E' means external, 'P' means PCS
		report_dtm varchar(20),
		ENU_1 int DEFAULT 0 ,
		RCM_1 int DEFAULT 0,
		RCP_1 int DEFAULT 0,
		RDT_1 int DEFAULT 0,
		RMP_1 int DEFAULT 0,
		RMT_1 int DEFAULT 0,
		RNU_1 int DEFAULT 0,
		ROP_1 int DEFAULT 0,						-- added CRE11-024-02
		ROT_1 int DEFAULT 0,
		RPT_1 int DEFAULT 0,
		RRD_1 int DEFAULT 0,
		Total_1 int DEFAULT 0,
		ENU_2 int DEFAULT 0,
		RCM_2 int DEFAULT 0,
		RCP_2 int DEFAULT 0,
		RDT_2 int DEFAULT 0,
		RMP_2 int DEFAULT 0,
		RMT_2 int DEFAULT 0,
		RNU_2 int DEFAULT 0,
		ROP_2 int DEFAULT 0,						-- added CRE11-024-02
		ROT_2 int DEFAULT 0,
		RPT_2 int DEFAULT 0,
		RRD_2 int DEFAULT 0,
		Total_2 int DEFAULT 0
	)

	-- # of Transaction by Profession
	create table #T
	(
		report_type char(1),	
		report_dtm varchar(20),
		ENU_1 int DEFAULT 0 ,
		RCM_1 int DEFAULT 0,
		RCP_1 int DEFAULT 0,
		RDT_1 int DEFAULT 0,
		RMP_1 int DEFAULT 0,
		RMT_1 int DEFAULT 0,
		RNU_1 int DEFAULT 0,
		ROP_1 int DEFAULT 0,						-- added CRE11-024-02
		ROT_1 int DEFAULT 0,
		RPT_1 int DEFAULT 0,
		RRD_1 int DEFAULT 0,
		Total_1 int DEFAULT 0,
		ENU_2 int DEFAULT 0,
		RCM_2 int DEFAULT 0,
		RCP_2 int DEFAULT 0,
		RDT_2 int DEFAULT 0,
		RMP_2 int DEFAULT 0,
		RMT_2 int DEFAULT 0,
		RNU_2 int DEFAULT 0,
		ROP_2 int DEFAULT 0,						-- added CRE11-024-02
		ROT_2 int DEFAULT 0,
		RPT_2 int DEFAULT 0,
		RRD_2 int DEFAULT 0,
		Total_2 int DEFAULT 0
	)

	-- # of Voucher Claimed by Profession
	create table #C
	(
	
		report_type char(1),	
		report_dtm varchar(20),
		ENU_1 int DEFAULT 0 ,
		RCM_1 int DEFAULT 0,
		RCP_1 int DEFAULT 0,
		RDT_1 int DEFAULT 0,
		RMP_1 int DEFAULT 0,
		RMT_1 int DEFAULT 0,
		RNU_1 int DEFAULT 0,
		ROP_1 int DEFAULT 0,						-- added CRE11-024-02
		ROT_1 int DEFAULT 0,
		RPT_1 int DEFAULT 0,
		RRD_1 int DEFAULT 0,
		Total_1 int DEFAULT 0,
		ENU_2 int DEFAULT 0,
		RCM_2 int DEFAULT 0,
		RCP_2 int DEFAULT 0,
		RDT_2 int DEFAULT 0,
		RMP_2 int DEFAULT 0,
		RMT_2 int DEFAULT 0,
		RNU_2 int DEFAULT 0,
		ROP_2 int DEFAULT 0,						-- added CRE11-024-02
		ROT_2 int DEFAULT 0,
		RPT_2 int DEFAULT 0,
		RRD_2 int DEFAULT 0,
		Total_2 int DEFAULT 0
	)

-- # of transation by Scheme added CRE11-022
	create table #TranScheme
	(
		report_type char(1),
		report_dtm varchar(20),
		HCVS_1   int DEFAULT 0,
		CIVSS_1  int DEFAULT 0,
		EVSS_1   int DEFAULT 0,		
		RVP_1    int DEFAULT 0,
		HSIVSS_1 int DEFAULT 0,		
		Total_1  int DEFAULT 0,
		HCVS_2   int DEFAULT 0,
		CIVSS_2  int DEFAULT 0,
		EVSS_2   int DEFAULT 0,		
		RVP_2    int DEFAULT 0,
		HSIVSS_2 int DEFAULT 0,		
		Total_2  int DEFAULT 0
		
	)
	create table #result
	(
		report_dtm varchar(11),
		claim_max char(8) default '0',
		claim_min char(8) default '0',
		claim_avg char(8) default '0',
		claim_T int default 0
	)
	
	--Get total Summary 
	INSERT INTO #TxAll (SP_ID, 
		Service_Type,
		Scheme_Code,
		TxCount,
		AppSource,
		DataEntry_By,
		Manual_Reimburse)
	SELECT 
		SP_ID,
		Service_Type,
		Scheme_Code,
		COUNT(Transaction_ID) TxCount,
		SourceApp,
		CASE WHEN IsUpload = 'Y' THEN DataEntry_By ELSE '' END DataEntry_By,
		ISNULL(Manual_Reimburse, 'N') Manual_Reimburse
	FROM VoucherTransaction WITH (NOLOCK)
	WHERE Transaction_Dtm < @end_dtm
		AND Record_Status Not in ('I','D') 
		AND ISNULL(Invalidation,'') <> 'I'
	GROUP BY
		SP_ID,
		Service_Type,
		Scheme_Code,
		SourceApp,
		CASE WHEN IsUpload = 'Y' THEN DataEntry_By ELSE '' END,
		Manual_Reimburse

	--Get daily Summary 
	INSERT INTO #TxDaily (Transaction_Dtm,
			SP_ID,
			Service_Type,
			Scheme_Code,
			TxCount,
			Amount,
			AppSource,
			DataEntry_By,
			Manual_Reimburse)
	SELECT 
		CONVERT(DATE, Transaction_Dtm),
		SP_ID,
		Service_Type,
		Scheme_Code,
		COUNT(Transaction_ID) TxCount,
		SUM(Claim_Amount) Amount,
		SourceApp,
		CASE WHEN IsUpload = 'Y' THEN DataEntry_By ELSE '' END DataEntry_By,
		ISNULL(Manual_Reimburse, 'N') Manual_Reimburse
	FROM VoucherTransaction WITH (NOLOCK)
	WHERE Transaction_Dtm >= @start_dtm AND transaction_dtm < @end_dtm
		AND Record_Status Not in ('I', 'D','W') 
		AND ISNULL(Invalidation,'') <> 'I'
	GROUP BY
		CONVERT(DATE, Transaction_Dtm),
		SP_ID,
		Service_Type,
		Scheme_Code,
		SourceApp,
		CASE WHEN IsUpload = 'Y' THEN DataEntry_By ELSE '' END,
		Manual_Reimburse


	--Get total Summary for WEB-FULL
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'WEB (FULL)',' ', cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct SP_ID),0) as varchar(20)) FROM #TxAll
	WHERE  AppSource in ('WEB', 'WEB-FULL') and Manual_Reimburse='N' 
	
	--Get total Summary for WEB-TEXT
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'WEB (TEXT ONLY)',' ', cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20))FROM #TxAll
	WHERE  AppSource = 'WEB-TEXT'  and Manual_Reimburse='N' 


	--Get total summary for IVRS
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'IVRS',' ',cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM #TxAll
	WHERE AppSource = 'IVRS'

	--Get total summary for PCS
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'PCS',' ', cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM #TxAll
	 WHERE AppSource='externalws'  AND Manual_Reimburse='N' 

	--Get total summary for HCVU
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'HCVU',' ', cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM #TxAll
	 WHERE AppSource not in ('externalws', 'IVRS')  AND manual_reimburse='Y'

	--Total Transaction
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' ','Total Transaction', cast(isnull(SUM(TxCount),0) as varchar(20)), ' ' FROM #TxAll
		
	--insert dummy line
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' ',' ', ' ',' '	 

	insert into #Summary (appsource, dummyspace,tol_no_trans, tol_no_SP )
	SELECT 'PCS', ' ',' ',' '
	
	--Get total summary for HKMA
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' -HKMA IH', ' ', cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM #TxAll
	WHERE AppSource='externalws' and dataentry_by='HKMA IH'

	--Get total summary for HKDA
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' -HKDA IH', ' ',cast(isnull(SUM(TxCount),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM #TxAll
	WHERE AppSource='externalws' and dataentry_by='HKDA IH' 
		
	

	-- Get Daily History
	select @temp_dtm = @start_dtm
	while datediff(d, @temp_dtm, @end_dtm) > 0
	BEGIN
		select @claim_max = 'N/A'
		select @claim_min = 'N/A'
		select @claim_avg = 'N/A'
		select @claim_T = 0
		
		--prepare record to excel 
		insert into #SP (report_type, report_dtm) values('E',CONVERT(VARCHAR(11), @temp_dtm, 106))
		insert into #SP (report_type, report_dtm) values('P',CONVERT(VARCHAR(11), @temp_dtm, 106))
		insert into #t (report_type, report_dtm) values('E',CONVERT(VARCHAR(11), @temp_dtm, 106))
		insert into #t (report_type, report_dtm) values('P',CONVERT(VARCHAR(11), @temp_dtm, 106))
		--Not need voucher by change of crp-022
		--		insert into #C (report_type, report_dtm) values('E',CONVERT(VARCHAR(11), @temp_dtm, 106))
		--		insert into #C (report_type, report_dtm) values('P',CONVERT(VARCHAR(11), @temp_dtm, 106))	
		insert into #TranScheme (report_type, report_dtm) values('E',CONVERT(VARCHAR(11), @temp_dtm, 106))
		insert into #TranScheme (report_type, report_dtm) values('P',CONVERT(VARCHAR(11), @temp_dtm, 106))	
		--loop by application source
		declare cur_tran cursor for
		select distinct AppSource  from #TxDaily 
		open  cur_tran
		fetch cur_tran into @appsource
		while @@Fetch_status=0
		begin
		
		-- # of SP using IVRS,HKMA,HKDA by Profession
		
		select @sp_ENU = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'ENU' and AppSource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RCM = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RCM' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RCP = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RCP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RDT = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RDT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RMP = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RMP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RMT = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RMT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RNU = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RNU' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_ROP = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'ROP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_ROT = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'ROT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RPT = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RPT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_RRD = isnull(count(distinct sp_id),0) from #TxDaily where service_type = 'RRD' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @sp_Total = isnull(count(distinct sp_id),0) from #TxDaily where appsource=@appsource and transaction_dtm = @temp_dtm

		-- # of Transaction using IVRS,HKMA,HKDA by Profession
		select @t_ENU = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'ENU' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RCM = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RCM' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RCP = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RCP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RDT = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RDT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RMP = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RMP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RMT = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RMT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RNU = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RNU' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_ROP = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'ROP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_ROT = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'ROT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RPT = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RPT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_RRD = isnull(SUM(TxCount),0) from #TxDaily where service_type = 'RRD' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @t_Total = isnull(SUM(TxCount),0) from #TxDaily where appsource=@appsource and transaction_dtm = @temp_dtm

		-- # of Voucher Claimed using IVRS, HKMA, HKDA by Profession
		select @c_ENU = isnull(sum(Amount),0) from #TxDaily where service_type = 'ENU' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RCM = isnull(sum(Amount),0) from #TxDaily where service_type = 'RCM' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RCP = isnull(sum(Amount),0) from #TxDaily where service_type = 'RCP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RDT = isnull(sum(Amount),0) from #TxDaily where service_type = 'RDT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RMP = isnull(sum(Amount),0) from #TxDaily where service_type = 'RMP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RMT = isnull(sum(Amount),0) from #TxDaily where service_type = 'RMT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RNU = isnull(sum(Amount),0) from #TxDaily where service_type = 'RNU' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_ROP = isnull(sum(Amount),0) from #TxDaily where service_type = 'ROP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_ROT = isnull(sum(Amount),0) from #TxDaily where service_type = 'ROT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RPT = isnull(sum(Amount),0) from #TxDaily where service_type = 'RPT' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_RRD = isnull(sum(Amount),0) from #TxDaily where service_type = 'RRD' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @c_Total = isnull(sum(Amount),0) from #TxDaily where  appsource=@appsource and transaction_dtm = @temp_dtm

		-- # of Transaction using IVRS, HKMA, HKDA by Scheme
		select @s_HCVS  = isnull(SUM(TxCount),0) from #TxDaily where scheme_code = 'HCVS' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @s_CIVSS = isnull(SUM(TxCount),0) from #TxDaily where scheme_code = 'CIVSS' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @s_EVSS  = isnull(SUM(TxCount),0) from #TxDaily where scheme_code = 'EVSS' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @s_RVP   = isnull(SUM(TxCount),0) from #TxDaily where scheme_code = 'RVP' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @s_HSIVSS = isnull(SUM(TxCount),0) from #TxDaily where scheme_code = 'HSIVSS' and appsource=@appsource and transaction_dtm = @temp_dtm
		select @s_Total  = isnull(SUM(TxCount),0) from #TxDaily where appsource=@appsource and transaction_dtm = @temp_dtm


	
		-- Save the # of SP using IVRS,HKMA,HKDA by Profession into Temp Table
		--
		if (@appsource='IVRS' or @appsource='HKMA IH')
		begin
		update #SP	set			
			ENU_1=@sp_ENU,
			RCM_1=@sp_RCM,
			RCP_1=@sp_RCP,
			RDT_1=@sp_RDT,
			RMP_1=@sp_RMP,
			RMT_1=@sp_RMT,
			RNU_1=@sp_RNU,
			ROP_1=@sp_ROP,				-- added CRE11-024-02
			ROT_1=@sp_ROT,
			RPT_1=@sp_RPT,
			RRD_1=@sp_RRD,
			Total_1=@sp_Total
		where report_type=case when (@appsource='IVRS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)

		-- Save the # of Transaction by Profession into Temp Table
		update #T set
			ENU_1=@t_ENU,
			RCM_1=@t_RCM,
			RCP_1=@t_RCP,
			RDT_1=@t_RDT,
			RMP_1=@t_RMP,
			RMT_1=@t_RMT,
			RNU_1=@t_RNU,
			ROP_1=@t_ROP,				-- added CRE11-024-02
			ROT_1=@t_ROT,
			RPT_1=@t_RPT,
			RRD_1=@t_RRD,
			Total_1=@t_Total
			where report_type=case when (@appsource='IVRS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)


		-- Save the # of Transaction by Scheme into Temp Table
		update #TranScheme set
			HCVS_1=@s_HCVS,
			CIVSS_1=@s_CIVSS,
			EVSS_1=@s_EVSS,
			RVP_1=@s_RVP,
			HSIVSS_1=@s_HSIVSS,
			Total_1=@s_Total
		where report_type=case when (@appsource='IVRS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
		end

		if (@appsource='HKMA IH' or @appsource='PCS')
		begin
		update #SP	set			
			ENU_2=@sp_ENU,
			RCM_2=@sp_RCM,
			RCP_2=@sp_RCP,
			RDT_2=@sp_RDT,
			RMP_2=@sp_RMP,
			RMT_2=@sp_RMT,
			RNU_2=@sp_RNU,
			ROP_2=@sp_ROP,				-- added CRE11-024-02
			ROT_2=@sp_ROT,
			RPT_2=@sp_RPT,
			RRD_2=@sp_RRD,
			Total_2=@sp_Total
			where report_type=case when (@appsource='PCS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
			
		
		-- Save the # of Transaction by Profession into Temp Table
		update #T set
			ENU_2=@t_ENU,
			RCM_2=@t_RCM,
			RCP_2=@t_RCP,
			RDT_2=@t_RDT,
			RMP_2=@t_RMP,
			RMT_2=@t_RMT,
			RNU_2=@t_RNU,
			ROP_2=@t_ROP,				-- added CRE11-024-02
			ROT_2=@t_ROT,
			RPT_2=@t_RPT,
			RRD_2=@t_RRD,
			Total_2=@t_Total
		where report_type=case when (@appsource='PCS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)


		-- Save the # of Transaction by Scheme into Temp Table
		update #TranScheme set
			HCVS_2=@s_HCVS,
			CIVSS_2=@s_CIVSS,
			EVSS_2=@s_EVSS,
			RVP_2=@s_RVP,
			HSIVSS_2=@s_HSIVSS,
			Total_2=@s_Total
		where report_type=case when (@appsource='PCS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
		end


	fetch next from cur_tran into @appsource
	end --loop for cursor of interface system
	
	close cur_tran
	deallocate cur_tran


		-- Statistics for getting voucher claim duration by using IVRS in second
		select @claim_max = max_value from RpteHSD0011_07_IVRSClaimDuration where report_dtm = @temp_dtm 
		select @claim_min = min_value from RpteHSD0011_07_IVRSClaimDuration where report_dtm = @temp_dtm 
		select @claim_avg = avg_value from RpteHSD0011_07_IVRSClaimDuration where report_dtm = @temp_dtm 
		select @claim_T = no_of_transaction from RpteHSD0011_07_IVRSClaimDuration where report_dtm = @temp_dtm 


	-- Duration Result of that day
		insert into #result
		(
			report_dtm,
			claim_max,
			claim_min,
			claim_avg,
			claim_T
		)
		values
		(
			CONVERT(VARCHAR(11), @temp_dtm, 106),
			isnull(@claim_max,'N/A'),
			isnull(@claim_min,'N/A'),
			isnull(@claim_avg,'N/A'),
			@claim_T
		)
		-- Increment to get next day's statistic
		select @temp_dtm = CONVERT(VARCHAR(11), dateadd(day, 1,@temp_dtm), 106)
	END

-- Process Cumulative Total

	INSERT INTO #SP (report_type,report_dtm) VALUES ('E','Cumulative Total')
	INSERT INTO #SP (report_type,report_dtm) VALUES ('P','Cumulative Total')
	UPDATE
		#SP
	SET
		ENU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ENU' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		RCM_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCM' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		RCP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCP' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		RDT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RDT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		RMP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMP' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		RMT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		RNU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RNU' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS' 
),
		ROP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROP' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),	-- added CRE11-024-02
		ROT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RPT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RPT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RRD_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RRD' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		Total_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Scheme_Code = 'HCVS' AND AppSource = 'IVRS')

	WHERE
		report_dtm = 'Cumulative Total' and report_type='E'
	UPDATE
		#SP
	SET
		ENU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ENU' AND  AppSource='externalws'
),
		RCM_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCM' AND  AppSource='externalws'
),
		RCP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCP' AND  AppSource='externalws'
),
		RDT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RDT' AND  AppSource='externalws'
),
		RMP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMP' AND  AppSource='externalws'
),
		RMT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMT' AND  AppSource='externalws'
),
		RNU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RNU' AND  AppSource='externalws'
),
		ROP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROP' AND  AppSource='externalws'
),	-- added CRE11-024-02
		ROT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROT' AND  AppSource='externalws'
),
		RPT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RPT' AND  AppSource='externalws'
),
		RRD_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RRD' AND  AppSource='externalws'
),
		Total_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE  AppSource='externalws')

	WHERE
		report_dtm = 'Cumulative Total' and report_type='E'

	UPDATE
		#SP
	SET
		ENU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ENU'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RCM_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCM'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RCP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCP'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RDT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RDT'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RMP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMP'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RMT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMT'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RNU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RNU'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		ROP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROP'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),	-- added CRE11-024-02
		ROT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROT'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RPT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RPT'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		RRD_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RRD'  AND  AppSource='externalws' and dataentry_by ='HKMA IH'
),
		Total_1 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE AppSource='externalws' and dataentry_by ='HKMA IH'
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='P'

		

	UPDATE
		#SP
	SET
		ENU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ENU' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
 ),
		RCM_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCM' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RCP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RCP' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RDT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RDT' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RMP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMP' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RMT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RMT' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RNU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RNU' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		ROP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROP' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),	-- added CRE11-024-02
		ROT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'ROT' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RPT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RPT' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		RRD_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE Service_Type = 'RRD' AND  AppSource='externalws' and dataentry_by ='HKDA IH'
),
		Total_2 = (SELECT COUNT(DISTINCT SP_ID) FROM #TxAll WHERE  AppSource='externalws' and dataentry_by ='HKDA IH'
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='P'




--
	
	
	INSERT INTO #T (report_type,report_dtm) VALUES ('E','Cumulative Total')
	INSERT INTO #T (report_type,report_dtm) VALUES ('P','Cumulative Total')
	

	UPDATE
		#T
	SET
		ENU_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ENU' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RCM_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCM' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RCP_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCP' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RDT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RDT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RMP_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMP' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RMT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RNU_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RNU' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		ROP_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROP' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),	-- added CRE11-024-02
		ROT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RPT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RPT' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		RRD_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RRD' AND Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		Total_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
)
	WHERE
			report_dtm = 'Cumulative Total' and report_type='E'

	UPDATE
		#T
	SET
		ENU_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ENU' AND AppSource='externalws'
),
		RCM_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCM' AND AppSource='externalws'
),
		RCP_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCP' AND AppSource='externalws'
),
		RDT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RDT' AND AppSource='externalws'
) ,
		RMP_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMP' AND AppSource='externalws'
),
		RMT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMT' and AppSource='externalws'
),
		RNU_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RNU' AND AppSource='externalws'
),
		ROP_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROP' AND AppSource='externalws'
),	-- added CRE11-024-02
		ROT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROT' AND AppSource='externalws'
),
		RPT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RPT' AND AppSource='externalws'
),
		RRD_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RRD' AND AppSource='externalws'
),
		Total_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE AppSource='externalws'
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='E'


	UPDATE
		#T
	SET
		ENU_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ENU' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RCM_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCM' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RCP_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCP' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RDT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RDT' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RMP_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMP' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RMT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMT' and AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RNU_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RNU' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		ROP_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROP' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),	-- added CRE11-024-02
		ROT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROT' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RPT_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RPT' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RRD_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RRD' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		Total_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE AppSource='externalws'and dataentry_by ='HKMA IH'
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='P'

	UPDATE
		#T
	SET
		ENU_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ENU' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RCM_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCM' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RCP_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RCP' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RDT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RDT' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RMP_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMP' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RMT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RMT' and AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RNU_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RNU' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		ROP_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROP' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),	-- added CRE11-024-02
		ROT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'ROT' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RPT_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RPT' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RRD_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Service_Type = 'RRD' AND AppSource='externalws'and dataentry_by ='HKDA IH'
) ,
		Total_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE AppSource='externalws'and dataentry_by ='HKDA IH'
)
	WHERE
			report_dtm = 'Cumulative Total' and report_type='P'


	INSERT INTO #TranScheme (report_type,report_dtm) VALUES ('E','Cumulative Total')
	INSERT INTO #TranScheme (report_type,report_dtm) VALUES ('P','Cumulative Total')
	


	UPDATE
		#TranScheme
	SET
		HCVS_1  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HCVS' AND AppSource = 'IVRS'
),
		CIVSS_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'CIVSS' AND AppSource = 'IVRS'
),
		EVSS_1  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'EVSS'  AND AppSource = 'IVRS'
),
		RVP_1	  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'RVP' AND AppSource = 'IVRS'
),
		HSIVSS_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HSIVSS' AND AppSource = 'IVRS'
),
		Total_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE AppSource = 'IVRS'
)
	WHERe report_dtm = 'Cumulative Total' and report_type='E'
	
	UPDATE
		#TranScheme
	SET
		HCVS_2  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HCVS' AND AppSource='externalws'
),
		CIVSS_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'CIVSS' AND AppSource='externalws'
),
		EVSS_2  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'EVSS' AND AppSource='externalws'
),
		RVP_2	  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'RVP'  AND AppSource='externalws'
),
		HSIVSS_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HSIVSS' AND AppSource='externalws'
),
		Total_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE AppSource='externalws'
)
	WHERE	report_dtm = 'Cumulative Total' and report_type='E'
	UPDATE
		#TranScheme
	SET
		HCVS_1  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HCVS' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		CIVSS_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'CIVSS' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		EVSS_1  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'EVSS' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		RVP_1	  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'RVP'  AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		HSIVSS_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HSIVSS' AND AppSource='externalws'and dataentry_by ='HKMA IH'
),
		Total_1 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE AppSource='externalws'and dataentry_by ='HKMA IH'
)
	WHERE report_dtm = 'Cumulative Total' and report_type='P'

	UPDATE
		#TranScheme
	SET
		HCVS_2  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HCVS' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		CIVSS_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'CIVSS' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		EVSS_2  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'EVSS' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		RVP_2	  = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'RVP'  AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		HSIVSS_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE Scheme_Code = 'HSIVSS' AND AppSource='externalws'and dataentry_by ='HKDA IH'
),
		Total_2 = (SELECT ISNULL(SUM(TxCount),0) FROM #TxAll WHERE   AppSource='externalws' and dataentry_by ='HKDA IH'
)
	WHERE report_dtm = 'Cumulative Total' and report_type='P'


-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- To Excel sheet: Content
-- ---------------------------------------------

	SELECT
		'Report Generation Time: ' + CONVERT(varchar, @Report_Dtm, 111) + ' ' + CONVERT(varchar(5), @Report_Dtm, 114)
-- ---------------------------------------------
-- To Excel sheet: Summary 
-- ---------------------------------------------
select * from #Summary

-- ---------------------------------------------
-- To Excel sheet: eHSD0011-01: No. of SP involved by Profession
-- ---------------------------------------------

	select report_dtm,	ENU_1 ,		RCM_1,
				RCP_1,	RDT_1 ,		RMP_1,
				RMT_1,	RNU_1 ,		ROP_1,
				ROT_1,	RPT_1 ,		RRD_1,
				Total_1,
				ENU_2 ,	RCM_2,
				RCP_2,	RDT_2 ,		RMP_2,
				RMT_2,	RNU_2 ,		ROP_2,
				ROT_2,	RPT_2 ,		RRD_2,
				Total_2 from #SP where report_type='E'
	
	
-- ---------------------------------------------
-- To Excel sheet: eHSD0011-02_PCS: No. of SP involved in PCS involved by Profession
-- ---------------------------------------------	

	select report_dtm,	ENU_1 ,		RCM_1,
				RCP_1,	RDT_1 ,		RMP_1,
				RMT_1,	RNU_1 ,		ROP_1,
				ROT_1,	RPT_1 ,		RRD_1,
				Total_1,
				ENU_2 ,	RCM_2,
				RCP_2,	RDT_2 ,		RMP_2,
				RMT_2,	RNU_2 ,		ROP_2,
				ROT_2,	RPT_2 ,		RRD_2,
				Total_2 from #SP where report_type='P'

-- ---------------------------------------------
-- To Excel sheet: eHSD0011-03: No. of Transaction by Profession
-- ---------------------------------------------
	
	select report_dtm,	ENU_1 ,		RCM_1,
				RCP_1,	RDT_1 ,		RMP_1,
				RMT_1,	RNU_1 ,		ROP_1,
				ROT_1,	RPT_1 ,		RRD_1,
				Total_1,
				ENU_2 ,	RCM_2,
				RCP_2,	RDT_2 ,		RMP_2,
				RMT_2,	RNU_2 ,		ROP_2,
				ROT_2,	RPT_2 ,		RRD_2,
				Total_2 from #T where report_type='E'
-- ---------------------------------------------
-- To Excel sheet: eHSD0011-04:  No. of Transaction involved in PCS by Profession
-- ---------------------------------------------
	
	select report_dtm,	ENU_1 ,		RCM_1,
				RCP_1,	RDT_1 ,		RMP_1,
				RMT_1,	RNU_1 ,		ROP_1,
				ROT_1,	RPT_1 ,		RRD_1,
				Total_1,
				ENU_2 ,	RCM_2,
				RCP_2,	RDT_2 ,		RMP_2,
				RMT_2,	RNU_2 ,		ROP_2,
				ROT_2,	RPT_2 ,		RRD_2,
				Total_2 from #T where report_type='P'

---- ---------------------------------------------
---- To Excel sheet: eHSD0011-03:No. of Voucher Claimed by Profession
---- ---------------------------------------------
--
--	select report_dtm,	ENU_1 ,		RCM_1,
--				RCP_1,	RDT_1 ,		RMP_1,
--				RMT_1,	RNU_1 ,		ROP_1,
--				ROT_1,	RPT_1 ,		RRD_1,
--				Total_1,
--				ENU_2 ,	RCM_2,
--				RCP_2,	RDT_2 ,		RMP_2,
--				RMT_2,	RNU_2 ,		ROP_2,
--				ROT_2,	RPT_2 ,		RRD_2,
--				Total_2 from #C where report_type='E'
--
---- ---------------------------------------------
---- To Excel sheet: eHSD0011-03: No. of Voucher involved in PCS Claimed by Profession
---- ---------------------------------------------
--
--	select report_dtm,	ENU_1 ,		RCM_1,
--				RCP_1,	RDT_1 ,		RMP_1,
--				RMT_1,	RNU_1 ,		ROP_1,
--				ROT_1,	RPT_1 ,		RRD_1,
--				Total_1,
--				ENU_2 ,	RCM_2,
--				RCP_2,	RDT_2 ,		RMP_2,
--				RMT_2,	RNU_2 ,		ROP_2,
--				ROT_2,	RPT_2 ,		RRD_2,
--				Total_2 from #C where report_type='P'

-- ---------------------------------------------
-- To Excel sheet: eHSD0011-05: No. of Transaction by Scheme
-- ---------------------------------------------

	select report_dtm ,
		HCVS_1 ,  
		CIVSS_1,  
		EVSS_1,   
		RVP_1,    
		HSIVSS_1, 
		Total_1,  
		HCVS_2,   
		CIVSS_2,  
		EVSS_2,   
		RVP_2,    
		HSIVSS_2, 
		Total_2   from #TranScheme where report_type='E'

-- ---------------------------------------------
-- To Excel sheet: eHSD0011-06:  No. of Transaction involved by PCS by Scheme
-- ---------------------------------------------

	select  report_dtm ,
		HCVS_1 ,  
		CIVSS_1,  
		EVSS_1,   
		RVP_1,    
		HSIVSS_1, 
		Total_1,  
		HCVS_2,   
		CIVSS_2,  
		EVSS_2,   
		RVP_2,    
		HSIVSS_2, 
		Total_2    from #TranScheme where report_type='P'


-- ---------------------------------------------
-- To Excel sheet: eHSD0011-07: Claim Duration (in second)
-- ---------------------------------------------

	select * from #result


-- =============================================
-- Finalizer
-- =============================================

	drop table #TxAll
	drop table #TxDaily
	drop table #SP
	drop table #T
	drop table #C
	drop table #result
	drop table #Summary
	drop table #TranScheme

SET NOCOUNT OFF	

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0011_Claim_By_Other_Mean_Stat_Read] TO HCVU
GO
