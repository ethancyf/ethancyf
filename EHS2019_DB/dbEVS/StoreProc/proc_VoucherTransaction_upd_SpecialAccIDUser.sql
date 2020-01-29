IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_upd_SpecialAccIDUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_upd_SpecialAccIDUser]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Change : CRE13-001 EHAPP
-- Author:  Karl LAM  
-- Create date: 03 Apr 2013
-- Description: Make use of new column schemeClaim.Confirmed_Transaction_Status
-- =============================================  
-- =============================================
-- Author:		Dedrick Ng
-- Create date: 27 Sep 2009
-- Description:	Update Voucher Transaction Set Voucher_Acc_ID By Special_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_SpecialAccIDUser]
	@Voucher_Acc_ID char(15),
	@Special_Acc_ID char(15),
	@User_ID varchar(20)
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
		Update_Dtm = GetDate(),
		Update_By = @User_ID
		
		--Record_Status = 'A'
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

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_SpecialAccIDUser] TO HCVU
GO
