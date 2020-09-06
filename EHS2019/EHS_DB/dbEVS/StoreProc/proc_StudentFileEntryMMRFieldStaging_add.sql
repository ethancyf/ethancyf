IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryMMRFieldStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryMMRFieldStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	15 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryMMRFieldStaging_add]
	@Student_File_ID				VARCHAR(15),
	@Student_Seq					INT,
	@Non_immune_to_measles			VARCHAR(2),
	@Ethnicity						VARCHAR(100),
	@Category1						VARCHAR(100),
	@Category2						VARCHAR(100),
	@Lot_Number						VARCHAR(15)
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

	INSERT INTO StudentFileEntryMMRFieldStaging (
		Student_File_ID,
		Student_Seq,
		Non_immune_to_measles,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number

	) VALUES (
		@Student_File_ID,
		@Student_Seq,
		@Non_immune_to_measles,
		@Ethnicity,
		@Category1,
		@Category2,
		@Lot_Number
	)
	
	CLOSE SYMMETRIC KEY sym_Key
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryMMRFieldStaging_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryMMRFieldStaging_add] TO HCSP
GO

