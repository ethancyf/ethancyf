IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_split_EngName]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_split_EngName]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Marco CHOI
-- Create date:		28 Aug 2018
-- Description:		Split the EngName to Surname and First Name
-- =============================================

CREATE FUNCTION [dbo].[func_split_EngName] (
	@string varchar(5000),
	@NameType char(1)
	)
RETURNS varchar(5000)
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @idx	int
	DECLARE @item	varchar(5000)
	DECLARE @delimiter varchar(5)
	DECLARE @TempTable Table (
		rowid int,
		Item varchar(5000)
	)
	DECLARE @rowno int
	DECLARE @result varchar(5000)
	
	SET @rowno = 1
	SET @delimiter = ','
-- =============================================
-- Validation
-- =============================================

	IF @string IS NULL
		RETURN ''

	SET @string = LTRIM(RTRIM(@string))

	IF LEN(@string) < 1 
		RETURN ''

	--@NameType
	--S: Surname
	--G: Givenname
	
	IF @NameType IS NULL
	RETURN ''
	
	IF @NameType NOT IN ('S', 'G')
	BEGIN
		RETURN ''
	END
	
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
				RETURN ''

			SET @idx = CHARINDEX(@delimiter, @string)
		END

	WHILE @idx != 0
		BEGIN
			SET @item = LEFT(@string, @idx - 1)
			
			INSERT INTO @TempTable (rowid, Item) VALUES (@rowno, LTRIM(RTRIM(@item)))
			
			SET @rowno = @rowno +1

			SET @string = RIGHT(@string, (LEN(@string) - @idx + 1) - LEN(@delimiter))
			
			SET @idx = 0
		END

	INSERT INTO @TempTable (rowid, Item) VALUES (@rowno, LTRIM(RTRIM(@string)))
	
	IF @NameType = 'S'
	BEGIN
		SELECT @result = Item FROM @TempTable WHERE rowid = 1
	END
	ELSE IF @NameType = 'G'
	BEGIN
		SELECT @result = Item FROM @TempTable WHERE rowid = 2
	END
	
	RETURN ISNULL(@result, '')
END
GO

GRANT EXECUTE ON [dbo].[func_split_EngName] TO HCVU
GO
