IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderVerification_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderVerification_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 13 June 2008
-- Description:	Delete Service Provider Verification Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderVerification_del]
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

	IF @checkTSMP = 1  AND (SELECT TSMP FROM ServiceProviderVerification
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

DELETE FROM [dbo].[ServiceProviderVerification]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No
END

GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderVerification_del] TO HCVU
GO
