IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_CodeMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_CodeMapping_get_all_cache]
GO

/****** Object:  StoredProcedure [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache]    Script Date: 06/25/2010 10:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	17 Sep 2019
-- CR No.			CRE20-005
-- Description:		Grant EXECUTE right for role WSEXT
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala Cheng
-- Create date: 12 Jan 2012
-- Description:	Retrieve all code mapping for 3rd party, e.g. PCD mapping profession code
-- =============================================

CREATE PROCEDURE [dbo].[proc_CodeMapping_get_all_cache] 
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
		[Source_System],
		[Target_System],
		[Code_Type],
		[Code_Source],
		[Code_Target],
		[Record_Status]
	FROM
		[CodeMapping] WITH (NOLOCK)
	WHERE
		[Record_Status] = 'A'
	ORDER BY [Source_System],[Target_System],[Code_Type],[Code_Source],[Code_Target]
END
GO

GRANT EXECUTE ON [dbo].[proc_CodeMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_CodeMapping_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_CodeMapping_get_all_cache] TO [HCPUBLIC]
GO

GRANT EXECUTE ON [dbo].[proc_CodeMapping_get_all_cache] TO WSEXT
GO

