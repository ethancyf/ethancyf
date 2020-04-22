IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryVaccine_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryVaccine_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	20 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Add StudentFileEntryVaccine
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryVaccine_add]
	@Student_File_ID				VARCHAR(15)		,
	@Student_Seq					INT				,
	@Vaccine_Seq					INT				,
	@Scheme_Code					CHAR(10)		,
	@Scheme_Seq						SMALLINT		,
	@Subsidize_Code					CHAR(10)		,
	@Subsidize_Item_Code			CHAR(10)		,
	@Subsidize_Desc					VARCHAR(1000)	,
	@Subsidize_Desc_Chi				NVARCHAR(200)	,
	@ForBar							BIT				,
	@Available_Item_Code			CHAR(20)		,
	@Available_Item_Desc			VARCHAR(1000)	,
	@Available_Item_Desc_Chi		NVARCHAR(100)	,
	@Provider						VARCHAR(100)	,
	@Service_Receive_Dtm			DATETIME		,
	@Record_Type					CHAR(1)			,
	@Is_Unknown_Vaccine				BIT				,
	@Practice_Name					NVARCHAR(100)	,
	@Practice_Name_Chi				NVARCHAR(100)	
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
		Provider,
		Service_Receive_Dtm,
		Record_Type,
		Is_Unknown_Vaccine,
		Practice_Name,
		Practice_Name_Chi,
		Create_Dtm
	) VALUES (
		@Student_File_ID,
		@Student_Seq,
		@Vaccine_Seq,
		@Scheme_Code,
		@Scheme_Seq,
		@Subsidize_Code,
		@Subsidize_Item_Code,
		@Subsidize_Desc,
		@Subsidize_Desc_Chi,
		@ForBar,
		@Available_Item_Code,
		@Available_Item_Desc,
		@Available_Item_Desc_Chi,
		@Provider,
		@Service_Receive_Dtm,
		@Record_Type,
		@Is_Unknown_Vaccine,
		@Practice_Name,
		@Practice_Name_Chi,
		GETDATE()
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryVaccine_add] TO HCVU
GO
