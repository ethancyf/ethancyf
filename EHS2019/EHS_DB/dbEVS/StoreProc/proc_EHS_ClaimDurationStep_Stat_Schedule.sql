IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_ClaimDurationStep_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_ClaimDurationStep_Stat_Schedule]
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
-- Modified date:	5 November 2010
-- Description:		(1) Exclude Invoke CMS Web service in Search Account
--					(2) Include [Session_ID] when finding CMS vaccination records
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
-- Create date: 23 Mar 2009
-- Description:	Statistics for getting voucher claim duration break down in step
--				1. Save related data to temp table (_ClaimDurationSummary)
--				function code 020201 Claim Voucher (Full Version)
--							  020202	Claim Voucher (Text Only Version)
--				log id	00008	Is TSW Case
--						00009	Is Not TSW Case
--						00011	Complete Enter Claim Details
--						00001	Search Voucher Account
--						00014	Complete Claim
--						00025	Complete Create Temporary Account
--						00030	Press Process To Claim
--						00020	Confirm Collect Consent
--						00022	Complete Enter Temporary Account Info 
--						00000	Claim Voucher (Full version) Loaded
--						00001	Search Voucher Account
--						00002	Not Is Existing Account
--						00003	Is Existing Validated Account
--						00004	Is Existing Temporary Account
--						00005	Search Fail
--						00019	Press Next Claim
--						00031	Press Next Creation
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_ClaimDurationStep_Stat_Schedule]
	@start_time datetime = null
	, @end_time datetime = null
AS
BEGIN
	-----------------------------------------------------------------
	-- function code	020201	Claim Voucher (Full Version)
	--				  	020202	Claim Voucher (Text Only Version)
	-- log id			(Enter Claim Detail)
	--					00042	[BEGIN] Enter Claim Detail Page Loaded	
	--					00010	Enter Claim Detail Start
	--					00011	[END] Enter Claim Detail Complete
	--					00013	[END] Enter Claim Detail Failed
	--
	--					(Process To Claim) - (Step from 'Search Account' to 'Proceed To Claim')
	--					00002	[BEGIN] Search EHS Account Start
	--					00006	[BEGIN] Search Prefilled Account Start
	--					00036	Confirm Detail Complete (Account Created)
	--					00040	Process To Claim
	--					00017	[END] Complete Claim
	--
	--					(Enter A/C Creation Details)
	--					00025	[BEGIN] Create Account Pressed
	--					00028	[BEGIN] Confirm Modify Account
	--					00055	Create Acccount By SmartID
	--					00031	Enter Detail Start
	--					00032	[END] Enter Detail Complete
	--					00033	[END] Enter Detail Failed
	--
	--					(Search Account)
	--					00001	[BEGIN] Full Version Claim Page Loaded		
	--					00005	[BEGIN] Search Account Failed
	--					00008	[BEGIN] Prefilled Search Failed
	--					00052	[BEGIN] Search & validate account with CFD Fail: <...>
	--					00063	[BEGIN] Get CFD Fail: <Artifact>
	--					00039	[BEGIN] Create new Account
	--					00044	[BEGIN] Complete Claim - Next Claim Clicked
	--					00004	[END] Search EHSAccount Complete
	--					00007	[END] Prefilled Search Complete
	-----------------------------------------------------------------


-- =============================================
-- Report setting 
-- =============================================
	DECLARE @Function_Code_Master table (
		Function_Code	char(6)
	)
	
	DECLARE @Log_ID_Master_Search_Account_System table (
		Log_ID			char(5)
	)

	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020201')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020202')
	
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('00002')	-- [B-Start]	Search EHSAccount Start
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('00006')	-- [B-Start]	Pre-Filled Search Start
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('00004')	-- [B-End]		Search EHSAccount Complete
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('00005')	-- [B-End]		Search Account Failed
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('00007')	-- [B-End]		Pre-Filled Search Complete
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('00008')	-- [B-End]		Pre-Filled Search Failed
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('01014')	-- [B-Mid]		Invoke CMS Web service: Start
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('01015')	-- [B-Mid]		Invoke CMS Web service: End
	INSERT INTO @Log_ID_Master_Search_Account_System (Log_ID) VALUES ('01016')	-- [B-Mid]		Invoke CMS Web service: Exception
		  
	
-- =============================================
-- Declaration
-- =============================================

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
	--declare @second as int
	--declare @min as int
	--declare @hour as int
	declare @function_code char(6)
	declare @temp_function_code char(6)
	declare @action_dtm datetime
	declare @end_dtm datetime
	--declare @temp_action_dtm datetime
	--declare @temp_end_dtm datetime
	--declare @max as char(8)
	--declare @mini as char(8)
	--declare @avg as char(8)

	if @start_time is null 
		select @start_time = CONVERT(VARCHAR(11), dateadd(day, -1, GETDATE()), 106) + ' 00:00'
		
	if @end_time is null
		select @end_time = CONVERT(VARCHAR(11), getdate(), 106) + ' 00:00'
		
	set @Year = CONVERT(varchar(2), @start_time, 12)	-- Extract the Calendar Year 

	-- Target Log Entry (Enter Claim Detail)
	create table #claim_temp
	(
		system_dtm datetime,
		action_dtm datetime,
		end_dtm datetime,
		sp_id char(8),
		data_entry varchar(20),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		session_id varchar(Max)		
	)

	-- Target Log Entry (Process To Claim)			(Step from 'Search Account' to 'Proceed To Claim')
	create table #proceedToClaim_temp
	(
		system_dtm datetime,
		action_dtm datetime,
		end_dtm datetime,
		sp_id char(8),
		data_entry varchar(20),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		session_id varchar(Max)		
	)

	-- Target Log Entry (Enter A/C Creation Details)
	create table #acc_temp
	(
		system_dtm datetime,
		action_dtm datetime,
		end_dtm datetime,
		sp_id char(8),
		data_entry varchar(20),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		session_id varchar(Max)	
	)

	-- Target Log Enter (Search Account)
	create table #search_temp
	(
		system_dtm datetime,
		action_dtm datetime,
		end_dtm datetime,
		sp_id char(8),
		data_entry varchar(20),
		function_code char(6),
		log_id char(5),
		description nvarchar(MAX),
		session_id varchar(Max)	
	)
	
	DECLARE @AuditLogSearchAccountSystem table (
		System_Dtm				datetime,
		Action_Dtm				datetime,
		End_Dtm					datetime,
		SP_ID					char(8),
		Data_Entry				varchar(20),
		Function_Code			char(6),
		Log_ID					char(5),
		Description				nvarchar(MAX),
		Session_ID				varchar(MAX)
	)
	
	DECLARE @ResultSearchAccountSystem table (
		start_time datetime,
		end_time datetime,
		second int,
		sys_second int,
		session_id varchar(Max)
	)

	-- Statistic Result (Enter Claim Detail)
	create table #claim_result
	(
		start_time datetime,
		end_time datetime,
		second int,
		sys_second int,
		session_id varchar(Max),
		Is_Smart_IC	char(1)
	)

	-- Statistic Result (Process To Claim)			(Step from 'Search Account' to 'Proceed To Claim')
	create table #proceedToClaim_result
	(
		start_time datetime,
		end_time datetime,
		second int,
		sys_second int,
		session_id varchar(Max),
		creation char(1),
		complete_create_temp datetime,
		proceed_to_claim datetime		
	)
	
	-- Statistic Result (Enter A/C Creation Details)
	create table #acc_result
	(
		start_time datetime,
		end_time datetime,
		second int,
		sys_second int,
		session_id varchar(Max),
		Is_Smart_IC	char(1)
	)
	
	-- Statistic Result (Search Account)
	create table #search_result
	(
		start_time datetime,
		end_time datetime,
		second int,
		sys_second int,
		session_id varchar(Max)
	)


-- =============================================
-- Retrieve audit log
-- =============================================
	
	-- Insert into Target Log Entry (Enter Claim Detail)
	IF @Year = '09' 
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp09
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP09
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				
	END
	ELSE IF @Year = '10'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp10
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP10
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				
	END
	ELSE IF @Year = '11'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp11
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP11
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				
	END
	ELSE IF @Year = '12'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp12
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
	
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP12
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				
	END
	ELSE IF @Year = '13'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp13
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP13
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				  
	END
	ELSE IF @Year = '14'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp14
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP14
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
					  
	END
	ELSE IF @Year = '15'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp15
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP15
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
					  
	END
	ELSE IF @Year = '16'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp16
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP16
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				 
	END
	ELSE IF @Year = '17'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp17
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP17
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)  
				
	END
	ELSE IF @Year = '18'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp18
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP18
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
					  
	END
	ELSE IF @Year = '19'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp19
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP19
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '20'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp20
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP20
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '21'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp21
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP21
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '22'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp22
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time

		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP22
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '23'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp23
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP23
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '24'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp24
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP24
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '25'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp25
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP25
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '26'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp26
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP26
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '27'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp27
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP27
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
			  
	END
	ELSE IF @Year = '28'
	BEGIN
		insert into #claim_temp
			(
				system_dtm,
				action_dtm,
				end_dtm,
				sp_id,
				data_entry,
				function_code,
				log_id,
				description,
				session_id
			)
			select system_dtm,
				   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
				   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
				   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
				   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
				   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
				   convert(nvarchar(MAX), DecryptByKey(E_Description)),
				   convert(varchar(MAX), DecryptByKey(E_Session_ID))
			from auditloghcsp28
			where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
			  and (EncryptByKey(KEY_GUID('sym_Key'), '00042') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00010') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00011') = E_Log_ID or
				   EncryptByKey(KEY_GUID('sym_Key'), '00013') = E_Log_ID)
			  and system_dtm between @start_time and @end_time
			  
		INSERT INTO @AuditLogSearchAccountSystem (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP28
		WHERE
			System_Dtm BETWEEN @start_time AND @end_time
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master_Search_Account_System)
				  
	END
	
	
	-- Insert into Target Log Entry (Process To Claim)			(Step from 'Search Account' to 'Proceed To Claim')
	IF @Year = '09' 
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp09
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '10'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp10
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '11'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp11
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '12'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp12
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '13'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp13
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '14'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp14
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '15'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp15
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '16'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp16
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '17'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp17
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '18'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp18
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '19'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp19
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '20'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp20
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '21'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp21
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '22'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp22
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '23'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp23
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '24'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp24
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '25'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp25
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '26'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp26
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '27'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp27
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '28'
	BEGIN
		insert into #proceedToClaim_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp28
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
				  EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00002') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00006') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00017') = E_Log_ID or
				 EncryptByKey(KEY_GUID('sym_Key'), '00036') = E_Log_ID or 		 
				 EncryptByKey(KEY_GUID('sym_Key'), '00040') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END


	-- Insert into Target Log Entry (Enter A/C Creation Details)
	IF @Year = '09' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp09
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '10' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp10
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '11' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp11
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '12' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp12
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '13' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp13
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '14' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp14
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '15' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp15
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '16' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp16
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '17' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp17
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '18' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp18
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '19' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp19
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '20' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp20
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '21' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp21
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '22' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp22
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '23' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp23
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '24' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp24
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '25' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp25
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '26' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp26
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '27' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp27
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '28' 
	BEGIN
		insert into #acc_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp28
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00025') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00028') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00055') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00031') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00032') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00033') = E_Log_ID )
		  and system_dtm between @start_time and @end_time
	END


	
	-- Insert into Target Log Entry (Search Account)
	IF @Year = '09'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp09
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '10'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp10
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END
	ELSE IF @Year = '11'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp11
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '12'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp12
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '13'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp13
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '14'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp14
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '15'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp15
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '16'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp16
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '17'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp17
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '18'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp18
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '19'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp19
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '20'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp20
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '21'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp21
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '22'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp22
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '23'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp23
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '24'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp24
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '25'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp25
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '26'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp26
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '27'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp27
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time

	END
	ELSE IF @Year = '28'
	BEGIN
		insert into #search_temp
		(
			system_dtm,
			action_dtm,
			end_dtm,
			sp_id,
			data_entry,
			function_code,
			log_id,
			description,
			session_id
		)
		select system_dtm,
			   convert(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_End_Dtm)),
			   convert(varchar(MAX), DecryptByKey(E_USer_ID)),
			   convert(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			   convert(varchar(MAX), DecryptByKey(E_Function_Code)),
			   convert(varchar(MAX), DecryptByKey(E_Log_ID)),
			   convert(nvarchar(MAX), DecryptByKey(E_Description)),
			   convert(varchar(MAX), DecryptByKey(E_Session_ID))
		from auditloghcsp28
		where (EncryptByKey(KEY_GUID('sym_Key'), '020201') = E_Function_Code or 
			   EncryptByKey(KEY_GUID('sym_Key'), '020202') = E_Function_Code)
		  and (EncryptByKey(KEY_GUID('sym_Key'), '00001') = E_Log_ID or 
			   EncryptByKey(KEY_GUID('sym_Key'), '00005') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00008') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00052') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00063') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00039') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00044') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00004') = E_Log_ID or
			   EncryptByKey(KEY_GUID('sym_Key'), '00007') = E_Log_ID)
		  and system_dtm between @start_time and @end_time
	END



-----------------------------------------------------------------------------------------------
-- Clear up the claim relating to Smart IC
-----------------------------------------------------------------------------------------------

	DELETE FROM
		#proceedToClaim_temp
	WHERE
		Log_ID = '00017'
			AND Description LIKE '%<Is Read by Smart ID Case: True>%'







	
	-- Clear up Enter Claim Detail's Audit Log (ensure the audit log comes with a pair)
	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #claim_temp
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
		
		if @temp_log_id = '00011' OR @temp_log_id = '00013' 
			begin
				select @end = 'Y'	
				select @temp_session_id = @session_id	
				select @temp_function_code = @function_code
			end
		else if @temp_log_id = '00042'
			begin
				if @end = 'Y'
					begin
						select @end = 'N'
					end
				else
					begin
						delete from #claim_temp
						where system_dtm = @temp_system_dtm
						and log_id = @temp_log_id
						and (function_code = @temp_function_code or @temp_function_code is null)
					end
			end		

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
	

	--


	-- Clear up Process To Claim's Audit Log (ensure the audit log comes with a pair)
	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #proceedToClaim_temp
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
				select @end = 'Y'	
				select @temp_session_id = @session_id	
				select @temp_function_code = @function_code
			end
		else if @temp_log_id = '00002' or @temp_log_id = '00006'
			begin
				if @end = 'Y'
					begin
						select @end = 'N'
					end
				else
					begin
						delete from #proceedToClaim_temp
						where system_dtm = @temp_system_dtm
						and log_id = @temp_log_id
						and (function_code = @temp_function_code or @temp_function_code is null)
					end
			end	

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
	

	-----
	
	
	-- Clear up Enter A/C Creation Details's Audit Log (ensure the audit log comes with a pair)
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
		
		if @temp_log_id = '00032' OR @temp_log_id = '00033'
			begin
				select @end = 'Y'	
				select @temp_session_id = @session_id	
				select @temp_function_code = @function_code
			end
		else if @temp_log_id = '00025' OR @temp_log_id = '00028'
			begin
				if @end = 'Y'
					begin
						select @end = 'N'
					end
				else
					begin
						delete from #acc_temp
						where system_dtm = @temp_system_dtm
						and log_id = @temp_log_id
						and (function_code = @temp_function_code or @temp_function_code is null)
					end
			end

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
	
	---


	-- Clear up Search Account's Audit Log (ensure the audit log comes with a pair)
	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id from #search_temp
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
		
		if @temp_log_id = '00004' OR @temp_log_id = '00007'
			begin
				select @end = 'Y'	
				select @temp_session_id = @session_id	
				select @temp_function_code = @function_code
			end
		else if @temp_log_id IN ('00001', '00005', '00008', '00052', '00063', '00039', '00044')
			begin
				if @end = 'Y'
					begin
						select @end = 'N'
					end
				else
					begin
						delete from #search_temp
						where system_dtm = @temp_system_dtm
						and log_id = @temp_log_id
						and (function_code = @temp_function_code or @temp_function_code is null)
					end
			end	

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
-- ---------------------------------------------------------------------------------
-- Clear up Search Account Audit Log (System Time)
-- ---------------------------------------------------------------------------------

	select @end = 'N'
	select @temp_log_id = null
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null

	DECLARE record CURSOR FOR
	SELECT
		Function_Code,
		System_Dtm,
		SP_ID,
		Data_Entry,
		Log_ID,
		Session_ID
	FROM
		@AuditLogSearchAccountSystem
	ORDER BY
		Function_Code,
		SP_ID,
		Data_Entry,
		Session_ID,
		System_Dtm DESC

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
		
		if @temp_log_id IN ('00004', '00005', '00007', '00008')
			begin
				select @end = 'Y'	
				select @temp_session_id = @session_id	
				select @temp_function_code = @function_code
			end
		else if @temp_log_id IN ('00002', '00006')
			begin
				if @end = 'Y'
					begin
						select @end = 'N'
					end
				else
					begin
						delete from @AuditLogSearchAccountSystem
						where System_Dtm = @temp_system_dtm
						and Log_ID = @temp_log_id
						and (Function_Code = @temp_function_code or @temp_function_code is null)
					end
			end	

		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id
	end

	CLOSE record
	DEALLOCATE record
	
	
-- ================================================
-- Prepare time information
-- ================================================


	-- Prepare time info for Enter Claim Detail
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null
	
	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id, action_dtm, end_dtm from #claim_temp
	order by function_code, sp_id, data_entry, session_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id = '00042'
		begin
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id = '00011' OR @log_id = '00013'
		begin 
			if @temp_system_dtm is not null AND
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code
			begin
				insert into #claim_result
				(
					start_time,
					end_time,
					second,
					sys_second,
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						datediff(second, @temp_system_dtm,@system_dtm),
						-- datediff(second, @action_dtm, @end_dtm),
						datediff(ms, @action_dtm, @end_dtm) / 1000,
						@session_id
				select @temp_system_dtm = NULL
			end
		END
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	end

	CLOSE record
	DEALLOCATE record


-- Delete the SmartIC case

	UPDATE
		#claim_result
	SET
		Is_Smart_IC = 'Y'
	FROM
		#claim_result R
			INNER JOIN (SELECT Session_ID, System_Dtm FROM #claim_temp WHERE Log_ID = '00010' AND Description LIKE '%<Is Read by Smart ID Case: True>%') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Time AND R.End_Time
	
	DELETE FROM
		#claim_result
	WHERE
		Is_Smart_IC = 'Y'


--


	-- Prepare time info for Proceed to Claim
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null
	
	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id, action_dtm, end_dtm from #proceedToClaim_temp
	order by function_code, sp_id, data_entry, session_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id = '00002' or @log_id = '00006'
		begin
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id = '00017'
		begin 
			if @temp_system_dtm is not null AND
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code
			begin
				insert into #proceedToClaim_result
				(
					start_time,
					end_time,
					second,							
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						datediff(second, @temp_system_dtm,@system_dtm),								
						@session_id

				select @temp_system_dtm = NULL
			end
		END
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	end

	CLOSE record
	DEALLOCATE record


----


	-- Prepare time info for Enter Account Creation Detail
	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null
	
	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id, action_dtm, end_dtm from #acc_temp
	order by function_code, sp_id, data_entry, session_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id = '00025' OR @log_id = '00028'
		begin
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id = '00032' OR @log_id = '00033'
		begin 
			if @temp_system_dtm is not null AND
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code
			begin
				insert into #acc_result
				(
					start_time,
					end_time,
					second,
					sys_second,
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						datediff(second, @temp_system_dtm,@system_dtm),
						-- datediff(second, @action_dtm, @end_dtm),
						datediff(ms, @action_dtm, @end_dtm) / 1000,
						@session_id

				select @temp_system_dtm = NULL
			end
		END
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	end

	CLOSE record
	DEALLOCATE record

-- Delete the SmartIC case
	
	UPDATE
		#acc_result
	SET
		Is_Smart_IC = 'Y'
	FROM
		#acc_result R
			INNER JOIN (SELECT System_Dtm, Session_ID FROM #acc_temp WHERE Log_ID = '00055') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Time AND R.End_Time
	
	DELETE FROM
		#acc_result
	WHERE
		Is_Smart_IC = 'Y'
	
	
		
	

-- ---------------------------------------------------------
-- Prepare time info for Search Account (User Time)
-- ---------------------------------------------------------

	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null
	
	DECLARE record CURSOR FOR
	select function_code, system_dtm, sp_id, data_entry, log_id, session_id, action_dtm, end_dtm from #search_temp
	order by function_code, sp_id, data_entry, session_id, system_dtm

	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id in ('00001', '00005', '00008', '00052', '00063', '00039', '00044')
		begin
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id in ('00004', '00007')
		begin 
			if @temp_system_dtm is not null AND
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code
			begin
				insert into #search_result
				(
					start_time,
					end_time,
					second,
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						datediff(second, @temp_system_dtm,@system_dtm),								
						@session_id
				
				select @temp_system_dtm = NULL
			end
		END
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	end

	
	
	CLOSE record
	DEALLOCATE record
	
	
-- ---------------------------------------------------------
-- Prepare time info for Search Account (System Time)
-- ---------------------------------------------------------

	select @temp_system_dtm = null
	select @temp_session_id = null
	select @temp_function_code = null
	
	
	DECLARE @InvokeCMSTime table (
		Log_ID		char(5),
		System_Dtm	datetime
	)
	
	DECLARE @InvokeCMSTimeRaw table (
		Time_Use	int
	)
	
	DECLARE InvokeCMSTime_Cursor_Desc CURSOR FOR
	SELECT
		Log_ID,
		System_Dtm
	FROM
		@InvokeCMSTime
	ORDER BY
		System_Dtm DESC
		
	DECLARE InvokeCMSTime_Cursor_Asc CURSOR FOR
	SELECT
		Log_ID,
		System_Dtm
	FROM
		@InvokeCMSTime
	ORDER BY
		System_Dtm
		
	DECLARE @CMS_Log_ID		char(5)
	DECLARE @CMS_System_Dtm	datetime
	
	DECLARE @CMS_Start_Dtm	datetime
	DECLARE @CMS_Total_Time	int
	
	DECLARE record CURSOR FOR
	SELECT
		Function_Code,
		System_Dtm,
		SP_ID,
		Data_Entry,
		Log_ID,
		Session_ID,
		Action_Dtm,
		End_Dtm
	FROM
		@AuditLogSearchAccountSystem
	ORDER BY
		Function_Code,
		SP_ID,
		Data_Entry,
		Session_ID,
		System_Dtm
		
	OPEN record
	FETCH next FROM record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	WHILE @@Fetch_status = 0
	BEGIN
		if @log_id in ('00002', '00006')
		begin
			select @temp_system_dtm = @system_dtm
			select @temp_session_id = @session_id
			select @temp_function_code = @function_code
		end
		else if @log_id in ('00004', '00005', '00007', '00008')
		begin 
			if @temp_system_dtm is not null AND
			   @temp_session_id is not null AND @temp_session_id = @session_id AND
			   @temp_function_code = @function_code
			   
			begin
				-- Determine the Invoke CMS Web service inside the search account
				
				DELETE FROM @InvokeCMSTime
				
				INSERT INTO @InvokeCMSTime (
					Log_ID,
					System_Dtm
				)
				SELECT
					Log_ID,
					System_Dtm
				FROM
					@AuditLogSearchAccountSystem
				WHERE
					Log_ID IN ('01014', '01015', '01016')
						AND System_Dtm BETWEEN @temp_system_dtm AND @system_dtm
						AND Session_ID = @session_id
						
				-- Clear log to ensure pairing
				SET @end = 'N'
				
				OPEN InvokeCMSTime_Cursor_Desc
				FETCH NEXT FROM InvokeCMSTime_Cursor_Desc INTO @CMS_Log_ID, @CMS_System_Dtm
				
				WHILE @@FETCH_STATUS = 0 BEGIN
					IF @CMS_Log_ID IN ('01015', '01016') BEGIN
						SET @end = 'Y'
					END ELSE IF @CMS_Log_ID IN ('01014') BEGIN
						IF @end = 'Y' BEGIN
							SET @end = 'N'
						END ELSE BEGIN
							DELETE FROM
								@InvokeCMSTime
							WHERE
								Log_ID = @CMS_Log_ID
									AND System_Dtm = @CMS_System_Dtm
						END
					END
				
					FETCH NEXT FROM InvokeCMSTime_Cursor_Desc INTO @CMS_Log_ID, @CMS_System_Dtm
				END
				
				CLOSE InvokeCMSTime_Cursor_Desc
				
				
				-- Calculate time
				OPEN InvokeCMSTime_Cursor_Asc
				FETCH NEXT FROM InvokeCMSTime_Cursor_Asc INTO @CMS_Log_ID, @CMS_System_Dtm
				
				SET @CMS_Start_Dtm = NULL
				SET @CMS_Total_Time = 0
				
				WHILE @@FETCH_STATUS = 0 BEGIN
					IF @CMS_Log_ID IN ('01014') BEGIN
						SET @CMS_Start_Dtm = @CMS_System_Dtm
						
					END ELSE IF @CMS_Log_ID IN ('01015', '01016') BEGIN
						IF @CMS_Start_Dtm IS NOT NULL BEGIN
							-- Insert into the table, not affecting the statistics, just for tracing
							INSERT INTO @InvokeCMSTimeRaw (
								Time_Use
							)
							SELECT
								DATEDIFF(ms, @CMS_Start_Dtm, @CMS_System_Dtm)
						
							SET @CMS_Total_Time = @CMS_Total_Time + DATEDIFF(ms, @CMS_Start_Dtm, @CMS_System_Dtm)
							SET @CMS_Start_Dtm = NULL
							
						END
					END
					
					FETCH NEXT FROM InvokeCMSTime_Cursor_Asc INTO @CMS_Log_ID, @CMS_System_Dtm
				
				END
				
				CLOSE InvokeCMSTime_Cursor_Asc
				
				
				insert into @ResultSearchAccountSystem
				(
					start_time,
					end_time,
					second,
					session_id
				)
				select	@temp_system_dtm, @system_dtm, 
						(DATEDIFF(ms, @temp_system_dtm, @system_dtm) - @CMS_Total_Time) / 1000,								
						@session_id
				
				select @temp_system_dtm = NULL
			end
		END
		FETCH record INTO @function_code, @system_dtm ,@sp_id, @data_entry, @log_id, @session_id, @action_dtm, @end_dtm
	end

	DEALLOCATE InvokeCMSTime_Cursor_Desc
		
	DEALLOCATE InvokeCMSTime_Cursor_Asc
	
	CLOSE record
	DEALLOCATE record


--

	update #proceedToClaim_result
	set creation = 'Y',
		proceed_to_claim = t.system_dtm
	from #proceedToClaim_result r, 
	(select * from #proceedToClaim_temp where log_id = '00040') as t
	where t.system_dtm between r.start_time and r.end_time
	and r.session_id = t.session_id

	update #proceedToClaim_result
	set complete_create_temp = t.system_dtm,
		-- sys_second = datediff(second, t.action_dtm, t.end_dtm)
		sys_second = datediff(ms, t.action_dtm, t.end_dtm) / 1000
	from #proceedToClaim_result r, 
	(select * from #proceedToClaim_temp where log_id = '00036') as t
	where t.system_dtm between r.start_time and r.end_time
	and r.session_id = t.session_id
	and r.creation = 'Y'

	update #proceedToClaim_result
	set second = datediff(second, complete_create_temp, proceed_to_claim)
	where creation = 'Y'

--select * from #claim_result
--order by second desc
--
--select * from #acc_result
--order by second desc
--
--select * from #proceedToClaim_result
--where creation = 'Y'
--order by second desc

--select * from #search_result
--order by second desc

/*
--	declare @max_search as int
--	declare @min_search as int
--	declare @avg_search as int

	-- select @max_search = max(datediff(second, action_dtm ,end_dtm))
	select @max_search = max(datediff(ms, action_dtm ,end_dtm) / 1000)
	from #search_temp
	where log_id in ('00004', '00005', '00007', '00008')
		and session_id in (select session_id from #search_result)

	-- select @min_search = min(datediff(second, action_dtm ,end_dtm))
	select @min_search = min(datediff(ms, action_dtm ,end_dtm) / 1000)
	from #search_temp
	where log_id in ('00004', '00005', '00007', '00008')
		and session_id in (select session_id from #search_result)

	-- select @avg_search = avg(datediff(second, action_dtm ,end_dtm))
	select @avg_search = avg(datediff(ms, action_dtm ,end_dtm) / 1000)
	from #search_temp
	where log_id in ('00004', '00005', '00007', '00008')
		and session_id in (select session_id from #search_result)
*/

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
			'Search (User Time)',
			max(second), 
			min(second),
			avg(second),
			count(*)
	from #search_result

	
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
			'Search (System Time)',
			max(second), 
			min(second),
			avg(second),
			count(1)
	from @ResultSearchAccountSystem

		
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
			'Enter Claim Details (User Time)',
			max(second), 
			min(second),
			avg(second),
			count(*)
	from #claim_result


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
			'Enter Claim Details (System Time)',
			max(sys_second),
			min(sys_second),
			avg(sys_second),
			count(*)
	from #claim_result


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
			'Enter A/C Creation Details (User Time)',
			max(second),
			min(second),
			avg(second),
			count(*)
	from #acc_result

	
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
			'Enter A/C Creation Details (System Time)',
			max(sys_second),
			min(sys_second),
			avg(sys_second),
			count(*)
	from #acc_result

	
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
			'Process To Claim (User Time)',
			max(second),
			min(second),
			avg(second),
			count(*)
	from #proceedToClaim_result
	where creation = 'Y'


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
			'Process To Claim (System Time)',
			max(sys_second),
			min(sys_second),
			avg(sys_second),
			count(*)
	from #proceedToClaim_result
	where creation = 'Y'
	
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	SELECT
		GETDATE(),
		@start_time,
		'Invoke CMS (not used)',
		MAX(Time_Use),
		MIN(Time_Use),
		AVG(Time_Use),
		COUNT(1)
	FROM
		@InvokeCMSTimeRaw
	
	EXEC [proc_SymmetricKey_close]
	
	drop table #claim_temp
	drop table #claim_result

	drop table #acc_temp
	drop table #acc_result
	
	drop table #proceedToClaim_temp
	drop table #proceedToClaim_result

	drop table #search_temp
	drop table #search_result

	
	-- Execute the stored procedure for Smart IC
	
	EXEC [proc_EHS_ClaimDurationStepSmartIC_Stat_Schedule] @start_time, @end_time


END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_ClaimDurationStep_Stat_Schedule] TO HCVU
GO
