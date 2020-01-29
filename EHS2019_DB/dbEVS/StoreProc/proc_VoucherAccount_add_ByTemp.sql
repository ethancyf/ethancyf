IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_add_ByTemp]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_add_ByTemp]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Insert Voucher Account By TempVoucher Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 30 Sep 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_add_ByTemp]
	@Voucher_Acc_ID char(15),
	@Temp_Voucher_Acc_ID char(15),
	@Scheme_Code char(10)
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
	INSERT INTO [dbo].[VoucherAccount]
	(
		Voucher_Acc_ID,
		Scheme_Code,
		--Voucher_Used,
		--Total_Voucher_Amt_Used,
		Record_Status,
		Remark,
		Public_Enquiry_Status,
		Public_Enq_Status_Remark,
		Effective_Dtm,
		Terminate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Deceased
	)	
	SELECT
		@Voucher_Acc_ID,
		Scheme_Code,
		--Voucher_Used,
		--Total_Voucher_Amt_Used,
		'A',
		NULL,
		'A',
		NULL,
		GetDate(),
		NULL,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Deceased
	FROM [dbo].[TempVoucherAccount]
	WHERE Voucher_Acc_ID = @Temp_Voucher_Acc_ID	AND Scheme_Code = @Scheme_Code
	
END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_add_ByTemp] TO HCVU
GO
