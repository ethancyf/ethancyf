 IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_format_DOB]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_format_DOB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Vincent
-- Create date:	27 JAN 2010
-- Description:	Format DOB for Display
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
CREATE FUNCTION [dbo].[func_format_DOB]
(
	@date_of_birth datetime,
	@exact_dob char(1),
	@language char(5),
	@ec_age int,
	@ec_date_of_registration datetime
)
RETURNS varchar(40)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @text_on nvarchar(4000)
	DECLARE @text_age nvarchar(4000)

	DECLARE @result varchar(40)

	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================

	-- Get System Resources
	IF LOWER(@language) = 'zh-tw' 
	BEGIN
				SELECT	@text_age = Chinese_Description 
				FROM		SystemResource
				WHERE	ObjectType = 'Text' 
								AND ObjectName = 'Age'

				SELECT	@text_on = Chinese_Description 
				FROM		SystemResource
				WHERE	ObjectType = 'Text' 
								AND ObjectName = 'RegisterOn'
	END
	ELSE
	BEGIN
				SELECT	@text_age = [Description] 
				FROM		SystemResource
				WHERE	ObjectType = 'Text' 
								AND ObjectName = 'Age'

				SELECT	@text_on = [Description]
				FROM		SystemResource
				WHERE	ObjectType = 'Text' 
								AND ObjectName = 'RegisterOn'
	END


	-- Format Date
	IF @exact_dob in ('Y', 'V', 'R')
	BEGIN
			IF LOWER(@language) = 'zh-tw' 
				SET @result	= CONVERT(VARCHAR(4), @date_of_birth, 111) -- Date Format: yyyy
			ELSE
				SET @result = CONVERT(VARCHAR(4), @date_of_birth, 111) -- Date Format: yyyy
	END
	ELSE IF @exact_dob in ('M', 'U')
	BEGIN
			IF LOWER(@language) = 'zh-tw' 
				SET @result	= RIGHT(CONVERT(VARCHAR(10), @date_of_birth, 105), 7) -- Date Format: MM-yyyy
			ELSE
				SET @result	= RIGHT(CONVERT(VARCHAR(10), @date_of_birth, 105), 7) -- Date Format: MM-yyyy
	END
	ELSE IF @exact_dob in ('D', 'T')
	BEGIN
			IF LOWER(@language) = 'zh-tw' 
				SET @result	= CONVERT(VARCHAR(10), @date_of_birth, 105) -- Date Format: MM-yyyy
			ELSE
				SET @result	= CONVERT(VARCHAR(10), @date_of_birth, 105) -- Date Format: MM-yyyy
	END
	ELSE IF @exact_dob in ('A')
	BEGIN
			IF LOWER(@language) = 'zh-tw' 
				SET @result	= @text_age + ' ' + CONVERT(VARCHAR, @ec_age) + ' ' + @text_on + ' ' + CONVERT(VARCHAR(4), @ec_date_of_registration, 111) + N'ж~' + LEFT(CONVERT(VARCHAR(4), @ec_date_of_registration, 101), 2) + N'ды' + LEFT(CONVERT(VARCHAR(4), @ec_date_of_registration, 103), 2)
			ELSE
				SET @result	= @text_age + ' ' + CONVERT(VARCHAR, @ec_age) + ' ' + @text_on + ' ' + CONVERT(varchar, CONVERT(smallint, LEFT(CONVERT(VARCHAR(4), @ec_date_of_registration, 103), 2))) + ' ' + DATENAME(MONTH, CONVERT(smallint, LEFT(CONVERT(VARCHAR(4), @ec_date_of_registration, 101), 2))) + ' ' + CONVERT(VARCHAR(4), @ec_date_of_registration, 111)
	END

	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_format_DOB] to HCSP
GO

Grant execute on [dbo].[func_format_DOB] to HCVU
GO

Grant execute on [dbo].[func_format_DOB] to HCPUBLIC
GO