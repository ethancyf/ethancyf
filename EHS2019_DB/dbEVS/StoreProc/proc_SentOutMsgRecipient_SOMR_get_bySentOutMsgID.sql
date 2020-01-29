IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SentOutMsgRecipient_SOMR_get_bySentOutMsgID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SentOutMsgRecipient_SOMR_get_bySentOutMsgID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- CR No.:  CRE12-012
-- Create Date:	25 July 2012
-- Description:	Retrieve pending message recipient - [SentOutMsgRecipient_SOMR]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsgRecipient_SOMR_get_bySentOutMsgID]
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
		SOMR_SentOutMsg_ID,
		SOMR_Profession,
		SOMR_Scheme
	FROM
		SentOutMsgRecipient_SOMR
	WHERE
		SOMR_SentOutMsg_ID = @sent_out_msg_id
	ORDER BY
		SOMR_Profession,
		SOMR_Scheme
END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsgRecipient_SOMR_get_bySentOutMsgID] TO HCVU
GO
