IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryVaccineStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryVaccineStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	20 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Delete StudentFileEntryVaccineStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryVaccineStaging_del]
	@Student_File_ID				VARCHAR(15)		,
	@Student_Seq					INT,
	@Provider						VARCHAR(100)
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

	DELETE FROM StudentFileEntryVaccineStaging WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq	AND Provider = @Provider

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryVaccineStaging_del] TO HCVU
GO
