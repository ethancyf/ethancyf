IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_upd_All]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_All]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	10 Dec 2009
-- Description:		Don't retrieve those subsidizegroup
--					which record_status = 'I'
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 14 OCt 2009
-- Description:	Based on the practice scheme information
--				to set the Scheme infomation in staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_All]
	@enrolment_ref_no		char(15),
	@sp_id char(8),
	@update_by varchar(20)
	
AS BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
create table #PracticeSchemeInfo
(
	scheme_code char(10) collate database_default,
	record_status char(1) collate database_default
)

create table #SchemeInfo
(
	scheme_code char(10) collate database_default,
	record_status char(1) collate database_default
)

create table #WillDeleteScheme
(
	scheme_Code char(10) collate database_default
)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

insert into #PracticeSchemeInfo
(
	scheme_code,
	record_status
)
select distinct p.scheme_code, p.record_status
from practiceschemeinfostaging p, schemebackoffice se, subsidizegroupbackoffice sg
WHERE	p.Enrolment_Ref_No = @enrolment_ref_no collate database_default 
		and p.scheme_code = se.scheme_code
		and getdate() between se.effective_dtm and se.expiry_dtm
		and se.record_status <> 'I'
		and se.scheme_seq = sg.scheme_seq
		and p.subsidize_code = sg.subsidize_code
		and sg.record_status <> 'I'

insert into #SchemeInfo
(
	scheme_code,
	record_status
)
select s.scheme_code, s.record_status
from  schemeinformationstaging s, schemebackoffice se
WHERE	s.Enrolment_Ref_No = @enrolment_ref_no  collate database_default
		and s.scheme_code = se.scheme_code
		and getdate() between se.effective_dtm and se.expiry_dtm
		and se.record_status <> 'I'

delete from #SchemeInfo
where scheme_code collate database_default not in
(select scheme_code from #PracticeSchemeInfo)

insert into #SchemeInfo
(scheme_code)
select distinct scheme_code from #PracticeSchemeInfo
where scheme_code not in (select scheme_code from #SchemeInfo)


-- Suspend Practice Scheme
update #SchemeInfo
set record_status = 'W' collate database_default 
where scheme_code collate database_default in (select scheme_code from #PracticeSchemeInfo where record_status = 'W')

-- Delist 
update #SchemeInfo
set record_status = 'V' collate database_default 
where scheme_code collate database_default in (select scheme_code from #PracticeSchemeInfo where record_status = 'V')

update #SchemeInfo
set record_status = 'I'collate database_default 
where scheme_code collate database_default in (select scheme_code from #PracticeSchemeInfo where record_status = 'I')

-- New Added
update #SchemeInfo
set record_status = 'A'collate database_default
where scheme_code collate database_default in (select scheme_code from #PracticeSchemeInfo where record_status = 'A')

-- Exist
update #SchemeInfo
set record_status = 'E' collate database_default
where scheme_code collate database_default in (select scheme_code from #PracticeSchemeInfo where record_status in ('E', 'U'))

-- Based on the permanent to update the schemeinformationstaging status

if isnull(@sp_id,'') <> ''
begin
	update #SchemeInfo
	set record_status = 'E'collate database_default
	where scheme_code collate database_default in (select scheme_code from schemeinformation where sp_id = @sp_id and record_Status = 'A')

	update #SchemeInfo
	set record_status = 'W'collate database_default
	where scheme_code collate database_default in (select scheme_code from schemeinformation where sp_id = @sp_id and record_Status = 'S')
end

insert into SchemeInformationStaging
(
	enrolment_ref_no,
	scheme_code,
	sp_id,
	create_dtm,
	create_by,
	update_dtm,
	update_by,
	record_status	
)
select @enrolment_ref_no,
		scheme_code,
		@sp_id,
		getdate(),
		@update_by,
		getdate(),
		@update_by,
		'A'
from #SchemeInfo
where scheme_code not in 
(select scheme_code from SchemeInformationStaging where enrolment_ref_no = @enrolment_ref_no)


update SchemeinformationStaging
set record_status = ts.record_status,
	update_by = @update_by,
	update_dtm = getdate(),
	delist_status = null,
	delist_dtm = null,
	logo_return_dtm = null,
	effective_dtm = null,
	remark = null
from SchemeinformationStaging s, #SchemeInfo ts
where s.enrolment_ref_no = @enrolment_ref_no collate database_default
and s.scheme_code = ts.scheme_code collate database_default
and s.record_status <> ts.record_status
and ts.record_status = 'A'

update SchemeinformationStaging
set record_status = ts.record_status,
	update_by = @update_by,
	update_dtm = getdate()
from SchemeinformationStaging s, #SchemeInfo ts
where s.enrolment_ref_no = @enrolment_ref_no collate database_default
and s.scheme_code = ts.scheme_code collate database_default
and s.record_status <> ts.record_status
and ts.record_status <> 'A'

if isnull(@sp_id,'') <> ''
begin

	update SchemeinformationStaging
	set record_Status = si.delist_status,
		delist_status = si.delist_status,
		delist_dtm = si.delist_dtm,
		logo_return_dtm = si.delist_dtm,
		remark = si.remark
	from Schemeinformation si, SchemeinformationStaging sig
	where si.sp_id = sig.sp_id
	and si.sp_id = @sp_id
	and si.scheme_code = sig.scheme_code
	and si.record_status = 'D'
	and sig.record_Status in ('I','V')

	update SchemeinformationStaging
	set remark = si.remark
	from Schemeinformation si, SchemeinformationStaging sig
	where si.sp_id = sig.sp_id
	and si.sp_id = @sp_id
	and si.scheme_code = sig.scheme_code
	and si.record_status not in ('A','D')
	and sig.record_Status <> 'A'
end

UPDATE	SchemeinformationStaging
SET		Record_Status = 'R',
		Update_by = @update_by,
		Update_Dtm = getdate()
WHERE	Enrolment_Ref_No = @enrolment_ref_no collate database_default and 		
		scheme_code collate database_default not in
		(select scheme_code from #SchemeInfo)
		
insert into #WillDeleteScheme
(Scheme_code)
select scheme_code from SchemeinformationStaging 
where enrolment_ref_no = @enrolment_Ref_no collate database_default and record_Status = 'R' collate database_default

DELETE	FROM SchemeinformationStaging
WHERE	Enrolment_Ref_No = @enrolment_ref_no and
		scheme_code not in
		(select scheme_code from #SchemeInfo)

declare @count smallint

select @count = count(record_status)
from Schemeinformation
WHERE	SP_ID = @sp_id and		
		Scheme_code in (select scheme_code from #WillDeleteScheme) and 
		record_Status = 'D'

if @count > 1
begin
		insert into SchemeinformationStaging
		(
			enrolment_Ref_no,
			scheme_code,
			sp_id,
			create_dtm,
			create_by,
			update_dtm,
			update_by,
			delist_status,
			effective_dtm,
			logo_return_dtm,
			record_status,
			remark			
		)
		select 
			enrolment_Ref_no,
			scheme_code,
			sp_id,
			create_dtm,
			create_by,
			update_dtm,
			update_by,
			delist_status,
			effective_dtm,
			logo_return_dtm,
			record_status,
			remark	
		from SchemeinformationStaging
		WHERE	Enrolment_Ref_No = @enrolment_ref_no collate database_default and
			scheme_code collate database_default in (select scheme_code from #WillDeleteScheme)	


end

drop table #SchemeInfo
drop table #PracticeSchemeInfo
drop table #WillDeleteScheme


END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_upd_All] TO HCVU
GO
