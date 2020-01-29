IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderStagingRowCount_byDataEntryStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderStagingRowCount_byDataEntryStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 25 June 2008
-- Description:	Get the no of record in "ServiceProvdierStaging"
--				which is in Data Entry Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderStagingRowCount_byDataEntryStatus]
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
	SELECT	COUNT(1)
	FROM	ServiceProviderStaging
	WHERE	Enrolment_Ref_No not in
			(SELECT	Enrolment_Ref_No FROM SPAccountUpdate)
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderStagingRowCount_byDataEntryStatus] TO HCVU
GO
