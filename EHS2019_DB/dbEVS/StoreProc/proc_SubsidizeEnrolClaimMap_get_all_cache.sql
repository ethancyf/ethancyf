IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeEnrolClaimMap_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeEnrolClaimMap_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 19 Nov 2009
-- Description:	Retrieve the Scheme + Subsidize Enrol <-> Scheme + Subsidize Claim Mapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:
-- Modified date:
-- Description:
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeEnrolClaimMap_get_all_cache] 
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
	[Scheme_Code_Enrol],
	[Subsidize_Code_Enrol],
	[Scheme_Code_Claim],
	[Subsidize_Code_Claim]
FROM
	[SubsidizeEnrolClaimMap]
WHERE
	[Record_Status] = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeEnrolClaimMap_get_all_cache] TO HCSP
GO
