IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	13 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	16 Jul 2019
-- CR No.			CRE19-001 (PPP)
-- Description:		1. Retrieve new column
--					2. Handle RCH
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	09 Jul 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileHeaderStaging
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_get]
	@Student_File_ID	varchar(15),
	@Record_Status		varchar(2)
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

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	--

	SELECT
		'S' AS [Table_Location],
		S.Student_File_ID,
		S.Upload_Precheck,
		S.School_Code,
		ISNULL(SH.Name_Eng, RH.Homename_Eng) AS [School_Name_Eng],
		ISNULL(SH.Name_Chi, RH.Homename_Chi) AS [School_Name_Chi],
		ISNULL(SH.Address_Eng, RH.Address_Eng) AS [School_Address_Eng],
		ISNULL(SH.Address_Chi, RH.Address_Chi) AS [School_Address_Chi],
		S.SP_ID,
		CONVERT(varchar(MAX), DECRYPTBYKEY(SP.Encrypt_Field2)) AS [SP_Name_Eng],
		CONVERT(nvarchar(MAX), DECRYPTBYKEY(SP.Encrypt_Field3)) AS [SP_Name_Chi],
		S.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		S.Scheme_Code,
		SC.Display_Code as [Scheme_Display_Code],
		S.Scheme_Seq,
		S.Subsidize_Code,
		SGC.Display_Code_For_Claim,
		-- If RVP + QIV THEN display 'QIV 20XX/XX' Else display 'QIV-C 2018/19','23vPPV'
		CASE WHEN S.Scheme_Code = 'RVP' AND SUB.vaccine_Type = 'QIV' THEN
				RTRIM(SUB.vaccine_Type) + ' ' +  RTRIM(VS.Season_Desc)
			ELSE
				sgc.Display_Code_For_Claim
			END AS SubsidizeDisplayName,
		S.Dose,
		S.Service_Receive_Dtm,
		S.Final_Checking_Report_Generation_Date,
		S.Service_Receive_Dtm_2ndDose,
		S.Final_Checking_Report_Generation_Date_2ndDose,
		S.Service_Receive_Dtm_2,
		S.Final_Checking_Report_Generation_Date_2,
		S.Service_Receive_Dtm_2ndDose_2,
		S.Final_Checking_Report_Generation_Date_2ndDose_2,
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
		S.Request_Remove_By,
		S.Request_Remove_Dtm,
		S.Request_Remove_Function,
		S.Confirm_Remove_By,
		S.Confirm_Remove_Dtm,
		S.Request_Claim_Reactivate_By,
		S.Request_Claim_Reactivate_Dtm,
		S.Confirm_Claim_Reactivate_By,
		S.Confirm_Claim_Reactivate_Dtm,
		S.Name_List_File_ID,
		S.Vaccination_Report_File_ID,
		S.Onsite_Vaccination_File_ID,
		S.Vaccination_Report_File_ID_2,
		S.Onsite_Vaccination_File_ID_2,
		S.Claim_Creation_Report_File_ID,
		S.Rectification_File_ID,
		S.Update_By,
		S.Update_Dtm,
		S.TSMP,
		S.Original_Student_File_ID,
		S.Request_Rectify_Status
	FROM
		StudentFileHeaderStaging S
			INNER JOIN ServiceProvider SP
				ON S.SP_ID = SP.SP_ID
			INNER JOIN Practice P
				ON S.SP_ID = P.SP_ID
					AND S.Practice_Display_Seq = P.Display_Seq
			INNER JOIN SchemeClaim SC
				ON S.Scheme_Code = SC.Scheme_Code
			LEFT JOIN SubsidizeGroupClaim SGC
				ON S.Scheme_Code = SGC.Scheme_Code AND S.Scheme_Seq = SGC.Scheme_Seq AND S.Subsidize_Code = SGC.Subsidize_Code
			LEFT JOIN Subsidize SUB
				ON S.Subsidize_Code = SUB.Subsidize_Code
			LEFT JOIN VaccineSeason VS
				ON VS.Scheme_Code = S.Scheme_Code
					AND VS.Scheme_Seq = S.Scheme_Seq
					AND VS.Subsidize_Item_Code = SUB.Subsidize_Item_Code
			LEFT JOIN School SH
				ON S.School_Code = SH.School_Code
			LEFT JOIN RVPHomeList RH
				ON S.School_Code = RH.RCH_code

	WHERE
		(@Student_File_ID IS NULL OR S.Student_File_ID = @Student_File_ID)
			AND (@Record_Status IS NULL OR S.Record_Status = @Record_Status)
	ORDER BY
		S.Student_File_ID
	
	--
	
	CLOSE SYMMETRIC KEY sym_Key
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_get] TO HCVU
GO

