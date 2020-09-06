IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_StudentFileEntryMMRFieldStaging_after_upd')
	DROP TRIGGER [dbo].[tri_StudentFileEntryMMRFieldStaging_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	15 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		New trigger for table StudentFileEntryMMRField
-- =============================================

CREATE TRIGGER [dbo].[tri_StudentFileEntryMMRFieldStaging_after_upd]
   ON		[dbo].[StudentFileEntryMMRFieldStaging]
   AFTER	INSERT, UPDATE
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

    INSERT INTO StudentFileEntryMMRFieldStagingLOG (
		System_Dtm,
		Student_File_ID,
		Student_Seq,
		Non_immune_to_measles,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number
	)
	SELECT
		GETDATE(),
		Student_File_ID,
		Student_Seq,
		Non_immune_to_measles,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number
	FROM
		inserted


END
GO

