IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0001_Stat_Schedule_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0001_Stat_Schedule_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- CR No.:			CRE11-014
-- Create date:		23 June 2011
-- Description:		Formulation of Monthly Statement Summary on Smart IC Claims
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0001_Stat_Schedule_get] 
	@request_time DATETIME,
	@From_Date VARCHAR(255),
    @To_Date VARCHAR(255),
    @Scheme_Code VARCHAR(10),
	@User_ID	varchar(255)
AS BEGIN

-- =============================================
-- Report setting
-- =============================================
	DECLARE @From_Dtm	datetime
	DECLARE @To_Dtm	datetime
	DECLARE @Now_Dtm	datetime

	SET @From_Dtm = @From_Date
	SET @To_Dtm = @To_Date
	SET @Now_Dtm = GETDATE()

-- =============================================
-- Constant
-- =============================================
	DECLARE @report_id	VARCHAR(10)
	SET @report_id = 'eHSU0001'

-- =============================================
-- Declaration
-- =============================================
	DECLARE  @ResultRaw TABLE(
		Seq	INTEGER,
		SP_ID VARCHAR(255),
		Practice_ID VARCHAR(255),
		Total INTEGER,
		UsingSmartIC INTEGER,
		NotUsingSmartIC INTEGER
	)

	DECLARE  @Summary TABLE(
		Seq	INTEGER,
		Field1 VARCHAR(500),
		Field2 VARCHAR(500),
		Field3 VARCHAR(500)
	)

	DECLARE  @Result TABLE(
		Seq	INTEGER,
		SP_ID VARCHAR(255),
		Practice_ID VARCHAR(255),
		Total VARCHAR(255),
		UsingSmartIC VARCHAR(255),
		UsingSmartICRate VARCHAR(255),
		NotUsingSmartIC VARCHAR(255),
		NotUsingSmartICRate VARCHAR(255)
	)
	
-- =============================================
-- Initialization 
-- =============================================
-- =============================================
-- Retrieve data
-- =============================================
	INSERT INTO @ResultRaw
	SELECT
		99,
		VT.SP_ID,
		VT.Practice_Display_Seq,
		COUNT(1) AS Total,
		SUM(CASE VT.create_by_smartid WHEN 'Y' THEN 1 ELSE 0 END) AS UsingSmartIC,
		SUM(CASE WHEN (VT.create_by_smartid = 'N' OR VT.create_by_smartid IS NULL) THEN 1 ELSE 0 END) AS NotUsingSmartIC
		--VT.create_by_smartid
	FROM
		(SELECT * FROM VoucherTransaction 
		 WHERE 
			Scheme_Code = @Scheme_Code 
			AND Transaction_Dtm <= @To_Dtm 
			AND Transaction_Dtm > @From_Dtm
			AND (Manual_Reimburse = 'N' OR Manual_Reimburse IS NULL) -- Ignore Outside Payment Claim
			AND Record_Status NOT IN ('D','I') -- Transaction not include Voided & Invalidated
			AND Record_Status NOT IN
				(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @report_id)
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'
				AND ((Effective_Date is null or Effective_Date>= @Now_Dtm) AND (Expiry_Date is null or Expiry_Date < @Now_Dtm)))
			AND (Invalidation IS NULL OR Invalidation NOT In
				(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @report_id)
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
				AND ((Effective_Date is null or Effective_Date>= @Now_Dtm) AND (Expiry_Date is null or Expiry_Date < @Now_Dtm))))
		) VT 
		INNER JOIN transactiondetail td
		ON vt.transaction_id = td.transaction_id  AND vt.scheme_code = @Scheme_Code AND vt.scheme_code = td.scheme_code	
	GROUP BY VT.SP_ID, VT.Practice_Display_Seq

	-- Summary
	INSERT INTO @Summary
	SELECT 0, 'Transaction time From: ', CONVERT(varchar, @From_Dtm, 111) + ' ' + CONVERT(varchar(5), @From_Dtm, 114) , NULL
	INSERT INTO @Summary
	SELECT 1, 'Transaction time To',  CONVERT(varchar, @To_Dtm, 111) + ' ' + CONVERT(varchar(5), @To_Dtm, 114), NULL
	INSERT INTO @Summary
	SELECT 2, 'Scheme', @Scheme_Code, NULL
	
	INSERT INTO @Summary
	SELECT 3, NULL, NULL, NULL
	
	INSERT INTO @Summary
	SELECT 4, 'No. of SP', (SELECT COUNT(DISTINCT SP_ID) FROM @ResultRaw), NULL

	INSERT INTO @Summary
	SELECT 5, 'No. of Practice', (SELECT COUNT(1) FROM (SELECT DISTINCT SP_ID, Practice_ID FROM @ResultRaw) a), NULL

	INSERT INTO @Summary
	SELECT 6, 'No. of Transaction', (SELECT SUM(Total) FROM @ResultRaw), NULL
			

	-- Result
	INSERT INTO @Result
	SELECT 0, 'Transaction time period: ' + CONVERT(varchar, @From_Dtm, 111) + ' ' + CONVERT(varchar(5), @From_Dtm, 114) + ' to ' +
								    + CONVERT(varchar, @To_Dtm, 111) + ' ' + CONVERT(varchar(5), @To_Dtm, 114), NULL, NULL, NULL, NULL, NULL, NULL
	INSERT INTO @Result
	SELECT 1, 'Scheme: ' + @Scheme_Code, NULL, NULL, NULL, NULL, NULL, NULL
	
	INSERT INTO @Result
	SELECT 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL

	INSERT INTO @Result
	SELECT 3, 'SPID', 'PracticeID', 'Total no. of transactions', 'No. of transactions using Smart IC', '%', 'No. of transactions not using Smart IC', '%'

	INSERT INTO @Result
	SELECT Seq,
			SP_ID, 
			Practice_ID, 
			Total, 
			UsingSmartIC, 
			CAST(CAST(UsingSmartIC AS DECIMAL)/CAST(Total AS DECIMAL)*100 AS DECIMAL(10,1)) AS UsingSmartICRate, 
			NotUsingSmartIC, 
			100.0 - CAST(CAST(UsingSmartIC AS DECIMAL)/CAST(Total AS DECIMAL)*100 AS DECIMAL(10,1)) AS NotUsingSmartICRate
	FROM @ResultRaw

	INSERT INTO @Result
	SELECT 100,
		'Grand Total',
		NULL,
		SUM(Total),
		SUM(UsingSmartIC),
		CAST(CAST(SUM(UsingSmartIC) AS DECIMAL)/CAST(SUM(Total) AS DECIMAL)*100 AS DECIMAL(10,1)),
		SUM(NotUsingSmartIC),
		100.0 - CAST(CAST(SUM(UsingSmartIC) AS DECIMAL)/CAST(SUM(Total) AS DECIMAL)*100 AS DECIMAL(10,1))
	FROM @ResultRaw
-- =============================================
-- Return result
-- =============================================

	-- ---------------------------------------------
	-- To Excel sheet: Content
	-- ---------------------------------------------
	SELECT
		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114)

	-- ---------------------------------------------
	-- To Excel sheet: Summary
	-- ---------------------------------------------
	SELECT 
		Field1, 
		Field2, 
		Field3 
	FROM @Summary
	ORDER BY Seq

	-- ---------------------------------------------
	-- To Excel sheet: 01
	-- ---------------------------------------------
	SELECT 
		SP_ID,
		Practice_ID,
		Total,
		NULL, -- Empty Column in report
		UsingSmartIC,
		UsingSmartICRate,
		NotUsingSmartIC,
		NotUsingSmartICRate
	FROM @Result
	ORDER BY Seq, SP_ID, Practice_ID

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0001_Stat_Schedule_get] TO HCVU
GO
