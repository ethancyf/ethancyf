IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_get_ByHKIDDOB]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByHKIDDOB]
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
-- Author:			Timothy LEUNG
-- Create date:		4 June 2008
-- Description:		Retrieve Voidable transaction by HKID and DOB
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Timothy LEUNG
-- Modified date:	4 November 2008
-- Description:		Add EC DOB option for retrieving criteria
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByHKIDDOB]
	-- Add the parameters for the stored procedure here
	@HKID char(9),
	@DOB datetime,
	@Exact_DOB char(1),
	@SP_ID char(8),
	@DataEntry_By varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

EXEC [proc_SymmetricKey_open]

    -- Insert statements for procedure here
	if @DataEntry_By = '' 
	begin
		Select Transaction_ID,
				Transaction_Dtm , 
				Voucher_Acc_ID, 
				SP_ID, 
				DataEntry_By, 
				Confirmed_Dtm, 
				Record_Status
	     from(
			select 
				VT.Transaction_ID , 
				VT.Transaction_Dtm , 
				VT.Voucher_Acc_ID, 
				VT.SP_ID, VT.DataEntry_By, 
				VT.Confirmed_Dtm, 
				VT.Record_Status,
				Case P.Exact_DOB 
					when 'R' then 'Y'
					when 'T' then 'D'
					when 'U' then 'M'
					when 'V' then 'Y'
					else P.Exact_DOB
				end as Exact_DOB
			from 
				VoucherTransaction VT, 
				VoucherAccount VA, 
				PersonalInformation P
			where 
				P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				P.DOB = @DOB and
				(P.Exact_DOB = @Exact_DOB or P.Exact_DOB = 'R' or P.Exact_DOB = 'T' or P.Exact_DOB = 'U' or P.Exact_DOB = 'V') and
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
				TVT.Transaction_ID, 
				TVT.Transaction_Dtm, 
				TVT.Voucher_Acc_ID, 
				TVT.SP_ID, TVT.DataEntry_By, 
				TVT.Confirmed_Dtm, 
				TVT.Record_Status,
				Case TP.Exact_DOB 
					when 'R' then 'Y'
					when 'T' then 'D'
					when 'U' then 'M'
					when 'V' then 'Y'
					else TP.Exact_DOB
				end as Exact_DOB
			from 
				VoucherTransaction TVT, 
				TempVoucherAccount TVA, 
				TempPersonalInformation TP
			where 
				TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				TP.DOB = @DOB and
				(TP.Exact_DOB = @Exact_DOB or TP.Exact_DOB = 'R' or TP.Exact_DOB = 'T' or TP.Exact_DOB = 'U' or TP.Exact_DOB = 'V') and
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
		where Exact_DOB = @Exact_DOB
		order by [VoucherTran].Transaction_Dtm
	end
	else
	begin
		Select Transaction_ID,
				Transaction_Dtm , 
				Voucher_Acc_ID, 
				SP_ID,
				DataEntry_By, 
				Confirmed_Dtm, 
				Record_Status
		 from(
			select 
				VT.Transaction_ID, 
				VT.Transaction_Dtm, 
				VT.Voucher_Acc_ID, 
				VT.SP_ID, 
				VT.DataEntry_By, 
				VT.Confirmed_Dtm, 
				VT.Record_Status,
				Case P.Exact_DOB 
					when 'R' then 'Y'
					when 'T' then 'D'
					when 'U' then 'M'
					when 'V' then 'Y'
					else P.Exact_DOB
				end as Exact_DOB
			from 
				VoucherTransaction VT, 
				VoucherAccount VA, 
				PersonalInformation P
			where 
				P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				P.DOB = @DOB and
				(P.Exact_DOB = @Exact_DOB or P.Exact_DOB = 'R' or P.Exact_DOB = 'T' or P.Exact_DOB = 'U' or P.Exact_DOB = 'V') and
				P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
				VT.Voucher_Acc_ID = VA.Voucher_Acc_ID and
				VT.Scheme_Code = VA.Scheme_Code and
				VT.SP_ID = @SP_ID and 
				VT.Void_Transaction_ID is null and
				VT.DataEntry_By = @DataEntry_By and
				(VT.Confirmed_Dtm is Null or DateAdd(dd, 1, VT.Confirmed_Dtm) > getdate()) and
				VT.Record_Status = 'P'
			union
			select 
				TVT.Transaction_ID, 
				TVT.Transaction_Dtm, 
				TVT.Voucher_Acc_ID, 
				TVT.SP_ID, 
				TVT.DataEntry_By, 
				TVT.Confirmed_Dtm, 
				TVT.Record_Status,
				Case TP.Exact_DOB 
					when 'R' then 'Y'
					when 'T' then 'D'
					when 'U' then 'M'
					when 'V' then 'Y'
					else TP.Exact_DOB
				end as Exact_DOB
			from 
				VoucherTransaction TVT, 
				TempVoucherAccount TVA, 
				TempPersonalInformation TP
			where 
				TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				TP.DOB = @DOB and
				(TP.Exact_DOB = @Exact_DOB or TP.Exact_DOB = 'R' or TP.Exact_DOB = 'T' or TP.Exact_DOB = 'U' or TP.Exact_DOB = 'V') and
				TP.Voucher_Acc_ID = TVA.Voucher_Acc_ID and
				TVT.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID and
				TVT.Scheme_Code = TVA.Scheme_Code and 
				TVT.SP_ID = @SP_ID and 
				TVT.Void_Transaction_ID is null and
				TVT.DataEntry_By = @DataEntry_By and
				(TVT.Confirmed_Dtm is Null or DateAdd(dd, 1, TVT.Confirmed_Dtm) > getdate()) and
				TVT.Record_Status = 'P'
		)[VoucherTran]
		where Exact_DOB = @Exact_DOB
		order by [VoucherTran].Transaction_Dtm
	end

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_get_ByHKIDDOB] TO HCSP
GO
