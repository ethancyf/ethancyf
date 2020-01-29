 IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_get_top_row]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_get_top_row]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
-- =============================================
-- Author:			Koala CHENG
-- Create date:		07 JAN 2013
-- Description:		Get the max top row number for stored procedure implement feature "RESULT_LIMIT_1ST_ENABLE" & "RESULT_LIMIT_OVERRIDE_ENABLE"
--					in table [FunctionFeature_FNFT].
--					This function should be work together with [proc_CheckFeatureResultRowLimit]
-- =============================================
CREATE FUNCTION [dbo].[func_get_top_row]
(
	@result_limit_1st_enable BIT, -- 1 = Enable the checking row limit (e.g. 500 rows), 0 = No row limit  
	@result_limit_override_enable BIT -- 1 = Enable the override checking row limit (e.g. 800 rows), 0 = No override
)
RETURNS INT
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @result INT
	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	-- =============================================
	-- Return results
	-- =============================================
	IF @result_limit_1st_enable = 1 AND @result_limit_override_enable = 1 
		-- Result limit enabled and override it, so get the 2nd row limit as top row
		SELECT @result = CONVERT(INT, parm_Value2) + 1 FROM SystemParameters WITH (NOLOCK) 
		WHERE parameter_name = 'MaxRowRetrieve' AND Record_Status = 'A'
	ELSE IF @result_limit_1st_enable = 1 AND @result_limit_override_enable = 0 
		-- Result limit enabled and not override it, so get the 1st row limit as top row
		SELECT @result = CONVERT(INT, parm_Value1) + 1 FROM SystemParameters WITH (NOLOCK) 
		WHERE parameter_name = 'MaxRowRetrieve' AND Record_Status = 'A'
	ELSE
		-- Result limit is not enabled (Ignore @override_result_limit), so get the 1st row limit as top row
		SELECT @result = 2147483647
	
	RETURN @result
END
GO

Grant execute on [dbo].[func_get_top_row] to HCVU
GO
