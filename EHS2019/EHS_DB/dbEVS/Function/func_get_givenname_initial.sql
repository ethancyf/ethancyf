IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_get_givenname_initial]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_get_givenname_initial]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	17 Sep 2019
-- CR No.:			CRE19-001 (VSS 2019/20)
-- Description:		Mask given name only
--					Input:	TAI MAN
--					Output: T. M.
-- ============================================= 
CREATE FUNCTION [dbo].[func_get_givenname_initial]
(
	@eng_givenname varchar(40)
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
	
	DECLARE @result varchar(40)

	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @result = ''
	SET @separator = ' '
	SET @temp_name = @separator + LTRIM(RTRIM(@eng_givenname))
	SET @separator_index = 1 

	-- Extract Initial one by one
	WHILE(@separator_index > 0)
	BEGIN
		SET @temp_name = LTRIM(RIGHT(@temp_name, LEN(@temp_name) - @separator_index))

		IF LEN(@temp_name) > 0
			SET @result = @result + ' ' + LEFT(@temp_name, 1) + '.'

		SET @separator_index = CHARINDEX(@separator, @temp_name)
	END

	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_get_givenname_initial] to HCVU
GO

