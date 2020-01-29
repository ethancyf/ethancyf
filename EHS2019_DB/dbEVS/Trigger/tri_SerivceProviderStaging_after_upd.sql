IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SerivceProviderStaging_after_upd')
	DROP TRIGGER [dbo].[tri_SerivceProviderStaging_after_upd]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	03 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Add [Join_PCD] column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	12 Feb 2016
-- CR No.			CRE15-019
-- Description:		Rename PPI-ePR to eHRSS
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into ServiceProviderStagingLOG
--				when a row is updated / inserted into ServiceProviderStaging
-- =============================================

CREATE TRIGGER [dbo].[tri_SerivceProviderStaging_after_upd]
   ON  [dbo].[ServiceProviderStaging]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO ServiceProviderStagingLOG
	(System_Dtm,
		Enrolment_Ref_No,
		SP_ID,
--		SP_HKID,
--		SP_Eng_Name, 
--		SP_Chi_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi, 
		District,
		Address_Code, 
		Phone_Daytime,
		Fax,
		Email, 
		Email_Changed, 
		Record_Status,
		Remark,
		Submission_Method, 
		Already_Joined_EHR,
		Join_EHR, 
		Enrolment_Dtm, 
		Application_Printed, 
		Create_Dtm, 
		Create_By,
		Update_Dtm,
		Update_By,
		Encrypt_Field1, 
		Encrypt_Field2,
		Encrypt_Field3,
		Join_PCD)
	SELECT getdate(),
		Enrolment_Ref_No,
		SP_ID,
--		SP_HKID,
--		SP_Eng_Name, 
--		SP_Chi_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi, 
		District,
		Address_Code, 
		Phone_Daytime,
		Fax,
		Email, 
		Email_Changed, 
		Record_Status,
		Remark,
		Submission_Method, 
		Already_Joined_EHR,
		Join_EHR, 
		Enrolment_Dtm, 
		Application_Printed, 
		Create_Dtm, 
		Create_By,
		Update_Dtm,
		Update_By,
		Encrypt_Field1, 
		Encrypt_Field2,
		Encrypt_Field3,
		Join_PCD
	FROM inserted

END
GO
