IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Practice to Table
--				PracticeEnrolment
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 04 April 2009
-- Description:	1. Insert 2 information in PracticeEnrolment
--				   'Practice_Name_Chi' and 'Phone_Daytime'
--				2. Remove 'Practice_type'
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeEnrolment_add] 
	@enrolment_ref_no char(15), @display_seq smallint, @mo_display_seq smallint,
	@practice_name nvarchar(100), @practice_name_chi nvarchar(100),
	@room nvarchar(5), @floor nvarchar(3), @block nvarchar(3),
	@building varchar(100), @building_chi nvarchar(100), @district char(4), 
	@professional_seq smallint, @phone_daytime varchar(20)
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

   INSERT INTO PracticeEnrolment
				(Enrolment_Ref_No,				
				Display_Seq,
				MO_Display_Seq,
				Practice_Name,
				Practice_Name_Chi,
				Room,
				[Floor],
				Block,
				Building,
				Building_Chi,
				District,
				Professional_Seq,
				Phone_Daytime)
	VALUES		(@enrolment_ref_no,				
				@display_seq,
				@mo_display_seq,
				@practice_name,
				@practice_name_chi,
				@room,
				@floor,
				@block,
				@building,
				@building_chi,
				@district,
				@professional_seq,
				@phone_daytime)
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeEnrolment_add] TO HCPUBLIC
GO
