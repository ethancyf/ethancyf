IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_get_ByHKIDAgeDOR]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByHKIDAgeDOR]
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
-- Author:		Timothy LEUNG
-- Create date: 21 October 2008
-- Description:	Retrieve Voidable transaction by HKID, Age and Date of Registration
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByHKIDAgeDOR]
	-- Add the parameters for the stored procedure here
	@HKID char(9),
	@EC_Age smallint,
	@EC_Date_of_Registration datetime,
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
		Select * from(
			select 
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
				P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				P.EC_Age = @EC_Age and
				P.EC_Date_of_Registration = @EC_Date_of_Registration and 
				P.Exact_DOB = @Exact_DOB and
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
				TVT.Record_Status
			from 
				VoucherTransaction TVT, 
				TempVoucherAccount TVA, 
				TempPersonalInformation TP
			where 
				TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				TP.EC_Age = @EC_Age and
				TP.EC_Date_of_Registration = @EC_Date_of_Registration and 
				TP.Exact_DOB = @Exact_DOB and
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
		)[VoucherTransaction]
		order by [VoucherTransaction].Transaction_Dtm
	end
	else
	begin
		Select * from(
			select 
				VT.Transaction_ID, 
				VT.Transaction_Dtm, 
				VT.Voucher_Acc_ID, 
				VT.SP_ID, 
				VT.DataEntry_By, 
				VT.Confirmed_Dtm, 
				VT.Record_Status
			from 
				VoucherTransaction VT, 
				VoucherAccount VA, 
				PersonalInformation P
			where 
				P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				P.EC_Age = @EC_Age and
				P.EC_Date_of_Registration = @EC_Date_of_Registration and 
				P.Exact_DOB = @Exact_DOB and
				P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
				VT.Voucher_Acc_ID =VA.Voucher_Acc_ID and
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
				TVT.Record_Status
			from 
				VoucherTransaction TVT, 
				TempVoucherAccount TVA, 
				TempPersonalInformation TP
			where 
				TP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
				TP.EC_Age = @EC_Age and
				TP.EC_Date_of_Registration = @EC_Date_of_Registration and 
				TP.Exact_DOB = @Exact_DOB and
				TP.Voucher_Acc_ID = TVA.Voucher_Acc_ID and
				TVT.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID and
				TVT.Scheme_Code = TVA.Scheme_Code and 
				TVT.SP_ID = @SP_ID and 
				TVT.Void_Transaction_ID is null and
				TVT.DataEntry_By = @DataEntry_By and
				(TVT.Confirmed_Dtm is Null or DateAdd(dd, 1, TVT.Confirmed_Dtm) > getdate()) and
				TVT.Record_Status = 'P'
		)[VoucherTransaction]
		order by [VoucherTransaction].Transaction_Dtm
	end

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_get_ByHKIDAgeDOR] TO HCSP
GO
