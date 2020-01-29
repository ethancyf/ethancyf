IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_upd_JoinPCD]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_upd_JoinPCD]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================        
-- Author:			Koala CHENG     
-- Create date:		03 Jul 2018 
-- CR No.			CRE17-016    
-- Description:     Checking of PCD status during VSS enrolment
--					Update the [ServiceProviderStaging].[Join_PCD] value when there is any changes on Profession
-- =============================================  

CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_upd_JoinPCD]
	@enrolment_ref_no char(15), @join_PCD char(1),
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
	Set		Join_PCD = @join_PCD,
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_upd_JoinPCD] TO HCVU
GO
