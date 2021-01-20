IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_get_ByPartialTransNo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByPartialTransNo]
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
-- Author:			Tommy Cheung
-- Create date:		11 Nov 2008
-- Description:		Retrieve Voidable transaction by Partial Transaction No
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tommy Cheung
-- Modified date:	10 Dec 2008
-- Description:		Pass in Transaction No is full trans no, but * is used when contains English alphabet.
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByPartialTransNo]
	-- Add the parameters for the stored procedure here
	@Partial_Trans_No varchar(20),
	@SP_ID char(8)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

EXEC [proc_SymmetricKey_open]

    -- Insert statements for procedure here
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
			VT.Record_Status
		from 
			VoucherTransaction VT, 
			VoucherAccount VA, 
			PersonalInformation P
		where 
			VT.Transaction_ID in (replace(@Partial_Trans_No,'*','A'), replace(@Partial_Trans_No,'*','B'),replace(@Partial_Trans_No,'*','C')) and
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
			TVT.Transaction_ID in (replace(@Partial_Trans_No,'*','A'), replace(@Partial_Trans_No,'*','B'),replace(@Partial_Trans_No,'*','C')) and
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
	order by [VoucherTran].Transaction_Dtm

EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_get_ByPartialTransNo] TO HCSP
GO
