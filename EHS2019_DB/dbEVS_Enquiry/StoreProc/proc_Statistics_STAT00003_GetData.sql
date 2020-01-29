IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00003_GetData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00003_GetData]
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
-- Create date:		19 Nov 2012
-- Description:		Get Data for Statistics - STAT00003
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00003_GetData]
	@statistic_row_type		char(1),
	@statistic_col_type		char(1),
	@period_type			char(1),
	@period_from			datetime,
	@period_to				datetime,
	@scheme_code			char(10),
	@statistics_unit		char(1),
	@subsidize_display_code	char(25),
	@transaction_method		varchar(5000),
	@category_filter		varchar(5000)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	SET NOCOUNT ON

	DECLARE @delimiter	varchar(5)

	DECLARE @TransactionMethod table (
		TransactionMethod		varchar(10) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	DECLARE @CategoryFilter table (
		CategoryFilter	varchar(15) COLLATE Chinese_Taiwan_Stroke_CI_AS
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

	IF @period_type IS NULL
		RETURN

	IF @period_type <> 'T' AND @period_type <> 'S'
		RETURN

	IF @period_from IS NULL
		SET @period_from = 0

	IF @period_from = ''
		SET @period_from = 0

	IF @period_to IS NULL
		SET @period_to = 2958462

	IF @period_to = ''
		SET @period_to = 2958462

	IF @period_from > @period_to AND @period_from <> 0 AND @period_to <> 2958462
		RETURN

	IF @scheme_code IS NULL
		SET @scheme_code = ''

	IF @subsidize_display_code IS NULL
		SET @subsidize_display_code = ''

	IF @statistics_unit IS NULL
		RETURN

	IF @statistics_unit <> 'T' AND @statistics_unit <> 'S'
		RETURN

	IF @transaction_method IS NULL
		SET @transaction_method = ''

	IF @category_filter IS NULL
		SET @category_filter = ''

-- =============================================
-- Initialization
-- =============================================

	SET @delimiter = ','

	IF @transaction_method <> ''
		BEGIN
			INSERT INTO @TransactionMethod (
				TransactionMethod
			)
			SELECT Item FROM func_split_string(@transaction_method, @delimiter)
		END

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
-- =============================================
-- Return results
-- =============================================

	IF @statistic_col_type = 'D'

		-- No. of Transactions by District
		BEGIN

			IF @statistics_unit = 'S'
				BEGIN

					SELECT
						CASE
							WHEN @statistic_row_type = 'Y' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'T' THEN CONVERT(varchar(10), VT.Transaction_Dtm, 20)
							WHEN @statistic_row_type = 'Y' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'S' THEN CONVERT(varchar(10), VT.Service_Receive_Dtm, 20)
						END AS [Period],
						VT.Transaction_ID,
						SUM(TD.Unit) AS [No_Of_Unit],
						DB.district_board_shortname_SD

					FROM
						VoucherTransaction VT
							INNER JOIN TransactionDetail TD
								ON VT.Transaction_ID = TD.Transaction_ID
							INNER JOIN Subsidize SS
								ON TD.Subsidize_Code = SS.Subsidize_Code
									AND TD.Subsidize_Item_Code = SS.Subsidize_Item_Code
							INNER JOIN Practice P
								ON VT.SP_ID = P.SP_ID
									AND VT.Practice_Display_Seq = P.Display_Seq
							INNER JOIN dbo.district D
								ON P.District = D.district_code
							INNER JOIN DistrictBoard DB
								ON D.district_board = DB.district_board

					WHERE
						(VT.Record_Status <> 'I' AND VT.Record_Status <> 'D')
						AND ((@period_type = 'T' AND VT.Transaction_Dtm >= @period_from AND VT.Transaction_Dtm < DATEADD(dd, 1, @period_to))
							OR (@period_type = 'S' AND VT.Service_Receive_Dtm >= @period_from AND VT.Service_Receive_Dtm < DATEADD(dd, 1, @period_to)))
						AND (@scheme_code = '' OR VT.Scheme_Code = @scheme_code COLLATE Chinese_Taiwan_Stroke_CI_AS)
						AND (@subsidize_display_code = '' OR SS.Display_Code = @subsidize_display_code COLLATE Chinese_Taiwan_Stroke_CI_AS)
						AND (@transaction_method = '' OR (	CASE
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND VT.Create_By_SmartID = 'Y' THEN 'WEB-FULLY'
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-FULLN'
																WHEN VT.SourceApp = 'WEB-TEXT' AND VT.Create_By_SmartID = 'Y' THEN 'WEB-TEXTY'
																WHEN VT.SourceApp = 'WEB-TEXT' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-TEXTN'
																ELSE VT.SourceApp
															END IN (SELECT * FROM @TransactionMethod)))
						AND (@category_filter = '' OR VT.Service_Type IN (SELECT * FROM @CategoryFilter))
						AND (ISNULL(VT.Invalidation, '') <> 'I')
						AND (DB.area_code <> '4')

					GROUP BY
						CASE
							WHEN @statistic_row_type = 'Y' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'T' THEN CONVERT(varchar(10), VT.Transaction_Dtm, 20)
							WHEN @statistic_row_type = 'Y' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'S' THEN CONVERT(varchar(10), VT.Service_Receive_Dtm, 20)
						END,
						VT.Transaction_ID,
						DB.district_board_shortname_SD

				END

			ELSE IF @statistics_unit = 'T'
				BEGIN

					SELECT
						CASE
							WHEN @statistic_row_type = 'Y' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'T' THEN CONVERT(varchar(10), VT.Transaction_Dtm, 20)
							WHEN @statistic_row_type = 'Y' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'S' THEN CONVERT(varchar(10), VT.Service_Receive_Dtm, 20)
						END AS [Period],
						VT.Transaction_ID,
						0 AS [No_Of_Unit],
						DB.district_board_shortname_SD

					FROM
						VoucherTransaction VT
							INNER JOIN Practice P
								ON VT.SP_ID = P.SP_ID
									AND VT.Practice_Display_Seq = P.Display_Seq
							INNER JOIN dbo.district D
								ON P.District = D.district_code
							INNER JOIN DistrictBoard DB
								ON D.district_board = DB.district_board

					WHERE
						(VT.Record_Status <> 'I' AND VT.Record_Status <> 'D')
						AND ((@period_type = 'T' AND VT.Transaction_Dtm >= @period_from AND VT.Transaction_Dtm < DATEADD(dd, 1, @period_to))
							OR (@period_type = 'S' AND VT.Service_Receive_Dtm >= @period_from AND VT.Service_Receive_Dtm < DATEADD(dd, 1, @period_to)))
						AND (@scheme_code = '' OR VT.Scheme_Code = @scheme_code COLLATE Chinese_Taiwan_Stroke_CI_AS)
						AND (@transaction_method = '' OR (	CASE
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND VT.Create_By_SmartID = 'Y' THEN 'WEB-FULLY'
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-FULLN'
																WHEN VT.SourceApp = 'WEB-TEXT' AND VT.Create_By_SmartID = 'Y' THEN 'WEB-TEXTY'
																WHEN VT.SourceApp = 'WEB-TEXT' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-TEXTN'
																ELSE VT.SourceApp
															END IN (SELECT * FROM @TransactionMethod)))
						AND (@category_filter = '' OR VT.Service_Type IN (SELECT * FROM @CategoryFilter))
						AND (ISNULL(VT.Invalidation, '') <> 'I')
						AND (DB.area_code <> '4')

				END

		END

	ELSE IF @statistic_col_type = 'P'

		-- No. of Transactions by Profession
		BEGIN

			IF @statistics_unit = 'S'
				BEGIN

					SELECT
						CASE
							WHEN @statistic_row_type = 'Y' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'T' THEN CONVERT(varchar(10), VT.Transaction_Dtm, 20)
							WHEN @statistic_row_type = 'Y' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'S' THEN CONVERT(varchar(10), VT.Service_Receive_Dtm, 20)
						END AS [Period],
						VT.Transaction_ID,
						SUM(TD.Unit) AS [No_Of_Unit],
						VT.Service_Type

					FROM
						VoucherTransaction VT
							INNER JOIN TransactionDetail TD
								ON VT.Transaction_ID = TD.Transaction_ID
							INNER JOIN Subsidize SS
								ON TD.Subsidize_Code = SS.Subsidize_Code
									AND TD.Subsidize_Item_Code = SS.Subsidize_Item_Code
							INNER JOIN Practice P
								ON VT.SP_ID = P.SP_ID
									AND VT.Practice_Display_Seq = P.Display_Seq
							INNER JOIN dbo.district D
								ON P.District = D.district_code
							INNER JOIN DistrictBoard DB
								ON D.district_board = DB.district_board

					WHERE
						(VT.Record_Status <> 'I' AND VT.Record_Status <> 'D')
						AND ((@period_type = 'T' AND VT.Transaction_Dtm >= @period_from AND VT.Transaction_Dtm < DATEADD(dd, 1, @period_to))
							OR (@period_type = 'S' AND VT.Service_Receive_Dtm >= @period_from AND VT.Service_Receive_Dtm < DATEADD(dd, 1, @period_to)))
						AND (@scheme_code = '' OR VT.Scheme_Code = @scheme_code COLLATE Chinese_Taiwan_Stroke_CI_AS)
						AND (@subsidize_display_code = '' OR SS.Display_Code = @subsidize_display_code COLLATE Chinese_Taiwan_Stroke_CI_AS)
						AND (@transaction_method = '' OR (	CASE
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND VT.Create_By_SmartID = 'Y' THEN 'WEB-FULLY'
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-FULLN'
																WHEN VT.SourceApp = 'WEB-TEXT' AND VT.Create_By_SmartID = 'Y' THEN 'WEB-TEXTY'
																WHEN VT.SourceApp = 'WEB-TEXT' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-TEXTN'
																ELSE VT.SourceApp
															END IN (SELECT * FROM @TransactionMethod)))
						AND (@category_filter = '' OR DB.district_board_shortname_SD IN (SELECT * FROM @CategoryFilter))
						AND (ISNULL(VT.Invalidation, '') <> 'I')
						AND (DB.area_code <> '4')

					GROUP BY
						CASE
							WHEN @statistic_row_type = 'Y' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'T' THEN CONVERT(varchar(10), VT.Transaction_Dtm, 20)
							WHEN @statistic_row_type = 'Y' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'S' THEN CONVERT(varchar(10), VT.Service_Receive_Dtm, 20)
						END,
						VT.Transaction_ID,
						VT.Service_Type

				END

			ELSE IF @statistics_unit = 'T'
				BEGIN

					SELECT
						CASE
							WHEN @statistic_row_type = 'Y' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'T' THEN SUBSTRING(CONVERT(varchar(10), VT.Transaction_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'T' THEN CONVERT(varchar(10), VT.Transaction_Dtm, 20)
							WHEN @statistic_row_type = 'Y' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 4)
							WHEN @statistic_row_type = 'M' AND @period_type = 'S' THEN SUBSTRING(CONVERT(varchar(10), VT.Service_Receive_Dtm, 20), 1, 7)
							WHEN @statistic_row_type = 'D' AND @period_type = 'S' THEN CONVERT(varchar(10), VT.Service_Receive_Dtm, 20)
						END AS [Period],
						VT.Transaction_ID,
						0 AS [No_Of_Unit],
						VT.Service_Type

					FROM
						VoucherTransaction VT
							INNER JOIN Practice P
								ON VT.SP_ID = P.SP_ID
									AND VT.Practice_Display_Seq = P.Display_Seq
							INNER JOIN dbo.district D
								ON P.District = D.district_code
							INNER JOIN DistrictBoard DB
								ON D.district_board = DB.district_board

					WHERE
						(VT.Record_Status <> 'I' AND VT.Record_Status <> 'D')
						AND ((@period_type = 'T' AND VT.Transaction_Dtm >= @period_from AND VT.Transaction_Dtm < DATEADD(dd, 1, @period_to))
							OR (@period_type = 'S' AND VT.Service_Receive_Dtm >= @period_from AND VT.Service_Receive_Dtm < DATEADD(dd, 1, @period_to)))
						AND (@scheme_code = '' OR VT.Scheme_Code = @scheme_code COLLATE Chinese_Taiwan_Stroke_CI_AS)
						AND (@transaction_method = '' OR (	CASE
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND VT.Create_By_SmartID = 'Y' THEN 'WEB-FULLY'
																WHEN (VT.SourceApp = 'WEB' OR VT.SourceApp = 'WEB-FULL') AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-FULLN'
																WHEN VT.SourceApp = 'WEB-TEXT' AND VT.Create_By_SmartID = 'Y' THEN 'WEB-TEXTY'
																WHEN VT.SourceApp = 'WEB-TEXT' AND ISNULL(VT.Create_By_SmartID, 'N') = 'N' THEN 'WEB-TEXTN'
																ELSE VT.SourceApp
															END IN (SELECT * FROM @TransactionMethod)))
						AND (@category_filter = '' OR DB.district_board_shortname_SD IN (SELECT * FROM @CategoryFilter))
						AND (ISNULL(VT.Invalidation, '') <> 'I')
						AND (DB.area_code <> '4')

				END

		END

	SET NOCOUNT OFF

END
GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00003_GetData] TO HCVU
GO
