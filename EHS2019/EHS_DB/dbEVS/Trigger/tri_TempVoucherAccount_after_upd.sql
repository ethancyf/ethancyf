IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_TempVoucherAccount_after_upd')
	DROP TRIGGER [dbo].[tri_TempVoucherAccount_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.			CRE17-018 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19
-- Modified date:	16 Aug 2018 
-- Description:		Add new column [SourceApp]
-- ============================================= 
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	09 Feb 2010
-- Description:	    Add column 'Original_Amend_Acc_ID' & 'Create_by_BO'
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 21 August 2008
-- Description:	Trigger an insert statment into TempVoucherAccountLOG
--				when a row is updated / inserted into TempVoucherAccount
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

CREATE TRIGGER [dbo].[tri_TempVoucherAccount_after_upd]
   ON		[dbo].[TempVoucherAccount]
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

    INSERT INTO TempVoucherAccountLOG
		(System_Dtm,
		Voucher_Acc_ID,
		Scheme_Code,
		Voucher_Used,
		Total_Voucher_Amt_Used,
		Validated_Acc_ID,
		Record_Status,
		Account_Purpose,
		Confirm_Dtm,
		Last_Fail_Validate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Original_Acc_ID,
		Original_Amend_Acc_ID,
		Create_by_BO,
		Deceased,
		SourceApp
	)
	SELECT getdate(),
		Voucher_Acc_ID,
		Scheme_Code,
		--Voucher_Used,
		NULL AS [Voucher_Used],
		--Total_Voucher_Amt_Used,
		NULL AS [Total_Voucher_Amt_Used],
		Validated_Acc_ID,
		Record_Status,
		Account_Purpose,
		Confirm_Dtm,
		Last_Fail_Validate_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Original_Acc_ID,
		Original_Amend_Acc_ID,
		Create_by_BO,
		Deceased,
		SourceApp
	FROM inserted

END
GO
