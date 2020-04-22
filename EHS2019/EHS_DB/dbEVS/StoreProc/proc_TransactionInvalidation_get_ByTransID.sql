IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionInvalidation_get_ByTransID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionInvalidation_get_ByTransID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul Yip	
-- Create date: 24 March 2010
-- Description:	Retrieve invalidated transaction details
-- =============================================

CREATE PROCEDURE [dbo].[proc_TransactionInvalidation_get_ByTransID]
	@Transaction_ID as varchar(30)
	
AS
BEGIN
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

SELECT 
Transaction_ID,
Record_Status,
Invalidation_Type,
Invalidation_Remark,
Create_Dtm,
Create_By,
Update_Dtm,
Update_By,
TSMP
FROM TransactionInvalidation
WHERE 
Transaction_ID = @Transaction_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionInvalidation_get_ByTransID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionInvalidation_get_ByTransID] TO HCVU
GO
