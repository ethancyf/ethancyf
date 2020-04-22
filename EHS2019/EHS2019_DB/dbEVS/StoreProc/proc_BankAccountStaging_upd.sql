IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountStaging_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountStaging_upd]
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
-- Description:	  Add @IsFreeTextFormat
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 May 2008
-- Description:	Update Bank Accoutn Infomation in
--				Table "BankAccountStaging" 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 08 May 2009
-- Description:	  Remove the BR Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankAccountStaging_upd]
	@enrolment_ref_no			CHAR(15), 
	@display_seq				SMALLINT, 
	@SP_Practice_Display_Seq	SMALLINT,
	@bank_name					NVARCHAR(100), 
	@branch_name				NVARCHAR(100),
	@bank_account_no			VARCHAR(30), 
	@bank_acc_holder			NVARCHAR(300), 
	@record_status				CHAR(1),
	@remark						NVARCHAR(255), 
	@update_By					VARCHAR(20), 
	@tsmp						TIMESTAMP,
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
	IF (SELECT TSMP FROM BankAccountStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Display_Seq = @display_seq And SP_Practice_Display_Seq = @SP_Practice_Display_Seq ) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	
		BankAccountStaging
	SET		
		Bank_Name = @bank_name, 
		Branch_Name = @branch_name,
		Bank_Account_No = @bank_account_no,
		Bank_Acc_Holder = @bank_acc_holder,
		Record_Status = @record_status,
		Remark = @remark,
		Update_Dtm = GETDATE(),
		Update_By = @update_By,
		IsFreeTextFormat = @isfreetextformat
	WHERE	
		Enrolment_Ref_No = @enrolment_ref_no AND
		Display_Seq = @display_seq AND
		SP_Practice_Display_Seq = @SP_Practice_Display_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountStaging_upd] TO HCVU
GO

