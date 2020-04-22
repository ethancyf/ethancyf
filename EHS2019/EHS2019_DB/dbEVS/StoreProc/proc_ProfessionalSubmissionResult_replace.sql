IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionResult_replace]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionResult_replace]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Modification History  
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:  Karl LAM  
-- Modified date: 21 Oct 2013  
-- Description:  Change File Name to Varchar(50)  
-- ============================================= 
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 May 2008
-- Description:	Insert / Update Professional Submission Result Record
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionResult_replace]
	@File_Name varchar(50),
	@Reference_No varchar(20),
	@Result char(1),
	@Remark nvarchar(70)	
AS
BEGIN
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

-- Two Case: 1. Newly Import, 2. Re-Import
-- 1. Newly Import
--  Insert [ProfessionalSubmissionResult]
	-- [ProfessionalSubmissionResult] Record Not Exist

-- 2. Re-Import
--	Update [ProfessionalSubmissionResult]
	-- [ProfessionalSubmissionResult] Record Exist


	IF (
		SELECT Count(*)
		FROM [dbo].[ProfessionalSubmissionResult]
		WHERE File_Name = @File_Name AND Reference_No = @Reference_No 
	) > 0
	BEGIN
		UPDATE [dbo].[ProfessionalSubmissionResult]
		SET
			Result = @Result,
			Reference_No = @Reference_No
		WHERE
			File_Name = @File_Name AND Reference_No = @Reference_No 

	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[ProfessionalSubmissionResult]
		(File_Name, Reference_No, Result, Remark)
		VALUES
		(@File_Name, @Reference_No, @Result, @Remark)
	END

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionResult_replace] TO HCVU
GO
