IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_ProfessionalSubmissionHeader_after_upd')
	DROP TRIGGER [dbo].[tri_ProfessionalSubmissionHeader_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE Trigger [dbo].[tri_ProfessionalSubmissionHeader_after_upd]
	ON [dbo] .[ProfessionalSubmissionHeader]
	
	AFTER UPDATE, INSERT
AS
BEGIN
	SET NOCOUNT ON;

-- deleted: old values
-- inserted: new values

INSERT INTO [dbo].[ProfessionalSubmissHeaderLOG]
(
	[System_Dtm],
	[File_Name],
	[Export_Dtm],
	[Export_By],
	[Service_Category_Code],
	[Import_Dtm],
	[Import_By],
	[Record_Status]
)
(
	SELECT
		GetDate(),
		[File_Name],
		[Export_Dtm],
		[Export_By],
		[Service_Category_Code],
		[Import_Dtm],
		[Import_By],
		[Record_Status]
	FROM inserted

)

END 

GO
