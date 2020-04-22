IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_BankAccVerification_after_upd')
	DROP TRIGGER [dbo].[tri_BankAccVerification_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 27 Jun 2008
-- Description:	Log BankAccVerification (After Image) To
--		BankAccVerificationLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE TRIGGER [dbo].[tri_BankAccVerification_after_upd]
	ON [dbo].[BankAccVerification]
	AFTER INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON;

-- deleted: old values
-- inserted: new values

INSERT INTO [dbo].[BankAccVerificationLOG]
(
	[System_Dtm],
	[Enrolment_Ref_No], [Display_Seq], [SP_Practice_Display_Seq], [SP_ID],
	[Verify_By], [Verify_Dtm], [Void_By], [Void_Dtm], [Defer_By], [Defer_Dtm],
	[Record_Status]

)

(
	SELECT
		GetDate(),
		[Enrolment_Ref_No], [Display_Seq], [SP_Practice_Display_Seq], [SP_ID],
		[Verify_By], [Verify_Dtm], [Void_By], [Void_Dtm], [Defer_By], [Defer_Dtm],
		[Record_Status]

	FROM inserted
)

END
	/*insert into */

GO
