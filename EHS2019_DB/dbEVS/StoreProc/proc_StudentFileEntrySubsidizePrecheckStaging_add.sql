IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	31 Sep 2019
-- CR No.:			CRE19-001-04 (RVP Precheck)
-- Description:		Add StudentFileEntrySubsidizePrecheckStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheckStaging_add]
	@Student_File_ID		varchar(15),
	@Student_Seq			int,
	@Class_Name				nvarchar(10),
	@Scheme_Code			char(10),
	@Scheme_Seq				smallint,
	@Subsidize_Code			char(10),
	@Entitle_ONLYDOSE		char(1),
	@Entitle_1STDOSE		char(1),
	@Entitle_2NDDOSE		char(1),
	@Remark_ONLYDOSE		varchar(1000),
	@Remark_1STDOSE			varchar(1000),
	@Remark_2NDDOSE			varchar(1000),
	@Entitle_Inject_Fail_Reason	varchar(1000)
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
