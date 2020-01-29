IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_subsidizeColumn_map_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_subsidizeColumn_map_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mattie LO
-- Create date: 30 Sep 2009
-- Description:	 Retrieve the fee column name and subsidize code mapping
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSD_get_subsidizeColumn_map_cache] 
AS
BEGIN

	SELECT [fee_column]
		  ,[subsidize_code]
	  FROM [dbo].[SDFeeColumnMapping]
	ORDER BY [fee_column]

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_subsidizeColumn_map_cache] TO HCPUBLIC
GO
