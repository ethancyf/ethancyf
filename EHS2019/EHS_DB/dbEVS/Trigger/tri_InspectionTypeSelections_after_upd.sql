IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_InspectionTypeSelections_after_upd')
	DROP TRIGGER [dbo].[tri_InspectionTypeSelections_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	24 July 2020
-- CR No.:			CRE19-022 (Inspection Module)
-- Description:		Trigger for InspectionTypeSelections
-- =============================================   

CREATE TRIGGER [dbo].[tri_InspectionTypeSelections_after_upd]
   ON		[dbo].[InspectionTypeSelections]
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

    INSERT INTO InspectionTypeSelectionsLOG (
		System_Dtm,
		Inspection_ID,
		Type_Of_Inspection
	)
	SELECT
		GETDATE(),
		Inspection_ID,
		Type_Of_Inspection
	FROM
		inserted


END
GO
