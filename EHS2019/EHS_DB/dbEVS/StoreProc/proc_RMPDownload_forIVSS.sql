IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RMPDownload_forIVSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RMPDownload_forIVSS]
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
-- Author:		Pak Ho LEE
-- Create date: 26 Jun 2008
-- Description:	Retrieve RMP Professional for IVSS Download
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_RMPDownload_forIVSS]	
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

SELECT

	--PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID,
	--PS.Service_Category_Code, 
	PS.Registration_Code as [Registration Code],
	--SD.Data_Value as Profession_Description,
	--SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm
	--convert(char,DecryptByKey(SPS.Encrypt_Field1)) as SP_HKID ,
	convert(varchar(40),DecryptByKey(SPS.Encrypt_Field2)) as [English Name],
	
	IsNull(Status.Status_Description,'') as [Enrolment Status],
	 
	CASE WHEN SPAU.Progress_Status = 'P' And Verification_result IS NOT NULL THEN
		IsNull(Status_Result.Status_Description,'')
	ELSE '' END as [Professional Registration Verification Result],
	
	CASE WHEN SPAU.Progress_Status = 'P' AND Verification_result IS NOT NULL AND Verification_result <> '' THEN
		CASE WHEN PV.Record_Status = 'D' THEN 'Defer'
			WHEN PV.Record_Status = 'I' THEN 'Active' END
	Else '' END as [Professional Registration Verification Record Status]
	--convert(nvarchar,DecryptByKey(SPS.Encrypt_Field3)) as SP_Chi_Name ,
	--PV.TSMP
	
FROM 

	[dbo].[ProfessionalVerification] PV 

		INNER JOIN [dbo].[ProfessionalStaging] PS
			ON PV.Enrolment_Ref_No = PS.Enrolment_Ref_No AND PV.Professional_Seq = PS.Professional_Seq

		INNER JOIN [dbo].[SPAccountUpdate] SPAU 
			ON PV.Enrolment_Ref_No = SPAU.Enrolment_Ref_No

		LEFT OUTER JOIN [dbo].[ServiceProviderStaging] SPS
			ON PV.Enrolment_Ref_No = SPS.Enrolment_Ref_No 

		LEFT OUTER JOIN [dbo].[StaticData] SD
			ON SD.Column_Name = 'PROFESSION' AND SD.Item_No = PS.Service_Category_Code
			
		LEFT OUTER JOIN [dbo].[StatusData] Status
			ON Status.Enum_Class = 'SPAccountUpdateProgressStatus' AND Status.Status_Value = SPAU.Progress_Status
			
		LEFT OUTER JOIN [dbo].[StatusData] Status_Result
			ON Status_Result.Enum_Class = 'ProfVRRecordResultCat' AND Status_Result.Status_Value = Verification_result

WHERE 
	--PV.Export_By Is NOT NULL AND PV.Export_Dtm IS NOT NULL AND
	--PV.Import_By Is NOT Null AND PV.Import_Dtm Is NOT Null AND
	--( Verification_result = 'Y' OR ( Verification_result ='S' AND PV.Record_Status = 'D') ) AND		
	--SPAU.Progress_Status = 'P' AND
	PS.Service_Category_Code = 'RMP'

ORDER BY SPS.Enrolment_Dtm ASC

	EXEC [proc_SymmetricKey_close]
	
END

GO

GRANT EXECUTE ON [dbo].[proc_RMPDownload_forIVSS] TO HCVU
GO
