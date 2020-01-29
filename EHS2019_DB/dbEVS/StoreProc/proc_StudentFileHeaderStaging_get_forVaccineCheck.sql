IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_get_forVaccineCheck]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forVaccineCheck]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	30 Oct 2018
-- CR No.:			CRE18-011 (Check vaccination record of students with rectified information in rectification file)
-- Description:		Reset rectify file too
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	26 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileHeaderStaging for vaccination checking
--					1. Final report generation (sort by service date)
--					2. Processing claim creation (sort by service date)
--					3. Processing upload student (sort by final report generation date)
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get_forVaccineCheck]
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
	--		Reach the date for final report generation
	--		Process earliest service date first
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], Service_Receive_Dtm AS [Priority_Date]
	FROM StudentFileheaderStaging H
		INNER JOIN StudentFileEntryStaging E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'FR'
		AND H.Final_Checking_Report_Generation_Date <= @Check_Dtm
	UNION
	-- 2nd Priority
	--		Processing claim creation 
	--		Process earliest service date first
	SELECT DISTINCT H.Student_File_ID, 2 AS [Priority], Service_Receive_Dtm AS [Priority_Date]
	FROM StudentFileheaderStaging H
		INNER JOIN StudentFileEntryStaging E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'PT'
	UNION
	-- 3rd Priority
	--		Processing upload
	--		Process earliest Final_Checking_Report_Generation_Date first
	SELECT DISTINCT H.Student_File_ID, 3 AS [Priority], Final_Checking_Report_Generation_Date AS [Priority_Date]
	FROM StudentFileheaderStaging H
		INNER JOIN StudentFileEntryStaging E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'PU'
	UNION
	-- 4th Priority
	--		Processing rectify
	--		Process earliest Final_Checking_Report_Generation_Date first
	SELECT DISTINCT H.Student_File_ID, 4 AS [Priority], Final_Checking_Report_Generation_Date AS [Priority_Date]
	FROM StudentFileheaderStaging H
		INNER JOIN StudentFileEntryStaging E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'PR'
	ORDER BY [Priority],  [Priority_Date]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_get_forVaccineCheck] TO HCVU
GO
