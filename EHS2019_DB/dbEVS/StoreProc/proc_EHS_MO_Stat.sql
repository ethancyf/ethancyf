IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_MO_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_MO_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 06 Nov 2008
-- Description:	MO Statistics
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_MO_Stat]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
declare @district_board varchar(100)
declare @x int
declare @rdistrict_board varchar(100)
declare @rservice_code char(3)
declare @status varchar(20)
declare @enrolment_ref_no varchar(15)

DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255),
		@display_seq smallint

------------------------
-- EVS Practice and Professional Log
------------------------
create table #tmpEVSPracticeLog
(
	Enrolment_Ref_No	char(15),
	Display_Seq	smallint,
	SP_ID	char(8),
	Practice_Name	nvarchar(100),
	Practice_Type	char(5),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
	Building	varchar(100),
	Building_Chi	nvarchar(100),
	District	char(4),
	Address_Code	int,
--	Professional_Seq	smallint,
--	Service_Category_Code char(5),
	Record_Status	char(1),
	Remark	nvarchar(255),
	Submission_Method	char(1)
)

insert into #tmpEVSPracticeLog
(
	Enrolment_Ref_No,
	Display_Seq,
	SP_ID,
	Practice_Name,
	Practice_Type,
	Room,
	[Floor],
	Block,
	Building,
	Building_Chi,
	District,
	Address_Code,
--	Professional_Seq,
--	Service_Category_Code,
	Record_Status,
	Remark,
	Submission_Method	
)

select 	p.Enrolment_Ref_No,
	p.Display_Seq,
	p.SP_ID,
	p.Practice_Name,
	p.Practice_Type,
	p.Room,
	p.[Floor],
	p.Block,
	p.Building,
	p.Building_Chi,
	p.District,
	p.Address_Code,
--	p.Professional_Seq,
--	pf.Service_Category_Code,
	p.Record_Status,
	p.Remark,
	p.Submission_Method
from dbEVS..practicestaginglog p--, dbEVS..professionalstaginglog pf
where p.enrolment_ref_no + convert(varchar(5),p.display_seq) +  convert(varchar(30), p.system_dtm, 126) in 
(select enrolment_ref_no + convert(varchar(5),display_seq) + convert(varchar(30),min(system_dtm), 126)
  from dbEVS..practicestaginglog
  where record_status = 'A'
  and submission_method = 'E'
  group by enrolment_ref_no, display_seq
) 
and p.record_status = 'A'
and p.submission_method = 'E'
--and pf.enrolment_ref_no + convert(varchar(5),pf.professional_seq) + convert(varchar(20), pf.system_dtm) in 
--(select enrolment_ref_no +  convert(varchar(5),professional_seq) + convert(varchar(20),min(system_dtm))
--  from dbEVS..professionalstaginglog
--  where record_status = 'A'
--  group by enrolment_ref_no, professional_seq
--)
--and pf.record_status = 'A'
--and p.enrolment_ref_no = pf.enrolment_ref_no
--and p.professional_seq = pf.professional_seq
and p.sp_id is null
--and pf.sp_id is null

------------------------
-- IVSS Practice and Professional Log
------------------------
create table #tmpIVSSPracticeLog
(
	Enrolment_Ref_No	char(15),
	Display_Seq	smallint,
	SP_ID	char(8),
	Practice_Name	nvarchar(100),
	Practice_Type	char(5),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
	Building	varchar(100),
	Building_Chi	nvarchar(100),
	District	char(4),
	Address_Code	int,
--	Professional_Seq	smallint,
--	Service_Category_Code char(5),
	Record_Status	char(1),
	Remark	nvarchar(255),
	Submission_Method	char(1)
)

insert into #tmpIVSSPracticeLog
(
	Enrolment_Ref_No,
	Display_Seq,
	SP_ID,
	Practice_Name,
	Practice_Type,
	Room,
	[Floor],
	Block,
	Building,
	Building_Chi,
	District,
	Address_Code,
--	Professional_Seq,
--	Service_Category_Code,
	Record_Status,
	Remark,
	Submission_Method	
)

select 	p.Enrolment_Ref_No,
	p.Display_Seq,
	p.SP_ID,
	p.Practice_Name,
	p.Practice_Type,
	p.Room,
	p.[Floor],
	p.Block,
	p.Building,
	p.Building_Chi,
	p.District,
	p.Address_Code,
--	p.Professional_Seq,
--	pf.Service_Category_Code,
	p.Record_Status,
	p.Remark,
	p.Submission_Method
from dbIVSS..practicestaginglog p--, dbIVSS..professionalstaginglog pf
where p.enrolment_ref_no + convert(varchar(5),p.display_seq) +  convert(varchar(30), p.system_dtm, 126) in 
(select enrolment_ref_no + convert(varchar(5),display_seq) + convert(varchar(30),min(system_dtm), 126)
  from dbIVSS..practicestaginglog
  where record_status = 'A'
and submission_method = 'E'
  group by enrolment_ref_no, display_seq
) 
and p.record_status = 'A'
and p.submission_method = 'E'
--and pf.enrolment_ref_no + convert(varchar(5),pf.professional_seq) + convert(varchar(20), pf.system_dtm) in 
--(select enrolment_ref_no +  convert(varchar(5),professional_seq) + convert(varchar(20),min(system_dtm))
--  from dbIVSS..professionalstaginglog
--  where record_status = 'A'
--  group by enrolment_ref_no, professional_seq
--)
--and pf.record_status = 'A'
--and p.enrolment_ref_no = pf.enrolment_ref_no
--and p.professional_seq = pf.professional_seq
and p.sp_id is null
--and pf.sp_id is null

------------------------
--MO Information
------------------------
create table #tmpMO
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
eng_name varchar(100),
chi_name nvarchar(100),
scheme_code varchar(10) collate database_default,
ide varchar(50) 
)

truncate table #tmpMO

insert into #tmpMO
select sp.enrolment_ref_no, sp.enrolment_dtm, isNull(m.mo_eng_name, ''), isNull(m.mo_chi_name, ''), 'EVS Only', sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
from dbevs..serviceprovider sp, dbevs..medicalorganization m, #tmpEVSPracticeLog p
where sp.enrolment_ref_no = m.enrolment_ref_no
and p.enrolment_ref_no = sp.enrolment_ref_no
and p.display_seq = m.sp_practice_display_seq
and sp.submission_method = 'E'

insert into #tmpMO
select sp.enrolment_ref_no, sp.enrolment_dtm, isNull(m.mo_eng_name, ''), isNull(m.mo_chi_name, ''), 'EVS Only', sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
from dbevs..serviceproviderstaging sp, dbevs..medicalorganization m, #tmpEVSPracticeLog p
where sp.enrolment_ref_no = m.enrolment_ref_no
and p.enrolment_ref_no = sp.enrolment_ref_no
and p.display_seq = m.sp_practice_display_seq
and sp.submission_method = 'E'
and sp.sp_id is null

insert into #tmpMO
select sp.enrolment_ref_no, sp.enrolment_dtm, isNull(m.mo_eng_name, ''), isNull(m.mo_chi_name, ''), 'EVS Only', sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
from dbevs..serviceproviderenrolment sp, dbevs..medicalorganizationenrolment m
where sp.enrolment_ref_no = m.enrolment_ref_no

update #tmpMO
set scheme_code = 'Both'
from #tmpMO t
where t.ide in
(select sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
 from dbivss..serviceproviderstaging sp, dbivss..medicalorganization m, dbivss..practicestaging p
 where sp.enrolment_ref_no = m.enrolment_ref_no
 and p.enrolment_ref_no = sp.enrolment_ref_no
 and p.display_seq = m.sp_practice_display_seq
 and sp.submission_method = 'E'
)

update #tmpMO
set scheme_code = 'Both'
from #tmpMO t
where t.ide in
(select sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
 from dbivss..serviceproviderenrolment sp, dbivss..medicalorganizationenrolment m
 where sp.enrolment_ref_no = m.enrolment_ref_no
)

insert into #tmpMO
select sp.enrolment_ref_no, sp.enrolment_dtm, isNull(m.mo_eng_name, ''), isNull(m.mo_chi_name, ''), 'IVSS Only', sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
from dbivss..serviceproviderstaging sp, dbivss..medicalorganization m, dbivss..practicestaging p
where sp.enrolment_ref_no = m.enrolment_ref_no 
and p.enrolment_ref_no = sp.enrolment_ref_no
and p.display_seq = m.sp_practice_display_seq
and sp.submission_method = 'E'
and sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq) not in 
(
select ide
from #tmpMO
)

insert into #tmpMO
select sp.enrolment_ref_no, sp.enrolment_dtm, isNull(m.mo_eng_name, ''), isNull(m.mo_chi_name, ''), 'IVSS Only', sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq)
from dbivss..serviceproviderenrolment sp, dbivss..medicalorganizationenrolment m
where sp.enrolment_ref_no = m.enrolment_ref_no 
and sp.enrolment_ref_no + convert(varchar(5), m.sp_practice_display_seq) + convert(varchar(5), m.display_seq) not in 
(
select ide
from #tmpMO
)

------------------------
--EVS Practice Information
------------------------
create table #tmpTableEVS
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)

create table #tmpTableEVS_P
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)

create table #tmpTableEVS_S
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)

create table #tmpTableEVS_E
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)


insert into  #tmpTableEVS_P
(
	enrolment_ref_no,
	enrolment_dtm,
	display_seq,
	practice_ename,
	Room,
	[Floor],
	Block,
	building,
	building_chi,
	district,
	service_category_code,
	address_code,
	Status
)
select	s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		p.practice_name,
		p.room,
		p.[floor],
		p.block,
		p.building,
		p.building_chi,
		p.district,
		pf.service_category_code,
		p.address_code,
		'Approved'		
from dbEVS..serviceprovider s, dbEVS..practice p, dbEVS..professional pf
where s.SP_ID = p.SP_ID
and	p.SP_ID = pf.SP_ID
and	p.Professional_Seq = pf.Professional_Seq
and s.submission_method = 'E'
--and p.submission_method = 'E'

insert into  #tmpTableEVS_S
(
	enrolment_ref_no,
	enrolment_dtm,
	display_seq,
	practice_ename,
	Room,
	[Floor],
	Block,
	building,
	building_chi,
	district,
	service_category_code,
	address_code,
	Status
)
select	s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		p.practice_name,
	p.Room,
	p.[Floor],
	p.Block,
		p.building,
		p.building_chi,
		p.district,
		pf.service_category_code,
		p.address_code,
		'Processing'		
from dbEVS..serviceproviderstaging s, dbEVS..practicestaging p, dbEVS..professionalstaging pf
where s.enrolment_ref_no = p.enrolment_ref_no
and	p.enrolment_ref_no = pf.enrolment_ref_no
and	p.Professional_Seq = pf.Professional_Seq
and s.submission_method = 'E'
--and p.submission_method = 'E'

insert into #tmpTableEVS_E
(
	enrolment_ref_no,
	enrolment_dtm,
	display_seq,
	practice_ename,
	Room,
	[Floor],
	Block,
	building,
	building_chi,
	district,
	service_category_code,
	address_code,
	Status
)
select	s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		p.practice_name,
	p.Room,
	p.[Floor],
	p.Block,
		p.building,
		p.building_chi,
		p.district,
		pf.service_category_code,
		p.address_code,
		'Submitted'		
from dbEVS..serviceproviderenrolment s, dbEVS..practiceenrolment p, dbEVS..professionalenrolment pf
where s.enrolment_ref_no = p.enrolment_ref_no
and	p.enrolment_ref_no = pf.enrolment_ref_no
and	p.Professional_Seq = pf.Professional_Seq

insert into #tmpTableEVS select * from #tmpTableEVS_E
insert into #tmpTableEVS select * from #tmpTableEVS_S
insert into #tmpTableEVS select * from #tmpTableEVS_P


DECLARE avail_cursor cursor 
FOR	SELECT address_code, display_seq, enrolment_ref_no
FROM #tmpTableEVS

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq, @enrolment_ref_no
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE #tmpTableEVS
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
	WHERE Display_Seq = @display_seq
			and enrolment_ref_no = @enrolment_ref_no
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq, @enrolment_ref_no
END

CLOSE avail_cursor 
DEALLOCATE avail_cursor

UPDATE #tmpTableEVS
SET	#tmpTableEVS.district_name = dbEVS..district.district_name,
	#tmpTableEVS.district_chi = dbEVS..district.district_chi,
	#tmpTableEVS.district_board = dbEVS..district.district_board,
	#tmpTableEVS.district_board_chi = dbEVS..district.district_board_chi,
	#tmpTableEVS.area_name = dbEVS..district_area.area_name,
	#tmpTableEVS.area_chi = dbEVS..district_area.area_chi
FROM	dbEVS..district, dbEVS..district_area
WHERE	#tmpTableEVS.district = district.district_code collate database_default
		and district.district_area = district_area.area_code
		

------------------------
--EVS Professional (18 districts) Information
------------------------
create table #tmpEVSProftable_E
(
--district_code char(4),
--district_name varchar(100),
district_board varchar(100),
ENU int, 
RCM int,
RCP int, 
RDT int, 
RMP int, 
RMT int, 
RNU int, 
ROT int, 
RPT int, 
RRD int 
)

create table #tmpEVSProftable_S
(
--district_code char(4),
--district_name varchar(100),
district_board varchar(100),
ENU int, 
RCM int,
RCP int, 
RDT int, 
RMP int, 
RMT int, 
RNU int, 
ROT int, 
RPT int, 
RRD int 
)

create table #tmpEVSProftable_P
(
--district_code char(4),
--district_name varchar(100),
district_board varchar(100),
ENU int, 
RCM int,
RCP int, 
RDT int, 
RMP int, 
RMT int, 
RNU int, 
ROT int, 
RPT int, 
RRD int 
)

--declare @district_code char(4)
--declare @district_name varchar(100)
--declare @district_board varchar(100)
--declare @x int

DECLARE district_cursor CURSOR FOR 
    SELECT distinct district_board --, district_name
    from dbEVS..district 
	where district_area <> '.'
	order by district_board 

    OPEN district_cursor
    FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
	WHILE @@FETCH_STATUS = 0
    BEGIN
		select @x = 0
		insert into #tmpEVSProftable_E
		values
		(--@district_code,
		 --@district_name,
		 @district_board,
		 @x,
  		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x)
        
        FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
    END

Close district_cursor
DEALLOCATE district_cursor

DECLARE district_cursor CURSOR FOR 
    SELECT distinct district_board --, district_name
    from dbEVS..district 
	where district_area <> '.'
	order by district_board 

    OPEN district_cursor
    FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
	WHILE @@FETCH_STATUS = 0
    BEGIN
		select @x = 0
		insert into #tmpEVSProftable_S
		values
		(--@district_code,
		 --@district_name,
		 @district_board,
		 @x,
  		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x)
        
        FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
    END

Close district_cursor
DEALLOCATE district_cursor

DECLARE district_cursor CURSOR FOR 
    SELECT distinct district_board --, district_name
    from dbEVS..district 
	where district_area <> '.'
	order by district_board 

    OPEN district_cursor
    FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
	WHILE @@FETCH_STATUS = 0
    BEGIN
		select @x = 0
		insert into #tmpEVSProftable_P
		values
		(--@district_code,
		 --@district_name,
		 @district_board,
		 @x,
  		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x)
        
        FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
    END

Close district_cursor
DEALLOCATE district_cursor

DECLARE record_cursor CURSOR FOR 
    SELECT district_board, service_category_code, status
    --from #tmpTableEVS_E
    from #tmpTableEVS
	
    OPEN record_cursor
    FETCH NEXT FROM record_cursor INTO @rdistrict_board, @rservice_code, @status
	WHILE @@FETCH_STATUS = 0
    BEGIN
		
		if @rservice_code = 'ENU'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set ENU = ENU + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set ENU = ENU + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set ENU = ENU + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set ENU = ENU + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set ENU = ENU + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set ENU = ENU + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RCM'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RCM = RCM + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RCM = RCM + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RCM = RCM + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RCM = RCM + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RCM = RCM + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RCM = RCM + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RCP'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RCP = RCP + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RCP = RCP + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RCP = RCP + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RCP = RCP + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RCP = RCP + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RCP = RCP + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RDT'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RDT = RDT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RDT = RDT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RDT = RDT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RDT = RDT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RDT = RDT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RDT = RDT + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RMP'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RMP = RMP + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RMP = RMP + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RMP = RMP + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RMP = RMP + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RMP = RMP + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RMP = RMP + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RMT'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RMT = RMT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RMT = RMT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RMT = RMT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RMT = RMT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RMT = RMT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RMT = RMT + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RNU'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RNU = RNU + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RNU = RNU + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RNU = RNU + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RNU = RNU + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RNU = RNU + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RNU = RNU + 1
				where district_board = @rdistrict_board
			end
		end
 
		if @rservice_code = 'ROT'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set ROT = ROT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set ROT = ROT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set ROT = ROT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set ROT = ROT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set ROT = ROT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set ROT = ROT + 1
				where district_board = @rdistrict_board
			end
		end
 
		if @rservice_code = 'RPT'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RPT = RPT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RPT = RPT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RPT = RPT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RPT = RPT + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RPT = RPT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RPT = RPT + 1
				where district_board = @rdistrict_board
			end
		end
 
		if @rservice_code = 'RRD'
		begin 
			if @status = 'Approved' 
			begin
				update #tmpEVSProftable_P 
				set RRD = RRD + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_S
				set RRD = RRD + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RRD = RRD + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Processing' 
			begin
				update #tmpEVSProftable_S
				set RRD = RRD + 1
				where district_board = @rdistrict_board
				
				update #tmpEVSProftable_E
				set RRD = RRD + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpEVSProftable_E
				set RRD = RRD + 1
				where district_board = @rdistrict_board
			end
		end
		FETCH NEXT FROM record_cursor INTO @rdistrict_board, @rservice_code, @status
    END
close record_cursor
DEALLOCATE record_cursor

------------------------
--IVSS Practice Information
------------------------
create table #tmpTableIVSS
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)

create table #tmpTableIVSS_S
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)

create table #tmpTableIVSS_E
(
enrolment_ref_no char(15),
enrolment_dtm datetime,
display_seq smallint,
practice_ename nvarchar(100),
	Room	nvarchar(5),
	[Floor]	nvarchar(3),
	Block	nvarchar(3),
building varchar(100),
building_chi nvarchar(100) collate database_default,
district char(4) collate database_default,
district_name char(15) collate database_default,
district_chi nchar(30) collate database_default,
district_board char(15) collate database_default,
district_board_chi nchar(30) collate database_default,
area_name char(50) collate database_default,
area_chi nchar(50) collate database_default,
service_category_code char(5),
address_code int,
Status char(50)
)

insert into  #tmpTableIVSS_S
(
	enrolment_ref_no,
	enrolment_dtm,
	display_seq,
	practice_ename,
	Room,
	[Floor],
	Block,
	building,
	building_chi,
	district,
	service_category_code,
	address_code,
	Status
)
select	s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		p.practice_name,
	p.Room,
	p.[Floor],
	p.Block,
		p.building,
		p.building_chi,
		p.district,
		pf.service_category_code,
		p.address_code,
		'Processing'		
from dbIVSS..serviceproviderstaging s, dbIVSS..practicestaging p, dbIVSS..professionalstaging pf
where s.enrolment_ref_no = p.enrolment_ref_no
and	p.enrolment_ref_no = pf.enrolment_ref_no
and	p.Professional_Seq = pf.Professional_Seq
and s.submission_method = 'E'
--and p.submission_method = 'E'

insert into #tmpTableIVSS_E
(
	enrolment_ref_no,
	enrolment_dtm,
	display_seq,
	practice_ename,
	Room,
	[Floor],
	Block,
	building,
	building_chi,
	district,
	service_category_code,
	address_code,
	Status
)
select	s.enrolment_ref_no,
		s.enrolment_dtm,
		p.display_seq,
		p.practice_name,
	p.Room,
	p.[Floor],
	p.Block,
		p.building,
		p.building_chi,
		p.district,
		pf.service_category_code,
		p.address_code,
		'Submitted'		
from dbIVSS..serviceproviderenrolment s, dbIVSS..practiceenrolment p, dbIVSS..professionalenrolment pf
where s.enrolment_ref_no = p.enrolment_ref_no
and	p.enrolment_ref_no = pf.enrolment_ref_no
and	p.Professional_Seq = pf.Professional_Seq

insert into #tmpTableIVSS select * from #tmpTableIVSS_E
insert into #tmpTableIVSS select * from #tmpTableIVSS_S


DECLARE avail_cursor cursor 
FOR	SELECT address_code, display_seq, enrolment_ref_no
FROM #tmpTableIVSS

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq, @enrolment_ref_no
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE #tmpTableIVSS
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
	WHERE Display_Seq = @display_seq
			and enrolment_ref_no = @enrolment_ref_no
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq, @enrolment_ref_no
END

CLOSE avail_cursor 
DEALLOCATE avail_cursor

UPDATE #tmpTableIVSS
SET	#tmpTableIVSS.district_name = dbIVSS..district.district_name,
	#tmpTableIVSS.district_chi = dbIVSS..district.district_chi,
	#tmpTableIVSS.district_board = dbIVSS..district.district_board,
	#tmpTableIVSS.district_board_chi = dbIVSS..district.district_board_chi,
	#tmpTableIVSS.area_name = dbIVSS..district_area.area_name,
	#tmpTableIVSS.area_chi = dbIVSS..district_area.area_chi
FROM	dbIVSS..district, dbIVSS..district_area
WHERE	#tmpTableIVSS.district = district.district_code collate database_default
		and district.district_area = district_area.area_code


------------------------
--IVSS Professional (18 districts) Information
------------------------
create table #tmpIVSSProftable_E
(
--district_code char(4),
--district_name varchar(100),
district_board varchar(100),
ENU int, 
RCM int,
RCP int, 
RDT int, 
RMP int, 
RMT int, 
RNU int, 
ROT int, 
RPT int, 
RRD int 
)

create table #tmpIVSSProftable_S
(
--district_code char(4),
--district_name varchar(100),
district_board varchar(100),
ENU int, 
RCM int,
RCP int, 
RDT int, 
RMP int, 
RMT int, 
RNU int, 
ROT int, 
RPT int, 
RRD int 
)


DECLARE district_cursor CURSOR FOR 
    SELECT distinct district_board --, district_name
    from dbIVSS..district 
	where district_area <> '.'
	order by district_board 

    OPEN district_cursor
    FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
	WHILE @@FETCH_STATUS = 0
    BEGIN
		select @x = 0
		insert into #tmpIVSSProftable_E
		values
		(--@district_code,
		 --@district_name,
		 @district_board,
		 @x,
  		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x)
        
        FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
    END

Close district_cursor
DEALLOCATE district_cursor

DECLARE district_cursor CURSOR FOR 
    SELECT distinct district_board --, district_name
    from dbIVSS..district 
	where district_area <> '.'
	order by district_board 

    OPEN district_cursor
    FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
	WHILE @@FETCH_STATUS = 0
    BEGIN
		select @x = 0
		insert into #tmpIVSSProftable_S
		values
		(--@district_code,
		 --@district_name,
		 @district_board,
		 @x,
  		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x,
		 @x)
        
        FETCH NEXT FROM district_cursor INTO @district_board --@district_code, @district_name
    END

Close district_cursor
DEALLOCATE district_cursor

DECLARE record_cursor CURSOR FOR 
    SELECT district_board, service_category_code, status
    --from #tmpTableIVSS_E
    from #tmpTableIVSS
	
    OPEN record_cursor
    FETCH NEXT FROM record_cursor INTO @rdistrict_board, @rservice_code, @status
	WHILE @@FETCH_STATUS = 0
    BEGIN
		
		if @rservice_code = 'ENU'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set ENU = ENU + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set ENU = ENU + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set ENU = ENU + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RCM'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RCM = RCM + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RCM = RCM + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RCM = RCM + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RCP'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RCP = RCP + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RCP = RCP + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RCP = RCP + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RDT'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RDT = RDT + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RDT = RDT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RDT = RDT + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RMP'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RMP = RMP + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RMP = RMP + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RMP = RMP + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RMT'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RMT = RMT + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RMT = RMT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RMT = RMT + 1
				where district_board = @rdistrict_board
			end
		end

		if @rservice_code = 'RNU'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RNU = RNU + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RNU = RNU + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RNU = RNU + 1
				where district_board = @rdistrict_board
			end
		end
 
		if @rservice_code = 'ROT'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set ROT = ROT + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set ROT = ROT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set ROT = ROT + 1
				where district_board = @rdistrict_board
			end
		end
 
		if @rservice_code = 'RPT'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RPT = RPT + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RPT = RPT + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RPT = RPT + 1
				where district_board = @rdistrict_board
			end
		end
 
		if @rservice_code = 'RRD'
		begin 
			if @status = 'Processing' 
			begin
				update #tmpIVSSProftable_S
				set RRD = RRD + 1
				where district_board = @rdistrict_board
				
				update #tmpIVSSProftable_E
				set RRD = RRD + 1
				where district_board = @rdistrict_board
			end
			
			if @status = 'Submitted' 
			begin
				update #tmpIVSSProftable_E
				set RRD = RRD + 1
				where district_board = @rdistrict_board
			end
		end
		FETCH NEXT FROM record_cursor INTO @rdistrict_board, @rservice_code, @status
    END
close record_cursor
DEALLOCATE record_cursor


select 
enrolment_ref_no,
enrolment_dtm,
eng_name,
chi_name,
scheme_code from #tmpMO
order by enrolment_dtm

select 
		enrolment_ref_no, 
		enrolment_dtm,
		practice_ename, 
		case isnull(building,'')
			when '' then ''
			else isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,'')
		end,
		case isnull(building_chi,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
		end collate database_default,
		district_name, 
		district_board, 
		area_name,
		service_category_code,
		Status
from #tmpTableEVS
order by enrolment_dtm

select 
		enrolment_ref_no, 
		enrolment_dtm,
		practice_ename, 
		case isnull(building,'')
			when '' then ''
			else isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,'')
		end,
		case isnull(building_chi,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
		end collate database_default,
		district_name, 
		district_board, 
		area_name,
		service_category_code,
		Status
from #tmpTableIVSS
order by enrolment_dtm

select * from #tmpEVSProftable_P
select * from #tmpEVSProftable_S
select * from #tmpEVSProftable_E

select district_board, RMP from #tmpIVSSProftable_S
select district_board, RMP from #tmpIVSSProftable_E

--select * from #tmpEVSProftable_P



--select district_board, RMP from #tmpIVSSProftable2

drop table #tmpMO

drop table #tmpTableEVS_E
drop table #tmpTableEVS_S
drop table #tmpTableEVS_P
drop table #tmpTableEVS

drop table #tmpTableIVSS_E
drop table #tmpTableIVSS_S
drop table #tmpTableIVSS

drop table #tmpEVSProftable_E
drop table #tmpEVSProftable_S
drop table #tmpEVSProftable_P

drop table #tmpIVSSProftable_E
drop table #tmpIVSSProftable_S

drop table #tmpEVSPracticeLog
drop table #tmpIVSSPracticeLog



END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_MO_Stat] TO HCVU
GO
