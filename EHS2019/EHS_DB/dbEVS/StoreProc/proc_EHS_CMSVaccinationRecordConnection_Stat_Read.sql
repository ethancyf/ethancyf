IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Raiman Chong
-- Modified date:	11 Jun 2021
-- CR No.:			INT21-0006 
-- Description:		Change eHSD0018-05 reporting period to 4 days
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	20 Apr 2020
-- CR No.:			INT20-0008
-- Description:		Fix incorrect sorting order
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	02 Nov 2018
-- CR No.:			CRE18-012 (Revise eHSD0018)
-- Description:	  	Rename the header and include num. of patient of each enquiry
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Winnie SUEN
-- Modified date:	30 Jul 2018
-- CR No.:			INT18-0009 (Fix eHSD0018 report)
-- Description:	  	Fix missing eHS -> DH CIMS health check record on 09 tab
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Winnie SUEN
-- Modified date:	22 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	(1) Revise report to include CIMS connection checking
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
-- CR No.:			CRP11-029 
-- Modified by: 	Koala CHENG
-- Modified date:   19 Apr 2012
-- Description:		1. Add EHS -> CMS health check performance
--					2. Rearrange report layout
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRP11-014 
-- Modified by: 	Koala CHENG
-- Modified date:   23 Nov 2011
-- Description:		Modify Sub Report (eHSD0018-04) to 4 days raw data
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRP11-013 
-- Modified by: 	Koala CHENG
-- Modified date:   04 Nov 2011
-- Description:		Modify Sub Report (eHSD0018-04) to 1 week raw data
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by: 	Lawrence TSANG
-- Modified date:   28 March 2011
-- Description:		(1) Separate CMS->EHS health check enquiries and non-health check enquiries in eHSD0018-01 : eVaccination Record Connection Summary
--					(2) Add indicator for health check enquiries and non-health check enquiries in eHSD0018-05 : Report on Interface Web Service for CMS
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by: 	Paul Yip
-- Modified date:   12 Jan 2011
-- Description:		Retrieve Statistics of enquiry CMS web service 
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by: 	Paul Yip
-- Modified date: 17 Dec 2010
-- Description: Fix date format for summary
-- =============================================  
-- =============================================
-- Author:		Paul Yip
-- Create date: 02 Dec 2010
-- Description:	Statistics for getting eVaccination Record Connection
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Read]
AS BEGIN

-- =============================================
-- Report setting 
-- =============================================

	DECLARE @No_Of_Days int  
	SELECT @No_Of_Days = 14  

	DECLARE @No_Of_Days_Raw_Data int 
	SELECT @No_Of_Days_Raw_Data = Parm_Value1 FROM SystemParameters WHERE Parameter_name = 'eHSD0018ReportRawDataDays'

-- =============================================  
-- Temporary tables  
-- ============================================= 
-- Result Table
	DECLARE @ResultTable table (  
		Display_Seq		smallint,  
		Result_Value1	varchar(100),  
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100)
	)

	DECLARE @ResultTable_Connection table (  
		Display_Seq		smallint,  
		Result_Value1	varchar(100),  
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100),
		Result_Value6	varchar(100),
		Result_Value7	varchar(100),
		Result_Value8	varchar(100),
		Result_Value9	varchar(100),
		Result_Value10	varchar(100),
		Result_Value11	varchar(100),
		Result_Value12	varchar(100),
		Result_Value13	varchar(100)
	)

	DECLARE @ResultTable_Duration table (  
		Display_Seq		smallint,  
		Result_Value1	varchar(100),  
		Result_Value2	nvarchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100),
		Result_Value6	varchar(100),
		Result_Value7	varchar(100),
		Result_Value8	varchar(100),
		Result_Value9	nvarchar(100),
		Result_Value10	varchar(100),
		Result_Value11	varchar(100),
		Result_Value12	varchar(100),
		Result_Value13	varchar(100),
		Result_Value14	varchar(100),
		Result_Value15	varchar(100)
	)



	DECLARE @DateFrame table (  
		ReportDate		DATETIME
	)

-- =============================================
-- Initialization
-- =============================================

	 DECLARE @Start_Dtm datetime  
	 DECLARE @EnquiryCMS_Start_Dtm datetime
	 DECLARE @VaccineTranConnStatus_Start_Dtm datetime
	 DECLARE @End_Dtm datetime  
	   
	 SELECT @End_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	 SELECT @Start_Dtm = DATEADD(dd, -(@No_Of_Days), @End_Dtm)  
	 SELECT @VaccineTranConnStatus_Start_Dtm = DATEADD(dd, -(@No_Of_Days_Raw_Data), @End_Dtm)  
	 SELECT @EnquiryCMS_Start_Dtm = DATEADD(dd, -(@No_Of_Days_Raw_Data), @End_Dtm)  
	
	
	DECLARE @i AS INT = 1
	WHILE @i <= @No_Of_Days
	BEGIN
		INSERT INTO @DateFrame (ReportDate) VALUES(
			DATEADD(dd, -(@i), @End_Dtm) 
		)
		SET @i += 1
	END
	
-- =============================================  
-- Return result  
-- =============================================  



	------------------ Report Generation Time ------------------
	SELECT 'Report Generation Time: ' + CONVERT(varchar,GETDATE(), 111) + ' ' + CONVERT(varchar, GETDATE(), 108)


	------------------ eHS(S)D0018-01 : eVaccination Record Connection Summary ------------------
	
	------------------------------------------------------------------------
	-- HA CMS 
	------------------------------------------------------------------------
	-- HA CMS Header
	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES (1, '1) HA CMS','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES (2, '', 'Vaccination Transaction','','','', 'Audit Log Error (Get HA CMS Vaccination)','','','','','','','')  

	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES (3, 'Date', 'Failed connection (A)','Need to enquiry (B)','Need not to enquiry', '% of Failed (A)/(B)*100%', 
			'Communication link error','Unknown error','Invalid parameter',	'Message ID mismatch with EHS',
			'Incorrect health check result ','EHS internal error', 'EAI Service Interruption','Total')  

	-- Result 
	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)									
	SELECT
		4,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_Fail_Vaccine_Trx AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Vaccine_Trx AS VARCHAR(100)),'N/A'), 
		ISNULL(CAST(No_Vaccine_Trx_no_CMS_enquire AS VARCHAR(100)),'N/A'),
		ISNULL(Percentage_Fail, 'N/A'),
		ISNULL(CAST(No_Communication_Link_Err_101 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Unknown_Err_99 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Invalid_Parameter_98 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_ID_Mismatch_104 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Health_Check_Result_Incorrect_100 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_EHS_Internal_Err_102 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_EAI_Service_Interruption_105 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(Total AS VARCHAR(100)),'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm
	
	-- Empty Row
	INSERT INTO @ResultTable_Connection (Display_Seq) VALUES (5)

	------------------------------------------------------------------------
	-- DH CIMS
	------------------------------------------------------------------------
	-- DH CIMS Header
	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES (6,'2) DH CIMS','','','','', '','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES (7,'','Vaccination Transaction','','','','Audit Log Error (Get DH CIMS Vaccination)','','','','','','','')  

	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES (8, 'Date', 'Failed connection (A)','Need to enquiry (B)','Need not to enquiry','% of Failed (A)/(B)*100%',
			'Communication link error','Unknown error','Invalid parameter','Message ID mismatch with EHS', 
			'Incorrect health check result','EHS internal error','EAI Service Interruption','Total')  

	-- Result
	INSERT INTO @ResultTable_Connection (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
								Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT
		9,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_Fail_Vaccine_Trx AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Vaccine_Trx AS VARCHAR(100)),'N/A'), 
		ISNULL(CAST(No_Vaccine_Trx_no_CIMS_enquire AS VARCHAR(100)),'N/A'),
		ISNULL(Percentage_Fail, 'N/A'),
		ISNULL(CAST(No_Communication_Link_Err_90001 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Unknown_Err_99999 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Invalid_Parameter_90005 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Client_Mismatch_90004 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Health_Check_Result_Incorrect_10001 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_EHS_internal_Err_90002 AS VARCHAR(100)),'N/A'),
		'N/A',
		ISNULL(CAST(Total AS VARCHAR(100)),'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CIMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm
	

	-- Return Result
	SELECT 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13 
	FROM 
		@ResultTable_Connection 
	ORDER BY 
		Display_Seq, Result_Value1


	------------------ eHS(S)D0018-02 : eVaccination Web Service Connection Duration Summary (Single Enquiry) ------------------

	------------------------------------------------------------------------
	-- HA CMS 
	------------------------------------------------------------------------
	-- HA CMS Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (1,N'1) HA CMS','','','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (2,'',N'EHS ⟶ HA CMS response time (Single Enquiry)','','','','','','',N'HA CMS ⟶ EHS response time (Single Enquiry)','','','','','','')  

	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (3, 'Date', 
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s',
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s')

	-- Result
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	SELECT
		4,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_log_Response0to2_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8_Enquire AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Enquire AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6_Enquire,'N/A'),		  
		ISNULL(CAST(No_log_Response0to2 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8 AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6,'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm

	-- Empty Row
	INSERT INTO @ResultTable_Duration (Display_Seq) VALUES (5)

	------------------------------------------------------------------------
	-- DH CIMS
	------------------------------------------------------------------------
	-- DH CIMS Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (6,N'2) DH CIMS','','','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (7,'',N'EHS ⟶ DH CIMS response time (Single Enquiry)','','','','','','',N'DH CIMS ⟶ EHS response time (Single Enquiry)','','','','','','')  

	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (8, 'Date', 
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s','>= 8s','Total','% of >= 6s',
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s','>= 8s','Total','% of >= 6s')

	-- Result
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	SELECT
		9,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_log_Response0to2_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8_Enquire AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8_Enquire AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Enquire AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6_Enquire,'N/A'),		  
		ISNULL(CAST(No_log_Response0to2 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8 AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6,'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CIMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm

	-- Return Result
	SELECT 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13,Result_Value14,Result_Value15
	FROM 
		@ResultTable_Duration
	ORDER BY 
		Display_Seq, Result_Value1

	DELETE FROM @ResultTable_Duration

	------------------ eHS(S)D0018-03 : eVaccination Web Service Connection Duration Summary (Batch Enquiry) ------------------

	------------------------------------------------------------------------
	-- HA CMS 
	------------------------------------------------------------------------
	-- HA CMS Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (1, '1) HA CMS', '','','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (2, '', N'EHS ⟶ HA CMS response time (Batch Enquiry)','','','','','','',N'HA CMS ⟶ EHS response time (Batch Enquiry)','','','','','','')  

	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (3, 'Date', 
			'< 10s','>= 10s and < 12s','>= 12s and < 15s','>= 15s and < 20s', '>= 20s','Total',' % of >= 15s',
			'< 10s','>= 10s and < 12s','>= 12s and < 15s','>= 15s and < 20s', '>= 20s','Total',' % of >= 15s')

	-- Result
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	SELECT
		4,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_log_ResponseP1_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP2_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP3_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP4_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP5_Enquire_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Enquire_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_ResponseP6_Enquire_Batch,'N/A'),		  
		ISNULL(CAST(No_log_ResponseP1_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP2_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP3_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP4_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP5_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_ResponseP6_Batch,'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm

	-- Empty Row
	INSERT INTO @ResultTable_Duration (Display_Seq) VALUES (5)

	------------------------------------------------------------------------
	-- DH CIMS
	------------------------------------------------------------------------
	-- DH CIMS Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (6, N'2) DH CIMS','','','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (7, '', N'EHS ⟶ DH CIMS response time (Batch Enquiry)','','','','','','',N'DH CIMS ⟶ EHS response time (Batch Enquiry)','','','','','','')  

	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (8, 'Date',
			'< 10s','>= 10s and < 12s','>= 12s and < 15s','>= 15s and < 20s', '>= 20s','Total',' % of >= 15s',
			'< 10s','>= 10s and < 12s','>= 12s and < 15s','>= 15s and < 20s', '>= 20s','Total',' % of >= 15s')

	-- Result
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	SELECT
		9,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_log_ResponseP1_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP2_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP3_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP4_Enquire_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP5_Enquire_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Enquire_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_ResponseP6_Enquire_Batch,'N/A'),		  
		ISNULL(CAST(No_log_ResponseP1_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP2_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP3_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP4_Batch AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP5_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Batch AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_ResponseP6_Batch,'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CIMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm

	-- Return Result
	SELECT 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13,Result_Value14,Result_Value15
	FROM 
		@ResultTable_Duration
	ORDER BY 
		Display_Seq, Result_Value1

	DELETE FROM @ResultTable_Duration

	------------------ eHS(S)D0018-04 : eVaccination Web Service Connection Duration Summary (Health Check) ------------------

	------------------------------------------------------------------------
	-- HA CMS 
	------------------------------------------------------------------------
	-- HA CMS Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (1,'1) HA CMS','','','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (2,'',N'EHS ⟶ HA CMS response time (Health Check)','','','','','','',N'HA CMS ⟶ EHS response time (Health Check)','','','','','','')  

	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (3,'Date', 
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s',
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s')

	-- Result
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	SELECT
		4,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_log_Response0to2_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8_Enquire_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Enquire_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6_Enquire_Health_Check,'N/A'),		  
		ISNULL(CAST(No_log_Response0to2_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6_Health_Check,'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm

	-- Empty Row
	INSERT INTO @ResultTable_Duration (Display_Seq) VALUES (5)

	------------------------------------------------------------------------
	-- DH CIMS
	------------------------------------------------------------------------
	-- DH CIMS Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (6,N'2) DH CIMS','','','','','','','','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (7, '', N'EHS ⟶ DH CIMS response time (Health Check)','','','','','','',N'DH CIMS ⟶ EHS response time (Health Check)','','','','','','')  

	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	VALUES (8, 'Date', 
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s',
			'< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s')

	-- Result
	INSERT INTO @ResultTable_Duration (Display_Seq, 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, 
		Result_Value15)
	SELECT
		9,
		CONVERT(varchar, ReportDate, 111),
		ISNULL(CAST(No_log_Response0to2_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8_Enquire_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8_Enquire_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Enquire_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6_Enquire_Health_Check,'N/A'),		  
		ISNULL(CAST(No_log_Response0to2_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response2to4_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response4to6_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response6to8_Health_Check AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_Response8_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs_Health_Check AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response6_Health_Check,'N/A')
	FROM
		@DateFrame F
		LEFT JOIN eHSD0018_CIMSVaccinationConnectionSummary_Stat S ON F.ReportDate = S.Report_Dtm

	-- Return Result
	SELECT 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
		Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13,Result_Value14,Result_Value15
	FROM 
		@ResultTable_Duration
	ORDER BY 
		Display_Seq, Result_Value1
	
	
	DELETE FROM @ResultTable_Duration

	------------------ eHS(S)D0018-05 : Report on vaccination transaction connection status --------------------------
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (1, 'Reporting period: ' + LTRIM(STR(@No_Of_Days_Raw_Data)) + ' days ending ' + CONVERT(varchar, Dateadd(Day, -1 ,GETDATE()), 111), '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)     
 VALUES (2, '', '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)      
 VALUES (3, 'Transaction Time', 'Connect System', 'Connection Status')  
 	
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)    
	SELECT
		4,
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		CASE Connect_System 
			WHEN 'CMS' THEN 'HA CMS'
			WHEN 'CIMS' THEN 'DH CIMS'
		END AS [Connect_System],
		Connection_Status
	FROM
		eHSD0018_CMSVaccinationConnectionStatus_Stat	
	WHERE
	     Report_Dtm BETWEEN @VaccineTranConnStatus_Start_Dtm AND @End_Dtm	
	ORDER BY
		 Report_Dtm ASC
		 
SELECT Result_Value1, Result_Value2, Result_Value3  FROM @ResultTable order by Display_Seq, Result_Value1
		 
DELETE FROM @ResultTable		 	
	
	
	
	------------------ eHS(S)D0018-06 : Report on vaccination connection problem from auditlog--------------------------
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)      
 VALUES (1, 'Reporting period: 2 weeks ending ' + CONVERT(varchar,Dateadd(Day, -1 ,GETDATE()), 111), '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)      
 VALUES (2, '', '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)      
 VALUES (3, 'System Date Time', 'Connect System', 'Error')  

 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)       	
	SELECT
		4,
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		CASE Connect_System 
			WHEN 'CMS' THEN 'HA CMS'
			WHEN 'CIMS' THEN 'DH CIMS'
		END AS [Connect_System],
		Error_Type
	FROM
		eHSD0018_CMSVaccinationConnectionErr_Stat	
	WHERE
	     Report_Dtm BETWEEN @Start_Dtm AND @End_Dtm	
	ORDER BY
		 Report_Dtm ASC
		 
SELECT Result_Value1, Result_Value2, Result_Value3  FROM @ResultTable order by Display_Seq, Result_Value1
		 
DELETE FROM @ResultTable			 	



	------------------ eHS(S)D0018-07 : Report on eVaccination Web Service connection duration (Single Enquiry)  -----------------
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (1, 'Reporting period: ' + LTRIM(STR(@No_Of_Days_Raw_Data)) + ' days ending ' + CONVERT(varchar,Dateadd(Day, -1 ,GETDATE()), 111), '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (2, '', '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (3, 'System Date Time', 'Time (in seconds)', 'Process System')  

 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)  
	SELECT
		4,
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Response_Time,
		[System]
	FROM
	(
		-- Enquire CMS response time
		SELECT Report_dtm, Response_Time, 'HA CMS' AS [System]
		FROM
			eHSD0018_EnquireCMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'N'
			AND Batch_Enquiry = 'N'
		UNION ALL
		-- Enquire EHS response time
		SELECT Report_dtm, Response_Time, 'EHS' AS [System]
		FROM
			eHSD0018_CMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'N'
			AND Batch_Enquiry = 'N'
		UNION ALL
		-- Enquire CIMS response time
		SELECT Report_dtm, Response_Time, 'DH CIMS' AS [System]
		FROM
			eHSD0018_EnquireCIMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'N'	
			AND Batch_Enquiry = 'N'
	) a
	ORDER BY
		 Report_Dtm ASC
	
SELECT Result_Value1, Result_Value2, Result_Value3  FROM @ResultTable order by Display_Seq, Result_Value1
		 
DELETE FROM @ResultTable


	------------------ eHS(S)D0018-08 : Report on eVaccination Web Service connection duration (Batch Enquiry)  -----------------
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)   
 VALUES (1, 'Reporting period: ' + LTRIM(STR(@No_Of_Days_Raw_Data)) + ' days ending ' + CONVERT(varchar,Dateadd(Day, -1 ,GETDATE()), 111), '', '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)   
 VALUES (2, '', '', '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)   
 VALUES (3, 'System Date Time', 'Time (in seconds)', 'Process System', 'No. of patients')  

 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)  
	SELECT
		4,
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Response_Time,
		[System],
		[Num_Of_Patient] = 
			CASE 
				WHEN Num_Of_Patient IS NULL THEN 'N/A'
				ELSE CONVERT(VARCHAR(MAX), Num_Of_Patient)
			END		
	FROM
	(
		-- Enquire CMS response time
		SELECT Report_dtm, Response_Time, 'HA CMS' AS [System], Num_Of_Patient
		FROM
			eHSD0018_EnquireCMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'N'
			AND Batch_Enquiry = 'Y'
		UNION ALL
		-- Enquire EHS response time
		SELECT Report_dtm, Response_Time, 'EHS' AS [System], NULL AS [Num_Of_Patient]
		FROM
			eHSD0018_CMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'N'
			AND Batch_Enquiry = 'Y'
		UNION ALL
		-- Enquire CIMS response time
		SELECT Report_dtm, Response_Time, 'DH CIMS' AS [System], Num_Of_Patient
		FROM
			eHSD0018_EnquireCIMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'N'	
			AND Batch_Enquiry = 'Y'
	) a
	ORDER BY
		 Report_Dtm ASC
	
SELECT Result_Value1, Result_Value2, Result_Value3, Result_Value4 FROM @ResultTable order by Display_Seq, Result_Value1
		 
DELETE FROM @ResultTable

	------------------ eHS(S)D0018-09 : Report on eVaccination Web Service connection duration (Health Check) -----------------
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (1, 'Reporting period: ' + LTRIM(STR(@No_Of_Days_Raw_Data)) + ' days ending ' + CONVERT(varchar,Dateadd(Day, -1 ,GETDATE()), 111), '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (2, '', '', '')  
   
 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)   
 VALUES (3, 'System Date Time', 'Time (in seconds)', 'Process System')  

 INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3)  
	SELECT
		4,
		CONVERT(varchar, Report_dtm, 111) + ' ' + CONVERT(varchar, Report_dtm, 108),
		Response_Time,
		[System]
	FROM
	(
		-- Enquire CMS response time
		SELECT Report_dtm, Response_Time, 'HA CMS' AS [System]
		FROM
			eHSD0018_EnquireCMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'Y'	
		UNION ALL
		-- Enquire EHS response time
		SELECT Report_dtm, Response_Time, 'EHS' AS [System]
		FROM
			eHSD0018_CMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'Y'
		UNION ALL
		-- Enquire CIMS response time
		SELECT Report_dtm, Response_Time, 'DH CIMS' AS [System]
		FROM
			eHSD0018_EnquireCIMSWebServiceResponseTime_Stat	
		WHERE
			Report_Dtm BETWEEN @EnquiryCMS_Start_Dtm AND @End_Dtm
			AND Health_Check = 'Y'
	) a
	ORDER BY
		 Report_Dtm ASC
		 
SELECT Result_Value1, Result_Value2, Result_Value3  FROM @ResultTable order by Display_Seq, Result_Value1
		 
DELETE FROM @ResultTable	
	
	END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_CMSVaccinationRecordConnection_Stat_Read] TO HCVU
GO

