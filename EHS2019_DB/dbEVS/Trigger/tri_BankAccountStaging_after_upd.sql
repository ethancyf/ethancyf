IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_BankAccountStaging_after_upd')
	DROP TRIGGER [dbo].[tri_BankAccountStaging_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:		  CRE13-019-02
-- Modified by:	  Winnie SUEN
-- Modified date: 19 Dec 2014
-- Description:	  Add "IsFreeTextFormat"
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 17 July 2008
-- Description:	Trigger an insert statment into BankAccountStagingLOG
--				when a row is updated / inserted into BankAccountStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 13 May 2009
-- Description:	  Remove the BR Code
-- =============================================

CREATE TRIGGER [dbo].[tri_BankAccountStaging_after_upd]
   ON  [dbo].[BankAccountStaging]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO BankAccountStagingLOG
	(System_Dtm,
		Enrolment_Ref_No, 
		Display_Seq, 
		SP_ID, 
		SP_Practice_Display_Seq,
		--BR_Code,
		Bank_Name, 
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder, 
		Record_Status, 
		Remark,
		Submission_Method,
		Create_Dtm, 
		Create_By, 
		Update_Dtm,
		Update_By,
		IsFreeTextFormat)
	SELECT getdate(),
		Enrolment_Ref_No, 
		Display_Seq, 
		SP_ID, 
		SP_Practice_Display_Seq,
		--BR_Code,
		Bank_Name, 
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder, 
		Record_Status, 
		Remark,
		Submission_Method,
		Create_Dtm, 
		Create_By, 
		Update_Dtm,
		Update_By,
		IsFreeTextFormat
	FROM inserted
END
GO
