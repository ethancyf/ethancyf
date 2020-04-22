IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_VaccineRefStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_VaccineRefStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	20 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Update DH, HA vaccine ref status in StudentFileEntryVaccineStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_VaccineRefStatus]
	@Student_File_ID				VARCHAR(15)	,
	@Student_Seq					INT,
	@Provider						VARCHAR(100),
	@Vaccine_Ref_Status				VARCHAR(10)
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
	
	IF @Provider = 'HA'
		UPDATE StudentFileEntryStaging SET Ext_Ref_Status = @Vaccine_Ref_Status, 
										   Vaccination_Process_Stage = CASE WHEN DH_Vaccine_Ref_Status IS NOT NULL THEN 'GOTVACCINE' ELSE NULL END, 
										   Vaccination_Process_Stage_Dtm = CASE WHEN DH_Vaccine_Ref_Status IS NOT NULL THEN CONVERT(DATE, GETDATE()) ELSE NULL END
		WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	ELSE IF @Provider = 'DH'
		UPDATE StudentFileEntryStaging SET DH_Vaccine_Ref_Status = @Vaccine_Ref_Status, 
										   Vaccination_Process_Stage = CASE WHEN Ext_Ref_Status IS NOT NULL THEN 'GOTVACCINE' ELSE NULL END, 
										   Vaccination_Process_Stage_Dtm = CASE WHEN Ext_Ref_Status IS NOT NULL THEN CONVERT(DATE, GETDATE()) ELSE NULL END
		WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	ELSE
		BEGIN
			RAISERROR('00011', 16, 1)
			RETURN @@error
		END

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_VaccineRefStatus] TO HCVU
GO
