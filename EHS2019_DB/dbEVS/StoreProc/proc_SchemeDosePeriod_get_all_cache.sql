IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeDosePeriod_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeDosePeriod_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modified by:		Kathy LEE
-- Modified date:	8 Jul 2010
-- Description:		Grant Right to HCVU
-- =============================================
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:	18 Nov 2009
-- Description:	Retrieve the Dose Period for Eligibilty Checking
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeDosePeriod_get_all_cache] 
AS
BEGIN

	SET NOCOUNT ON;
	-- =============================================
	-- Declaration
	-- =============================================
	-- =============================================
	-- Validation 
	-- =============================================
	-- =============================================
	-- Initialization
	-- =============================================
	-- =============================================
	-- Return results
	-- =============================================
	
	SELECT 
		[Scheme_Code],
		[Scheme_Seq],
		[Subsidize_Code],
		[Period_Seq],
		[Dose_name],
		[From_dtm],
		[To_dtm],
		[Record_Status]
	FROM 
		[SchemeDosePeriod]
	WHERE
		[Record_Status] = 'A'
	ORDER BY 
		[Scheme_Code], [Scheme_Seq], [Period_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDosePeriod_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDosePeriod_get_all_cache] TO HCVU
GO
