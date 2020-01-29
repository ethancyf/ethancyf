IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeBankAccountAll_get_bySPIDDEID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeBankAccountAll_get_bySPIDDEID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Return [Registration_Code]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	15 May 2015
-- Description:	Sort by Pracitce Display_Seq
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 23 June 2008
-- Description:	Retrieve the all Bank Account information 
--              by using SP_ID and Data Entry Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	22 Jun 2009
-- Description:		Handle Scheme Change for Practice
--					Remove Delist_Status
--					Remove Delist_Dtm
--					Remove Effective_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	13 July 2009
-- Description:		Handle Scheme Change for Practice
--					Remove Practice_Type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	31 July 2009
-- Description:		Retrieve Practice Chi Name
--					Add Practice_Name_Display_Chi 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	14 Aug 2009
-- Description:		Remove Practice_Name_Display
--					Remove Practice_Name_Display_Chi (Handle in Middle Tier)
--					Practice_Display_Seq -> PracticeID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeBankAccountAll_get_bySPIDDEID]
	@SP_ID char(8),
	@Data_Entry_Account varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi varchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255),
		@display_seq smallint

DECLARE @tmp_practice table( Display_Seq smallint,
							 SP_ID char(8),
							 Practice_Name nvarchar(100),
							 Practice_Name_Chi nvarchar(100),
							 Room varchar(5),
							 [Floor] varchar(3),
							 Block varchar(3),
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
							 Update_By varchar(20))

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO @tmp_practice (  Display_Seq,
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
							 Submission_Method,
							 Create_Dtm,
							 Create_By,
							 Update_Dtm,
							 Update_By)
	SELECT					 Display_Seq,
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
							 Submission_Method,
							 Create_Dtm,
							 Create_By,
							 Update_Dtm,
							 Update_By
	FROM Practice
	WHERE SP_ID = @SP_ID

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

SELECT	P.SP_ID, 
		--P.Display_Seq as Practice_Display_Seq, 
		P.Display_Seq as PracticeID, 		
		P.Practice_Name, 
		--P.Practice_Name as Practice_Name_Display,
		P.Practice_Name_Chi, 
		--P.Practice_Name_Chi as Practice_Name_Display_Chi,
		PFS.Service_Category_Code,
		PFS.Registration_Code,
		P.Record_Status as Practice_Status,
		B.Display_Seq as Bank_Account_Display_Seq,
		B.Bank_Account_No Bank_Account_No, 
		B.Bank_Acc_Holder,
		B.Record_Status as BankAcct_Status,
		P.SP_ID + '-' + convert(varchar(3), P.Display_Seq) + '-' + convert(varchar(3), B.Display_Seq) as BankAccountKey,
		P.Room, P.[Floor], P.Block, P.Building , P.Building_Chi, P.District, P.Address_Code

FROM @tmp_practice P 

	INNER JOIN [Professional] PFS 
		ON P.Professional_Seq = PFS.Professional_Seq and P.SP_ID = PFS.SP_ID 
		
	INNER JOIN [Practice] PS 
		ON P.SP_ID = PS.SP_ID and P.Display_Seq = PS.Display_Seq 
		
	INNER JOIN [DataEntryPracticeMapping] DEPM
		ON P.SP_ID = DEPM.SP_ID and PS.Display_Seq = DEPM.SP_Practice_Display_Seq
	Inner JOIN BankAccount B 
		ON P.Display_Seq = B.SP_Practice_Display_Seq and P.SP_ID = B.SP_ID 

WHERE P.SP_ID = @SP_ID and DEPM.Data_Entry_Account = @Data_Entry_Account

ORDER BY P.Display_Seq



END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountAll_get_bySPIDDEID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PracticeBankAccountAll_get_bySPIDDEID] TO WSEXT
GO