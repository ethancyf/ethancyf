IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_MedicalOrganizationStaging_after_upd')
	DROP TRIGGER [dbo].[tri_MedicalOrganizationStaging_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into MedicalOrganizationStagingLog
--				when a row is updated / inserted into MedicalOrganizationStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:  
-- Description:	   
-- =============================================
CREATE TRIGGER [dbo].[tri_MedicalOrganizationStaging_after_upd]
   ON  [dbo].[MedicalOrganizationStaging]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO MedicalOrganizationStagingLOG
	(System_Dtm,
		Enrolment_Ref_No,
		Display_Seq,
		SP_ID,
		MO_Eng_Name,
		MO_Chi_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		BR_Code, 
		Phone_Daytime, 
		Email, 
		Fax, 
		Relationship,
		Relationship_remark,
		Record_Status, 		
		Create_Dtm, 
		Create_By, 
		Update_Dtm, 
		Update_By)
	SELECT getdate(),
		Enrolment_Ref_No,
		Display_Seq,
		SP_ID,
		MO_Eng_Name,
		MO_Chi_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		BR_Code, 
		Phone_Daytime, 
		Email, 
		Fax, 
		Relationship,
		Relationship_remark,
		Record_Status, 		
		Create_Dtm, 
		Create_By, 
		Update_Dtm, 
		Update_By
	FROM inserted

END
GO
