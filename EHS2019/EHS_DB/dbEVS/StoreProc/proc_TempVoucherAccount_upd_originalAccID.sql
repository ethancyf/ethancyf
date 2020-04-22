IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_upd_originalAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_upd_originalAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		20-12-2008
-- Description:		Update Orginal Voucher Account ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_TempVoucherAccount_upd_originalAccID]

@Voucher_Acc_ID char(15),
@Original_Acc_ID char(15)

AS

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Insert Transcation
-- =============================================

UPDATE TempVoucherAccount

SET Original_Acc_ID = @Original_Acc_ID
	
WHERE Voucher_Acc_ID = @Voucher_Acc_ID
	


GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_upd_originalAccID] TO HCSP
GO
