IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_ProfessionalVerification_after_upd')
	DROP TRIGGER [dbo].[tri_ProfessionalVerification_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 18 Jun 2008
-- Description:	Log ProfessionVerification (After Image) To
--		ProfessionalVerificationLog
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Trigger [dbo].[tri_ProfessionalVerification_after_upd]
	ON [dbo].[ProfessionalVerification]
	AFTER INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON;

-- deleted: old values
-- inserted: new values

INSERT INTO [dbo].[ProfessionalVerificationLOG]
(
	[System_Dtm],
	[Enrolment_Ref_No], [Professional_Seq], [SP_ID],
	[Export_By], [Export_Dtm], [Import_By], [Import_Dtm],
	[Verification_Result], [Verification_Remark], [Final_Result],
	[Confirm_By], [Confirm_Dtm], [Void_By], [Void_Dtm],
	[Defer_By], [Defer_Dtm],
	[Record_Status]
)
(
	SELECT
		GetDate(),
		[Enrolment_Ref_No], [Professional_Seq], [SP_ID],
		[Export_By], [Export_Dtm], [Import_By], [Import_Dtm],
		[Verification_Result], [Verification_Remark], [Final_Result],
		[Confirm_By], [Confirm_Dtm], [Void_By], [Void_Dtm],
		[Defer_By], [Defer_Dtm],
		[Record_Status]
	FROM inserted
)

END	

GO
