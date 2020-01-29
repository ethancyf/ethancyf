IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_get_forVaccineEntitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_get_forVaccineEntitle]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
	SELECT DISTINCT H.Student_File_ID, 1 AS [Priority], Service_Receive_Dtm AS [Priority_Date]
	FROM StudentFileheader H
		INNER JOIN StudentFileEntry E
		ON H.Student_File_ID = E.Student_File_ID
	WHERE H.Record_Status = 'FR'
		AND H.Final_Checking_Report_Generation_Date <= @Check_Dtm
		--AND E.Vaccination_Checking_Status = 'P'
		--AND E.Acc_Type IS NOT NULL
	ORDER BY [Priority], [Priority_Date]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_get_forVaccineEntitle] TO HCVU
GO
