IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountVoucherInfo_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountVoucherInfo_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 03 May 2008
-- Description:	Update Voucher Account Informaion
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccountVoucherInfo_upd]
	@Voucher_Acc_ID char(15),
	@Scheme_Code char(10),
	@Voucher_Used smallint,
	@Total_Voucher_Amt_Used money,
	@Update_By varchar(20), 
	@VoucherAccountType char(1)

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
	if @VoucherAccountType = 'V' 
	begin
		update VoucherAccount 
		set Voucher_Used = @Voucher_Used,
			Total_Voucher_Amt_Used = @Total_Voucher_Amt_Used,
			Update_By = @Update_by,
			Update_Dtm = getdate()
		where 
			Voucher_Acc_ID = @Voucher_Acc_ID and Scheme_Code = @Scheme_Code
    end
	else
	begin
		update TempVoucherAccount 
		set Voucher_Used = @Voucher_Used,
			Total_Voucher_Amt_Used = @Total_Voucher_Amt_Used,
			Update_By = @Update_by,
			Update_Dtm = getdate()
		where 
			Voucher_Acc_ID = @Voucher_Acc_ID and Scheme_Code = @Scheme_Code
	end
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountVoucherInfo_upd] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountVoucherInfo_upd] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountVoucherInfo_upd] TO HCVU
GO
