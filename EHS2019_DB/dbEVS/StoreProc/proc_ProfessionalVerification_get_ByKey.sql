IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_get_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_get_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 May 2008
-- Description:	Retrieve Professional Verification Record
--		With Valid Status [SPAccountUpdate].Progess_Status = 'P'
--		By Enrolment_Ref_No & Professional_Seq
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_get_ByKey]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint
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

-- To Retrieve [dbo].[ProfessionalVerification] Valid Status
-- 1). [SPAccountUpdate].Progess_Status = 'P'

SELECT

	PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID,
	PV.Export_By, PV.Export_Dtm,
	PV.Import_By, PV.Import_Dtm,
	PV.Verification_Result, PV.Verification_Remark, PV.Final_Result,
	PV.Confirm_By, PV.Confirm_Dtm, 
	PV.Void_By, PV.Void_Dtm,
	PV.Defer_By, PV.Defer_Dtm,
	PV.Record_Status, PV.TSMP

FROM 
	[dbo].[ProfessionalVerification] PV 
		INNER JOIN [dbo].[SPAccountUpdate] SPAU 
			ON PV.Enrolment_Ref_No = SPAU.Enrolment_Ref_No
WHERE
	PV.Enrolment_Ref_No = @Enrolment_Ref_No AND
	PV.Professional_Seq = @Professional_Seq AND
	SPAU.Progress_Status = 'P'

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_get_ByKey] TO HCVU
GO
