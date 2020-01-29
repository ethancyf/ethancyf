IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccVerification_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccVerification_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 01 June 2008
-- Description:	Insert the Bank Account Verification Information
--				to Table BankAccVerification
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccVerification_add]
		@enrolment_ref_no char(15), @display_seq smallint, @sp_practice_Display_Seq smallint, @sp_id char(8),
		@record_status	char(1)
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

	INSERT INTO	BankAccVerification
				(Enrolment_Ref_No,
				 Display_Seq,
				 sp_practice_Display_Seq,
				 SP_ID,
				 Record_Status)
	VALUES		(@enrolment_ref_no,
				 @display_seq,
				 @sp_practice_Display_Seq,
				 @sp_id,
				 @record_status)

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccVerification_add] TO HCVU
GO
