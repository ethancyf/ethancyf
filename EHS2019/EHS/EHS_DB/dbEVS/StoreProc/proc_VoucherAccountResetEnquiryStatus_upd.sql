IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountResetEnquiryStatus_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountResetEnquiryStatus_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 16 Oct 2008
-- Description:	Reset the 'Auto Locked' Enquiry Status to 'Available' in VoucherAccount
--              and reset the fail enquiry count in VoucherAccEnquiryFailRecord
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
create procedure [dbo].[proc_VoucherAccountResetEnquiryStatus_upd]
as
begin
SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Declaration
-- =============================================

--Delete the Fail Enquiry Count
	delete from VoucherAccEnquiryFailRecord
	
--Reset the Enquiry Status to Available 'A' if the Account is locked automatically by System 'L'
	update VoucherAccount
	set Public_Enquiry_Status = 'A' 
	where Public_Enquiry_Status = 'L'
	
end
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountResetEnquiryStatus_upd] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountResetEnquiryStatus_upd] TO HCVU
GO
