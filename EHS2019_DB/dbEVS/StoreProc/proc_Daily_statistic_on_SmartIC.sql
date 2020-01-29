IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Daily_statistic_on_SmartIC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Daily_statistic_on_SmartIC]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Cheuk LAI
-- Create date:		2 September 2010
-- Description:		Generate Report for Daily Statistic on (1) Smart IC and (2) Search Account Manually
-- =============================================

CREATE PROCEDURE [dbo].[proc_Daily_statistic_on_SmartIC] 
	-- @Cutoff_Dtm	datetime
AS BEGIN









declare @start_time_temp datetime
declare @start_time datetime
declare @end_time_temp datetime
declare @end_time datetime

--select @start_time		= '20100802  00:00:00'
select @start_time_temp	= DateAdd(day, -14, GETDATE() ) 
select @start_time		= convert(varchar(4), YEAR(@start_time_temp)) + right('00'+ convert(varchar, MONTH(@start_time_temp)) , 2) + right('00'+ convert(varchar, DAY(@start_time_temp)), 2) + '  00:00:00'
select @end_time_temp	= DateAdd(day, -1, GETDATE() )  
select @end_time		= convert(varchar(4), YEAR(@end_time_temp)) + right('00'+ convert(varchar, MONTH(@end_time_temp)) , 2) + right('00'+ convert(varchar, DAY(@end_time_temp)), 2) + '  23:59:59'
--select @end_time		= DateAdd(day, -2, GETDATE() ) 
--select distinct @end_time_temp, @end_time from vouchertransaction



select 'Report Generation Time: ' + CONVERT(varchar, GETDATE())


-- min date of smart IC

SELECT distinct [SP_ID] 
INTO #tmp_SP 
FROM [VoucherTransaction]
WHERE [Transaction_Dtm] >= @start_time - 1
--select distinct @start_time + 14 from vouchertransaction

select v.sp_id, v.practice_display_seq
	,min(v.create_dtm) as min_smartIC_date
	,' ' as last_pilot_run
into #tmp_smartIC
from 
	vouchertransaction v 
	inner join #tmp_SP p	
		on v.sp_id = p.sp_id 
	where  	
		v.Transaction_Dtm >= '20100614' and
		v.sp_id not in (	select SP_ID from [SPExceptionList]	  )
/*		and v.sp_id in (	
			SELECT distinct [SP_ID] FROM [VoucherTransaction]
			WHERE [Transaction_Dtm] >= '2010-08-26 00:00:00'
		)
*/		and isnull(v.create_by_smartid,'') = 'Y'
group by v.sp_id, v.practice_display_seq
order by v.sp_id, v.practice_display_seq

update #tmp_smartIC
	set last_pilot_run = 'Y'
	where min_smartIC_date < @start_time




-- Check the daily count of Smart IC tx
select 
left(convert(varchar, transaction_dtm, 120),10) 
, count(distinct v.sp_id) as [SP Count] , count(distinct (v.sp_id + '-' + convert(varchar, v.practice_display_seq) ) ) as [Practice Count]
, sum (CASE WHEN isnull(v.create_by_smartid,'') = 'Y' THEN 1 ELSE 0 END ) as 'Smart IC Transaction'
, count(transaction_id) as 'Tx after 1st reading smart IC' 
--left(convert(varchar, transaction_dtm, 120),10), v.sp_id, v.practice_display_seq
from 
	--[Practice] p, 
	vouchertransaction v 
	left join personalinformation pr
		on v.voucher_acc_id = pr.voucher_acc_id
	left join  temppersonalinformation tpr
		on v.temp_voucher_acc_id = tpr.voucher_acc_id
	left join #tmp_smartIC t
		on v.sp_id = t.sp_id and v.practice_display_seq = t.practice_display_seq
	where  	
		v.Transaction_Dtm > @start_time and v.transaction_dtm <= @end_time
		--and v.sp_id = p.sp_id and v.practice_display_seq = p.display_seq
		and ( pr.doc_code='HKIC' or tpr.doc_code ='HKIC')
		and v.sp_id not in (	select SP_ID from [SPExceptionList]	)
		and v.Transaction_Dtm >=  isnull(t.min_smartIC_date,getdate()+1 )
--and v.sp_id = '00011585'
	group by left(convert(varchar, transaction_dtm, 120),10)
	order by left(convert(varchar, transaction_dtm, 120),10)


drop table  #tmp_SP
drop table  #tmp_smartIC









END
GO

GRANT EXECUTE ON [dbo].[proc_Daily_statistic_on_SmartIC] TO HCVU
GO

 