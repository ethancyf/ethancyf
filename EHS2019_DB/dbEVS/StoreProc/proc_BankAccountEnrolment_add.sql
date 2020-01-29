IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:		  CRE17-013
-- Modified by:	  Chris YIM
-- Modified date: 27 Feb 2018
-- Description:	  Extend bank account name to 300 chars
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Bank Account from Table
--				BankAccountEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 08 May 2009
-- Description:	  Remove the BR Code
-- =============================================


CREATE PROCEDURE [dbo].[proc_BankAccountEnrolment_add]
	@enrolment_ref_no			CHAR(15), 
	@display_seq				SMALLINT, 
	@sp_practice_display_seq	SMALLINT,
	@bank_name					NVARCHAR(100), 
	@branch_name				NVARCHAR(100), 
	@bank_account_no			VARCHAR(30), 
	@bank_acc_holder			NVARCHAR(300)
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

	INSERT INTO BankAccountEnrolment(
		Enrolment_Ref_No,				
		Display_Seq,
		SP_Practice_Display_Seq,
		Bank_Name,
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder)
	VALUES(
		@enrolment_ref_no,				
		@display_seq,
		@sp_practice_display_seq,
		@bank_name,
		@branch_name,
		@bank_account_no,
		@bank_acc_holder)
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountEnrolment_add] TO HCPUBLIC
GO


