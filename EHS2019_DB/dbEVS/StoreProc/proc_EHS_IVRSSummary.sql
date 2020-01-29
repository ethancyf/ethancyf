IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_IVRSSummary]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_IVRSSummary]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
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

create PROCEDURE [dbo].[proc_EHS_IVRSSummary]
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
	

	-- Transaction Information
	create table #transaction
	(
		appsource	varchar(20), -- added CRE11-022
		transaction_id	char(20),
		transaction_dtm	datetime,
		voucher_claim smallint,
		service_type varchar(5),
		create_dtm	datetime,
		record_status char(1),
		sp_id char(8),
		display_seq smallint,
		scheme_code varchar(10) -- added CRE11-022
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
	
	
	-- Begin Retrieve Data for IVRS and PCS and for validated transaction only
	insert into #transaction
	(
		appsource,
		transaction_id,
		transaction_dtm,
		voucher_claim,
		service_type,
		create_dtm,
		record_status,					
		sp_id,
		display_seq,
		scheme_code
	)
	select	case  when (VT.sourceapp='IVRS') then 'IVRS' else 'PCS'  end,
			VT.transaction_id,
			VT.transaction_dtm,
			TD.unit,
			rtrim(VT.service_type),
			VT.create_dtm,
			VT.record_status,
			VT.sp_id,
			VT.Practice_display_seq,
			rtrim(VT.scheme_code)
	from VoucherTransaction VT
	inner join TransactionDetail TD 
		ON VT.Transaction_ID = TD.Transaction_ID
	where VT.sourceapp in ('IVRS', 'externalws' )	
	and VT.transaction_dtm between @start_dtm and @end_dtm
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))

	--  Retrieve Data for HKMA and HKDA
	insert into #transaction
	(
		appsource,
		transaction_id,
		transaction_dtm,
		voucher_claim,
		service_type,
		create_dtm,
		record_status,
		sp_id,
		display_seq,
		scheme_code
	)
	select	VT.dataentry_by,
			VT.transaction_id,
			VT.transaction_dtm,
			TD.unit,
			rtrim(VT.service_type),
			VT.create_dtm,
			VT.record_status,	
			VT.sp_id,
			VT.Practice_display_seq,
			rtrim(VT.scheme_code)
	from VoucherTransaction VT
	inner join TransactionDetail TD 
		ON VT.Transaction_ID = TD.Transaction_ID
	where VT.sourceapp = 'externalws' and isupload='Y'
	and VT.dataentry_by in ('HKMA IH' , 'HKDA IH')
	and VT.transaction_dtm between @start_dtm and @end_dtm
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))


	--Get total Summary for WEB-FULL
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'WEB (FULL)',' ', cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM VoucherTransaction
	 WHERE  SourceApp in ('WEB', 'WEB-FULL') AND Transaction_Dtm < @end_dtm and (manual_reimburse='N' or manual_reimburse is null)
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
	
	--Get total Summary for WEB-TEXT
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'WEB (TEXT ONLY)',' ', cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20))FROM VoucherTransaction
	 WHERE  SourceApp = 'WEB-TEXT' AND Transaction_Dtm < @end_dtm and (manual_reimburse='N' or manual_reimburse is null)
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))


	--Get total summary for IVRS
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'IVRS',' ',cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM VoucherTransaction
	 WHERE Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))

	--Get total summary for PCS
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'PCS',' ', cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM VoucherTransaction
	 WHERE sourceapp='externalws'  AND Transaction_Dtm < @end_dtm and (manual_reimburse='N' or manual_reimburse is null)
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))

	--Get total summary for HCVU
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT 'HCVU',' ', cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM VoucherTransaction
	 WHERE sourceapp not in ('externalws', 'IVRS')  AND Transaction_Dtm < @end_dtm and manual_reimburse='Y'
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))

	--Total Transaction
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' ','Total Transaction', cast(isnull(COUNT(1),0) as varchar(20)), ' ' FROM VoucherTransaction
	 WHERE  Transaction_Dtm < @end_dtm 	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
		
	--insert dummy line
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' ',' ', ' ',' '	 

	insert into #Summary (appsource, dummyspace,tol_no_trans, tol_no_SP )
	SELECT 'PCS', ' ',' ',' '
	
	--Get total summary for HKMA
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' -HKMA IH', ' ', cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM VoucherTransaction
	 WHERE sourceapp='externalws' and isupload='Y' and dataentry_by='HKMA IH' and Transaction_Dtm < @end_dtm
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))

	--Get total summary for HKDA
	insert into #Summary (appsource,dummyspace, tol_no_trans, tol_no_SP )
	SELECT ' -HKDA IH', ' ',cast(isnull(COUNT(1),0) as varchar(20)), cast(isnull(count(distinct sp_id),0) as varchar(20)) FROM VoucherTransaction
	 WHERE sourceapp='externalws' and isupload='Y' and dataentry_by='HKDA IH' and Transaction_Dtm < @end_dtm
	And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
		
	

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
		select distinct appsource  from #transaction 
		open  cur_tran
		fetch cur_tran into @appsource
		while @@Fetch_status=0
		begin
		
		-- # of SP using IVRS,HKMA,HKDA by Profession
		
		select @sp_ENU = isnull(count(distinct sp_id),0) from #transaction where service_type = 'ENU' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RCM = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RCM' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RCP = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RCP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RDT = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RDT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RMP = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RMP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RMT = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RMT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RNU = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RNU' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_ROP = isnull(count(distinct sp_id),0) from #transaction where service_type = 'ROP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)	-- added CRE11-024-02
		select @sp_ROT = isnull(count(distinct sp_id),0) from #transaction where service_type = 'ROT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RPT = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RPT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_RRD = isnull(count(distinct sp_id),0) from #transaction where service_type = 'RRD' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @sp_Total = isnull(count(distinct sp_id),0) from #transaction where appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)

		-- # of Transaction using IVRS,HKMA,HKDA by Profession
		select @t_ENU = isnull(count(*),0) from #transaction where service_type = 'ENU' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RCM = isnull(count(*),0) from #transaction where service_type = 'RCM' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RCP = isnull(count(*),0) from #transaction where service_type = 'RCP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RDT = isnull(count(*),0) from #transaction where service_type = 'RDT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RMP = isnull(count(*),0) from #transaction where service_type = 'RMP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RMT = isnull(count(*),0) from #transaction where service_type = 'RMT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RNU = isnull(count(*),0) from #transaction where service_type = 'RNU' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_ROP = isnull(count(*),0) from #transaction where service_type = 'ROP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)	-- added CRE11-024-02
		select @t_ROT = isnull(count(*),0) from #transaction where service_type = 'ROT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RPT = isnull(count(*),0) from #transaction where service_type = 'RPT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_RRD = isnull(count(*),0) from #transaction where service_type = 'RRD' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @t_Total = isnull(count(*),0) from #transaction where appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)

		-- # of Voucher Claimed using IVRS, HKMA, HKDA by Profession
		select @c_ENU = isnull(sum(voucher_claim),0) from #transaction where service_type = 'ENU' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RCM = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RCM' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RCP = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RCP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RDT = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RDT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RMP = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RMP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RMT = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RMT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RNU = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RNU' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_ROP = isnull(sum(voucher_claim),0) from #transaction where service_type = 'ROP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)	-- added CRE11-024-02
		select @c_ROT = isnull(sum(voucher_claim),0) from #transaction where service_type = 'ROT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RPT = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RPT' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_RRD = isnull(sum(voucher_claim),0) from #transaction where service_type = 'RRD' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @c_Total = isnull(sum(voucher_claim),0) from #transaction where  appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)

		-- # of Transaction using IVRS, HKMA, HKDA by Scheme
		select @s_HCVS  = isnull(count(*),0) from #transaction where scheme_code = 'HCVS' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @s_CIVSS = isnull(count(*),0) from #transaction where scheme_code = 'CIVSS' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @s_EVSS  = isnull(count(*),0) from #transaction where scheme_code = 'EVSS' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @s_RVP   = isnull(count(*),0) from #transaction where scheme_code = 'RVP' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)
		select @s_HSIVSS = isnull(count(*),0) from #transaction where scheme_code = 'HSIVSS' and appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)		
		select @s_Total  = isnull(count(*),0) from #transaction where appsource=@appsource and transaction_dtm between @temp_dtm and dateadd(day, 1, @temp_dtm)


	
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
		-- Save the # of Voucher Claimed by Profession into Temp Table
--		update #C set
--			ENU_1=@c_ENU,
--			RCM_1=@c_RCM,
--			RCP_1=@c_RCP,
--			RDT_1=@c_RDT,
--			RMP_1=@c_RMP,
--			RMT_1=@c_RMT,
--			RNU_1=@c_RNU,
--			ROP_1=@c_ROP,				-- added CRE11-024-02
--			ROT_1=@c_ROT,
--			RPT_1=@c_RPT,
--			RRD_1=@c_RRD,
--			Total_1=@c_Total
--			where report_type=case when (@appsource='IVRS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)

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
--		if (@appsource=='HKMA IH')
--		begin
--		update #SP	set			
--			ENU_1=@sp_ENU,
--			RCM_1=@sp_RCM,
--			RCP_1=@sp_RCP,
--			RDT_1=@sp_RDT,
--			RMP_1=@sp_RMP,
--			RMT_1=@sp_RMT,
--			RNU_1=@sp_RNU,
--			ROP_1=@sp_ROP,				-- added CRE11-024-02
--			ROT_1=@sp_ROT,
--			RPT_1=@sp_RPT,
--			RRD_1=@sp_RRD,
--			Total_1=@sp_Total
--		where report_type='P' and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
--			
--		
--		-- Save the # of Transaction by Profession into Temp Table
--		update #T set
--			ENU_1=@t_ENU,
--			RCM_1=@t_RCM,
--			RCP_1=@t_RCP,
--			RDT_1=@t_RDT,
--			RMP_1=@t_RMP,
--			RMT_1=@t_RMT,
--			RNU_1=@t_RNU,
--			ROP_1=@t_ROP,				-- added CRE11-024-02
--			ROT_1=@t_ROT,
--			RPT_1=@t_RPT,
--			RRD_1=@t_RRD,
--			Total_1=@t_Total
--		where report_type='P' and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
--		-- Save the # of Voucher Claimed by Profession into Temp Table
--		update #C set
--			ENU_1=@c_ENU,
--			RCM_1=@c_RCM,
--			RCP_1=@c_RCP,
--			RDT_1=@c_RDT,
--			RMP_1=@c_RMP,
--			RMT_1=@c_RMT,
--			RNU_1=@c_RNU,
--			ROP_1=@c_ROP,				-- added CRE11-024-02
--			ROT_1=@c_ROT,
--			RPT_1=@c_RPT,
--			RRD_1=@c_RRD,
--			Total_1=@c_Total
--		where report_type='P' and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
--
--		-- Save the # of Transaction by Scheme into Temp Table
--		update #TranScheme set
--			HCVS_1=@s_HCVS,
--			CIVSS_1=@s_CIVSS,
--			EVSS_1=@s_EVSS,
--			RVP_1=@s_RVP,
--			HSIVSS_1=@s_HSIVSS,
--			Total_1=@s_Total
--		where report_type='P' and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)
--		end
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
		-- Save the # of Voucher Claimed by Profession into Temp Table
--		update #C set
--			ENU_2=@c_ENU,
--			RCM_2=@c_RCM,
--			RCP_2=@c_RCP,
--			RDT_2=@c_RDT,
--			RMP_2=@c_RMP,
--			RMT_2=@c_RMT,
--			RNU_2=@c_RNU,
--			ROP_2=@c_ROP,				-- added CRE11-024-02
--			ROT_2=@c_ROT,
--			RPT_2=@c_RPT,
--			RRD_2=@c_RRD,
--			Total_2=@c_Total
--		where report_type=case when (@appsource='PCS') then 'E' else 'P' end and report_dtm=CONVERT(VARCHAR(11), @temp_dtm, 106)

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
		select @claim_max = max_value from _IVRSClaimDurationSummary where report_dtm = @temp_dtm 
		select @claim_min = min_value from _IVRSClaimDurationSummary where report_dtm = @temp_dtm 
		select @claim_avg = avg_value from _IVRSClaimDurationSummary where report_dtm = @temp_dtm 
		select @claim_T = no_of_transaction from _IVRSClaimDurationSummary where report_dtm = @temp_dtm 


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
		ENU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm  And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I')))

	WHERE
		report_dtm = 'Cumulative Total' and report_type='E'
	UPDATE
		#SP
	SET
		ENU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND  sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE  sourceapp='externalws' and isupload='Y' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I')))

	WHERE
		report_dtm = 'Cumulative Total' and report_type='E'

	UPDATE
		#SP
	SET
		ENU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ENU'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCM'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCP'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RDT'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMP'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMT'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RNU'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROP'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROT'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RPT'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RRD'  AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_1 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE   sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='P'

		

	UPDATE
		#SP
	SET
		ENU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
 ),
		RCM_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_2 = (SELECT COUNT(DISTINCT SP_ID) FROM VoucherTransaction WHERE  sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='P'




--
	
	
	INSERT INTO #T (report_type,report_dtm) VALUES ('E','Cumulative Total')
	INSERT INTO #T (report_type,report_dtm) VALUES ('P','Cumulative Total')
	

	UPDATE
		#T
	SET
		ENU_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE
			report_dtm = 'Cumulative Total' and report_type='E'

	UPDATE
		#T
	SET
		ENU_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
) ,
		RMP_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMT' and sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='E'


	UPDATE
		#T
	SET
		ENU_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMT' and sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE
		report_dtm = 'Cumulative Total' and report_type='P'

	UPDATE
		#T
	SET
		ENU_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCM_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RCP_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RDT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMP_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RMT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RMT' and sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RNU_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		ROP_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),	-- added CRE11-024-02
		ROT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RPT_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RRD_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
) ,
		Total_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE
			report_dtm = 'Cumulative Total' and report_type='P'

--
	
	
--	INSERT INTO #C (report_type,report_dtm) VALUES ('E','Cumulative Total')
--	INSERT INTO #C (report_type,report_dtm) VALUES ('P','Cumulative Total')
--	
--		
--	UPDATE
--		#C
--	SET
--		ENU_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RCM_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RCP_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RDT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RMP_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RMT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RNU_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		ROP_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),	-- added CRE11-024-02
--		ROT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RPT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		RRD_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm),
--		Total_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm)
--	WHERE	report_dtm = 'Cumulative Total' and report_type='E'
--	
--	UPDATE
--		#C
--	SET
--		ENU_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RCM_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RCP_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RDT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RMP_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RMT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RNU_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		ROP_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),	-- added CRE11-024-02
--		ROT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RPT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		RRD_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm),
--		Total_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm)
--	WHERE report_dtm = 'Cumulative Total' and report_type='E'
--
--UPDATE
--		#C
--	SET
--		ENU_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RCM_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RCP_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RDT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RMP_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RMT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RNU_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		ROP_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),	-- added CRE11-024-02
--		ROT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RPT_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		RRD_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm),
--		Total_1 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm)
--	WHERE
--		report_dtm = 'Cumulative Total' and report_type='P'
--UPDATE
--		#C
--	SET
--		ENU_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ENU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RCM_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCM' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RCP_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RCP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RDT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RDT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RMP_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RMT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RMT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RNU_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RNU' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		ROP_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROP' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),	-- added CRE11-024-02
--		ROT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'ROT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RPT_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RPT' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		RRD_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE Service_Type = 'RRD' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm),
--		Total_2 = (SELECT ISNULL(SUM(Voucher_Before_Claim - Voucher_After_Claim), 0) FROM VoucherTransaction WHERE sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm)
--	WHERE report_dtm = 'Cumulative Total' and report_type='P'
----

	INSERT INTO #TranScheme (report_type,report_dtm) VALUES ('E','Cumulative Total')
	INSERT INTO #TranScheme (report_type,report_dtm) VALUES ('P','Cumulative Total')
	


	UPDATE
		#TranScheme
	SET
		HCVS_1  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		CIVSS_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'CIVSS' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		EVSS_1  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'EVSS' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RVP_1	  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'RVP' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		HSIVSS_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HSIVSS' AND Scheme_Code = 'HCVS' AND SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE  SourceApp = 'IVRS' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERe report_dtm = 'Cumulative Total' and report_type='E'
	
	UPDATE
		#TranScheme
	SET
		HCVS_2  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		CIVSS_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'CIVSS' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		EVSS_2  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'EVSS' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RVP_2	  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'RVP'  AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		HSIVSS_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HSIVSS' AND sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE   sourceapp='externalws' and isupload='Y'  AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE	report_dtm = 'Cumulative Total' and report_type='E'
	UPDATE
		#TranScheme
	SET
		HCVS_1  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		CIVSS_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'CIVSS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		EVSS_1  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'EVSS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RVP_1	  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'RVP'  AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		HSIVSS_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HSIVSS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_1 = (SELECT COUNT(1) FROM VoucherTransaction WHERE   sourceapp='externalws' and isupload='Y' and dataentry_by ='HKMA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
)
	WHERE report_dtm = 'Cumulative Total' and report_type='P'

	UPDATE
		#TranScheme
	SET
		HCVS_2  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HCVS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		CIVSS_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'CIVSS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		EVSS_2  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'EVSS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		RVP_2	  = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'RVP'  AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		HSIVSS_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE Scheme_Code = 'HSIVSS' AND sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
),
		Total_2 = (SELECT COUNT(1) FROM VoucherTransaction WHERE   sourceapp='externalws' and isupload='Y' and dataentry_by ='HKDA IH' AND Transaction_Dtm < @end_dtm And Record_Status Not in ('I', 'D','W') And (Invalidation IS NULL OR Invalidation Not in ('I'))
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

	drop table #transaction
	drop table #SP
	drop table #T
	drop table #C
	drop table #result
	drop table #Summary
	drop table #TranScheme

SET NOCOUNT OFF	

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_IVRSSummary] TO HCVU
GO
