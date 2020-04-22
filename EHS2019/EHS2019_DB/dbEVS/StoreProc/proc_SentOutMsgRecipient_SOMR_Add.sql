IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SentOutMsgRecipient_SOMR_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SentOutMsgRecipient_SOMR_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:  CRE12-012
-- Create Date:	12 July 2012
-- Description:	Insert record into table - [SentOutMsgRecipient_SOMR]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsgRecipient_SOMR_Add]
    @sent_out_msg_id varchar(15),
    @profession varchar(3),
    @scheme varchar(10),
    @create_by varchar(20)
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

    INSERT INTO SentOutMsgRecipient_SOMR (
        [SOMR_SentOutMsg_ID],
        [SOMR_Profession],
        [SOMR_Scheme],
        [SOMR_Create_By],
        [SOMR_Create_Dtm])
    VALUES (
        @sent_out_msg_id,
        @profession,
        @scheme,
        @create_by,
        GETDATE())
END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsgRecipient_SOMR_Add] TO HCVU
GO
