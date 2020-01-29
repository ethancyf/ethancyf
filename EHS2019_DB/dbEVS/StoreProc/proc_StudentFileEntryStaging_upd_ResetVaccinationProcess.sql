IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_ResetVaccinationProcess]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_ResetVaccinationProcess]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG	
-- Modified date:	31 Sep 2019
-- CR No.			CRE19-001-04 (RVP Precheck)
-- Description:		After reset, clear [StudentFileEntrySubsidizePrecheckStaging] too
-- =============================================
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
-- Modified date:	03 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Reset Vaccination process stage from StudentFileEntryStaging and StudentFileEntryVaccineStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_ResetVaccinationProcess]
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
		Entitle_Inject = NULL,
		Entitle_Inject_Fail_Reason = NULL,
		Ext_Ref_Status = NULL,
		DH_Vaccine_Ref_Status = NULL
	FROM StudentFileEntryStaging E
		INNER JOIN StudentFileHeaderStaging AS H
		ON E.Student_File_ID = H.Student_File_ID 
			AND E.Vaccination_Process_Stage_Dtm < @Check_Dtm
			AND E.Transaction_ID IS NULL
			AND E.Transaction_Result IS NULL
			AND H.Record_Status IN ('PU','PR','PT','FR')
			

	-- Clear HA and DH vaccination record too
	DELETE FROM StudentFileEntryVaccineStaging
	WHERE EXISTS (SELECT Student_File_ID, Student_Seq FROM StudentFileEntryStaging E
				  WHERE E.Vaccination_Process_Stage_Dtm IS NULL
						AND StudentFileEntryVaccineStaging.Student_File_ID = E.Student_File_ID AND StudentFileEntryVaccineStaging.Student_Seq = E.Student_Seq
						AND Ext_Ref_Status IS NULL AND DH_Vaccine_Ref_Status IS NULL)
	
	-- Clear Pre-check entitle 
	DELETE FROM StudentFileEntrySubsidizePrecheckStaging
	WHERE EXISTS (SELECT Student_File_ID, Student_Seq FROM StudentFileEntryStaging E
				  WHERE E.Vaccination_Process_Stage_Dtm IS NULL
						AND StudentFileEntrySubsidizePrecheckStaging.Student_File_ID = E.Student_File_ID AND StudentFileEntrySubsidizePrecheckStaging.Student_Seq = E.Student_Seq
						AND Ext_Ref_Status IS NULL AND DH_Vaccine_Ref_Status IS NULL)
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_ResetVaccinationProcess] TO HCVU
GO
