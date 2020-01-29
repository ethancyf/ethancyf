IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccPendingVerify_upd_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccPendingVerify_upd_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:			Clark Yip
-- Create date:		20 Oct 2008
-- Description:		Delete record from TempVoucherAccPendingVerify table by VRAccID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 22 Sep 2009
-- Description:	Remove scheme_code
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_TempVoucherAccPendingVerify_upd_del] @temp_VR_Acct_ID		 char(15)
								, @scheme	char(10)

as
BEGIN
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

Delete from TempVoucherAccPendingVerify
where Voucher_acc_id = @temp_VR_Acct_ID
--and scheme_code=@scheme

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccPendingVerify_upd_del] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccPendingVerify_upd_del] TO HCVU
GO
