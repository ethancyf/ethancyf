IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationStaging_del_All]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationStaging_del_All]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 29 Jun 2009
-- Description:	Delete All MedicalOrganizationStagin Record of the given ERN
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganizationStaging_del_All]
	@Enrolment_Ref_No char(15)
	
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

DELETE FROM [dbo].[MedicalOrganizationStaging]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No 
END

GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationStaging_del_All] TO HCVU
GO
