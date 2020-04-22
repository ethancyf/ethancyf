IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOPractice_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOPractice_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 16 June 2009
-- Description:	Get the record in
--				Table MOPractice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_MOPractice_get]	
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

	SELECT  distinct Enrolment_ref_no, Practice_Name_Chi, Phone_Daytime	
	FROM    MOPractice
	WHERE	enrolment_ref_no=@enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_MOPractice_get] TO HCVU
GO
