IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_MassageFreeTextInput]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_MassageFreeTextInput]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Marco CHOI
-- Create date:		29 Oct 2017
-- Description:		Massage Free Text Input
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE FUNCTION [dbo].[func_MassageFreeTextInput]
(
	@Input		nvarchar(1000)
)
RETURNS nvarchar(1000)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Return result
-- =============================================	
	SET @Input = UPPER(LTRIM(RTRIM(@Input)))
	SET @Input = REPLACE(@Input, '\', '\\')
	SET @Input = REPLACE(@Input, '%', '\%')
	SET @Input = REPLACE(@Input, '_', '\_')
	SET @Input = REPLACE(@Input, '[', '\[')
	SET @Input = REPLACE(@Input, ']', '\]')
	SET @Input = REPLACE(@Input, '^', '\^')
	SET @Input = REPLACE(@Input, ',', '%')
	SET @Input = REPLACE(@Input, ' ', '%')
	SET @Input = '%' + @Input + '%'	

	RETURN @Input


END

GO


GRANT EXECUTE ON [dbo].[func_MassageFreeTextInput] TO HCPUBLIC
GO
