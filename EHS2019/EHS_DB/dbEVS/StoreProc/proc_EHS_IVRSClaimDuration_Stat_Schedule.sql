IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_IVRSClaimDuration_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_IVRSClaimDuration_Stat_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Change table from [_IVRSClaimDurationSummary] to [RpteHSD0011_07_IVRSClaimDuration]
--					Delete old history data from [RpteHSD0011_07_IVRSClaimDuration] (< 14 days)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	31 Dec 2009
-- Description:		Update using the corresponding Audit Log Table for different year
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	15 Oct 2009
-- Description:		Update using new Audit Log entry
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 10 Feb 2009
-- Description:	Statistics for getting voucher claim duration by using IVRS
--				1. Save related data to temp table (_IVRSClaimDurationSummary)
--				990210 - 00001	Search Voucher Account
--				990203 - 00002	Complete Claim
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_IVRSClaimDuration_Stat_Schedule]
	@start_time datetime = null
	, @end_time datetime = null
AS
BEGIN

	SET NOCOUNT ON;
	
	EXEC [proc_SymmetricKey_open]

	--declare @start_time as datetime
	--declare @end_time as datetime
	declare @Year		smallint	-- for identifying the audit log table
	
	declare @sp_id char(8)
	declare @log_id char(5)
	declare @call_ID varchar(MAX)
	declare @system_dtm datetime
	declare @temp_system_dtm datetime
	declare @temp_log_id char(5)
	declare @temp_call_id varchar(MAX)
	declare @end char(1)
	declare @second as int
	declare @min as int
	declare @hour as int
	declare @function_code char(6)
	declare @temp_function_code char(6)

	declare @max as char(8)
	declare @mini as char(8)
	declare @avg as char(8)

	if @start_time is null
		select @start_time = CONVERT(VARCHAR(11), dateadd(day, -1, GETDATE()), 106) + ' 00:00'
	
	if @end_time is null
		select @end_time = CONVERT(VARCHAR(11), GETDATE(), 106) + ' 00:00'

	set @Year = CONVERT(varchar(2), @start_time, 12)	-- Extract the Calendar Year 

	create table #temp
	(
		system_dtm datetime,
		sp_id char(8),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		call_id varchar(Max)		
	)

	create table #result
	(
		start_time datetime,
		end_time datetime,
		second int,		
		call_id varchar(Max)
	)


	IF @Year = '09' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp09 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '10' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp10 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '11' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp11 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '12' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp12 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '13' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp13 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '14' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp14 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '15' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp15 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '16' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp16 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '17' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp17 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '18' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp18 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '19' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp19 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '20' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp20 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '21' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp21 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '22' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp22 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '23' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp23 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '24' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp24 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '25' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp25 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '26' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp26 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '27' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp27 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '28' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			function_code,
			log_id,
			description,
			call_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_User_ID)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_callconnectionID))
		from auditlogivrshcsp28 WITH (NOLOCK)
		where (EncryptByKey(KEY_GUID('sym_Key'), '990210') = E_Function_Code and
		EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID) or 
		(EncryptByKey(KEY_GUID('sym_Key'), '990203') = E_Function_Code and
			EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID)		
		and system_dtm between @start_time and @end_time
	END


	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_call_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
	select system_dtm, sp_id, log_id, call_id from #temp
	order by sp_id, call_id, system_dtm desc

	OPEN record
	FETCH next FROM record INTO @system_dtm ,@sp_id, @log_id, @call_id
	WHILE @@Fetch_status = 0
	BEGIN

		select @temp_log_id = @log_id
		select @temp_system_dtm = @system_dtm
		
		if @temp_log_id = '00002'
			begin
				select @end = 'Y'	
				select @temp_call_id = @call_id	
			end
		else if @temp_log_id = '00001'
			begin
				if @end = 'Y'
					begin
						select @end = 'N'
					end
				else
					begin
						delete from #temp
						where system_dtm = @temp_system_dtm
						and log_id = '00001'
					end
			end

	FETCH record INTO @system_dtm ,@sp_id, @log_id, @call_id
	end

	CLOSE record
	DEALLOCATE record

	
	select @temp_system_dtm = null
	select @temp_call_id = null

	DECLARE record CURSOR FOR
	select system_dtm, sp_id, log_id, call_id from #temp
	order by sp_id, call_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @system_dtm ,@sp_id, @log_id, @call_id
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id = '00001'
		begin
			select @temp_system_dtm = @system_dtm
			select @temp_call_id = @call_id
		end
		else if @log_id = '00002'
		begin 
			if @temp_system_dtm is not null
			begin
				if @temp_call_id is not null
				begin 
					if @temp_call_id = @call_id
					begin
							insert into #result
							(
								start_time,
								end_time,
								second,
								call_id
							)
							select	@temp_system_dtm, @system_dtm, 
									datediff(second, @temp_system_dtm,@system_dtm),
									@call_id

							select @temp_system_dtm = NULL
					end
				end			
			end
		END
		FETCH record INTO @system_dtm ,@sp_id, @log_id, @call_id
	end

	CLOSE record
	DEALLOCATE record

	
	-- Delete today data
	DELETE FROM [RpteHSD0011_07_IVRSClaimDuration] WHERE report_dtm = @start_time

	-- All claim duration 
	insert into RpteHSD0011_07_IVRSClaimDuration
	(
		system_dtm,
		report_dtm,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	select	getdate(),
			@start_time,	
			max(second), 
			min(second), 
			avg(second),
			count(*)
	from #result

	-- Delete old history data
	DELETE FROM [RpteHSD0011_07_IVRSClaimDuration] WHERE report_dtm < DATEADD(DD, -14, @end_time)
	
	--select * from #result
	--order by second desc

	
	EXEC [proc_SymmetricKey_close]
	
	drop table #temp
	drop table #result


END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_IVRSClaimDuration_Stat_Schedule] TO HCVU
GO
