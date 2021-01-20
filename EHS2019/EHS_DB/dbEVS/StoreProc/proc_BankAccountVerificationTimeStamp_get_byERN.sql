IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccountVerificationTimeStamp_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccountVerificationTimeStamp_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Clark Yip
-- Create date: 9 July 2008
-- Description:	Retrieve the TimeStamp, Bank Account, SP, Practice information (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP
-- Modified date: 11 May 2009
-- Description:	  Remove the practice_type
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccountVerificationTimeStamp_get_byERN] 	
	@enrolment_ref_no	Char(15)	
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
    EXEC [proc_SymmetricKey_open]
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
-- =============================================
-- Return results
-- =============================================
	SELECT	distinct b.enrolment_ref_no as appNum, 
		convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as SPName, --sp.sp_eng_name  as SPName,
		convert(nvarchar, DecryptByKey(sp.[Encrypt_Field3])) as SPChiName, --sp.sp_chi_name  as SPChiName,
		p.practice_name as practiceName,
		--p.practice_type as practiceType,
		b.bank_acc_Holder as bankAccountOwner,
		b.bank_name as BankName,
		b.branch_name as BranchName,
		b.bank_account_no as bankAccountNum,
		prof.registration_code as businessRegNum,
		b.record_status as status,
		bv.record_status as bankStatus,
		bv.display_seq as displaySeq,
		u.tsmp as TSMP,		
		bv.tsmp as BankAccVerTSMP,
		b.tsmp as BankAccStagingTSMP,
		bv.sp_practice_display_seq,
		sp.SP_ID as SPID,
		convert(varchar, DecryptByKey(sp.[Encrypt_Field1])) as SPHKID, --sp.SP_HKID as SPHKID,
		sp.Phone_Daytime as DaytimeContact,
		prof.Service_Category_Code as HealthProf
FROM	BankAccountStaging b, ServiceProviderStaging sp, PracticeStaging p, BankAccVerification bv, SPAccountUpdate u, ProfessionalStaging prof
WHERE	b.Enrolment_ref_no = sp.Enrolment_ref_no and p.Enrolment_ref_no = prof.Enrolment_ref_no and p.Professional_seq = prof.professional_seq
		and b.Enrolment_ref_no = p.Enrolment_ref_no
		and b.Enrolment_ref_no = bv.Enrolment_ref_no 
		and b.display_seq = bv.display_seq
		and b.Enrolment_ref_no = u.Enrolment_ref_no 		
		and b.sp_practice_display_seq = p.display_seq
		and bv.sp_practice_display_seq = b.sp_practice_display_seq
		and u.progress_status='B'		
		AND b.Enrolment_ref_no = @enrolment_ref_no

		EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccountVerificationTimeStamp_get_byERN] TO HCVU
GO
