IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderVerification_upd_Reject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_Reject]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 30 May 2008
-- Description:	Update the Void details in table
--				"ServicProviderVerification"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Pak Ho LEE
-- Modified date: 19 Jun 2008
-- Description:	Add Void Dtm as Paramter
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderVerification_upd_Reject]
		@enrolment_ref_no char(15),
		@update_by varchar(20),
		@void_by varchar(20),
		@void_dtm datetime,
		@record_status char(1),
		@tsmp timestamp
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
			Void_By = @void_by,
			Void_Dtm = @void_dtm,
			Record_Status = @record_status
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderVerification_upd_Reject] TO HCVU
GO
