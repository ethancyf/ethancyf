IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubProfession_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubProfession_get_all_cache]
GO

/****** Object:  StoredProcedure [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache]    Script Date: 06/25/2010 10:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:		
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala Cheng
-- Create date: 13 Jan 2012
-- Description:	Retrieve all sub profession for profession, e.g. RCM -> RCMP
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubProfession_get_all_cache] 
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
		[Service_Category_Code],
		[Sub_Service_Category_Code],
		[Sub_Service_Category_Desc],
		[Sub_Service_Category_Desc_Chi],
		[Display_Seq]
	FROM
		[SubProfession] WITH (NOLOCK)
	ORDER BY [Service_Category_Code], [Sub_Service_Category_Code], [Display_Seq]
END
GO

GRANT EXECUTE ON [dbo].[proc_SubProfession_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SubProfession_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SubProfession_get_all_cache] TO [HCPUBLIC]
GO