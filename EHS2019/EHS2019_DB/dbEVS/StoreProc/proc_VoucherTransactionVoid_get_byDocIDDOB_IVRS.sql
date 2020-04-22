IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_get_byDocIDDOB_IVRS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_byDocIDDOB_IVRS]
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
-- Modified by:	    Chris YIM
-- Modified date:	25 May 2017
-- CR No.:			CRE16-026-03 (Add PCV13)
-- Description:	  	Add Column - [VoucherTransaction].[High_Risk]
--								 [VoucherTransaction].[EHS_Vaccine_Ref]
--								 [VoucherTransaction].[HA_Vaccine_Ref]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT16-0017 (Fix IVRS cannot search voidable transaction by VR info)
-- Modified by:	    Lawrence TSANG
-- Modified date:	1 November 2016
-- Description:	  	Fix IVRS cannot search voidable transaction by VR info
-- =============================================
-- =============================================  
-- Modified by:  Winnie SUEN
-- Modified date: 13 May 2015  
-- Description:	 Add [Consent_Form_Printed]
-- =============================================  
-- =============================================
-- Modification History
-- CR No.:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:	    Lawrence TSANG
-- Modified date:	11 Jun 2014
-- Description:	  	Search account by provided Doc_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	30 Aug 2010
-- Description:		Filter out the Manual Reimburse Transaction
--					Add back [Create_By_SmartID]
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
-- Author:  Stanley Chan  
-- Create date: 12 Oct 2009  
-- Description: Retrieve Voidable Transaction By Document & IdentityNum  for IVRS
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  
-- Modified date:  
-- Description:  
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_byDocIDDOB_IVRS]   
	@Doc_Code char(20),
	@identity varchar(20),  
	@DOB datetime,  
	@Exact_DOB char(1),  
	@SP_ID char(8)
AS  
BEGIN  
  
	SET NOCOUNT ON;  
-- =============================================  
-- Declaration  
-- =============================================  
	DECLARE @tmpTransaction TABLE(  
		Transaction_ID char(20)  
	)  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
	OPEN SYMMETRIC KEY sym_Key   
		DECRYPTION BY ASYMMETRIC KEY asym_Key   
-- =============================================  
-- Return results  
-- =============================================  
  
	-- Voucher Account Related Transaction   
	INSERT INTO @tmpTransaction (Transaction_ID)  
	SELECT  
		VT.[Transaction_ID]  
	FROM   
		[VoucherTransaction] VT  
			INNER JOIN [VoucherAccount] VA 
				ON VT.[Voucher_Acc_ID] = VA.[Voucher_Acc_ID]  
			INNER JOIN [PersonalInformation] P
				ON VA.[Voucher_Acc_ID] =  P.[Voucher_Acc_ID]  
	WHERE  
		P.[Doc_Code] = @Doc_Code AND   
		P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND  
		P.DOB = @DOB AND
		( P.Exact_DOB = @Exact_DOB OR   
			(@Exact_DOB = 'Y' AND (P.Exact_DOB = 'R' OR P.Exact_DOB = 'V')) OR  
			(@Exact_DOB = 'M' AND  P.Exact_DOB = 'U') OR  
			(@Exact_DOB = 'D' AND  P.Exact_DOB = 'T')  
		) AND  
		VT.[SP_ID] = @SP_ID AND  
		VT.[Scheme_Code] = 'HCVS' AND
		VT.[Void_Transaction_ID] IS NULL AND 
		-- Service Provider Case  
		((VT.Confirmed_Dtm is Null OR (DateAdd(dd, 1, VT.Confirmed_Dtm) > GetDate() and VT.Record_Status = 'A')) OR  
			VT.Record_Status in ('V', 'P'))
		AND (VT.[Manual_Reimburse] IS NULL OR VT.[Manual_Reimburse] <> 'Y') 
  
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
			LEFT OUTER JOIN [ReimbursementAuthTran] RAT  
				ON RAT.[Transaction_ID] = VT.[Transaction_ID] 
			LEFT OUTER JOIN [ManualReimbursement] MR
				ON MR.[Transaction_ID] = VT.[Transaction_ID] AND ISNULL(VT.[Manual_Reimburse],'') = 'Y'
	WHERE  
		VT.[Transaction_ID] IN   
			( SELECT Distinct([Transaction_ID]) FROM @tmpTransaction )  
    
	CLOSE SYMMETRIC KEY sym_Key  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_get_byDocIDDOB_IVRS] TO HCSP
GO

