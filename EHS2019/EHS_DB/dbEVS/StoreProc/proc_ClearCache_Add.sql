IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClearCache_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClearCache_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Insert into [ClearCache]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClearCache_Add]
	@Application_Server		varchar(20),
	@Cache_File				varchar(255)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Declaration
-- =============================================
	DECLARE @Request_ID		varchar(8)


-- =============================================
-- Initialization
-- =============================================
	SELECT @Request_ID = CONVERT(varchar, MAX(CONVERT(int, Request_ID)) + 1)
	FROM ClearCache
	
	SELECT @Request_ID = RIGHT('00000000' + CONVERT(varchar, @Request_ID), 8)
	
	IF @Request_ID IS NULL BEGIN
		SELECT @Request_ID = '00000001'
	END


-- =============================================
-- Execute
-- =============================================
	INSERT INTO ClearCache (
		Request_ID,
		Application_Server,
		Cache_File,
		Record_Status,
		Message,
		Request_Dtm
	) VALUES (
		@Request_ID,
		@Application_Server,
		@Cache_File,
		'A',
		NULL,
		GETDATE()
	)
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ClearCache_Add] TO HCVU
GO
