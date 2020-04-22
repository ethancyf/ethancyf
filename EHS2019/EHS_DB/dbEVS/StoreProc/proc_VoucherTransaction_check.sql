IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_check]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_check]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	03 Sep 2019
-- CR No.			CRE19-001 (VSS 2019 - Claim Creation)
-- Description:		Grant EXECUTE right for role HCVU
-- =============================================
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		19-11-2008
-- Description:		Check Insert Transaction Concurrence
-- =============================================

CREATE procedure [dbo].[proc_VoucherTransaction_check]

@Temp_Voucher_Acc_ID char(15)

AS

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Insert transaction
-- =============================================

IF (
	SELECT Count(*) FROM [VoucherTransaction] WHERE
	Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID
	) > 0 
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_check] TO HCSP

GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_check] TO HCVU
GO