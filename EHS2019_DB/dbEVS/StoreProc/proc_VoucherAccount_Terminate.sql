IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_Terminate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_Terminate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:	22 January 2010
-- Description:		Update [VoucherAccount].[Terminate_Dtm]
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 02 June 2008
-- Description:	Terminate Voucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	17 Sep 2009
-- Description:		Remove @Scheme_Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccount_Terminate]
	@Voucher_Acc_ID char(15),
	@Remark nvarchar(255),
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
	set Record_Status = 'D',
	Remark = @Remark,
	Update_By = @Update_By,
	Update_dtm = getdate(),
	Terminate_Dtm = GETDATE()
	where Voucher_Acc_ID = @Voucher_Acc_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_Terminate] TO HCVU
GO
