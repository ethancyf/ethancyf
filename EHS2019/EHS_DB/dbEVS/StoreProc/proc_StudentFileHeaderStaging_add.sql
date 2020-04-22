﻿IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	28 Aug 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	16 Jul 2019
-- CR No.			CRE19-001 (PPP)
-- Description:		Add input
--					@Scheme_Code, @Subsidize_Code,@Upload_Precheck
--					@Service_Receive_Dtm_2ndDose, @Final_Checking_Report_Generation_Date_2ndDose
--					@Vaccination_Report_File_ID, @Onsite_Vaccination_File_ID
--					@Claim_Creation_Report_File_ID, @Rectification_File_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Add StudentFileHeaderStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_add]
	@Student_File_ID							varchar(15),
	@School_Code								varchar(30),
	@SP_ID										char(8),
	@Practice_Display_Seq						smallint,
	@Service_Receive_Dtm						datetime,
	@Dose										varchar(20),
	@Final_Checking_Report_Generation_Date		datetime,
	@Record_Status								varchar(2),
	@Upload_By									varchar(20),
	@Upload_Dtm									datetime,
	@Last_Rectify_By							varchar(20),
	@Last_Rectify_Dtm							datetime,
	@Claim_Upload_By							varchar(20),
	@Claim_Upload_Dtm							datetime,
	@File_Confirm_By							varchar(20),
	@File_Confirm_Dtm							datetime,
	@Request_Remove_By							varchar(20),
	@Request_Remove_Dtm							datetime,
	@Request_Remove_Function					varchar(20),
	@Confirm_Remove_By							varchar(20),
	@Confirm_Remove_Dtm							datetime,
	@Request_Claim_Reactivate_By				varchar(20),
	@Request_Claim_Reactivate_Dtm				datetime,
	@Confirm_Claim_Reactivate_By				varchar(20),
	@Confirm_Claim_Reactivate_Dtm				datetime,
	@Name_List_File_ID							VARCHAR(15),		
	@Vaccination_Report_File_ID					varchar(15),		
	@Onsite_Vaccination_File_ID					varchar(15),
	@Claim_Creation_Report_File_ID				varchar(15),
	@Rectification_File_ID						varchar(15),
	@Update_By									varchar(20),
	@Update_Dtm									datetime,
	@Scheme_Code								char(10),
	@Subsidize_Code								char(10),
	@Service_Receive_Dtm_2ndDose				datetime,
	@Final_Checking_Report_Generation_Date_2ndDose		datetime,
	@Upload_Precheck							char(1),
	@Original_Student_File_ID					varchar(15),
	@Request_Rectify_Status						varchar(2)
AS BEGIN

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
	DECLARE @Scheme_Seq		smallint = 0
	
	IF @Upload_Precheck = 'N'
	BEGIN
		SELECT
			@Scheme_Seq = ISNULL( MAX(Scheme_Seq), 0 )
		FROM
			SubsidizeGroupClaim
		WHERE
			Scheme_Code = @Scheme_Code
				AND Subsidize_Code = @Subsidize_Code
				AND @Service_Receive_Dtm >= Claim_Period_From
				AND Record_Status= 'A'
	END


-- =============================================
-- Return results
-- =============================================
	
	INSERT INTO StudentFileHeaderStaging (
		Student_File_ID,
		School_Code,
		SP_ID,
		Practice_Display_Seq,
		Service_Receive_Dtm,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Dose,
		Final_Checking_Report_Generation_Date,
		Remark,
		Record_Status,
		Upload_By,
		Upload_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Claim_Upload_By,
		Claim_Upload_Dtm,
		File_Confirm_By,
		File_Confirm_Dtm,
		Request_Remove_By,
		Request_Remove_Dtm,
		Request_Remove_Function,
		Confirm_Remove_By,
		Confirm_Remove_Dtm,
		Request_Claim_Reactivate_By,
		Request_Claim_Reactivate_Dtm,
		Confirm_Claim_Reactivate_By,
		Confirm_Claim_Reactivate_Dtm,
		Name_List_File_ID,
		Vaccination_Report_File_ID,
		Onsite_Vaccination_File_ID,
		Claim_Creation_Report_File_ID,
		Rectification_File_ID,
		Update_By,
		Update_Dtm,
		Service_Receive_Dtm_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose,
		Upload_Precheck,
		Original_Student_File_ID,
		Request_Rectify_Status
	) VALUES (
		@Student_File_ID,
		@School_Code,
		@SP_ID,
		@Practice_Display_Seq,
		@Service_Receive_Dtm,
		@Scheme_Code,
		@Scheme_Seq,
		@Subsidize_Code,
		@Dose,
		@Final_Checking_Report_Generation_Date,
		NULL,
		@Record_Status,
		@Upload_By,
		@Upload_Dtm,
		@Last_Rectify_By,
		@Last_Rectify_Dtm,
		@Claim_Upload_By,
		@Claim_Upload_Dtm,
		@File_Confirm_By,
		@File_Confirm_Dtm,
		@Request_Remove_By,
		@Request_Remove_Dtm,
		@Request_Remove_Function,
		@Confirm_Remove_By,
		@Confirm_Remove_Dtm,
		@Request_Claim_Reactivate_By,
		@Request_Claim_Reactivate_Dtm,
		@Confirm_Claim_Reactivate_By,
		@Confirm_Claim_Reactivate_Dtm,
		@Name_List_File_ID,
		@Vaccination_Report_File_ID,
		@Onsite_Vaccination_File_ID,
		@Claim_Creation_Report_File_ID,
		@Rectification_File_ID,
		@Update_By,
		@Update_Dtm,
		@Service_Receive_Dtm_2ndDose,
		@Final_Checking_Report_Generation_Date_2ndDose,
		@Upload_Precheck,
		@Original_Student_File_ID,
		@Request_Rectify_Status
	)
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_add] TO HCSP
GO
