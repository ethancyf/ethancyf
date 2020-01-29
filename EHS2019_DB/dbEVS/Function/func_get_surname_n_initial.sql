IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_get_surname_n_initial]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_get_surname_n_initial]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Vincent
-- Create date:	2 FEB 2010
-- Description:	Return Surname and Initial for the Given English Number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
CREATE FUNCTION [dbo].[func_get_surname_n_initial]
(
	@eng_name varchar(40)
)
RETURNS varchar(40)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @separator char
	DECLARE @separator_surname char
	DECLARE @separator_index int
	DECLARE @temp_name varchar(40)
	
	DECLARE @result varchar(20)

	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @result = ''
	SET @separator = ' '
	SET @separator_surname = ', '
	SET @temp_name = LTRIM(RTRIM(@eng_name))
	SET @separator_index = CHARINDEX(@separator_surname, @temp_name)

	IF @separator_index > 0
	BEGIN
		-- Extract Surname
		SET @result = SUBSTRING(@temp_name, 1, @separator_index - 1) + ','

		-- Extract Initial one by one
		WHILE(@separator_index > 0)
		BEGIN
			SET @temp_name = LTRIM(RIGHT(@temp_name, LEN(@temp_name) - @separator_index))

			IF LEN(@temp_name) > 0
				SET @result = @result + ' ' + LEFT(@temp_name, 1) + '.'

			SET @separator_index = CHARINDEX(@separator, @temp_name)
		END

	END
	ELSE
	BEGIN
		-- No Initial in the English name
		SET @result = @eng_name

	END


	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_get_surname_n_initial] to HCSP
GO

Grant execute on [dbo].[func_get_surname_n_initial] to HCVU
GO

Grant execute on [dbo].[func_get_surname_n_initial] to HCPUBLIC
GO