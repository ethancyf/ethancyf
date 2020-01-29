IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_PracticeStaging_after_upd')
	DROP TRIGGER [dbo].[tri_PracticeStaging_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into PracticeStagingLOG
--				when a row is updated / inserted into PracticeStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date:  08 Apr 2009
-- Description:	   Remove Practice Type, Delist Status, Delist Dtm, Effective Dtm
--				   Add Practice_Name_Chi, MO_Display_Seq, Phone_Daytime
-- =============================================
CREATE TRIGGER [dbo].[tri_PracticeStaging_after_upd]
   ON  [dbo].[PracticeStaging]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO PracticeStagingLOG
	(System_Dtm,
		Enrolment_Ref_No,
		Display_Seq,
		SP_ID,
		Practice_Name,
		Practice_Name_Chi,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Professional_Seq, 
		Record_Status, 
		--Delist_Status, 
		Remark, 
		Phone_Daytime,
		MO_Display_seq,
		Submission_Method, 
		--Effective_Dtm, 
		--Delist_Dtm, 
		Create_Dtm, 
		Create_By, 
		Update_Dtm, 
		Update_By)
	SELECT getdate(),
		Enrolment_Ref_No,
		Display_Seq,
		SP_ID,
		Practice_Name,
		Practice_Name_Chi,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Professional_Seq, 
		Record_Status, 
		--Delist_Status, 
		Remark, 
		Phone_Daytime,
		MO_Display_seq,
		Submission_Method, 
		--Effective_Dtm, 
		--Delist_Dtm, 
		Create_Dtm, 
		Create_By, 
		Update_Dtm, 
		Update_By
	FROM inserted

END
GO
