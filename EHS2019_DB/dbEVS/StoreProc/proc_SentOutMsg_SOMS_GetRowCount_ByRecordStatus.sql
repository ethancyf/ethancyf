IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SentOutMsg_SOMS_GetRowCount_ByRecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SentOutMsg_SOMS_GetRowCount_ByRecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==================================================================================================
-- Author:	Tommy LAM
-- CR No.:  CRE12-012
-- Create Date:	10 Aug 2012
-- Description:	Get [Row Count] value from table - [SentOutMsg_SOMS] by [SOMS_Record_Status]
-- ==================================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_GetRowCount_ByRecordStatus]
    @record_status char(1)
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
        COUNT(1)
    FROM
        SentOutMsg_SOMS
    WHERE
        SOMS_Record_Status = @record_status

END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_GetRowCount_ByRecordStatus] TO HCVU
GO
