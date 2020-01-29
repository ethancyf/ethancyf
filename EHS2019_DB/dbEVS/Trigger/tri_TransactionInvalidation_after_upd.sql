IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_TransactionInvalidation_after_upd')
	DROP TRIGGER [dbo].[tri_TransactionInvalidation_after_upd] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		24 March 2010
-- Description:		Trigger an insert statement into TransactionInvalidationLOG when a row is inserted/updated from TransactionInvalidation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE TRIGGER [dbo].[tri_TransactionInvalidation_after_upd]
   ON		[dbo].[TransactionInvalidation]
   AFTER	INSERT, UPDATE
AS BEGIN

	SET NOCOUNT ON;
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
	INSERT INTO [dbo].[TransactionInvalidationLOG] (
		[System_Dtm],
		[Transaction_ID],
		[Record_Status],
		[Invalidation_Type],
		[Invalidation_Remark],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By]
	)
	SELECT 
		GETDATE(),
		[Transaction_ID],
		[Record_Status],
		[Invalidation_Type],
		[Invalidation_Remark],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By]
	FROM 
		Inserted

END
GO
