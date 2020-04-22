IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_get_forVaccineEntitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forVaccineEntitle]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	30 Oct 2018
-- CR No.:			CRE18-011 (Check vaccination record of students with rectified information in rectification file)
-- Description:		Revise to include student rectification
--					1. Processing upload student (sort by final report generation date)
--					2. Processing rectify student (sort by final report generation date)
-- =============================================    
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	12 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileHeaderStaging for vaccination checking
--					1. Final report generation (sort by service date)
--					2. Processing upload student (sort by final report generation date)
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forVaccineEntitle]
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

	-- 1st Priority
	--		Processing upload
	--		Process earliest Final_Checking_Report_Generation_Date first
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], Final_Checking_Report_Generation_Date AS [Priority_Date]
	FROM StudentFileheaderStaging H
		INNER JOIN StudentFileEntryStaging E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'PU'
	UNION
	-- 2nd Priority
	--		Processing rectify
	--		Process earliest Final_Checking_Report_Generation_Date first
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], Final_Checking_Report_Generation_Date AS [Priority_Date]
	FROM StudentFileheaderStaging H
		INNER JOIN StudentFileEntryStaging E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'PR'
	ORDER BY [Priority],  [Priority_Date]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_get_forVaccineEntitle] TO HCVU
GO
