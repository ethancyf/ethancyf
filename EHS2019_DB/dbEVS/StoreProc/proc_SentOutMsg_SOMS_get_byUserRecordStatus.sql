IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SentOutMsg_SOMS_get_byUserRecordStatus' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SentOutMsg_SOMS_get_byUserRecordStatus
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- CR No.:  CRE12-012
-- Create Date:	1 Aug 2012
-- Description:	Retrieve pending message with user and record status - [SentOutMsg_SOMS]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_get_byUserRecordStatus]
	@record_status char(1),
	@create_by varchar(20)
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
DECLARE @day int
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

IF @record_status = 'S'
BEGIN

SELECT @day = parm_value1
FROM
	SystemParameters
WHERE
	Parameter_Name = 'SentMsgKeepDay'
	AND Record_Status = 'A'
	AND [Scheme_Code] = 'ALL'

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
	SOMS_Sent_Dtm,
	SOMS_Message_ID,
	SOMS_TSMP
FROM
	SentOutMsg_SOMS
WHERE
	SOMS_Record_Status = @record_status
	AND SOMS_Create_By = @create_by
	AND DATEDIFF(dd,SOMS_Sent_Dtm,GETDATE()) <= @day
ORDER BY
	SOMS_Sent_Dtm DESC
END

ELSE IF @record_status = 'R'
BEGIN

SELECT @day = parm_value1
FROM
	SystemParameters
WHERE
	Parameter_Name = 'RejectedMsgKeepDay'
	AND Record_Status = 'A'
	AND [Scheme_Code] = 'ALL'

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
	SOMS_Sent_Dtm,
	SOMS_Message_ID,
	SOMS_TSMP
FROM
	SentOutMsg_SOMS
WHERE
	SOMS_Record_Status = @record_status
	AND SOMS_Create_By = @create_by
	AND DATEDIFF(dd,SOMS_Reject_Dtm,GETDATE()) <= @day
ORDER BY
	SOMS_Reject_Dtm DESC
END

ELSE

BEGIN
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
	SOMS_Sent_Dtm,
	SOMS_Message_ID,
	SOMS_TSMP
FROM
	SentOutMsg_SOMS
WHERE
	SOMS_Record_Status = @record_status
	AND SOMS_Create_By = @create_by
ORDER BY
	SOMS_Create_Dtm DESC
END

END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_get_byUserRecordStatus] TO HCVU
GO