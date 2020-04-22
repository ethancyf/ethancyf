IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_add_byFileID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_add_byFileID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	3 July 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		Insert Schedule_Gen_Dtm with {NULL} to [FileGenerationQueue]
-- =============================================
-- =============================================
-- Author:				Vincent YUEN
-- Create date:		22 Jan 2010
-- Description:		Add file generation queue by file Id
-- =============================================


CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_add_byFileID]
	@file_id varchar(30),
	@in_parameter ntext, 
	@request_by varchar(20)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @generation_id int
	DECLARE @formatted_generation_id char(12)
	DECLARE @output_file_name varchar(50)
	DECLARE @status char(1)
	DECLARE @password varchar(30)
	DECLARE @file_description nvarchar(500)
	DECLARE @Schedule_Gen_Dtm DATETIME

	-- Helper parameter for Generation ID
	DECLARE @gen_id_year_prefix varchar(2)
	DECLARE @tmpSystemProfile AS TABLE 
	(
	   profile_num INT,
	   profile_prefix VARCHAR(4)
	)


	IF EXISTS(SELECT 1 FROM FileGeneration WHERE [File_ID] = @file_id) 
	BEGIN
		------------------------------------------------------------------------------------------
		-- Get File ID Property
		SELECT	@output_file_name = ISNULL(File_Name_Prefix, '') + CONVERT(VARCHAR(8), GETDATE(), 112) + LOWER(ISNULL('.' + File_Type, '')), 
						@file_description = ISNULL(File_Desc, '')
		FROM		FileGeneration 
		WHERE	[File_ID] = @file_id 

		SET @status = 'P'
		SET @password = ''
		SET @file_description = @file_description + ' - ' + @output_file_name
		SET @Schedule_Gen_Dtm = NULL

		------------------------------------------------------------------------------------------
		-- Get Next Generation Number
		INSERT @tmpSystemProfile 
			Exec [proc_SystemProfile_get_byProfileID] 'FSN', ''

		SELECT	@generation_id = profile_num 
		FROM		@tmpSystemProfile

		SET @gen_id_year_prefix = CONVERT(VARCHAR(2), GETDATE(), 2)
		SET @formatted_generation_id = @gen_id_year_prefix + RIGHT(REPLICATE('0', 12) + CONVERT(VARCHAR, @generation_id), 10)

		------------------------------------------------------------------------------------------
		-- Add Queue
		EXECUTE [proc_FileGenerationQueue_add] 
			   @formatted_generation_id
			  ,@file_id
			  ,@in_parameter
			  ,@output_file_name
			  ,@status
			  ,@password
			  ,@request_by
			  ,@file_description
			  ,@Schedule_Gen_Dtm

	END
	ELSE
	BEGIN
		RAISERROR('Invalid File ID', 1, 2) 
	END

	
END
GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_add_byFileID] TO HCVU
GO
