IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_get_byERN_toDisplay]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_get_byERN_toDisplay]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
-- Create date: 6 June 2008
-- Description:	Retrieve Professional Verification Records
--		By Enrolment_Ref_No For Display
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_get_byERN_toDisplay]
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

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
-- =============================================
-- Return results
-- =============================================

SELECT
	PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
	PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
	PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status,

	PS.Service_Category_Code, PS.Registration_Code, 
	[profession].[Service_Category_Desc] as Profession_Description,
	--SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm,
	convert(char,DecryptByKey(SPS.Encrypt_Field1)) as SP_HKID ,
	convert(varchar(40),DecryptByKey(SPS.Encrypt_Field2)) as SP_Eng_Name ,
	convert(nvarchar,DecryptByKey(SPS.Encrypt_Field3)) as SP_Chi_Name ,
	SPS.Enrolment_Dtm,

	Submission.File_Name
	--PSH.File_Name

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

		LEFT OUTER JOIN 
		(

			SELECT     ProSub.Reference_No, ProSubHead.File_Name
			FROM         ProfessionalSubmission AS ProSub INNER JOIN
								  ProfessionalSubmissionHeader AS ProSubHead ON ProSub.File_Name = ProSubHead.File_Name

			WHERE     EXISTS 
			(
				SELECT Grouped.Reference_No, Grouped.Export_Dtm
				FROM
				(
					SELECT     ProSub3.Reference_No, Max(ProSubHead3.Export_Dtm) as Export_Dtm
					FROM          ProfessionalSubmission AS ProSub3 INNER JOIN
					ProfessionalSubmissionHeader AS ProSubHead3 ON ProSub3.File_Name = ProSubHead3.File_Name
					WHERE ProSubHead3.Record_Status = 'A'
					GROUP BY ProSub3.Reference_No
				) AS Grouped
				WHERE Grouped.Reference_No = ProSub.Reference_No AND Grouped.Export_Dtm = ProSubHead.Export_Dtm
			)

		) Submission
			ON Submission.Reference_No = PV.Enrolment_Ref_No + RIGHT('00000' + CONVERT(Varchar, PV.Professional_Seq), 5)


--		LEFT OUTER JOIN 
--		(
--			SELECT
--				ProSub.Reference_No, ProSub.File_Name
--			FROM [dbo].[ProfessionalSubmission] ProSub
--				INNER JOIN [dbo].[ProfessionalSubmissionHeader] AS ProSubHead
--					ON ProSub.File_Name = ProSubHead.File_Name AND ProSubHead.Record_Status ='A'
--		) AS PSH
--			ON PSH.Reference_No = PV.Enrolment_Ref_No + RIGHT('00000' + CONVERT(Varchar, PV.Professional_Seq), 5)

WHERE
	PV.Enrolment_Ref_No = @Enrolment_Ref_No AND SPAU.Progress_Status = 'P'
	
	CLOSE SYMMETRIC KEY sym_Key
	
END



GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_get_byERN_toDisplay] TO HCVU
GO
