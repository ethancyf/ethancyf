IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_StudentFileHeaderStaging_after_upd')
	DROP TRIGGER [dbo].[tri_StudentFileHeaderStaging_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	13 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	09 Jul 2019
-- CR No.			CRE19-001
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 September 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Trigger for StudentFileHeaderStaging
-- =============================================   

CREATE TRIGGER [dbo].[tri_StudentFileHeaderStaging_after_upd]
   ON		[dbo].[StudentFileHeaderStaging]
   AFTER	INSERT, UPDATE
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
-- =============================================
-- Return results
-- =============================================

    INSERT INTO StudentFileHeaderStagingLOG (
		System_Dtm,
		Student_File_ID,
		School_Code,
		SP_ID,
		Practice_Display_Seq,
		Service_Receive_Dtm,
		Scheme_Code,
		Scheme_Seq,
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
		Update_By,
		Update_Dtm,
		Service_Receive_Dtm_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose,
		Subsidize_Code,
		Upload_Precheck,
		Name_List_File_ID,
		Vaccination_Report_File_ID,
		Onsite_Vaccination_File_ID,
		Claim_Creation_Report_File_ID,
		Rectification_File_ID,
		Request_Claim_Reactivate_By,
		Request_Claim_Reactivate_Dtm,
		Confirm_Claim_Reactivate_By,
		Confirm_Claim_Reactivate_Dtm,
		Original_Student_File_ID,
		Request_Rectify_Status,
		Service_Receive_Dtm_2,
		Final_Checking_Report_Generation_Date_2,
		Service_Receive_Dtm_2ndDose_2,
		Final_Checking_Report_Generation_Date_2ndDose_2,
		Vaccination_Report_File_ID_2,
		Onsite_Vaccination_File_ID_2
	)
	SELECT
		GETDATE(),
		Student_File_ID,
		School_Code,
		SP_ID,
		Practice_Display_Seq,
		Service_Receive_Dtm,
		Scheme_Code,
		Scheme_Seq,
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
		Update_By,
		Update_Dtm,
		Service_Receive_Dtm_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose,
		Subsidize_Code,
		Upload_Precheck,
		Name_List_File_ID,
		Vaccination_Report_File_ID,
		Onsite_Vaccination_File_ID,
		Claim_Creation_Report_File_ID,
		Rectification_File_ID,
		Request_Claim_Reactivate_By,
		Request_Claim_Reactivate_Dtm,
		Confirm_Claim_Reactivate_By,
		Confirm_Claim_Reactivate_Dtm,
		Original_Student_File_ID,
		Request_Rectify_Status,
		Service_Receive_Dtm_2,
		Final_Checking_Report_Generation_Date_2,
		Service_Receive_Dtm_2ndDose_2,
		Final_Checking_Report_Generation_Date_2ndDose_2,
		Vaccination_Report_File_ID_2,
		Onsite_Vaccination_File_ID_2
	FROM
		inserted


END
GO

