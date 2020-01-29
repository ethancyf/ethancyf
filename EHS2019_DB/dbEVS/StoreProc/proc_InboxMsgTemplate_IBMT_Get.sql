IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMsgTemplate_IBMT_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_InboxMsgTemplate_IBMT_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:  CRE12-012
-- Create Date:	12 July 2012
-- Description:	Get record from table - [InboxMsgTemplate_IBMT] by [IBMT_MsgTemplate_ID]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_InboxMsgTemplate_IBMT_Get]
    @msg_template_id varchar(10)
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
        IBMT_MsgTemplateContent,
        IBMT_MsgTemplateCategory,
        IBMT_Record_Status,
        IBMT_Create_Dtm
    FROM
        InboxMsgTemplate_IBMT
    WHERE
        IBMT_MsgTemplate_ID = @msg_template_id

END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMsgTemplate_IBMT_Get] TO HCVU
GO
