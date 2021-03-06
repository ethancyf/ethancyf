IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountStaging_add]
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
-- Modification History
-- CR No.:		  CRE13-019-02
-- Modified by:	  Winnie SUEN
-- Modified date: 19 Dec 2014
-- Description:	  Add "IsFreeTextFormat"
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 22 May 2008
-- Description:	Insert the Bank Account from Table
--				BankAccountStaging
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 08 May 2009
-- Description:	  Remove the BR Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankAccountStaging_add]
	@enrolment_ref_no			CHAR(15), 
	@display_seq				SMALLINT, 
	@sp_id						CHAR(8), 
	@sp_practice_display_seq	SMALLINT,
	@bank_name					NVARCHAR(100), 
	@branch_name				NVARCHAR(100), 
	@bank_account_no			VARCHAR(30), 
	@bank_acc_holder			NVARCHAR(300), 
	@record_status				CHAR(1),
	@remark						NVARCHAR(255), 
	@submission_method			CHAR(1), 
	@create_by					VARCHAR(20), 
	@update_by					VARCHAR(20),
	@isfreetextformat			CHAR(1)
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
declare @rowcount int

SELECT	@rowcount = count(1)       
FROM	BankAccountStaging
WHERE	Enrolment_Ref_No = @enrolment_ref_no and Display_Seq  = @display_seq and SP_Practice_Display_Seq = @sp_practice_display_seq
		
	IF @rowcount > 0
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@ERROR
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	
	INSERT INTO BankAccountStaging(
		Enrolment_Ref_No,
		Display_Seq,
		SP_ID,
		SP_Practice_Display_Seq,
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
	VALUES(
		@enrolment_ref_no,
		@display_seq,
		@sp_id,
		@sp_practice_display_seq,
		@bank_name,
		@branch_name,
		@bank_account_no,
		@bank_acc_holder,
		@record_status,
		@remark,
		@submission_method,
		GETDATE(),
		@create_by,
		GETDATE(),
		@update_by,
		@isfreetextformat)

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountStaging_add] TO HCVU
GO

