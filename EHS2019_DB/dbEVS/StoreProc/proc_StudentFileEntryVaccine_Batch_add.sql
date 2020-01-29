IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryVaccine_Batch_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryVaccine_Batch_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:		
-- CR No.:				
-- Description:			
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM		
-- Modified date:	10 Sep 2019
-- CR No.			CRE19-001
-- Description:		Create Batch File
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryVaccine_Batch_add]
	@Original_Student_File_ID		VARCHAR(15)
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

	INSERT INTO StudentFileEntryVaccine (
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
		Practice_Name_Chi,
		Create_Dtm
	)
	SELECT 
		SFE.Student_File_ID,
		SFE.Student_Seq,
		SFEV.Provider,
		SFEV.Vaccine_Seq,
		SFEV.Scheme_Code,
		SFEV.Scheme_Seq,
		SFEV.Subsidize_Code,
		SFEV.Subsidize_Item_Code,
		SFEV.Subsidize_Desc,
		SFEV.Subsidize_Desc_Chi,
		SFEV.ForBar,
		SFEV.Available_Item_Code,
		SFEV.Available_Item_Desc,
		SFEV.Available_Item_Desc_Chi,
		SFEV.Service_Receive_Dtm,
		SFEV.Record_Type,
		SFEV.Is_Unknown_Vaccine,
		SFEV.Practice_Name,
		SFEV.Practice_Name_Chi,
		SFEV.Create_Dtm
	FROM
		StudentFileEntry SFE
			INNER JOIN StudentFileEntryVaccine SFEV
				ON SFE.Original_Student_File_ID = SFEV.Student_File_ID
				AND SFE.Original_Student_Seq = SFEV.Student_Seq

	WHERE 
		SFE.Original_Student_File_ID = @Original_Student_File_ID
		
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryVaccine_Batch_add] TO HCSP
GO

