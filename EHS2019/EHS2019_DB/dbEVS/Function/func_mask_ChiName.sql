IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_mask_ChiName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_mask_ChiName]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	17 Sep 2019
-- CR No.:			CRE19-001 (VSS 2019/20)
-- Description:		Mask Chinese name 
--					Input:	愛新覺羅溥儀
--					Output: 愛*覺*溥*
-- ============================================= 
CREATE FUNCTION [dbo].[func_mask_ChiName]
(
	@chi_name NVARCHAR(6)
)
RETURNS varchar(40)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @index int
	DECLARE @temp_name varchar(40)
	
	DECLARE @result varchar(20)

	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @result = @chi_name
	SET @index = 2

	-- Extract Initial one by one
	WHILE(@index <= LEN(@chi_name))
	BEGIN
		SET @result = STUFF(@result, @index, 1, '*')

		SET @index = @index + 2
	END

	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_mask_ChiName] to HCVU
GO

