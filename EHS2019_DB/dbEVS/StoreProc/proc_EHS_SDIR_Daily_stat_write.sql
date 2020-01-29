IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_SDIR_Daily_stat_write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_SDIR_Daily_stat_write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified By:		Mattie LO
-- Modified date: 30 December 2009
-- Description:	handle the cross year issue of reading the table [AuditLogVR99] from Year 2009 to 2028
-- =============================================

-- =============================================
-- Author:		Mattie LO
-- Create date: 21 October 2009
-- Description:	Daily statistics on SDIR, # of Search in total, # of Search by distinct session_ID and # of distinct session_ID
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE Procedure [dbo].[proc_EHS_SDIR_Daily_stat_write]
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @system_Dtm			datetime
	DECLARE @start_Dtm			datetime
	DECLARE @end_Dtm			datetime
	DECLARE @searchCnt			integer
	DECLARE @searchBySessionCnt integer
	DECLARE @sessionCnt			integer
	DECLARE @year				varchar(2)				
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @system_Dtm = getdate()
	SET @start_Dtm = convert(varchar(12), dateadd(dd, -1, getdate()), 106) + ' 00:00:00'
	SET @end_Dtm = convert(varchar(12), dateadd(dd, -1, getdate()), 106) + ' 23:59:59'
	SET @year = convert(varchar(2), @start_Dtm, 12)

OPEN SYMMETRIC KEY sym_Key  DECRYPTION BY ASYMMETRIC KEY asym_Key

CREATE TABLE #tmplog
(
	system_dtm		datetime,
	log_ID			varchar(5)		COLLATE DATABASE_DEFAULT,
	session_ID		varchar(MAX)
)
CREATE NONCLUSTERED INDEX IX1_tmplog ON #tmplog(system_dtm)

if @year = '09'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr09 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end
else if @year = '10'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr10 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '11'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr11 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '12'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr12 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '13'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr13 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '14'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr14 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '15'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr15 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end
else if @year = '16'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr16 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end		
else if @year = '17'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr17 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '18'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr18 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '19'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr19 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '20'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr20 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '21'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr21 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '22'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr22 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '23'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr23 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '24'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr24 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '25'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr25 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '26'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr26 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '27'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr27 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
else if @year = '28'
begin
	insert into #tmplog
	(	system_dtm,
		log_ID, 
		session_ID
	)
	(Select
		system_dtm
		,convert(varchar(5), DecryptByKey(e_log_ID))
		,convert(varchar(MAX), DecryptByKey(e_session_ID)) 
	from auditlogvr28 
		where 
	e_function_code = EncryptByKey(KEY_GUID('sym_Key'), '040101')
	and system_dtm between @start_Dtm and @end_Dtm)
end	
--and (
--	 e_log_id = EncryptByKey(KEY_GUID('sym_Key'), '00005')
--	or e_log_id = EncryptByKey(KEY_GUID('sym_Key'), '00010')
--)

SET @searchCnt = (select count(1) from #tmplog where log_ID = '00005' or log_ID = '00010')
SET @searchBySessionCnt = (select count(distinct session_ID) from #tmplog where log_ID = '00005' or log_ID = '00010')
SET @sessionCnt = (select count(distinct session_ID) from #tmplog)

-- =============================================
-- Final Result to Table
-- =============================================
INSERT INTO [_EHS_SDIR_Daily_stat](
			[system_dtm],
			[report_dtm],
			[total_search],
			[total_search_bySessionID],
			[total_sessionID]	)
(select	@system_Dtm	as system_Dtm,
		@start_Dtm as report_Dtm,
		@searchCnt as total_search, 
		@searchBySessionCnt as total_search_bySessionID,
		@sessionCnt as total_sessionID
)
drop table #tmplog

END

GO

GRANT EXECUTE ON [dbo].[proc_EHS_SDIR_Daily_stat_write] TO HCVU
GO
