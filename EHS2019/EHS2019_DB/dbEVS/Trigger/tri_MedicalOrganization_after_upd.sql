IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_MedicalOrganization_after_upd')
	DROP TRIGGER [dbo].[tri_MedicalOrganization_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 17 Sep 2009
-- Description:	Trigger an insert statment into MedicalOrganizationLog
--				when a row is updated / inserted into MedicalOrganization
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:  
-- Description:	   
-- =============================================
CREATE TRIGGER [dbo].[tri_MedicalOrganization_after_upd]
   ON  [dbo].[MedicalOrganization]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO MedicalOrganizationLOG
	(System_Dtm,
		SP_ID,
		Display_Seq,
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
		Relationship_Remark,
		Record_Status,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By)
	SELECT getdate(),
		SP_ID,
		Display_Seq,
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
		Relationship_Remark,
		Record_Status,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By
	FROM inserted

END
GO
