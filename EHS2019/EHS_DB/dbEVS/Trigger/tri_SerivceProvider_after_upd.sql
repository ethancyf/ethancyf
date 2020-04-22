 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SerivceProvider_after_upd')
	BEGIN
		DROP  Trigger [dbo].[tri_SerivceProvider_after_upd]
	END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	17 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Add [PCD_Account_Status], [PCD_Enrolment_Status],[PCD_Professional],[PCD_Status_Last_Check_Dtm] column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Marco CHOI
-- Modified date:	13 Jun 2017
-- CR No.			I-CRE17-007-02
-- Description:		Add field [Activation_Code_Level]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	12 Feb 2016
-- CR No.			CRE15-019
-- Description:		Rename PPI-ePR to eHRSS
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	19 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Tentative_Email_Input_By]
--									[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
--									[ServiceProviderLOG].[Tentative_Email_Input_By]
--									[ServiceProviderLOG].[Data_Input_By]
--									[ServiceProviderLOG].[Data_Input_Effective_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	29 July 2009
-- Description:		Include information 'UnderModification' in this tigger
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 12 May 2009
-- Description:	  Remove Delist_Status, Effective_Dtm, Delist_Dtm,
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into ServiceProviderLOG
--				when a row is updated / inserted into ServiceProvider
-- =============================================

CREATE TRIGGER [dbo].[tri_SerivceProvider_after_upd]
   ON  [dbo].[ServiceProvider]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO ServiceProviderLOG
	(System_Dtm,
		SP_ID,
		Enrolment_Ref_No,
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
		Tentative_Email,
		Activation_Code,
		Record_Status,
		Remark,
		Submission_Method,
		Already_Joined_EHR,
		Join_EHR,
		UnderModification,
		Enrolment_Dtm,
		Application_Printed,
		Effective_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Token_Return_dtm,
		Tentative_Email_Input_By,
		Data_Input_By,
		Data_Input_Effective_Dtm,
		Activation_Code_Level,
		PCD_Account_Status,
		PCD_Enrolment_Status,
		PCD_Professional,
		PCD_Status_Last_Check_Dtm
)
	SELECT getdate(),
		SP_ID,
		Enrolment_Ref_No,
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
		Tentative_Email,
		Activation_Code,
		Record_Status,
		Remark,
		Submission_Method,
		Already_Joined_EHR,
		Join_EHR,
		UnderModification,
		Enrolment_Dtm,
		Application_Printed,
		Effective_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Token_Return_dtm,
		Tentative_Email_Input_By,
		Data_Input_By,
		Data_Input_Effective_Dtm,
		Activation_Code_Level,
		PCD_Account_Status,
		PCD_Enrolment_Status,
		PCD_Professional,
		PCD_Status_Last_Check_Dtm
	FROM inserted

END

GO

