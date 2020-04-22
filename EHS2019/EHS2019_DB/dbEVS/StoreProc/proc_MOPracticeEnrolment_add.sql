IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOPracticeEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOPracticeEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 17 August 2008
-- Description:	Insert the Medical Organization Practice to Table
--				MOPracticeEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MOPracticeEnrolment_add]
	@enrolment_ref_no char(15), @display_seq smallint, @practice_name_chi nvarchar(100), 
	@phone_daytime	varchar(20), @practice_type_remark nvarchar(255)
AS
BEGIN

	SET NOCOUNT ON;

  INSERT INTO MOPracticeEnrolment
				(Enrolment_Ref_No,
				Display_Seq,
				Practice_Name_Chi,
				Phone_Daytime,
				Practice_Type_Remark)
	VALUES		(@enrolment_ref_no,
				@display_seq,
				@practice_name_chi,
				@phone_daytime,
				@practice_type_remark)
END
GO

GRANT EXECUTE ON [dbo].[proc_MOPracticeEnrolment_add] TO HCPUBLIC
GO
