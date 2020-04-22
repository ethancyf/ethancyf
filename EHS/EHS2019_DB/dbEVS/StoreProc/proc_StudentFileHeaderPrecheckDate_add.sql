IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderPrecheckDate_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderPrecheckDate_add]
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
-- Modified by:		Chris YIM		
-- Modified date:	05 Sep 2019
-- CR No.			CRE19-001
-- Description:		Assign Date for batch
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderPrecheckDate_add]
	@Student_File_ID							VARCHAR(15),
	@Scheme_Code								CHAR(10),
	@Scheme_Seq									SMALLINT,
	@Subsidize_Code								CHAR(10),
	@Subsidize_Item_Code						CHAR(10),
	@Class_Name									NVARCHAR(10),
	@Service_Receive_Dtm						DATETIME,
	@Final_Checking_Report_Generation_Date		DATETIME,
	@Service_Receive_Dtm_2ndDose				DATETIME,
	@Final_Checking_Report_Generation_Date_2ndDose		DATETIME,
	@Create_By									VARCHAR(20),
	@Create_Dtm									DATETIME,
	@Update_By									VARCHAR(20),
	@Update_Dtm									DATETIME

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
	
	INSERT INTO StudentFileHeaderPrecheckDate (
		Student_File_ID					
		,Scheme_Code
		,Scheme_Seq
		,Subsidize_Code
		,Subsidize_Item_Code
		,Class_Name
		,Service_Receive_Dtm
		,Final_Checking_Report_Generation_Date
		,Service_Receive_Dtm_2ndDose
		,Final_Checking_Report_Generation_Date_2ndDose
		,Create_By
		,Create_Dtm
		,Update_By
		,Update_Dtm
	) VALUES (
		@Student_File_ID
		,@Scheme_Code
		,@Scheme_Seq
		,@Subsidize_Code
		,@Subsidize_Item_Code
		,@Class_Name
		,@Service_Receive_Dtm
		,@Final_Checking_Report_Generation_Date
		,@Service_Receive_Dtm_2ndDose
		,@Final_Checking_Report_Generation_Date_2ndDose
		,@Create_By
		,@Create_Dtm
		,@Update_By
		,@Update_Dtm
	)
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderPrecheckDate_add] TO HCSP
GO

