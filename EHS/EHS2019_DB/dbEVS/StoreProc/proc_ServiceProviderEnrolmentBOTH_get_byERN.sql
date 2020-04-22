IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderEnrolmentBOTH_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderEnrolmentBOTH_get_byERN]
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

CREATE PROCEDURE [dbo].[proc_ServiceProviderEnrolmentBOTH_get_byERN]
	@enrolment_ref_no	char(15), @scheme char(5)
	
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
		exec dbIVSS..proc_ServiceProviderEnrolment_get_byERN @enrolment_ref_no
	END
	ELSE 
	BEGIN
		exec dbEVS..proc_ServiceProviderEnrolment_get_byERN @enrolment_ref_no
	END

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolmentBOTH_get_byERN] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolmentBOTH_get_byERN] TO HCVU
GO
