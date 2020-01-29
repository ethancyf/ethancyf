IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationMigration_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationMigration_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 16 June 2009
-- Description:	Get the record in
--				Table MedicalOrganizationMigration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganizationMigration_get_byERN]	
	@enrolment_ref_no	char(15)	
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

	SELECT  Enrolment_Ref_No,
			SP_ID,
			Display_Seq, 			
			MO_Eng_Name, 
			MO_Chi_Name, 
			Room,
			[Floor],
			Block, 
			Building, 
			Building_Chi, 
			District, 
			Address_Code, 
			BR_Code, 
			Phone_Daytime, 
			Email, 
			Fax,
			Relationship,
			Relationship_Remark,			
			Record_Status
	FROM    MedicalOrganizationMigration
	WHERE	enrolment_ref_no = @enrolment_ref_no
		
END

GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationMigration_get_byERN] TO HCVU
GO
