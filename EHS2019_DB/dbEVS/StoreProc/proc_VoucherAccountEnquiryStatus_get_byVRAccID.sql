IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountEnquiryStatus_get_byVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountEnquiryStatus_get_byVRAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 08 Dec 2008
-- Description:	Get the public enquiry status in table
--				VoucherAccount
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_VoucherAccountEnquiryStatus_get_byVRAccID]
	@Voucher_Acc_ID	char(15)
as
BEGIN

	SET NOCOUNT ON;
	
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

	SELECT Public_Enquiry_Status
	FROM VoucherAccount
	WHERE Voucher_Acc_ID = @Voucher_Acc_ID
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountEnquiryStatus_get_byVRAccID] TO HCPUBLIC
GO
