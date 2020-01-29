IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementVoid_update_byTranIDAuthoriseStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementVoid_update_byTranIDAuthoriseStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Void reimbursement authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   31 Dec 2008
-- Description:	    Update the logic not to loop each transaction, now by a batch
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   12 Aug 2009
-- Description:	    1. Add scheme as the key to search in ReimbursementAuthorisation
--					2. No need to "touch" the VoucherTransaction table when cancel authorization
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 August 2009
-- Description:		(1)	Remove @tran_status, @authorised_dtm, as they are actually not included in the code
--					(2) Group @authorised_by and @current_user to @user_id, as they always refer to the same data
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   18 August 2009
-- Description:	    Remove @authorised_status, the Authorised_Status 'P', '1', and '2' will be updated at the same time
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementVoid_update_byTranIDAuthoriseStatus] 
	@user_id	      	varchar(20),
	@void_remark		nvarchar(255),
	@reimburse_id		char(15),
	@scheme_code		char(10)
WITH RECOMPILE
AS BEGIN
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

	-- Delete [ReimbursementAuthTran]
	DELETE FROM
		ReimbursementAuthTran
	WHERE
		Reimburse_ID = @reimburse_id
			AND Scheme_Code = @scheme_code
	
	-- Update [ReimbursementAuthorisation]
	UPDATE	
		ReimbursementAuthorisation
	SET	
		[Void_By] = @user_id,
		[Void_Remark]= @void_remark,
		[Void_Dtm] = GETDATE(),
		[Update_By] = @user_id,
		[Update_Dtm] = GETDATE(),
		[Record_status] = 'V'
	WHERE
		Reimburse_ID = @reimburse_id
			AND Scheme_Code = @scheme_code
			AND Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementVoid_update_byTranIDAuthoriseStatus] TO HCVU
GO
