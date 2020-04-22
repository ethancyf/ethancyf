IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionVoid_get_ByTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByTranID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Stanley Chan
-- Create date: 5 JUNE 2008
-- Description:	Retrieve Voidable transaction by Transaction No
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherTransactionVoid_get_ByTranID] 
	-- Add the parameters for the stored procedure here
	@TranNo char(20),
	@SP_ID char(8),
	@DataEntry_By varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @DataEntry_By = '' 
	begin
		select * from(
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
			VT.Transaction_ID = @TranNo and
			P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
			VT.Voucher_Acc_ID = P.Voucher_Acc_ID and
			VT.SP_ID = @SP_ID and 
			VT.Record_Status in ('A', 'V', 'P')
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
			TVT.Transaction_ID = @TranNo and
			TP.Voucher_Acc_ID = TVA.Voucher_Acc_ID and
			TVT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID and
			TVT.SP_ID = @SP_ID and 
			TVT.Record_Status in ('A', 'V', 'P')
			)[VoucherTransaction]
		order by [VoucherTransaction].Transaction_Dtm
	end
	else
	begin		
	select * from(
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
			VT.Transaction_ID = @TranNo and
			P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
			VT.Voucher_Acc_ID = P.Voucher_Acc_ID and
			VT.SP_ID = @SP_ID and 
			VT.DataEntry_By = @DataEntry_By and
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
			TVT.Transaction_ID = @TranNo and
			TP.Voucher_Acc_ID = TVA.Voucher_Acc_ID and
			TVT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID and
			TVT.SP_ID = @SP_ID and 
			TVT.DataEntry_By = @DataEntry_By and
			TVT.Record_Status = 'P')[VoucherTransaction]
		order by [VoucherTransaction].Transaction_Dtm
	end
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionVoid_get_ByTranID] TO HCSP
GO
