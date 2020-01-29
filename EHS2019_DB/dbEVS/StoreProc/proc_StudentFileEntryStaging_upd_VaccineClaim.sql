IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_VaccineClaim]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_VaccineClaim]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	20 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Update claimed transaction information in StudentFileEntryVaccineStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_VaccineClaim]
	@Student_File_ID				VARCHAR(15)	,
	@Student_Seq					INT,
	@Transaction_ID					CHAR(20),
	@Transaction_Result				VARCHAR(1000)
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

	UPDATE StudentFileEntryStaging SET Transaction_ID = @Transaction_ID,
									 Transaction_Result = @Transaction_Result, 
									 Vaccination_Process_Stage = 'CLAIMED',
									 Vaccination_Process_Stage_Dtm = CONVERT(DATE, GETDATE())
	WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_VaccineClaim] TO HCVU
GO
