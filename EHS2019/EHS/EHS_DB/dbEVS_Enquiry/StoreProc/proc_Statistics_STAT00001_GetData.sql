IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00001_GetData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00001_GetData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Apr 2015
-- Description:		1. Refine District Structure
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			INT13-0011
-- Modified by:		Koala CHENG  
-- Modified date:	14 May 2013  
-- Description:		Fix database collation problem
-- =============================================  
-- =============================================
-- Author:			Tommy Lam
-- Create date:		06 Nov 2012
-- Description:		Get Data for Statistics - STAT00001
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00001_GetData] 
	@statistic_row_type	char(1),
	@statistic_col_type	char(1),
	@period_from		datetime,
	@period_to			datetime,
	@cut_off_date		datetime,
	@age_from			int,
	@age_to				int,
	@category_filter	varchar(5000)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	SET NOCOUNT ON

	DECLARE @delimiter	varchar(5)
	DECLARE @dept_health_code char(2)

	DECLARE @CategoryFilter table	(
		CategoryFilter	varchar(15) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	CREATE TABLE #RawData (
		Voucher_Acc_ID		char(15) COLLATE Chinese_Taiwan_Stroke_CI_AS,
		Effective_Dtm		datetime,
		Create_By_Category	varchar(15) COLLATE Chinese_Taiwan_Stroke_CI_AS,
		DOB					datetime
	)

-- =============================================
-- Validation
-- =============================================

	IF @statistic_row_type IS NULL OR @statistic_col_type IS NULL
		RETURN

	IF @statistic_row_type <> 'Y' AND @statistic_row_type <> 'M' AND @statistic_row_type <> 'D'
		RETURN

	IF @statistic_col_type <> 'D' AND @statistic_col_type <> 'P'
		RETURN

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL

	IF @period_from > @period_to AND (NOT(@period_from IS NULL)) AND (NOT(@period_to IS NULL))
		RETURN

	IF @cut_off_date IS NULL
		RETURN

	IF @cut_off_date = ''
		RETURN

	IF @age_from < 0 OR @age_to < 0
		RETURN

	IF @age_from > @age_to AND (NOT(@age_from IS NULL)) AND (NOT(@age_to IS NULL))
		RETURN

	IF @category_filter IS NULL
		SET @category_filter = ''

-- =============================================
-- Initialization
-- =============================================

	SET @delimiter = ','
	SET @dept_health_code = 'DH'

	IF @category_filter <> ''
		BEGIN
			INSERT INTO @CategoryFilter (
				CategoryFilter
			)
			SELECT Item FROM func_split_string(@category_filter, @delimiter)
		END

-- =============================================
-- Retrieve Data
-- =============================================

	-- Get Raw Data by District / by Profession

	IF @statistic_col_type = 'D'

		-- No. of Accounts by District
		BEGIN

			INSERT INTO #RawData (
				Voucher_Acc_ID,
				Effective_Dtm,
				Create_By_Category,
				DOB
			)
			SELECT
				VA.Voucher_Acc_ID,
				VA.Effective_Dtm,
				COALESCE(DB.district_board_shortname_SD, @dept_health_code),
				MIN(PINFO.DOB)
			FROM
				VoucherAccount VA
					INNER JOIN VoucherAccountCreationLOG VACL
						ON VA.Voucher_Acc_ID = VACL.Voucher_Acc_ID
					INNER JOIN PersonalInformation PINFO
						ON VA.Voucher_Acc_ID = PINFO.Voucher_Acc_ID
					LEFT JOIN Practice PT
						ON VACL.SP_ID = PT.SP_ID
							AND VACL.SP_Practice_Display_Seq = PT.Display_Seq
					LEFT JOIN Professional PROF
						ON PT.SP_ID = PROF.SP_ID
							AND PT.Professional_Seq = PROF.Professional_Seq
					LEFT JOIN district DT
						ON PT.District = DT.district_code
					LEFT JOIN DistrictBoard DB
						ON DT.district_board = DB.district_board
			WHERE
				(VA.Record_Status = 'A' OR VA.Record_Status = 'S')
					AND ((VA.Effective_Dtm >= @period_from AND VA.Effective_Dtm < DATEADD(dd, 1, @period_to))
						OR (@period_from IS NULL AND @period_to IS NULL)
						OR (@period_from IS NULL AND VA.Effective_Dtm < DATEADD(dd, 1, @period_to))
						OR (@period_to IS NULL AND VA.Effective_Dtm >= @period_from))
					AND VACL.Voucher_Acc_Type = 'V'
					AND (DT.district_board IS NULL OR DT.district_area <> '4')
					AND (@category_filter = '' OR (COALESCE(PROF.Service_Category_Code, @dept_health_code) IN (SELECT * FROM @CategoryFilter)))
			GROUP BY
				VA.Voucher_Acc_ID,
				VA.Effective_Dtm,
				COALESCE(DB.district_board_shortname_SD, @dept_health_code)
			

		END

	ELSE IF @statistic_col_type = 'P'

		-- No. of Accounts by Profession
		BEGIN

			INSERT INTO #RawData (
				Voucher_Acc_ID,
				Effective_Dtm,
				Create_By_Category,
				DOB
			)
			SELECT
				VA.Voucher_Acc_ID,
				VA.Effective_Dtm,
				COALESCE(PROF.Service_Category_Code, @dept_health_code),
				MIN(PINFO.DOB)
			FROM
				VoucherAccount VA
					INNER JOIN VoucherAccountCreationLOG VACL
						ON VA.Voucher_Acc_ID = VACL.Voucher_Acc_ID
					INNER JOIN PersonalInformation PINFO
						ON VA.Voucher_Acc_ID = PINFO.Voucher_Acc_ID
					LEFT JOIN Practice PT
						ON VACL.SP_ID = PT.SP_ID
							AND VACL.SP_Practice_Display_Seq = PT.Display_Seq
					LEFT JOIN Professional PROF
						ON PT.SP_ID = PROF.SP_ID
							AND PT.Professional_Seq = PROF.Professional_Seq
					LEFT JOIN district DT
						ON PT.District = DT.district_code
					LEFT JOIN DistrictBoard DB
						ON DT.district_board = DB.district_board
			WHERE
				(VA.Record_Status = 'A' OR VA.Record_Status = 'S')
					AND ((VA.Effective_Dtm >= @period_from AND VA.Effective_Dtm < DATEADD(dd, 1, @period_to))
						OR (@period_from IS NULL AND @period_to IS NULL)
						OR (@period_from IS NULL AND VA.Effective_Dtm < DATEADD(dd, 1, @period_to))
						OR (@period_to IS NULL AND VA.Effective_Dtm >= @period_from))
					AND VACL.Voucher_Acc_Type = 'V'
					AND (DT.district_board IS NULL OR DT.district_area <> '4')
					AND (@category_filter = '' OR (COALESCE(DB.district_board_shortname_SD, @dept_health_code) IN (SELECT * FROM @CategoryFilter)))
			GROUP BY
				VA.Voucher_Acc_ID,
				VA.Effective_Dtm,
				COALESCE(PROF.Service_Category_Code, @dept_health_code)

		END

-- =============================================
-- Return results
-- =============================================

	-- Calculate Age and then filter by Age

	SELECT
		CASE
			WHEN @statistic_row_type = 'Y' THEN SUBSTRING(CONVERT(varchar(10), Effective_Dtm, 20), 1, 4)
			WHEN @statistic_row_type = 'M' THEN SUBSTRING(CONVERT(varchar(10), Effective_Dtm, 20), 1, 7)
			WHEN @statistic_row_type = 'D' THEN CONVERT(varchar(10), Effective_Dtm, 20)
		END AS [Period],
		Voucher_Acc_ID,
		Create_By_Category
	FROM
		(SELECT
			Voucher_Acc_ID,
			Effective_Dtm,
			Create_By_Category,
			DATEDIFF(yy, DOB, @cut_off_date) -	CASE
													WHEN (MONTH(DOB) > MONTH(@cut_off_date)) OR (MONTH(DOB) = MONTH(@cut_off_date) AND DAY(DOB) > DAY(@cut_off_date)) THEN 1
													ELSE 0
												END AS [Age]
		FROM
			#RawData
		WHERE
			DOB < DATEADD(dd, 1, @cut_off_date)) AS T0
	WHERE
		(Age >= @age_from AND Age <= @age_to)
		OR (@age_from IS NULL AND @age_to IS NULL)
		OR (@age_from IS NULL AND Age <= @age_to)
		OR (@age_to IS NULL AND Age >= @age_from)

	SET NOCOUNT OFF

END
GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00001_GetData] TO HCVU
GO
