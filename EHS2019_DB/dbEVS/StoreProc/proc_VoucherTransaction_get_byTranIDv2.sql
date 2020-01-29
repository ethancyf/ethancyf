IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_byTranIDv2]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_byTranIDv2]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Add Column - [VoucherTransaction].[DHC_Service]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (IDEAS2)
-- Modified by:		Winnie SUEN
-- Modified date:	3 Jan 2019
-- Description:		Add Column - [VoucherTransaction].[SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	10 Sep 2018
-- CR No.:			CRE17-010 (OCSSS integration)
-- Description:		Add Column - [VoucherTransaction].[HKIC_Symbol]
--								 [VoucherTransaction].[OCSSS_Ref_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	04 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	Add Column    - [VoucherTransaction].[DH_Vaccine_Ref]
--                                  [VoucherTransaction].[DH_Vaccine_Ref_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	26 February 2018
-- CR No.:			I-CRE17-007
-- Description:		Performance Tuning
--					1. Add WITH (NOLOCK) to table [ReimbursementAuthTran] and [ManualReimbursement]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	25 May 2017
-- CR No.:			CRE16-026-03 (Add PCV13)
-- Description:	  	Add Column - [VoucherTransaction].[High_Risk]
--								 [VoucherTransaction].[EHS_Vaccine_Ref]
--								 [VoucherTransaction].[HA_Vaccine_Ref]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	29 Aug 2016
-- CR No.			CRE16-002
-- Description:		Revamp VSS - Add column Category_Code
-- =============================================
-- =============================================  
-- Modified by:  Winnie SUEN
-- Modified date: 13 May 2015  
-- Description:	 Add [Consent_Form_Printed]
-- =============================================  
-- =============================================  
-- Modified by:  Derek LEUNG
-- Modified date: 03 Nov 2010  
-- Description:  Do not get record status from ManualReimbursement  
-- =============================================  
-- =============================================  
-- Modified by:  Koala Cheng  
-- Modified date: 23 Jul 2010  
-- Description:  Add Ext_Ref_Status  
-- =============================================  
-- =============================================
-- Modified by:		Kathy LEE
-- Modified date:	8 Jul 2010
-- Description:		Add Creation_Reason, Creation_Remark,
--						Override_Reason, Payment_Method,
--						Payment_Remark,
--						Approval_By, Approval_Dtm,
--						Reject_By, Reject_Dtm, Manual_Reimburse
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Stanley Chan
-- Modified date:	9 Fed 2010
-- Description:		SmartID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	10 Dec 2009
-- Description:		PreSchool
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Aug 2009
-- Description:	Retrieve Concise Transaction Information For HCSP
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_get_byTranIDv2] 
	@tran_id	varchar(20)
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

	SELECT
		VT.[Transaction_ID],
		VT.[Transaction_Dtm],
		VT.[Voucher_Acc_ID],
		VT.[Temp_Voucher_Acc_ID],
		VT.[Scheme_Code],
		VT.[Service_Receive_Dtm],
		VT.[Service_Type],
		VT.[Voucher_Before_Claim],
		VT.[Voucher_After_Claim],
		VT.[SP_ID],
		VT.[Practice_Display_Seq],	
		VT.[Bank_Acc_Display_Seq],
		VT.[Bank_Account_No],
		VT.[Bank_Acc_Holder],
		VT.[DataEntry_By],
		VT.[Confirmed_Dtm],	
		VT.[Consent_Form_Printed],
		VT.[Record_Status],
		VT.[Void_Transaction_ID],
		VT.[Void_Dtm],
		VT.[Void_Remark],
		VT.[Void_By],
		VT.[Void_By_DataEntry],
		VT.[Create_Dtm],
		VT.[Create_By],
		VT.[Update_Dtm],
		VT.[Update_By],		
		VT.[TSMP],
		VT.[Void_By_HCVU],
		VT.[Claim_Amount],
		VT.[Doc_Code],
		VT.[Special_Acc_ID],
		VT.[Invalid_Acc_ID],
		VT.[PreSchool],	
		-----------------
		RAT.[Authorised_status],
		P.[Practice_Name],
		P.[Practice_Name_Chi],
		-----------------
		VT.[Create_By_SmartID],
		-----------------
		MR.[Creation_Reason],
		MR.[Creation_Remark],
		MR.[Payment_Method],
		MR.[Payment_Remark],
		MR.[Override_Reason],
		MR.[Approval_By],
		MR.[Approval_Dtm],
		MR.[Reject_By],
		MR.[Reject_Dtm],
		MR.[TSMP] as [Manual_Reimburse_TSMP],
		-----------------  
		VT.[Manual_Reimburse],
		VT.[Ext_Ref_Status],
		VT.[Category_Code],
		VT.[High_Risk],
		VT.[EHS_Vaccine_Ref],
		VT.[HA_Vaccine_Ref],
		VT.[DH_Vaccine_Ref],
		VT.[DH_Vaccine_Ref_Status],
		VT.[HKIC_Symbol],
		VT.[OCSSS_Ref_Status],
		VT.[SmartID_Ver],
		VT.[DHC_Service]
	FROM
		[VoucherTransaction] VT
			INNER JOIN [Practice] P 
				ON VT.[SP_ID] = P.[SP_ID] AND VT.[Practice_Display_Seq] = P.[Display_Seq]
			LEFT OUTER JOIN [ReimbursementAuthTran] RAT WITH (NOLOCK)
				ON RAT.[Transaction_ID] = VT.[Transaction_ID]
			LEFT OUTER JOIN [ManualReimbursement] MR WITH (NOLOCK)
				ON MR.[Transaction_ID] = VT.Transaction_ID AND ISNULL(VT.[Manual_Reimburse],'') = 'Y'

	WHERE
		VT.[Transaction_ID] = @tran_id

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byTranIDv2] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byTranIDv2] TO HCVU
GO

