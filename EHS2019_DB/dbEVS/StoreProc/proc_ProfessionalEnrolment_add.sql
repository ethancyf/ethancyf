IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Professional to Table
--				ProfessionalEnrolment
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalEnrolment_add]
	@enrolment_ref_no char(15), @professional_seq smallint, @service_category_code char(5),
	@registration_code varchar(15)
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

	INSERT INTO ProfessionalEnrolment
				(Enrolment_Ref_No,				
				 Professional_Seq,
				 Service_Category_Code,
				 Registration_Code)
	VALUES		(@enrolment_ref_no,				
				 @professional_seq,
				 @service_category_code,
				 @registration_code)

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalEnrolment_add] TO HCPUBLIC
GO
