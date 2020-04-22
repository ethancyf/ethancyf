IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTran_delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthTran_delete]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Delete record in ReimbursementAuthTran table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthTran_delete]	@tran_id	char(20)							
as
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

	--Delete the record in ReimbursementAuthTran table
	DELETE FROM ReimbursementAuthTran
	 WHERE [Transaction_ID]=@tran_id


END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTran_delete] TO HCVU
GO
