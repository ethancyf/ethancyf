IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_eligibleServiceProf_map_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_eligibleServiceProf_map_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mattie LO
-- Create date: 30 August 2009
-- Description:	 Retrieve the eligible service and Professional mapping
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSD_get_eligibleServiceProf_map_cache] 
AS
BEGIN

	SELECT distinct [service_category_code_SD]
					,[eligible_service]
	  FROM [dbo].[SDProfessionalMapping]

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_eligibleServiceProf_map_cache] TO HCPUBLIC
GO
