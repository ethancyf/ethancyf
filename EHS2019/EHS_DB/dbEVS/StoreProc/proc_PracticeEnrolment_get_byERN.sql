IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeEnrolment_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeEnrolment_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Retrieve the Practice from Table
--				PracticeEnrolment
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeEnrolment_get_byERN] 
	@enrolment_ref_no	char(15)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255),
		@display_seq smallint

DECLARE @tmp_practice table( Enrolment_Ref_No char(15),
							 Display_Seq smallint,
							 Practice_Name nvarchar(100),
							 Practice_Type char(1),
							 Room nvarchar(5),
							 [Floor] nvarchar(3),
							 Block nvarchar(3),
							 Building varchar(255),
							 Building_Chi nvarchar(255) collate database_default,
							 District char(4),
							 Address_Code int,
							 Service_Category_Code smallint,
							 Registration_Code varchar(15))

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO @tmp_practice ( Enrolment_Ref_No,
							 Display_Seq,
							 Practice_Name,
							 Practice_Type,
							 Room,
							 [Floor],
							 Block,
							 Building,
							 Building_Chi,
							 District,
							 Address_Code,
							 Service_Category_Code,
							 Registration_Code)
	SELECT Enrolment_Ref_No, Display_Seq, Practice_Name, Practice_Type,
			Room, [Floor], Block, Building, Building_Chi, District, Address_Code,
			Service_Category_Code, Registration_Code
	FROM PracticeEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

DECLARE avail_cursor cursor 
FOR	SELECT Address_Code, Display_Seq
FROM @tmp_practice

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE @tmp_practice
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
	WHERE Display_Seq = @display_seq
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq
END
CLOSE avail_cursor 
DEALLOCATE avail_cursor
-- =============================================
-- Return results
-- =============================================

	/*SELECT Enrolment_Ref_No, Display_Seq, Practice_Name, Practice_Type,
			Room, [Floor], Block, Building, Building_Chi, District, Address_Code,
			Service_Category_Code, Registration_Code
	FROM PracticeEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no*/
	SELECT Enrolment_Ref_No, Display_Seq, Practice_Name, Practice_Type,
			Room, [Floor], Block, Building, Building_Chi, District, Address_Code,
			Service_Category_Code, Registration_Code
	FROM @tmp_practice
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeEnrolment_get_byERN] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_PracticeEnrolment_get_byERN] TO HCVU
GO
