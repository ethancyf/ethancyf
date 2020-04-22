IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0004_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0004_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================    
-- Modification History    
-- Modified by:      
-- Modified date:     
-- Description:      
-- =============================================    
-- =============================================
-- CR No.:			CRE16-019
-- Author:			Chris YIM
-- Create date:		06 Feb 2017
-- Description:		Monthly Deactivated eHRSS Token Report - eHSM0004
-- =============================================    

CREATE Procedure [proc_EHS_eHSM0004_Report_get]    
	@request_dtm			DATETIME = NULL,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from		DATETIME = NULL,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = NULL			-- The Target Period To. If defined, it will override the value from the @request_dtm
AS BEGIN    

SET NOCOUNT ON;    
	-- =============================================    
	-- Declaration    
	-- =============================================
	------------------------------------------------------------------------------------------
	-- Report Helper Field
	DECLARE @Report_ID				VARCHAR(30)	= 'eHSM0004'
	DECLARE @Result1Count			INT = 0
	DECLARE @Result2Count			INT = 0

	------------------------------------------------------------------------------------------
	-- Date 
	DECLARE @IN_request_dtm			DATETIME
	DECLARE @IN_target_period_from	DATETIME
	DECLARE @IN_target_period_to	DATETIME

	------------------------------------------------------------------------------------------
	-- Result Table
	CREATE TABLE #ResultTable1 (
		Result_Seq INT IDENTITY(1,1),	-- Sorting Sequence
		Result_Value1 VARCHAR(100),		-- eHRSS Notification Time
		Result_Value2 VARCHAR(100),		-- Deactivation Time in eHRSS
		Result_Value3 VARCHAR(100),		-- Deactivated Token Serial No. in eHRSS
		Result_Value4 VARCHAR(100),		-- Deactivated Token Serial No. (New) in eHRSS
		Result_Value5 VARCHAR(100)		-- SP ID
	)

	CREATE TABLE #ResultTable2 (
		Result_Seq INT IDENTITY(1,1),	-- Sorting Sequence
		Result_Value1 VARCHAR(100),		-- eHRSS Notification Time
		Result_Value2 VARCHAR(100),		-- Deactivation Time in eHRSS
		Result_Value3 VARCHAR(100),		-- Deactivated Token Serial No. in eHRSS
		Result_Value4 VARCHAR(100),		-- Deactivated Token Serial No. (New) in eHRSS
		Result_Value5 VARCHAR(100)		-- SP ID
	)
		
	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================   
	SET @IN_request_dtm = @request_dtm
	SET @IN_target_period_from = @target_period_from
	SET @IN_target_period_to = @target_period_to
	 
	------------------------------------------------------------------------------------------
	-- Init the Request_Dtm (Reference) DateTime to Avoid Null value
	IF @IN_request_dtm IS NULL
		SET @IN_request_dtm = GETDATE()

	-- First Day of Last Month, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @IN_target_period_from IS NULL
		SET @IN_target_period_from = DATEFROMPARTS(DATEPART(YEAR,DATEADD(MONTH, -1, GETDATE())) ,DATEPART(MONTH, DATEADD(MONTH, -1, GETDATE())), 1)
	ELSE
		SET @IN_target_period_from = DATEFROMPARTS(DATEPART(YEAR, @IN_target_period_from) ,DATEPART(MONTH, @IN_target_period_from), DATEPART(DAY, @IN_target_period_from))

	-- Last Day of Last Month, ensure the time start from 00:00 (datetime compare logic use "<", so should be First Day of Current Month)
	IF @IN_target_period_to IS NULL
		SET @IN_target_period_to = DATEFROMPARTS(DATEPART(YEAR,DATEADD(MONTH, 0, GETDATE())) ,DATEPART(MONTH, DATEADD(MONTH, 0, GETDATE())), 1)
	ELSE
		SET @IN_target_period_to = DATEFROMPARTS(DATEPART(YEAR, @IN_target_period_to) ,DATEPART(MONTH, @IN_target_period_to), DATEPART(DAY, @IN_target_period_to))
	
	-- =============================================    
	-- Determine whether has result record(s)
	-- =============================================  

	SELECT 
		@Result1Count = COUNT(1) 
	FROM 
		TokenAction WITH (NOLOCK)
	WHERE
		[Notification_Dtm] >= @IN_target_period_from AND [Notification_Dtm] < @IN_target_period_to
		AND Action_Type = 'NOTIFYDELETETOKEN'
		AND Source_Party = 'EHR'
		AND Action_Result = 'C'

	SELECT 
		@Result2Count = COUNT(1) 
	FROM 
		TokenAction WITH (NOLOCK)
	WHERE
		[Notification_Dtm] >= @IN_target_period_from AND [Notification_Dtm] < @IN_target_period_to
		AND Action_Type = 'NOTIFYSETSHARE'
		AND Source_Party = 'EHR'
		AND Action_Remark = 'N'
		AND Action_Result = 'C'

	-- =============================================    
	-- Prepare Data
	-- =============================================  

	-------------------------
	-- Result Table : 01
	-------------------------
	-- Header 1
	INSERT INTO #ResultTable1 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) 
	VALUES ('Reporting Period: ' + CONVERT(VARCHAR(10), @IN_target_period_from, 111) + ' to ' + CONVERT(VARCHAR(10), DATEADD(d, -1, @IN_target_period_to), 111),'','','','')														

	-- Line Break Before Data
	INSERT INTO #ResultTable1 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES ('','','','','')

	IF @Result1Count > 0
	BEGIN
		-- Column Header
		INSERT INTO #ResultTable1 (
			Result_Value1, 
			Result_Value2, 
			Result_Value3, 
			Result_Value4, 
			Result_Value5)
		VALUES(
			'eHRSS Notification Time', 
			'Deactivation Time in eHRSS', 
			'Deactivated Token Serial No. in eHRSS', 
			'Deactivated Token Serial No. (New) in eHRSS', 
			'SPID'
			)

		-- Report Content
		INSERT INTO #ResultTable1 (
			Result_Value1, 
			Result_Value2, 
			Result_Value3, 
			Result_Value4, 
			Result_Value5)
		SELECT	
			FORMAT([Notification_Dtm], 'yyyy-MM-dd HH:mm:ss', 'en-us') AS [Notification_Dtm],
			FORMAT([Action_Dtm], 'yyyy-MM-dd HH:mm:ss', 'en-us') AS [Notification_Timestamp],
			[Token_Serial_No],
			ISNULL([Token_Serial_No_Replacement],''),
			LTRIM(RTRIM([User_ID]))
		FROM
			[TokenAction] WITH (NOLOCK)
		WHERE
			[Notification_Dtm] >= @IN_target_period_from AND [Notification_Dtm] < @IN_target_period_to
			AND Action_Type = 'NOTIFYDELETETOKEN'
			AND Source_Party = 'EHR'
			AND Action_Result = 'C'
	END
	ELSE
	BEGIN
		-- Statement for no record
		INSERT INTO #ResultTable1 (
			Result_Value1, 
			Result_Value2, 
			Result_Value3, 
			Result_Value4, 
			Result_Value5)
		VALUES(
			'There is no record in the reporting period', 
			'', 
			'', 
			'', 
			''
			)
	END

	-------------------------
	-- Result Table : 02
	-------------------------
	-- Header 1
	INSERT INTO #ResultTable2 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) 
	VALUES ('Reporting Period: ' + CONVERT(VARCHAR(10), @IN_target_period_from, 111) + ' to ' + CONVERT(VARCHAR(10), DATEADD(d, -1, @IN_target_period_to), 111),'','','','')														

	-- Line Break Before Data
	INSERT INTO #ResultTable2 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES ('','','','','')

	IF @Result2Count > 0
	BEGIN
		-- Column Header
		INSERT INTO #ResultTable2 (
			Result_Value1, 
			Result_Value2, 
			Result_Value3, 
			Result_Value4, 
			Result_Value5)
		VALUES(
			'eHRSS Notification Time', 
			'Not Share Token Time in eHRSS', 
			'Token Serial No. in eHS(S)', 
			'Token Serial No. (New) in eHS(S)', 
			'SPID'
			)

		-- Report Content
		INSERT INTO #ResultTable2 (
			Result_Value1, 
			Result_Value2, 
			Result_Value3, 
			Result_Value4, 
			Result_Value5)
		SELECT	
			FORMAT(TA.[Notification_Dtm], 'yyyy-MM-dd HH:mm:ss', 'en-us') AS [Notification_Dtm],
			FORMAT(TA.[Action_Dtm], 'yyyy-MM-dd HH:mm:ss', 'en-us') AS [Notification_Timestamp],
			[Token_Serial_No] = 
				CASE 
					WHEN T.[User_ID] IS NOT NULL THEN T.[Token_Serial_No]
					WHEN DELETETOKEN_TA.[User_ID] IS NOT NULL THEN DELETETOKEN_TA.[Token_Serial_No] 
					ELSE ''
				END,
			[Token_Serial_No_Replacement]=
							CASE 
					WHEN T.[Token_Serial_No_Replacement] IS NOT NULL THEN T.[Token_Serial_No_Replacement]
					WHEN DELETETOKEN_TA.[Token_Serial_No_Replacement] IS NOT NULL THEN DELETETOKEN_TA.[Token_Serial_No_Replacement] 
					ELSE ''
				END,
			LTRIM(RTRIM(TA.[User_ID]))
		FROM
			[TokenAction] TA WITH (NOLOCK)
				LEFT OUTER JOIN [Token] T
					ON TA.[User_ID] = T.[User_ID]
				LEFT OUTER JOIN (
						SELECT 
							[USER_ID],
							[Token_Serial_No],
							[Token_Serial_No_Replacement],
							[Action_Dtm]
						FROM	
							[TokenAction]
						WHERE
							[Action_Type] = 'DELETETOKEN'
							AND [Action_Result] = 'C'
					) DELETETOKEN_TA
					ON TA.[User_ID] = DELETETOKEN_TA.[User_ID]
						AND  TA.[Action_Dtm] < DELETETOKEN_TA.[Action_Dtm]	
				LEFT OUTER JOIN (
						SELECT 
							[USER_ID],
							MAX([Action_Dtm]) AS [Action_Dtm]
						FROM	
							[TokenAction]
						WHERE
							[Action_Type] = 'DELETETOKEN'
							AND [Action_Result] = 'C'
						GROUP BY
							[USER_ID]
					) LATEST_DELETETOKEN_TA
					ON DELETETOKEN_TA.[User_ID] = LATEST_DELETETOKEN_TA.[User_ID]
						AND DELETETOKEN_TA.[Action_Dtm] = LATEST_DELETETOKEN_TA.[Action_Dtm]	
						
		WHERE
			TA.[Notification_Dtm] >= @IN_target_period_from AND TA.[Notification_Dtm] < @IN_target_period_to
			AND TA.Action_Type = 'NOTIFYSETSHARE'
			AND TA.Source_Party = 'EHR'
			AND TA.Action_Remark = 'N'
			AND TA.Action_Result = 'C'
			AND ((DELETETOKEN_TA.[User_ID] IS NULL AND LATEST_DELETETOKEN_TA.[User_ID] IS NULL)
				OR
				(DELETETOKEN_TA.[User_ID] IS NOT NULL AND LATEST_DELETETOKEN_TA.[User_ID] IS NOT NULL))
	END
	ELSE
	BEGIN
		-- Statement for no record
		INSERT INTO #ResultTable2 (
			Result_Value1, 
			Result_Value2, 
			Result_Value3, 
			Result_Value4, 
			Result_Value5)
		VALUES(
			'There is no record in the reporting period', 
			'', 
			'', 
			'', 
			''
			)
	END
	   
-- =============================================
-- Return results
-- =============================================	
			
	--------------------------
	-- Result Set 1: Content
	--------------------------
	SELECT	
		'Report Generation Time: ' + CONVERT(VARCHAR, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value
	
	--------------------------
	-- Result Set 2: 01
	--------------------------
	SELECT	
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5
	FROM	
		#ResultTable1
	ORDER BY	
		Result_Seq

	--------------------------
	-- Result Set 3: 02
	--------------------------
	SELECT	
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5
	FROM	
		#ResultTable2
	ORDER BY	
		Result_Seq


	DROP TABLE #ResultTable1
	DROP TABLE #ResultTable2
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0004_Report_get] TO HCVU

GO
