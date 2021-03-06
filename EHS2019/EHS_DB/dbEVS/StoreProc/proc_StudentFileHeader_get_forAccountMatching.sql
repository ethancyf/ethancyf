IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_get_forAccountMatching]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_get_forAccountMatching]
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
-- Description:		Get StudentFileHeaderStaging for Account Matching
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_get_forAccountMatching]
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
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], H.Service_Receive_Dtm AS [Priority_Date]
	FROM StudentFileheader H
	WHERE H.Record_Status = 'FR'
		AND H.Final_Checking_Report_Generation_Date <= @Check_Dtm

	UNION
	-- 1st Priority
	--		Reach the date for 2nd final report generation
	--		Process earliest service date first
	SELECT DISTINCT 
		H.Student_File_ID, 
		1 AS [Priority], 
		H.Service_Receive_Dtm_2 AS [Priority_Date]
	FROM 
		StudentFileheader H
			LEFT OUTER JOIN FileGenerationQueue FGQ
				ON H.Onsite_Vaccination_File_ID_2 = FGQ.[Generation_ID]
	WHERE 
		(H.Record_Status = 'UT' OR	--Pending Upload Vaccination Claim 
		H.Record_Status = 'ST')		--Pending SP Confirmation Claim
		AND H.Final_Checking_Report_Generation_Date_2 IS NOT NULL
		AND H.Final_Checking_Report_Generation_Date_2 <= @Check_Dtm
		AND (H.Onsite_Vaccination_File_ID_2 IS NULL OR H.Final_Checking_Report_Generation_Date_2 > CAST(CONVERT(VARCHAR(10), FGQ.Request_Dtm, 121) AS DATETIME))

	ORDER BY 
		[Priority], [Priority_Date]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_get_forAccountMatching] TO HCVU
GO

