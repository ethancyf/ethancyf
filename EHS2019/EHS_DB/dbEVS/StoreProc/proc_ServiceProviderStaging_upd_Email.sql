IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_upd_Email]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_upd_Email]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_ServiceProviderStaging_upd_Email]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
--drop procedure [dbo].[proc_ServiceProviderStaging_upd_Email]
--GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 19 Sep 2008
-- Description:	Update Service Provider Email in
--				Table "ServiceProviderStaging" 
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_upd_Email]
	@enrolment_ref_no char(15), @email varchar(255)
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


	UPDATE	ServiceProviderStaging
	SET		Email = @email,			
			Update_Dtm = getdate(),
			Update_By = SP_ID

	 WHERE Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_upd_Email] TO HCSP
GO
