IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_Remove_EngNameSpecialChar]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_Remove_EngNameSpecialChar]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		27 Apr 2016
-- Description:		Return the Eng Name which removed the special char
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
CREATE FUNCTION [dbo].[func_Remove_EngNameSpecialChar]
(
	@eng_name varchar(40)
)
RETURNS varchar(40)
AS
BEGIN

	SET @eng_name = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@eng_name, ' ',''), ',',''), '-',''), '.',''),'''','')
	
	-- =============================================
	-- Return results
	-- =============================================
	RETURN @eng_name

END
GO

Grant execute on [dbo].[func_Remove_EngNameSpecialChar] to HCVU
GO

