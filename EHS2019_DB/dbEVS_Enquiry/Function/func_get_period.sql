IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_get_period]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_get_period]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Lam
-- Create date:		08 Nov 2012
-- Description:		Get the period
-- =============================================

CREATE FUNCTION [dbo].[func_get_period] (
	@period_from	datetime,
	@period_to		datetime,
	@date_part		char(1)
	)
RETURNS @TempTable TABLE (Period varchar(10))
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @period	varchar(10)

-- =============================================
-- Validation
-- =============================================

	IF @period_from IS NULL OR @period_to IS NULL
		RETURN

	IF @period_from = '' OR @period_to = ''
		RETURN

	IF @date_part IS NULL
		RETURN

-- =============================================
-- Initialization
-- =============================================

	IF @date_part = 'Y'
		BEGIN
			SET @period_from = CONVERT(varchar(4), YEAR(@period_from)) + '-1-1'
			SET @period_to = CONVERT(varchar(4), YEAR(@period_to)) + '-1-1'
		END

	ELSE IF @date_part = 'M'
		BEGIN
			SET @period_from = CONVERT(varchar(4), YEAR(@period_from)) + '-' + CONVERT(varchar(2), MONTH(@period_from)) + '-1'
			SET @period_to = CONVERT(varchar(4), YEAR(@period_to)) + '-' + CONVERT(varchar(2), MONTH(@period_to)) + '-1'
		END

	ELSE IF @date_part <> 'D'
		BEGIN
			RETURN
		END

-- =============================================
-- Return results
-- =============================================

	WHILE @period_from <= @period_to
		BEGIN

			SET @period = CONVERT(varchar(10), @period_from, 20)

			IF @date_part = 'Y'
				BEGIN
					SET @period = SUBSTRING(@period, 1, 4)
					SET @period_from = DATEADD(yy, 1, @period_from)
				END

			ELSE IF @date_part = 'M'
				BEGIN
					SET @period = SUBSTRING(@period, 1, 7)
					SET @period_from = DATEADD(mm, 1, @period_from)
				END

			ELSE IF @date_part = 'D'
				BEGIN
					SET @period_from = DATEADD(dd, 1, @period_from)
				END

			INSERT INTO @TempTable (Period) VALUES (@period)

		END

	RETURN

END
GO

GRANT SELECT ON [dbo].[func_get_period] TO HCVU
GO
