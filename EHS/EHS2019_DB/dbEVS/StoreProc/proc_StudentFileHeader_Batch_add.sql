IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_Batch_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_Batch_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:				
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM		
-- Created date:	09 Sep 2019
-- CR No.			CRE19-001
-- Description:		Create Batch File
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_Batch_add]
	@Student_File_ID							VARCHAR(15),
	@School_Code								VARCHAR(10),
	@SP_ID										CHAR(8),
	@Practice_Display_Seq						SMALLINT,
	@Service_Receive_Dtm						DATETIME,
	@Scheme_Seq									SMALLINT,
	@Dose										VARCHAR(20),
	@Final_Checking_Report_Generation_Date		DATETIME,
	@Record_Status								VARCHAR(2),
	@Upload_By									VARCHAR(20),
	@Upload_Dtm									DATETIME,
	@Last_Rectify_By							VARCHAR(20),
	@Last_Rectify_Dtm							DATETIME,
	@Claim_Upload_By							VARCHAR(20),
	@Claim_Upload_Dtm							DATETIME,
	@File_Confirm_By							VARCHAR(20),
	@File_Confirm_Dtm							DATETIME,
	@Request_Remove_By							VARCHAR(20),
	@Request_Remove_Dtm							DATETIME,
	@Request_Remove_Function					VARCHAR(20),
	@Confirm_Remove_By							VARCHAR(20),
	@Confirm_Remove_Dtm							DATETIME,
	@Request_Claim_Reactivate_By				VARCHAR(20),
	@Request_Claim_Reactivate_Dtm				DATETIME,
	@Confirm_Claim_Reactivate_By				VARCHAR(20),
	@Confirm_Claim_Reactivate_Dtm				DATETIME,
	@Vaccination_Report_File_ID					VARCHAR(15),		
	@Onsite_Vaccination_File_ID					VARCHAR(15),
	@Claim_Creation_Report_File_ID				VARCHAR(15),
	@Rectification_File_ID						VARCHAR(15),
	@Name_List_File_ID							VARCHAR(15),
	@Update_By									VARCHAR(20),
	@Update_Dtm									DATETIME,
	@Scheme_Code								CHAR(10),
	@Subsidize_Code								CHAR(10),
	@Service_Receive_Dtm_2ndDose				DATETIME,
	@Final_Checking_Report_Generation_Date_2ndDose		DATETIME,
	@Upload_Precheck							CHAR(1),
	@Original_Student_File_ID					VARCHAR(15),
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
	--DECLARE @Scheme_Seq		SMALLINT
	
	--SELECT
	--	@Scheme_Seq = ISNULL( MAX(Scheme_Seq), 0 )
	--FROM
	--	SubsidizeGroupClaim
	--WHERE
	--	Scheme_Code = @Scheme_Code
	--		AND @Service_Receive_Dtm >= Claim_Period_From
	--		AND Record_Status= 'A'
	

-- =============================================
-- Return results
-- =============================================
	
	INSERT INTO StudentFileHeader (
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
		Vaccination_Report_File_ID,
		Onsite_Vaccination_File_ID,
		Claim_Creation_Report_File_ID,
		Rectification_File_ID,
		Name_List_File_ID,
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
		@Vaccination_Report_File_ID,
		@Onsite_Vaccination_File_ID,
		@Claim_Creation_Report_File_ID,
		@Rectification_File_ID,
		@Name_List_File_ID,
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

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_Batch_add] TO HCSP
GO
