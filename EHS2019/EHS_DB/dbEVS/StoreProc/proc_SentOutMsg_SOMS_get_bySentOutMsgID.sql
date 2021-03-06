IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SentOutMsg_SOMS_get_bySentOutMsgID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SentOutMsg_SOMS_get_bySentOutMsgID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- CR No.:  CRE12-012
-- Create Date:	12 July 2012
-- Description:	Retrieve pending message - [SentOutMsg_SOMS]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_get_bySentOutMsgID]
    @sent_out_msg_id varchar(15)
AS 
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
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
		SOMS_Sent_Dtm,
		SOMS_Message_ID,
		SOMS_TSMP
	FROM
		SentOutMsg_SOMS
	WHERE
		SOMS_SentOutMsg_ID = @sent_out_msg_id
END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_get_bySentOutMsgID] TO HCVU
GO