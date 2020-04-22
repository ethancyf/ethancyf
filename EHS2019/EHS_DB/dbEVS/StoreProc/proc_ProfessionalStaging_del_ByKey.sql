IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalStaging_del_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalStaging_del_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 June 2008
-- Description:	Delete Professional Staging Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalStaging_del_ByKey]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint
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

DELETE FROM [dbo].[ProfessionalStaging]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No AND
	Professional_Seq = @Professional_Seq 
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalStaging_del_ByKey] TO HCVU
GO
