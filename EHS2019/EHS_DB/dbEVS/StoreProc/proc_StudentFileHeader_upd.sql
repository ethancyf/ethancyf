IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_upd]
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
	@Student_File_ID							varchar(15),
	@School_Code								varchar(30),
	@SP_ID										char(8),
	@Practice_Display_Seq						smallint,
	@Service_Receive_Dtm						datetime,
	@Dose										varchar(20),
	@Final_Checking_Report_Generation_Date		datetime,
	@Record_Status								varchar(2),
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
	@Request_Rectify_Status						varchar(2),
	@Update_By									varchar(20),
	@Update_Dtm									datetime,
	@Service_Receive_Dtm_2ndDose				datetime,
	@Final_Checking_Report_Generation_Date_2ndDose		datetime,
	@Subsidize_Code								char(10),
	@TSMP										binary(8)
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
		Dose = @Dose,
		Final_Checking_Report_Generation_Date = @Final_Checking_Report_Generation_Date,
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
		Onsite_Vaccination_File_ID = @Onsite_Vaccination_File_ID,
		Claim_Creation_Report_File_ID = @Claim_Creation_Report_File_ID,
		Rectification_File_ID = @Rectification_File_ID,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm,
		Service_Receive_Dtm_2ndDose = @Service_Receive_Dtm_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose = @Final_Checking_Report_Generation_Date_2ndDose,
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
