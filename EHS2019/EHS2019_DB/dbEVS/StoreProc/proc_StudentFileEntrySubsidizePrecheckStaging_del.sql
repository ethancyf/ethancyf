IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	31 Sep 2019
-- CR No.:			CRE19-001-04 (RVP Precheck)
-- Description:		Delete StudentFileEntrySubsidizePrecheckStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_del]
	@Student_File_ID				VARCHAR(15)		,
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

	DELETE FROM StudentFileEntrySubsidizePrecheckStaging WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_del] TO HCVU
GO
