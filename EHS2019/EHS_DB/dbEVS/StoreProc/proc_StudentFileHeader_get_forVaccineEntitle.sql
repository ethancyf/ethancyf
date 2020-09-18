IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_get_forVaccineEntitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_get_forVaccineEntitle]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	24 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Retrieve student file of 2nd Vaccination date for generate report
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	20 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns (HKICSymbol, Service_Receive_Dtm)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	12 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileHeaderStaging for vaccination checking
--					1. Final report generation (sort by service date)
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_get_forVaccineEntitle]
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
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], H.Service_Receive_Dtm AS [Priority_Date], 1 AS [Visit]
	FROM StudentFileheader H
		INNER JOIN StudentFileEntry E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'FR'
		AND H.Final_Checking_Report_Generation_Date <= @Check_Dtm
		--AND E.Vaccination_Checking_Status = 'P'
		--AND E.Acc_Type IS NOT NULL
	UNION
	-- 1st Priority
	--		Reach the date for 2nd final report generation
	--		Process earliest service date first
	SELECT DISTINCT 
		H.Student_File_ID, 
		1 AS [Priority], 
		H.Service_Receive_Dtm_2 AS [Priority_Date],
		2 AS Visit
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

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_get_forVaccineEntitle] TO HCVU
GO

