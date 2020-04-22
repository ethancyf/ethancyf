IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderVerification_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderVerification_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Retrieve the Service Provider Verification details
--				from Table ServiceProviderVerification
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 2 Jun 2009
-- Description:	Retrieve two information MO_Confirmed and Scheme_Confirmed
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProviderVerification_get_byERN]
	@enrolment_ref_no	char(15)
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
	SELECT	Enrolment_Ref_No, SP_ID, Update_By, Update_Dtm, 
			SP_Confirmed, MO_Confirmed, Practice_Confirmed, Bank_Acc_Confirmed,
			Scheme_Confirmed, Enter_Confirm_By, Enter_Confirm_Dtm, Vetting_By, Vetting_Dtm,
			Void_By, Void_Dtm, Defer_By, Defer_Dtm, Return_for_Amend_By, 
			Return_for_Amend_Dtm, Record_Status, TSMP
	FROM	ServiceProviderVerification
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderVerification_get_byERN] TO HCVU
GO
