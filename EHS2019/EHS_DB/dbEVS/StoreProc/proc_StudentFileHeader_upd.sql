IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	24 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns (Second Vaccination Date)
-- =============================================
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
-- Modified date:	15 Aug 2019
-- CR No.			CRE19-001 (VSS 2019)
-- Description:		- Add [Vaccination_Report_File_ID], [Onsite_Vaccination_File_ID]
--					[Claim_Creation_Report_File_ID], [Rectification_File_ID]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	05 Aug 2019
-- CR No.:			CRE19-001 (New initiatives for VSS and RVP in 2019-20)
-- Description:		Handle new columns [Service_Receive_Dtm_2ndDose],[Final_Checking_Report_Generation_Date_2ndDose], [Subsidize_Code]
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	23 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Update StudentFileHeader
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_upd]
	@Student_File_ID							VARCHAR(15),
	@School_Code								VARCHAR(30),
	@SP_ID										CHAR(8),
	@Practice_Display_Seq						SMALLINT,
	@Service_Receive_Dtm						DATETIME,
	@Service_Receive_Dtm_2						DATETIME,
	@Dose										VARCHAR(20),
	@Final_Checking_Report_Generation_Date		DATETIME,
	@Final_Checking_Report_Generation_Date_2	DATETIME,
	@Record_Status								VARCHAR(2),
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
	@Name_List_File_ID							VARCHAR(15),
	@Vaccination_Report_File_ID					VARCHAR(15),		
	@Vaccination_Report_File_ID_2				VARCHAR(15),		
	@Onsite_Vaccination_File_ID					VARCHAR(15),
	@Onsite_Vaccination_File_ID_2				VARCHAR(15),
	@Claim_Creation_Report_File_ID				VARCHAR(15),
	@Rectification_File_ID						VARCHAR(15),
	@Request_Rectify_Status						VARCHAR(2),
	@Update_By									VARCHAR(20),
	@Update_Dtm									DATETIME,
	@Service_Receive_Dtm_2ndDose				DATETIME,
	@Service_Receive_Dtm_2ndDose_2				DATETIME,
	@Final_Checking_Report_Generation_Date_2ndDose		DATETIME,
	@Final_Checking_Report_Generation_Date_2ndDose_2	DATETIME,
	@Subsidize_Code								CHAR(10),
	@TSMP										BINARY(8)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT TSMP FROM StudentFileHeader WHERE Student_File_ID = @Student_File_ID
	) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END


-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE
		StudentFileHeader
	SET	
		School_Code = @School_Code,
		SP_ID = @SP_ID,
		Practice_Display_Seq = @Practice_Display_Seq,
		Service_Receive_Dtm = @Service_Receive_Dtm,
		Service_Receive_Dtm_2 = @Service_Receive_Dtm_2,
		Dose = @Dose,
		Final_Checking_Report_Generation_Date = @Final_Checking_Report_Generation_Date,
		Final_Checking_Report_Generation_Date_2 = @Final_Checking_Report_Generation_Date_2,
		Record_Status = @Record_Status,
		Last_Rectify_By = @Last_Rectify_By,
		Last_Rectify_Dtm = @Last_Rectify_Dtm,
		Claim_Upload_By = @Claim_Upload_By,
		Claim_Upload_Dtm = @Claim_Upload_Dtm,
		File_Confirm_By = @File_Confirm_By,
		File_Confirm_Dtm = @File_Confirm_Dtm,
		Request_Remove_By = @Request_Remove_By,
		Request_Remove_Dtm = @Request_Remove_Dtm,
		Request_Remove_Function = @Request_Remove_Function,
		Confirm_Remove_By = @Confirm_Remove_By,
		Confirm_Remove_Dtm = @Confirm_Remove_Dtm,
		Request_Claim_Reactivate_By = @Request_Claim_Reactivate_By,
		Request_Claim_Reactivate_Dtm = @Request_Claim_Reactivate_Dtm,
		Confirm_Claim_Reactivate_By = @Confirm_Claim_Reactivate_By,
		Confirm_Claim_Reactivate_Dtm = @Confirm_Claim_Reactivate_Dtm,
		Name_List_File_ID = @Name_List_File_ID,
		Vaccination_Report_File_ID = @Vaccination_Report_File_ID,
		Vaccination_Report_File_ID_2 = @Vaccination_Report_File_ID_2,
		Onsite_Vaccination_File_ID = @Onsite_Vaccination_File_ID,
		Onsite_Vaccination_File_ID_2 = @Onsite_Vaccination_File_ID_2,
		Claim_Creation_Report_File_ID = @Claim_Creation_Report_File_ID,
		Rectification_File_ID = @Rectification_File_ID,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm,
		Service_Receive_Dtm_2ndDose = @Service_Receive_Dtm_2ndDose,
		Service_Receive_Dtm_2ndDose_2 = @Service_Receive_Dtm_2ndDose_2,
		Final_Checking_Report_Generation_Date_2ndDose = @Final_Checking_Report_Generation_Date_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose_2 = @Final_Checking_Report_Generation_Date_2ndDose_2,
		Subsidize_Code = @Subsidize_Code,
		Request_Rectify_Status = @Request_Rectify_Status
	WHERE
		Student_File_ID = @Student_File_ID
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_upd] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_upd] TO HCSP
GO

