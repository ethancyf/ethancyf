IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_upd_EHRSS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_upd_EHRSS]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================        
-- Author:			Chris YIM     
-- Create date:		11 Feb 2016 
-- CR No.			CRE15-019    
-- Description:     Create new one SP "proc_ServiceProviderStaging_upd_EHRSS" From "proc_ServiceProviderStaging_upd_PPIePR".
-- =============================================  

CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_upd_EHRSS]
	@enrolment_ref_no char(15), @already_joined_EHR char(1),
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
	IF (SELECT TSMP FROM ServiceProviderStaging
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
	UPDATE	ServiceProviderStaging
	Set		Already_Joined_EHR = @already_joined_EHR,
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_upd_EHRSS] TO HCVU
GO
