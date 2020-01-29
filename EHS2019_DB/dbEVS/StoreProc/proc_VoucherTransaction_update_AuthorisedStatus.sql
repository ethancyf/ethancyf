IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_update_AuthorisedStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_update_AuthorisedStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026-03 (Add PCV13)
-- Modified by:		Lawrence TSANG
-- Modified date:	17 October 2017
-- Description:		Stored procedure not used anymore
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Update authorised_status in VoucherTransaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_VoucherTransaction_update_AuthorisedStatus]	@tran_id	char(20)
							,@authorised_status		 char(1)
							,@scheme_code	char(10)
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
--	where Transaction_ID = @tran_id) != @tsmp
--begin
--	Raiserror('600002', 16, 1)
--end

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================

	--Update the authorised status in VoucherTransaction table
	UPDATE VoucherTransaction
	SET [Authorised_status] = @authorised_status	  
	WHERE [Transaction_ID]=@tran_id
	and [Scheme_code]=@scheme_code

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_update_AuthorisedStatus] TO HCVU
GO
*/
