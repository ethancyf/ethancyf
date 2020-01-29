IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_Get_ByTransactionDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_Get_ByTransactionDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	04 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	Add Column    - [VoucherTransaction].[DH_Vaccine_Ref_Status]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Get VoucherTransaction by Transaction_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_Get_ByTransactionDtm]
	@Transaction_Dtm_From	datetime,
	@Transaction_Dtm_To		datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Return
-- =============================================

	SELECT
		Transaction_ID,
		Transaction_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Scheme_Code,
		Service_Receive_Dtm,
		Service_Type,
		Voucher_Before_Claim,
		Voucher_After_Claim,
		SP_ID,
		Practice_Display_Seq,
		Bank_Acc_Display_Seq,
		Bank_Account_No,
		Bank_Acc_Holder,
		DataEntry_By,
		Confirmed_Dtm,
		Consent_Form_Printed,
		Record_Status,
		Void_Transaction_ID,
		Void_Dtm,
		Void_Remark,
		Void_By,
		Void_By_DataEntry,
		TSWProgram,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		TSMP,
		Void_By_HCVU,
		Claim_Amount,
		SourceApp,
		Doc_Code,
		Special_Acc_ID,
		Invalid_Acc_ID,
		PreSchool,
		Invalidation,
		Create_By_SmartID,
		Manual_Reimburse,
		Ext_Ref_Status,
		DH_Vaccine_Ref_Status
	FROM
		VoucherTransaction
	WHERE
		Transaction_Dtm BETWEEN @Transaction_Dtm_From AND @Transaction_Dtm_To
	ORDER BY
		Transaction_Dtm DESC


END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_Get_ByTransactionDtm] TO HCVU
GO

