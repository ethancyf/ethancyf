IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_ProfessionalStaging_after_upd')
	DROP TRIGGER [dbo].[tri_ProfessionalStaging_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into ProfessionalStagingLOG
--				when a row is updated / inserted into ProfessionalStaging
-- =============================================
CREATE TRIGGER [dbo].[tri_ProfessionalStaging_after_upd]
   ON  [dbo].[ProfessionalStaging]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO ProfessionalStagingLOG
	(System_Dtm,
		Enrolment_Ref_No, 
		Professional_Seq, 
		SP_ID, 
		Service_Category_Code, 
		Registration_Code,
		Record_Status,
		Create_Dtm, 
		Create_By)
	SELECT getdate(),
		Enrolment_Ref_No, 
		Professional_Seq, 
		SP_ID, 
		Service_Category_Code, 
		Registration_Code,
		Record_Status,
		Create_Dtm, 
		Create_By
	FROM inserted

END
GO
