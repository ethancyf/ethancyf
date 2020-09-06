IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_VaccineCheck]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_VaccineCheck]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	29 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns (Entitle_3RDDOSE)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	20 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Update vaccine entitlement in StudentFileEntryVaccineStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_VaccineCheck]
	@Student_File_ID				VARCHAR(15),
	@Student_Seq					INT,
	@Entitle_ONLYDOSE				CHAR(1),
	@Entitle_1STDOSE				CHAR(1),
	@Entitle_2NDDOSE				CHAR(1),
	@Entitle_3RDDOSE				CHAR(1),
	@Entitle_Inject					CHAR(1),
	@Entitle_Inject_Fail_Reason		VARCHAR(1000)
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

	UPDATE StudentFileEntryStaging SET Entitle_ONLYDOSE = @Entitle_ONLYDOSE,
									Entitle_1STDOSE = @Entitle_1STDOSE,
									Entitle_2NDDOSE = @Entitle_2NDDOSE,
									Entitle_3RDDOSE = @Entitle_3RDDOSE,
									Entitle_Inject = @Entitle_Inject,
									Entitle_Inject_Fail_Reason = @Entitle_Inject_Fail_Reason,
									Vaccination_Process_Stage = 'CALENTITLE',
									Vaccination_Process_Stage_Dtm = CONVERT(DATE, GETDATE())
	WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_VaccineCheck] TO HCVU
GO

