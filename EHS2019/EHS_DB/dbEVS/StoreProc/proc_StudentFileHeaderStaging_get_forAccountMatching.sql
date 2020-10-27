IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_get_forAccountMatching]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forAccountMatching]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Sep 2020
-- CR No.:			CRE20-003 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Get StudentFileHeader for Account Matching
-- =============================================      

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forAccountMatching]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Check_Dtm AS DATETIME
	SELECT @Check_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	--		Processing upload / Processing rectify / Processing Claim
	--		Process earliest Final_Checking_Report_Generation_Date first
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], Final_Checking_Report_Generation_Date AS [Priority_Date]
	FROM StudentFileheaderStaging H
	WHERE H.Record_Status IN ('PU','PR','PT')
	ORDER BY 
		[Priority], [Priority_Date]
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_get_forAccountMatching] TO HCVU
GO
