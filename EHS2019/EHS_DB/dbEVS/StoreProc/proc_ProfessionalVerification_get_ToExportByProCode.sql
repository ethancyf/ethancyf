IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_get_ToExportByProCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_get_ToExportByProCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tommy TSE
-- Modified date:	9 Sep 2011
-- CR No.:			CRE11-024-01 (Enhancement on HCVS Extension Part 1)
-- Description:		Profession related data is
--					retrieved from table [profession]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 May 2008
-- Description:	Retrieve Professional Verify Record 
--			    To Be Export By Profession Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_get_ToExportByProCode]
	@Profession_Code char(5)
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

EXEC [proc_SymmetricKey_open]
	
-- =============================================
-- Return results
-- =============================================

-- To Retrieve Professional Verify Record to Be Export By Profession Code
-- 1). Waiting for Verify Profession: [SPAccountUpdate].Progress_Status = 'P' 
-- 2). Not Yet Export : [ProfessionalVerification].Export_By, Export_Dtm, Import_By, Import_Dtm = Null

-- [The Logic of Check Record Exist in <proc_ProfessionalSubmissionHeader_add> is the same as proc_ProfessionalVerification_get_ToExportByProCode]

SELECT

	PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID,
	PS.Service_Category_Code, PS.Registration_Code, 
	[profession].[Service_Category_Desc] as Profession_Description,
	--SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm
	convert(char,DecryptByKey(SPS.Encrypt_Field1)) as SP_HKID ,
	convert(varchar(40),DecryptByKey(SPS.Encrypt_Field2)) as SP_Eng_Name ,
	convert(nvarchar,DecryptByKey(SPS.Encrypt_Field3)) as SP_Chi_Name ,
	PV.TSMP
	
FROM 

	[dbo].[ProfessionalVerification] PV 

		INNER JOIN [dbo].[ProfessionalStaging] PS
			ON PV.Enrolment_Ref_No = PS.Enrolment_Ref_No AND PV.Professional_Seq = PS.Professional_Seq

		INNER JOIN [dbo].[SPAccountUpdate] SPAU 
			ON PV.Enrolment_Ref_No = SPAU.Enrolment_Ref_No

		LEFT OUTER JOIN [dbo].[ServiceProviderStaging] SPS
			ON PV.Enrolment_Ref_No = SPS.Enrolment_Ref_No 

		LEFT OUTER JOIN [dbo].[profession]
			ON [profession].[Service_Category_Code] = PS.Service_Category_Code
			
WHERE 
	PV.Export_By Is NULL AND PV.Export_Dtm IS NULL AND
	PV.Import_By Is Null AND PV.Import_Dtm Is NULL AND
	SPAU.Progress_Status = 'P' AND PS.Service_Category_Code = @Profession_Code

ORDER BY SPS.Enrolment_Dtm ASC

	EXEC [proc_SymmetricKey_close]
	
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_get_ToExportByProCode] TO HCVU
GO
