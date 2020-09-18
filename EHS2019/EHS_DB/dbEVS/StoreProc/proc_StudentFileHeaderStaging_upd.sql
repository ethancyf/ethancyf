IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_upd]
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
-- Modified date:	30 Aug 2019
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
-- Modified by:		Lawrence TSANG
-- Modified date:	17 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Update StudentFileHeaderStaging
-- =============================================  

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_upd]
	@Student_File_ID				VARCHAR(15),
	@Record_Status					VARCHAR(2),
	@Last_Rectify_By				VARCHAR(20),
	@Last_Rectify_Dtm				DATETIME,
	@Claim_Upload_By				VARCHAR(20),
	@Claim_Upload_Dtm				DATETIME,
	@File_Confirm_By				VARCHAR(20),
	@File_Confirm_Dtm				DATETIME,
	@Request_Remove_By				VARCHAR(20),
	@Request_Remove_Dtm				DATETIME,
	@Request_Remove_Function		VARCHAR(20),
	@Confirm_Remove_By				VARCHAR(20),
	@Confirm_Remove_Dtm				DATETIME,
	@Request_Claim_Reactivate_By	VARCHAR(20),
	@Request_Claim_Reactivate_Dtm	DATETIME,
	@Confirm_Claim_Reactivate_By	VARCHAR(20),
	@Confirm_Claim_Reactivate_Dtm	DATETIME,
	@Name_List_File_ID				VARCHAR(15),		
	@Vaccination_Report_File_ID		VARCHAR(15),		
	@Vaccination_Report_File_ID_2	VARCHAR(15),		
	@Onsite_Vaccination_File_ID		VARCHAR(15),
	@Onsite_Vaccination_File_ID_2	VARCHAR(15),
	@Claim_Creation_Report_File_ID	VARCHAR(15),
	@Rectification_File_ID			VARCHAR(15),
	@Request_Rectify_Status			VARCHAR(2),
	@Update_By						VARCHAR(20),
	@Update_Dtm						DATETIME,
	@TSMP							BINARY(8)
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	DECLARE @Temp_TSMP BINARY(8)

	SET @Temp_TSMP = (SELECT TSMP FROM StudentFileHeaderStaging WHERE Student_File_ID = @Student_File_ID)

	IF @Temp_TSMP != @TSMP OR @Temp_TSMP IS NULL
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
		StudentFileHeaderStaging
	SET	
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
		Request_Rectify_Status = @Request_Rectify_Status,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm
	WHERE
		Student_File_ID = @Student_File_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_upd] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_upd] TO HCSP
GO

