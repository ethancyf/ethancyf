IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMsgTemplate_IBMT_List]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_InboxMsgTemplate_IBMT_List]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:  CRE12-012
-- Create Date:	12 July 2012
-- Description:	List all active record(s) from table - [InboxMsgTemplate_IBMT]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_InboxMsgTemplate_IBMT_List]
AS BEGIN
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
        IBMT_MsgTemplate_ID,
        IBMT_MsgTemplateSubject,
        IBMT_MsgTemplateCategory,
	IBMT_Record_Status,
        IBMT_Create_Dtm
    FROM
        InboxMsgTemplate_IBMT
    WHERE
        IBMT_Record_Status = 'A'
    ORDER BY
        IBMT_MsgTemplate_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMsgTemplate_IBMT_List] TO HCVU
GO
