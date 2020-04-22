IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_upd_PublicEnquiryStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_upd_PublicEnquiryStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 08 Dec 2008
-- Description:	Update the public enquiry status to "L"
--				in table VoucherAccount
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_VoucherAccount_upd_PublicEnquiryStatus]
	@Voucher_Acc_ID	char(15)
as
BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Public_Enquiry_Status char(1)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SELECT @Public_Enquiry_Status = Public_Enquiry_Status
	FROM VoucherAccount
	WHERE Voucher_Acc_ID = @Voucher_Acc_ID
-- =============================================
-- Return results
-- =============================================

	if @Public_Enquiry_Status <> 'S'
	begin
		update VoucherAccount
			set Public_Enquiry_Status = 'L'
		where Voucher_Acc_ID = @Voucher_Acc_ID
	end
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_upd_PublicEnquiryStatus] TO HCPUBLIC
GO
