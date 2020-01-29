if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_VoucherTransaction_upd_SpecialAccID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_VoucherTransaction_upd_SpecialAccID]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================  
-- Change : CRE13-001 EHAPP
-- Author:  Karl LAM  
-- Create date: 03 Apr 2013
-- Description: Make use of new column schemeClaim.Confirmed_Transaction_Status
-- =============================================  
-- =============================================
-- Author:		Paul Yip
-- Create date: 5 Feb 2010
-- Description:	Update Voucher Transaction Set Voucher_Acc_ID By Special_Acc_ID
--				(For IMMD batch job)
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_SpecialAccID]
	@Voucher_Acc_ID char(15),
	@Special_Acc_ID char(15)
AS
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
	UPDATE [dbo].[VoucherTransaction]
	SET 
		Voucher_Acc_ID = @Voucher_Acc_ID,
		Update_Dtm = GetDate()

	WHERE Special_Acc_ID = @Special_Acc_ID 
	
	UPDATE	[dbo].[VoucherTransaction]  
	SET		Record_Status = sc.confirmed_transaction_status
	FROM	[dbo].[voucherTransaction] vt INNER JOIN [dbo].[schemeClaim] sc
			on sc.Scheme_Code = vt.Scheme_Code and sc.Record_Status = 'A'
	WHERE   
		vt.Special_Acc_ID = @Special_Acc_ID AND  
		vt.Record_Status = 'V'  

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_VoucherTransaction_upd_SpecialAccID] to HCVU
GO       