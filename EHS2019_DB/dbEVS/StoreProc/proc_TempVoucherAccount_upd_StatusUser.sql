IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_StatusUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_StatusUser]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Mar 2010
-- Description:		Grant right to [HCSP]
-- =============================================
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
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_upd_StatusUser]
	@Voucher_Acc_ID char(15),
	@Scheme_Code char(10),
	@Record_Status char(1),
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
	UPDATE [dbo].[TempVoucherAccount]
	SET
		Record_Status = @Record_Status,
		Update_Dtm = GetDate(),
		Update_By = @User_ID
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Scheme_Code = @Scheme_Code
END

GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_StatusUser] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_StatusUser] TO HCVU
GO
