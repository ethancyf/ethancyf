IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_mask_doc_id]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_mask_doc_id]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Vincent
-- Create date:	27 JAN 2010
-- Description:	Mask Document Number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
CREATE FUNCTION [dbo].[func_mask_doc_id]
(
	@doc_code varchar(10),
	@doc_id1	varchar(20),
	@doc_id2	varchar(20)
)
RETURNS varchar(20)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @mask char
	DECLARE @temp_trim_id varchar(20)
	DECLARE @result varchar(20)

	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	SET @mask = 'X'
	SET @result = ''

	IF @doc_code = 'ADOPC'
	BEGIN

		SET @result = LEFT(@doc_id1, 4) + REPLICATE(@mask, 3) + '/' + LEFT(@doc_id2, 2) + REPLICATE(@mask, 3)

	END
	ELSE IF @doc_code = 'Doc/I'
	BEGIN

		SET @result = LEFT(@doc_id1, 4) + REPLICATE(@mask, 5)

	END
	ELSE IF @doc_code = 'EC' OR @doc_code = 'HKBC' OR @doc_code = 'HKIC'
	BEGIN

		SET @temp_trim_id = LTRIM(RTRIM(@doc_id1))
		IF LEN(@temp_trim_id) = 9 
			SET @result = LEFT(@temp_trim_id, 5) + REPLICATE(@mask, 3) + '(' + @mask + ')'
		ELSE
			SET @result = LEFT(@temp_trim_id, 4) + REPLICATE(@mask, 3) + '(' + @mask + ')'

	END
	ELSE IF @doc_code = 'ID235B'
	BEGIN

		SET @result = LEFT(@doc_id1, 5) + REPLICATE(@mask, 3)

	END
	ELSE IF @doc_code = 'REPMT'
	BEGIN

		SET @result = LEFT(@doc_id1, 5) + REPLICATE(@mask, 4)

	END
	ELSE IF @doc_code = 'VISA'
	BEGIN

		SET @result = LEFT(@doc_id1, 9) + REPLICATE(@mask, 5)
		SET @result = SUBSTRING(@result, 1, 4) + '-' + SUBSTRING(@result, 5, 7) + '-' + SUBSTRING(@result, 12, 2) + '(' + SUBSTRING(@result, 14, 1) + ')'

	END


	-- =============================================
	-- Return results
	-- =============================================
	RETURN @result

END
GO


Grant execute on [dbo].[func_mask_doc_id] to HCSP
GO

Grant execute on [dbo].[func_mask_doc_id] to HCVU
GO

Grant execute on [dbo].[func_mask_doc_id] to HCPUBLIC
GO 