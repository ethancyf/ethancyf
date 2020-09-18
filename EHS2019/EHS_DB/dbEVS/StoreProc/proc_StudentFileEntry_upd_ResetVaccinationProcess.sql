IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_ResetVaccinationProcess]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_ResetVaccinationProcess]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	07 Sep 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns (Second Vaccination Date)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	03 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Reset Vaccination process stage from StudentFileEntry and StudentFileEntryVaccine
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_ResetVaccinationProcess]
AS BEGIN

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
	
	DECLARE @Check_Dtm AS DATETIME
	SELECT @Check_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  

	-- If the vaccination process is not done on today, clear it
	UPDATE E SET 
		Vaccination_Process_Stage = NULL,
		Vaccination_Process_Stage_Dtm = NULL,
		Entitle_ONLYDOSE = NULL,
		Entitle_1STDOSE = NULL,
		Entitle_2NDDOSE = NULL,
		Entitle_3RDDOSE = NULL,
		Entitle_Inject = NULL,
		Entitle_Inject_Fail_Reason = NULL,
		Ext_Ref_Status = NULL,
		DH_Vaccine_Ref_Status = NULL
	FROM StudentFileEntry E
		INNER JOIN StudentFileHeader AS H
		ON E.Student_File_ID = H.Student_File_ID 
			AND E.Vaccination_Process_Stage_Dtm < @Check_Dtm
			AND H.Record_Status IN ('FR')
			AND H.Final_Checking_Report_Generation_Date <= @Check_Dtm

	UPDATE E SET 
		Vaccination_Process_Stage = NULL,
		Vaccination_Process_Stage_Dtm = NULL,
		Entitle_ONLYDOSE = NULL,
		Entitle_1STDOSE = NULL,
		Entitle_2NDDOSE = NULL,
		Entitle_3RDDOSE = NULL,
		Entitle_Inject = NULL,
		Entitle_Inject_Fail_Reason = NULL,
		Ext_Ref_Status = NULL,
		DH_Vaccine_Ref_Status = NULL
	FROM 
		StudentFileEntry E
			INNER JOIN StudentFileHeader AS H
				ON E.Student_File_ID = H.Student_File_ID AND E.Vaccination_Process_Stage_Dtm < @Check_Dtm
			LEFT OUTER JOIN FileGenerationQueue FGQ
				ON H.Onsite_Vaccination_File_ID_2 = FGQ.[Generation_ID]
	WHERE
		H.Record_Status IN ('UT','ST')
		AND H.Final_Checking_Report_Generation_Date_2 IS NOT NULL
		AND H.Final_Checking_Report_Generation_Date_2 <= @Check_Dtm
		AND (H.Onsite_Vaccination_File_ID_2 IS NULL OR H.Final_Checking_Report_Generation_Date_2 > CAST(CONVERT(VARCHAR(10), FGQ.Request_Dtm, 121) AS DATETIME))

	-- Clear HA and DH vaccination record too
	DELETE FROM StudentFileEntryVaccine
	WHERE EXISTS (SELECT Student_File_ID, Student_Seq FROM StudentFileEntry E
				  WHERE E.Vaccination_Process_Stage_Dtm IS NULL
						AND StudentFileEntryVaccine.Student_File_ID = E.Student_File_ID AND StudentFileEntryVaccine.Student_Seq = E.Student_Seq
						AND Ext_Ref_Status IS NULL AND DH_Vaccine_Ref_Status IS NULL)

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_ResetVaccinationProcess] TO HCVU
GO


