IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountStaging_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:		  CRE13-019-02
-- Modified by:	  Winnie SUEN
-- Modified date: 19 Dec 2014
-- Description:	  Add "IsFreeTextFormat"
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 May 2008
-- Description:	Retrieve the Bank Account, 
--				Practice Information and professional information
--				by enrolment reference number in Staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date:  05 Apr 2009
-- Description:	   Not get the practice_type, but the MO_Display_Seq
--				   Get the practice chi name and contact no
--				   Remove Bank.BR_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	   Kathy LEE
-- Modified date:  23 Nov 2009
-- Description:	   Retrieve results without joining SPAccoutMaintenance
-- =============================================


CREATE PROCEDURE [dbo].[proc_PracticeBankAccountStaging_get_byERN]
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
							 SP_ID char(8),
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
							 Record_Status char(1),
							 Remark nvarchar(255),
							 Phone_Daytime varchar(20),
							 MO_Display_Seq smallint,
							 Submission_Method char(1),
							 Create_Dtm datetime,
							 Create_By varchar(20),
							 Update_Dtm datetime,
							 Update_By varchar(20)
							 )
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO @tmp_practice ( Enrolment_Ref_No,
							 Display_Seq,
							 SP_ID,
							 Practice_Name,
							 Practice_Name_Chi,							 
							 Room,
							 [Floor],
							 Block,
							 Building ,
							 Building_Chi,
							 District,
							 Address_Code,
							 Professional_Seq,
							 Record_Status,
							 Remark,
							 Phone_Daytime,
							 MO_Display_Seq,
							 Submission_Method,
							 Create_Dtm,
							 Create_By,
							 Update_Dtm,
							 Update_By							 
							 )
	SELECT					 Enrolment_Ref_No,
							 Display_Seq,
							 SP_ID,
							 Practice_Name,
							 Practice_Name_Chi,
							 Room,
							 [Floor],
							 Block,
							 Building ,
							 Building_Chi,
							 District,
							 Address_Code,
							 Professional_Seq,
							 Record_Status,
							 Remark,
							 Phone_Daytime,
							 MO_Display_Seq,
							 Submission_Method,
							 Create_Dtm,
							 Create_By,
							 Update_Dtm,
							 Update_By
							 
	FROM PracticeStaging
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
--SELECT	P.Enrolment_Ref_No, P.Display_Seq, P.SP_ID, P.Practice_Name, P.Practice_Type,
--		P.Room, P.[Floor], P.Block, P.Building , P.Building_Chi, P.District, P.Address_Code,
--		P.Professional_Seq,  P.Remark as Practice_Remark, --P.Record_Status as Practice_Record_Status,
--		P.Submission_Method as Practice_Submission_Method, P.Create_Dtm as Practice_Create_Dtm,
--		P.Create_By as Practice_Create_By, P.Update_Dtm as Practice_Update_Dtm, P.Update_By as Practice_Update_By,
--		PS.TSMP as Practice_TSMP,
--		PFS.Service_Category_Code, PFS.Registration_Code, PFS.Record_Status as Professional_Record_Status, 
--		PFS.Create_Dtm as Professional_Create_Dtm, PFS.Create_By as Professional_Create_By,
--		B.Display_Seq as Bank_Display_Seq, B.BR_Code, B.Bank_Name, B.Branch_Name, B.Bank_Account_No, B.Bank_Acc_Holder,
--		B.Record_Status as Bank_Record_Status, B.Remark as Bank_Remark, B.Submission_Method as Bank_Submission_Method, 
--		B.Create_Dtm as Bank_Create_Dtm, B.Create_By as Bank_Create_By, B.Update_Dtm as Bank_Update_Dtm, 
--		B.Update_By as Bank_Update_By, B.TSMP as Bank_TSMP, 
--		P.Record_Status as Practice_Record_Status
--	FROM @tmp_practice P
--	INNER JOIN ProfessionalStaging PFS
--	ON P.Professional_Seq = PFS.Professional_Seq and P.Enrolment_Ref_No = PFS.Enrolment_Ref_No
--	INNER JOIN PracticeStaging PS
--	ON P.Enrolment_Ref_No = PS.Enrolment_Ref_No and P.Display_Seq = PS.Display_Seq
--	LEFT JOIN BankAccountStaging B
--	ON P.Display_Seq = B.SP_Practice_Display_Seq and P.Enrolment_Ref_No = B.Enrolment_Ref_No
--	WHERE P.Enrolment_Ref_No = @enrolment_ref_no
--	order by P.Display_Seq
SELECT	P.Enrolment_Ref_No, P.Display_Seq, P.SP_ID, P.Practice_Name, P.MO_Display_Seq,
		P.Room, P.[Floor], P.Block, P.Building , P.Building_Chi, P.District, P.Address_Code,
		P.Professional_Seq,  P.Remark as Practice_Remark, --P.Record_Status as Practice_Record_Status,
		P.Submission_Method as Practice_Submission_Method, P.Create_Dtm as Practice_Create_Dtm,
		P.Create_By as Practice_Create_By, P.Update_Dtm as Practice_Update_Dtm, P.Update_By as Practice_Update_By,
		PS.TSMP as Practice_TSMP,
		PFS.Service_Category_Code, PFS.Registration_Code, PFS.Record_Status as Professional_Record_Status, 
		PFS.Create_Dtm as Professional_Create_Dtm, PFS.Create_By as Professional_Create_By,
		B.Display_Seq as Bank_Display_Seq, B.Bank_Name, B.Branch_Name, B.Bank_Account_No, B.Bank_Acc_Holder,
		B.Record_Status as Bank_Record_Status, B.Remark as Bank_Remark, B.Submission_Method as Bank_Submission_Method, 
		B.Create_Dtm as Bank_Create_Dtm, B.Create_By as Bank_Create_By, B.Update_Dtm as Bank_Update_Dtm, 
		B.Update_By as Bank_Update_By, B.TSMP as Bank_TSMP, B.IsFreeTextFormat as Bank_IsFreeTextFormat,
		--case P.Record_Status	
		--	when 'A' then P.Record_Status
			--when 'S' then 'W'
		--	when 'S' then case isNull(SPAM.Upd_Type, '')
		--						when 'RP' then 'R'
		--						when 'DP' then 'T'
		--						else 'W'
		--					  end
		--	when 'I' then P.Record_Status 
		--	when 'V' then P.Record_Status
		--	when 'E' then case SPAM.Upd_Type
		--						when 'DP' then 'D'
		--						--when 'RP' then 'R'
		--						when 'SP' then 'S'
		--						else 'E'
		--				  end 
		--	when 'U' then P.Record_Status
		-- end 
		P.Record_Status	as Practice_Record_Status, P.Practice_Name_Chi, P.Phone_Daytime
	FROM @tmp_practice P
	INNER JOIN PracticeStaging PS
	ON P.Enrolment_Ref_No = PS.Enrolment_Ref_No and P.Display_Seq = PS.Display_Seq
	LEFT JOIN ProfessionalStaging PFS
	ON P.Professional_Seq = PFS.Professional_Seq and P.Enrolment_Ref_No = PFS.Enrolment_Ref_No
	LEFT JOIN BankAccountStaging B
	ON P.Display_Seq = B.SP_Practice_Display_Seq and P.Enrolment_Ref_No = B.Enrolment_Ref_No
	--left join SPAccountMaintenance SPAM
	--ON P.SP_ID = SPAM.SP_ID and P.Display_Seq = SPAM.SP_Practice_Display_Seq and SPAM.Record_Status = 'A'
	WHERE P.Enrolment_Ref_No = @enrolment_ref_no
	order by P.Display_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountStaging_get_byERN] TO HCVU
GO