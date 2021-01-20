IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_ClaimDuration_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_ClaimDuration_Stat_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 June 2010
-- Description:		(1) Add audit log for SmartIC enhancement
--					(2) Exclude the SmartIC log
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	25 Jan 2010
-- Description:		Fix bug on Reading Audit Log
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	30 Dec 2009
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
-- Description:	Statistics for getting voucher claim duration
--				1. Save related data to temp table (_ClaimDurationSummary)
-- 				function code 	020201	Claim Voucher (Full Version)
--				 				020202	Claim Voucher (Text Only Version)
-- 				log id			00001	Search Voucher Account
--								00014	Complete Claim
--								00017	Print English Consent Form
--								00018	Print Chinese Consent Form
--								00030	Press Process To Claim
--								00028	Print Eng Temp Account Creation Form
--								00029	Print Chi Temp Account Creation Form
--								00025	Complete Create Temporary Account 
--								00020	Confirm Collect Consent
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_ClaimDuration_Stat_Schedule]
	@start_time datetime = null
	, @end_time datetime = null
AS
BEGIN
	-----------------------------------------------------------------
	-- function code 020201 Claim Voucher (Full Version)
	--				 020202	Claim Voucher (Text Only Version)
	-- log id
	--		[Claim]
	--			00002	[BEGIN] Search EHSAccount Start
	--			00006	[BEGIN] Pre-filled Search Start
	--			00018	[BEGIN] Claim for same patient
	--			00017	[END] Complete Claim
	--		[Check include Account Creation for Claim]
	--			00040	Proceed to Claim
	--		[Print]
	--			00023	Form Printed
	--		[Creation]
	--			00025	[BEGIN] Create Account Pressed
	--			00028	[BEGIN] Confirm Modify Account
	--			00055	Create Acccount By SmartID
	--			00038	[END] Complete Account Creation
	-----------------------------------------------------------------
	SET NOCOUNT ON;


EXEC [proc_SymmetricKey_open]

	--declare @start_time as datetime
	--declare @end_time as datetime
	declare @Year		smallint	-- for identifying the audit log table
	
	declare @sp_id char(8)
	declare @data_entry varchar(20)
	declare @log_id char(5)
	declare @session_id varchar(MAX)
	declare @system_dtm datetime
	declare @temp_system_dtm datetime
	declare @temp_log_id char(5)
	declare @temp_session_id varchar(MAX)
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


	-- Target Log Entry (Claim)
	create table #temp
	(
		system_dtm datetime,
		sp_id char(8),
		data_entry varchar(20),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		session_id varchar(Max)		
	)

	-- Target Log Entry (Account Creation)
	create table #acc_temp
	(
		system_dtm datetime,
		sp_id char(8),
		data_entry varchar(20),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		session_id varchar(Max)	
	)

	-- Statistic Result (Claim)
	create table #result
	(
		start_time datetime,
		end_time datetime,
		--diff_time varchar(10),
		[second] int,
		creation char(1),
		creation_time datetime,
		creation_second int,
		print_consent char(1),
		print_consent_time datetime,
		print_consent_second int,
		session_id varchar(Max)
	)

	-- Statistic Result (Claim)
	create table #acc_result
	(
		start_time datetime,
		end_time datetime,
		[second] int,
		--print_form char(1),
		--print_form_time datetime,
		--print_form_second int,
		session_id varchar(Max),
		Is_SmartIC	char(1)
	)

	
	
	-- Insert into Target Log Entry (Claim)
	IF @Year = '09' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   --convert(varchar(MAX), DecryptByKey(E_Client_IP)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   --convert(varchar(MAX), DecryptByKey(E_Application_Server)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp09
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '10'
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp10
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '11' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp11
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '12' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp12
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '13' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp13
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '14' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp14
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '15' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp15
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '16' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp16
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '17' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp17
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '18' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp18
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '19' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp19
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '20' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp20
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '21' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp21
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '22' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp22
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '23' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp23
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '24' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp24
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '25' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp25
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '26' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp26
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '27' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp27
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '28' 
	BEGIN
		insert into #temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp28
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00018') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00023') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END



	-- Insert into Target Log Entry (Account Creation)
	IF @Year = '09' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   --convert(varchar(MAX), DecryptByKey(E_Client_IP)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   --convert(varchar(MAX), DecryptByKey(E_Application_Server)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp09
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '10' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp10
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '11' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp11
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '12' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp12
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '13' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp13
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '14' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp14
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '15' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp15
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '16' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp16
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '17' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp17
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '18' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp18
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '19' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp19
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '20' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp20
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '21' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp21
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '22' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp22
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '23' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp23
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '24' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp24
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '25' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp25
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '26' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp26
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '27' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp27
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '28' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp28
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and 
			  (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00038') = E_Log_ID)
		and system_dtm between @start_time and @end_time
	END



-----------------------------------------------------------------------------------------------
-- Clear up the claim relating to Smart IC
-----------------------------------------------------------------------------------------------

	DELETE FROM
		#temp
	WHERE
		Log_ID = '00017'
			AND Description LIKE '%<Is Read by Smart ID Case: True>%'





	-- Clear up Claim's Audit Log (ensure the audit log comes with a pair)
	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
		select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #temp
		order by function_code, sp_id, data_entry, session_id, system_dtm desc

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	WHILE @@Fetch_status = 0
	BEGIN

		select @temp_log_id = @log_id
		select @temp_system_dtm = @system_dtm

		if @temp_function_code <> @function_code
		begin
			select @temp_function_code = null
		end
		
		if @temp_log_id = '00017'
			begin
				-- Complete Claim: Mark the Complete Flag
				select @end = 'Y'
				select @temp_session_id = @session_id
				select @temp_function_code = @function_code
			end
		else if @temp_log_id = '00002' or @temp_log_id = '00006' or @temp_log_id = '00018'
			begin
				-- Search EHSAccount Start
				if @end = 'Y'
					begin
						-- Matched A Pair of Record
						select @end = 'N'
					end
				else
					begin
						-- Located a Start Search Account log without Complete Claim
						-- Ignore the log
						delete from #temp
						where system_dtm = @temp_system_dtm
						and log_id = @temp_log_id
						and (function_code = @temp_function_code or @temp_function_code is null)
					end
			end

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record

	
	-- Clear up Account Creation's Audit Log (ensure the audit log comes with a pair)
	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #acc_temp
	order by function_code, sp_id, data_entry, session_id, system_dtm desc

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	WHILE @@Fetch_status = 0
	BEGIN

		select @temp_log_id = @log_id
		select @temp_system_dtm = @system_dtm

		if @temp_function_code <> @function_code
		begin
			select @temp_function_code = null
		end
		
		if @temp_log_id = '00038'
			begin
				-- Create Account Complete
				select @end = 'Y'
				select @temp_session_id = @session_id	
				select @temp_function_code = @function_code
			end
		else if @temp_log_id = '00025' OR @temp_log_id = '00028'
			begin
				-- Create Account Start
				if @end = 'Y'
					begin
						-- Matched A Pair of Record
						select @end = 'N'
					end
				else
					begin
						-- Located a Start Create Account log without a Complete Log
						-- Ignore the log
						delete from #temp
						where system_dtm = @temp_system_dtm
						and log_id = @temp_log_id
						and (function_code = @temp_function_code or @temp_function_code is null)
					end
			end

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
	
	-----------------------------------------------------------------------------------------------
	
	
	-- Prepare time info for Claim (with A/C creation + claim printing)
	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #temp
	order by function_code, sp_id, data_entry, session_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id = '00002' or @log_id = '00006' or @log_id = '00018'
		begin
			-- Search Account Begin: Log the time
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id = '00017'
		begin 
			-- Complete Claim
			if @temp_system_dtm is not null AND 
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code
			begin
				-- Located a Pair: Put into the Result table
				insert into #result
				(
					start_time,
					end_time,
					[second],
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						datediff(second, @temp_system_dtm,@system_dtm),
						@session_id

				select @temp_system_dtm = NULL
			end
		end
		
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record


	-- Prepare time info for Account creation
	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #acc_temp
	order by function_code, sp_id, data_entry, session_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id = '00025' OR @log_id = '00028' 
		begin
			-- Account Creation: Enter Detail Start
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id = '00038'
		begin 
			-- Complete Account Creation
			if @temp_system_dtm is not null AND 
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code		   
			begin
				-- Located a Pair: Put into the Result table
				insert into #acc_result
				(
					start_time,
					end_time,
					[second],
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						datediff(second, @temp_system_dtm,@system_dtm),
						@session_id

				select @temp_system_dtm = NULL
			end
		end
		
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
	
-- Delete the SmartIC case
	
	UPDATE
		#acc_result
	SET
		Is_SmartIC = 'Y'
	FROM
		#acc_result R
			INNER JOIN (SELECT System_Dtm, Session_ID FROM #acc_temp WHERE Log_ID = '00055') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Time AND R.End_Time
	
	DELETE FROM
		#acc_result
	WHERE
		Is_SmartIC = 'Y'
	
	
	
	-----------------------------------------------------------------------------------------------
	
	-- Check which Claim log included Account Creation Process
	update #result
		set creation = 'Y'
	from #result r, 
		(select system_dtm, session_id from #temp where log_id = '00040') as t
	where r.session_id = t.session_id AND
		  t.system_dtm between r.start_time and r.end_time
		
	update #result
		set creation = 'N'
	where creation is null
	
	
	-- Check which Claim log included Print Consent form
	update #result
		set print_consent = 'Y',
			print_consent_time = t.system_dtm,
			print_consent_second = datediff(second, t.system_dtm, r.end_time)
	from #result r, 
		(select session_id, system_dtm from #temp where log_id = '00023') as t
	where r.session_id = t.session_id AND 
		  t.system_dtm between r.start_time and r.end_time

	update #result
	set print_consent = 'N'
	where print_consent_second is null

	/*--- Account Creation Dont have Consent form to Print	---
	update #acc_result
	set print_form = 'Y',
		print_form_time = t.system_dtm
	from #acc_result r,
	(select * from #acc_temp where log_id in ('00028', '00029')) as t
	where t.system_dtm between r.start_time and r.end_time
	and r.session_id = t.session_id

	update #acc_result
	set print_form_second = datediff(second, r.print_form_time, t.system_dtm)
	from #acc_result r,
	(select * from #acc_temp where log_id = '00025') as t
	where t.system_dtm between r.start_time and r.end_time
	and r.session_id = t.session_id

	update #acc_result
	set print_form = 'N'
	where print_form is null
	*/

	-- All claim duration (without A/C Creation &  - printing)
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	select	getdate(),
			@start_time,
			'Claim (without A/C creation & - claim printing)',
			max([second] - isnull(print_consent_second,0)), 
			min([second] - isnull(print_consent_second,0)), 
			avg([second] - isnull(print_consent_second,0)),
			count(*)
	from #result
	where creation = 'N'

	
	-- All claim duration (without A/C Creation &  + printing)
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	select	getdate(),
			@start_time,
			'Claim (without A/C creation & + claim printing)',
			max([second]),
			min([second]),
			avg([second]),
			count(*)
	from #result
	where creation = 'N'
	and print_consent = 'Y'

	
	-- All claim duration (with A/C Creation &  - printing)
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	select	getdate(),
			@start_time,
			'Claim (with A/C creation & - claim printing)',
			max([second] - isnull(print_consent_second,0)), 
			min([second] - isnull(print_consent_second,0)), 
			avg([second] - isnull(print_consent_second,0)),
			count(*)
	from #result 
	where creation = 'Y'

	
	-- All claim duration (with A/C Creation &  + printing)
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	select	getdate(),
			@start_time,
			'Claim (with A/C creation & + claim printing)',
			max([second]), 
			min([second]), 
			avg([second]),
			count(*)
	from #result 
	where creation = 'Y'
	 and print_consent = 'Y'
	
	-- Printing Time
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	select	getdate(),
			@start_time,
			'Claim Printing Time',
			max(print_consent_second),
			min(print_consent_second),
			avg(print_consent_second)
	from #result
	where print_consent = 'Y'

	/*--- Account Creation Dont have Consent form to Print	---
	-- A/C Creation (- printing)
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	select	getdate(),
			@start_time,
			'A/C Creation (- creation printing)',
			max(second - isnull(print_form_second,0)),
			min(second - isnull(print_form_second,0)),
			avg(second - isnull(print_form_second,0))
	from #acc_result

	-- A/C Creation (+ printing)
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	select	getdate(),
			@start_time,
			'A/C Creation (+ creation printing)',
			max(second),
			min(second),
			avg(second)
	from #acc_result
	where print_form = 'Y'

	-- A/C Creation Printing Time
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	select	getdate(),
			@start_time,
			'A/C Creation Printing Time',
			max(print_form_second),
			min(print_form_second),
			avg(print_form_second)
	from #acc_result
	where print_form = 'Y'
	*/
	-- A/C Creation
	insert into _ClaimDurationSummary
	(
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	select	getdate(),
			@start_time,
			'A/C Creation (- creation printing)',
			max([second]),
			min([second]),
			avg([second])
	from #acc_result

	
	EXEC [proc_SymmetricKey_close]
	
	drop table #temp
	drop table #result

	drop table #acc_temp
	drop table #acc_result


	-- Execute the stored procedure for Smart IC
	
	EXEC [proc_EHS_ClaimDurationSmartIC_Stat_Schedule] @start_time, @end_time


END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_ClaimDuration_Stat_Schedule] TO HCVU
GO
