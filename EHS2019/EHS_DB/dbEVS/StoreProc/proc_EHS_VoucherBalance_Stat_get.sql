IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_VoucherBalance_Stat_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_VoucherBalance_Stat_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Oct 2008
-- Description:	Statistics for getting voucher account and claim
--				1. Get related data to temp table (__VoucherClaimByVoucherAcc, _TransactionByVoucherRemainZero)
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_VoucherBalance_Stat_get]

AS
BEGIN

	SET NOCOUNT ON;
	
declare @start_dtm varchar(50)
declare @end_dtm varchar(50)
select @start_dtm  =  CONVERT(VARCHAR(10), GETDATE(), 6) + ' 00:00'
select @end_dtm  =  CONVERT(VARCHAR(10), GETDATE(), 6) + ' 01:00'

create table #temp
(
	title varchar(50),
	voucher_claim_5 int,
	voucher_claim_4 int,
	voucher_claim_3 int,
	voucher_claim_2 int,
	voucher_claim_1 int,
	voucher_claim_0 int,
	voucher_claim_total int
)

insert into #temp
(
	title,
	voucher_claim_5,
	voucher_claim_4,
	voucher_claim_3,
	voucher_claim_2,
	voucher_claim_1,
	voucher_claim_0,
	voucher_claim_total
)
select 'No. of Voucher Account', voucher_claim_5, voucher_claim_4, voucher_claim_3, voucher_claim_2, voucher_claim_1, voucher_claim_0, voucher_claim_total
	from _VoucherClaimByVoucherAcc
	where system_dtm between @start_dtm and @end_dtm
	
insert into #temp
(
	title,
	voucher_claim_5,
	voucher_claim_4,
	voucher_claim_3,
	voucher_claim_2,
	voucher_claim_1,
	voucher_claim_0,
	voucher_claim_total
)
select 'No. of Vouchers', voucher_claim_5*0, voucher_claim_4*1, voucher_claim_3*2, voucher_claim_2*3, voucher_claim_1*4, voucher_claim_0*5, 
	voucher_claim_5*0+voucher_claim_4*1+voucher_claim_3*2+voucher_claim_2*3+voucher_claim_1*4+voucher_claim_0*5
	from _VoucherClaimByVoucherAcc
	where system_dtm between @start_dtm and @end_dtm
	
	select * from #temp
	
	select 'No. of Voucher Account', transaction_1, transaction_2, transaction_3, transaction_4, transaction_5, transaction_total 
	from _TransactionByVoucherRemainZero
	where system_dtm between @start_dtm and @end_dtm
	
	drop table #temp
	
	END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_VoucherBalance_Stat_get] TO HCVU
GO
