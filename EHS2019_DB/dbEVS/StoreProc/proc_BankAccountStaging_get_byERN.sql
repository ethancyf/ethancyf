IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountStaging_get_byERN]
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
-- Author:		Kathy LEE
-- Create date: 25 May 2008
-- Description:	Retrieve the Bank Information from Table
--				PracticeSTtging
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 12 May 2009
-- Description:	  Not get the BR_Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankAccountStaging_get_byERN]
	@enrolment_ref_no	char(15)
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
	SELECT	Enrolment_Ref_No, Display_Seq, SP_Practice_Display_Seq, SP_ID,
			--BR_Code, 
			Bank_Name, Branch_Name, Bank_Account_No, Bank_Acc_Holder,
			Record_Status, Remark, Submission_Method, Create_Dtm, 
			Create_By, Update_Dtm, Update_By, TSMP, IsFreeTextFormat
	FROM	BankAccountStaging
	WHERE	Enrolment_Ref_No = @enrolment_ref_no


END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountStaging_get_byERN] TO HCVU
GO
