IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SentOutMsg_SOMS_get_OutboxCountByUser' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SentOutMsg_SOMS_get_OutboxCountByUser
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- CR No.:  CRE12-012
-- Create Date:	28 Aug 2012
-- Description:	Get Outbox message count - [SentOutMsg_SOMS]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_get_OutboxCountByUser]	
	@create_by varchar(20)
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

SELECT COUNT(1)
FROM
	SentOutMsg_SOMS
WHERE
	SOMS_Create_By = @create_by
	AND SOMS_Record_Status IN ('P','T')

END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_get_OutboxCountByUser] TO HCVU
GO