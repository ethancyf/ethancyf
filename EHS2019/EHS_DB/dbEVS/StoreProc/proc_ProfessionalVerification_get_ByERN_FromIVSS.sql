IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_get_ByERN_FromIVSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_get_ByERN_FromIVSS]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 10 Jul 2009
-- Description:	Retrieve Professional Verification Record
--		By Enrolment_Ref_No From IVSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_get_ByERN_FromIVSS]
	@Enrolment_Ref_No char(15)
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
	[dbIVSS].[dbo].[ProfessionalVerification] PV 		
WHERE
	PV.Enrolment_Ref_No = @Enrolment_Ref_No

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_get_ByERN_FromIVSS] TO HCVU
GO
