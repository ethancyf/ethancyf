IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_update_ClaimCreationApproval]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_update_ClaimCreationApproval]
GO

/****** Object:  StoredProcedure [dbo].[proc_VoucherTransaction_update_ClaimCreationApproval]    Script Date: 07/13/2010 19:06:07 ******/
/*
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Derek Leung
-- Create date:		15 May 2008
-- Description:		Update record_status in VoucherTransaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_VoucherTransaction_update_ClaimCreationApproval]	@tran_id	char(20)
							,@record_status		 char(1)
							,@update_by		varchar(20)
							,@update_dtm		datetime							
							,@tsmp	timestamp
as
BEGIN
-- =============================================
-- Declaration
-- ============================================= 

-- =============================================
-- Validation 
-- =============================================

 IF (SELECT TSMP FROM VoucherTransaction WHERE Transaction_ID = @tran_id) != @tsmp  
 BEGIN  
  RAISERROR('00011', 16, 1)  
  RETURN @@error  
 END  

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================

	--Update the record status in VoucherTransaction table

	IF @record_status = 'M' -- approve
	BEGIN
		UPDATE VoucherTransaction
		SET [Record_Status] = @record_status,
			[Approval_By]	= @update_by,
			[Approval_Dtm]   = @update_dtm
		WHERE [Transaction_ID]=@tran_id
	END

	IF @record_status = 'D' -- reject
	BEGIN
		UPDATE VoucherTransaction
		SET [Record_Status] = @record_status, 
			[Reject_By]	   =  @update_by, 
			[Reject_Dtm]   =  @update_dtm 
		WHERE [Transaction_ID]=@tran_id
	END

END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_update_ClaimCreationApproval] TO HCVU
GO

*/