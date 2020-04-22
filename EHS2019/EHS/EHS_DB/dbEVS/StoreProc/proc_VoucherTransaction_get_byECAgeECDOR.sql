IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_byECAgeECDOR]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_byECAgeECDOR]
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
-- Modified date:	14 July 2017
-- CR No.:			CRE16-026-03 (Add PCV13)
-- Description:	  	Add Column - [VoucherTransaction].[High_Risk]
--								 [VoucherTransaction].[EHS_Vaccine_Ref]
--								 [VoucherTransaction].[HA_Vaccine_Ref]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:	26 May 2017
-- CR No.:			INT17-0008
-- Description:	  	Fix text-only version cannot search voidable transaction by VR info
-- =============================================
-- =============================================  
-- Modified by:  Winnie SUEN
-- Modified date: 13 May 2015  
-- Description:	 Add [Consent_Form_Printed]
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	10 March 2015
-- Description:		Add Input Parameter "Available_HCSP_SubPlatform"
-- =============================================  
-- =============================================
-- Author:		Koala CHENG
-- CR NO.:		CRE13-001 EHAPP
-- Create date: 24 Apr 2013
-- Description:	Modify [proc_VoucherTransactionVoid_get_byECAgeECDOR] to search all transaction for text only version search
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransaction_get_byECAgeECDOR] 
	@Doc_Code char(20),
	@identity varchar(20),
	@EC_Age int,
	@DOR datetime,
	@SP_ID char(8),
	@DataEntry_By varchar(20),
	@Available_HCSP_SubPlatform	char(2)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @In_Available_HCSP_SubPlatform	char(2)
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

	DECLARE @tmpTransaction TABLE
	(
		Transaction_ID char(20),
		Transaction_Dtm	datetime
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
			INNER JOIN [VoucherAccount] VA ON
				VT.[Voucher_Acc_ID] = VA.[Voucher_Acc_ID]
			INNER JOIN [PersonalInformation] P ON
				VA.[Voucher_Acc_ID] =  P.[Voucher_Acc_ID]
	WHERE
		P.[Doc_Code] = @Doc_Code AND 
		P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
		EC_Age = @EC_Age AND
		EC_Date_of_Registration = @DOR AND
		P.Exact_DOB = 'A' AND
		VT.[SP_ID] = @SP_ID AND
		(@DataEntry_By = '' OR VT.[DataEntry_By] = @DataEntry_By) AND
		VT.Record_Status NOT IN ('B', 'D') 
		
	-- Temp Voucher Account Related Transaction
	INSERT INTO @tmpTransaction (Transaction_ID)
	SELECT
		VT.[Transaction_ID]
	FROM 
		[VoucherTransaction] VT
			INNER JOIN [TempVoucherAccount] VA ON
				VT.[Temp_Voucher_Acc_ID] = VA.[Voucher_Acc_ID]
			INNER JOIN [TempPersonalInformation] P ON
				VA.[Voucher_Acc_ID] =  P.[Voucher_Acc_ID]
	WHERE
		P.[Doc_Code] = @Doc_Code AND 
		P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
		EC_Age = @EC_Age AND
		EC_Date_of_Registration = @DOR AND
		P.Exact_DOB = 'A' AND
		VT.[SP_ID] = @SP_ID AND
		(@DataEntry_By = '' OR VT.[DataEntry_By] = @DataEntry_By) AND
		VT.Record_Status NOT IN ('B', 'D') 
						
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
		-----------------
		P.[Practice_Name],
		P.[Practice_Name_Chi],
		-----------------
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
		VT.[Create_By_SmartID],
		-----------------
		RAT.[Authorised_status],
		-----------------
		MR.[Creation_Reason],
		MR.[Creation_Reason],
		MR.[Creation_Remark],
		MR.[Override_Reason],
		MR.[Payment_Method],
		MR.[Payment_Remark],
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
				ON MR.[Transaction_ID] = VT.Transaction_ID AND ISNULL(VT.[Manual_Reimburse],'') = 'Y'
			LEFT JOIN SchemeClaim SC
				ON VT.Scheme_Code = SC.Scheme_Code

	WHERE
		VT.[Transaction_ID] in 
		( SELECT Distinct([Transaction_ID]) FROM @tmpTransaction )
		AND (@In_Available_HCSP_SubPlatform is null or SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byECAgeECDOR] TO HCSP
GO

