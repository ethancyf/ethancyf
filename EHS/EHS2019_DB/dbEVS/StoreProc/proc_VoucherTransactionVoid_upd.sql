IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Void VoucherTansaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure proc_VoucherTransactionVoid_upd
@Transaction_ID			char(20)
, @Void_Transaction_ID	char(20)
, @Void_Dtm			datetime
, @Void_Remark		nvarchar(255)
, @Void_By			varchar(20)
, @Status			char(1)
, @tsmp				timestamp
as
-- =============================================
-- Validation 
-- =============================================
if (select tsmp from VoucherTransaction 
		where Transaction_ID = @Transaction_ID) != @tsmp
begin
	Raiserror('00011', 16, 1)
	return @@error
end

-- =============================================
-- Update Transaction 
-- =============================================
if @Status = 'S'
begin
	Update VoucherTransaction
	set Record_Status = 'I'
	, Void_Transaction_ID = @Void_Transaction_ID
	, Void_Dtm = getdate()
	, Void_Remark = @Void_Remark
	, Void_By = @Void_By
	, Update_Dtm = getdate()
	, Update_By = @Void_By
	where Transaction_ID = @Transaction_ID
end
else
begin
	Update VoucherTransaction
	set Record_Status = 'I'
	, Void_Transaction_ID = @Void_Transaction_ID
	, Void_Dtm = getdate()
	, Void_Remark = @Void_Remark
	, Void_By_DataEntry = @Void_By
	, Update_Dtm = getdate()
	, Update_By = @Void_By
	where Transaction_ID = @Transaction_ID
end

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_upd] TO HCSP
GO
