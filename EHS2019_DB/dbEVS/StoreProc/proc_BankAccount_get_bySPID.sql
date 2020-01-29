IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccount_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccount_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE13-019-02
-- Modified by:		Winnie SUEN
-- Modified date:	19 Dec 2014
-- Description:		Add "IsFreeTextFormat"
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 12 June 2008
-- Description:	Retrieve Bank Account Information from Table
--				"BankAccount"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	28 May 2009
-- Description:		Removw BR_Code, Effective_DTm and Delist_dtm
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankAccount_get_bySPID]
	@sp_id	char(8)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	SELECT	Display_Seq, SP_Practice_Display_Seq, SP_ID,
			Bank_Name, Branch_Name, Bank_Account_No, Bank_Acc_Holder,
			Record_Status, Remark, Submission_Method, Create_Dtm, 
			Create_By, Update_Dtm, Update_By, TSMP, IsFreeTextFormat
	FROM	BankAccount
	WHERE	SP_ID = @sp_id
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccount_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_BankAccount_get_bySPID] TO HCVU
GO
