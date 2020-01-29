IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderVerification_upd_PracticeBank]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_PracticeBank]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 28 May 2008
-- Description:	Update the BAnk_Acc_Confirmed in table
--				"ServicProviderVerification"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	15 July 2009
-- Description:		Also Need to update Scheme Confirmed
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_PracticeBank]
	@enrolment_ref_no	char(15), @practice_confirmed char(1), 
	@bank_acc_confirmed char(1), 
	@scheme_confirmed char(1),
	@update_by varchar(20), @tsmp timestamp
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
	SET		Practice_Confirmed = @practice_confirmed,
			Bank_Acc_Confirmed = @bank_acc_confirmed,
			scheme_confirmed = @scheme_confirmed,
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderVerification_upd_PracticeBank] TO HCVU
GO
