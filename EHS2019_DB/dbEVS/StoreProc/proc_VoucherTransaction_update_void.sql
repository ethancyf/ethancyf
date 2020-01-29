IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_update_void]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_update_void]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Stanley, Chan
-- Create date: 29/5/2008	
-- Description:	update voucher transaction for void
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	19 Nov 2009	
-- Description:		Handle Mutil Logon, Throw Exception
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_update_void]
	@transaction_id char(20), 
	@void_transaction_id char(20), 
	@void_remark nvarchar(255),
	@void_by char(20),
	@void_by_DataEntry char(20),
	@tsmp timestamp
as
BEGIN
-- =============================================
-- Declaration
-- =============================================
DECLARE	@ori_record_status char(1)
DECLARE @counter int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if (select tsmp from VoucherTransaction 
		where Transaction_ID = @Transaction_ID) != @tsmp
begin
		RAISERROR('00011', 16, 1)
		return @@error
end

IF (SELECT Create_By from VoucherTransaction 
		where Transaction_ID = @Transaction_ID) != @void_by
BEGIN
		RAISERROR('000XX', 16, 1)
		RETURN @@error
END

--select @counter = count(1) from VoucherTransaction
--WHERE [Transaction_ID]=@transaction_id and DATEDIFF(hh, Confirmed_dtm, getdate()) > 24

--if (@counter = 1)
--begin
--	select @ori_record_status = record_status from VoucherTransaction
--	WHERE [Transaction_ID]=@transaction_id

--	if (@ori_record_status<>'S')
--	begin
--			RAISERROR('00011', 16, 1)
--			return @@error
--	end
--end

    -- Insert statements for procedure here
	IF @void_by_DataEntry is not null
		begin
			UPDATE VoucherTransaction
			SET 
			[Void_Transaction_ID] = @void_transaction_id,
			[Void_Dtm] = getdate(),
			[Void_Remark] = @void_remark,
			[Void_By] = @void_by,
			[Void_By_DataEntry] = @void_by_DataEntry,
			[Update_By] = @void_by,
			[Update_Dtm] = getdate(),
			[Record_Status] = 'I'
			WHERE 
			[Transaction_ID]=@transaction_id
		end
	else
		begin
			UPDATE VoucherTransaction
			SET 
			[Void_Transaction_ID] = @void_transaction_id,
			[Void_Dtm] = getdate(),
			[Void_Remark] = @void_remark,
			[Void_By] = @void_by,
			[Update_By] = @void_by,
			[Update_Dtm] = getdate(),
			[Record_Status] = 'I'
			WHERE 
			[Transaction_ID]=@transaction_id
		end
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_update_void] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_update_void] TO HCVU
GO
