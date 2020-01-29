IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_Status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update TempVoucherAccount Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_upd_Status]
	@Voucher_Acc_ID char(15),
	@Scheme_Code char(10),
	@Record_Status char(1)
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
		Record_Status = @Record_Status
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Scheme_Code = @Scheme_Code
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_Status] TO HCVU
GO
