IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PatientPortalConnection_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PatientPortalConnection_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:	  	
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		12 Jun 2020
-- CR No.			CRE20-005
-- Description:		Statistics for eHRSS Patient Portal Connection
--					Preparation (Insert into temp tables)
-- =============================================

CREATE PROCEDURE [dbo].[proc_PatientPortalConnection_Stat_Write]
	@Report_Dtm		datetime = NULL
AS BEGIN

	SET NOCOUNT ON;

	-- ===========================================================
	-- DECLARATION
	-- ===========================================================
	DECLARE @Current_Dtm		DATETIME  

	DECLARE @Filter_start_time	DATETIME
	DECLARE @Filter_end_time	DATETIME

	DECLARE @Start_Dtm			DATETIME  
	DECLARE @End_Dtm			DATETIME  
	   
	DECLARE @Year				SMALLINT	-- For identifying the audit log table

	DECLARE @No_Of_Days			INT  

	-- Function Code
	DECLARE @EHRSSToEHSFunctionCodeMaster TABLE (
		Function_Code	CHAR(6)
	)
	-- Log ID
	DECLARE @EHRSSToEHSLogIDMaster TABLE (
		Log_ID			CHAR(5)
	)
	
	-- Doctor List Result
	DECLARE @DoctorListSummary TABLE (  
		Dtm						DATETIME,  
		No_Log_Response0to2		INT DEFAULT 0,			-- EHRSS to EHS (DoctorList)
		No_Log_Response2to4		INT DEFAULT 0,
		No_Log_Response4to6		INT DEFAULT 0,
		No_Log_Response6to8		INT DEFAULT 0,
		No_Log_Response8		INT DEFAULT 0,	  	
		No_Log_Response6		INT DEFAULT 0,	  	  	    
		No_Logs					INT DEFAULT 0,  
		Percentage_Response6	VARCHAR(8) DEFAULT ''
	) 

	-- Voucher Balacne Result
	DECLARE @VoucherBalanceSummary TABLE (  
		Dtm						DATETIME,  
		No_Log_Response0to2		INT DEFAULT 0,			-- EHRSS to EHS (VoucherBalacne)
		No_Log_Response2to4		INT DEFAULT 0,
		No_Log_Response4to6		INT DEFAULT 0,
		No_Log_Response6to8		INT DEFAULT 0,
		No_Log_Response8		INT DEFAULT 0,	  	
		No_Log_Response6		INT DEFAULT 0,	  	  	    
		No_Logs					INT DEFAULT 0,  
		Percentage_Response6	VARCHAR(8) DEFAULT ''
	) 

	-- Patient Portal AuditLog
	DECLARE @EnquiryEHS_AuditLog TABLE (		-- From AuditlogInterface
		System_Dtm			DATETIME,
		Action_Key			VARCHAR(50),
		Log_ID				CHAR(5),
		Action_Dtm			DATETIME,
		End_Dtm				DATETIME,
		[Data]				VARCHAR(MAX),
		[Desc]				VARCHAR(MAX)			
	)
	  
	-- Doctor List Response
	DECLARE @DoctorListResponse TABLE (  
		Dtm					DATETIME,  
		EHSResponseTime		DECIMAL(12,2),
		Result				VARCHAR(100)
	)

	-- Voucher Balacne Response
	DECLARE @VoucherBalanceResponse TABLE (  
		Dtm					DATETIME,  
		EHSResponseTime		DECIMAL(12,2),
		Result				VARCHAR(100)
	)

	-- ===========================================================
	-- Initialization
	-- ===========================================================
	SET @No_Of_Days = 1

	INSERT INTO @EHRSSToEHSFunctionCodeMaster (Function_Code) VALUES ('070202')		-- Patient Portal (EHRSS->EHS)

	INSERT INTO @EHRSSToEHSLogIDMaster (Log_ID) VALUES ('00011')		-- WebMethod geteHSDoctorList End: The E_Action_Dtm and E_End_Dtm in this entry stores the whole processing time
	INSERT INTO @EHRSSToEHSLogIDMaster (Log_ID) VALUES ('00012')		-- WebMethod geteHSVoucherBalacne End: The E_Action_Dtm and E_End_Dtm in this entry stores the whole processing time
	
	IF @Report_Dtm IS NOT NULL 
		BEGIN
			SET @End_Dtm = CONVERT(VARCHAR, DATEADD(dd, 1, @Report_Dtm), 106)
		END 
	ELSE 
		BEGIN
			SET @End_Dtm = CONVERT(VARCHAR, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
		END
	
	SELECT @Start_Dtm = DATEADD(dd, -(@No_Of_Days), @End_Dtm)  	  
	SELECT @Current_Dtm = @Start_Dtm  
	 
	SET @Year = CONVERT(varchar(2), @Start_Dtm, 12)	-- Extract the Calendar Year: "12" gives the format YYMMDD

	-- =============================================  
	-- Clear tables if today records exist
	-- =============================================   

	DELETE FROM eHSD0034_PatientPortalConnectionSummary_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	DELETE FROM eHSD0034_PatientPortalResponseTime_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)

	-- =============================================  
	-- Retrieve data
	-- =============================================  

	-- Retrieve Interface Auditlog
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	INSERT INTO @EnquiryEHS_AuditLog (
			System_Dtm,
			[Data],
			Action_Key,
			Log_ID,
			Action_Dtm,
			End_Dtm,
			[Desc]
		)
		SELECT 
			System_Dtm
			,CONVERT(NVARCHAR(MAX), DecryptByKey(E_Data)) AS [E_Data]
			,CONVERT(VARCHAR(MAX), DecryptByKey(E_Action_Key)) As E_Action_Key
			,CONVERT(VARCHAR(MAX), DecryptByKey(E_Log_ID)) AS [Log_ID]
			,CONVERT(VARCHAR(MAX), DecryptByKey(E_Action_Dtm)) As E_Action_Dtm
			,CONVERT(VARCHAR(MAX), DecryptByKey(E_End_Dtm)) As E_End_Dtm
			,CONVERT(NVARCHAR(MAX), DecryptByKey(E_Description)) AS [E_Description]
		FROM 
			(
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface20] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface21] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface22] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface23] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface24] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface25] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface26] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface27] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface28] with (nolock)
			) AS a
		WHERE
				(System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @EHRSSToEHSFunctionCodeMaster)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHRSSToEHSLogIDMaster)
	
	CLOSE SYMMETRIC KEY sym_Key	

	-- =============================================  
	-- Process data
	-- ============================================= 	

	-- ---------------------------------------------
	-- Doctor List
	-- ---------------------------------------------

	INSERT INTO @DoctorListResponse (
		Dtm,
		EHSResponseTime,
		Result
	)
	SELECT
		System_Dtm,
		CAST(
			CAST(
				DATEDIFF(ms, Action_Dtm, End_Dtm)
			AS decimal(12, 2)) / 1000
		AS decimal(12, 2)) AS [ResponseTime],
		'Normal' AS [Result]
	FROM
		@EnquiryEHS_AuditLog
	WHERE
		Log_ID = '00011'

	-- ---------------------------------------------
	-- Voucher Balance
	-- ---------------------------------------------

	INSERT INTO @VoucherBalanceResponse (
		Dtm,
		EHSResponseTime,
		Result
	)
	SELECT
		System_Dtm,
		CAST(
			CAST(
				DATEDIFF(ms, Action_Dtm, End_Dtm)
			AS decimal(12, 2)) / 1000
		AS decimal(12, 2)) AS [ResponseTime],
		'Normal' AS [Result]
	FROM
		@EnquiryEHS_AuditLog
	WHERE
		Log_ID = '00012'

	-- =============================================  
	-- Retrieve data (For Summary) 
	-- ============================================= 

	WHILE @Current_Dtm < @End_Dtm BEGIN   

		SET @Filter_start_time	=	CONVERT(varchar(4), YEAR(@Current_Dtm)) + RIGHT('00'+ CONVERT(VARCHAR, MONTH(@Current_Dtm)) , 2) + RIGHT('00'+ CONVERT(VARCHAR, DAY(@Current_Dtm)), 2) + '  00:00:00'
		SET @Filter_end_time =		CONVERT(varchar(4), YEAR(@Current_Dtm)) + RIGHT('00'+ CONVERT(VARCHAR, MONTH(@Current_Dtm)) , 2) + RIGHT('00'+ CONVERT(VARCHAR, DAY(@Current_Dtm)), 2) + '  23:59:59'
	 
		-----------------------------------
		-- Doctor List Response 
		-----------------------------------
		INSERT INTO @DoctorListSummary (Dtm) VALUES (@Current_Dtm)

		UPDATE  
			@DoctorListSummary
		SET
			No_Log_Response0to2 = ISNULL(EHS.[No_Log_ResponseP1], 0),
			No_Log_Response2to4 = ISNULL(EHS.[No_Log_ResponseP2], 0),
			No_Log_Response4to6 = ISNULL(EHS.[No_Log_ResponseP3], 0),
			No_Log_Response6to8 = ISNULL(EHS.[No_Log_ResponseP4], 0),
			No_Log_Response8 = ISNULL(EHS.[No_Log_ResponseP5], 0),
			No_Log_Response6 = ISNULL(EHS.[No_Log_ResponseP6], 0),
			No_Logs = ISNULL(EHS.No_Logs, 0)
		FROM
			(
				SELECT
					SUM(CASE WHEN EHSResponseTime < 10.00 THEN 1 ELSE 0 END) [No_Log_ResponseP1],
					SUM(CASE WHEN EHSResponseTime >= 10.00 and EHSResponseTime < 12.00 THEN 1 ELSE 0 END) [No_Log_ResponseP2],
					SUM(CASE WHEN EHSResponseTime >= 12.00 and EHSResponseTime < 15.00 THEN 1 ELSE 0 END) [No_Log_ResponseP3],
					SUM(CASE WHEN EHSResponseTime >= 15.00 and EHSResponseTime < 20.00 THEN 1 ELSE 0 END) [No_Log_ResponseP4],
					SUM(CASE WHEN EHSResponseTime >= 20.00 THEN 1 ELSE 0 END) [No_Log_ResponseP5],
					SUM(CASE WHEN EHSResponseTime >= 15.00 THEN 1 ELSE 0 END) [No_Log_ResponseP6],
					COUNT(1) [No_Logs]
				FROM
					@DoctorListResponse
				WHERE 
					Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			) EHS
		WHERE 
			Dtm = @Current_Dtm 
	

		--Calculate Percentage 
		UPDATE  
		   @DoctorListSummary  
		SET  
			[Percentage_Response6] = 
				CONVERT(VARCHAR(MAX) ,
					(SELECT 
						CASE WHEN No_Log_Response6 = 0 THEN 0 ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6 / No_Logs)) END 
					FROM 
						@DoctorListSummary 
					WHERE 
						Dtm = @Current_Dtm)) + '%'
		WHERE 
			Dtm = @Current_Dtm 
	
		-----------------------------------
		-- Voucher Balance Response Time
		-----------------------------------
		INSERT INTO @VoucherBalanceSummary (Dtm) VALUES (@Current_Dtm)

		UPDATE  
			@VoucherBalanceSummary
		SET
			No_Log_Response0to2 = ISNULL(EHS.[No_Log_Response0to2], 0),
			No_Log_Response2to4 = ISNULL(EHS.[No_Log_Response2to4], 0),
			No_Log_Response4to6 = ISNULL(EHS.[No_Log_Response4to6], 0),
			No_Log_Response6to8 = ISNULL(EHS.[No_Log_Response6to8], 0),
			No_Log_Response8 = ISNULL(EHS.[No_Log_Response8], 0),
			No_Log_Response6 = ISNULL(EHS.[No_Log_Response6], 0),
			No_Logs = ISNULL(EHS.No_Logs, 0)
		FROM
		(
			SELECT
				SUM(CASE WHEN EHSResponseTime < 2.00 THEN 1 ELSE 0 END) [No_Log_Response0to2],
				SUM(CASE WHEN EHSResponseTime >= 2.00 and EHSResponseTime < 4.00 THEN 1 ELSE 0 END) [No_Log_Response2to4],
				SUM(CASE WHEN EHSResponseTime >= 4.00 and EHSResponseTime < 6.00 THEN 1 ELSE 0 END) [No_Log_Response4to6],
				SUM(CASE WHEN EHSResponseTime >= 6.00 and EHSResponseTime < 8.00 THEN 1 ELSE 0 END) [No_Log_Response6to8],
				SUM(CASE WHEN EHSResponseTime >= 8.00 THEN 1 ELSE 0 END) [No_Log_Response8],
				SUM(CASE WHEN EHSResponseTime >= 6.00 THEN 1 ELSE 0 END) [No_Log_Response6],
				COUNT(1) [No_Logs]
			FROM
				@VoucherBalanceResponse
			WHERE 
				Dtm BETWEEN @Filter_start_time AND @Filter_end_time
		) EHS
		WHERE 
			Dtm = @Current_Dtm 

		--Calculate Percentage 
		UPDATE  
		   @VoucherBalanceSummary  
		SET  
			Percentage_Response6 = 
				CONVERT(VARCHAR(MAX) ,
					(SELECT 
						CASE WHEN No_Log_Response6 = 0 THEN 0 ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6 / No_Logs)) END 
					FROM 
						@VoucherBalanceSummary 
					WHERE Dtm = @Current_Dtm)) + '%'
		WHERE 
			Dtm = @Current_Dtm 

		SET @Current_Dtm = DATEADD(dd, 1, @Current_Dtm)  
	    
	END -- While

	-- =============================================
	-- Result
	-- =============================================


	-- Insert record, Store into tables
	------------------ Summary ------------------
	INSERT INTO eHSD0034_PatientPortalConnectionSummary_Stat (
		System_Dtm,
		Report_Dtm,
		Web_Service_Type,
		No_Log_ResponseP1, 
		No_Log_ResponseP2, 
		No_Log_ResponseP3, 
		No_Log_ResponseP4, 
		No_Log_ResponseP5,
		No_Logs,  
		Percentage_Response
	) 
	SELECT
		GETDATE(),
		CONVERT(VARCHAR(10), Dtm, 20),  -- To yyyy-mm-dd	
		'VoucherBalance',
		No_Log_Response0to2, 
		No_Log_Response2to4, 
		No_Log_Response4to6, 
		No_Log_Response6to8, 
		No_Log_Response8,		  		  		  		   
		No_Logs,  
		Percentage_Response6
	FROM
		@VoucherBalanceSummary
	

	INSERT INTO eHSD0034_PatientPortalConnectionSummary_Stat (
		System_Dtm,
		Report_Dtm,  
		Web_Service_Type,
		No_Log_ResponseP1, 
		No_Log_ResponseP2, 
		No_Log_ResponseP3, 
		No_Log_ResponseP4, 
		No_Log_ResponseP5,
		No_Logs,  
		Percentage_Response
	) 
	SELECT
		GETDATE(),
		CONVERT(VARCHAR(10), Dtm, 20),  -- To yyyy-mm-dd	 		
		'DoctorList',
		No_Log_Response0to2, 
		No_Log_Response2to4, 
		No_Log_Response4to6, 
		No_Log_Response6to8, 
		No_Log_Response8,		  		  		  		   
		No_Logs,  
		Percentage_Response6
	FROM
		@DoctorListSummary

	------------------ Response time of web service for Patient Portal-----------------
	INSERT INTO eHSD0034_PatientPortalResponseTime_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Web_Service_Type,
		  Response_Time
	) 
	SELECT   
		GETDATE(),
	    Dtm,
		'DoctorList',
		EHSResponseTime
	FROM 
		@DoctorListResponse
	UNION ALL
	SELECT   
		  GETDATE(),
	      Dtm,
		  'VoucherBalance',
		  EHSResponseTime
	FROM 
		@VoucherBalanceResponse	

END
GO

GRANT EXECUTE ON [dbo].[proc_PatientPortalConnection_Stat_Write] TO HCVU
GO

