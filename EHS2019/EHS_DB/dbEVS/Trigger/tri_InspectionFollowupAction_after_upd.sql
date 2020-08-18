IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_InspectionFollowupAction_after_upd')
	DROP TRIGGER [dbo].[tri_InspectionFollowupAction_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	24 July 2020
-- CR No.:			CRE19-022 (Inspection Module)
-- Description:		Trigger for InspectionFollowupAction
-- =============================================   

CREATE TRIGGER [dbo].[tri_InspectionFollowupAction_after_upd]
   ON		[dbo].[InspectionFollowupAction]
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

    INSERT INTO InspectionFollowupActionLOG (
		System_Dtm,
		Inspection_ID,
		Followup_Action_Seq,
		Action_Date,
		Action_Desc
	)
	SELECT
		GETDATE(),
		Inspection_ID,
		Followup_Action_Seq,
		Action_Date,
		Action_Desc
	FROM
		inserted


END
GO
