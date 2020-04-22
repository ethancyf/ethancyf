IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTran_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthTran_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Update record in ReimbursementAuthTran table for 2nd reimbursement
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthTran_update]	@tran_id	char(20)
					,@authorised_dtm datetime
					,@authorised_by varchar(20)
					,@authorised_status char(1)
					,@tsmp	timestamp
as
BEGIN
-- =============================================
-- Declaration
-- ============================================= 

-- =============================================
-- Validation 
-- =============================================

--if (select tsmp from ReimbursementAuthTran 
--	where Transaction_ID = @tran_id) != @tsmp
--begin
--	Raiserror('600001', 16, 1)
--end

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================

	
	--Update record in ReimbursementAuthTran table
	UPDATE ReimbursementAuthTran
	SET [Second_Authorised_dtm] = @authorised_dtm, [Second_Authorised_by] = @authorised_by, [Authorised_status] = @authorised_status
	WHERE [Transaction_ID]=@tran_id	

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTran_update] TO HCVU
GO
