IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_ReactivateEnquiry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_ReactivateEnquiry]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 02 June 2008
-- Description:	Reactivate Public Enquiry of Voucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	17 Sep 2009
-- Description:		Remove @Scheme_Code
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_ReactivateEnquiry]
	-- Add the parameters for the stored procedure here
	@Voucher_Acc_ID char(15),
	@Public_Enq_Status_Remark nvarchar(255),
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
	set Public_Enquiry_Status = 'A', 
	Public_Enq_Status_Remark = @Public_Enq_Status_Remark,
	Update_By = @Update_By,
	Update_dtm = getdate()
	where Voucher_Acc_ID = @Voucher_Acc_ID
	
	update VoucherAccEnquiryFailRecord 
	set Enquiry_Fail_Count = 0
	where Voucher_Acc_ID = @Voucher_Acc_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_ReactivateEnquiry] TO HCVU
GO
