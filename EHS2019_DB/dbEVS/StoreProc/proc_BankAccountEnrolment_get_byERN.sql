IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountEnrolment_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountEnrolment_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Retrieve the Bank Information from Table
--				BankAccountEnrolment
-- =============================================

-- =============================================
-- Modification History
-- Modified by: Kathy LEE
-- Modified date: 02 Jun 2009
-- Description:	Remove BR_Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankAccountEnrolment_get_byERN] 
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
	SELECT	Enrolment_Ref_No, Display_Seq, SP_Practice_Display_Seq,
			Bank_Name, Branch_Name, Bank_Account_No,
			Bank_Acc_Holder
	FROM	BankAccountEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountEnrolment_get_byERN] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountEnrolment_get_byERN] TO HCVU
GO
