IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccEnquiryFailRecord_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccEnquiryFailRecord_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[proc_VoucherAccEnquiryFailRecord_add] @Voucher_Acc_ID	char(15)
as

declare @Enquiry_Fail_Count	smallint

if exists (select 1 from VoucherAccEnquiryFailRecord where Voucher_Acc_ID = @Voucher_Acc_ID )
begin
	-- Update
	--select @Enquiry_Fail_Count = Enquiry_Fail_Count
	--from VoucherAccEnquiryFailRecord
	--where Voucher_Acc_ID = @Voucher_Acc_ID

	--if (@Enquiry_Fail_Count >= 9)
	--begin
	--	update VoucherAccount
	--	set Public_Enquiry_Status = 'L'
	--	where Voucher_Acc_ID = @Voucher_Acc_ID
	--end
	
	update VoucherAccEnquiryFailRecord 
	set Enquiry_Fail_Count = Enquiry_Fail_Count + 1
	, Last_Enquiry_Fail_Dtm = GetDate()
	where Voucher_Acc_ID = @Voucher_Acc_ID

end
else
begin
	-- Insert
	INSERT INTO VoucherAccEnquiryFailRecord
			   (Voucher_Acc_ID
			   ,Last_Enquiry_Fail_Dtm
			   ,Enquiry_Fail_Count)
		 VALUES
			   ( @Voucher_Acc_ID
			   ,GetDate()
			   ,1)

end 
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccEnquiryFailRecord_add] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccEnquiryFailRecord_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccEnquiryFailRecord_add] TO HCVU
GO
