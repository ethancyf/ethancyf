IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_get_ForVerify]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_get_ForVerify]
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
-- Create date: 2 June 2008
-- Description:	Retrieve Professional Verify Record For Verify
--			    
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_get_ForVerify]
	@Enrolment_Ref_No char(15),
	@SP_HKID char(9),
	@Verify_Status as char(1),
	@Record_Status as char(1)
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

-- To Retrieve Professional Verify Record to be Verify 
-- 'Y' for Valid, 'N' for Invalid, 'S' for Suspect
-- N/A for Exported and waiting for result

-- 1. Waiting for Verify Profession: [SPAccountUpdate].Progress_Status = 'P' 
-- 2. Already Export : [ProfessionalVerification].Export_By, Export_Dtm Not Null
-- 3. Either Imported Or Pending for Import
-- 3.1) Imported: Not Yet Confiremd / Reject : [ProfessionalVerification].Confirm_By, Confirmed_Dtm, Void_By, Void_Dtm, Final_Result = Null, Record Status ='I','D'
-- 3.2) Pending for Import : N/A



SELECT
	PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
	PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
	PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status, PV.TSMP,

	PS.Service_Category_Code, PS.Registration_Code, 
	[profession].[Service_Category_Desc] as Profession_Description,
	convert(char,DecryptByKey(SPS.Encrypt_Field1)) as SP_HKID ,
	convert(varchar(40),DecryptByKey(SPS.Encrypt_Field2)) as SP_Eng_Name ,
	convert(nvarchar,DecryptByKey(SPS.Encrypt_Field3)) as SP_Chi_Name ,
	SPS.Enrolment_Dtm,
	--PSH.File_Name,
	Submission.File_Name,
	SPAU.TSMP as TSMPSpAccountUpdate,

	(SELECT Count(1) as C FROM [dbo].[ProfessionalVerification] TPV
		WHERE TPV.Enrolment_Ref_No = PV.Enrolment_Ref_No ) as Count
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


		INNER JOIN 
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



--		LEFT OUTER JOIN [dbo].[ProfessionalSubmission] ProSub
--			ON ProSub.Reference_No = PV.Enrolment_Ref_No + RIGHT('00000' + CONVERT(Varchar, PV.Professional_Seq), 5)
--		LEFT OUTER JOIN [dbo].[ProfessionalSubmissionHeader] PSH
--			ON ProSub.File_Name = PSH.File_Name

WHERE 
	SPAU.Progress_Status = 'P' AND --Submission.Record_Status = 'A' AND
		
	( @SP_HKID IS NULL OR EncryptByKey(KEY_GUID('sym_Key'), @SP_HKID) = SPS.Encrypt_Field1) AND
	( @Enrolment_Ref_No IS NULL OR PV.Enrolment_Ref_No = @Enrolment_Ref_No ) AND
	
	PV.Export_By Is NOT NULL AND PV.Export_Dtm IS NOT NULL AND
	(
		(PV.Import_By Is NULL AND PV.Import_Dtm Is NULL AND PV.Record_Status = 'O') OR
		(
			PV.Import_By Is NOT Null AND PV.Import_Dtm Is Not Null AND PV.Confirm_By Is Null AND
			PV.Confirm_Dtm Is Null And PV.Void_By Is Null AND PV.Void_Dtm Is Null AND
			PV.Final_Result IS Null AND (PV.Record_Status = 'I' OR PV.Record_Status = 'D' )
		)
	) AND
	
	( Verification_result = @Verify_Status OR 
		(Verification_result IS NULL AND @Verify_Status IS NULL) ) AND
	
	( @Record_Status IS NULL OR 
		(@Record_Status = 'D' AND PV.Record_Status = 'D' ) OR 
		(@Record_Status <> 'D' AND PV.Record_Status <> 'D' ) )
	
ORDER BY Enrolment_Ref_No ASC


-- To Retrieve Professional Verify Record to be Verify
-- 'Y' for Valid, 'N' for Invalid, 'S' for Suspect
-- N/A for Exported and waiting for result
-- 1). Waiting for Verify Profession: [SPAccountUpdate].Progress_Status = 'P' 
-- 2). Already Export : [ProfessionalVerification].Export_By, Export_Dtm Not Null
-- 3). Either Imported Or Pending for Import
-- 3.1) Imported: Not Yet Confiremd / Reject : [ProfessionalVerification].Confirm_By, Confirmed_Dtm, Void_By, Void_Dtm, Final_Result = Null, Record Status ='I','D'

/*
SELECT
	PV.Enrolment_Ref_No, PV.Professional_Seq, PV.SP_ID, PV.Export_By, PV.Export_Dtm,
	PV.Import_By, PV.Import_Dtm, PV.Verification_result, PV.Verification_Remark,
	PV.Final_Result, PV.Defer_By, Defer_Dtm, PV.Record_Status, PV.TSMP,

	PS.Service_Category_Code, PS.Registration_Code, 
	SD.Data_Value as Profession_Description,
	--SPS.SP_HKID, SPS.SP_Eng_Name, SPS.SP_Chi_Name, SPS.Enrolment_Dtm,
	convert(char,DecryptByKey(SPS.Encrypt_Field1)) as SP_HKID ,
	convert(varchar(40),DecryptByKey(SPS.Encrypt_Field2)) as SP_Eng_Name ,
	convert(nvarchar,DecryptByKey(SPS.Encrypt_Field3)) as SP_Chi_Name ,
	SPS.Enrolment_Dtm,

	PSH.File_Name,
	SPAU.TSMP as TSMPSpAccountUpdate,

	(SELECT Count(1) as C FROM [dbo].[ProfessionalVerification] TPV
		WHERE TPV.Enrolment_Ref_No = PV.Enrolment_Ref_No ) as Count
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

		LEFT OUTER JOIN [dbo].[ProfessionalSubmission] ProSub
			ON ProSub.Reference_No = PV.Enrolment_Ref_No + RIGHT('00000' + CONVERT(Varchar, PV.Professional_Seq), 5)
		LEFT OUTER JOIN [dbo].[ProfessionalSubmissionHeader] PSH
			ON ProSub.File_Name = PSH.File_Name

WHERE 
	SPAU.Progress_Status = 'P' AND PSH.Record_Status = 'A' AND
	
	PV.Export_By Is NOT NULL AND PV.Export_Dtm IS NOT NULL AND
	(
		(PV.Import_By Is NULL AND PV.Import_Dtm Is NULL AND PV.Record_Status = 'O') OR
		(
			PV.Import_By Is NOT Null AND PV.Import_Dtm Is Not Null AND PV.Confirm_By Is Null AND
			PV.Confirm_Dtm Is Null And PV.Void_By Is Null AND PV.Void_Dtm Is Null AND
			PV.Final_Result IS Null AND (PV.Record_Status = 'I' OR PV.Record_Status = 'D' )
		)
	)

ORDER BY Enrolment_Ref_No ASC
*/
EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_get_ForVerify] TO HCVU
GO
