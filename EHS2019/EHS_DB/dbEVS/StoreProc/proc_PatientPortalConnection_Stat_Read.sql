IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PatientPortalConnection_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PatientPortalConnection_Stat_Read]
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
--					Generation (Read from temp tables)
-- =============================================
CREATE PROCEDURE [dbo].[proc_PatientPortalConnection_Stat_Read]
AS BEGIN

	-- ===========================================================
	-- DECLARATION
	-- ===========================================================
	DECLARE @No_Of_Days INT  
	DECLARE @No_Of_Days_Raw_Data INT 

	DECLARE @Start_Dtm DATETIME  
	DECLARE @End_Dtm DATETIME  

	DECLARE @Data_Start_Dtm DATETIME

	-- Result Table
	DECLARE @ResultTable_Data TABLE (  
		Display_Seq		SMALLINT,  
		Result_Value1	VARCHAR(100),  
		Result_Value2	VARCHAR(100),
		Result_Value3	VARCHAR(100),
		Result_Value4	VARCHAR(100)
	)

	DECLARE @ResultTable_ResponseTime TABLE (  
		Display_Seq		SMALLINT,  
		Result_Value1	VARCHAR(100),  
		Result_Value2	NVARCHAR(100),
		Result_Value3	VARCHAR(100),
		Result_Value4	VARCHAR(100),
		Result_Value5	VARCHAR(100),
		Result_Value6	VARCHAR(100),
		Result_Value7	VARCHAR(100),
		Result_Value8	VARCHAR(100)
	)

	DECLARE @DateFrame TABLE (  
		ReportDate		DATETIME
	)

	-- =============================================
	-- Initialization
	-- =============================================
	SET @No_Of_Days = 14  
	SET @No_Of_Days_Raw_Data = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_name = 'eHSD0034ReportRawDataDays')
	   
	SET @End_Dtm = CONVERT(VARCHAR, GETDATE(), 106) -- "106" gives "dd MMM yyyy
	SET @Start_Dtm = DATEADD(dd, -(@No_Of_Days), @End_Dtm)  

	SET @Data_Start_Dtm = DATEADD(dd, -(@No_Of_Days_Raw_Data), @End_Dtm) 

	DECLARE @i AS INT = 1
	WHILE @i <= @No_Of_Days
	BEGIN
		INSERT INTO @DateFrame (ReportDate) VALUES(
			DATEADD(dd, -(@i), @End_Dtm) 
		)
		SET @i += 1
	END
	
	-- =============================================  
	-- Result  
	-- =============================================  

	------------------ Report Generation Time ------------------
	SELECT 'Report Generation Time: ' + CONVERT(varchar,GETDATE(), 111) + ' ' + CONVERT(varchar, GETDATE(), 108)


	------------------ eHS(S)D0034-01 : eHRSS Patient Portal Connection Duration Summary (Enquiry) ------------------

	------------------------------------------------------------------------
	-- Voucher Balance
	------------------------------------------------------------------------
	-- Voucher Balance Header
	INSERT INTO @ResultTable_ResponseTime 
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	VALUES 
		(1,N'1) Voucher Balance','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	VALUES 
		(2,'',N'EHRSS ⟶ EHS response time (Enquiry)','','','','','','')  

	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	VALUES 
		(3, 'Date', '< 2s','>= 2s and < 4s','>= 4s and < 6s','>= 6s and < 8s', '>= 8s','Total',' % of >= 6s')

	-- Result
	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	SELECT
		4,
		CONVERT(VARCHAR, ReportDate, 111),	  
		ISNULL(CAST(No_Log_ResponseP1 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Log_ResponseP2 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Log_ResponseP3 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Log_ResponseP4 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP5 AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response,'N/A')
	FROM
		@DateFrame F
			LEFT JOIN eHSD0034_PatientPortalConnectionSummary_Stat S 
				ON F.ReportDate = S.Report_Dtm
	WHERE
		Web_Service_Type = 'VoucherBalance'

	-- Empty Row
	INSERT INTO @ResultTable_ResponseTime (Display_Seq) VALUES (5)

	------------------------------------------------------------------------
	-- Doctor List
	------------------------------------------------------------------------
	-- Doctor List Header
	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	VALUES
		(6,N'2) Doctor List','','','','','','','')  

	-- Column Header
	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	VALUES
		(7,'',N'EHRSS ⟶ EHS response time (Enquiry)','','','','','','')  

	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	VALUES
		(8, 'Date', '< 10s','>= 10s and < 12s','>= 12s and < 15s','>= 15s and < 20s','>= 20s','Total','% of >= 15s')

	-- Result
	INSERT INTO @ResultTable_ResponseTime
		(Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	SELECT
		9,
		CONVERT(varchar, ReportDate, 111),	  
		ISNULL(CAST(No_Log_ResponseP1 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Log_ResponseP2 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Log_ResponseP3 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_Log_ResponseP4 AS VARCHAR(100)),'N/A'),
		ISNULL(CAST(No_log_ResponseP5 AS VARCHAR(100)),'N/A'),  
		ISNULL(CAST(No_logs AS VARCHAR(100)),'N/A'),  
		ISNULL(Percentage_Response,'N/A')
	FROM
		@DateFrame F
			LEFT JOIN eHSD0034_PatientPortalConnectionSummary_Stat S 
				ON F.ReportDate = S.Report_Dtm
	WHERE
		Web_Service_Type = 'DoctorList'

	-- Return Result
	SELECT 
		Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8
	FROM 
		@ResultTable_ResponseTime
	ORDER BY 
		Display_Seq, Result_Value1

	DELETE FROM @ResultTable_ResponseTime
	 	

	------------------ eHS(S)D0018-07 : Report on eVaccination Web Service connection duration (Single Enquiry)  -----------------
	INSERT INTO @ResultTable_Data (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)   
	VALUES (1, 'Reporting period: ' + LTRIM(STR(@No_Of_Days_Raw_Data)) + ' days ending ' + CONVERT(varchar,Dateadd(Day, -1 ,GETDATE()), 111), '', '', '')  
   
	INSERT INTO @ResultTable_Data (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)   
	VALUES (2, '', '', '', '')  
   
	INSERT INTO @ResultTable_Data (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)  
	VALUES (3, 'System Date Time', 'Time (in seconds)', 'Process System', 'Web Service')  

	INSERT INTO @ResultTable_Data (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)  
	SELECT
		4,
		CONVERT(VARCHAR, Report_dtm, 111) + ' ' + CONVERT(VARCHAR, Report_dtm, 108),
		Response_Time,
		'EHS',
		Web_Service_Type
	FROM
		eHSD0034_PatientPortalResponseTime_Stat	
	WHERE
		Report_Dtm >= @Data_Start_Dtm AND Report_Dtm < @End_Dtm
	ORDER BY
		Report_Dtm ASC
	
	SELECT Result_Value1, Result_Value2, Result_Value3, Result_Value4  FROM @ResultTable_Data order by Display_Seq, Result_Value1
		 
	DELETE FROM @ResultTable_Data
	
END 
GO

GRANT EXECUTE ON [dbo].[proc_PatientPortalConnection_Stat_Read] TO HCVU
GO

