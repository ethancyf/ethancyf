IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_search_ClaimCreation]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_search_ClaimCreation]
GO

/*
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	16 Jul 2019
-- CR No.			CRE19-001 (VSS 2019)
-- Description:		No longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	23 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Search StudentFileHeader and StudentFileHeaderStaging
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_search_ClaimCreation]
	@Student_File_ID		varchar(15),
	@School_Code			varchar(10),
	@SPID					varchar(8),
	@Scheme_Code			VARCHAR(10),
	@VaccinationDateFrom	datetime,
	@VaccinationDateTo		datetime
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

	SELECT 
		S.Student_File_ID,
		S.Upload_Precheck,
		S.School_Code,
		S.SP_ID,
		S.Practice_Display_Seq,
		S.Service_Receive_Dtm,
		S.Service_Receive_Dtm_2ndDose,
		S.Scheme_Code,
		SC.Display_Code as [Scheme_Display_Code],
		S.Scheme_Seq,
		S.Subsidize_Code,
		S.Dose,
		SGC.Display_Code_For_Claim,
		S.Final_Checking_Report_Generation_Date,
		S.Final_Checking_Report_Generation_Date_2ndDose,
		S.Remark,
		S.Record_Status,
		S.Upload_By,
		S.Upload_Dtm,
		S.Last_Rectify_By,
		S.Last_Rectify_Dtm,
		S.Claim_Upload_By,
		S.Claim_Upload_Dtm,
		S.File_Confirm_By,
		S.File_Confirm_Dtm,
		S.Vaccination_Report_File_ID,
		S.Onsite_Vaccination_File_ID,
		S.Claim_Creation_Report_File_ID,
		S.Rectification_File_ID,
		S.Update_By,
		S.Update_Dtm,
		S.TSMP
	FROM
		StudentFileHeader S
			INNER JOIN SchemeClaim SC
				ON S.Scheme_Code = SC.Scheme_Code
			LEFT JOIN SubsidizeGroupClaim SGC
				ON S.Scheme_Code = SGC.Scheme_Code AND S.Scheme_Seq = SGC.Scheme_Seq AND S.Subsidize_Code = SGC.Subsidize_Code

	WHERE
		(@Student_File_ID IS NULL OR S.Student_File_ID = @Student_File_ID)
			AND (@School_Code IS NULL OR S.School_Code = @School_Code)
			AND (@Scheme_Code IS NULL OR LTRIM(RTRIM(S.Scheme_Code)) = @Scheme_Code)
			AND (@SPID IS NULL OR S.SP_ID = @SPID)
			AND S.Record_Status in ('UT', 'CT')
			AND (@VaccinationDateFrom IS NULL OR @VaccinationDateFrom <= S.Service_Receive_Dtm)
			AND (@VaccinationDateTo IS NULL OR S.Service_Receive_Dtm <= @VaccinationDateTo)
	ORDER BY
		S.Upload_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_search_ClaimCreation] TO HCVU
GO
*/
