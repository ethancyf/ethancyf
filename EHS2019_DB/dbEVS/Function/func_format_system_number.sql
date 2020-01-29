 IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_format_system_number]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_format_system_number]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Vincent
-- Create date:	27 JAN 2010
-- Description:	Format System Number for Display
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
CREATE FUNCTION [dbo].[func_format_system_number]
(
	@system_number varchar(50)
)
RETURNS varchar(50)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @result varchar(50)
	DECLARE @temp_system_number varchar(50)
	DECLARE @check_digit char(1)
	DECLARE @prefix varchar(50)
	DECLARE @prefix_length int
	DECLARE @generated_number varchar(50)


	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @temp_system_number = (LTRIM(RTRIM(@system_number)))
	SET @check_digit = ''
	SET @prefix = ''
	SET @generated_number = ''
	SET @result = ''

	IF LEN(@temp_system_number) = 16
		SET @prefix_length = 7
	ELSE
		SET @prefix_length = 6


	-- Extract System Number
	SET @prefix = SUBSTRING(@temp_system_number, 1, @prefix_length)		-- First 7 Characters
	SET @temp_system_number = RIGHT(@temp_system_number, LEN(@temp_system_number) - @prefix_length)
	SET @generated_number = CONVERT(VARCHAR, CONVERT(BIGINT, SUBSTRING(@temp_system_number, 1, LEN(@temp_system_number) - 1)))
	SET @check_digit = RIGHT(@temp_system_number, 1)

	SET @result = @prefix + '-' + @generated_number + '-' + @check_digit

	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_format_system_number] to HCSP
GO

Grant execute on [dbo].[func_format_system_number] to HCVU
GO

Grant execute on [dbo].[func_format_system_number] to HCPUBLIC
GO