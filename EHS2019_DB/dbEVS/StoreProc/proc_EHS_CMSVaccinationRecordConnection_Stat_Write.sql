IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	02 Nov 2018
-- CR No.:			CRE18-012 (Revise eHSD0018)
-- Description:	  	Include num. of patient of each enquiry
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Winnie SUEN
-- Modified date:	22 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	(1) Revise report to include CIMS connection checking
--					(2) Fine tune performance
--					(3) Retrieve [InterfaceLog] for EHS to CMS audit log instead of audit log from [HCSP/VU] 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 January 2017
-- CR No.			INT16-0031 (Fix eHSD0018 due to audit log table change)
-- Description:		Remove the SELECT * FROM
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	24 Mar 2016
-- CR No.			I-CRE16-001
-- Description:		Revise the criteria of CMS log monitor in eHS(S)
-- =============================================
-- =============================================
-- Modification History
-- CR#:			CRE13-019-02
-- Modified by:	Karl LAM	
-- Modified date:	2015 Mar 10
-- Description:		1. Remove hardcoding 'HCVS'
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			INT14-0005
-- Modified by: 	Chris YIM
-- Modified date:   13 Mar 2014
-- Description:		1. Reduce chance of deadlock by adding nolock on all physical tables
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRE12-012
-- Modified by: 	Koala CHENG
-- Modified date:   28 Sep 2012
-- Description:		1. Rectify dataype size for AuditLogHCVU user id (8 -> 20)
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRP11-029 
-- Modified by: 	Koala CHENG
-- Modified date:   19 Apr 2012
-- Description:		1. Add EHS -> CMS health check performance
--					2. Rearrange report layout
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 March 2011
-- Description:		(1) Create master tables for easier configuration
--					(2) Separate CMS->EHS health check enquiries and non-health check enquiries in eHSD0018-01 : eVaccination Record Connection Summary
--					(3) Add indicator for health check enquiries and non-health check enquiries in eHSD0018-05 : Report on Interface Web Service for CMS
--					(4) Include EHS->CMS enquiries in VU and interface platforms
--					(5) Include upload claim and outside claim records for transactions (Rollback the 2nd change in 17 Jan 2011)
--					(6) Add parameter @Report_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala Cheng
-- Modified date:	02 Feb 2011	
-- Description:		[CRE11-002] Amend "Audit Log Error (Get CMS Vaccination)" unknown error definition
--					New Log ID : 01009, 01026, 01027, 01028
--					Old: Unknown Error  > Log ID - 01010
--						 Internal Error > Log ID - 01012
--						 Timeout        > Log ID - 01011
--					New: Unknown Error  > Log ID - 01010 or 01009 or 01026 or 01028
--						 Internal Error > Log ID - 01012 or 01027
--						 Timeout        > Log ID - 01011
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	17 Jan 2011	
-- Description:		Add Statistics of enquiry CMS web service
--					For voucher transaction, filter out upload claim and outside claim records
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	10 Jan 2011	
-- Description:		bug fix for cross year
-- =============================================
-- =============================================
-- Author:			Paul Yip
-- Create date:		3 Decemeber 2010
-- Description:		Statistics for CMS Vaccination Record Connection
--					Preparation (Insert into temp tables)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Write]
	@Report_Dtm		datetime = NULL
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting 
-- =============================================
	DECLARE @No_Of_Days int  
	SELECT @No_Of_Days = 1
	
	
--	
	CREATE TABLE #SchemeCodeVaccine(
	Scheme_Code char(10))
	
	INSERT INTO 	#SchemeCodeVaccine
	SELECT Distinct sc.Scheme_Code 
	FROM  SchemeClaim sc with (nolock) inner join SubsidizeGroupClaim sgc with (nolock) on sc.Scheme_Code = sgc.Scheme_Code
	inner join Subsidize s with (nolock) on sgc.Subsidize_Code = s.Subsidize_Code
	inner join SubsidizeItem si with (nolock) on s.Subsidize_Item_Code = si.Subsidize_Item_Code
	WHERE si.Subsidize_Type = 'VACCINE'

--

	CREATE TABLE #VoucherTx (
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		HA_Vaccine_Ref_Status	VARCHAR(10),
		DH_Vaccine_Ref_Status	CHAR(10)
	)

--
	DECLARE @EHSToCMSFunctionCodeMaster table (
		Function_Code	char(6)
	)

	INSERT INTO @EHSToCMSFunctionCodeMaster (Function_Code) VALUES ('060104')	-- EHS > HA CMS

	DECLARE @EHSToCMSLogIDMaster table (
		Log_ID			char(5)
	)
	
	INSERT INTO @EHSToCMSLogIDMaster (Log_ID) VALUES ('00004')	-- [EHS>CMS] Receive response fail 
	INSERT INTO @EHSToCMSLogIDMaster (Log_ID) VALUES ('00005')	-- [EHS>CMS] Receive response success	

--
	DECLARE @EHSToCIMSFunctionCodeMaster table (
		Function_Code	char(6)
	)

	INSERT INTO @EHSToCIMSFunctionCodeMaster (Function_Code) VALUES ('060105')	-- EHS > HA CIMS

	DECLARE @EHSToCIMSLogIDMaster table (
		Log_ID			char(5)
	)
	
	INSERT INTO @EHSToCIMSLogIDMaster (Log_ID) VALUES ('00004')	-- [EHS>CIMS] Receive response fail 
	INSERT INTO @EHSToCIMSLogIDMaster (Log_ID) VALUES ('00005')	-- [EHS>CIMS] Receive response success	

--

	DECLARE @CMSToEHSFunctionCodeMaster table (
		Function_Code	char(6)
	)
	
	INSERT INTO @CMSToEHSFunctionCodeMaster (Function_Code) VALUES ('060101')		-- eVaccination Record Sharing (CMS->EHS)

	DECLARE @CMSToEHSLogIDMaster table (
		Log_ID			char(5)
	)
	
	INSERT INTO @CMSToEHSLogIDMaster (Log_ID) VALUES ('00003')		-- WebMethod geteHSVaccineRecord End: The E_Action_Dtm and E_End_Dtm in this entry stores the whole processing time
	--INSERT INTO @CMSToEHSLogIDMaster (Log_ID) VALUES ('00004')	-- CMS Request: The E_Data stores the CMS to EHS XML


--

	DECLARE @EHSToCMSAllErrorLogIDMaster table (
		Log_ID			char(5)
	)

	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01009')	-- Get CMS Vaccination fail: Invalid parameter		
	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01010')	-- Get CMS Vaccination fail: Unknown error
	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01011')	-- Get CMS Vaccination fail: Communication link error
	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01012')	-- Get CMS Vaccination fail: EHS internal error
	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01026')	-- Get CMS Vaccination fail: CMS result Message ID mismatch with EHS request Message ID
	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01027')	-- Get CMS Vaccination fail: EAI Service Interruption
	INSERT INTO @EHSToCMSAllErrorLogIDMaster (Log_ID) VALUES ('01028')	-- Get CMS Vaccination fail: Returned health check result incorrect (Return Code: 100)

-- 

	DECLARE @EHSToCIMSAllErrorLogIDMaster table (
		Log_ID			char(5)
	)

	INSERT INTO @EHSToCIMSAllErrorLogIDMaster (Log_ID) VALUES ('01109')	-- Get CIMS Vaccination fail: Invalid parameter
	INSERT INTO @EHSToCIMSAllErrorLogIDMaster (Log_ID) VALUES ('01110')	-- Get CIMS Vaccination fail: Unknown error
	INSERT INTO @EHSToCIMSAllErrorLogIDMaster (Log_ID) VALUES ('01111')	-- Get CIMS Vaccination fail: Communication link error
	INSERT INTO @EHSToCIMSAllErrorLogIDMaster (Log_ID) VALUES ('01112')	-- Get CIMS Vaccination fail: EHS internal error
	INSERT INTO @EHSToCIMSAllErrorLogIDMaster (Log_ID) VALUES ('01126')	-- Get CIMS Vaccination fail: CIMS result client mismatch with EHS request client
	INSERT INTO @EHSToCIMSAllErrorLogIDMaster (Log_ID) VALUES ('01128')	-- Get CIMS Vaccination fail: Returned health check result incorrect (Return Code: 10001)

--
	DECLARE @CMSHealthCheckProgramIDMaster table (
		Program_ID	VARCHAR(30)
	)

	INSERT INTO @CMSHealthCheckProgramIDMaster (Program_ID) VALUES ('110101')	-- CMS Health Check

	DECLARE @EHSToCMSHealthCheckLogIDMaster table (
		Log_ID			VARCHAR(10)
	)
	
	INSERT INTO @EHSToCMSHealthCheckLogIDMaster (Log_ID) VALUES ('00001')	-- EHS -> CMS Health Check End

	DECLARE @EHSToCIMSHealthCheckLogIDMaster table (
		Log_ID			VARCHAR(10)
	)

	INSERT INTO @EHSToCIMSHealthCheckLogIDMaster (Log_ID) VALUES ('00021')	-- EHS -> CIMS Health Check End

--
-- =============================================  
-- Constant
-- =============================================
	DECLARE @HA_Vaccine_Ref_Status_CN char(3)
	SET @HA_Vaccine_Ref_Status_CN = '_CN'
	
	DECLARE @HA_Vaccine_Ref_Status_UN char(3)
	SET @HA_Vaccine_Ref_Status_UN = '_UN'
	
	DECLARE @DH_Vaccine_Ref_Status_CN char(3)
	SET @DH_Vaccine_Ref_Status_CN = '_CN'
	
	DECLARE @DH_Vaccine_Ref_Status_UN char(3)
	SET @DH_Vaccine_Ref_Status_UN = '_UN'

	DECLARE @Get_From_Session varchar(50)
	SET @Get_From_Session = '%Get from session%'
 
	DECLARE @Health_Check_Y NVARCHAR(50)
	SET @Health_Check_Y = '%<HealthCheck: Y>%'

	DECLARE @Batch_Enquiry_Y NVARCHAR(50)
	SET @Batch_Enquiry_Y = '%<BatchEnquiry: Y>%'

-- =============================================  
-- Temporary tables  
-- =============================================  
	DECLARE @CMSSummary table (  
		Dtm      datetime,  
		No_Fail_Vaccine_Trx				int Default 0,
		No_Vaccine_Trx					int Default 0,  
		No_Vaccine_Trx_no_CMS_enquire int Default 0,
		Percentage_Fail   varchar(8) Default ''  ,  
		No_Communication_Link_Err_101	int	Default 0,
		No_Unknown_Err_99				int	Default 0,
		No_Invalid_Parameter_98			int	Default 0,
		No_ID_Mismatch_104				int	Default 0,
		No_Health_Check_Result_Incorrect_100	int	Default 0,
		No_EHS_internal_Err_102			int	Default 0,
		No_EAI_Service_interruption_105	int	Default 0,
		Total							int Default 0,  
		No_Log_Response0to2_Enquire   int Default 0,			-- EHS to CMS (Single)
		No_Log_Response2to4_Enquire   int Default 0,
		No_Log_Response4to6_Enquire   int Default 0,
		No_Log_Response6to8_Enquire   int Default 0,
		No_Log_Response8_Enquire   int Default 0,	  	
		No_Log_Response6_Enquire   int Default 0,	  	  	    
		No_Logs_Enquire   int Default 0,  
		Percentage_Response6_Enquire   varchar(8) Default '',  
		No_Log_Response0to2   int Default 0,					-- CMS to EHS (Single)
		No_Log_Response2to4   int Default 0,
		No_Log_Response4to6   int Default 0,
		No_Log_Response6to8   int Default 0,
		No_Log_Response8   int Default 0,	  	
		No_Log_Response6   int Default 0,	  	  	    
		No_Logs   int Default 0,  
		Percentage_Response6	varchar(8) Default '',
		No_Log_Response0to2_Health_Check	int Default 0,				-- EHS to CMS Health Check
		No_Log_Response2to4_Health_Check	int Default 0,
		No_Log_Response4to6_Health_Check	int Default 0,
		No_Log_Response6to8_Health_Check	int Default 0,
		No_Log_Response8_Health_Check		int Default 0,
		No_Log_Response6_Health_Check		int Default 0,
		No_Logs_Health_Check				int Default 0,
		Percentage_Response6_Health_Check	varchar(8) Default '',			-- CMS to EHS Health Check
		No_Log_Response0to2_Enquire_Health_Check	int Default 0,  
		No_Log_Response2to4_Enquire_Health_Check	int Default 0,  
		No_Log_Response4to6_Enquire_Health_Check	int Default 0,  
		No_Log_Response6to8_Enquire_Health_Check	int Default 0,  
		No_Log_Response8_Enquire_Health_Check		int Default 0,  
		No_Log_Response6_Enquire_Health_Check		int Default 0,
		No_Logs_Enquire_Health_Check				int Default 0,  
		Percentage_Response6_Enquire_Health_Check	varchar(8) Default '',
		No_Log_ResponseP1_Enquire_Batch   int Default 0,			-- EHS to CMS (Batch Enquiry)
		No_Log_ResponseP2_Enquire_Batch   int Default 0,
		No_Log_ResponseP3_Enquire_Batch   int Default 0,
		No_Log_ResponseP4_Enquire_Batch   int Default 0,
		No_Log_ResponseP5_Enquire_Batch   int Default 0,	  	
		No_Log_ResponseP6_Enquire_Batch   int Default 0,	  	  	    
		No_Logs_Enquire_Batch   int Default 0,  
		Percentage_ResponseP6_Enquire_Batch   varchar(8) Default '',  
		No_Log_ResponseP1_Batch   int Default 0,					-- CMS to EHS (Batch Enquiry)
		No_Log_ResponseP2_Batch   int Default 0,
		No_Log_ResponseP3_Batch   int Default 0,
		No_Log_ResponseP4_Batch   int Default 0,
		No_Log_ResponseP5_Batch		int Default 0,	  	
		No_Log_ResponseP6_Batch		int Default 0,	  	  	    
		No_Logs_Batch				int Default 0,
		Percentage_ResponseP6_Batch   varchar(8) Default ''
	) 

	DECLARE @CIMSSummary table (  
		Dtm      datetime,  
		No_Fail_Vaccine_Trx				int Default 0,  
		No_Vaccine_Trx					int Default 0,  
		No_Vaccine_Trx_no_CIMS_enquire	int Default 0,
		Percentage_Fail					varchar(8) Default ''  ,  
		No_Communication_Link_Err_90001	int Default 0,
		No_Unknown_Err_99999			int Default 0,
		No_Invalid_Parameter_90005		int Default 0,
		No_Client_Mismatch_90004		int Default 0,
		No_Health_Check_Result_Incorrect_10001 int Default 0,
		No_EHS_internal_Err_90002		int Default 0,
		Total							int Default 0,  
		No_Log_Response0to2_Enquire		int Default 0,			-- EHS to CIMS (Single)
		No_Log_Response2to4_Enquire		int Default 0,
		No_Log_Response4to6_Enquire		int Default 0,
		No_Log_Response6to8_Enquire		int Default 0,
		No_Log_Response8_Enquire		int Default 0,	  	
		No_Log_Response6_Enquire		int Default 0,	  	  	    
		No_Logs_Enquire					int Default 0,  
		Percentage_Response6_Enquire	varchar(8) Default '',  
		No_Log_Response0to2		int Default 0,				-- CIMS to EHS (Single)
		No_Log_Response2to4		int Default 0,
		No_Log_Response4to6		int Default 0,
		No_Log_Response6to8		int Default 0,
		No_Log_Response8		int Default 0,	  	
		No_Log_Response6		int Default 0,	  	  	    
		No_Logs					int Default 0,  
		Percentage_Response6	varchar(8) Default '',
		No_Log_Response0to2_Health_Check	int Default 0,		-- EHS to CIMS Health Check
		No_Log_Response2to4_Health_Check	int Default 0,
		No_Log_Response4to6_Health_Check	int Default 0,
		No_Log_Response6to8_Health_Check	int Default 0,
		No_Log_Response8_Health_Check		int Default 0,
		No_Log_Response6_Health_Check		int Default 0,
		No_Logs_Health_Check				int Default 0,
		Percentage_Response6_Health_Check	varchar(8) Default '',	-- CIMS to EHS Health Check
		No_Log_Response0to2_Enquire_Health_Check	int Default 0,  
		No_Log_Response2to4_Enquire_Health_Check	int Default 0,  
		No_Log_Response4to6_Enquire_Health_Check	int Default 0,  
		No_Log_Response6to8_Enquire_Health_Check	int Default 0,  
		No_Log_Response8_Enquire_Health_Check		int Default 0,  
		No_Log_Response6_Enquire_Health_Check		int Default 0,
		No_Logs_Enquire_Health_Check				int Default 0,  
		Percentage_Response6_Enquire_Health_Check	varchar(8) Default '',
		No_Log_ResponseP1_Enquire_Batch		int Default 0,		-- EHS to CIMS (Batch Enquiry)
		No_Log_ResponseP2_Enquire_Batch		int Default 0,
		No_Log_ResponseP3_Enquire_Batch		int Default 0,
		No_Log_ResponseP4_Enquire_Batch		int Default 0,
		No_Log_ResponseP5_Enquire_Batch		int Default 0,	  	
		No_Log_ResponseP6_Enquire_Batch		int Default 0,	  	  	    
		No_Logs_Enquire_Batch				int Default 0,  
		Percentage_ResponseP6_Enquire_Batch	varchar(8) Default '',  
		No_Log_ResponseP1_Batch				int Default 0,		-- CIMS to EHS (Batch Enquiry)
		No_Log_ResponseP2_Batch				int Default 0,
		No_Log_ResponseP3_Batch				int Default 0,
		No_Log_ResponseP4_Batch				int Default 0,
		No_Log_ResponseP5_Batch				int Default 0,	  	
		No_Log_ResponseP6_Batch				int Default 0,	  	  	    
		No_Logs_Batch						int Default 0,
		Percentage_ResponseP6_Batch			varchar(8) Default ''
	) 


	DECLARE @Evacc_ErrorResult table (					-- From AuditlogHCSP / AuditlogHCVU
		System_Dtm			DATETIME, 
		Log_ID				CHAR(5)
	)

--
-- *****************************
-- Enquiry EHS 
-- *****************************	
	DECLARE @EnquiryEHS_AuditLog table (		-- From AuditlogInterface
		System_Dtm			datetime,
		Action_Key			varchar(50),
		Log_ID				char(5),
		Action_Dtm			datetime,
		End_Dtm				datetime,
		Data				varchar(MAX),
		Description			varchar(MAX)			
	)
	  
	-- CMS to EHS
	DECLARE @CMSToEHS table (  
		Dtm					datetime,  
		EHSResponseTime		DECIMAL(12,2),
		Result				VARCHAR(100),
		Health_Check		char(1),
		Batch_Enquiry		CHAR(1)
	)

	-- CIMS to EHS
	DECLARE @CIMSToEHS table (  
		Dtm					DATETIME,  
		EHSResponseTime		DECIMAL(12,2),
		Result				VARCHAR(100),
		Health_Check		CHAR(1),
		Batch_Enquiry		CHAR(1)
	)

--
-- *****************************
-- Enquiry HA CMS
-- *****************************	

	-- EHS to CMS (Health Check)
	DECLARE @EHStoCMS_HealthCheck table (	-- From ScheduleJobLog
		Dtm					datetime,  
		CMSResponseTime		DECIMAL(12,2),
		Result				VARCHAR(100),
		Health_Check		char(1)
	)

--

	-- EHS to CMS (Single Enquiry / Batch Enquiry)
	DECLARE @EHSToCMSAuditLog table (		-- From AuditlogInterface
		System_Dtm			DATETIME,
		Action_Key			VARCHAR(50),
		Log_ID				CHAR(5),
		Action_Dtm			DATETIME,
		End_Dtm				DATETIME,
		Data				NVARCHAR(MAX),
		[Description]		NVARCHAR(MAX)
	)

	DECLARE @EnquiryCMS_Result TABLE (
		start_time			datetime,
		end_time			datetime,
		CMSResponseTime		DECIMAL(12,2),
		Batch_Enquiry		CHAR(1),
		NumOfPatient		INT
	)

	DECLARE @EHSToCMS_ReturnCode_Result TABLE (
		LOG_ID			CHAR(5),
		COUNT_LOG_ID	INT
	)

--
-- *****************************
-- Enquiry DH CIMS
-- *****************************	

	-- EHS to CIMS (Health Check)
	DECLARE @EHStoCIMS_HealthCheck table (	-- From ScheduleJobLog
		Dtm					datetime,  
		CIMSResponseTime	DECIMAL(12,2),
		Result				VARCHAR(100),
		Health_Check		char(1)
	)

--

	-- EHS to CIMS (Single Enquiry / Batch Enquiry)
	DECLARE @EHSToCIMSAuditLog table (		-- From AuditlogInterface
		System_Dtm			DATETIME,
		Action_Key			VARCHAR(50),
		Log_ID				CHAR(5),
		Action_Dtm			DATETIME,
		End_Dtm				DATETIME,
		Data				NVARCHAR(MAX),
		[Description]		NVARCHAR(MAX)
	)

	DECLARE @EnquiryCIMS_Result TABLE (
		start_time			datetime,
		end_time			datetime,
		CIMSResponseTime	DECIMAL(12,2),
		Batch_Enquiry		CHAR(1),
		NumOfPatient		INT
	)

	DECLARE @EHSToCIMS_ReturnCode_Result TABLE (
		LOG_ID			CHAR(5),
		COUNT_LOG_ID	INT
	)

-- =============================================
-- Debugging use
-- =============================================

/*
	DECLARE @eHSD0018_CMSVaccinationConnectionSummary_Stat table (
		System_Dtm							datetime,
		Report_Dtm							datetime,
		No_Fail_Vaccine_Trx					int,
		No_Vaccine_Trx						int,
		Percentage_Fail						varchar(8),
		No_Communication_Link_Err_101		int,
		No_Unknown_Err_99					int,
		No_Invalid_Parameter_98				int,
		No_ID_Mismatch_104					int,
		No_Health_Check_Result_Incorrect_100	int,
		No_EHS_Internal_Err_102				int,
		No_EAI_Service_Interruption_105		int,
		Total								int,
		No_Log_Response0to2					int,
		No_Log_Response2to4					int,
		No_Log_Response4to6					int,
		No_Log_Response6to8					int,
		No_Log_Response8					int,
		No_Logs								int,
		Percentage_Response6				varchar(8),
		No_Log_Response0to2_Enquire			int,
		No_Log_Response2to4_Enquire			int,
		No_Log_Response4to6_Enquire			int,
		No_Log_Response6to8_Enquire			int,
		No_Log_Response8_Enquire			int,
		No_Logs_Enquire						int,
		Percentage_Response6_Enquire		varchar(8),
		No_Vaccine_Trx_no_CMS_enquire		int,
		No_Log_Response0to2_Health_Check	int,
		No_Log_Response2to4_Health_Check	int,
		No_Log_Response4to6_Health_Check	int,
		No_Log_Response6to8_Health_Check	int,
		No_Log_Response8_Health_Check		int,
		No_Log_Response6_Health_Check		int,
		No_Logs_Health_Check				int,
		Percentage_Response6_Health_Check	varchar(8),
		No_Log_Response0to2_Enquire_Health_Check	int,
		No_Log_Response2to4_Enquire_Health_Check	int,
		No_Log_Response4to6_Enquire_Health_Check	int,
		No_Log_Response6to8_Enquire_Health_Check	int,
		No_Log_Response8_Enquire_Health_Check		int,
		No_Log_Response6_Enquire_Health_Check		int,
		No_Logs_Enquire_Health_Check				int,
		Percentage_Response6_Enquire_Health_Check	varchar(8),
		No_Log_ResponseP1_Batch	int,
		No_Log_ResponseP2_Batch	int,
		No_Log_ResponseP3_Batch	int,
		No_Log_ResponseP4_Batch	int,
		No_Log_ResponseP5_Batch		int,
		No_Logs_Batch				int,
		Percentage_ResponseP6_Batch	varchar(8),
		No_Log_ResponseP1_Enquire_Batch	int,
		No_Log_ResponseP2_Enquire_Batch	int,
		No_Log_ResponseP3_Enquire_Batch	int,
		No_Log_ResponseP4_Enquire_Batch	int,
		No_Log_ResponseP5_Enquire_Batch		int,
		No_Logs_Enquire_Batch				int,
		Percentage_ResponseP6_Enquire_Batch	varchar(8)
	)

	DECLARE @eHSD0018_CIMSVaccinationConnectionSummary_Stat table (
		System_Dtm							datetime,
		Report_Dtm							datetime,
		No_Fail_Vaccine_Trx					int,
		No_Vaccine_Trx						int,
		Percentage_Fail						varchar(8),
		No_Communication_Link_Err_90001		int,
		No_Unknown_Err_99999				int,
		No_Invalid_Parameter_90005			int,
		No_Client_Mismatch_90004			int,
		No_Health_Check_Result_Incorrect_10001	int,
		No_EHS_internal_Err_90002			int,
		Total								int,
		No_Log_Response0to2					int,
		No_Log_Response2to4					int,
		No_Log_Response4to6					int,
		No_Log_Response6to8					int,
		No_Log_Response8					int,
		No_Logs								int,
		Percentage_Response6				varchar(8),
		No_Log_Response0to2_Enquire			int,
		No_Log_Response2to4_Enquire			int,
		No_Log_Response4to6_Enquire			int,
		No_Log_Response6to8_Enquire			int,
		No_Log_Response8_Enquire			int,
		No_Logs_Enquire						int,
		Percentage_Response6_Enquire		varchar(8),
		No_Vaccine_Trx_no_CIMS_enquire		int,
		No_Log_Response0to2_Health_Check	int,
		No_Log_Response2to4_Health_Check	int,
		No_Log_Response4to6_Health_Check	int,
		No_Log_Response6to8_Health_Check	int,
		No_Log_Response8_Health_Check		int,
		No_Log_Response6_Health_Check		int,
		No_Logs_Health_Check				int,
		Percentage_Response6_Health_Check	varchar(8),
		No_Log_Response0to2_Enquire_Health_Check	int,
		No_Log_Response2to4_Enquire_Health_Check	int,
		No_Log_Response4to6_Enquire_Health_Check	int,
		No_Log_Response6to8_Enquire_Health_Check	int,
		No_Log_Response8_Enquire_Health_Check		int,
		No_Log_Response6_Enquire_Health_Check		int,
		No_Logs_Enquire_Health_Check				int,
		Percentage_Response6_Enquire_Health_Check	varchar(8),
		No_Log_ResponseP1_Batch	int,
		No_Log_ResponseP2_Batch	int,
		No_Log_ResponseP3_Batch	int,
		No_Log_ResponseP4_Batch	int,
		No_Log_ResponseP5_Batch		int,
		No_Logs_Batch				int,
		Percentage_ResponseP6_Batch	varchar(8),
		No_Log_ResponseP1_Enquire_Batch	int,
		No_Log_ResponseP2_Enquire_Batch	int,
		No_Log_ResponseP3_Enquire_Batch	int,
		No_Log_ResponseP4_Enquire_Batch	int,
		No_Log_ResponseP5_Enquire_Batch		int,
		No_Logs_Enquire_Batch				int,
		Percentage_ResponseP6_Enquire_Batch	varchar(8)
	)

	DECLARE @eHSD0018_CMSVaccinationConnectionStatus_Stat table (
		System_Dtm			datetime,
		Report_Dtm			datetime,
		Connection_Status	varchar(30),
		Connect_System		char(10)
	)

	DECLARE @eHSD0018_CMSVaccinationConnectionErr_Stat table (
		System_Dtm			datetime,
		Report_Dtm			datetime,
		Error_Type			varchar(30),
		Connect_System		char(10)
	)

	DECLARE @eHSD0018_CMSWebServiceResponseTime_Stat table (
		System_Dtm			datetime,
		Report_Dtm			datetime,
		Response_Time		varchar(30),
		Health_Check		char(1),
		Batch_Enquiry		char(1)
	)

	DECLARE @eHSD0018_EnquireCMSWebServiceResponseTime_Stat table (
		System_Dtm			datetime,
		Report_Dtm			datetime,
		Response_Time		varchar(30),
		Health_Check		char(1),
		Batch_Enquiry		char(1)
	)

	DECLARE @eHSD0018_EnquireCIMSWebServiceResponseTime_Stat table (
		System_Dtm			datetime,
		Report_Dtm			datetime,
		Response_Time		varchar(30),
		Health_Check		char(1),
		Batch_Enquiry		char(1)
	)
*/

-- =============================================
-- Initialization
-- =============================================
	DECLARE @Current_Dtm datetime  
	DECLARE @Filter_start_time datetime
	DECLARE @Filter_end_time datetime

	DECLARE @Start_Dtm datetime  
	DECLARE @End_Dtm datetime  
	   
	DECLARE @Year		smallint	-- For identifying the audit log table
	
	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @End_Dtm = CONVERT(varchar, DATEADD(dd, 1, @Report_Dtm), 106)
	END ELSE BEGIN
		SELECT @End_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END
	
	SELECT @Start_Dtm = DATEADD(dd, -(@No_Of_Days), @End_Dtm)  	  
	SELECT @Current_Dtm = @Start_Dtm  
	 
	SET @Year = CONVERT(varchar(2), @Start_Dtm, 12)	-- Extract the Calendar Year: "12" gives the format YYMMDD

-- =============================================  
-- Clear tables if today records exist
-- =============================================   

	delete from eHSD0018_CMSVaccinationConnectionSummary_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	delete from eHSD0018_CMSVaccinationConnectionStatus_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	delete from eHSD0018_CMSVaccinationConnectionErr_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	delete from eHSD0018_CMSWebServiceResponseTime_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	delete from eHSD0018_EnquireCMSWebServiceResponseTime_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	delete from eHSD0018_EnquireCIMSWebServiceResponseTime_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	delete from eHSD0018_CIMSVaccinationConnectionSummary_Stat WHERE (Report_Dtm >= @Start_Dtm AND Report_Dtm < @End_Dtm)
	

-- =============================================  
-- Retrieve data
-- =============================================  

	-- Retrieve Voucher Transaction
	INSERT INTO #VoucherTx
	SELECT
		Transaction_ID,
		Transaction_Dtm,
		Ext_Ref_Status,
		DH_Vaccine_Ref_Status
	FROM VoucherTransaction VT with (nolock) 
	INNER JOIN #SchemeCodeVaccine v ON VT.Scheme_Code = v.Scheme_Code
	WHERE (transaction_dtm >= @Start_Dtm AND transaction_dtm < @End_Dtm)
	OPTION (RECOMPILE)

-- 

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	INSERT INTO @EnquiryEHS_AuditLog (
			System_Dtm,
			Data,
			Action_Key,
			Log_ID,
			Action_Dtm,
			End_Dtm,
			Description
		)
		SELECT 
			System_Dtm
			,CONVERT(nvarchar(MAX), DecryptByKey(E_Data)) AS [E_Data]
			,CONVERT(varchar(MAX), DecryptByKey(E_Action_Key)) As E_Action_Key
			,CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)) AS [Log_ID]
			,CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)) As E_Action_Dtm
			,CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)) As E_End_Dtm
			,CONVERT(nvarchar(MAX), DecryptByKey(E_Description)) AS [E_Description]
		FROM 
			(
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface10] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface11] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface12] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface13] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface14] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface15] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface16] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface17] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface18] with (nolock)
				UNION ALL
				SELECT System_Dtm, E_Data, E_Description, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface19] with (nolock)
				UNION ALL
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
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @CMSToEHSFunctionCodeMaster)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @CMSToEHSLogIDMaster)
	

	INSERT INTO @EHSToCMSAuditLog(
		System_Dtm,
		Data,
		Action_Key,
		Log_ID,
		Action_Dtm,
		End_Dtm,
		[Description]
	)
	SELECT 
		System_Dtm
		,CONVERT(nvarchar(MAX), DecryptByKey(E_Data)) AS [E_Data]
		,CONVERT(varchar(MAX), DecryptByKey(E_Action_Key)) As E_Action_Key
		,CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)) AS [Log_ID]
		,CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)) As E_Action_Dtm
		,CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)) As E_End_Dtm
		,CONVERT(nvarchar(MAX), DecryptByKey(E_Description)) As E_Description
	FROM 
		(
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface10] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface11] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface12] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface13] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface14] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface15] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface16] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface17] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface18] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface19] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface20] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface21] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface22] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface23] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface24] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface25] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface26] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface27] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface28] with (nolock)
		) AS a
	WHERE
		(System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)	
			AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @EHSToCMSFunctionCodeMaster)
			AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHSToCMSLogIDMaster)
	

	INSERT INTO @EHSToCIMSAuditLog(
		System_Dtm,
		Data,
		Action_Key,
		Log_ID,
		Action_Dtm,
		End_Dtm,
		[Description]
	)
	SELECT 
		System_Dtm
		,CONVERT(nvarchar(MAX), DecryptByKey(E_Data)) AS [E_Data]
		,CONVERT(varchar(MAX), DecryptByKey(E_Action_Key)) As E_Action_Key
		,CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)) AS [Log_ID]
		,CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)) As E_Action_Dtm
		,CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)) As E_End_Dtm
		,CONVERT(nvarchar(MAX), DecryptByKey(E_Description)) As E_Description
	FROM 
		(
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface10] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface11] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface12] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface13] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface14] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface15] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface16] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface17] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface18] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface19] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface20] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface21] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface22] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface23] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface24] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface25] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface26] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface27] with (nolock)
			UNION ALL
			SELECT System_Dtm, E_Description, E_Data, E_Action_Key, E_Function_Code, E_Log_ID, E_Action_Dtm, E_End_Dtm FROM [dbEVS_Interfacelog_Replication]..[AuditLogInterface28] with (nolock)
		) AS a
	WHERE
		(System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)	
			AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @EHSToCIMSFunctionCodeMaster)
			AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHSToCIMSLogIDMaster)


-- ---------------------------------------------
-- EHS->CMS Health Check
-- ---------------------------------------------
	INSERT INTO @EHStoCMS_HealthCheck (
		Dtm,
		CMSResponseTime,
		Result,
		Health_Check
	)
	SELECT
		System_Dtm,
		CAST(
			CAST(
				DATEDIFF(ms, Start_Dtm, End_Dtm)
			AS decimal(12, 2)) / 1000
		AS decimal(12, 2)) AS [CMSResponseTime],
		CASE 
			WHEN Description LIKE '%<ReturnCode: 100>%' THEN 'Normal'
			ELSE 'Unknown Error'
		END AS [Result],
		'Y' AS [Health_Check]
	FROM
		(
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog10] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog11] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog12] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog13] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog14] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog15] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog16] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog17] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog18] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog19] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog20] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog21] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog22] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog23] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog24] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog25] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog26] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog27] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog28] with (nolock)
		) AS a
	WHERE
		Program_ID IN (SELECT Program_ID FROM @CMSHealthCheckProgramIDMaster)
		AND Log_ID IN (SELECT Log_ID FROM @EHSToCMSHealthCheckLogIDMaster)
		AND (System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)



-- ---------------------------------------------
-- EHS->CIMS Health Check
-- ---------------------------------------------
	INSERT INTO @EHStoCIMS_HealthCheck (
		Dtm,
		CIMSResponseTime,
		Result,
		Health_Check
	)
	SELECT
		System_Dtm,
		CAST(
			CAST(
				DATEDIFF(ms, Start_Dtm, End_Dtm)
			AS decimal(12, 2)) / 1000
		AS decimal(12, 2)) AS [CIMSResponseTime],
		CASE 
			WHEN Description LIKE '%<ReturnCode: 10001>%' THEN 'Normal'
			ELSE 'Unknown Error'
		END AS [Result],
		'Y' AS [Health_Check]
	FROM
		(
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog10] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog11] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog12] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog13] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog14] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog15] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog16] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog17] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog18] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog19] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog20] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog21] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog22] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog23] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog24] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog25] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog26] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog27] with (nolock)
			UNION ALL
			SELECT System_Dtm, Start_Dtm, End_Dtm, Program_ID, Log_ID, Description FROM [ScheduleJobLog28] with (nolock)
		) AS a
	WHERE
		Program_ID IN (SELECT Program_ID FROM @CMSHealthCheckProgramIDMaster)
		AND Log_ID IN (SELECT Log_ID FROM @EHSToCIMSHealthCheckLogIDMaster)
		AND (System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)


--
	
	INSERT INTO @Evacc_ErrorResult(
		System_Dtm,
		Log_ID)
	SELECT
		System_Dtm,
		CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)) as LOG_ID
	FROM(
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP10] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP11] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP12] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP13] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP14] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP15] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP16] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP17] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP18] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP19] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP20] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP21] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP22] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP23] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP24] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP25] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP26] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP27] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCSP28] with (nolock)
		) AS TMP
	WHERE	
		(System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)
		AND (E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHSToCMSAllErrorLogIDMaster)
			OR E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHSToCIMSAllErrorLogIDMaster))
		AND CONVERT(nvarchar(MAX), DecryptByKey(E_Description)) NOT LIKE @Get_From_Session

--

	INSERT INTO @Evacc_ErrorResult(
		System_Dtm,
		Log_ID)
	SELECT
		System_Dtm,
		CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)) as LOG_ID
	FROM(
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU10] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU11] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU12] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU13] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU14] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU15] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU16] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU17] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU18] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU19] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU20] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU21] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU22] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU23] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU24] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU25] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU26] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU27] with (nolock)
		UNION ALL
		SELECT System_Dtm, E_Log_ID, E_Description FROM [AuditLogHCVU28] with (nolock)
		) AS TMP
	WHERE	
		(System_Dtm >= @Start_Dtm AND System_Dtm < @End_Dtm)
		AND (E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHSToCMSAllErrorLogIDMaster)
			OR E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @EHSToCIMSAllErrorLogIDMaster))
		AND CONVERT(nvarchar(MAX), DecryptByKey(E_Description)) NOT LIKE @Get_From_Session

	CLOSE SYMMETRIC KEY sym_Key	
-- =============================================  
-- Process data
-- ============================================= 	

-- ---------------------------------------------
-- EHS->CMS Enquire
-- ---------------------------------------------

	-- Prepare CMS Response Time
	INSERT INTO @EnquiryCMS_Result
	(
		start_time,
		end_time,
		CMSResponseTime,
		Batch_Enquiry,
		NumOfPatient
	)
	SELECT	
		Action_Dtm, 
		End_Dtm,
		CAST( CAST( DATEDIFF(ms,  Action_dtm ,  End_Dtm )  AS DECIMAL(12,2) ) / 1000 AS DECIMAL(12,2) ) AS [CMSResponseTime],
		CASE
			WHEN [Description] LIKE @Batch_Enquiry_Y THEN 'Y'
			ELSE 'N'
		END AS [Batch_Enquiry],
		CASE
			WHEN [Description] LIKE @Batch_Enquiry_Y THEN 
				CASE
					WHEN ISNUMERIC(REPLACE(SUBSTRING([Description], CHARINDEX('<NumOfPatient: ', [Description]) + LEN('<NumOfPatient: '), 3),'>','')) = 1 THEN CAST(REPLACE(SUBSTRING([Description], CHARINDEX('<NumOfPatient: ', [Description]) + LEN('<NumOfPatient: '), 3),'>','') AS INTEGER)
					ELSE NULL
				END
			ELSE '1'
		END AS [NumOfPatient]
	FROM
		@EHSToCMSAuditLog


-- ---------------------------------------------
-- EHS->CIMS Enquire
-- ---------------------------------------------
	-- Prepare CIMS Response Time
	INSERT INTO @EnquiryCIMS_Result
	(
		start_time,
		end_time,
		CIMSResponseTime,
		Batch_Enquiry,
		NumOfPatient
	)
	SELECT	
		Action_Dtm, 
		End_Dtm,
		CAST( CAST( DATEDIFF(ms,  Action_dtm ,  End_Dtm )  AS DECIMAL(12,2) ) / 1000 AS DECIMAL(12,2) ) AS [CIMSResponseTime],
		CASE
			WHEN [Description] LIKE @Batch_Enquiry_Y THEN 'Y'
			ELSE 'N'
		END AS [Batch_Enquiry],
		CASE
			WHEN [Description] LIKE @Batch_Enquiry_Y THEN 
				CASE
					WHEN ISNUMERIC(REPLACE(SUBSTRING([Description], CHARINDEX('<NumOfPatient: ', [Description]) + LEN('<NumOfPatient: '), 3),'>','')) = 1 THEN CAST(REPLACE(SUBSTRING([Description], CHARINDEX('<NumOfPatient: ', [Description]) + LEN('<NumOfPatient: '), 3),'>','') AS INTEGER)
					ELSE NULL
				END
			ELSE '1'
		END AS [NumOfPatient]
	FROM
		@EHSToCIMSAuditLog


-- ---------------------------------------------
-- CMS->EHS
-- ---------------------------------------------

	INSERT INTO @CMSToEHS (
		Dtm,
		EHSResponseTime,
		Result,
		Health_Check,
		Batch_Enquiry
	)
	SELECT
		System_Dtm,
		CAST(
			CAST(
				DATEDIFF(ms, Action_Dtm, End_Dtm)
			AS decimal(12, 2)) / 1000
		AS decimal(12, 2)) AS [CMSResponseTime],
		'Normal' AS [Result],
		CASE
			WHEN [Description] LIKE @Health_Check_Y THEN 'Y'
			ELSE 'N'
		END AS [Health_Check],
		CASE
			WHEN [Description] LIKE @Batch_Enquiry_Y THEN 'Y'
			ELSE 'N'
		END AS [Batch_Enquiry]
	FROM
		@EnquiryEHS_AuditLog
	WHERE
		[Description] LIKE '%<RequestSystem: CMS>%'


-- ---------------------------------------------
-- CIMS->EHS
-- ---------------------------------------------

	INSERT INTO @CIMSToEHS (
		Dtm,
		EHSResponseTime,
		Result,
		Health_Check,
		Batch_Enquiry
	)
	SELECT
		System_Dtm,
		CAST(
			CAST(
				DATEDIFF(ms, Action_Dtm, End_Dtm)
			AS decimal(12, 2)) / 1000
		AS decimal(12, 2)) AS [CIMSResponseTime],
		'Normal' AS [Result],
		CASE
			WHEN [Description] LIKE @Health_Check_Y THEN 'Y'
			ELSE 'N'
		END AS [Health_Check],
		CASE
			WHEN [Description] LIKE @Batch_Enquiry_Y THEN 'Y'
			ELSE 'N'
		END AS [Batch_Enquiry]
	FROM
		@EnquiryEHS_AuditLog
	WHERE
		[Description] LIKE '%<RequestSystem: CIMS>%'


-- =============================================  
-- Retrieve data (For Summary) 
-- ============================================= 

WHILE @Current_Dtm < @End_Dtm BEGIN   

	 select @Filter_start_time	= convert(varchar(4), YEAR(@Current_Dtm)) + right('00'+ convert(varchar, MONTH(@Current_Dtm)) , 2) + right('00'+ convert(varchar, DAY(@Current_Dtm)), 2) + '  00:00:00'
	 select @Filter_end_time = convert(varchar(4), YEAR(@Current_Dtm)) + right('00'+ convert(varchar, MONTH(@Current_Dtm)) , 2) + right('00'+ convert(varchar, DAY(@Current_Dtm)), 2) + '  23:59:59'
	 
	 
	 INSERT INTO @CMSSummary (Dtm) VALUES (@Current_Dtm)

	-- tx
	UPDATE  
		@CMSSummary  
	SET  
		No_Fail_Vaccine_Trx = (  
		SELECT count(1) from #VoucherTx
			where transaction_dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND (
					ISNULL(HA_Vaccine_Ref_Status, '') LIKE @HA_Vaccine_Ref_Status_CN
					OR ISNULL(HA_Vaccine_Ref_Status, '') LIKE @HA_Vaccine_Ref_Status_UN
				)
			),  
		No_Vaccine_Trx = (  
		SELECT count(1) from #VoucherTx			
			where transaction_dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND substring(HA_Vaccine_Ref_Status, 2, 1) <> 'D'
			),
		No_Vaccine_Trx_no_CMS_enquire = (  
		SELECT count(1) from #VoucherTx
			where transaction_dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND substring(HA_Vaccine_Ref_Status, 2, 1) = 'D'
			)
	WHERE Dtm = @Current_Dtm 


	-- CMS Response Time
	UPDATE  
		@CMSSummary
	SET
		No_Log_Response0to2_Enquire = ISNULL(CMS_Single.No_Log_Response0to2_Enquire, 0),
		No_Log_Response2to4_Enquire = ISNULL(CMS_Single.No_Log_Response2to4_Enquire, 0),
		No_Log_Response4to6_Enquire = ISNULL(CMS_Single.No_Log_Response4to6_Enquire, 0),
		No_Log_Response6to8_Enquire = ISNULL(CMS_Single.No_Log_Response6to8_Enquire, 0),
		No_Log_Response8_Enquire = ISNULL(CMS_Single.No_Log_Response8_Enquire, 0),
		No_Log_Response6_Enquire = ISNULL(CMS_Single.No_Log_Response6_Enquire, 0),
		No_Logs_Enquire = ISNULL(CMS_Single.No_Logs_Enquire, 0),

		No_Log_Response0to2_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Log_Response0to2_Enquire, 0),
		No_Log_Response2to4_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Log_Response2to4_Enquire, 0),
		No_Log_Response4to6_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Log_Response4to6_Enquire, 0),
		No_Log_Response6to8_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Log_Response6to8_Enquire, 0),
		No_Log_Response8_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Log_Response8_Enquire, 0),
		No_Log_Response6_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Log_Response6_Enquire, 0),
		No_Logs_Enquire_Health_Check = ISNULL(CMS_HealthCheck.No_Logs_Enquire, 0),

		No_Log_ResponseP1_Enquire_Batch = ISNULL(CMS_Batch.No_Log_ResponseP1_Enquire, 0),
		No_Log_ResponseP2_Enquire_Batch = ISNULL(CMS_Batch.No_Log_ResponseP2_Enquire, 0),
		No_Log_ResponseP3_Enquire_Batch = ISNULL(CMS_Batch.No_Log_ResponseP3_Enquire, 0),
		No_Log_ResponseP4_Enquire_Batch = ISNULL(CMS_Batch.No_Log_ResponseP4_Enquire, 0),
		No_Log_ResponseP5_Enquire_Batch = ISNULL(CMS_Batch.No_Log_ResponseP5_Enquire, 0),
		No_Log_ResponseP6_Enquire_Batch = ISNULL(CMS_Batch.No_Log_ResponseP6_Enquire, 0),
		No_Logs_Enquire_Batch = ISNULL(CMS_Batch.No_Logs_Enquire, 0)
	FROM
	(
		SELECT
			SUM(CASE WHEN CMSResponseTime < 2.00 THEN 1 ELSE 0 END) [No_Log_Response0to2_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 2.00 and CMSResponseTime < 4.00 THEN 1 ELSE 0 END) [No_Log_Response2to4_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 4.00 and CMSResponseTime < 6.00 THEN 1 ELSE 0 END) [No_Log_Response4to6_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 6.00 and CMSResponseTime < 8.00 THEN 1 ELSE 0 END) [No_Log_Response6to8_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 8.00 THEN 1 ELSE 0 END) [No_Log_Response8_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 6.00 THEN 1 ELSE 0 END) [No_Log_Response6_Enquire],
			COUNT(1) [No_Logs_Enquire]
		FROM
			@EnquiryCMS_Result
		WHERE 
			start_time BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'N' 
	) CMS_Single,
	(
		SELECT
			SUM(CASE WHEN CMSResponseTime < 2.00 THEN 1 ELSE 0 END) [No_Log_Response0to2_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 2.00 and CMSResponseTime < 4.00 THEN 1 ELSE 0 END) [No_Log_Response2to4_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 4.00 and CMSResponseTime < 6.00 THEN 1 ELSE 0 END) [No_Log_Response4to6_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 6.00 and CMSResponseTime < 8.00 THEN 1 ELSE 0 END) [No_Log_Response6to8_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 8.00 THEN 1 ELSE 0 END) [No_Log_Response8_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 6.00 THEN 1 ELSE 0 END) [No_Log_Response6_Enquire],
			COUNT(1) [No_Logs_Enquire]
		FROM
			@EHStoCMS_HealthCheck
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
	) CMS_HealthCheck,
	(
		SELECT
			SUM(CASE WHEN CMSResponseTime < 10.00 THEN 1 ELSE 0 END) [No_Log_ResponseP1_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 10.00 and CMSResponseTime < 12.00 THEN 1 ELSE 0 END) [No_Log_ResponseP2_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 12.00 and CMSResponseTime < 15.00 THEN 1 ELSE 0 END) [No_Log_ResponseP3_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 15.00 and CMSResponseTime < 20.00 THEN 1 ELSE 0 END) [No_Log_ResponseP4_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 20.00 THEN 1 ELSE 0 END) [No_Log_ResponseP5_Enquire],
			SUM(CASE WHEN CMSResponseTime >= 15.00 THEN 1 ELSE 0 END) [No_Log_ResponseP6_Enquire],
			COUNT(1) [No_Logs_Enquire]
		FROM
			@EnquiryCMS_Result
		WHERE 
			start_time BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'Y' 
	) CMS_Batch
	WHERE 
		Dtm = @Current_Dtm 

--

	-- CMS to EHS Response Time
	UPDATE  
		@CMSSummary
	SET
		No_Log_Response0to2 = ISNULL(EHS_Single.No_Log_Response0to2, 0),
		No_Log_Response2to4 = ISNULL(EHS_Single.No_Log_Response2to4, 0),
		No_Log_Response6to8 = ISNULL(EHS_Single.No_Log_Response6to8, 0),
		No_Log_Response8 = ISNULL(EHS_Single.No_Log_Response8, 0),
		No_Log_Response6 = ISNULL(EHS_Single.No_Log_Response6, 0),
		No_Logs = ISNULL(EHS_Single.No_Logs, 0),

		No_Log_Response0to2_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response0to2, 0),
		No_Log_Response2to4_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response2to4, 0),
		No_Log_Response6to8_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response6to8, 0),
		No_Log_Response8_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response8, 0),
		No_Log_Response6_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response6, 0),
		No_Logs_Health_Check = ISNULL(EHS_HealthCheck.No_Logs, 0),

		No_Log_ResponseP1_Batch = ISNULL(EHS_Batch.No_Log_ResponseP1, 0),
		No_Log_ResponseP2_Batch = ISNULL(EHS_Batch.No_Log_ResponseP2, 0),
		No_Log_ResponseP3_Batch = ISNULL(EHS_Batch.No_Log_ResponseP3, 0),
		No_Log_ResponseP4_Batch = ISNULL(EHS_Batch.No_Log_ResponseP4, 0),
		No_Log_ResponseP5_Batch = ISNULL(EHS_Batch.No_Log_ResponseP5, 0),
		No_Log_ResponseP6_Batch = ISNULL(EHS_Batch.No_Log_ResponseP6, 0),
		No_Logs_Batch = ISNULL(EHS_Batch.No_Logs, 0)
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
			@CMSToEHS
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'N'
			AND Health_Check = 'N'
	) EHS_Single,
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
			@CMSToEHS
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'N'
			AND Health_Check = 'Y'
	) EHS_HealthCheck,
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
			@CMSToEHS
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'Y'
			AND Health_Check = 'N'
	) EHS_Batch
	WHERE 
		Dtm = @Current_Dtm 
	

--


	INSERT INTO @EHSToCMS_ReturnCode_Result(
		LOG_ID,
		COUNT_LOG_ID)
	SELECT
		Log_ID, COUNT(1)
	FROM
		@Evacc_ErrorResult
	WHERE
		Log_ID IN (SELECT Log_ID FROM @EHSToCMSAllErrorLogIDMaster)
		AND System_Dtm BETWEEN @Filter_start_time AND @Filter_end_time
	GROUP BY 
		Log_ID


	UPDATE @CMSSummary
	SET
		No_Communication_Link_Err_101 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01011'),
		No_Unknown_Err_99 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01010'),
		No_Invalid_Parameter_98 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01009'),
		No_ID_Mismatch_104 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01026'),
		No_Health_Check_Result_Incorrect_100 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01028'),
		No_EHS_Internal_Err_102 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01012'),
		No_EAI_Service_Interruption_105 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCMS_ReturnCode_Result WHERE LOG_ID = '01027')
	WHERE
		Dtm = @Current_Dtm 

	--

	INSERT INTO eHSD0018_CMSVaccinationConnectionErr_Stat (
		System_Dtm,
		Report_Dtm,
		Error_Type,
		Connect_System
	)
	SELECT  
		GETDATE(),
		System_Dtm,			
		CASE 
			WHEN Log_ID = '01011' THEN 'Communication link error'
			WHEN Log_ID = '01010' THEN 'Unknown error'
			WHEN Log_ID = '01009' THEN 'Invalid parameter'
			WHEN Log_ID = '01026' THEN 'CMS result Message ID mismatch with EHS request Message ID'
			WHEN Log_ID = '01028' THEN 'Returned health check result incorrect'
			WHEN Log_ID = '01012' THEN 'EHS internal error'
			WHEN Log_ID = '01027' THEN 'EAI Service Interruption'
			ELSE 'OTHER ERROR'
		END AS [Error_Type],
		'CMS'
	FROM
		@Evacc_ErrorResult
	WHERE
		Log_ID IN (SELECT Log_ID FROM @EHSToCMSAllErrorLogIDMaster)
		AND System_Dtm BETWEEN @Filter_start_time AND @Filter_end_time


	  --Calculate Percentage 
	UPDATE  
	   @CMSSummary  
	SET  
		Percentage_Fail = 
			convert(varchar ,
				(SELECT CASE WHEN No_Fail_Vaccine_Trx = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Fail_Vaccine_Trx / No_Vaccine_Trx)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%',
		Percentage_Response6 = 
			convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6 = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6 / No_Logs)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%', 
		Percentage_Response6_Health_Check = 
			convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6_Health_Check = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6_Health_Check / No_Logs_Health_Check)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%', 
		Percentage_Response6_Enquire = 
			convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6_Enquire = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6_Enquire / No_Logs_Enquire)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%',
		Percentage_Response6_Enquire_Health_Check = 
			ISNULL(convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6_Enquire_Health_Check = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6_Enquire_Health_Check / No_Logs_Enquire_Health_Check)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%', '0.00%'),
		Percentage_ResponseP6_Batch =
			convert(varchar ,
				(SELECT CASE WHEN No_Log_ResponseP6_Batch = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_ResponseP6_Batch / No_Logs_Batch)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%',
		Percentage_ResponseP6_Enquire_Batch =
			convert(varchar ,
				(SELECT CASE WHEN No_Log_ResponseP6_Enquire_Batch = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_ResponseP6_Enquire_Batch / No_Logs_Enquire_Batch)) END 
				FROM @CMSSummary WHERE Dtm = @Current_Dtm)) + '%'
	WHERE Dtm = @Current_Dtm 
	

	--Total
	UPDATE  
		@CMSSummary  
	SET  
		Total = No_Communication_Link_Err_101 + 
				No_Unknown_Err_99 + 
				No_Invalid_Parameter_98 +
				No_ID_Mismatch_104 +
				No_Health_Check_Result_Incorrect_100 +
				No_EHS_Internal_Err_102 + 
				No_EAI_Service_Interruption_105
	WHERE Dtm = @Current_Dtm 


------------------------
-- Prepare CIMS Summary
------------------------
	INSERT INTO @CIMSSummary (Dtm) VALUES (@Current_Dtm)
	
	-- tx
	UPDATE  
		@CIMSSummary  
	SET  
		No_Fail_Vaccine_Trx = (  
		SELECT count(1) from #VoucherTx
			where transaction_dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND (
					ISNULL(DH_Vaccine_Ref_Status, '') LIKE @DH_Vaccine_Ref_Status_CN
					OR ISNULL(DH_Vaccine_Ref_Status, '') LIKE @DH_Vaccine_Ref_Status_UN
				)
			),  
		No_Vaccine_Trx = (  
		SELECT count(1) from #VoucherTx			
			where transaction_dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND substring(DH_Vaccine_Ref_Status, 2, 1) <> 'D'
			),
		No_Vaccine_Trx_no_CIMS_enquire = (  
		SELECT count(1) from #VoucherTx
			where transaction_dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND substring(DH_Vaccine_Ref_Status, 2, 1) = 'D'
			)
	WHERE Dtm = @Current_Dtm 


	-- CIMS Response Time
	UPDATE  
		@CIMSSummary
	SET
		No_Log_Response0to2_Enquire = ISNULL(CIMS_Single.No_Log_Response0to2_Enquire, 0),
		No_Log_Response2to4_Enquire = ISNULL(CIMS_Single.No_Log_Response2to4_Enquire, 0),
		No_Log_Response4to6_Enquire = ISNULL(CIMS_Single.No_Log_Response4to6_Enquire, 0),
		No_Log_Response6to8_Enquire = ISNULL(CIMS_Single.No_Log_Response6to8_Enquire, 0),
		No_Log_Response8_Enquire = ISNULL(CIMS_Single.No_Log_Response8_Enquire, 0),
		No_Log_Response6_Enquire = ISNULL(CIMS_Single.No_Log_Response6_Enquire, 0),
		No_Logs_Enquire = ISNULL(CIMS_Single.No_Logs_Enquire, 0),

		No_Log_Response0to2_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Log_Response0to2_Enquire, 0),
		No_Log_Response2to4_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Log_Response2to4_Enquire, 0),
		No_Log_Response4to6_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Log_Response4to6_Enquire, 0),
		No_Log_Response6to8_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Log_Response6to8_Enquire, 0),
		No_Log_Response8_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Log_Response8_Enquire, 0),
		No_Log_Response6_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Log_Response6_Enquire, 0),
		No_Logs_Enquire_Health_Check = ISNULL(CIMS_HealthCheck.No_Logs_Enquire, 0),

		No_Log_ResponseP1_Enquire_Batch = ISNULL(CIMS_Batch.No_Log_ResponseP1_Enquire, 0),
		No_Log_ResponseP2_Enquire_Batch = ISNULL(CIMS_Batch.No_Log_ResponseP2_Enquire, 0),
		No_Log_ResponseP3_Enquire_Batch = ISNULL(CIMS_Batch.No_Log_ResponseP3_Enquire, 0),
		No_Log_ResponseP4_Enquire_Batch = ISNULL(CIMS_Batch.No_Log_ResponseP4_Enquire, 0),
		No_Log_ResponseP5_Enquire_Batch = ISNULL(CIMS_Batch.No_Log_ResponseP5_Enquire, 0),
		No_Log_ResponseP6_Enquire_Batch = ISNULL(CIMS_Batch.No_Log_ResponseP6_Enquire, 0),
		No_Logs_Enquire_Batch = ISNULL(CIMS_Batch.No_Logs_Enquire, 0)
	FROM
	(
		SELECT
			SUM(CASE WHEN CIMSResponseTime < 2.00 THEN 1 ELSE 0 END) [No_Log_Response0to2_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 2.00 and CIMSResponseTime < 4.00 THEN 1 ELSE 0 END) [No_Log_Response2to4_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 4.00 and CIMSResponseTime < 6.00 THEN 1 ELSE 0 END) [No_Log_Response4to6_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 6.00 and CIMSResponseTime < 8.00 THEN 1 ELSE 0 END) [No_Log_Response6to8_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 8.00 THEN 1 ELSE 0 END) [No_Log_Response8_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 6.00 THEN 1 ELSE 0 END) [No_Log_Response6_Enquire],
			COUNT(1) [No_Logs_Enquire]
		FROM
			@EnquiryCIMS_Result
		WHERE 
			start_time BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'N' 
	) CIMS_Single,
	(
		SELECT
			SUM(CASE WHEN CIMSResponseTime < 2.00 THEN 1 ELSE 0 END) [No_Log_Response0to2_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 2.00 and CIMSResponseTime < 4.00 THEN 1 ELSE 0 END) [No_Log_Response2to4_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 4.00 and CIMSResponseTime < 6.00 THEN 1 ELSE 0 END) [No_Log_Response4to6_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 6.00 and CIMSResponseTime < 8.00 THEN 1 ELSE 0 END) [No_Log_Response6to8_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 8.00 THEN 1 ELSE 0 END) [No_Log_Response8_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 6.00 THEN 1 ELSE 0 END) [No_Log_Response6_Enquire],
			COUNT(1) [No_Logs_Enquire]
		FROM
			@EHStoCIMS_HealthCheck
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
	) CIMS_HealthCheck,
	(
		SELECT
			SUM(CASE WHEN CIMSResponseTime < 10.00 THEN 1 ELSE 0 END) [No_Log_ResponseP1_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 10.00 and CIMSResponseTime < 12.00 THEN 1 ELSE 0 END) [No_Log_ResponseP2_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 12.00 and CIMSResponseTime < 15.00 THEN 1 ELSE 0 END) [No_Log_ResponseP3_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 15.00 and CIMSResponseTime < 20.00 THEN 1 ELSE 0 END) [No_Log_ResponseP4_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 20.00 THEN 1 ELSE 0 END) [No_Log_ResponseP5_Enquire],
			SUM(CASE WHEN CIMSResponseTime >= 15.00 THEN 1 ELSE 0 END) [No_Log_ResponseP6_Enquire],
			COUNT(1) [No_Logs_Enquire]
		FROM
			@EnquiryCIMS_Result
		WHERE 
			start_time BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'Y' 
	) CIMS_Batch
	WHERE 
		Dtm = @Current_Dtm 

--

	-- CIMS to EHS Response Time
	UPDATE  
		@CIMSSummary
	SET
		No_Log_Response0to2 = ISNULL(EHS_Single.No_Log_Response0to2, 0),
		No_Log_Response2to4 = ISNULL(EHS_Single.No_Log_Response2to4, 0),
		No_Log_Response6to8 = ISNULL(EHS_Single.No_Log_Response6to8, 0),
		No_Log_Response8 = ISNULL(EHS_Single.No_Log_Response8, 0),
		No_Log_Response6 = ISNULL(EHS_Single.No_Log_Response6, 0),
		No_Logs = ISNULL(EHS_Single.No_Logs, 0),

		No_Log_Response0to2_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response0to2, 0),
		No_Log_Response2to4_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response2to4, 0),
		No_Log_Response6to8_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response6to8, 0),
		No_Log_Response8_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response8, 0),
		No_Log_Response6_Health_Check = ISNULL(EHS_HealthCheck.No_Log_Response6, 0),
		No_Logs_Health_Check = ISNULL(EHS_HealthCheck.No_Logs, 0),

		No_Log_ResponseP1_Batch = ISNULL(EHS_Batch.No_Log_ResponseP1, 0),
		No_Log_ResponseP2_Batch = ISNULL(EHS_Batch.No_Log_ResponseP2, 0),
		No_Log_ResponseP3_Batch = ISNULL(EHS_Batch.No_Log_ResponseP3, 0),
		No_Log_ResponseP4_Batch = ISNULL(EHS_Batch.No_Log_ResponseP4, 0),
		No_Log_ResponseP5_Batch = ISNULL(EHS_Batch.No_Log_ResponseP5, 0),
		No_Log_ResponseP6_Batch = ISNULL(EHS_Batch.No_Log_ResponseP6, 0),
		No_Logs_Batch = ISNULL(EHS_Batch.No_Logs, 0)
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
			@CIMSToEHS
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'N'
			AND Health_Check = 'N'
	) EHS_Single,
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
			@CIMSToEHS
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'N'
			AND Health_Check = 'Y'
	) EHS_HealthCheck,
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
			@CIMSToEHS
		WHERE 
			Dtm BETWEEN @Filter_start_time AND @Filter_end_time
			AND Batch_Enquiry = 'Y'
			AND Health_Check = 'N'
	) EHS_Batch
	WHERE 
		Dtm = @Current_Dtm 
	

--

	INSERT INTO @EHSToCIMS_ReturnCode_Result(
		LOG_ID,
		COUNT_LOG_ID)
	SELECT
		Log_ID, COUNT(1)
	FROM
		@Evacc_ErrorResult
	WHERE
		Log_ID IN (SELECT Log_ID FROM @EHSToCIMSAllErrorLogIDMaster)
		AND System_Dtm BETWEEN @Filter_start_time AND @Filter_end_time
	GROUP BY 
		Log_ID


	UPDATE @CIMSSummary
	SET
		No_Communication_Link_Err_90001 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCIMS_ReturnCode_Result WHERE LOG_ID = '01111'),
		No_Unknown_Err_99999 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCIMS_ReturnCode_Result WHERE LOG_ID = '01110'),
		No_Invalid_Parameter_90005 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCIMS_ReturnCode_Result WHERE LOG_ID = '01109'),
		No_Client_Mismatch_90004 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCIMS_ReturnCode_Result WHERE LOG_ID = '01126'),
		No_Health_Check_Result_Incorrect_10001 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCIMS_ReturnCode_Result WHERE LOG_ID = '01128'),
		No_EHS_internal_Err_90002 = 
			(SELECT ISNULL(SUM(COUNT_LOG_ID),0) FROM @EHSToCIMS_ReturnCode_Result WHERE LOG_ID = '01112')
	WHERE
		Dtm = @Current_Dtm 

--

	--Calculate Percentage 
	UPDATE  
	   @CIMSSummary  
	SET  
		Percentage_Fail = 
			convert(varchar ,
				(SELECT CASE WHEN No_Fail_Vaccine_Trx = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Fail_Vaccine_Trx / No_Vaccine_Trx)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%',
		Percentage_Response6 = 
			convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6 = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6 / No_Logs)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%', 
		Percentage_Response6_Health_Check = 
			convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6_Health_Check = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6_Health_Check / No_Logs_Health_Check)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%', 
		Percentage_Response6_Enquire = 
			convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6_Enquire = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6_Enquire / No_Logs_Enquire)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%',
		Percentage_Response6_Enquire_Health_Check = 
			ISNULL(convert(varchar ,
				(SELECT CASE WHEN No_Log_Response6_Enquire_Health_Check = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_Response6_Enquire_Health_Check / No_Logs_Enquire_Health_Check)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%', '0.00%'),
		Percentage_ResponseP6_Batch =
			convert(varchar ,
				(SELECT CASE WHEN No_Log_ResponseP6_Batch = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_ResponseP6_Batch / No_Logs_Batch)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%',
		Percentage_ResponseP6_Enquire_Batch =
			convert(varchar ,
				(SELECT CASE WHEN No_Log_ResponseP6_Enquire_Batch = 0 THEN 0
					   ELSE (CONVERT(DECIMAL(5,2),100.0 * No_Log_ResponseP6_Enquire_Batch / No_Logs_Enquire_Batch)) END 
				FROM @CIMSSummary WHERE Dtm = @Current_Dtm)) + '%'
	WHERE Dtm = @Current_Dtm 


	--Total
	UPDATE  
		@CIMSSummary  
	SET  
		Total = No_Communication_Link_Err_90001 + 
				No_Unknown_Err_99999 + 
				No_Invalid_Parameter_90005 +
				No_Client_Mismatch_90004 +
				No_Health_Check_Result_Incorrect_10001 +
				No_EHS_internal_Err_90002
	WHERE Dtm = @Current_Dtm 



	INSERT INTO eHSD0018_CMSVaccinationConnectionErr_Stat (
		System_Dtm,
		Report_Dtm,
		Error_Type,
		Connect_System
	)
	SELECT  
		GETDATE(),
		System_Dtm,		
		CASE 
			WHEN Log_ID = '01109' THEN 'Invalid parameter'
			WHEN Log_ID = '01110' THEN 'Unknown error'
			WHEN Log_ID = '01111' THEN 'Communication link error'
			WHEN Log_ID = '01112' THEN 'EHS internal error'
			WHEN Log_ID = '01126' THEN 'CIMS result client mismatch with EHS request client'
			WHEN Log_ID = '01128' THEN 'Returned health check result incorrect'
			ELSE 'OTHER ERROR'
		END AS [Error_Type],
		'CIMS'
	FROM
		@Evacc_ErrorResult
	WHERE
		Log_ID IN (SELECT Log_ID FROM @EHSToCIMSAllErrorLogIDMaster)
		AND System_Dtm BETWEEN @Filter_start_time AND @Filter_end_time

	SELECT @Current_Dtm = DATEADD(dd, 1, @Current_Dtm)  
	    
END -- While

-- =============================================
-- Store into tables
-- =============================================


-- Insert record
	------------------ Summary ------------------
	INSERT INTO eHSD0018_CMSVaccinationConnectionSummary_Stat (
		System_Dtm,
		Report_Dtm,
		No_Fail_Vaccine_Trx,  
		No_Vaccine_Trx,  
		No_Vaccine_Trx_no_CMS_enquire,
		Percentage_Fail,   
		No_Communication_Link_Err_101,
		No_Unknown_Err_99,
		No_Invalid_Parameter_98,
		No_ID_Mismatch_104,
		No_Health_Check_Result_Incorrect_100,
		No_EHS_Internal_Err_102,
		No_EAI_Service_Interruption_105,
		Total,  
		No_Log_Response0to2_Enquire, 
		No_Log_Response2to4_Enquire, 
		No_Log_Response4to6_Enquire, 
		No_Log_Response6to8_Enquire, 
		No_Log_Response8_Enquire,
		No_Logs_Enquire,  
		Percentage_Response6_Enquire,		  
		No_Log_Response0to2, 
		No_Log_Response2to4, 
		No_Log_Response4to6, 
		No_Log_Response6to8, 
		No_Log_Response8,
		No_Logs,  
		Percentage_Response6,
		No_Log_Response0to2_Health_Check,
		No_Log_Response2to4_Health_Check,
		No_Log_Response4to6_Health_Check,
		No_Log_Response6to8_Health_Check,
		No_Log_Response8_Health_Check,
		No_Logs_Health_Check,
		Percentage_Response6_Health_Check,
		No_Log_Response0to2_Enquire_Health_Check,
		No_Log_Response2to4_Enquire_Health_Check,
		No_Log_Response4to6_Enquire_Health_Check,
		No_Log_Response6to8_Enquire_Health_Check,
		No_Log_Response8_Enquire_Health_Check,
		No_Logs_Enquire_Health_Check,
		Percentage_Response6_Enquire_Health_Check,
		No_Log_ResponseP1_Batch,
		No_Log_ResponseP2_Batch,
		No_Log_ResponseP3_Batch,
		No_Log_ResponseP4_Batch,
		No_Log_ResponseP5_Batch,
		No_Logs_Batch,
		Percentage_ResponseP6_Batch,
		No_Log_ResponseP1_Enquire_Batch,
		No_Log_ResponseP2_Enquire_Batch,
		No_Log_ResponseP3_Enquire_Batch,
		No_Log_ResponseP4_Enquire_Batch,
		No_Log_ResponseP5_Enquire_Batch,
		No_Logs_Enquire_Batch,
		Percentage_ResponseP6_Enquire_Batch
	) 
	SELECT
		GETDATE(),
		CONVERT(varchar(10), Dtm, 20),  -- To yyyy-mm-dd
		No_Fail_Vaccine_Trx,  
		No_Vaccine_Trx,
		No_Vaccine_Trx_no_CMS_enquire,  
		Percentage_Fail,   
		No_Communication_Link_Err_101,
		No_Unknown_Err_99,
		No_Invalid_Parameter_98,
		No_ID_Mismatch_104,
		No_Health_Check_Result_Incorrect_100,
		No_EHS_Internal_Err_102,
		No_EAI_Service_Interruption_105,
		Total,  
		No_Log_Response0to2_Enquire, 
		No_Log_Response2to4_Enquire, 
		No_Log_Response4to6_Enquire, 
		No_Log_Response6to8_Enquire, 
		No_Log_Response8_Enquire,
		No_Logs_Enquire,  
		Percentage_Response6_Enquire,			  
		No_Log_Response0to2, 
		No_Log_Response2to4, 
		No_Log_Response4to6, 
		No_Log_Response6to8, 
		No_Log_Response8,		  		  		  		   
		No_Logs,  
		Percentage_Response6,
		No_Log_Response0to2_Health_Check,
		No_Log_Response2to4_Health_Check,
		No_Log_Response4to6_Health_Check,
		No_Log_Response6to8_Health_Check,
		No_Log_Response8_Health_Check,
		No_Logs_Health_Check,
		Percentage_Response6_Health_Check,
		No_Log_Response0to2_Enquire_Health_Check,
		No_Log_Response2to4_Enquire_Health_Check,
		No_Log_Response4to6_Enquire_Health_Check,
		No_Log_Response6to8_Enquire_Health_Check,
		No_Log_Response8_Enquire_Health_Check,
		No_Logs_Enquire_Health_Check,
		Percentage_Response6_Enquire_Health_Check,
		No_Log_ResponseP1_Batch,
		No_Log_ResponseP2_Batch,
		No_Log_ResponseP3_Batch,
		No_Log_ResponseP4_Batch,
		No_Log_ResponseP5_Batch,
		No_Logs_Batch,
		Percentage_ResponseP6_Batch,
		No_Log_ResponseP1_Enquire_Batch,
		No_Log_ResponseP2_Enquire_Batch,
		No_Log_ResponseP3_Enquire_Batch,
		No_Log_ResponseP4_Enquire_Batch,
		No_Log_ResponseP5_Enquire_Batch,
		No_Logs_Enquire_Batch,
		Percentage_ResponseP6_Enquire_Batch
	FROM
		@CMSSummary
	

	INSERT INTO eHSD0018_CIMSVaccinationConnectionSummary_Stat (
		System_Dtm,
		Report_Dtm,
		No_Fail_Vaccine_Trx,  
		No_Vaccine_Trx,  
		No_Vaccine_Trx_no_CIMS_enquire,
		Percentage_Fail,   
		No_Communication_Link_Err_90001,
		No_Unknown_Err_99999,
		No_Invalid_Parameter_90005,
		No_Client_Mismatch_90004,
		No_Health_Check_Result_Incorrect_10001,
		No_EHS_internal_Err_90002,
		Total,  
		No_Log_Response0to2_Enquire, 
		No_Log_Response2to4_Enquire, 
		No_Log_Response4to6_Enquire, 
		No_Log_Response6to8_Enquire, 
		No_Log_Response8_Enquire,
		No_Logs_Enquire,  
		Percentage_Response6_Enquire,		  
		No_Log_Response0to2, 
		No_Log_Response2to4, 
		No_Log_Response4to6, 
		No_Log_Response6to8, 
		No_Log_Response8,
		No_Logs,  
		Percentage_Response6,
		No_Log_Response0to2_Health_Check,
		No_Log_Response2to4_Health_Check,
		No_Log_Response4to6_Health_Check,
		No_Log_Response6to8_Health_Check,
		No_Log_Response8_Health_Check,
		No_Logs_Health_Check,
		Percentage_Response6_Health_Check,
		No_Log_Response0to2_Enquire_Health_Check,
		No_Log_Response2to4_Enquire_Health_Check,
		No_Log_Response4to6_Enquire_Health_Check,
		No_Log_Response6to8_Enquire_Health_Check,
		No_Log_Response8_Enquire_Health_Check,
		No_Logs_Enquire_Health_Check,
		Percentage_Response6_Enquire_Health_Check,
		No_Log_ResponseP1_Batch,
		No_Log_ResponseP2_Batch,
		No_Log_ResponseP3_Batch,
		No_Log_ResponseP4_Batch,
		No_Log_ResponseP5_Batch,
		No_Logs_Batch,
		Percentage_ResponseP6_Batch,
		No_Log_ResponseP1_Enquire_Batch,
		No_Log_ResponseP2_Enquire_Batch,
		No_Log_ResponseP3_Enquire_Batch,
		No_Log_ResponseP4_Enquire_Batch,
		No_Log_ResponseP5_Enquire_Batch,
		No_Logs_Enquire_Batch,
		Percentage_ResponseP6_Enquire_Batch
	) 
	SELECT
		GETDATE(),
		CONVERT(varchar(10), Dtm, 20),  -- To yyyy-mm-dd
		No_Fail_Vaccine_Trx,  
		No_Vaccine_Trx,
		No_Vaccine_Trx_no_CIMS_enquire,  
		Percentage_Fail,  
		No_Communication_Link_Err_90001,
		No_Unknown_Err_99999,
		No_Invalid_Parameter_90005,
		No_Client_Mismatch_90004,
		No_Health_Check_Result_Incorrect_10001,
		No_EHS_internal_Err_90002,
		Total,  
		No_Log_Response0to2_Enquire, 
		No_Log_Response2to4_Enquire, 
		No_Log_Response4to6_Enquire, 
		No_Log_Response6to8_Enquire, 
		No_Log_Response8_Enquire,
		No_Logs_Enquire,  
		Percentage_Response6_Enquire,			  
		No_Log_Response0to2, 
		No_Log_Response2to4, 
		No_Log_Response4to6, 
		No_Log_Response6to8, 
		No_Log_Response8,		  		  		  		   
		No_Logs,  
		Percentage_Response6,
		No_Log_Response0to2_Health_Check,
		No_Log_Response2to4_Health_Check,
		No_Log_Response4to6_Health_Check,
		No_Log_Response6to8_Health_Check,
		No_Log_Response8_Health_Check,
		No_Logs_Health_Check,
		Percentage_Response6_Health_Check,
		No_Log_Response0to2_Enquire_Health_Check,
		No_Log_Response2to4_Enquire_Health_Check,
		No_Log_Response4to6_Enquire_Health_Check,
		No_Log_Response6to8_Enquire_Health_Check,
		No_Log_Response8_Enquire_Health_Check,
		No_Logs_Enquire_Health_Check,
		Percentage_Response6_Enquire_Health_Check,
		No_Log_ResponseP1_Batch,
		No_Log_ResponseP2_Batch,
		No_Log_ResponseP3_Batch,
		No_Log_ResponseP4_Batch,
		No_Log_ResponseP5_Batch,
		No_Logs_Batch,
		Percentage_ResponseP6_Batch,
		No_Log_ResponseP1_Enquire_Batch,
		No_Log_ResponseP2_Enquire_Batch,
		No_Log_ResponseP3_Enquire_Batch,
		No_Log_ResponseP4_Enquire_Batch,
		No_Log_ResponseP5_Enquire_Batch,
		No_Logs_Enquire_Batch,
		Percentage_ResponseP6_Enquire_Batch
	FROM
		@CIMSSummary


	------------------ Connection Status (Transaction) --------------------------
	INSERT INTO eHSD0018_CMSVaccinationConnectionStatus_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Connection_Status,
		  Connect_System
	) 
	SELECT 
			GETDATE(),
			transaction_dtm,			
			substring(HA_Vaccine_Ref_Status, 2, 1),
			'CMS'
		FROM #VoucherTx		
		WHERE HA_Vaccine_Ref_Status IS NOT NULL
	UNION ALL
	SELECT 
			GETDATE(),
			transaction_dtm,
			substring(DH_Vaccine_Ref_Status, 2, 1),
			'CIMS'
		FROM #VoucherTx		
		WHERE DH_Vaccine_Ref_Status IS NOT NULL
	------------------ Error Type (Audit Log) --------------------------

	
	------------------ Enquiry CMS duration -----------------
	-- Non health check
	INSERT INTO eHSD0018_EnquireCMSWebServiceResponseTime_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Response_Time,
		  Health_Check,
		  Batch_Enquiry,
		  Num_Of_Patient
	) 
	SELECT
		  getdate(),
		  start_time,   
		  CMSResponseTime,
		  'N' AS [Health_Check],
		  Batch_Enquiry,
		  NumOfPatient
	FROM @EnquiryCMS_Result

	-- health check
	INSERT INTO eHSD0018_EnquireCMSWebServiceResponseTime_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Response_Time,
		  Health_Check,
		  Batch_Enquiry,
		  Num_Of_Patient
	) 
	SELECT
		  GETDATE(),
	      Dtm,
		  CMSResponseTime,
		  Health_Check,
		  'N',
		  NULL		  
	FROM @EHStoCMS_HealthCheck
	WHERE Health_Check = 'Y'
	

	------------------ Enquiry CIMS duration -----------------
	-- Non health check
	INSERT INTO eHSD0018_EnquireCIMSWebServiceResponseTime_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Response_Time,
		  Health_Check,
		  Batch_Enquiry,
		  Num_Of_Patient
	) 
	SELECT
		  getdate(),
		  start_time,   
		  CIMSResponseTime,
		  'N' AS [Health_Check],
		  Batch_Enquiry,
		  NumOfPatient
	FROM @EnquiryCIMS_Result

	-- health check
	INSERT INTO eHSD0018_EnquireCIMSWebServiceResponseTime_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Response_Time,
		  Health_Check,
		  Batch_Enquiry,
		  Num_Of_Patient
	) 
	SELECT
		  GETDATE(),
	      Dtm,
		  CIMSResponseTime,
		  Health_Check,
		  'N',
		  NULL		  
	FROM @EHStoCIMS_HealthCheck
	WHERE Health_Check = 'Y'

	------------------ Response time of web service for CMS-----------------
	INSERT INTO eHSD0018_CMSWebServiceResponseTime_Stat (
		  System_Dtm,
		  Report_Dtm,
		  Response_Time,
		  Health_Check,
		  Batch_Enquiry
	) 
	SELECT   
		  GETDATE(),
	      Dtm,
		  EHSResponseTime,
		  Health_Check,
		  Batch_Enquiry
	  FROM @CMSToEHS	
	UNION ALL
	SELECT   
		  GETDATE(),
	      Dtm,
		  EHSResponseTime,
		  Health_Check,
		  Batch_Enquiry
	  FROM @CIMSToEHS	

-- =============================================
-- Debugging use
-- =============================================
/*
	SELECT 
		CONVERT(varchar, Report_dtm, 111),
		No_Fail_Vaccine_Trx,  
		No_Vaccine_Trx, 
		No_Vaccine_Trx_no_CMS_enquire, 
		Percentage_Fail,  
		No_Communication_Link_Err_101,
		No_Unknown_Err_99,
		No_Invalid_Parameter_98,
		No_ID_Mismatch_104,
		No_Health_Check_Result_Incorrect_100,
		No_EHS_Internal_Err_102,
		No_EAI_Service_Interruption_105,
		Total,  
		No_Log_Response0to2_Enquire,
		No_Log_Response2to4_Enquire,
		No_Log_Response4to6_Enquire,
		No_Log_Response6to8_Enquire,
		No_Log_Response8_Enquire,  
		No_Logs_Enquire,  
		Percentage_Response6_Enquire,		  
		No_Log_Response0to2,
		No_Log_Response2to4,
		No_Log_Response4to6,
		No_Log_Response6to8,
		No_Log_Response8,  
		No_Logs,  
		Percentage_Response6,
		No_Log_Response0to2_Health_Check,
		No_Log_Response2to4_Health_Check,
		No_Log_Response4to6_Health_Check,
		No_Log_Response6to8_Health_Check,
		No_Log_Response8_Health_Check,
		No_Logs_Health_Check,
		Percentage_Response6_Health_Check,
		No_Log_Response0to2_Enquire_Health_Check,
		No_Log_Response2to4_Enquire_Health_Check,
		No_Log_Response4to6_Enquire_Health_Check,
		No_Log_Response6to8_Enquire_Health_Check,
		No_Log_Response8_Enquire_Health_Check,
		No_Logs_Enquire_Health_Check,
		Percentage_Response6_Enquire_Health_Check,
		No_Log_ResponseP1_Batch,
		No_Log_ResponseP2_Batch,
		No_Log_ResponseP3_Batch,
		No_Log_ResponseP4_Batch,
		No_Log_ResponseP5_Batch,
		No_Logs_Batch,
		Percentage_ResponseP6_Batch,
		No_Log_ResponseP1_Enquire_Batch,
		No_Log_ResponseP2_Enquire_Batch,
		No_Log_ResponseP3_Enquire_Batch,
		No_Log_ResponseP4_Enquire_Batch,
		No_Log_ResponseP5_Enquire_Batch,
		No_Logs_Enquire_Batch,
		Percentage_ResponseP6_Enquire_Batch
	FROM
		@eHSD0018_CMSVaccinationConnectionSummary_Stat
	ORDER BY
		Report_Dtm


	SELECT 
		CONVERT(varchar, Report_dtm, 111),
		No_Fail_Vaccine_Trx,  
		No_Vaccine_Trx, 
		No_Vaccine_Trx_no_CIMS_enquire, 
		Percentage_Fail,  
		No_Communication_Link_Err_90001,
		No_Unknown_Err_99999,
		No_Invalid_Parameter_90005,
		No_Client_Mismatch_90004,
		No_Health_Check_Result_Incorrect_10001,
		No_EHS_internal_Err_90002,
		Total,  
		No_Log_Response0to2_Enquire,
		No_Log_Response2to4_Enquire,
		No_Log_Response4to6_Enquire,
		No_Log_Response6to8_Enquire,
		No_Log_Response8_Enquire,  
		No_Logs_Enquire,  
		Percentage_Response6_Enquire,		  
		No_Log_Response0to2,
		No_Log_Response2to4,
		No_Log_Response4to6,
		No_Log_Response6to8,
		No_Log_Response8,  
		No_Logs,  
		Percentage_Response6,
		No_Log_Response0to2_Health_Check,
		No_Log_Response2to4_Health_Check,
		No_Log_Response4to6_Health_Check,
		No_Log_Response6to8_Health_Check,
		No_Log_Response8_Health_Check,
		No_Logs_Health_Check,
		Percentage_Response6_Health_Check,
		No_Log_Response0to2_Enquire_Health_Check,
		No_Log_Response2to4_Enquire_Health_Check,
		No_Log_Response4to6_Enquire_Health_Check,
		No_Log_Response6to8_Enquire_Health_Check,
		No_Log_Response8_Enquire_Health_Check,
		No_Logs_Enquire_Health_Check,
		Percentage_Response6_Enquire_Health_Check,
		No_Log_ResponseP1_Batch,
		No_Log_ResponseP2_Batch,
		No_Log_ResponseP3_Batch,
		No_Log_ResponseP4_Batch,
		No_Log_ResponseP5_Batch,
		No_Logs_Batch,
		Percentage_ResponseP6_Batch,
		No_Log_ResponseP1_Enquire_Batch,
		No_Log_ResponseP2_Enquire_Batch,
		No_Log_ResponseP3_Enquire_Batch,
		No_Log_ResponseP4_Enquire_Batch,
		No_Log_ResponseP5_Enquire_Batch,
		No_Logs_Enquire_Batch,
		Percentage_ResponseP6_Enquire_Batch
	FROM
		@eHSD0018_CIMSVaccinationConnectionSummary_Stat
	ORDER BY
		Report_Dtm

	SELECT
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Connection_Status,
		Connect_System
	FROM
		@eHSD0018_CMSVaccinationConnectionStatus_Stat
	ORDER BY
		Report_Dtm
	
	SELECT
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Error_Type,
		Connect_System
	FROM
		@eHSD0018_CMSVaccinationConnectionErr_Stat
	ORDER BY
		Report_Dtm
		
	SELECT
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Response_Time,
		Health_Check,
		Batch_Enquiry
	FROM
		@eHSD0018_EnquireCMSWebServiceResponseTime_Stat
	ORDER BY
		Report_Dtm
		
	SELECT
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Response_Time,
		Health_Check,
		Batch_Enquiry
	FROM
		@eHSD0018_CMSWebServiceResponseTime_Stat
	ORDER BY
		Report_Dtm

	SELECT
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Response_Time,
		Health_Check,
		Batch_Enquiry
	FROM
		@eHSD0018_EnquireCIMSWebServiceResponseTime_Stat
	ORDER BY
		Report_Dtm
*/

--House Keeping
DROP TABLE #SchemeCodeVaccine
DROP TABLE #VoucherTx
END
GO



GRANT EXECUTE ON [dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Write] TO HCVU
GO

