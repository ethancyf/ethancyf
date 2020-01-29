IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountOriginal_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountOriginal_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 16 Jan 2012
-- Description:	Retrieve the Bank Account with 
--				Practice Information Original record by enrollment
--				reference number
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeBankAccountOriginal_get_byERN]
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
							 Practice_Name_Chi nvarchar(100),
							 Room nvarchar(5),
							 [Floor] nvarchar(3),
							 Block nvarchar(3),
							 Building varchar(255),
							 Building_Chi nvarchar(255) collate database_default,
							 District char(4),
							 Address_Code int,
							 Professional_Seq smallint,
							 Phone_Daytime varchar(20),
							 MO_Display_Seq smallint)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO @tmp_practice ( Enrolment_Ref_No,
							 Display_Seq,
							 Practice_Name,
							 Practice_Name_Chi,
							 Room,
							 [Floor],
							 Block,
							 Building,
							 Building_Chi,
							 District,
							 Address_Code,
							 Professional_Seq,
							 Phone_Daytime,
							 MO_Display_Seq)
	SELECT Enrolment_Ref_No, Display_Seq, Practice_Name, Practice_Name_Chi, 
			Room, [Floor], Block, Building, Building_Chi, District, Address_Code,
			Professional_Seq, Phone_Daytime, MO_Display_Seq
	FROM PracticeOriginal
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

    SELECT P.Enrolment_Ref_No, P.Display_Seq, P.Practice_Name, P.Practice_Name_Chi, 
			P.Room, P.[Floor], P.Block, P.Building, P.Building_Chi, P.District, P.Address_Code, P.Phone_Daytime, P.MO_Display_Seq,
			P.Professional_Seq, PE.Service_Category_Code, PE.Registration_Code, B.Display_Seq as Bank_Display_Seq,
			B.Bank_Name, B.Branch_Name, B.Bank_Account_No, B.Bank_Acc_Holder
	FROM @tmp_practice P
	INNER JOIN ProfessionalOriginal PE
	ON P.Professional_Seq = PE.Professional_Seq and P.Enrolment_Ref_No = PE.Enrolment_Ref_No
	LEFT JOIN BankAccountOriginal B
	ON P.Display_Seq = B.SP_Practice_Display_Seq and P.Enrolment_Ref_No = B.Enrolment_Ref_No
	WHERE P.Enrolment_Ref_No = @enrolment_ref_no
	order by P.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountOriginal_get_byERN] TO HCVU
GO
