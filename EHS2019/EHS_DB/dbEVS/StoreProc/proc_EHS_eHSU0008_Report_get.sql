IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0008_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0008_Report_get]
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
-- ==============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	17 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Add Scheme "HCVSDHC"
-- ===============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		07 Feb 2018
-- CR No.:			CRE16-014 to 016 (Voucher aberrant and new monitoring)
-- Description:		Get Report for eHSU0008
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0008_Report_get]
	@request_time	datetime,
	@From_Date		varchar(17),
	@To_Date		varchar(17)
AS BEGIN	

--SET @From_Date = '2019 Nov 01'
--SET @To_Date = '2019 Nov 20'
-- =============================================
-- Declaration
-- =============================================

	DECLARE @Scheme_Code		CHAR(10) 
	DECLARE @current_dtm		datetime
	DECLARE @seq				int
	DECLARE @RStatus			VARCHAR(100)
	DECLARE @DateTypeFN			char(30)
	DECLARE @From_Dtm			datetime
	DECLARE @To_Dtm				datetime
	DECLARE @DisplayFromDate	varchar(17)
	DECLARE @DisplayToDate		varchar(17)

	DECLARE @TargetRank			INT	-- No. of SP with highest daily claim to be shown (in Rank) 
	DECLARE @Report_ID			VARCHAR(10)
	
	-- Voucher Tx list within period
	CREATE TABLE #VoucherTransaction (
		SP_ID					CHAR(8),
		Service_Date			DATETIME,
		Practice_Display_Seq	INT,
		Daily_Tx_Count			INT,
		Create_By_SmartID_Y		INT,
		Create_By_SmartID_N		INT,
		Record_Status_P			INT,
		Record_Status_V			INT,
		Record_Status_A			INT,
		Record_Status_R			INT,
		Record_Status_S			INT,
		Record_Status_B			INT,
		Record_Status_U			INT
	)

	-- Create Daily Claim Master list
	CREATE TABLE #Daily_Claim (
		Rank					INT,
		SP_ID					CHAR(8),
		Service_Date			DATETIME,
		Daily_Tx_Count			INT
	)

	-- Create Daily Claim Master list with detail by Practice
	CREATE TABLE #Daily_Claim_MasterList_Practice (
		Rank					INT,
		SP_ID					CHAR(8),
		SP_Name					VARCHAR(100),
		SP_Name_Chi				NVARCHAR(100),
		Professional_Code		CHAR(5),
		Practice_Display_Seq	SMALLINT,
		Practice_Name			VARCHAR(200),
		Practice_Name_Chi		NVARCHAR(200),
		Practice_Address		VARCHAR(300),
		Practice_Address_Chi	NVARCHAR(300),		
		Phone_Daytime			VARCHAR(20),
		Practice_district_board	CHAR(15),
		Practice_district_name	CHAR(15),
		Service_Date			DATETIME,
		Daily_Tx_Count			INT,
		Create_By_SmartID_Y		INT,
		Create_By_SmartID_N		INT,
		Record_Status_P			INT,
		Record_Status_V			INT,
		Record_Status_A			INT,
		Record_Status_R			INT,
		Record_Status_S			INT,
		Record_Status_B			INT,
		Record_Status_U			INT
	)

	-- Create Daily Claim Master list with detail by SP
	CREATE TABLE #Daily_Claim_MasterList_SP (
		Rank					INT,
		SP_ID					CHAR(8),
		SP_Name					VARCHAR(100),
		SP_Name_Chi				NVARCHAR(100),
		Professional_Code		CHAR(100),	-- Group by Practice
		No_of_Practice			INT,
		Service_Date			DATETIME,
		Daily_Tx_Count			INT,
		Create_By_SmartID_Y		INT,
		Create_By_SmartID_N		INT,
		Record_Status_P			INT,
		Record_Status_V			INT,
		Record_Status_A			INT,
		Record_Status_R			INT,
		Record_Status_S			INT,
		Record_Status_B			INT,
		Record_Status_U			INT
	)

	-- Create Worksheet 02 Result Table
	DECLARE @WS02 AS TABLE(
		Seq		INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	-- Create Worksheet 03 Result Table  01-EHCP
	DECLARE @WS03 AS TABLE (
		Seq		INT,
		SubSeq	INT		IDENTITY(1,1),
		Col01	VARCHAR(100)	DEFAULT '',
		Col02	VARCHAR(100)	DEFAULT '',	
		Col03	VARCHAR(100)	DEFAULT '',
		Col04	NVARCHAR(100)	DEFAULT '',	
		Col05	VARCHAR(100)	DEFAULT '',	
		Col06	VARCHAR(100)	DEFAULT '',	
		Col07	VARCHAR(100)	DEFAULT '',	
		Col08	VARCHAR(100)	DEFAULT '',	
		Col09	VARCHAR(100)	DEFAULT '',	
		Col10	VARCHAR(100)	DEFAULT '',	
		Col11	NVARCHAR(100)	DEFAULT '',	
		Col12	VARCHAR(100)	DEFAULT '',	
		Col13	NVARCHAR(100)	DEFAULT '',	
		Col14	VARCHAR(100)	DEFAULT '',	
		Col15	VARCHAR(100)	DEFAULT '',	
		Col16	VARCHAR(100)	DEFAULT '',	
		Col17	VARCHAR(100)	DEFAULT '',	
		Col18	VARCHAR(100)	DEFAULT '',	
		Col19	VARCHAR(100)	DEFAULT ''
	)

	-- Create Worksheet 04 Result Table  02-Practice
	DECLARE @WS04 AS TABLE(
		Seq		INT,
		SubSeq	INT		IDENTITY(1,1),						
		Col01	VARCHAR(100)	DEFAULT '',
		Col02	VARCHAR(100)	DEFAULT '',	
		Col03	VARCHAR(100)	DEFAULT '',
		Col04	NVARCHAR(100)	DEFAULT '',	
		Col05	VARCHAR(100)	DEFAULT '',	
		Col06	VARCHAR(100)	DEFAULT '',	
		Col07	VARCHAR(200)	DEFAULT '',	
		Col08	NVARCHAR(200)	DEFAULT '',	
		Col09	VARCHAR(300)	DEFAULT '',	
		Col10	NVARCHAR(300)	DEFAULT '',	
		Col11	NVARCHAR(100)	DEFAULT '',	
		Col12	VARCHAR(100)	DEFAULT '',	
		Col13	NVARCHAR(100)	DEFAULT '',	
		Col14	VARCHAR(100)	DEFAULT '',	
		Col15	VARCHAR(100)	DEFAULT '',	
		Col16	VARCHAR(100)	DEFAULT '',	
		Col17	VARCHAR(100)	DEFAULT '',	
		Col18	VARCHAR(100)	DEFAULT '',	
		Col19	VARCHAR(100)	DEFAULT '',
		Col20	VARCHAR(100)	DEFAULT '',	
		Col21	VARCHAR(100)	DEFAULT '',	
		Col22	VARCHAR(100)	DEFAULT '',	
		Col23	VARCHAR(100)	DEFAULT '',	
		Col24	VARCHAR(100)	DEFAULT '',
		Col25	VARCHAR(100)	DEFAULT '',	
		Col26	VARCHAR(100)	DEFAULT ''
	)

	-- Worksheet: Remark
	DECLARE @Remark AS TABLE (
		Seq		int,
		Col01	varchar(1000),
		Col02	varchar(1000)
	)

	-- Scheme
	DECLARE @Scheme AS TABLE(
		Scheme_Code		CHAR(10) 
	)

-- =============================================
-- Initialization
-- =============================================

	INSERT INTO @Scheme (Scheme_Code) VALUES 
	('HCVS'), ('HCVSDHC')
	

	SET @Report_ID = 'eHSU0008'
	
	-- No. of SP with highest daily claim to be shown (in Rank) 
	SET @TargetRank = 300

   	IF @request_time IS NULL BEGIN    
	  SET @request_time = DATEADD(dd, -1, CONVERT(VARCHAR(11), GETDATE(), 106)) -- "106" gives "dd MMM yyyy"    
	END    

	SET @current_dtm = GETDATE()

	--Filter Date (DATETIME)
	SELECT @From_Dtm = Cast(CONVERT(VARCHAR(11), @From_Date, 111) + ' 00:00:00' as datetime) 

	SELECT @To_Dtm = Cast(CONVERT(VARCHAR(11), DATEADD(D, +1, @To_Date)) + ' 00:00:00' as datetime) 

	--Display Date (VARCHAR)
	SELECT @DisplayFromDate = FORMAT(@From_Dtm, 'yyyy/MM/dd' , 'en-US')

	SELECT @DisplayToDate = FORMAT(DATEADD(D, -1, @To_Dtm), 'yyyy/MM/dd' , 'en-US')
	
-- =============================================
-- Retrieve Data
-- =============================================

	--Get all VT within period
	INSERT INTO #VoucherTransaction (SP_ID, Service_Date, Practice_Display_Seq, Daily_Tx_Count, 
									Create_By_SmartID_Y, Create_By_SmartID_N,
									Record_Status_P, Record_Status_V, Record_Status_A,
									Record_Status_R, Record_Status_S, Record_Status_B, Record_Status_U)
	SELECT
		SP_ID,
		Service_Receive_Dtm,
		Practice_Display_Seq,
		COUNT(Transaction_ID) AS [Daily_Tx_Count],
		SUM(CASE WHEN ISNULL(Create_By_SmartID,'N') = 'Y' THEN 1 ELSE 0 END) AS [Create_By_SmartID_Y],
		SUM(CASE WHEN ISNULL(Create_By_SmartID,'N') = 'N' THEN 1 ELSE 0 END) AS [Create_By_SmartID_N],
		SUM(CASE WHEN Record_Status = 'P' THEN 1 ELSE 0 END) AS [Record_Status_P],
		SUM(CASE WHEN Record_Status = 'V' THEN 1 ELSE 0 END) AS [Record_Status_V],
		SUM(CASE WHEN Record_Status = 'A' THEN 1 ELSE 0 END) AS [Record_Status_A],
		SUM(CASE WHEN Record_Status = 'R' THEN 1 ELSE 0 END) AS [Record_Status_R],
		SUM(CASE WHEN Record_Status = 'S' THEN 1 ELSE 0 END) AS [Record_Status_S],
		SUM(CASE WHEN Record_Status = 'B' THEN 1 ELSE 0 END) AS [Record_Status_B],
		SUM(CASE WHEN Record_Status = 'U' THEN 1 ELSE 0 END) AS [Record_Status_U]
	FROM 
		VoucherTransaction WITH (NOLOCK)
	WHERE 
		Scheme_Code IN (SELECT Scheme_Code FROM @Scheme)
		AND Service_Receive_Dtm >= @From_Dtm 
		AND Service_Receive_Dtm < @To_Dtm
		AND Record_Status NOT IN    
			(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
			AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm)))       
		AND (Invalidation IS NULL OR Invalidation NOT IN
			(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
			AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm))))
	GROUP BY
		SP_ID, Service_Receive_Dtm, Practice_Display_Seq
		

	--Get Daily Claim Master list
	INSERT INTO #Daily_Claim (Rank, SP_ID, Service_Date, Daily_Tx_Count)
	SELECT
		SP_Rank,
		SP_ID, 
		Service_Date,
		Daily_Tx_Count
	FROM
	(	-- Get SP in rank with highest daily claim
		SELECT
			DENSE_RANK() OVER(ORDER BY [Daily_Tx_Count] DESC) AS [SP_Rank],
			SP_ID, 
			Service_Date,
			Daily_Tx_Count
		FROM 
		(	-- Get highest daily claim for each SP
			SELECT
				ROW_NUMBER() OVER(PARTITION BY SP_ID ORDER BY SUM(Daily_Tx_Count) DESC, Service_Date DESC) AS [Claim_Rank],
				SP_ID, 
				Service_Date,
				SUM(Daily_Tx_Count) AS [Daily_Tx_Count]
			FROM #VoucherTransaction
			GROUP BY SP_ID, Service_Date
		) SP_Daily
		WHERE Claim_Rank = 1
	) R 
	WHERE R.SP_Rank <= @TargetRank


	EXEC [proc_SymmetricKey_open]

	--Get SP daily claim with detail
	INSERT INTO #Daily_Claim_MasterList_Practice
	(
		Rank,
		SP_ID,					
		SP_Name,				
		SP_Name_Chi,
		Professional_Code,		
		Practice_Display_Seq,	
		Practice_Name,			
		Practice_Name_Chi,
		Practice_Address,		
		Practice_Address_Chi,
		Phone_Daytime,
		Practice_district_board,
		Practice_district_name,
		Service_Date,
		Daily_Tx_Count,
		Create_By_SmartID_Y,
		Create_By_SmartID_N,
		Record_Status_P,
		Record_Status_V,
		Record_Status_A,
		Record_Status_R,
		Record_Status_S,
		Record_Status_B,
		Record_Status_U					
	)
	SELECT 
		DC.Rank,
		DC.SP_ID,
		CONVERT(VARCHAR(MAX), DECRYPTBYKEY(SP.Encrypt_Field2)) AS [SP Name (In English)],
		CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(SP.Encrypt_Field3)) AS [SP Name (In Chinese)],
		Pro.Service_Category_Code,
		P.Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		[dbo].[func_formatEngAddress](P.[Room], P.[Floor], P.[Block], P.[Building], P.[District]) AS Practice_Address,
		[dbo].[func_format_Address_Chi](P.[Room], P.[Floor], P.[Block], P.[Building_Chi], P.[District]) AS  Practice_Address_Chi,		
		P.Phone_Daytime,
		D.district_board,
		D.district_name,
		VT.Service_Date,
		VT.Daily_Tx_Count,
		VT.Create_By_SmartID_Y,
		VT.Create_By_SmartID_N,
		VT.Record_Status_P,
		VT.Record_Status_V,
		VT.Record_Status_A,
		VT.Record_Status_R,
		VT.Record_Status_S,
		VT.Record_Status_B,
		VT.Record_Status_U	
	FROM 
		#Daily_Claim DC
	INNER JOIN
		#VoucherTransaction VT
			ON DC.SP_ID = VT.SP_ID AND DC.Service_Date = VT.Service_Date
	INNER JOIN
		ServiceProvider SP WITH (NOLOCK)
			ON VT.SP_ID = SP.SP_ID
	INNER JOIN 
		Practice P WITH (NOLOCK)
			ON VT.SP_ID = P.SP_ID AND VT.Practice_Display_Seq = P.Display_Seq
	INNER JOIN
		Professional Pro  WITH (NOLOCK)
			ON P.SP_ID = Pro.SP_ID AND P.Professional_Seq = Pro.Professional_Seq
	INNER JOIN
		District D  WITH (NOLOCK)
			ON P.District = D.district_code


	EXEC [proc_SymmetricKey_close]


	--Get SP daily claim with detail
	INSERT INTO #Daily_Claim_MasterList_SP
	(
		Rank,
		SP_ID,					
		SP_Name,				
		SP_Name_Chi,
		Professional_Code,		
		No_of_Practice,
		Service_Date,
		Daily_Tx_Count,
		Create_By_SmartID_Y,
		Create_By_SmartID_N,
		Record_Status_P,
		Record_Status_V,
		Record_Status_A,
		Record_Status_R,
		Record_Status_S,
		Record_Status_B,
		Record_Status_U					
	)
	SELECT
		Rank,
		SP_ID,
		SP_Name,
		SP_Name_Chi,
		STUFF(
			(SELECT
					', ' + Pro.Service_Category_Code
				FROM #Daily_Claim_MasterList_Practice DP2
				INNER JOIN
					Profession Pro 
						ON DP2.Professional_Code = Pro.Service_Category_Code
				WHERE DP1.SP_ID = DP2.SP_ID
				GROUP BY
					SD_Display_Seq, Pro.Service_Category_Code
				ORDER BY
					SD_Display_Seq
				FOR XML PATH(''))
		  , 1, 2, '') AS [Professional],
		COUNT(Practice_Display_Seq),
		Service_Date,		
		SUM(Daily_Tx_Count) AS [Daily_Tx_Count],
		SUM(Create_By_SmartID_Y) AS [Create_By_SmartID_Y],
		SUM(Create_By_SmartID_N) AS [Create_By_SmartID_N],		
		SUM(Record_Status_P) AS [Record_Status_P],
		SUM(Record_Status_V) AS [Record_Status_V],
		SUM(Record_Status_A) AS [Record_Status_A],
		SUM(Record_Status_R) AS [Record_Status_R],
		SUM(Record_Status_S) AS [Record_Status_S],
		SUM(Record_Status_B) AS [Record_Status_B],
		SUM(Record_Status_U) AS [Record_Status_U]
	FROM 
		#Daily_Claim_MasterList_Practice DP1
	GROUP BY
		Rank,
		SP_ID,
		SP_Name,
		SP_Name_Chi,
		Service_Date

	--select * from #Daily_Claim_MasterList_Practice order by Rank, SP_ID, Practice_Display_Seq
	--select * from #Daily_Claim_MasterList_SP order by Rank, SP_ID

-- ---------------------------------------------
-- For Excel Sheet (02): Criteria
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO @WS02 (Seq, Col01, Col02)
	VALUES (@seq, 'Criteria', '')

	SET @seq = @seq + 1

	INSERT INTO @WS02 (Seq, Col01, Col02)
	VALUES (@seq, 'Service Date', @DisplayFromDate +' to '+ @DisplayToDate)
						  	
-- ---------------------------------------------
-- For Excel Sheet (03): 01-EHCP
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO @WS03 (Seq, Col01)
	VALUES (@seq, 'Service date period: '+@DisplayFromDate+' to '+ @DisplayToDate)

	SET @seq = @seq + 1

	INSERT INTO @WS03 (Seq) VALUES (@seq)

	SET @seq = @seq + 1

	-- Check record exsit for generate 'Record Result' or generate 'No Record Result' 
	IF EXISTS
	(	
		SELECT TOP 1 * FROM #Daily_Claim
	)
	BEGIN
	-- Record Exsit
		-- Create Report Header
		INSERT INTO @WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19)
		VALUES (@seq, '', '', '', '', '', '', '', '', '', '', '', '', 'Breakdown claims by Transaction Status', 
				'', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO @WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19)
		VALUES (
			@seq,
			'Rank', 'SPID', 'SP Name (In English)', 'SP Name (In Chinese)', 'Profession', 'Total no. of practices involved',
			'Service Date of the most daily claim', 'Total no. of claims of the most daily claim', 'No. of claims using Smart IC', '%',
			'No. of claims Not using Smart IC', '%', 'Pending Confirmation', 'Pending eHealth (Subsidies) Account Validation',
			'Ready to Reimburse', 'Reimbursed', 'Suspended', 'Pending Approval (Back Office)', 'Incomplete')
																	
		-- Create Report Result
		SET @seq = @seq + 1

		INSERT INTO @WS03 (
			Seq,
			Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19)
		SELECT 
			@seq,
			Rank,
			SP_ID,					
			SP_Name,				
			SP_Name_Chi,
			Professional_Code,		
			No_of_Practice,
			CONVERT(VARCHAR(10),Service_Date,20),
			Daily_Tx_Count,
			Create_By_SmartID_Y,
			CAST(CAST(Create_By_SmartID_Y AS DECIMAL)/CAST(Daily_Tx_Count AS DECIMAL)*100 AS DECIMAL(10,1)) AS [Create_By_SmartID_Y_Rate],		
			Create_By_SmartID_N,
			CAST(CAST(Create_By_SmartID_N AS DECIMAL)/CAST(Daily_Tx_Count AS DECIMAL)*100 AS DECIMAL(10,1)) AS [Create_By_SmartID_N_Rate],
			Record_Status_P,
			Record_Status_V,
			Record_Status_A,
			Record_Status_R,
			Record_Status_S,
			Record_Status_B,
			Record_Status_U	
		FROM #Daily_Claim_MasterList_SP
		ORDER BY Rank, SP_ID	

	END
	ELSE
	BEGIN
	-- Record Not Exsit

		-- Create No Record Result
		INSERT INTO @WS03 (Seq, Col01)
		VALUES (@seq, 'There is no record in the service date period')

		SET @seq = @seq + 1

		INSERT INTO @WS03 (Seq) VALUES (@seq)
	END

-- ---------------------------------------------
-- For Excel Sheet (04): 02-Practice
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO @WS04 (Seq, Col01)
	VALUES (@seq, 'Service date period: '+@DisplayFromDate+' to '+ @DisplayToDate)

	SET @seq = @seq + 1

	INSERT INTO @WS04 (Seq) VALUES (@seq)

	SET @seq = @seq + 1

	-- Check record exsit for generate 'Record Result' or generate 'No Record Result' 
	IF EXISTS
	(	
		SELECT TOP 1 * FROM #Daily_Claim
	)
	BEGIN
	-- Record Exsit
		-- Create Report Header
		INSERT INTO @WS04 (
			Seq,
			Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25, Col26)
		VALUES (@seq, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '',
				'Breakdown claims by Transaction Status', '', '', '', '', '','')

		SET @seq = @seq + 1

		INSERT INTO @WS04 (
			Seq,
			Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25, Col26)
		VALUES (
			@seq,
			'Rank', 'SPID', 'SP Name (In English)', 'SP Name (In Chinese)', 'Profession', 
			'Practice No.', 'Practice Name (In English)','Practice Name (In Chinese)', 'Practice Address (In English)', 
			'Practice Address (In Chinese)', 'Phone No. of Practice', 'District Board', 'District', 
			'Service Date of the most daily claim', 'Total no. of claims of the most daily claim', 'No. of claims using Smart IC', '%',
			'No. of claims Not using Smart IC', '%', 'Pending Confirmation', 'Pending eHealth (Subsidies) Account Validation',
			'Ready to Reimburse', 'Reimbursed', 'Suspended', 'Pending Approval (Back Office)', 'Incomplete'
		)
																	
		-- Create Report Result
		SET @seq = @seq + 1

		INSERT INTO @WS04 (
			Seq,
			Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25, Col26)
		SELECT
			@seq,
			Rank,
			SP_ID,					
			SP_Name,				
			SP_Name_Chi,
			Professional_Code,		
			Practice_Display_Seq,	
			Practice_Name,			
			Practice_Name_Chi,
			Practice_Address,		
			Practice_Address_Chi,
			Phone_Daytime,
			Practice_district_board,
			Practice_district_name,
			CONVERT(VARCHAR(10),Service_Date,20),
			Daily_Tx_Count,
			Create_By_SmartID_Y,
			CAST(CAST(Create_By_SmartID_Y AS DECIMAL)/CAST(Daily_Tx_Count AS DECIMAL)*100 AS DECIMAL(10,1)) AS [Create_By_SmartID_Y_Rate],		
			Create_By_SmartID_N,
			CAST(CAST(Create_By_SmartID_N AS DECIMAL)/CAST(Daily_Tx_Count AS DECIMAL)*100 AS DECIMAL(10,1)) AS [Create_By_SmartID_N_Rate],
			Record_Status_P,
			Record_Status_V,
			Record_Status_A,
			Record_Status_R,
			Record_Status_S,
			Record_Status_B,
			Record_Status_U	
		FROM #Daily_Claim_MasterList_Practice
		ORDER BY 
			Rank, SP_ID, Practice_Display_Seq

	END
	ELSE
	BEGIN
	-- Record Not Exsit

		-- Create No Record Result
		INSERT INTO @WS04 (Seq, Col01)
		VALUES (@seq, 'There is no record in the service date period')

		SET @seq = @seq + 1

		INSERT INTO @WS04 (Seq) VALUES (@seq)
	END

-- ---------------------------------------------
-- For Excel Sheet (05): Remark
-- ---------------------------------------------

	SET @seq = 0

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '(A) Legend', '')

	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '1. Profession Type' , '')    
	
	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)    
	SELECT	@seq,
			rtrim(Service_Category_Code) as Service_Category_Code,
			rtrim(Service_Category_Desc) as Service_Category_Desc
	FROM Profession
	ORDER BY Service_Category_Code
	
	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02) VALUES (@seq, '', '')

	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '(B) Common Note(s) for the report', '')

	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '1. Service Provider:', '')
	
	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '   a. Generate the statistics of the top 300 rankings of service providers for the specified period', '')

	SET @seq = @seq + 1
	
	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '      - For the service provider has the same highest daily transaction on several different days, only the latest day''s statistics for this service provider will be counted and listed', '')

	SET @seq = @seq + 1

	DECLARE @SchemeString AS VARCHAR(1000)
	SET @SchemeString =  STUFF((SELECT '/' + RTRIM(LTRIM(Scheme_Code)) FROM @Scheme FOR XML PATH('')), 1, 1, '')
					
	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '      - For service providers with same no. of most daily ' + @SchemeString + ' claims, all the service providers will be listed and with same ranking', '')

		

	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '2. Claim Transactions:', '')
	
	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', '')

	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated', '')
		
	SET @seq = @seq + 1

	INSERT INTO @Remark (Seq, Col01, Col02)
	VALUES (@seq, '   c. Exclude voided/removed transactions', '')
	
---- =============================================
---- Return results
---- =============================================

-- ---------------------------------------------
-- To Excel Sheet (01): Content
-- ---------------------------------------------

	SELECT 'Report Generation Time: ' + CONVERT(varchar(10), @current_dtm, 111) + ' ' + CONVERT(varchar(5), @current_dtm, 114)
	
-- ---------------------------------------------
-- To Excel Sheet (02): Criteria
-- ---------------------------------------------

	SELECT Col01, RTRIM(Col02) FROM @WS02 ORDER BY Seq

-- ---------------------------------------------
-- To Excel Sheet (03): 01-EHCP
-- ---------------------------------------------

	SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19
	FROM @WS03 
	ORDER BY Seq, SubSeq

-- ---------------------------------------------
-- To Excel Sheet (04): 02-Practice
-- ---------------------------------------------

	SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, 
			Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
			Col21, Col22, Col23, Col24, Col25, Col26
	FROM @WS04 
	ORDER BY Seq , SubSeq

-- ---------------------------------------------
-- To Excel Sheet (05): Remarks
-- ---------------------------------------------

	SELECT Col01, Col02 FROM @Remark ORDER BY Seq, Col01


-- House Keeping
	DROP TABLE #VoucherTransaction
	DROP TABLE #Daily_Claim
	DROP TABLE #Daily_Claim_MasterList_Practice
	DROP TABLE #Daily_Claim_MasterList_SP
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0008_Report_get] TO HCVU
GO
