IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_VoucherAccount_after_upd')
	DROP TRIGGER [dbo].[tri_VoucherAccount_after_upd]
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
-- Author:		Tommy Cheung
-- Create date: 21 August 2008
-- Description:	Trigger an insert statment into VoucherAccountLOG
--				when a row is updated / inserted into VoucherAccount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   30 September 2009
-- Description:	    Replace the column [Voucher_Used] and [Total_Voucher_Amt_Used] with constant value "-1"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   30 September 2009
-- Description:	    Replace the column [Voucher_Used] and [Total_Voucher_Amt_Used] with NULL
-- =============================================

CREATE TRIGGER [dbo].[tri_VoucherAccount_after_upd]
   ON		[dbo].[VoucherAccount]
   AFTER	INSERT, UPDATE
   
AS BEGIN
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

    INSERT INTO VoucherAccountLOG
		(System_Dtm,
		Voucher_Acc_ID,
		Scheme_Code,
		Voucher_Used,
		Total_Voucher_Amt_Used,
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
	SELECT getdate(),
		Voucher_Acc_ID,
		Scheme_Code,
		--Voucher_Used,
		NULL AS [Voucher_Used],
		--Total_Voucher_Amt_Used,
		NULL AS [Total_Voucher_Amt_Used],
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
	FROM inserted

END
GO
