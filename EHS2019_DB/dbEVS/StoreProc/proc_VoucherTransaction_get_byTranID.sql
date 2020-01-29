IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_byTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_byTranID]
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
-- Modified by:		Koala CHENG
-- Modified date:	18 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Add Column	- [VoucherTransaction].[SourceApp]
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
-- Modified date:	26 May 2017
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
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:    Lawrence TSANG
-- Modified date:  3 November 2010
-- Description:    Handle new [VoucherTransaction].[Record_Status]:
--						B: Pending Approval
--						R: Reimbursed
-- =============================================  
-- =============================================
-- Modification History  
-- Modified by:  Derek LEUNG  
-- Modified date: 24 September 2010   
-- Description:     Retrieve [IsUpload]  
-- =============================================  
-- =============================================  
-- Modified by:		Kathy LEE
-- Modified date:	8 Jul 2010
-- Description:		Add Creation_Reason, Creation_Remark,
--						Override_Reason, Payment_Method,
--						Payment_Remark,
--						Approval_By, Approval_Dtm,
--						Reject_By, Reject_Dtm,
--						Create_By, Create_Dtm, Manual_Reimburse
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	24 March 2010	
-- Description:	    Retrieve [Invalidation]
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		29 Apr 2008
-- Description:		Get Claim Tran (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Add to select the total amount. Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   13 Aug 2009
-- Description:	    1.  Join the ReimbursementAuthTran table
--					2.	Add the PracticeNameChi field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   14 Aug 2009
-- Description:	    1.  Change to left outer Join the ReimbursementAuthTran table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   21 August 2009
-- Description:	    SELECT Doc_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   18 Sep 2009
-- Description:	    SELECT Special_Acc_ID, Invalid_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   29 Sep 2009
-- Description:	    Handle the drop of columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   30 September 2009
-- Description:	    Remove column [VoucherTransaction].[Reason_For_Visit_L1] and [VoucherTransaction].[Reason_For_Visit_L2] and
--					retrieve from [TransactionAdditionalField]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   6 October 2009
-- Description:	    Retrieve [ServiceProvider].[Encrypt_Field3]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_VoucherTransaction_get_byTranID] 
	@tran_id	varchar(20)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
declare   @first_authorised_by    as  varchar(20)
declare   @first_authorised_date    as  datetime
declare   @second_authorised_by   as  varchar(20)
declare   @second_authorised_date	as	datetime
declare   @reimbursed_by   as  varchar(20)
declare   @reimbursed_date	as	datetime
declare	  @tsmp		as	timestamp
declare @reason_for_visit_L1	varchar(50)
declare @reason_for_visit_L2	varchar(50)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

select @first_authorised_by = First_Authorised_By from ReimbursementAuthTran where transaction_id = @tran_id
select @first_authorised_date = First_Authorised_dtm from ReimbursementAuthTran where transaction_id = @tran_id
select @second_authorised_by = Second_Authorised_By from ReimbursementAuthTran where transaction_id = @tran_id
select @second_authorised_date = Second_Authorised_dtm from ReimbursementAuthTran where transaction_id = @tran_id
select @reimbursed_by =  b.submitted_by from VoucherTransaction t, BankIn b, reimbursementauthtran rat where rat.Transaction_id = t.Transaction_id and rat.reimburse_id = b.reimburse_id and t.Transaction_id=@tran_id
select @reimbursed_date = b.submission_dtm from VoucherTransaction t, BankIn b, reimbursementauthtran rat where rat.Transaction_id = t.Transaction_id and rat.reimburse_id = b.reimburse_id and t.Transaction_id=@tran_id
select @reason_for_visit_l1 = AdditionalFieldValueCode from [TransactionAdditionalField] where Transaction_ID = @tran_id and AdditionalFieldID = 'Reason_for_Visit_L1'
select @reason_for_visit_l2 = AdditionalFieldValueCode from [TransactionAdditionalField] where Transaction_ID = @tran_id and AdditionalFieldID = 'Reason_for_Visit_L2'


-- =============================================
-- Return results
-- =============================================

	SELECT 
		t.transaction_id as tranNum, 
		t.transaction_dtm as tranDate,
		convert(VARCHAR(40), DecryptByKey(a.[Encrypt_Field2])) as SPName,
		convert(NVARCHAR(40), DecryptByKey(a.[Encrypt_Field3])) as [SPNameChi],
		t.sp_id as SPID,
		t.bank_account_no as BankAccountNo,
		t.bank_acc_holder as BankAccountHolder, 
		t.bank_acc_display_seq as BankAccountID,
		t.practice_display_seq as practiceid,
		p.Practice_name as PracticeName,
		p.Practice_name_chi as PracticeNameChi,
		0 as voucherRedeem, 
		0 as voucherAmount,
		t.Record_Status AS [status],
		t.Service_receive_dtm as serviceDate,
		t.Service_type as serviceType,
		@reason_for_visit_l1 as visitReason_L1,
		@reason_for_visit_l2 as visitReason_L2,
		t.Voucher_Before_Claim as voucherBeforeClaim,
		t.Voucher_After_Claim as voucherAfterClaim,
		r.Authorised_Status AS [Authorised_status],
		t.Voucher_Acc_ID, 
		t.Temp_Voucher_Acc_ID,
		t.DataEntry_by,
		t.confirmed_dtm,
		t.Void_Transaction_ID,
		t.Void_Dtm,
		t.Void_Remark,
		t.Void_By,
		@first_authorised_by as firstAuthorizedBy,
		@first_authorised_date as firstAuthorizedDate,
		@second_authorised_by as secondAuthorizedBy,
		@second_authorised_date as secondAuthorizedDate,
		@reimbursed_date as ReimbursedDate,
		@reimbursed_by as ReimbursedBy,
		t.tsmp,
		t.Scheme_Code,
		t.Void_By_DataEntry as Void_By_DataEntry,
		isNull(t.Void_By_HCVU,'') as Void_By_HCVU,
		t.Claim_Amount as totalAmount,
		t.SourceApp,
		t.Doc_Code,
		isNull(t.Special_Acc_ID, '') as Special_Acc_ID,
		isNull(t.Invalid_Acc_ID, '') as Invalid_Acc_ID,
		isNull(t.Invalidation, '') as Invalidation,
		t.[Create_By],
		t.[Create_Dtm],
		t.[Create_By_SmartID],
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
		t.[Manual_Reimburse],
		t.[Ext_Ref_Status], 
		t.[IsUpload],
		t.[Category_Code],
		t.[High_Risk],  
		t.[EHS_Vaccine_Ref],
		t.[HA_Vaccine_Ref],
		t.[DH_Vaccine_Ref],
		t.[DH_Vaccine_Ref_Status],
		t.[HKIC_Symbol],
		t.[OCSSS_Ref_Status],
		t.[SmartID_Ver],
		t.[DHC_Service]
	FROM 
		ServiceProvider a, Practice p, VoucherTransaction t
			left outer join ReimbursementAuthTran r on t.transaction_id = r.transaction_id
		   
		   LEFT OUTER JOIN [ManualReimbursement] MR
				ON MR.[Transaction_ID] = T.Transaction_ID AND ISNULL(T.[Manual_Reimburse],'') = 'Y'
			
	WHERE 
		a.SP_ID = t.SP_ID 
			and a.SP_ID = p.SP_ID 
			and t.transaction_id = @tran_id
			and t.sp_id = p.sp_id 
			and t.practice_display_seq = p.display_seq

	CLOSE SYMMETRIC KEY sym_Key
	
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byTranID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byTranID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byTranID] TO WSEXT
GO

