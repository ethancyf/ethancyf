IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderVerification_upd_ReturnAmend]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_ReturnAmend]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 29 May 2008
-- Description:	Update the Enter Confirmed details in table
--				"ServicProviderVerification"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 25 May 2009 
-- Description:	  Set the MO_Confirmed, Scheme_Confirmed to null
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_ReturnAmend]
	@enrolment_ref_no char(15), @update_by varchar(20), 
	@return_for_Amend_by varchar(20), @record_status char(1), @tsmp timestamp
AS
BEGIN

	SET NOCOUNT ON;

   -- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProviderVerification
		WHERE Enrolment_Ref_No = @enrolment_ref_no) != @tsmp
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
	UPDATE	ServiceProviderVerification
	SET		Update_By = @update_by,
			Update_Dtm = getdate(),
			Return_for_Amend_By = @return_for_Amend_by,
			Return_for_Amend_Dtm = getdate(),
			Record_Status = @record_status,
			SP_Confirmed = null,
			Practice_Confirmed = null,
			Bank_Acc_Confirmed = null,
			Enter_Confirm_by = null,
			Enter_confirm_Dtm = null,
			Vetting_By = null,
			Vetting_Dtm = null,
			Void_By = null,
			Void_Dtm = null,
			Defer_By = null,
			Defer_Dtm = null,
			MO_Confirmed = null,
			Scheme_Confirmed = null			
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderVerification_upd_ReturnAmend] TO HCVU
GO
