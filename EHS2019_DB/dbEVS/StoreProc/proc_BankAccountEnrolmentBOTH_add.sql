IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountEnrolmentBOTH_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountEnrolmentBOTH_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Service Provider to Table
--				ServiceProviderEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankAccountEnrolmentBOTH_add]
	@enrolment_ref_no char(15), @display_seq smallint, @sp_practice_display_seq smallint,
	@br_code varchar(50), @bank_name nvarchar(100), @branch_name nvarchar(100), 
	@bank_account_no varchar(30), @bank_acc_holder nvarchar(100), @scheme char(5)
	
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
	if @scheme = 'IVSS'
	BEGIN
		exec dbIVSS..proc_BankAccountEnrolment_add @enrolment_ref_no, @display_seq, @sp_practice_display_seq,
												   @br_code, @bank_name, @branch_name, @bank_account_no, @bank_acc_holder
	END
	ELSE IF @scheme = 'EHCVS'
	BEGIN
		exec dbEVS..proc_BankAccountEnrolment_add @enrolment_ref_no, @display_seq, @sp_practice_display_seq,
												   @br_code, @bank_name, @branch_name, @bank_account_no, @bank_acc_holder
	END
	ELSE IF @scheme = 'BOTH'
	BEGIN
		exec dbIVSS..proc_BankAccountEnrolment_add @enrolment_ref_no, @display_seq, @sp_practice_display_seq,
												   @br_code, @bank_name, @branch_name, @bank_account_no, @bank_acc_holder

		exec dbEVS..proc_BankAccountEnrolment_add @enrolment_ref_no, @display_seq, @sp_practice_display_seq,
												   @br_code, @bank_name, @branch_name, @bank_account_no, @bank_acc_holder

	END

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountEnrolmentBOTH_add] TO HCPUBLIC
GO
