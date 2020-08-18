IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PatientPortalDoctorList_upd_FileContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_PatientPortalDoctorList_upd_FileContent]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:
-- CR No.:
-- Description:	
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		08-06-2020
-- CR No.:			CRE20-005
-- Description:		Save Doctor List
-- =============================================
CREATE PROCEDURE [dbo].[proc_PatientPortalDoctorList_upd_FileContent]
	@SDIR_Last_Update_Dtm AS DATETIME,
	@File_Content AS IMAGE,
	@File_Type AS VARCHAR(3)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @MaxNumOfFiles INT
	DECLARE @NumOfFiles INT
	DECLARE @DACO_CutOff_Dtm DATETIME
	DECLARE @Old_System_Dtm DATETIME

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @MaxNumOfFiles = (SELECT CONVERT(INT,Parm_Value1) FROM SystemParameters WITH (NOLOCK) WHERE	Parameter_Name = 'eHRSS_PP_DoctorList_NumOfLatestFileKeep')
	SET @DACO_CutOff_Dtm = (SELECT DACO_CutOff_Dtm FROM DataCutOff_DACO WITH (NOLOCK) WHERE	DACO_DataType_ID = 'SDIR_LastUpdateDate')
	
-- =============================================
-- Return results
-- =============================================

	INSERT [dbEVS_File].[dbo].[PatientPortalDoctorList_File](
		[System_Dtm],
		[SDIR_Last_Update_Dtm],
		[File_Content],
		[File_Type]
		)
	VALUES(
		GETDATE()
		,@SDIR_Last_Update_Dtm
		,@File_Content
		,@File_Type
		)

	SET @NumOfFiles = (SELECT COUNT(1) FROM [dbEVS_File].[dbo].[PatientPortalDoctorList_File] WITH (NOLOCK) )

	IF @NumOfFiles > @MaxNumOfFiles
		BEGIN
			SET @Old_System_Dtm = (SELECT TOP 1 
										tmp.[System_Dtm] 
									FROM 
										(SELECT TOP (@MaxNumOfFiles) 
											[System_Dtm] 
										FROM 
											[dbEVS_File].[dbo].[PatientPortalDoctorList_File] WITH (NOLOCK) 
										ORDER BY 
											[System_Dtm] DESC) tmp 
									ORDER BY 
										[System_Dtm])
		
			IF @Old_System_Dtm IS NOT NULL
				BEGIN
					DELETE FROM [dbEVS_File].[dbo].[PatientPortalDoctorList_File] WHERE [System_Dtm] < @Old_System_Dtm
				END
		END
END
GO

GRANT EXECUTE ON [dbo].[proc_PatientPortalDoctorList_upd_FileContent] TO HCVU
GO