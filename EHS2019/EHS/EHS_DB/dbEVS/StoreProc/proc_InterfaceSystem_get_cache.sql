 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceSystem_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceSystem_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	   
-- Modified date:   
-- Description:	  
-- =============================================
-- =============================================
-- Author:		Derek LEUNG
-- Create date: 	3 December 2010
-- Description:	Retrieve items from InterfaceSystem table
-- =============================================

CREATE PROCEDURE [dbo].[proc_InterfaceSystem_get_cache]
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SELECT
		[System_Name], 
		[Other_For_Encrypt_Cert], 
		[Other_For_Verify_Cert], 
		[EHS_For_Decrypt_Cert], 
		[EHS_For_Sign_Cert]
	FROM 
		[dbo].[InterfaceSystem]
	WHERE [Record_Status] = 'A'
	ORDER BY [System_Name]	
END
GO

GRANT EXECUTE ON [dbo].[proc_InterfaceSystem_get_cache] TO WSEXT
GO