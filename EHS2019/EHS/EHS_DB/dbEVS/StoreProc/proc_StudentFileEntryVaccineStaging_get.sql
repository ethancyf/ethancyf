IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryVaccineStaging_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryVaccineStaging_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	21 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileEntryVaccineStaging for vaccination checking
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryVaccineStaging_get]
	@Student_File_ID				VARCHAR(15),
	@Student_Seq					INT
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
		Student_File_ID,
		Student_Seq,
		Provider,
		Vaccine_Seq,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Subsidize_Item_Code,
		Subsidize_Desc,
		Subsidize_Desc_Chi,
		ForBar,
		Available_Item_Code,
		Available_Item_Desc,
		Available_Item_Desc_Chi,
		Service_Receive_Dtm,
		Record_Type,
		Is_Unknown_Vaccine,
		Practice_Name,
		Practice_Name_Chi
	FROM StudentFileEntryVaccineStaging WITH (NOLOCK)
	WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	ORDER BY Service_Receive_Dtm

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryVaccineStaging_get] TO HCVU
GO
