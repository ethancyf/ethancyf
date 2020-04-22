IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SentOutMsg_SOMS_upd_RecordStatusRejected' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SentOutMsg_SOMS_upd_RecordStatusRejected
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- CR No.:  CRE12-012
-- Create Date:	17 July 2012
-- Description:	Update record_status in table - [SentOutMsg_SOMS]
--				Status = Rejected (with reject reason)
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_upd_RecordStatusRejected]
    @sent_out_msg_id varchar(15),
	@reject_by varchar(20),
	@reject_reason nvarchar(1000),
	@tsmp timestamp
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
IF (SELECT SOMS_TSMP FROM SentOutMsg_SOMS
WHERE SOMS_SentOutMsg_ID = @sent_out_msg_id) <> @tsmp
BEGIN
	RAISERROR('00011',16,1)
	RETURN @@ERROR
END
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

    UPDATE SentOutMsg_SOMS
	SET
		SOMS_Record_Status = 'R',
		SOMS_Reject_By = @reject_by,
		SOMS_Reject_Dtm = getdate(),
		SOMS_Reject_Reason = @reject_reason
	WHERE
		SOMS_SentOutMsg_ID = @sent_out_msg_id AND
		SOMS_TSMP = @tsmp
END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_upd_RecordStatusRejected] TO HCVU
GO