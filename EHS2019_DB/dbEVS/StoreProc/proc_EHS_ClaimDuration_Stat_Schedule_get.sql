IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_ClaimDuration_Stat_Schedule_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_ClaimDuration_Stat_Schedule_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 June 2010
-- Description:		(1) Refine code
--					(2) Read more columns for SmartIC
--					(3) Limit the Get CFD Max, Min, and Avg to 3 seconds
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	2 Feb 2010
-- Description:		Fix error on convert 'N/A' to int
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	15 Dec 2009
-- Description:		Update the checking for @start_dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	15 Oct 2009
-- Description:		Update using new Audit Log entry
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	23 Mar 2009
-- Description:		Return another result set which is about the statistics for
--					getting claim during which break down in steps
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Oct 2008
-- Description:	Statistics for getting voucher account and claim
--				1. Get related data to temp table (_ClaimDurationSummary)
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_ClaimDuration_Stat_Schedule_get]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting 
-- =============================================
	DECLARE @No_Of_Days	int
	SELECT @No_Of_Days = 14


-- =============================================
-- Initialization
-- =============================================
	DECLARE @Start_Dtm	datetime
	DECLARE @End_Dtm	datetime
	
	SELECT @End_Dtm = CONVERT(varchar, GETDATE(), 106)	-- "106" gives "dd MMM yyyy"
	SELECT @Start_Dtm = DATEADD(dd, -(@No_Of_Days), @End_Dtm)
	
	IF @Start_Dtm < CONVERT(datetime, '01 Jan 2009', 106) BEGIN
		SELECT @Start_Dtm = '01 Jan 2009'
	END


-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @Summary table (
		Type					char(1),
		Dtm						datetime,
		Claim_NC_NP_Max			char(8),
		Claim_NC_NP_Min			char(8),
		Claim_NC_NP_Avg			char(8),
		Claim_NC_NP_T			int,
		Claim_NC_P_Max			char(8),
		Claim_NC_P_Min			char(8),
		Claim_NC_P_Avg			char(8),
		Claim_NC_P_T			int,
		Claim_C_NP_Max			char(8),
		Claim_C_NP_Min			char(8),
		Claim_C_NP_Avg			char(8),
		Claim_C_NP_T			int,
		Claim_C_P_Max			char(8),
		Claim_C_P_Min			char(8),
		Claim_C_P_Avg			char(8),
		Claim_C_P_T				int,
		Claim_P_Max				char(8),
		Claim_P_Min				char(8),
		Claim_P_Avg				char(8),
		AC_Max					char(8),
		AC_Min					char(8),
		AC_Avg					char(8)
	)

	DECLARE @ClaimDurationStep table (
		Type					char(1),
		Dtm						datetime,
		Search_User_Max			char(8),
		Search_User_Min			char(8),
		Search_User_Avg			char(8),
		Reading_Card_Max		char(8),
		Reading_Card_Min		char(8),
		Reading_Card_Avg		char(8),
		Get_CFD_Max				char(8),
		Get_CFD_Min				char(8),
		Get_CFD_Avg				char(8),
		Search_Sys_Max			char(8),
		Search_Sys_Min			char(8),
		Search_Sys_Avg			char(8),
		Search_Count			int,
		EnterClaim_User_Max		char(8),
		EnterClaim_User_Min		char(8),
		EnterClaim_User_Avg		char(8),
		EnterClaim_Sys_Max		char(8),
		EnterClaim_Sys_Min		char(8),
		EnterClaim_Sys_Avg		char(8),
		EnterClaim_Count		int,
		EnterAC_User_Max		char(8),
		EnterAC_User_Min		char(8),
		EnterAC_User_Avg		char(8),
		EnterAC_Sys_Max			char(8),
		EnterAC_Sys_Min			char(8),
		EnterAC_Sys_Avg			char(8),
		EnterAC_Count			int,
		ProcessClaim_User_Max	char(8),
		ProcessClaim_User_Min	char(8),
		ProcessClaim_User_Avg	char(8),
		ProcessClaim_Sys_Max	char(8),
		ProcessClaim_Sys_Min	char(8),
		ProcessClaim_Sys_Avg	char(8),
		ProcessClaim_Count		int
	)


-- =============================================
-- Retrieve data
-- =============================================
	DECLARE @Current_Dtm datetime
	SELECT @Current_Dtm = @Start_Dtm
	
	WHILE @Current_Dtm < @End_Dtm BEGIN

		-- Summary (Manual)
		
		INSERT INTO @Summary (
			Type,
			Dtm,
			Claim_NC_NP_Max,
			Claim_NC_NP_Min,
			Claim_NC_NP_Avg,
			Claim_NC_NP_T,
			Claim_NC_P_Max,
			Claim_NC_P_Min,
			Claim_NC_P_Avg,
			Claim_NC_P_T,
			Claim_C_NP_Max,
			Claim_C_NP_Min,
			Claim_C_NP_Avg,
			Claim_C_NP_T,
			Claim_C_P_Max,
			Claim_C_P_Min,
			Claim_C_P_Avg,
			Claim_C_P_T,
			Claim_P_Max,
			Claim_P_Min,
			Claim_P_Avg,
			AC_Max,
			AC_Min,
			AC_Avg
		) VALUES (
			'M',
			@Current_Dtm,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			''
		)
		
		UPDATE
			@Summary
		SET
			Claim_NC_NP_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm
			),
			Claim_NC_NP_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_NP_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm
			),
			Claim_NC_NP_T = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_T = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_T = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_T  = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_P_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Claim Printing Time' AND report_dtm = @Current_Dtm 
			),
			Claim_P_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Claim Printing Time' AND report_dtm = @Current_Dtm 
			),
			Claim_P_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Claim Printing Time' AND report_dtm = @Current_Dtm 
			),
			AC_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'A/C Creation (- creation printing)' AND report_dtm = @Current_Dtm 
			),
			AC_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'A/C Creation (- creation printing)' AND report_dtm = @Current_Dtm 
			),
			AC_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'A/C Creation (- creation printing)' AND report_dtm = @Current_Dtm 
			)
		WHERE
			Type = 'M'
				AND Dtm = @Current_Dtm
		
		
		-- ClaimDurationStep (Manual)
	
		INSERT INTO @ClaimDurationStep (
			Type,
			Dtm,
			Search_User_Max,
			Search_User_Min,
			Search_User_Avg,
			Search_Sys_Max,
			Search_Sys_Min,
			Search_Sys_Avg,
			Search_Count,
			EnterClaim_User_Max,
			EnterClaim_User_Min,
			EnterClaim_User_Avg,
			EnterClaim_Sys_Max,
			EnterClaim_Sys_Min,
			EnterClaim_Sys_Avg,
			EnterClaim_Count,
			EnterAC_User_Max,
			EnterAC_User_Min,
			EnterAC_User_Avg,
			EnterAC_Sys_Max,
			EnterAC_Sys_Min,
			EnterAC_Sys_Avg,
			EnterAC_Count,
			ProcessClaim_User_Max,
			ProcessClaim_User_Min,
			ProcessClaim_User_Avg,
			ProcessClaim_Sys_Max,
			ProcessClaim_Sys_Min,
			ProcessClaim_Sys_Avg,
			ProcessClaim_Count				
		) VALUES (
			'M',
			@Current_Dtm,
			'',
			'',
			'',
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			'',
			0
		)
		
		UPDATE
			@ClaimDurationStep
		SET
			Search_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Search (User Time)' AND report_dtm = @Current_Dtm 
			),
			Search_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Search (User Time)' AND report_dtm = @Current_Dtm 
			),
			Search_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Search (User Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'Process To Claim (User Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Process To Claim (User Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'Process To Claim (User Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			)
		WHERE
			Type = 'M'
				AND Dtm = @Current_Dtm
		
		-- Summary (Smart IC)
		
		INSERT INTO @Summary (
			Type,
			Dtm,
			Claim_NC_NP_Max,
			Claim_NC_NP_Min,
			Claim_NC_NP_Avg,
			Claim_NC_NP_T,
			Claim_NC_P_Max,
			Claim_NC_P_Min,
			Claim_NC_P_Avg,
			Claim_NC_P_T,
			Claim_C_NP_Max,
			Claim_C_NP_Min,
			Claim_C_NP_Avg,
			Claim_C_NP_T,
			Claim_C_P_Max,
			Claim_C_P_Min,
			Claim_C_P_Avg,
			Claim_C_P_T,
			Claim_P_Max,
			Claim_P_Min,
			Claim_P_Avg,
			AC_Max,
			AC_Min,
			AC_Avg
		) VALUES (
			'S',
			@Current_Dtm,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			''
		)
		
		UPDATE
			@Summary
		SET
			Claim_NC_NP_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm
			),
			Claim_NC_NP_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_NP_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm
			),
			Claim_NC_NP_T = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_NC_P_T = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (without A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_NP_T = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & - claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_C_P_T  = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim (with A/C creation & + claim printing)' AND report_dtm = @Current_Dtm 
			),
			Claim_P_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim Printing Time' AND report_dtm = @Current_Dtm 
			),
			Claim_P_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim Printing Time' AND report_dtm = @Current_Dtm 
			),
			Claim_P_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Claim Printing Time' AND report_dtm = @Current_Dtm 
			),
			AC_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC A/C Creation (- creation printing)' AND report_dtm = @Current_Dtm 
			),
			AC_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC A/C Creation (- creation printing)' AND report_dtm = @Current_Dtm 
			),
			AC_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC A/C Creation (- creation printing)' AND report_dtm = @Current_Dtm 
			)
		WHERE
			Type = 'S'
				AND Dtm = @Current_Dtm
		
		
		-- ClaimDurationStep (Smart IC)
	
		INSERT INTO @ClaimDurationStep (
			Type,
			Dtm,
			Search_User_Max,
			Search_User_Min,
			Search_User_Avg,
			Reading_Card_Max,
			Reading_Card_Min,
			Reading_Card_Avg,
			Get_CFD_Max,
			Get_CFD_Min,
			Get_CFD_Avg,
			Search_Sys_Max,
			Search_Sys_Min,
			Search_Sys_Avg,
			Search_Count,
			EnterClaim_User_Max,
			EnterClaim_User_Min,
			EnterClaim_User_Avg,
			EnterClaim_Sys_Max,
			EnterClaim_Sys_Min,
			EnterClaim_Sys_Avg,
			EnterClaim_Count,
			EnterAC_User_Max,
			EnterAC_User_Min,
			EnterAC_User_Avg,
			EnterAC_Sys_Max,
			EnterAC_Sys_Min,
			EnterAC_Sys_Avg,
			EnterAC_Count,
			ProcessClaim_User_Max,
			ProcessClaim_User_Min,
			ProcessClaim_User_Avg,
			ProcessClaim_Sys_Max,
			ProcessClaim_Sys_Min,
			ProcessClaim_Sys_Avg,
			ProcessClaim_Count				
		) VALUES (
			'S',
			@Current_Dtm,
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			'',
			0,
			'',
			'',
			'',
			'',
			'',
			'',
			0
		)
		
		UPDATE
			@ClaimDurationStep
		SET
			Search_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (User Time)' AND report_dtm = @Current_Dtm 
			),
			Search_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (User Time)' AND report_dtm = @Current_Dtm 
			),
			Search_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (User Time)' AND report_dtm = @Current_Dtm 
			),
			Reading_Card_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Reading Card' AND report_dtm = @Current_Dtm 
			),
			Reading_Card_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Reading Card' AND report_dtm = @Current_Dtm 
			),
			Reading_Card_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Reading Card' AND report_dtm = @Current_Dtm 
			),
			Get_CFD_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Get CFD' AND report_dtm = @Current_Dtm 
			),
			Get_CFD_Min = (
				SELECT CASE WHEN min_value IS NOT NULL AND CONVERT(int, min_value) > 3 THEN '3' ELSE min_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Get CFD' AND report_dtm = @Current_Dtm 
			),
			Get_CFD_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Get CFD' AND report_dtm = @Current_Dtm 
			),
			Search_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			Search_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Search (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterClaim_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter Claim Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (User Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			EnterAC_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Enter A/C Creation Details (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_User_Max = (
				SELECT max_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (User Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_User_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (User Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_User_Avg = (
				SELECT avg_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (User Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Sys_Max = (
				SELECT CASE WHEN max_value IS NOT NULL AND CONVERT(int, max_value) > 3 THEN '3' ELSE max_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Sys_Min = (
				SELECT min_value FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Sys_Avg = (
				SELECT CASE WHEN avg_value IS NOT NULL AND CONVERT(int, avg_value) > 3 THEN '3' ELSE avg_value END FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			),
			ProcessClaim_Count = (
				SELECT no_of_transaction FROM _ClaimDurationSummary WHERE title = 'SmartIC Process To Claim (System Time)' AND report_dtm = @Current_Dtm 
			)
		WHERE
			Type = 'S'
				AND Dtm = @Current_Dtm
		
			
		SELECT @Current_Dtm = DATEADD(dd, 1, @Current_Dtm)
		
	END


-- =============================================
-- Return result
-- =============================================

	-- Summary (Manual)

	SELECT
		CONVERT(varchar, Dtm, 111),
		ISNULL(Claim_NC_NP_Max, 'N/A'),
		ISNULL(Claim_NC_NP_Min, 'N/A'),
		ISNULL(Claim_NC_NP_Avg, 'N/A'),
		ISNULL(Claim_NC_NP_T, 0),
		ISNULL(Claim_NC_P_Max, 'N/A'),
		ISNULL(Claim_NC_P_Min, 'N/A'),
		ISNULL(Claim_NC_P_Avg, 'N/A'),
		ISNULL(Claim_NC_P_T, 0),
		ISNULL(Claim_C_NP_Max, 'N/A'),
		ISNULL(Claim_C_NP_Min, 'N/A'),
		ISNULL(Claim_C_NP_Avg, 'N/A'),
		ISNULL(Claim_C_NP_T, 0),
		ISNULL(Claim_C_P_Max, 'N/A'),
		ISNULL(Claim_C_P_Min, 'N/A'),
		ISNULL(Claim_C_P_Avg, 'N/A'),
		ISNULL(Claim_C_P_T, 0),
		ISNULL(Claim_P_Max, 'N/A'),
		ISNULL(Claim_P_Min, 'N/A'),
		ISNULL(Claim_P_Avg, 'N/A'),
		ISNULL(AC_Max, 'N/A'),
		ISNULL(AC_Min, 'N/A'),
		ISNULL(AC_Avg, 'N/A')
	FROM
		@Summary 
	WHERE
		Type = 'M'
	ORDER BY
		Dtm
	
	-- ClaimDurationStep (Manual)

	SELECT
		CONVERT(varchar, Dtm, 111),
		ISNULL(Search_User_Max, 'N/A'),
		ISNULL(Search_User_Min, 'N/A'),
		ISNULL(Search_User_Avg, 'N/A'),
		ISNULL(Search_Sys_Max, 'N/A'),
		ISNULL(Search_Sys_Min, 'N/A'),
		ISNULL(Search_Sys_Avg, 'N/A'),
		ISNULL(Search_Count, 0),
		ISNULL(EnterClaim_User_Max, 'N/A'),
		ISNULL(EnterClaim_User_Min, 'N/A'),
		ISNULL(EnterClaim_User_Avg, 'N/A'),
		ISNULL(EnterClaim_Sys_Max, 'N/A'),
		ISNULL(EnterClaim_Sys_Min, 'N/A'),
		ISNULL(EnterClaim_Sys_Avg, 'N/A'),
		ISNULL(EnterClaim_Count, 0),
		ISNULL(EnterAC_User_Max, 'N/A'),
		ISNULL(EnterAC_User_Min, 'N/A'),
		ISNULL(EnterAC_User_Avg, 'N/A'),
		ISNULL(EnterAC_Sys_Max, 'N/A'),
		ISNULL(EnterAC_Sys_Min, 'N/A'),
		ISNULL(EnterAC_Sys_Avg, 'N/A'),
		ISNULL(EnterAC_Count, 0),
		ISNULL(ProcessClaim_User_Max, 'N/A'),
		ISNULL(ProcessClaim_User_Min, 'N/A'),
		ISNULL(ProcessClaim_User_Avg, 'N/A'),
		ISNULL(ProcessClaim_Sys_Max, 'N/A'),
		ISNULL(ProcessClaim_Sys_Min, 'N/A'),
		ISNULL(ProcessClaim_Sys_Avg, 'N/A'),
		ISNULL(ProcessClaim_Count, 0)
	FROM
		@ClaimDurationStep
	WHERE
		Type = 'M'
	ORDER BY
		Dtm
	
	-- Summary (Smart IC)

	SELECT
		CONVERT(varchar, Dtm, 111),
		ISNULL(Claim_NC_NP_Max, 'N/A'),
		ISNULL(Claim_NC_NP_Min, 'N/A'),
		ISNULL(Claim_NC_NP_Avg, 'N/A'),
		ISNULL(Claim_NC_NP_T, 0),
		ISNULL(Claim_NC_P_Max, 'N/A'),
		ISNULL(Claim_NC_P_Min, 'N/A'),
		ISNULL(Claim_NC_P_Avg, 'N/A'),
		ISNULL(Claim_NC_P_T, 0),
		ISNULL(Claim_C_NP_Max, 'N/A'),
		ISNULL(Claim_C_NP_Min, 'N/A'),
		ISNULL(Claim_C_NP_Avg, 'N/A'),
		ISNULL(Claim_C_NP_T, 0),
		ISNULL(Claim_C_P_Max, 'N/A'),
		ISNULL(Claim_C_P_Min, 'N/A'),
		ISNULL(Claim_C_P_Avg, 'N/A'),
		ISNULL(Claim_C_P_T, 0),
		ISNULL(Claim_P_Max, 'N/A'),
		ISNULL(Claim_P_Min, 'N/A'),
		ISNULL(Claim_P_Avg, 'N/A'),
		ISNULL(AC_Max, 'N/A'),
		ISNULL(AC_Min, 'N/A'),
		ISNULL(AC_Avg, 'N/A')
	FROM
		@Summary 
	WHERE
		Type = 'S'
	ORDER BY
		Dtm
	
	-- ClaimDurationStep (Smart IC)

	SELECT
		CONVERT(varchar, Dtm, 111),
		ISNULL(Search_User_Max, 'N/A'),
		ISNULL(Search_User_Min, 'N/A'),
		ISNULL(Search_User_Avg, 'N/A'),
		ISNULL(Reading_Card_Max, 'N/A'),
		ISNULL(Reading_Card_Min, 'N/A'),
		ISNULL(Reading_Card_Avg, 'N/A'),
		ISNULL(Get_CFD_Max, 'N/A'),
		ISNULL(Get_CFD_Min, 'N/A'),
		ISNULL(Get_CFD_Avg, 'N/A'),
		ISNULL(Search_Sys_Max, 'N/A'),
		ISNULL(Search_Sys_Min, 'N/A'),
		ISNULL(Search_Sys_Avg, 'N/A'),
		ISNULL(Search_Count, 0),
		ISNULL(EnterClaim_User_Max, 'N/A'),
		ISNULL(EnterClaim_User_Min, 'N/A'),
		ISNULL(EnterClaim_User_Avg, 'N/A'),
		ISNULL(EnterClaim_Sys_Max, 'N/A'),
		ISNULL(EnterClaim_Sys_Min, 'N/A'),
		ISNULL(EnterClaim_Sys_Avg, 'N/A'),
		ISNULL(EnterClaim_Count, 0),
		ISNULL(EnterAC_User_Max, 'N/A'),
		ISNULL(EnterAC_User_Min, 'N/A'),
		ISNULL(EnterAC_User_Avg, 'N/A'),
		ISNULL(EnterAC_Sys_Max, 'N/A'),
		ISNULL(EnterAC_Sys_Min, 'N/A'),
		ISNULL(EnterAC_Sys_Avg, 'N/A'),
		ISNULL(EnterAC_Count, 0),
		ISNULL(ProcessClaim_User_Max, 'N/A'),
		ISNULL(ProcessClaim_User_Min, 'N/A'),
		ISNULL(ProcessClaim_User_Avg, 'N/A'),
		ISNULL(ProcessClaim_Sys_Max, 'N/A'),
		ISNULL(ProcessClaim_Sys_Min, 'N/A'),
		ISNULL(ProcessClaim_Sys_Avg, 'N/A'),
		ISNULL(ProcessClaim_Count, 0)
	FROM
		@ClaimDurationStep
	WHERE
		Type = 'S'
	ORDER BY
		Dtm
	
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_ClaimDuration_Stat_Schedule_get] TO HCVU
GO
