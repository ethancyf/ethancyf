IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by		Chris YIM
-- Modified date	27 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description		Enlarge column size (Class_Name)
-- =============================================
-- =============================================
-- Modification History
-- Modified by		Koala CHENG
-- Modified date	31 Sep 2019
-- CR No.			CRE19-001-04 (RVP Precheck)
-- Description		Add StudentFileEntrySubsidizePrecheckStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add]
	@Student_File_ID		VARCHAR(15),
	@Student_Seq			INT,
	@Class_Name				NVARCHAR(40),
	@Scheme_Code			CHAR(10),
	@Scheme_Seq				SMALLINT,
	@Subsidize_Code			CHAR(10),
	@Entitle_ONLYDOSE		CHAR(1),
	@Entitle_1STDOSE		CHAR(1),
	@Entitle_2NDDOSE		CHAR(1),
	@Remark_ONLYDOSE		VARCHAR(1000),
	@Remark_1STDOSE			VARCHAR(1000),
	@Remark_2NDDOSE			VARCHAR(1000),
	@Entitle_Inject_Fail_Reason	VARCHAR(1000)
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
	
	INSERT INTO StudentFileEntrySubsidizePrecheckStaging (
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Entitle_ONLYDOSE,
		Entitle_1STDOSE,
		Entitle_2NDDOSE,
		Remark_ONLYDOSE,
		Remark_1STDOSE,
		Remark_2NDDOSE,
		Entitle_Inject_Fail_Reason,
		Inject_ONLYDOSE_1STDOSE,
		Inject_2NDDOSE,
		Create_Dtm
	) VALUES (
		@Student_File_ID,
		@Student_Seq,
		@Class_Name,
		@Scheme_Code,
		@Scheme_Seq,
		@Subsidize_Code,
		@Entitle_ONLYDOSE,
		@Entitle_1STDOSE,
		@Entitle_2NDDOSE,
		@Remark_ONLYDOSE,
		@Remark_1STDOSE,
		@Remark_2NDDOSE,
		@Entitle_Inject_Fail_Reason,
		NULL,
		NULL,
		GETDATE()
	)
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add] TO HCVU
GO

