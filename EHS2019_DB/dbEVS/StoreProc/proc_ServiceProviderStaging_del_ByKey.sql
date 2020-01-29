IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStaging_del_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStaging_del_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 June 2008
-- Description:	Delete Service Provider Staging Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderStaging_del_ByKey]
	@Enrolment_Ref_No char(15),
	@tsmp timestamp,
	@checkTSMP tinyint
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF @checkTSMP  = 1 AND (SELECT TSMP FROM ServiceProviderStaging
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

DELETE FROM [dbo].[ServiceProviderStaging]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No
END

GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStaging_del_ByKey] TO HCVU
GO
