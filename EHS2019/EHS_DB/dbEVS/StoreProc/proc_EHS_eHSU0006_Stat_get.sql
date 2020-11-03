IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0006_Stat_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0006_Stat_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	28 Oct 2020
-- CR No.:			INT20-0042 (Fix eHSU0006 report)
-- Description:		Extend to '100' columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	3 Sep 2017
-- CR No.:			CRE16-026
-- Description:		Add PCV13 
-- =============================================
-- =============================================
-- CR No.:		CRE16-011
-- Author:		Winnie SUEN
-- Create date: 26 Oct 2016
-- Description:	Get Stat for report eHSU0006
-- ============================================= 
--exec proc_EHS_eHSU0006_Stat_get '', '2016-Apr-16 00:00',  '2016-jun-18 00:00'

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0006_Stat_get]  
	@request_time datetime,
	@From_Date varchar(17),
	@To_Date varchar(17)	
AS BEGIN  

SET NOCOUNT ON;  

-- =============================================  
-- Declaration  
-- =============================================  
 
	DECLARE @Report_ID Char(8)  
	DECLARE @Scheme_Code AS VARCHAR(10)

	DECLARE @pivot_table_column_header		varchar(MAX)
	DECLARE @pivot_table_column_list		varchar(MAX)
	DECLARE @pivot_table_column_name_alias	varchar(MAX)
	DECLARE @pivot_table_column_null_to_zero	varchar(MAX)
	DECLARE @pivot_table_column_place_value		varchar(MAX)
	DECLARE @pivot_table_column_category_value	varchar(MAX)
	
	DECLARE @sql_script						varchar(MAX)
	DECLARE @sql_script_pivot_table_insert	varchar(MAX)	
	
	DECLARE @current_scheme_Seq INT
	
	DECLARE @R	INT
	DECLARE @C	INT
	DECLARE @i	INT

-- =============================================          
-- Temporary tables          
-- =============================================          
	CREATE TABLE #Transaction (          
		SP_ID					VARCHAR(10),
		Practice_Display_Seq	SMALLINT,
		Subsidize_Item_Code		VARCHAR(10),
		PlaceVaccination		VARCHAR(10),
		Category_Code			VARCHAR(20),
		Display_Desc			VARCHAR(30),
		TransactionCount		INT
	)        

	CREATE TABLE #SP_List (          
		SP_ID					VARCHAR(10),
		Practice_Display_Seq	SMALLINT,
		Seq						INT
	)  

	DECLARE @Place_Category TABLE (          
		PlaceVaccination		VARCHAR(10),
		Category_Code			VARCHAR(20),
		Display_Desc			VARCHAR(30),
		PlaceVaccination_Desc	VARCHAR(4000),
		Category_Code_Desc		VARCHAR(4000),
		Display_Seq				INT,
		Display_Seq2			INT
	)
	
	DECLARE @SubsidizeCode TABLE (
		Subsidize_Item_Code		VARCHAR(10),
		Display_txt				VARCHAR(50),
		Seq						INT
	)   

	CREATE TABLE #DataTable (
		R			INT,
		C			INT,
		Txt			NVARCHAR(255)
	)
		
	DECLARE @FrameTable TABLE (
		C			INT,
		ColName		VARCHAR(5)
	)

	DECLARE @OutputTable TABLE(
		Result_Value1			NVARCHAR(255), 
		Result_Value2			NVARCHAR(255), 
		Result_Value3			NVARCHAR(255), 
		Result_Value4			NVARCHAR(255), 
		Result_Value5			NVARCHAR(255), 
		Result_Value6			NVARCHAR(255),  
		Result_Value7			NVARCHAR(255), 
		Result_Value8			NVARCHAR(255), 
		Result_Value9			NVARCHAR(255), 
		Result_Value10			NVARCHAR(255),
		Result_Value11			NVARCHAR(255),
		Result_Value12			NVARCHAR(255), 
		Result_Value13			NVARCHAR(255),
		Result_Value14			NVARCHAR(255),
		Result_Value15			NVARCHAR(255),
		Result_Value16			NVARCHAR(255),
		Result_Value17			NVARCHAR(255),
		Result_Value18			NVARCHAR(255), 
		Result_Value19			NVARCHAR(255),
		Result_Value20			NVARCHAR(255),
		Result_Value21			NVARCHAR(255),
		Result_Value22			NVARCHAR(255),
		Result_Value23			NVARCHAR(255),
		Result_Value24			NVARCHAR(255),
		Result_Value25			NVARCHAR(255),
		Result_Value26			NVARCHAR(255),
		Result_Value27			NVARCHAR(255),
		Result_Value28			NVARCHAR(255),
		Result_Value29			NVARCHAR(255),
		Result_Value30			NVARCHAR(255),  
		Result_Value31			NVARCHAR(255),
		Result_Value32			NVARCHAR(255),
		Result_Value33			NVARCHAR(255),
		Result_Value34			NVARCHAR(255),
		Result_Value35			NVARCHAR(255),
		Result_Value36			NVARCHAR(255), 
		Result_Value37			NVARCHAR(255),
		Result_Value38			NVARCHAR(255),
		Result_Value39			NVARCHAR(255),
		Result_Value40			NVARCHAR(255),
		Result_Value41			NVARCHAR(255),
		Result_Value42			NVARCHAR(255), 
		Result_Value43			NVARCHAR(255),
		Result_Value44			NVARCHAR(255),
		Result_Value45			NVARCHAR(255),
		Result_Value46			NVARCHAR(255),
		Result_Value47			NVARCHAR(255),
		Result_Value48			NVARCHAR(255),
		Result_Value49			NVARCHAR(255),
		Result_Value50			NVARCHAR(255),
		Result_Value51			NVARCHAR(255),
		Result_Value52			NVARCHAR(255), 
		Result_Value53			NVARCHAR(255),
		Result_Value54			NVARCHAR(255),
		Result_Value55			NVARCHAR(255),
		Result_Value56			NVARCHAR(255),
		Result_Value57			NVARCHAR(255),
		Result_Value58			NVARCHAR(255),
		Result_Value59			NVARCHAR(255),
		Result_Value60			NVARCHAR(255),
		Result_Value61			NVARCHAR(255),
		Result_Value62			NVARCHAR(255), 
		Result_Value63			NVARCHAR(255),
		Result_Value64			NVARCHAR(255),
		Result_Value65			NVARCHAR(255),
		Result_Value66			NVARCHAR(255),
		Result_Value67			NVARCHAR(255),
		Result_Value68			NVARCHAR(255),
		Result_Value69			NVARCHAR(255),
		Result_Value70			NVARCHAR(255),
		Result_Value71			NVARCHAR(255),
		Result_Value72			NVARCHAR(255), 
		Result_Value73			NVARCHAR(255),
		Result_Value74			NVARCHAR(255),
		Result_Value75			NVARCHAR(255),
		Result_Value76			NVARCHAR(255),
		Result_Value77			NVARCHAR(255),
		Result_Value78			NVARCHAR(255),
		Result_Value79			NVARCHAR(255),
		Result_Value80			NVARCHAR(255),
		Result_Value81			NVARCHAR(255),
		Result_Value82			NVARCHAR(255), 
		Result_Value83			NVARCHAR(255),
		Result_Value84			NVARCHAR(255),
		Result_Value85			NVARCHAR(255),
		Result_Value86			NVARCHAR(255),
		Result_Value87			NVARCHAR(255),
		Result_Value88			NVARCHAR(255),
		Result_Value89			NVARCHAR(255),
		Result_Value90			NVARCHAR(255),
		Result_Value91			NVARCHAR(255),
		Result_Value92			NVARCHAR(255), 
		Result_Value93			NVARCHAR(255),
		Result_Value94			NVARCHAR(255),
		Result_Value95			NVARCHAR(255),
		Result_Value96			NVARCHAR(255),
		Result_Value97			NVARCHAR(255),
		Result_Value98			NVARCHAR(255),
		Result_Value99			NVARCHAR(255),
		Result_Value100			NVARCHAR(255)
	)
-- =============================================    
-- Initialization    
-- =============================================
	SET @Report_ID = 'eHSU0006'
	SET @Scheme_Code = 'VSS'   

	EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @To_Date 

	DELETE #DataTable
	DELETE @FrameTable

	SET @i = 1

	WHILE @i <= 100 BEGIN
		INSERT INTO @FrameTable (C, ColName) VALUES (@i, 'C' + CONVERT(VARCHAR, @i))
		SET @i = @i + 1
	END   
-- =============================================  
-- Report Setting  
-- =============================================  
  
	IF @request_time IS NULL BEGIN    
	  SET @request_time = DATEADD(dd, -1, CONVERT(VARCHAR(11), GETDATE(), 106)) -- "106" gives "dd MMM yyyy"    
	END    
	    
	DECLARE @reporting_period varchar(50)      
	DECLARE @reporting_date varchar(50)   
	DECLARE @reporting_dtm datetime  
	DECLARE @From_Dtm datetime
	DECLARE @To_Dtm datetime

	SELECT @From_Dtm = CAST(CONVERT(VARCHAR(11), @From_Date, 111) + ' 00:00:00' as datetime) 
	SELECT @To_Dtm = CAST(CONVERT(VARCHAR(11), @To_Date, 111) + ' 00:00:00' as datetime) 

	SET @reporting_date =  CONVERT(VARCHAR(10), @From_Dtm, 111) + ' to ' + CONVERT(VARCHAR(10), @To_Dtm, 111)
	SET @reporting_period = 'Reporting period: ' + @reporting_date
	 
 -- =============================================  
-- Build frame  
-- =============================================  
  
	---- Generate static layout ----      
	---- Content    
	SELECT 'Report Generation Time: ' + CONVERT(VARCHAR(10), getdate(), 111) + ' ' + CONVERT(VARCHAR(5), getdate(), 114)
	  
	--Criteria  
	-- Insert workbook data
	SELECT 'Transaction Date', @reporting_date
	  
	--01 sub Report  
	INSERT INTO #DataTable (R, C, Txt)	SELECT 1, 1, @reporting_period
	INSERT INTO #DataTable (R, C, Txt)	SELECT 2, 1, ''

-- =============================================  
-- Prepare Data 
-- =============================================  
	INSERT INTO @SubsidizeCode ( Subsidize_Item_Code, Display_txt, Seq  )
	SELECT DISTINCT Subsidize_Item_Code, 
			CASE Subsidize_Item_Code 
				WHEN 'SIV'	THEN 'SI'
				WHEN 'PV'	THEN '23vPPV'
				WHEN 'PV13' THEN 'PCV13'
			END,
			CASE Subsidize_Item_Code 
				WHEN 'SIV'	THEN 1
				WHEN 'PV'	THEN 2
				WHEN 'PV13' THEN 3
			END
	FROM SubsidizeGroupClaim SGC
	INNER JOIN Subsidize	S
		ON SGC.Subsidize_Code = S.Subsidize_Code	
	WHERE 
		SGC.Scheme_Code = 'VSS'		
		AND(
			(S.Subsidize_Item_Code = 'SIV' AND SGC.Scheme_Seq = @current_scheme_Seq)
			OR S.Subsidize_Item_Code = 'PV' 
			OR S.Subsidize_Item_Code = 'PV13' 
		)
		
-- ---------------------------------------------          
-- Transactions          
-- ---------------------------------------------                
          
	SELECT          
		VT.SP_ID,          
		VT.Practice_Display_Seq,
		TD.Subsidize_Item_Code,
		TAF.AdditionalFieldValueCode AS PlaceVaccination,
		VT.Category_Code,
		--COUNT(*) As TransactionCount
		VT.Transaction_ID
	INTO #Temp_Transaction
	FROM          
		VoucherTransaction VT 
	INNER JOIN TransactionAdditionalField TAF
			ON VT.Transaction_ID = TAF.Transaction_ID  AND TAF.AdditionalFieldID = 'PlaceVaccination'
	INNER JOIN TransactionDetail TD
			ON VT.Transaction_ID = TD.Transaction_ID
			AND VT.Scheme_Code = TD.Scheme_Code
	WHERE          
		VT.Scheme_Code = @Scheme_Code 
	AND VT.Transaction_Dtm >= @From_Dtm 
	AND VT.Transaction_Dtm < dateadd(dd, 1, @To_Dtm)
    AND VT.Record_Status NOT IN
		(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
			AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm)))
	AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In     
		(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
			AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm))))
	AND TD.Subsidize_Item_Code IN ( SELECT Subsidize_Item_Code FROM @SubsidizeCode )    
	--GROUP BY
	--	VT.SP_ID, 
	--	VT.Practice_Display_Seq, 
	--	TD.Subsidize_Item_Code, 
	--	TAF.AdditionalFieldValueCode, 
	--	VT.Category_Code
	ORDER BY   
		VT.SP_ID, 
		VT.Practice_Display_Seq
		
	--==========================================
	-- Add Count  
	-- Row: SP + DisplaySeq, Subsidize code
	-- Col: Place + Category Grand Total
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		SP_ID,
		Practice_Display_Seq,
		Subsidize_Item_Code,
		'',
		'GRANDTOTAL',
		'PLACE_CAT-GRANDTOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY 
		SP_ID, 
		Practice_Display_Seq, 
		Subsidize_Item_Code
		
	-- Row: SP + DisplaySeq, Subsidize code
	-- Col: Place + Category
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT 
		SP_ID,          
		Practice_Display_Seq,
		Subsidize_Item_Code,
		PlaceVaccination,
		Category_Code,
		LTRIM(RTRIM(PlaceVaccination)) + '-' + LTRIM(RTRIM(Category_Code)),
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM #Temp_Transaction
	GROUP BY
		SP_ID, 
		Practice_Display_Seq, 
		Subsidize_Item_Code, 
		PlaceVaccination, 
		Category_Code
	
	-- Row: SP + DisplaySeq, Subsidize code
	-- Col: Place TOTAL
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		SP_ID,
		Practice_Display_Seq,
		Subsidize_Item_Code,
		PlaceVaccination,
		'TOTAL',
		PlaceVaccination + '-TOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY 
		SP_ID, 
		Practice_Display_Seq, 
		Subsidize_Item_Code, 
		PlaceVaccination	
	-------------------------------------------------------
	-- Add Total  
	-- Row: SP + DisplaySeq
	-- Col: Place + Category Grand Total
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		SP_ID,
		Practice_Display_Seq,
		'TOTAL',
		'',
		'GRANDTOTAL',
		'PLACE_CAT-GRANDTOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY 
		SP_ID, 
		Practice_Display_Seq
		
	-- Row: SP + DisplaySeq, 
	-- Col: Place + Category
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT 
		SP_ID,          
		Practice_Display_Seq,
		'TOTAL',
		PlaceVaccination,
		Category_Code,
		LTRIM(RTRIM(PlaceVaccination)) + '-' + LTRIM(RTRIM(Category_Code)),
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM #Temp_Transaction
	GROUP BY
		SP_ID, 
		Practice_Display_Seq,  
		PlaceVaccination, 
		Category_Code
	
	-- Row: SP + DisplaySeq
	-- Col: Place TOTAL
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		SP_ID,
		Practice_Display_Seq,
		'TOTAL',
		PlaceVaccination,
		'TOTAL',
		PlaceVaccination + '-TOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY 
		SP_ID, 
		Practice_Display_Seq, 
		PlaceVaccination	

	--===================================================	
	-- Grand Total
	-- Row: GRAND TOTAL, Subsidize code
	-- Col: Place Grand Total
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		'GRANDTOTAL',
		NULL,
		Subsidize_Item_Code,
		'',
		'GRANDTOTAL',
		'PLACE_CAT-GRANDTOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY 
		Subsidize_Item_Code
		
	-- Row: GRANDTOTAL, Subsidize code
	-- Col: Place + Category
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT 
		'GRANDTOTAL',
		NULL,
		Subsidize_Item_Code,
		PlaceVaccination,
		Category_Code,
		LTRIM(RTRIM(PlaceVaccination)) + '-' + LTRIM(RTRIM(Category_Code)),
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM #Temp_Transaction
	GROUP BY
		Subsidize_Item_Code, 
		PlaceVaccination, 
		Category_Code
	
	-- Row: GRANDTOTAL, Subsidize code
	-- Col: Place TOTAL
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		'GRANDTOTAL',
		NULL,
		Subsidize_Item_Code,
		PlaceVaccination,
		'TOTAL',
		PlaceVaccination + '-TOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY 
		Subsidize_Item_Code, 
		PlaceVaccination	
	-------------------------------------------------------
	-- Add Total  
	-- Row: GRANDTOTAL
	-- Col: Place Grand Total
	IF (SELECT COUNT(Transaction_ID) FROM #Temp_Transaction) >0 
	BEGIN
		INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
		SELECT
			'GRANDTOTAL',
			NULL,
			'TOTAL',
			'',
			'GRANDTOTAL',
			'PLACE_CAT-GRANDTOTAL',
			--SUM(TransactionCount)
			COUNT(DISTINCT Transaction_ID)
		FROM
			#Temp_Transaction
	END
		
	-- Row: GRANDTOTAL, 
	-- Col: Place + Category
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT 
		'GRANDTOTAL',
		NULL,
		'TOTAL',
		PlaceVaccination,
		Category_Code,
		LTRIM(RTRIM(PlaceVaccination)) + '-' + LTRIM(RTRIM(Category_Code)),
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM #Temp_Transaction
	GROUP BY  
		PlaceVaccination, 
		Category_Code
	
	-- Row: GRANDTOTAL
	-- Col: Place TOTAL
	INSERT INTO #Transaction (SP_ID, Practice_Display_Seq, Subsidize_Item_Code, PlaceVaccination, Category_Code, Display_Desc, TransactionCount	)  
	SELECT
		'GRANDTOTAL',
		NULL,
		'TOTAL',
		PlaceVaccination,
		'TOTAL',
		PlaceVaccination + '-TOTAL',
		--SUM(TransactionCount)
		COUNT(DISTINCT Transaction_ID)
	FROM
		#Temp_Transaction
	GROUP BY  
		PlaceVaccination	

	--=========================================
	-- Get Place_Category Header
	INSERT INTO @Place_Category (PlaceVaccination, Category_Code, Display_Desc, PlaceVaccination_Desc, Category_Code_Desc, Display_Seq, Display_Seq2)
	SELECT
			T.PlaceVaccination, 
			T.Category_Code, 
			T.Display_Desc,
			SD.Data_Value,
			CC.Category_Name,
			ROW_NUMBER () OVER (ORDER BY SD.Display_Order, ISNULL(CC.Display_Seq,99)),
			ROW_NUMBER () OVER (PARTITION BY T.PlaceVaccination ORDER BY SD.Display_Order, ISNULL(CC.Display_Seq,99))
	FROM
		(SELECT	DISTINCT
			PlaceVaccination, 
			Category_Code, 
			Display_Desc
			FROM
			#Transaction) T
	INNER JOIN
		StaticData SD ON SD.Column_Name = 'VSS_PLACEOFVACCINATION' AND T.PlaceVaccination = SD.Item_No
	LEFT JOIN
		ClaimCategory CC ON T.Category_Code = CC.Category_Code

	-- ==================================================
	INSERT INTO #SP_List (SP_ID, Practice_Display_Seq, Seq)
	SELECT SP_ID, 
		Practice_Display_Seq, 
		ROW_NUMBER() OVER (ORDER BY SP_ID, Practice_Display_Seq)
	FROM (
		SELECT DISTINCT SP_ID, Practice_Display_Seq
		FROM #Transaction 
		WHERE SP_ID <> 'GRANDTOTAL'
	)T 
	--ensure GrandTotal should be the last one in SP ID list
	INSERT INTO #SP_List (SP_ID, Practice_Display_Seq, Seq)
	SELECT 'GRANDTOTAL', NULL, 999

	INSERT INTO @SubsidizeCode (Subsidize_Item_Code, Display_txt, Seq)
	SELECT 'TOTAL', 'Total no. of transactions*', 4


	-- ==================================================
	IF EXISTS(SELECT 1 FROM #Transaction)
	BEGIN
		-- Header	
		INSERT INTO #DataTable (R, C, Txt) SELECT 4, 1, 'SPID (Practice No.)' 
		INSERT INTO #DataTable (R, C, Txt) SELECT 4, 3, 'GRAND TOTAL'	

		SET @C = 4
		DECLARE Cursor1 CURSOR FOR 
		SELECT Display_Desc, Display_Seq2
		FROM @Place_Category
		ORDER BY Display_Seq,  Display_Seq2 

		DECLARE @Cursor1_Display_Desc	VARCHAR(30)
		DECLARE @Cursor1_Display_Seq	INT
		OPEN Cursor1
		FETCH NEXT FROM Cursor1 INTO @Cursor1_Display_Desc, @Cursor1_Display_Seq
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			IF @Cursor1_Display_Seq = 1
			BEGIN
				-- Add Place Header
				INSERT INTO #DataTable (R, C, Txt) 
				SELECT 3, @C, PlaceVaccination_Desc 
				FROM @Place_Category 
				WHERE Display_Desc = @Cursor1_Display_Desc
			END			
		
			-- Add Category Header
			INSERT INTO #DataTable (R, C, Txt) 
			SELECT 4, @C, ISNULL(Category_Code_Desc, 'TOTAL') 
			FROM @Place_Category 
			WHERE Display_Desc = @Cursor1_Display_Desc

		
			SET @C = @C +1
			FETCH NEXT FROM Cursor1 INTO @Cursor1_Display_Desc, @Cursor1_Display_Seq
		END

		CLOSE Cursor1
		DEALLOCATE Cursor1
		--

		--Content	
		SET @R = 5	
		DECLARE Cursor2 CURSOR FOR 
		SELECT SP_ID, 
			Practice_Display_Seq,
			Seq
		FROM #SP_List
		ORDER BY SP_ID, 
			Practice_Display_Seq
	
		DECLARE @Cursor2_SP_ID					VARCHAR(10)
		DECLARE @Cursor2_Practice_Display_Seq	SMALLINT
		DECLARE @Cursor2_Seq					INT

		OPEN Cursor2
		FETCH NEXT FROM Cursor2 INTO 
			@Cursor2_SP_ID,					
			@Cursor2_Practice_Display_Seq,
			@Cursor2_Seq	
		WHILE @@FETCH_STATUS = 0 
		BEGIN	
			--Insert SPID
			IF @Cursor2_SP_ID = 'GRANDTOTAL'
			BEGIN
				INSERT INTO #DataTable (R, C, Txt) SELECT @R, 1, 'GRAND TOTAL'
			END
			ELSE
			BEGIN
				INSERT INTO #DataTable (R, C, Txt) SELECT @R, 1, @Cursor2_SP_ID+'('+ CONVERT(VARCHAR(5), @Cursor2_Practice_Display_Seq)+')' 
			END
			--
				DECLARE Cursor3 CURSOR FOR 
				SELECT Subsidize_Item_Code,	Display_txt
				FROM @SubsidizeCode
				ORDER BY Seq

				DECLARE @Cursor3_Subsidize_Item_Code	VARCHAR(10)
				DECLARE @Cursor3_Display_txt			VARCHAR(50)
				OPEN Cursor3
				FETCH NEXT FROM Cursor3 INTO @Cursor3_Subsidize_Item_Code, @Cursor3_Display_txt
				WHILE @@FETCH_STATUS = 0 
				BEGIN
					-- Insert Subsidize Code		
					INSERT INTO #DataTable (R, C, Txt) SELECT @R, 2, @Cursor3_Display_txt 
				
					-- Insert GrandTotal Column value				
					DECLARE @GrandTotalValue INT
					SET @GrandTotalValue = 0 
					SELECT @GrandTotalValue = TransactionCount
					FROM #Transaction
					WHERE Category_Code='GRANDTOTAL'
						AND SP_ID = @Cursor2_SP_ID
						AND ISNULL(Practice_Display_Seq, '') = ISNULL(@Cursor2_Practice_Display_Seq, '')
						AND Subsidize_Item_Code = @Cursor3_Subsidize_Item_Code 
					INSERT INTO #DataTable (R, C, Txt) SELECT @R, 3, @GrandTotalValue

					--				
					SET @C = 4
					DECLARE Cursor4 CURSOR FOR 
					SELECT PlaceVaccination, Category_Code
					FROM @Place_Category
					ORDER BY Display_Seq,  Display_Seq2 

					DECLARE @Cursor4_PlaceVaccination	VARCHAR(10)
					DECLARE @Cursor4_Category_Code	VARCHAR(10)
					OPEN Cursor4
					FETCH NEXT FROM Cursor4 INTO @Cursor4_PlaceVaccination, @Cursor4_Category_Code
					WHILE @@FETCH_STATUS = 0 
					BEGIN
						DECLARE @Value	INT
						SET @Value  = 0
						SELECT @Value = TransactionCount 
						FROM #Transaction
						WHERE Category_Code= @Cursor4_Category_Code
							AND PlaceVaccination = @Cursor4_PlaceVaccination
							AND SP_ID = @Cursor2_SP_ID
							AND ISNULL(Practice_Display_Seq, '') = ISNULL(@Cursor2_Practice_Display_Seq, '')
							AND Subsidize_Item_Code = @Cursor3_Subsidize_Item_Code
												
						-- Insert content Column value				
						INSERT INTO #DataTable (R, C, Txt) SELECT @R, @C, @Value
					
						SET @C = @C + 1

						FETCH NEXT FROM Cursor4 INTO @Cursor4_PlaceVaccination, @Cursor4_Category_Code
					END

					CLOSE Cursor4
					DEALLOCATE Cursor4
					--
					SET @C = 2
					SET @R = @R + 1
					FETCH NEXT FROM Cursor3 INTO @Cursor3_Subsidize_Item_Code, @Cursor3_Display_txt
				END

				CLOSE Cursor3
				DEALLOCATE Cursor3
			--		
			FETCH NEXT FROM Cursor2 INTO 
				@Cursor2_SP_ID,					
				@Cursor2_Practice_Display_Seq,
				@Cursor2_Seq	
		END

		CLOSE Cursor2
		DEALLOCATE Cursor2
		
		INSERT INTO #DataTable (R, C, Txt) SELECT @R+1, 1, '' 	
		INSERT INTO #DataTable (R, C, Txt) SELECT @R+2, 1, '* The total no. of transactions should be equal or smaller than the total number of SI, 23vPPV and PCV13 claim units' 
	
	END
	ELSE
	BEGIN		
		INSERT INTO #DataTable (R, C, Txt) SELECT 3, 1, 'There is no record in the reporting period' 
	END

	-- ====================================================
	INSERT INTO @OutputTable (
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10, 
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, 
		Result_Value21, Result_Value22, Result_Value23, Result_Value24,	Result_Value25, Result_Value26, Result_Value27, Result_Value28, Result_Value29, Result_Value30,  
		Result_Value31, Result_Value32, Result_Value33, Result_Value34, Result_Value35, Result_Value36, Result_Value37, Result_Value38, Result_Value39, Result_Value40, 
		Result_Value41, Result_Value42, Result_Value43, Result_Value44, Result_Value45, Result_Value46, Result_Value47, Result_Value48,	Result_Value49, Result_Value50,
		Result_Value51, Result_Value52, Result_Value53, Result_Value54, Result_Value55, Result_Value56, Result_Value57, Result_Value58,	Result_Value59, Result_Value60,
		Result_Value61, Result_Value62, Result_Value63, Result_Value64, Result_Value65, Result_Value66, Result_Value67, Result_Value68,	Result_Value69, Result_Value70,
		Result_Value71, Result_Value72, Result_Value73, Result_Value74, Result_Value75, Result_Value76, Result_Value77, Result_Value78,	Result_Value79, Result_Value80,
		Result_Value81, Result_Value82, Result_Value83, Result_Value84, Result_Value85, Result_Value86, Result_Value87, Result_Value88,	Result_Value89, Result_Value90,
		Result_Value91, Result_Value92, Result_Value93, Result_Value94, Result_Value95, Result_Value96, Result_Value97, Result_Value98,	Result_Value99, Result_Value100
	)
	SELECT
		ISNULL(C1, ''),  ISNULL(C2, ''),  ISNULL(C3, ''),  ISNULL(C4, ''),  ISNULL(C5, ''),  ISNULL(C6, ''),  ISNULL(C7, ''),  ISNULL(C8, ''),  ISNULL(C9, ''),  ISNULL(C10, ''), 
		ISNULL(C11, ''), ISNULL(C12, ''), ISNULL(C13, ''), ISNULL(C14, ''), ISNULL(C15, ''), ISNULL(C16, ''), ISNULL(C17, ''), ISNULL(C18, ''), ISNULL(C19, ''), ISNULL(C20, ''), 
		ISNULL(C21, ''), ISNULL(C22, ''), ISNULL(C23, ''), ISNULL(C24, ''),	ISNULL(C25, ''), ISNULL(C26, ''), ISNULL(C27, ''), ISNULL(C28, ''), ISNULL(C29, ''), ISNULL(C30, ''),  
		ISNULL(C31, ''), ISNULL(C32, ''), ISNULL(C33, ''), ISNULL(C34, ''), ISNULL(C35, ''), ISNULL(C36, ''), ISNULL(C37, ''), ISNULL(C38, ''), ISNULL(C39, ''), ISNULL(C40, ''), 
		ISNULL(C41, ''), ISNULL(C42, ''), ISNULL(C43, ''), ISNULL(C44, ''), ISNULL(C45, ''), ISNULL(C46, ''), ISNULL(C47, ''), ISNULL(C48, ''),	ISNULL(C49, ''), ISNULL(C50, ''),
		ISNULL(C51, ''), ISNULL(C52, ''), ISNULL(C53, ''), ISNULL(C54, ''), ISNULL(C55, ''), ISNULL(C56, ''), ISNULL(C57, ''), ISNULL(C58, ''),	ISNULL(C59, ''), ISNULL(C60, ''),
		ISNULL(C61, ''), ISNULL(C62, ''), ISNULL(C63, ''), ISNULL(C64, ''), ISNULL(C65, ''), ISNULL(C66, ''), ISNULL(C67, ''), ISNULL(C68, ''),	ISNULL(C69, ''), ISNULL(C70, ''),
		ISNULL(C71, ''), ISNULL(C72, ''), ISNULL(C73, ''), ISNULL(C74, ''), ISNULL(C75, ''), ISNULL(C76, ''), ISNULL(C77, ''), ISNULL(C78, ''),	ISNULL(C79, ''), ISNULL(C80, ''),
		ISNULL(C81, ''), ISNULL(C82, ''), ISNULL(C83, ''), ISNULL(C84, ''), ISNULL(C85, ''), ISNULL(C86, ''), ISNULL(C87, ''), ISNULL(C88, ''),	ISNULL(C89, ''), ISNULL(C90, ''),
		ISNULL(C91, ''), ISNULL(C92, ''), ISNULL(C93, ''), ISNULL(C94, ''), ISNULL(C95, ''), ISNULL(C96, ''), ISNULL(C97, ''), ISNULL(C98, ''),	ISNULL(C99, ''), ISNULL(C100, '')
	FROM (
		SELECT
			D.R,
			F.ColName,
			D.Txt
		FROM
			@FrameTable F
		INNER JOIN #DataTable D
			ON F.C = D.C
		) P 
	PIVOT (
		MAX(Txt)
		FOR ColName IN (C1,  C2,  C3,  C4,	C5,	 C6,  C7,  C8,  C9,  C10, 
						C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, 
						C21, C22, C23, C24,	C25, C26, C27, C28, C29, C30,  
						C31, C32, C33, C34, C35, C36, C37, C38, C39, C40, 
						C41, C42, C43, C44, C45, C46, C47, C48,	C49, C50,
						C51, C52, C53, C54, C55, C56, C57, C58,	C59, C60,
						C61, C62, C63, C64, C65, C66, C67, C68,	C69, C70,
						C71, C72, C73, C74, C75, C76, C77, C78,	C79, C80,
						C81, C82, C83, C84, C85, C86, C87, C88,	C89, C90,
						C91, C92, C93, C94, C95, C96, C97, C98,	C99, C100
					)
	) AS PVT
	ORDER BY
	R
		

	SELECT
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10, 
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, 
		Result_Value21, Result_Value22, Result_Value23, Result_Value24,	Result_Value25, Result_Value26, Result_Value27, Result_Value28, Result_Value29, Result_Value30,  
		Result_Value31, Result_Value32, Result_Value33, Result_Value34, Result_Value35, Result_Value36, Result_Value37, Result_Value38, Result_Value39, Result_Value40, 
		Result_Value41, Result_Value42, Result_Value43, Result_Value44, Result_Value45, Result_Value46, Result_Value47, Result_Value48,	Result_Value49, Result_Value50,
		Result_Value51, Result_Value52, Result_Value53, Result_Value54, Result_Value55, Result_Value56, Result_Value57, Result_Value58,	Result_Value59, Result_Value60,
		Result_Value61, Result_Value62, Result_Value63, Result_Value64, Result_Value65, Result_Value66, Result_Value67, Result_Value68,	Result_Value69, Result_Value70,
		Result_Value71, Result_Value72, Result_Value73, Result_Value74, Result_Value75, Result_Value76, Result_Value77, Result_Value78,	Result_Value79, Result_Value80,
		Result_Value81, Result_Value82, Result_Value83, Result_Value84, Result_Value85, Result_Value86, Result_Value87, Result_Value88,	Result_Value89, Result_Value90,
		Result_Value91, Result_Value92, Result_Value93, Result_Value94, Result_Value95, Result_Value96, Result_Value97, Result_Value98,	Result_Value99, Result_Value100
	FROM 
		@OutputTable 

	-- Remark
	SELECT '(A) Common Note(s) for the report' 
	UNION ALL
	SELECT '1. Transactions:'
	UNION ALL
	SELECT'   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))'
	UNION ALL
	SELECT '   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated' 
	UNION ALL
	SELECT '   c. Exclude voided/deleted transactions' 


	DROP TABLE #SP_List
	DROP TABLE #Temp_Transaction
	DROP TABLE #Transaction
	DROP TABLE #DataTable
	
END     
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0006_Stat_get] TO HCVU
GO
