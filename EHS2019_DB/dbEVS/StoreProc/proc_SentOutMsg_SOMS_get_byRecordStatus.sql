IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SentOutMsg_SOMS_get_byRecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SentOutMsg_SOMS_get_byRecordStatus]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- =============================================  
-- Modification History      
-- CR No.:			CRE12-014  
-- Modified by:		Karl LAM   
-- Modified date:	03 Jan 2013  
-- Description:		Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- CR No.:			CRE12-012
-- Author:			Nick POON
-- Create Date:		13 July 2012
-- Description:		Retrieve pending message - [SentOutMsg_SOMS]
-- ==========================================================================================

--exec  proc_SentOutMsg_SOMS_get_byRecordStatus 'A',1,1,0 --501

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_get_byRecordStatus]
	@record_status char(1),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
	DECLARE @rowcount INT
	DECLARE @row_cnt_error varchar(max)
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
	SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		SOMS_SentOutMsg_ID,
		SOMS_Record_Status,
		SOMS_SentOutMsgSubject,
		SOMS_SentOutMsgContent,
		SOMS_SentOutMsgCategory,
		SOMS_Create_By,
		SOMS_Create_Dtm,
		SOMS_Confirm_By,
		SOMS_Confirm_Dtm,
		SOMS_Reject_By,
		SOMS_Reject_Dtm,
		SOMS_Reject_Reason,
		SOMS_Message_ID,
		SOMS_TSMP
	INTO 
		#tempSOMS
	FROM
		SentOutMsg_SOMS
	WHERE
		SOMS_Record_Status = @record_status
	ORDER BY
		SOMS_Create_Dtm DESC


-- =============================================    
-- Max Row Checking  
-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = COUNT(1) FROM	#tempSOMS
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    
		RAISERROR (@row_cnt_error,16,1)    
		DROP TABLE #tempSOMS
		RETURN
	END CATCH 

-- ============================================================
-- Return results
-- ============================================================

	SELECT 
		SOMS_SentOutMsg_ID,
		SOMS_Record_Status,
		SOMS_SentOutMsgSubject,
		SOMS_SentOutMsgContent,
		SOMS_SentOutMsgCategory,
		SOMS_Create_By,
		SOMS_Create_Dtm,
		SOMS_Confirm_By,
		SOMS_Confirm_Dtm,
		SOMS_Reject_By,
		SOMS_Reject_Dtm,
		SOMS_Reject_Reason,
		SOMS_Message_ID,
		SOMS_TSMP
	FROM 
		#tempSOMS
	ORDER BY
		SOMS_Create_Dtm DESC

	DROP TABLE #tempSOMS
		
END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_get_byRecordStatus] TO HCVU
GO