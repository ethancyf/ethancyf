IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PCDStatusUpdateQueue_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_PCDStatusUpdateQueue_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Modification History
-- CR No.:			
-- Modified by:	    
-- Modified date:   
-- Description:		
-- ==========================================================================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- ==========================================================================================
-- Author:	Koala CHENG
-- CR No.:	CRE17-016
-- Create Date:	17 Jul 2018
-- Description:	Get record from table - [PCDStatusUpdateQueue]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_PCDStatusUpdateQueue_get]
	@record_status	CHAR(1)
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

	EXEC [proc_SymmetricKey_open]

	SELECT
		[SP_ID],
		CONVERT(varchar(100), DECRYPTBYKEY([Encrypt_Field1])) AS [HKIC],
		[Record_Status],
		[Create_Dtm],
		[Update_Dtm]
	FROM
		PCDStatusUpdateQueue
	WHERE
		[Record_Status] = @record_status

	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_PCDStatusUpdateQueue_get] TO HCVU
GO
