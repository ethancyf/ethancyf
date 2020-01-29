IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 28 July 2008
-- Description:	Delete Scheme Information Staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_del]
	@Enrolment_Ref_No char(15),
	@Scheme_Code char(10)
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

DELETE FROM [dbo].[SchemeInformationStaging]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No AND Scheme_Code = @Scheme_Code 
END

GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_del] TO HCVU
GO
