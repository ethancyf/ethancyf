IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_ValidateAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_ValidateAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update TempVoucherAccount, Validated_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_upd_ValidateAccID]
	@Voucher_Acc_ID	char(15),
	@Scheme_Code char(10),
	@Validated_Acc_ID char(15)
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

	UPDATE [dbo].[TempVoucherAccount]
	SET
		Validated_Acc_ID = @Validated_Acc_ID
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Scheme_Code = @Scheme_Code
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_ValidateAccID] TO HCVU
GO
