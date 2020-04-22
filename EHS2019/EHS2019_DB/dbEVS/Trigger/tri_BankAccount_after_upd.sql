IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_BankAccount_after_upd')
	DROP TRIGGER [dbo].[tri_BankAccount_after_upd]
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
-- Description:	Trigger an insert statment into BankAccountLOG
--				when a row is updated / inserted into BankAccount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 13 May 2009
-- Description:	  Remove the BR Code, Effective Dtm, Delist Dtm
-- =============================================

CREATE TRIGGER [dbo].[tri_BankAccount_after_upd]
   ON  [dbo].[BankAccount]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO BankAccountLOG
	(System_Dtm,
		SP_ID,
		Display_Seq,
		SP_Practice_Display_Seq,
		--BR_Code,
		Bank_Name,
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder,
		Record_Status,
		Remark,
		Submission_Method,
		--Effective_Dtm,
		--Delist_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		IsFreeTextFormat)
	SELECT getdate(),
		SP_ID,
		Display_Seq,
		SP_Practice_Display_Seq,
		--BR_Code,
		Bank_Name,
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder,
		Record_Status,
		Remark,
		Submission_Method,
		--Effective_Dtm,
		--Delist_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		IsFreeTextFormat
	FROM inserted

END
GO
