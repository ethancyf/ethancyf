IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountVerification_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountVerification_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:			Clark Yip
-- Create date:		5 May 2008
-- Description:		Bank Account Verification
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_BankAccountVerification_update] @enrolment_ref		 char(15)
							,@display_seq			smallint	
							,@sp_practice_Display_Seq smallint												
							,@record_status			char(1)	
							,@verified_by			varchar(20)
							,@verified_dtm			datetime
							,@tsmp					timestamp					
as
BEGIN
-- =============================================
-- Declaration
-- =============================================

-- =============================================
-- Validation 
-- =============================================
IF (SELECT TSMP FROM BankAccVerification
		WHERE Enrolment_Ref_No = @enrolment_ref AND display_seq = @display_seq AND sp_practice_Display_Seq = @sp_practice_Display_Seq) != @tsmp
	BEGIN
		RAISERROR('00006', 16, 1)
		return @@error
	END

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================

--select @display_seq = display_seq from BankAccountStaging 
--where [Enrolment_Ref_No]=@enrolment_ref AND
--	   [Bank_Account_No] = @bank_acc

UPDATE [BankAccVerification]
   SET [Record_status] = @record_status, [Verify_By] = @verified_by, [Verify_Dtm]=@verified_dtm
 WHERE [Enrolment_Ref_No]=@enrolment_ref 
		AND @display_seq = display_seq
		AND sp_practice_Display_Seq = @sp_practice_Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountVerification_update] TO HCVU
GO
