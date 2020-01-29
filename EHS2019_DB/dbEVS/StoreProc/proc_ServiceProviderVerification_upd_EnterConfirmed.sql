IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderVerification_upd_EnterConfirmed]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_EnterConfirmed]
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
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_EnterConfirmed]
	@enrolment_ref_no char(15), @update_by varchar(20), 
	@enter_confirm_by varchar(20), @record_status char(1), @tsmp timestamp
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
			Enter_Confirm_By = @enter_confirm_by,
			Enter_Confirm_Dtm = getdate(),
			Record_Status = @record_status
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderVerification_upd_EnterConfirmed] TO HCVU
GO
