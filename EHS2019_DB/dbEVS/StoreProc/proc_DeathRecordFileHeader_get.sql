IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordFileHeader_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordFileHeader_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Author:			Lawrence TSANG
-- Create date:		27 April 2011
-- CR No.:			CRE11-007
-- Description:		Get DeathRecordFileHeader
-- =============================================
-- =============================================

--exec proc_DeathRecordFileHeader_get 'CONFIRMALL',1,1,0


CREATE PROCEDURE [dbo].[proc_DeathRecordFileHeader_get]
	@Action_Code		varchar(20),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Keep_Day	int
	DECLARE @rowcount INT
	DECLARE @row_cnt_error varchar(max)

	DECLARE @tempFileHeader table(
			Death_Record_File_ID char(15),
			Description varchar(100),
			Import_Dtm datetime,
			Import_By varchar(20),
			Confirm_Dtm datetime,
			Confirm_By varchar(20),
			Match_Dtm datetime,
			Record_Status char(1),
			Processing char(1))
	
	SELECT
		@Keep_Day = Parm_Value1
	FROM
		SystemParameters
	WHERE
		Parameter_Name = 'DRFileKeepDay'

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	IF @Action_Code = 'IMPORT' 
	BEGIN
		-- Death Record File Import
		INSERT INTO @tempFileHeader 
		SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			Death_Record_File_ID,
			Description,
			Import_Dtm,
			Import_By,
			Confirm_Dtm,
			Confirm_By,
			Match_Dtm,
			Record_Status,
			Processing
		FROM
			DeathRecordFileHeader
		WHERE
			Record_Status <> 'R'
				AND DATEDIFF(dd, Import_Dtm, GETDATE()) <= @Keep_Day
		ORDER BY
			Import_Dtm DESC
	
	END ELSE IF @Action_Code = 'CONFIRMALL' 
	BEGIN
		-- Death Record File Confirmation (All records)
		INSERT INTO @tempFileHeader 
		SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			Death_Record_File_ID,
			Description,
			Import_Dtm,
			Import_By,
			Confirm_Dtm,
			Confirm_By,
			Match_Dtm,
			Record_Status,
			Processing
		FROM
			DeathRecordFileHeader
		WHERE
			Record_Status IN ('C', 'P', 'S', 'F')
				AND DATEDIFF(dd, Import_Dtm, GETDATE()) <= @Keep_Day
		ORDER BY
			Import_Dtm DESC
	
	END ELSE IF @Action_Code = 'CONFIRM' 
	BEGIN
		-- Death Record File Confirmation (Show Pending Confirmation records only)
		INSERT INTO @tempFileHeader 
		SELECT  TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
			Death_Record_File_ID,
			Description,
			Import_Dtm,
			Import_By,
			Confirm_Dtm,
			Confirm_By,
			Match_Dtm,
			Record_Status,
			Processing
		FROM
			DeathRecordFileHeader
		WHERE
			Record_Status = 'C'
				AND DATEDIFF(dd, Import_Dtm, GETDATE()) <= @Keep_Day
		ORDER BY
			Import_Dtm DESC
			
	END

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = COUNT(1)
		FROM	@tempFileHeader

		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    
		RAISERROR (@row_cnt_error,16,1)    
		RETURN
	END CATCH 

-- =============================================
-- Return results
-- =============================================
	SELECT 
			Death_Record_File_ID,
			Description,
			Import_Dtm,
			Import_By,
			Confirm_Dtm,
			Confirm_By,
			Match_Dtm,
			Record_Status,
			Processing
	FROM
			@tempFileHeader
	ORDER BY
			Import_Dtm DESC
	

END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordFileHeader_get] TO HCVU
GO
