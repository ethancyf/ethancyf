IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_split_string]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_split_string]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Lam
-- Create date:		07 Nov 2012
-- Description:		Split the string by delimiter
-- =============================================

CREATE FUNCTION [dbo].[func_split_string] (
	@string varchar(5000),
	@delimiter varchar(5)
	)
RETURNS @TempTable TABLE (Item varchar(5000))
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @idx	int
	DECLARE @item	varchar(5000)

-- =============================================
-- Validation
-- =============================================

	IF @string IS NULL
		RETURN

	IF @delimiter IS NULL
		RETURN

	SET @string = LTRIM(RTRIM(@string))
	SET @delimiter = LTRIM(RTRIM(@delimiter))

	IF LEN(@string) < 1 OR LEN(@delimiter) < 1
		RETURN

	IF @string = @delimiter
		RETURN

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SET @idx = CHARINDEX(@delimiter, @string)

	WHILE @idx = 1
		BEGIN
			SET @string = RIGHT(@string, LEN(@string) - LEN(@delimiter))

			IF @string = @delimiter
				RETURN

			SET @idx = CHARINDEX(@delimiter, @string)
		END

	WHILE @idx != 0
		BEGIN
			SET @item = LEFT(@string, @idx - 1)
			INSERT INTO @TempTable (Item) VALUES (LTRIM(RTRIM(@item)))

			SET @string = RIGHT(@string, (LEN(@string) - @idx + 1) - LEN(@delimiter))

			IF LEN(@string) < 1 OR @string = @delimiter
				RETURN

			SET @idx = CHARINDEX(@delimiter, @string)

			WHILE @idx = 1
				BEGIN
					SET @string = RIGHT(@string, LEN(@string) - LEN(@delimiter))

					IF @string = @delimiter
						RETURN

					SET @idx = CHARINDEX(@delimiter, @string)
				END
		END

	INSERT INTO @TempTable (Item) VALUES (LTRIM(RTRIM(@string)))
	RETURN

END
GO

GRANT SELECT ON [dbo].[func_split_string] TO HCVU
GO
