IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SentOutMsg_SOMS_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SentOutMsg_SOMS_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:  CRE12-012
-- Create Date:	12 July 2012
-- Description:	Insert record into table - [SentOutMsg_SOMS]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_Add]
    @sent_out_msg_id varchar(15),
    @record_status char(1),
    @sent_out_msg_subject nvarchar(1000),
    @sent_out_msg_content nvarchar(MAX),
    @sent_out_msg_category varchar(20),
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

    INSERT INTO SentOutMsg_SOMS (
        [SOMS_SentOutMsg_ID],
        [SOMS_Record_Status],
        [SOMS_SentOutMsgSubject],
        [SOMS_SentOutMsgContent],
	[SOMS_SentOutMsgCategory],
        [SOMS_Create_By],
        [SOMS_Create_Dtm])
    VALUES (
        @sent_out_msg_id,
        @record_status,
        @sent_out_msg_subject,
        @sent_out_msg_content,
	@sent_out_msg_category,
        @create_by,
        GETDATE())
END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_Add] TO HCVU
GO
