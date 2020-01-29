IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderEnrolmentBatch_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderEnrolmentBatch_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 18 August 2009
-- Description:	Get the Batch_ID in ServiceProviderEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProviderEnrolmentBatch_get_byERN]
	@enrolment_ref_no char(15)
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
	
	select enrolment_ref_no from serviceproviderenrolment
	where batch_id in (select Batch_id from serviceproviderenrolment
	where enrolment_ref_no = @enrolment_ref_no)

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolmentBatch_get_byERN] TO HCPUBLIC
GO
