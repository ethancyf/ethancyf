IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_ServiceProviderVerification_after_upd')
	DROP TRIGGER [dbo].[tri_ServiceProviderVerification_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into ServiceProviderVerificationLOG
--				when a row is updated / inserted into ServiceProviderVerification
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	14 August 2009
-- Description:		Add Scheme_Confirmed and MO_confirmed
-- =============================================
CREATE TRIGGER [dbo].[tri_ServiceProviderVerification_after_upd]
   ON  [dbo].[ServiceProviderVerification]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO ServiceProviderVerificationLOG
	(System_Dtm,
		Enrolment_Ref_No, 
		SP_ID, 
		Update_By,
		Update_Dtm, 
		SP_Confirmed,
		Practice_Confirmed,
		Bank_Acc_Confirmed,
		Enter_Confirm_By, 
		Enter_Confirm_Dtm, 
		Vetting_By,
		Vetting_Dtm,
		Void_By, 
		Void_Dtm, 
		Defer_By,
		Defer_Dtm, 
		Return_for_Amend_By, 
		Return_for_Amend_Dtm, 
		Record_Status,
		Scheme_Confirmed,
		MO_Confirmed)
	SELECT getdate(),
		Enrolment_Ref_No, 
		SP_ID, 
		Update_By,
		Update_Dtm, 
		SP_Confirmed,
		Practice_Confirmed,
		Bank_Acc_Confirmed,
		Enter_Confirm_By, 
		Enter_Confirm_Dtm, 
		Vetting_By,
		Vetting_Dtm,
		Void_By, 
		Void_Dtm, 
		Defer_By,
		Defer_Dtm, 
		Return_for_Amend_By, 
		Return_for_Amend_Dtm, 
		Record_Status,
		Scheme_Confirmed,
		MO_Confirmed
	FROM inserted

END
GO
