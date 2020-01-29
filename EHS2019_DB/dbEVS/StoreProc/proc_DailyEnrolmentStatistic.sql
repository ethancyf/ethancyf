IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DailyEnrolmentStatistic]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DailyEnrolmentStatistic]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	07 Sep 2011
-- CR No.:			CRE11-024-01 (Enhancement on HCVS Extension)
-- Description:		Added profession 'ROP' for registered optometrists
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	16 Dec 2009
-- Description:		Add HSVISS for the Scheme Combiniation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	16 Dec 2009
-- Description:		Add HSVISS for the Scheme Combiniation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	16 Oct 2009
-- Description:		Exclude the SP in "SPExceptionList"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	1 Sept 2009
-- Description:		Generate Enrolment Statistic
--					1. Scheme enrolment
--					2. No. of MO
--					3. Relationship of MO
--					4. No. of Practice
--					5. Professional Type
--					6. Cumulative SP Approved Account
--					7. Cumulative SP Activated Account
--					8. Cumulative Data Entry Account
--					9. Cumulative Delisted SP
--					10. Approved Account Scheme Information
--					11. Activated Account Scheme Information
--					12. Approved Account (RMP Only) Scheme Information
--					13. Activated Account (RMP Only) Scheme Information
--					14. Relationship of MO (HCVS) for Send Message to HCVU Inbox
--					15. Professional Type (HCVS) for Send Message to HCVU Inbox
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	7 April 2009
-- Description:		Extend the no of retrieved data entry to 30
-- =============================================
-- =============================================
-- Author:			Timothy LEUNG
-- Create date:		10-10-2008
-- Description:		Get the static of Approved Account / Activated Account and 
--					data entry
-- =============================================

create proc proc_DailyEnrolmentStatistic 
as
begin

declare @start_dtm varchar(10)
declare @end_dtm varchar(10)

select @start_dtm = convert(varchar, dateadd(day, -1, getdate()), 120)
select @end_dtm = convert(varchar, dateadd(day, 0, getdate()), 120)

----- 1. Scheme Enrolment -----
create table #tempEnrolment 
(
	sp_id char(8),
	enrolment_ref_no char(15),
	enrolment_dtm datetime,
	scheme_code varchar(255),
	table_Location char(1),
	submission_method char(1)
)

insert into #tempEnrolment
(
	enrolment_ref_no,
	enrolment_dtm,
	table_Location,
	submission_method
)
select enrolment_ref_no, enrolment_dtm, 'E', 'E' from serviceproviderenrolment
where enrolment_ref_no in
(select distinct Batch_ID from serviceproviderenrolment)
and enrolment_dtm between @start_dtm and @end_dtm

insert into #tempEnrolment
(
	enrolment_ref_no,
	enrolment_dtm,
	table_Location,
	submission_method
)
select enrolment_ref_no, enrolment_dtm, 'S', Submission_Method  from serviceproviderstaging
where enrolment_dtm between @start_dtm and @end_dtm
and isnull(sp_id,'') = ''

insert into #tempEnrolment
(
	sp_id,
	enrolment_ref_no,
	enrolment_dtm,
	table_Location,
	submission_method
)
select sp_id, enrolment_ref_no, enrolment_dtm, 'P', Submission_Method from serviceprovider
where enrolment_dtm between @start_dtm and @end_dtm
and record_status <> 'D'
and sp_id not in (select sp_id from SPExceptionList)


update #tempEnrolment
set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code)) collate database_default
from (SELECT [Scheme].enrolment_ref_no, 
		(SELECT  ', ' + ltrim(rtrim(scheme_code))
		 FROM SchemeInformationenrolment
		 WHERE enrolment_ref_no=[Scheme].enrolment_ref_no collate database_default
		 order by scheme_code
		 FOR XML PATH('')) 		 
		 AS scheme_code
 FROM(select distinct enrolment_ref_no from #tempEnrolment) AS [Scheme]
)AS t, #tempEnrolment s
where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
and isnull(s.scheme_code,'') = '' collate database_default
and s.table_Location = 'E'

update #tempEnrolment
set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code)) collate database_default
from (SELECT [Scheme].enrolment_ref_no, 
		(SELECT  ', ' + ltrim(rtrim(scheme_code))
		 FROM SchemeInformationstaging
		 WHERE enrolment_ref_no=[Scheme].enrolment_ref_no collate database_default
		 order by scheme_code
		 FOR XML PATH('')) 		 
		 AS scheme_code
 FROM(select distinct enrolment_ref_no from #tempEnrolment) AS [Scheme]
)AS t, #tempEnrolment s
where t.enrolment_ref_no =  s.enrolment_ref_no collate database_default
and isnull(s.scheme_code,'') = '' collate database_default
and s.table_Location = 'S'

update #tempEnrolment
set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code)) collate database_default
from (SELECT [Scheme].sp_id, 
		(SELECT  ', ' + ltrim(rtrim(scheme_code))
		 FROM SchemeInformation
		 WHERE sp_id=[Scheme].sp_id collate database_default
		 order by scheme_code
		 FOR XML PATH('')) 		 
		 AS scheme_code
 FROM(select distinct sp_id from #tempEnrolment) AS [Scheme]
)AS t, #tempEnrolment s
where t.sp_id =  s.sp_id collate database_default
and isnull(s.scheme_code,'') = '' collate database_default
and s.table_Location = 'P'

-- e-Enrolment
-- HCVS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS_E'  from #tempEnrolment where scheme_code = 'HCVS'
and Submission_Method = 'E' 

-- CIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS_E' from #tempEnrolment where scheme_code = 'CIVSS'
and Submission_Method = 'E' 

-- EVSSHSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS_E' from #tempEnrolment where scheme_code = 'EVSSHSIVSS'
and Submission_Method = 'E' 

-- HCVS + CIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVSE_E' from #tempEnrolment where scheme_code = 'CIVSS, HCVS'
and Submission_Method = 'E' 

-- HCVS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+EVSSHSIVSS_E' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HCVS'
and Submission_Method = 'E' 

-- CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS_E' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS'
and Submission_Method = 'E' 

-- HCVS + CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS_E' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS'
and Submission_Method = 'E'

-- Total for e-Enrolment
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'E_Total' from #tempEnrolment
where Submission_Method = 'E'

-- Paper
/* ---------------- Total 31 Combination  ----------------
	HCVS Only
	CIVSS Only
	EVSS Only
	HSIVSS Only
	RVP Only

	HCVS+CIVSS
	HCVS+EVSS
	HCVS+HSIVSS
	HCVS+RVP
	CIVSS+EVSS
	CIVSS+HSIVSS
	CIVSS+RVP
	EVSS+HSIVSS
	EVSS+RVP
	HSIVSS+RVP

	HCVS+CIVSS+EVSS
	HCVS+CIVSS+HSIVSS
	HCVS+CIVSS+RVP
	HCVS+EVSS+HSIVSS
	HCVS+EVSS+RVP
	HCVS+HSIVSS+RVP
	CIVSS+EVSS+HSIVSS
	CIVSS+EVSS+RVP
	CIVSS+HSIVSS+RVP
	EVSS+HSIVSS+RVP

	HCVS+CIVSS+EVSS+HSIVSS
	HCVS+CIVSS+EVSS+RVP
	HCVS+CIVSS+HSIVSS+RVP
	HCVS+EVSS+HSIVSS+RVP
	CIVSS+EVSS+HSIVSS+RVP

	HCVS+CIVSS+EVSS+HSIVSS+RVP
---------------------------------------------------------------- */
-- HCVS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS_P' from #tempEnrolment where scheme_code = 'HCVS'
and Submission_Method = 'P' 

-- CIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS'
and Submission_Method = 'P' 

-- EVSSHSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS'
and Submission_Method = 'P' 

-- HSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS_P' from #tempEnrolment where scheme_code = 'HSIVSS'
and Submission_Method = 'P' 

-- RVP Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'RVP_P' from #tempEnrolment where scheme_code = 'RVP'
and Submission_Method = 'P' 

-- HCVS + CIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+CIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS, HCVS'
and Submission_Method = 'P' 

-- HCVS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+EVSSHSIVSS_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HCVS'
and Submission_Method = 'P' 

-- HCVS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS_P' from #tempEnrolment where scheme_code = 'HCVS, HSIVSS'
and Submission_Method = 'P' 

-- HCVS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+RVP_P' from #tempEnrolment where scheme_code = 'HCVS, RVP'
and Submission_Method = 'P' 

-- CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS'
and Submission_Method = 'P' 

-- CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS, HSIVSS'
and Submission_Method = 'P' 

-- CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, RVP'
and Submission_Method = 'P' 

-- EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HSIVSS'
and Submission_Method = 'P' 

-- EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, RVP'
and Submission_Method = 'P' 

-- HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'HSIVSS, RVP'
and Submission_Method = 'P' 

-- HCVS + CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS'
and Submission_Method = 'P' 

-- HCVS + CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS, HCVS, HSIVSS'
and Submission_Method = 'P' 

-- HCVS + CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, HCVS, RVP'
and Submission_Method = 'P' 

-- HCVS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS'
and Submission_Method = 'P' 

-- HCVS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+RVP_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HCVS, RVP'
and Submission_Method = 'P' 

-- HCVS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'HCVS, HSIVSS, RVP'
and Submission_Method = 'P' 

-- CIVSS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS'
and Submission_Method = 'P' 

-- CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, RVP'
and Submission_Method = 'P' 

-- CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, HSIVSS, RVP'
and Submission_Method = 'P' 

-- EVSSHSIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HSIVSS, RVP'
and Submission_Method = 'P' 

-- HCVS + CIVSS + EVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS'
and Submission_Method = 'P' 

-- HCVS + CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, RVP'
and Submission_Method = 'P' 

-- HCVS + CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, HCVS, HSIVSS, RVP'
and Submission_Method = 'P'

-- HCVS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS, RVP'
and Submission_Method = 'P'

-- CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS, RVP'
and Submission_Method = 'P'

-- HCVS + CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_P' from #tempEnrolment where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS, RVP'
and Submission_Method = 'P'

-- None
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'None_P' from #tempEnrolment where scheme_code = ''
and Submission_Method = 'P' 

-- Total for Paper
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'P_Total' from #tempEnrolment
where Submission_Method = 'P'


----- 2. No. of MO -----

create table #MO
(
	batch_id char(15),
	enrolment_ref_no char(15),
	enrolment_dtm datetime,
	display_seq smallint,
	relationship char(1)
)

insert into #MO
select	s.batch_id,
		m.enrolment_ref_no,
		s.enrolment_dtm,
		m.display_seq,
		m.relationship
from serviceproviderenrolment s, medicalorganizationenrolment m
where s.enrolment_ref_no = m.enrolment_ref_no
and s.enrolment_dtm between @start_dtm and @end_dtm

insert into #MO
select	m.enrolment_ref_no,
		m.enrolment_ref_no,
		s.enrolment_dtm,
		m.display_seq,
		m.relationship		
from serviceproviderstaging s, medicalorganizationstaging m
where s.enrolment_ref_no = m.enrolment_ref_no
and s.enrolment_dtm between @start_dtm and @end_dtm
and isnull(s.sp_id,'') = ''

insert into #MO
select	s.enrolment_ref_no,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		m.display_seq,
		m.relationship
from serviceprovider s, medicalorganization m
where s.sp_id = m.sp_id
and s.enrolment_dtm between @start_dtm and @end_dtm
and m.record_status <> 'D'
and s.record_status <> 'D'
and s.sp_id not in (select sp_id from SPExceptionList)

-- 0 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(enrolment_ref_no), 'MO_0'
from serviceproviderstaging
where enrolment_ref_no not in (select enrolment_ref_no from medicalorganizationstaging)
and (enrolment_dtm between @start_dtm and @end_dtm)
and isnull(sp_id,'') = ''

-- 1 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_1' from (select count(batch_id) as total from #MO
group by batch_id
having count(batch_id) = 1) as t

-- 2 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_2' from (select count(batch_id) as total from #MO
group by batch_id
having count(batch_id) = 2) as t

-- 3 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_3' from (select count(batch_id) as total from #MO
group by batch_id
having count(batch_id) = 3) as t

-- 4 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_4' from (select count(batch_id) as total from #MO
group by batch_id
having count(batch_id) = 4) as t

-- 5 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_5' from (select count(batch_id) as total from #MO
group by batch_id
having count(batch_id) = 5) as t

-- 6 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_6' from (select count(batch_id) as total from #MO
group by batch_id
having count(batch_id) = 6) as t

-- > 6 Medical Organization
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'MO_A6' from (select count(batch_id) as total from #MO
group by enrolment_ref_no
having count(enrolment_ref_no) > 6) as t

-- MO Total
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), isnull(sum(total),0), 'MO_Total' from (select count(batch_id) as total from #MO
group by batch_id) as t

----- 3. Relationship of MO -----

-- Solo
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Solo' from (select count(enrolment_ref_no) as total from #MO
group by relationship
having relationship = 'S') as t

-- Partenship
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Partenship' from (select count(enrolment_ref_no) as total from #MO
group by relationship
having relationship = 'P') as t

-- Shareholder
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Shareholder' from (select count(enrolment_ref_no) as total from #MO
group by relationship
having relationship = 'H') as t

-- Director
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total,'Director' from (select count(enrolment_ref_no) as total from #MO
group by relationship
having relationship = 'D') as t

-- Employee
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Employee' from (select count(enrolment_ref_no) as total from #MO
group by relationship
having relationship = 'E') as t

-- Others
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Others' from (select count(enrolment_ref_no) as total from #MO
group by relationship
having relationship = 'O') as t

-- MO Relationship Total
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), isnull(sum(total),0), 'MO_Rel_Total' from (select count(enrolment_ref_no) as total from #MO
group by relationship) as t


----- 4. No. of Practice -----

create table #Practice
(
	batch_id char(15),
	enrolment_ref_no char(15),
	enrolment_dtm datetime,
	display_seq smallint,
	service_category_code char(5)
)

insert into #Practice
select	s.batch_id,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		pf.service_category_code
from serviceproviderenrolment s, practiceenrolment p, professionalenrolment pf
where s.enrolment_ref_no = p.enrolment_ref_no
and s.enrolment_ref_no = pf.enrolment_ref_no
and p.professional_seq = pf.professional_Seq
and s.enrolment_dtm between @start_dtm and @end_dtm

insert into #Practice
select	s.enrolment_ref_no,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		pf.service_category_code
from serviceproviderstaging s, practicestaging p, professionalstaging pf
where s.enrolment_ref_no = p.enrolment_ref_no
and s.enrolment_ref_no = pf.enrolment_ref_no
and p.professional_seq = pf.professional_Seq
and s.enrolment_dtm between @start_dtm and @end_dtm
and isnull(s.sp_id,'') = ''

insert into #Practice
select	s.enrolment_ref_no,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		pf.service_category_code
from serviceprovider s, practice p, professional pf
where s.sp_id = p.sp_id
and s.sp_id = pf.sp_id
and p.professional_seq = pf.professional_Seq
and s.enrolment_dtm between @start_dtm and @end_dtm
and p.record_status <> 'D'
and s.record_status <> 'D'
and s.sp_id not in (select sp_id from SPExceptionList)


-- 0 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(enrolment_ref_no), 'Practice_0'
from serviceproviderstaging
where enrolment_ref_no not in (select enrolment_ref_no from practicestaging)
and enrolment_dtm between @start_dtm and @end_dtm
and isnull(sp_id,'') = ''

-- 1 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_1' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) = 1) as t

-- 2 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_2' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) = 2) as t

-- 3 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_3' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) = 3) as t

-- 4 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_4' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) = 4) as t

-- 5 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_5' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) = 5) as t

-- 6 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_6' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) = 6) as t

-- > 6 Practice
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(total), 'Practice_A6' from (select count(batch_id) as total from #Practice
group by batch_id
having count(batch_id) > 6) as t

-- Practice Total
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), isnull(sum(total),0), 'Practice_Total' from (select count(batch_id) as total from #Practice
group by batch_id) as t


----- 5. Professional Type -----
-- ENU
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'ENU' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'ENU') as t

-- RCM
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RCM' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RCM') as t

-- RCP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RCP' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RCP') as t

-- RDT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total,'RDT' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RDT') as t

-- RMP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RMP' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RMP') as t

-- RMT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RMT' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RMT') as t

-- RNU
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RNU' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RNU') as t

-- CRE11-024-01: added
-- ROP 
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'ROP' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'ROP') as t

-- ROT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'ROT' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'ROT') as t

-- RPT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RPT' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RPT') as t

-- RRD
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RRD' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code
having service_category_code = 'RRD') as t



-- Professional Total
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), isnull(sum(total),0), 'Prof_Total' from (select count(distinct batch_id) as total from #Practice
where display_seq = 1
group by service_category_code) as t


----- 6. Cumulative SP Approved Account------

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_ENU', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'ENU'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RCM', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RCM'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RCP', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RCP'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RDT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RDT'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RMP', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RMP'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RMT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RMT'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RNU', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RNU'
and U.sp_id not in (select sp_id from SPExceptionList)

-- CRE11-024-01: added
insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_ROP', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'ROP'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_ROT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'ROT'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RPT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RPT'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'App_RRD', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RRD'
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'ApprovedAccount', count(SP_ID)
from HCSPUserAC
where sp_id not in (select sp_id from SPExceptionList)


----- 7. Cumulative SP Activated Account ------
insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_ENU', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'ENU'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RCM', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RCM'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RCP', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RCP'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RDT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RDT'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RMP', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RMP'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RMT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RMT'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RNU', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RNU'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)

-- CRE11-024-01: added
insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_ROP', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'ROP'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_ROT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'ROT'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RPT', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RPT'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'Act_RRD', count(U.SP_ID)
from HCSPUserAC U, Practice P, Professional PF
where U.SP_ID = P.SP_ID
and P.SP_ID = PF.SP_ID
and P.Professional_Seq = PF.Professional_Seq
and	P.Display_Seq = 1
and PF.Service_Category_Code = 'RRD'
--and ((U.Record_Status = 'A' or U.Record_Status = 'S')
and (U.SP_Password is not null and U.SP_Password <> '')
and U.sp_id not in (select sp_id from SPExceptionList)


insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'ActivatedAccount', count(SP_ID)
from HCSPUserAC U
where
--and ((U.Record_Status = 'A' or U.Record_Status = 'S') and
 (U.SP_Password is not null and U.SP_Password <> '')
 and U.sp_id not in (select sp_id from SPExceptionList)



----- 8. Cumulative Data Entry Account ---------
create table #tmpDataEntry
(
sp_id varchar(8) collate database_default,
NumDataEntryAC int
)

insert into #tmpDataEntry
select sp_id, count(data_entry_account)
from dataentryuserac
group by sp_id

insert into #tmpDataEntry
select sp_id, 0
from HCSPUserAC A
where ((A.Record_Status = 'A' or A.Record_Status = 'S') and (A.SP_Password is not null and A.SP_Password <> ''))
and A.SP_ID collate database_default not in 
(
select sp_id from #tmpDataEntry
)
and A.sp_id not in (select sp_id from SPExceptionList)

create table #tmp2
(
DEACNum int,
DENumCNT int
)

insert into #tmp2
values
(0,0)

insert into #tmp2
values
(1,0)

insert into #tmp2
values
(2,0)

insert into #tmp2
values
(3,0)

insert into #tmp2
values
(4,0)

insert into #tmp2
values
(5,0)

insert into #tmp2
values
(6,0)

insert into #tmp2
values
(7,0)

insert into #tmp2
values
(8,0)

insert into #tmp2
values
(9,0)

insert into #tmp2
values
(10,0)

insert into #tmp2
values
(11,0)

insert into #tmp2
values
(12,0)

insert into #tmp2
values
(13,0)

insert into #tmp2
values
(14,0)

insert into #tmp2
values
(15,0)

insert into #tmp2
values
(16,0)

insert into #tmp2
values
(17,0)

insert into #tmp2
values
(18,0)

insert into #tmp2
values
(19,0)

insert into #tmp2
values
(20,0)

insert into #tmp2
values
(21,0)

insert into #tmp2
values
(22,0)

insert into #tmp2
values
(23,0)

insert into #tmp2
values
(24,0)

insert into #tmp2
values
(25,0)

insert into #tmp2
values
(26,0)

insert into #tmp2
values
(27,0)

insert into #tmp2
values
(28,0)

insert into #tmp2
values
(29,0)

insert into #tmp2
values
(30,0)

 
declare @NumDataEntryAC int
declare @NumDataEntryCnt int 

DECLARE DataEntry_cursor CURSOR FOR 
	select NumDataEntryAC, count(sp_id) 
	from #tmpDataEntry
	group by NumDataEntryAC

    OPEN DataEntry_cursor
    FETCH NEXT FROM DataEntry_cursor INTO @NumDataEntryAC, @NumDataEntryCnt
	WHILE @@FETCH_STATUS = 0
    BEGIN
		if @NumDataEntryAC < 30 
		begin
			update #tmp2
			set DENumCNT = DENumCNT + @NumDataEntryCnt 
			where DEACNum = @NumDataEntryAC
		end
		else
		begin
			update #tmp2
			set DENumCNT = DENumCNT + @NumDataEntryCnt 
			where DEACNum = 30
		end

		FETCH NEXT FROM DataEntry_cursor INTO @NumDataEntryAC, @NumDataEntryCnt
	end
Close DataEntry_cursor
DEALLOCATE DataEntry_cursor

insert into StatisticTable (system_dtm, statisticDate, statistictype, statisticvalue)
select getdate(), dateadd(dd,-1,@end_dtm), 'DEAcct' + convert(varchar(5), DEACNum), DENumCNT
from #tmp2


----- 9. Cumulative Delisted SP ---------
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(t.sp_id), 'DelistSP_NA' from (
select s.sp_id, max(si.delist_dtm) as delist_dtm
from serviceprovider s, hcspuserac a, schemeInformation si
where s.sp_id  = a.sp_id
and s.sp_id = si.sp_id
and isnull(a.[sp_password],'') = ''
and s.record_status = 'D' 
and si.record_status = 'D'
and s.sp_id not in (select sp_id from SPExceptionList)
group by s.sp_id) as t


insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(t.sp_id), 'DelistSP_A' from (
select s.sp_id, max(si.delist_dtm) as delist_dtm
from serviceprovider s, hcspuserac a, schemeInformation si
where s.sp_id  = a.sp_id
and s.sp_id = si.sp_id
and isnull(a.[sp_password],'') <> ''
and s.record_status = 'D' 
and si.record_status = 'D'
and s.sp_id not in (select sp_id from SPExceptionList)
group by s.sp_id) as t


----- 10. Approved Account Scheme Information ---------
create table #tempSP
(
	sp_id char(8),	
	scheme_code varchar(255),
	record_status char(1),
	activiated char(1),
	service_category_code char(5)
)

insert into #tempSP
(sp_id, record_status, activiated, service_category_code)
select	s.sp_id,
		s.record_status,
		case isnull(a.[sp_password],'')
			when '' then 'N'
			else 'Y'
		end,
		pf.service_category_code
	from serviceprovider s, hcspuserac a, practice p, professional pf
	where s.sp_id = a.sp_id
	and s.sp_id = p.sp_id
	and s.sp_id = pf.sp_id
	and s.sp_id not in (select sp_id from SPExceptionList)
	and p.professional_Seq = pf.professional_seq
	and p.display_seq = 1	

update #tempSP
set scheme_code = Substring(t.scheme_code, 3, LEN(t.scheme_code)) collate database_default
from (SELECT [Scheme].sp_id, 
		(SELECT  ', ' + ltrim(rtrim(scheme_code))
		 FROM SchemeInformation
		 WHERE sp_id=[Scheme].sp_id collate database_default
		 order by scheme_code
		 FOR XML PATH('')) 		 
		 AS scheme_code
 FROM(select distinct sp_id from #tempSP) AS [Scheme]
)AS t, #tempSP s
where t.sp_id =  s.sp_id collate database_default
and isnull(s.scheme_code,'') = '' collate database_default

/* ---------------- Total 31 Combination  ---------------- */
-- HCVS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS_A' from #tempSP where scheme_code = 'HCVS'
and record_status <> 'D' 

-- CIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS_A' from #tempSP where scheme_code = 'CIVSS'
and record_status <> 'D' 

-- EVSSHSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS_A' from #tempSP where scheme_code = 'EVSSHSIVSS'
and record_status <> 'D' 

-- HSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS_A' from #tempSP where scheme_code = 'HSIVSS'
and record_status <> 'D' 

-- RVP Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'RVP_A' from #tempSP where scheme_code = 'RVP'
and record_status <> 'D' 

-- HCVS + CIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+CIVSS_A' from #tempSP where scheme_code = 'CIVSS, HCVS'
and record_status <> 'D' 

-- HCVS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+EVSSHSIVSS_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS'
and record_status <> 'D' 

-- HCVS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS_A' from #tempSP where scheme_code = 'HCVS, HSIVSS'
and record_status <> 'D' 

-- HCVS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+RVP_A' from #tempSP where scheme_code = 'HCVS, RVP'
and record_status <> 'D' 

-- CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS'
and record_status <> 'D' 

-- CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS_A' from #tempSP where scheme_code = 'CIVSS, HSIVSS'
and record_status <> 'D' 

-- CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+RVP_A' from #tempSP where scheme_code = 'CIVSS, RVP'
and record_status <> 'D' 

-- EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS'
and record_status <> 'D' 

-- EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+RVP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, RVP'
and record_status <> 'D' 

-- HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS+RVP_A' from #tempSP where scheme_code = 'HSIVSS, RVP'
and record_status <> 'D' 

-- HCVS + CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS'
and record_status <> 'D' 

-- HCVS + CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS_A' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS'
and record_status <> 'D' 

-- HCVS + CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+RVP_A' from #tempSP where scheme_code = 'CIVSS, HCVS, RVP'
and record_status <> 'D' 

-- HCVS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' 

-- HCVS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+RVP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' 

-- HCVS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'HCVS, HSIVSS, RVP'
and record_status <> 'D' 

-- CIVSS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS'
and record_status <> 'D' 

-- CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+RVP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, RVP'
and record_status <> 'D' 

-- CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'CIVSS, HSIVSS, RVP'
and record_status <> 'D' 

-- EVSSHSIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' 

-- HCVS + CIVSS + EVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' 

-- HCVS + CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+RVP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' 

-- HCVS + CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D'

-- HCVS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D'

-- CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D'

-- HCVS + CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D'

-- Total for Delisted SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'Deliat_Total_A' from #tempSP where record_status = 'D'

-- Total for Approval SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'A_Total' from #tempSP


----- 11. Activated Account Scheme Information ---------
/* ---------------- Total 31 Combination  ---------------- */
-- HCVS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS_AC' from #tempSP where scheme_code = 'HCVS'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS_AC' from #tempSP where scheme_code = 'CIVSS'
and record_status <> 'D' and activiated = 'Y'

-- EVSSHSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS_AC' from #tempSP where scheme_code = 'EVSSHSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- HSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS_AC' from #tempSP where scheme_code = 'HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- RVP Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'RVP_AC' from #tempSP where scheme_code = 'RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+CIVSS_AC' from #tempSP where scheme_code = 'CIVSS, HCVS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+EVSSHSIVSS_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS_AC' from #tempSP where scheme_code = 'HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+RVP_AC' from #tempSP where scheme_code = 'HCVS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS_AC' from #tempSP where scheme_code = 'CIVSS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+RVP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS+RVP_AC' from #tempSP where scheme_code = 'HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS_AC' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, HCVS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+RVP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- EVSSHSIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + EVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- HCVS + CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y'

-- Total for Delisted SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'Deliat_Total_AC' from #tempSP where record_status = 'D' and activiated = 'Y'

-- Total for Activated SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'AC_Total' from #tempSP where activiated = 'Y'


----- 11. Approval Account (RMP Only) Scheme Information ---------
/* ---------------- Total 31 Combination  ---------------- */
-- HCVS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS_RMP_A' from #tempSP where scheme_code = 'HCVS'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- EVSSHSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS_RMP_A' from #tempSP where scheme_code = 'HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- RVP Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'RVP_RMP_A' from #tempSP where scheme_code = 'RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+CIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS, HCVS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+EVSSHSIVSS_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'HCVS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+RVP_RMP_A' from #tempSP where scheme_code = 'HCVS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, HCVS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+RVP_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'HCVS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- EVSSHSIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_A' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and service_category_code = 'RMP'

-- Total for Delisted SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'Deliat_Total_RMP_A' from #tempSP where record_status = 'D' and service_category_code = 'RMP'

-- Total for Approval SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'A_RMP_Total' from #tempSP where service_category_code = 'RMP'


----- 11. Activated Account (RMP Only) Scheme Information ---------
/* ---------------- Total 31 Combination  ---------------- */
-- HCVS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS_RMP_AC' from #tempSP where scheme_code = 'HCVS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- EVSSHSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HSIVSS Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS_RMP_AC' from #tempSP where scheme_code = 'HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- RVP Only
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'RVP_RMP_AC' from #tempSP where scheme_code = 'RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+CIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, HCVS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+EVSSHSIVSS_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+RVP_RMP_AC' from #tempSP where scheme_code = 'HCVS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSSHSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, HCVS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+RVP_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'HCVS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + EVSSHSIVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- EVSSHSIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSS + HSIVSS
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSSHSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+HCVS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- HCVS + CIVSS + EVSS + HSIVSS + RVP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_AC' from #tempSP where scheme_code = 'CIVSS, EVSSHSIVSS, HCVS, HSIVSS, RVP'
and record_status <> 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- Total for Delisted SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'Deliat_Total_RMP_AC' from #tempSP where record_status = 'D' and activiated = 'Y' and service_category_code = 'RMP'

-- Total for Approval SP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), count(1), 'AC_RMP_Total' from #tempSP where activiated = 'Y' and service_category_code = 'RMP'


----- 12. Relationship of MO (HCVS) for Send Message to HCVU Inbox -----

create table #MO_HCVS
(
	enrolment_ref_no char(15),
	enrolment_dtm datetime,
	display_seq smallint,
	relationship char(1)
)

insert into #MO_HCVS
select	m.enrolment_ref_no,
		s.enrolment_dtm,
		m.display_seq,
		m.relationship
from serviceproviderenrolment s, medicalorganizationenrolment m
where s.enrolment_ref_no = m.enrolment_ref_no
and s.enrolment_ref_no in (select enrolment_ref_no from schemeinformationenrolment where scheme_code = 'HCVS')
and (s.enrolment_dtm between @start_dtm and @end_dtm)

insert into #MO_HCVS
select	m.enrolment_ref_no,
		s.enrolment_dtm,
		m.display_seq,
		m.relationship
from serviceproviderstaging s, medicalorganizationstaging m
where s.enrolment_ref_no = m.enrolment_ref_no
and s.enrolment_ref_no in (select enrolment_ref_no from schemeinformationstaging where scheme_code = 'HCVS')
and (s.enrolment_dtm between @start_dtm and @end_dtm)
and isnull(s.sp_id,'') = ''
and s.Submission_Method = 'E'

insert into #MO_HCVS
select	s.enrolment_ref_no,
		s.enrolment_dtm,
		m.display_seq,
		m.relationship
from serviceprovider s, medicalorganization m
where s.sp_id = m.sp_id
and s.sp_id in (select sp_id from schemeinformation where scheme_code = 'HCVS')
and s.sp_id not in (select sp_id from SPExceptionList)
and (s.enrolment_dtm between @start_dtm and @end_dtm)
and m.record_status <> 'D'
and s.record_status <> 'D'
and s.Submission_Method = 'E'

-- Solo
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Solo_HCVS' from (select count(enrolment_ref_no) as total from #MO_HCVS
group by relationship
having relationship = 'S') as t

-- Partenship
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Partenship_HCVS' from (select count(enrolment_ref_no) as total from #MO_HCVS
group by relationship
having relationship = 'P') as t

-- Shareholder
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Shareholder_HCVS' from (select count(enrolment_ref_no) as total from #MO_HCVS
group by relationship
having relationship = 'H') as t

-- Director
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total,'Director_HCVS' from (select count(enrolment_ref_no) as total from #MO_HCVS
group by relationship
having relationship = 'D') as t

-- Employee
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Employee_HCVS' from (select count(enrolment_ref_no) as total from #MO_HCVS
group by relationship
having relationship = 'E') as t

-- Others
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'Others_HCVS' from (select count(enrolment_ref_no) as total from #MO_HCVS
group by relationship
having relationship = 'O') as t

------ 13. Professional Type (HCVS) for Send Message to HCVU Inbox -------------
create table #Practice_HCVS
(
	batch_id char(15),
	enrolment_ref_no char(15),
	enrolment_dtm datetime,
	display_seq smallint,
	service_category_code char(5)
)

insert into #Practice_HCVS
select	s.batch_id,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		pf.service_category_code
from serviceproviderenrolment s, practiceenrolment p, professionalenrolment pf
where s.enrolment_ref_no = p.enrolment_ref_no
and s.enrolment_ref_no in (select enrolment_ref_no from schemeinformationenrolment where scheme_code = 'HCVS')
and s.enrolment_ref_no = pf.enrolment_ref_no
and p.professional_seq = pf.professional_Seq
and (s.enrolment_dtm between @start_dtm and @end_dtm)

insert into #Practice_HCVS
select	s.enrolment_ref_no,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		pf.service_category_code
from serviceproviderstaging s, practicestaging p, professionalstaging pf
where s.enrolment_ref_no = p.enrolment_ref_no
and s.enrolment_ref_no in (select enrolment_ref_no from schemeinformationstaging where scheme_code = 'HCVS')
and s.enrolment_ref_no = pf.enrolment_ref_no
and p.professional_seq = pf.professional_Seq
and (s.enrolment_dtm between @start_dtm and @end_dtm)
and isnull(s.sp_id,'') = ''
and s.Submission_Method = 'E'

insert into #Practice_HCVS
select	s.enrolment_ref_no,
		s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		pf.service_category_code
from serviceprovider s, practice p, professional pf
where s.sp_id = p.sp_id
and s.sp_id = pf.sp_id
and s.sp_id in (select sp_id from schemeinformation where scheme_code = 'HCVS')
and s.sp_id not in (select sp_id from SPExceptionList)
and p.professional_seq = pf.professional_Seq
and (s.enrolment_dtm between @start_dtm and @end_dtm)
and p.record_status <> 'D'
and s.record_status <> 'D'
and s.Submission_Method = 'E'


-- ENU
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'ENU_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'ENU') as t

-- RCM
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RCM_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RCM') as t

-- RCP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RCP_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RCP') as t

-- RDT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total,'RDT_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RDT') as t

-- RMP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RMP_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RMP') as t

-- RMT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RMT_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RMT') as t

-- RNU
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RNU_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RNU') as t

-- CRE11-024-01: added
-- ROP
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'ROP_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'ROP') as t

-- ROT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'ROT_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'ROT') as t

-- RPT
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RPT_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RPT') as t

-- RRD
insert into StatisticTable (system_dtm, statisticDate, statisticvalue, statistictype)
select getdate(), dateadd(dd,-1,@end_dtm), total, 'RRD_HCVS' from (select count(distinct batch_id) as total from #Practice_HCVS
where display_seq = 1
group by service_category_code
having service_category_code = 'RRD') as t


drop table #tempEnrolment
drop table #MO
drop table #Practice
drop table #tmpDataEntry
drop table #tmp2
drop table #tempSP
drop table #MO_HCVS
drop table #Practice_HCVS

end
GO

GRANT EXECUTE ON [dbo].[proc_DailyEnrolmentStatistic] TO HCVU
GO
