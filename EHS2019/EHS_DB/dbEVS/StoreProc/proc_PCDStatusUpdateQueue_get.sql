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

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

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

	CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_PCDStatusUpdateQueue_get] TO HCVU
GO
