IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Update record_status in VoucherTransaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_VoucherTransaction_update]	@tran_id	char(20)
							,@record_status		 char(1)
							,@tsmp	timestamp
as
BEGIN
-- =============================================
-- Declaration
-- ============================================= 

-- =============================================
-- Validation 
-- =============================================

--if (select tsmp from VoucherTransaction 
--		where Transaction_ID = @tran_id) != @tsmp
--begin
--	Raiserror('600002', 16, 1)
--end

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================


	--Update the record status in VoucherTransaction table
	UPDATE VoucherTransaction
	SET [Record_Status] = @record_status	  
	WHERE [Transaction_ID]=@tran_id



END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_update] TO HCVU
GO
