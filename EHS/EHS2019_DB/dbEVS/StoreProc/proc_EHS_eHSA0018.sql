IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0018]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0018]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 February 2011
-- Description:		eHSA0018 - FHB statistics for 2010
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0018]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting
-- =============================================
	DECLARE	@Year	int
	SET @Year = 2010


-- =============================================
-- Retrieve data
-- =============================================

-- Content

	EXEC [proc_EHS_eHSA0018-Content]

-- 01-02

	EXEC [proc_EHS_eHSA0018-01-02] @Year
	
-- 03-04

	EXEC [proc_EHS_eHSA0018-03-04] @Year

-- 05-06

	EXEC [proc_EHS_eHSA0018-05-06] @Year
	
-- 07

	EXEC [proc_EHS_eHSA0018-07] @Year

-- 08-09-10

	EXEC [proc_EHS_eHSA0018-08-09-10] @Year

-- 11

	EXEC [proc_EHS_eHSA0018-11] @Year

-- 12

	EXEC [proc_EHS_eHSA0018-12] @Year

-- Legend

	EXEC [proc_EHS_eHSA0018-Legend] @Year


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0018] TO HCVU
GO

