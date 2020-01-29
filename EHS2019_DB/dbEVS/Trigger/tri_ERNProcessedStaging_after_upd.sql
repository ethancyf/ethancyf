IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_ERNProcessedStaging_after_upd')
	DROP TRIGGER [dbo].[tri_ERNProcessedStaging_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 08 August 2009
-- Description:	Trigger an insert statment into ERNProcessedStagingLOG
--				when a row is updated / inserted into ERNProcessedStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:
-- =============================================
Create TRIGGER [dbo].[tri_ERNProcessedStaging_after_upd]
   ON  [dbo].[ERNProcessedStaging]
   AFTER INSERT,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO ERNProcessedStagingLOG
				(System_Dtm,
				Enrolment_Ref_No,
				SP_ID,
				Create_Dtm,
				Create_By,
				Sub_Enrolment_Ref_No

)
	SELECT		getdate(),
				Enrolment_Ref_No,
				SP_ID,
				Create_Dtm,
				Create_By,
				Sub_Enrolment_Ref_No
	FROM inserted

END
GO
