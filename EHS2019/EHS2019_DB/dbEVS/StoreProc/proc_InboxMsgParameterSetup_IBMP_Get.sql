IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InboxMsgParameterSetup_IBMP_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_InboxMsgParameterSetup_IBMP_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================================================
-- Author:	Tommy LAM
-- CR No.:  CRE12-012
-- Create Date:	12 July 2012
-- Description:	Get record(s) from table - [InboxMsgParameterSetup_IBMP] by [IBMT_MsgTemplate_ID]
-- ===============================================================================================

CREATE PROCEDURE [dbo].[proc_InboxMsgParameterSetup_IBMP_Get]
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
        IBMP_MsgTemplate_ID,
        IBMP_MsgParameter_ID,
        IBMP_MsgParameterType,
	IBMP_MsgParameterArgument,
        IBMP_MsgParameterDisplayName
    FROM
        InboxMsgParameterSetup_IBMP
    WHERE
        IBMP_MsgTemplate_ID = @msg_template_id
    ORDER BY
        IBMP_MsgParameter_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_InboxMsgParameterSetup_IBMP_Get] TO HCVU
GO
