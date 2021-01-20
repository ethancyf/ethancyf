IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountVoid_get_byPartialHKIDSPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountVoid_get_byPartialHKIDSPID]
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
-- Author:		Tommy Cheung
-- Create date: 17 Nov 2008
-- Description:	Retrieve the Voidable Voucher Recipient 
--			    Account information by HKID and SPID (for IVRS use)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:
-- Modified date:
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccountVoid_get_byPartialHKIDSPID]
	@HKID char(7),
	@SP_ID char(8)
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
EXEC [proc_SymmetricKey_open]

Select distinct HKID
	     from(
			select
				convert(varchar, DecryptByKey(P.Encrypt_Field1)) as HKID, 
				VT.Transaction_ID , 
				VT.Transaction_Dtm , 
				VT.Voucher_Acc_ID, 
				VT.SP_ID, VT.DataEntry_By, 
				VT.Confirmed_Dtm, 
				VT.Record_Status
			from 
				VoucherTransaction VT, 
				VoucherAccount VA, 
				PersonalInformation P
			where 
				P.Encrypt_Field10 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				--P.DOB = @DOB and
				--(P.Exact_DOB = @Exact_DOB or P.Exact_DOB = 'R' or P.Exact_DOB = 'T' or P.Exact_DOB = 'U' or P.Exact_DOB = 'V') and
				P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
				VT.Voucher_Acc_ID = VA.Voucher_Acc_ID and
				VT.Scheme_Code = VA.Scheme_Code and
				VT.SP_ID = @SP_ID and 
				VT.Void_Transaction_ID is null and
				(
				(VT.Confirmed_Dtm is Null or (DateAdd(dd, 1, VT.Confirmed_Dtm) > getdate() and VT.Record_Status = 'A')) 
				or
				VT.Record_Status in ('V', 'P')
				)
			union
			select
				convert(varchar, DecryptByKey(TP.Encrypt_Field1)) as HKID, 
				TVT.Transaction_ID, 
				TVT.Transaction_Dtm, 
				TVT.Voucher_Acc_ID, 
				TVT.SP_ID, TVT.DataEntry_By, 
				TVT.Confirmed_Dtm, 
				TVT.Record_Status
			from 
				VoucherTransaction TVT, 
				TempVoucherAccount TVA, 
				TempPersonalInformation TP
			where 
				TP.Encrypt_Field10 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				--TP.DOB = @DOB and
				--(TP.Exact_DOB = @Exact_DOB or TP.Exact_DOB = 'R' or TP.Exact_DOB = 'T' or TP.Exact_DOB = 'U' or TP.Exact_DOB = 'V') and
				TP.Voucher_Acc_ID = TVA.Voucher_Acc_ID and
				TVT.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID and
				TVT.Scheme_Code = TVA.Scheme_Code and 
				TVT.SP_ID = @SP_ID and 
				TVT.Void_Transaction_ID is null and
				(
				(TVT.Confirmed_Dtm is Null or (DateAdd(dd, 1, TVT.Confirmed_Dtm) > getdate() and (TVT.Record_Status in ('A','V')))) 
				or
				TVT.Record_Status = 'P'
				or
				TVA.Record_Status = 'I'
				)
		)[VoucherTran]
		--where Exact_DOB = @Exact_DOB
		order by [VoucherTran].HKID
			
EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountVoid_get_byPartialHKIDSPID] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountVoid_get_byPartialHKIDSPID] TO HCSP
GO
