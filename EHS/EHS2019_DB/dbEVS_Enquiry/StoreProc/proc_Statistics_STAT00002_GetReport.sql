IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00002_GetReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00002_GetReport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Dickson Law
-- Modified date:   27 Dec 2017
-- Description:		Separate statistics data for Alive and Deceased
-- =============================================
-- =============================================
-- Author:			Koala Cheng
-- Create date:		03 Dec 2012
-- Description:		Get Report for Statistics - STAT00002
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00002_GetReport] 
	@cut_off_date	datetime,
	@age_from		int,
	@age_to			int
AS BEGIN

-- =============================================
-- Declaration
-- =============================================
	DECLARE @separator_month int    
	DECLARE @grand_total_text varchar(15)  
	DECLARE @grand_total	int 
	DECLARE @alive_total	int
	DECLARE @deceased_total	int
	
	DECLARE @AccountALL table (
		Voucher_Acc_ID	varchar(20),
		DOB				DATETIME,
		DOD				DATETIME,
		Deceased		CHAR(1)
	)

-- =============================================
-- Validation
-- =============================================

	IF @cut_off_date IS NULL
		RETURN

	IF @age_from < 0 OR @age_to < 0
		RETURN

	IF @age_from > @age_to AND (NOT(@age_from IS NULL)) AND (NOT(@age_to IS NULL))
		RETURN

-- =============================================
-- Initialization
-- =============================================

	SET @grand_total_text = 'Total'
	SET @separator_month = 5

-- =============================================
-- Retrieve Data
-- =============================================

	-- =====================================================
	-- Get all account with DOB, DOD and Deceased status
	-- =====================================================
	INSERT INTO @AccountALL (
		Voucher_Acc_ID,	
		DOB,				
		DOD,				
		Deceased
	)
	SELECT				
		PINFO.Voucher_Acc_ID,	
		CASE WHEN PINFO.Exact_DOB IN ('M', 'U')
			THEN DATEADD(mm, DATEDIFF(mm, 0, DATEADD(mm, 1, MIN(PINFO.DOB))), -1)
			ELSE MIN(PINFO.DOB)
		END AS DOB,	
		CASE WHEN PINFO.Exact_DOD ='M'
				THEN DATEADD(mm, DATEDIFF(mm, 0, DATEADD(mm, 1, MIN(PINFO.DOD))), -1)
			WHEN PINFO.Exact_DOD ='Y' 
				THEN  DATEADD(yy, DATEDIFF(yy, 0, DATEADD(yy, 1, MIN(PINFO.DOD))), -1)
			ELSE
				MIN(PINFO.DOD) 
		END AS DOD,
		CASE WHEN PINFO.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN PINFO.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END 
	FROM  
		VoucherAccount VA    
	INNER JOIN 
		PersonalInformation PINFO    
			ON VA.Voucher_Acc_ID = PINFO.Voucher_Acc_ID    
	WHERE (VA.Record_Status = 'A' OR VA.Record_Status = 'S')    
		AND VA.Effective_Dtm < DATEADD(dd, 1, @cut_off_date)  
	GROUP BY 
		PINFO.Voucher_Acc_ID,PINFO.Deceased,PINFO.Exact_DOD,PINFO.Exact_DOB

	-- =====================================================
	-- Get Age Count and Insert into temp table
	-- =====================================================
	SELECT 
		AgeName, 
		COUNT(Age) AS Total,
		ISNULL(SUM(CASE WHEN Deceased= 'N' THEN 1 ELSE 0 END),0) AS Alive,
		ISNULL(SUM(CASE WHEN Deceased= 'Y' THEN 1 ELSE 0 END),0) AS Deceased, 
		Unit, 
		Age 
	INTO 
		#temp_dob 
	FROM (
		SELECT 
			CASE   
				WHEN Y > 1 THEN CONVERT(VARCHAR(10),Y) + ' Years'  
				WHEN Y = 1 THEN CONVERT(VARCHAR(10),Y) + ' Year'  
				WHEN Y = 0 THEN   
				CASE  
					WHEN M <= 5 THEN '0 to 5 Months'  
					WHEN M > 5 THEN CONVERT(VARCHAR(10),M) + ' Months'  
				END  
				WHEN Y < 0 THEN 'E'  
			END AS AgeName,  
			CASE   
				WHEN Y > 0 THEN 'Y'  
				WHEN Y = 0 THEN 'M'  
				WHEN Y < 0 THEN 'E'  
			END AS Unit,  
			CASE   
				WHEN Y > 0 THEN Y  
				WHEN Y = 0 THEN M  
				WHEN Y < 0 THEN -1  
			END AS Age,
			Deceased 
		FROM (  
			SELECT 
				CASE WHEN Deceased = 'N'
					THEN DATEDIFF(yy, DOB, @cut_off_date) - 
						CASE WHEN (MONTH(DOB) > MONTH(@cut_off_date)) 
							OR (MONTH(DOB) = MONTH(@cut_off_date) AND DAY(DOB) > DAY(@cut_off_date)) 
						THEN 1 
						ELSE 0 
					END 
					ELSE
						DATEDIFF(yy, DOB, DOD) - 
						CASE WHEN (MONTH(DOB) > MONTH(DOD)) 
							OR (MONTH(DOB) = MONTH(DOD) AND DAY(DOB) > DAY(DOD)) 
						THEN 1 
						ELSE 0 
					END 
				END AS Y, 
				CASE WHEN Deceased = 'N'
					THEN
						DATEDIFF(m, DATEADD(yy, DATEDIFF(yy, DOB, @cut_off_date) - 
						CASE WHEN (MONTH(DOB) > MONTH(@cut_off_date)) 
							OR (MONTH(DOB) = MONTH(@cut_off_date) AND DAY(DOB) > DAY(@cut_off_date)) 
							THEN 1 
							ELSE 0 
							END, DOB) , @cut_off_date) - 
						CASE WHEN 
							DAY(DOB) > DAY(@cut_off_date) 
							THEN 1 
							ELSE 0 
						END 
					ELSE
						DATEDIFF(m, DATEADD(yy, DATEDIFF(yy, DOB, DOD) - 
						CASE WHEN (MONTH(DOB) > MONTH(DOD)) 
							OR (MONTH(DOB) = MONTH(DOD) AND DAY(DOB) > DAY(DOD)) 
							THEN 1 
							ELSE 0 
							END, DOB) , DOD) - 
						CASE WHEN 
							DAY(DOB) > DAY(DOD) 
							THEN 1 
							ELSE 0 
						END 
				END AS M,
				Deceased  
			FROM 
				@AccountALL T0 
		)T1
	)T2
	GROUP BY 
		AgeName, Unit, Age  
	ORDER BY 
		Unit, Age 

	-- =====================================================
	-- Get Max Age
	-- =====================================================
	IF @age_to IS NULL
	BEGIN
		SELECT @age_to = MAX(ISNULL(Age,0)) FROM #temp_dob WHERE Unit = 'Y'
	END

	-- =====================================================
	-- Get Result By Age Filter
	-- =====================================================
	SELECT 
		N.Age AS Age, 
		ISNULL(A.Total, 0) AS Alive_deceased, 
		ISNULL(A.Alive, 0) AS Alive, 
		ISNULL(A.Deceased, 0) AS Deceased, 
		N.Seq  
	INTO 
		#temp_result  
	FROM (
		SELECT 
			* 
		FROM 
			dbo.func_get_age_range_for_statistics(@age_from, @age_to, @separator_month)
		) N  
	LEFT JOIN (
		SELECT * FROM #temp_dob
	) A  
	ON 
		N.Age = A.AgeName  
	WHERE (   
		(@age_from IS NOT NULL AND N.Years >= @age_from)   
	AND  
		(@age_to IS NOT NULL AND N.Years <= @age_to)  
	)  

	DROP TABLE #temp_dob  
	 
	SELECT @grand_total = ISNULL(SUM(Alive_deceased), 0) FROM #temp_result  
	SELECT @alive_total = ISNULL(SUM(Alive), 0) FROM #temp_result 
	SELECT @deceased_total = ISNULL(SUM(Deceased), 0) FROM #temp_result 
	
	INSERT INTO 
		#temp_result(
			[Age],
			Alive_deceased,
			Alive, 
			Deceased, 
			[Seq]) 
	VALUES (
		@grand_total_text, 
		@grand_total, 
		@alive_total, 
		@deceased_total, 
		99999)  
	
	SELECT 
		[Age],
		Alive_deceased,
		Alive, 
		Deceased
	FROM 
		#temp_result
	ORDER BY
		Seq  
	
	DROP TABLE #temp_result  

END
GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00002_GetReport] TO HCVU
GO
