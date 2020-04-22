IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeSetup_SESU_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SchemeSetup_SESU_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:	CRE13-001
-- Create Date:	18 April 2013
-- Description:	Get all record(s) from table - [SchemeSetup_SESU] for Cache
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SchemeSetup_SESU_get_all_cache]
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
        SESU_Scheme_Code,
        SESU_TransactionStatus,
        SESU_SetupType,
	SESU_SetupValue
    FROM
        SchemeSetup_SESU

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeSetup_SESU_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeSetup_SESU_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeSetup_SESU_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeSetup_SESU_get_all_cache] TO WSEXT
Go
