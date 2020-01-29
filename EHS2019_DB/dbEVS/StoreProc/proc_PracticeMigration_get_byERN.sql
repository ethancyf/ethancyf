IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeMigration_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeMigration_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 16 June 2009
-- Description:	Get the record in
--				Table PracticeMigration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 18 Jul 2009
-- Description:	  Remove Professional_seq and submission_method
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeMigration_get_byERN]	
	@enrolment_ref_no	char(15),
	@display_seq smallint
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

	SELECT 
		Enrolment_Ref_No, 
		Display_Seq, 
		SP_ID, 
		Practice_Name, 
		Practice_Name_Chi,
		Room, 
		[Floor], 
		Block, 
		Building, 
		Building_Chi, 
		District, 
		Address_Code,
		--Professional_Seq, 
		Registration_Code,
		Service_Category_Code,
		Record_Status,
		--Submission_Method,
		Phone_DayTime,
		MO_Display_Seq		
	FROM    PracticeMigration
	WHERE	enrolment_ref_no = @enrolment_ref_no and Display_seq = @display_seq
		
END

GO

GRANT EXECUTE ON [dbo].[proc_PracticeMigration_get_byERN] TO HCVU
GO
