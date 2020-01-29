IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountStaging_get_byERNBankStagingStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountStaging_get_byERNBankStagingStatus]
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
-- Author:		Clark YIP
-- Create date: 26 Jun 2008
-- Description:	Retrieve the Bank Account, 
--				Practice Information and professional information
--				by enrolment reference number in Staging and Bank Acct Staging record_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date:  05 Apr 2009
-- Description:	   Not get the practice_type, br_code, but the MO_Display_Seq
--				   Get the practice chi name and contact no
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	   Kathy LEE
-- Modified date:  16 July 2009
-- Description:	   Change the type of budilding_chi to nvarchar in temp table
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeBankAccountStaging_get_byERNBankStagingStatus]
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
							 MO_Display_Seq smallint,
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
							 Submission_Method char(1),
							 Create_Dtm datetime,
							 Create_By varchar(20),
							 Update_Dtm datetime,
							 Update_By varchar(20),
							 Practice_Name_Chi nvarchar(100),
							 Phone_Daytime varchar(20))
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
							 MO_Display_Seq,
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
							 Submission_Method,
							 Create_Dtm,
							 Create_By,
							 Update_Dtm,
							 Update_By,
							 Practice_Name_Chi,
							 Phone_Daytime)
	SELECT					 Enrolment_Ref_No,
							 Display_Seq,
							 SP_ID,
							 Practice_Name,
							 MO_Display_Seq,
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
							 Submission_Method,
							 Create_Dtm,
							 Create_By,
							 Update_Dtm,
							 Update_By,
							 Practice_Name_Chi,
							 Phone_Daytime
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
SELECT	P.Enrolment_Ref_No, P.Display_Seq, P.SP_ID, P.Practice_Name, P.MO_Display_Seq,
		P.Room, P.[Floor], P.Block, P.Building , P.Building_Chi, P.District, P.Address_Code,
		P.Professional_Seq, P.Record_Status as Practice_Record_Status, P.Remark as Practice_Remark, 
		P.Submission_Method as Practice_Submission_Method, P.Create_Dtm as Practice_Create_Dtm,
		P.Create_By as Practice_Create_By, P.Update_Dtm as Practice_Update_Dtm, P.Update_By as Practice_Update_By,
		PS.TSMP as Practice_TSMP,
		PFS.Service_Category_Code, PFS.Registration_Code, PFS.Record_Status as Professional_Record_Status, 
		PFS.Create_Dtm as Professional_Create_Dtm, PFS.Create_By as Professional_Create_By,
		B.Display_Seq as Bank_Display_Seq, --B.BR_Code, 
		B.Bank_Name, B.Branch_Name, B.Bank_Account_No, B.Bank_Acc_Holder,
		B.Record_Status as Bank_Record_Status, B.Remark as Bank_Remark, B.Submission_Method as Bank_Submission_Method, 
		B.Create_Dtm as Bank_Create_Dtm, B.Create_By as Bank_Create_By, B.Update_Dtm as Bank_Update_Dtm, 
		B.Update_By as Bank_Update_By, B.TSMP as Bank_TSMP, B.IsFreeTextFormat as Bank_IsFreeTextFormat,
		P.Practice_Name_Chi, P.Phone_Daytime
	FROM @tmp_practice P
	INNER JOIN ProfessionalStaging PFS
	ON P.Professional_Seq = PFS.Professional_Seq and P.Enrolment_Ref_No = PFS.Enrolment_Ref_No
	INNER JOIN PracticeStaging PS
	ON P.Enrolment_Ref_No = PS.Enrolment_Ref_No and P.Display_Seq = PS.Display_Seq
	LEFT JOIN BankAccountStaging B
	ON P.Display_Seq = B.SP_Practice_Display_Seq and P.Enrolment_Ref_No = B.Enrolment_Ref_No
	WHERE P.Enrolment_Ref_No = @enrolment_ref_no AND B.record_status in ('A','D')
	AND P.Record_Status='A'
	order by P.Display_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountStaging_get_byERNBankStagingStatus] TO HCVU
GO
