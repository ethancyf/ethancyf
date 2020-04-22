IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_Suspend]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_Suspend]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 02 June 2008
-- Description:	Suspend Voucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	17 Sep 2009
-- Description:		Remove @Scheme_Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccount_Suspend]
	-- Add the parameters for the stored procedure here
	@Voucher_Acc_ID char(15),	
	@Remark nvarchar(255) ,
	@Update_By varchar(20),
	@TSMP timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

	if (select TSMP from VoucherAccount where Voucher_Acc_ID = @Voucher_Acc_ID) != @TSMP 
	begin
		Raiserror('00011' , 16,1)
		return @@error
	end

    update VoucherAccount 
	set Record_Status = 'S', 
	remark = @Remark,
	Update_By = @Update_By,
	Update_dtm = getdate()
	where Voucher_Acc_ID = @Voucher_Acc_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_Suspend] TO HCVU
GO
