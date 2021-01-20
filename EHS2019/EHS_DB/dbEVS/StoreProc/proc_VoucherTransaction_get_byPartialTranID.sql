IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_get_byPartialTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_get_byPartialTranID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
 
 -- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================  
-- Author:   Stanley Chan  
-- Create date:  14 Oct 2008  
-- Description:  Get Claim Tran by partial transaction No.(ych480:Encryption)  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:       
-- Modified date:     
-- Description:       
-- =============================================  
  
CREATE PROCEDURE  [dbo].[proc_VoucherTransaction_get_byPartialTranID]   
 @Partial_Trans_No varchar(20)  
AS BEGIN  
-- =============================================  
-- Declaration  
-- =============================================  
declare   @first_authorised_by    as  varchar(20)  
declare   @first_authorised_date    as  datetime  
declare   @second_authorised_by   as  varchar(20)  
declare   @second_authorised_date as datetime  
declare   @reimbursed_by   as  varchar(20)  
declare   @reimbursed_date as datetime  
declare   @tsmp  as timestamp  
declare @reason_for_visit_L1 varchar(50)  
declare @reason_for_visit_L2 varchar(50)  
  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
  
EXEC [proc_SymmetricKey_open]
  
select @first_authorised_by = First_Authorised_By from ReimbursementAuthTran where transaction_id = @Partial_Trans_No  
select @first_authorised_date = First_Authorised_dtm from ReimbursementAuthTran where transaction_id = @Partial_Trans_No  
select @second_authorised_by = Second_Authorised_By from ReimbursementAuthTran where transaction_id = @Partial_Trans_No  
select @second_authorised_date = Second_Authorised_dtm from ReimbursementAuthTran where transaction_id = @Partial_Trans_No  
select @reimbursed_by =  b.submitted_by from VoucherTransaction t, BankIn b, reimbursementauthtran rat where rat.Transaction_id = t.Transaction_id and rat.reimburse_id = b.reimburse_id and t.Transaction_id=@Partial_Trans_No  
select @reimbursed_date = b.submission_dtm from VoucherTransaction t, BankIn b, reimbursementauthtran rat where rat.Transaction_id = t.Transaction_id and rat.reimburse_id = b.reimburse_id and t.Transaction_id=@Partial_Trans_No  
select @reason_for_visit_l1 = AdditionalFieldValueCode from [TransactionAdditionalField] where Transaction_ID = @Partial_Trans_No and AdditionalFieldID = 'Reason_for_Visit_L1'  
select @reason_for_visit_l2 = AdditionalFieldValueCode from [TransactionAdditionalField] where Transaction_ID = @Partial_Trans_No and AdditionalFieldID = 'Reason_for_Visit_L2'  
  
  
-- =============================================  
-- Return results  
-- =============================================  
  
 select   
  t.transaction_id as tranNum,   
  t.transaction_dtm as tranDate,  
  convert(varchar(40), DecryptByKey(a.[Encrypt_Field2])) as SPName,  
  convert(nvarchar(40), DecryptByKey(a.[Encrypt_Field3])) as [SPNameChi],  
  t.sp_id as SPID,  
  t.bank_account_no as BankAccountNo,  
  t.bank_acc_holder as BankAccountHolder,   
  t.bank_acc_display_seq as BankAccountID,  
  t.practice_display_seq as practiceid,  
  p.Practice_name as PracticeName,  
  p.Practice_name_chi as PracticeNameChi,  
  --t.Voucher_Claim as voucherRedeem,   
  --t.Per_Voucher_Value as voucherAmount,  
  0 as voucherRedeem,   
  0 as voucherAmount,  
  t.Record_status as status,  
  t.Service_receive_dtm as serviceDate,  
  t.Service_type as serviceType,  
  @reason_for_visit_l1 as visitReason_L1,  
  --t.reason_for_visit_L1 as visitReason_L1,  
  @reason_for_visit_l2 as visitReason_L2,  
  --t.reason_for_visit_L2 as visitReason_L2,  
  t.Voucher_Before_Claim as voucherBeforeClaim,  
  t.Voucher_After_Claim as voucherAfterClaim,  
  r.Authorised_status as Authorised_status,  
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
  t.Doc_Code,  
  isNull(t.Special_Acc_ID, '') as Special_Acc_ID,  
  isNull(t.Invalid_Acc_ID, '') as Invalid_Acc_ID  
    
 from   
  ServiceProvider a, Practice p, VoucherTransaction t  
   left outer join ReimbursementAuthTran r on t.transaction_id = r.transaction_id  
     
 where   
  a.SP_ID = t.SP_ID   
   and a.SP_ID = p.SP_ID   
   and t.transaction_id in (replace(@Partial_Trans_No,'*','A'), replace(@Partial_Trans_No,'*','B'),replace(@Partial_Trans_No,'*','C'))
   and t.sp_id = p.sp_id   
   and t.practice_display_seq = p.display_seq  
  
 EXEC [proc_SymmetricKey_close]
   
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_get_byPartialTranID] TO HCSP
GO
