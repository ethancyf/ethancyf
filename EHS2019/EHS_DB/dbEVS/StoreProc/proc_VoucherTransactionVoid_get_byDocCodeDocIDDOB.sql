IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_get_byDocCodeDocIDDOB]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_byDocCodeDocIDDOB]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- CRE13-001 EHAPP
-- Modification History
-- Modified by:		Karl LAM
-- Modified date:	12 Apr 2013
-- Description:		Add Joined (J)
-- =============================================
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tommy TSE
-- Modified date:	20 Oct 2011
-- Description:		Add Incomplete
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	30 Aug 2010
-- Description:		Filter out the Manual Reimburse Transaction
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
-- Modified by:		Vincent YUEN
-- Modified date:	09 Dec 2009
-- Description:		Add Default Sorting
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	12 Oct 2009
-- Description:		Add Back DOB Checking
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 Sep 2009
-- Description:	Retrieve Voidable Transaction By Document & IdentityNum
-- =============================================


CREATE PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_byDocCodeDocIDDOB] 
	@Doc_Code char(20),
	@identity varchar(20),
	@Adoption_Prefix as char(7),
	@DOB datetime,
	@Exact_DOB char(1),
	@SP_ID char(8),
	@DataEntry_By varchar(20)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

DECLARE @tmpTransaction TABLE
(
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
			INNER JOIN [VoucherAccount] VA ON
				VT.[Voucher_Acc_ID] = VA.[Voucher_Acc_ID]
			INNER JOIN [PersonalInformation] P ON
				VA.[Voucher_Acc_ID] =  P.[Voucher_Acc_ID]
	WHERE
		P.[Doc_Code] = @Doc_Code AND 
		P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND
		(@Adoption_Prefix = '' OR P.[Encrypt_Field11] = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix)) AND
		[DOB] = @DOB AND
		(	P.Exact_DOB = @Exact_DOB OR 
			(@Exact_DOB = 'Y' AND (P.Exact_DOB = 'R' OR P.Exact_DOB = 'V')) OR
			(@Exact_DOB = 'M' AND  P.Exact_DOB = 'U') OR
			(@Exact_DOB = 'D' AND  P.Exact_DOB = 'T')
		) AND
		VT.[SP_ID] = @SP_ID AND
		VT.[Void_Transaction_ID] IS NULL AND
		(
			(
				-- Service Provider Case
				@DataEntry_By = '' AND 
				(
					(VT.Confirmed_Dtm is Null OR (DateAdd(dd, 1, VT.Confirmed_Dtm) > GetDate() and VT.Record_Status in ('A', 'J'))) OR
					VT.Record_Status in ('P', 'U', 'V')
				)
			) OR (
				-- DataEntry Case
				-- Self created and (not yet confirm by SP or confirmed within 24 hour)
				@DataEntry_By <> '' AND 
				VT.[DataEntry_By] = @DataEntry_By AND
				(VT.[Confirmed_Dtm] IS NULL OR DateAdd(dd,1,VT.[Confirmed_Dtm]) > GetDate()) OR
				VT.[Record_Status] in ('P', 'U')
			)
		)		
		AND (VT.[Manual_Reimburse] IS NULL OR VT.[Manual_Reimburse] <> 'Y') 
		
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
		(@Adoption_Prefix = '' OR P.[Encrypt_Field11] = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix)) AND
		[DOB] = @DOB AND
		(	P.Exact_DOB = @Exact_DOB OR 
			(@Exact_DOB = 'Y' AND (P.Exact_DOB = 'R' OR P.Exact_DOB = 'V')) OR
			(@Exact_DOB = 'M' AND  P.Exact_DOB = 'U') OR
			(@Exact_DOB = 'D' AND  P.Exact_DOB = 'T')) AND		
		VT.[SP_ID] = @SP_ID AND
		VT.[Void_Transaction_ID] IS NULL AND
		(
			(
				-- Service Provider Case
				@DataEntry_By = '' AND 
				(
					(VT.Confirmed_Dtm is Null or (DateAdd(dd, 1, VT.Confirmed_Dtm) > GetDate() and VT.Record_Status in ('A','V','J')) OR
					VT.Record_Status in ('P', 'U') OR
					VA.Record_Status ='I'
				)
			) OR (
				-- DataEntry Case
				-- Self created and (not yet confirm by SP or confirmed within 24 hour)
				@DataEntry_By <> '' AND 
				VT.[DataEntry_By] = @DataEntry_By AND
				(VT.[Confirmed_Dtm] IS NULL OR DateAdd(dd,1,VT.[Confirmed_Dtm]) > GetDate())) AND
				VT.Record_Status in ('P', 'U')
			)
		)
		AND (VT.[Manual_Reimburse] IS NULL OR VT.[Manual_Reimburse] <> 'Y') 
		
	CLOSE SYMMETRIC KEY sym_Key

	-- Return Result
	SELECT
		VT.[Transaction_ID],
		VT.[Transaction_Dtm],
		VT.[Voucher_Acc_ID],
		VT.[Temp_Voucher_Acc_ID],
		VT.[Scheme_Code],		
		--VT.[Voucher_Claim],
		--VT.[Per_Voucher_Value],
		VT.[Service_Receive_Dtm],
		VT.[Service_Type],
		--VT.[Reason_for_Visit_L1],		
		--VT.[Reason_for_Visit_L2],
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
		--VT.[Consent_Form_Printed],
		VT.[Record_Status],
		--VT.[Authorised_Status],
		--VT.[Reimburse_ID],
		VT.[Void_Transaction_ID],
		VT.[Void_Dtm],
		VT.[Void_Remark],
		VT.[Void_By],		
		VT.[Void_By_DataEntry],
		--VT.[TSWProgram],
		VT.[Create_Dtm],
		VT.[Create_By],
		VT.[Update_Dtm],
		VT.[Update_By],		
		--VT.[Authorised_Cutoff_Dtm],
		--VT.[Authorised_Cutoff_By],
		VT.[TSMP],
		VT.[Void_By_HCVU],
		VT.[Claim_Amount],
		--VT.[SourceApp],
		VT.[Doc_Code],
		VT.[Special_Acc_ID],		
		VT.[Invalid_Acc_ID],
		VT.[Create_By_SmartID],
		-----------------
		RAT.[Authorised_status],
		-----------------
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
		  VT.[Manual_Reimburse],
		    -----------------  
		VT.[Ext_Ref_Status] 
		
	FROM
		[VoucherTransaction] VT
			INNER JOIN [Practice] P 
				ON VT.[SP_ID] = P.[SP_ID] AND VT.[Practice_Display_Seq] = P.[Display_Seq]
			LEFT OUTER JOIN [ReimbursementAuthTran] RAT
				ON RAT.[Transaction_ID] = VT.[Transaction_ID]
			LEFT OUTER JOIN [ManualReimbursement] MR
				ON MR.[Transaction_ID] = VT.Transaction_ID AND ISNULL(VT.[Manual_Reimburse],'') = 'Y'
	WHERE
		VT.[Transaction_ID] in 
		( SELECT Distinct([Transaction_ID]) FROM @tmpTransaction )
	ORDER BY VT.[Transaction_Dtm] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_get_byDocCodeDocIDDOB] TO HCSP
GO
