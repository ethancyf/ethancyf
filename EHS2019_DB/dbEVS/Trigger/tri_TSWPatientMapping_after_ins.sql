IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_TSWPatientMapping_after_ins')
	DROP TRIGGER [dbo].[tri_TSWPatientMapping_after_ins] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 2 Dec 2008
-- Description:	Move TSWPatientMapping  to TSWPatientMapping Log (After Image)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Trigger [dbo].[tri_TSWPatientMapping_after_ins]
	ON [dbo].[TSWPatientMapping]
	AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;

-- deleted: old values
-- inserted: new values

INSERT INTO [dbo].[TSWPatientMappingLOG]
(
	[System_Dtm],
	[GP_Registration_Code],
	[GP_SPID],
	[Encrypt_Field1]
)
(
	SELECT
		GetDate(),
		[GP_Registration_Code],
		[GP_SPID],
		[Encrypt_Field1]		
	FROM inserted	
)

END 
GO
