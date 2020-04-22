IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_upd_VoucherAccIDUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_upd_VoucherAccIDUser]
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
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Feb 2010
-- Description:		1. Grant right to [HCSP]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update Voucher Transaction Set Voucher_Acc_ID By Temp_Voucher_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 30 Nov 2009
-- Description:	Update Original_Record_Status in VoucherTranSuspendLOG when VoucherTransaction is suspended
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_upd_VoucherAccIDUser]
	@Voucher_Acc_ID char(15),
	@Temp_Voucher_Acc_ID char(15),
	@User_ID varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE @Tran_ID as char(20)
DECLARE @Tran_Dtm as datetime

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
	WHERE Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID 
	
	UPDATE [dbo].[VoucherTransaction]  
	SET	Record_Status = sc.confirmed_transaction_status
	FROM	[dbo].[voucherTransaction] vt INNER JOIN [dbo].[schemeClaim] sc
			on sc.Scheme_Code = vt.Scheme_Code and sc.Record_Status = 'A'
	WHERE   
	vt.Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID AND  
	vt.Record_Status = 'V'  

	SELECT TOP 1 @Tran_ID = Transaction_ID, @Tran_Dtm = System_Dtm 
	From [dbo].[VoucherTranSuspendLOG]
	WHERE Transaction_ID IN 
	(
		SELECT Transaction_ID FROM [dbo].[VoucherTransaction]
		WHERE Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID 
		AND Record_Status = 'S'
	)
	AND Record_Status = 'S'
	AND Original_Record_Status = 'V'
	Order by System_Dtm Desc 
	
	IF @Tran_ID <> ''
	BEGIN

		UPDATE [dbo].[VoucherTranSuspendLOG]  
		SET   
			Original_Record_Status = sc.confirmed_transaction_status 
		FROM	[dbo].[VoucherTranSuspendLOG] vtl 
				INNER JOIN [dbo].[voucherTransaction] vt on vtl.Transaction_ID = vt.Transaction_ID
				INNER JOIN [dbo].[schemeClaim] sc on sc.Scheme_Code = vt.Scheme_Code and sc.Record_Status = 'A'
		WHERE vtl.Transaction_ID = @Tran_ID  
		AND vtl.System_Dtm = @Tran_Dtm  
		AND vtl.Record_Status = 'S'  
		AND vtl.Original_Record_Status = 'V'  

	END
	
END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_VoucherAccIDUser] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_upd_VoucherAccIDUser] TO HCVU
GO
