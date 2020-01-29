IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerificationRowCount_ToExport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerificationRowCount_ToExport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 25 Jun 2008
-- Description:	Retrieve Professional Verify Record 
--			    To Be Export Count
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerificationRowCount_ToExport]
AS
BEGIN
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


-- To Retrieve Professional Verify Record to Be Export
-- 1). Waiting for Verify Profession: [SPAccountUpdate].Progress_Status = 'P' 
-- 2). Not Yet Export : [ProfessionalVerification].Export_By, Export_Dtm, Import_By, Import_Dtm = Null

SELECT Count(1)

FROM 
	[dbo].[ProfessionalVerification] PV
	
		INNER JOIN [dbo].[ProfessionalStaging] PS
			ON PV.Enrolment_Ref_No = PS.Enrolment_Ref_No AND PV.Professional_Seq = PS.Professional_Seq

		INNER JOIN [dbo].[SPAccountUpdate] SPAU 
			ON PV.Enrolment_Ref_No = SPAU.Enrolment_Ref_No
WHERE	
	PV.Export_By Is NULL AND PV.Export_Dtm IS NULL AND
	PV.Import_By Is Null AND PV.Import_Dtm Is NULL AND
	SPAU.Progress_Status = 'P'

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerificationRowCount_ToExport] TO HCVU
GO
