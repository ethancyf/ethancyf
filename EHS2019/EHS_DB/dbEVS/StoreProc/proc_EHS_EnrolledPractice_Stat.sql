IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_EnrolledPractice_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_EnrolledPractice_Stat]
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
-- Modification History
-- Modified by:		Tommy TSE
-- Modified date:	9 Sep 2011
-- CR No.:			CRE11-024-01 (Enhancement on HCVS Extension Part 1)
-- Description:		Profession related data is
--					retrieved from table [profession]
-- =============================================

-- =============================================
-- Author:		Kathy LEE
-- Create date: 14 Nov 2008
-- Description:	Retrieve the Enrolled Practice Information			
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_EnrolledPractice_Stat]
AS
BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255),
		@display_seq smallint
DECLARE @enrolment_ref_no varchar(15)

create table #tmpEVS
(
	enrolment_ref_no char(15),
	effective_dtm datetime,
	sp_id char(8),
	sp_eng_name varchar(40),
	sp_chi_name nvarchar(6),
	display_seq smallint,
	practice_name nvarchar(100),
	room	nvarchar(5),
	[floor]	nvarchar(3),
	block	nvarchar(3),
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
	service_category varchar(50),
	service_category_chi nvarchar(50),
	address_code int,
	phone_daytime varchar(20)
)

EXEC [proc_SymmetricKey_close]

insert into #tmpEVS
(
	enrolment_ref_no,
	effective_dtm,
	sp_id,
	sp_eng_name,
	sp_chi_name,
	display_seq,
	practice_name,
	room,
	[floor],
	block,
	building,
	building_chi,
	district,
	service_category_code,
	service_category,
	service_category_chi,
	address_code		
)
select	s.enrolment_ref_no,
		s.effective_dtm,
		s.sp_id,
		convert(varchar(40), DecryptByKey(s.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(s.Encrypt_Field3)),
		p.display_seq,
		p.practice_name,
		p.room,
		p.[floor],
		p.block,
		p.building,
		p.building_chi,
		p.district,
		pf.service_category_code,
		[profession].[Service_Category_Desc],
		[profession].[Service_Category_Desc_Chi],
		p.address_code
		
from	serviceprovider s,
		practice p,
		professional pf,
		[profession]
where	s.sp_id = p.sp_id
	and p.sp_id = pf.sp_id
	and p.professional_seq = pf.professional_seq
	and [profession].[Service_Category_Code] = pf.service_category_code
	and ([profession].[Enrol_Period_From] IS NULL OR DATEDIFF(SECOND, [profession].[Enrol_Period_From], GETDATE()) >= 0 )
	and ([profession].[Enrol_Period_To] IS NULL OR DATEDIFF(SECOND, GETDATE(), [profession].[Enrol_Period_To]) > 0 )

EXEC [proc_SymmetricKey_close]

DECLARE avail_cursor cursor 
FOR	SELECT address_code, display_seq, enrolment_ref_no
FROM #tmpEVS

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

	UPDATE #tmpEVS
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


UPDATE #tmpEVS
SET	#tmpEVS.district_name = district.district_name,
	#tmpEVS.district_chi = district.district_chi,
	#tmpEVS.district_board = district.district_board,
	#tmpEVS.district_board_chi = district.district_board_chi,
	#tmpEVS.area_name = district_area.area_name,
	#tmpEVS.area_chi = district_area.area_chi
FROM	district, district_area
WHERE	#tmpEVS.district = district.district_code collate database_default
		and district.district_area = district_area.area_code
		
UPDATE #tmpEVS
SET #tmpEVS.phone_daytime = MOPractice.Phone_Daytime
FROM MOPractice
WHERE #tmpEVS.enrolment_ref_no = MOPractice.enrolment_ref_no
	  and #tmpEVS.display_seq = MOPractice.display_seq

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime
from #tmpEVS
where service_category_code = 'ENU'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime		
from #tmpEVS
where service_category_code = 'RCM'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime
from #tmpEVS
where service_category_code = 'RCP'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime	
from #tmpEVS
where service_category_code = 'RDT'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime
from #tmpEVS
where service_category_code = 'RMP'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime
from #tmpEVS
where service_category_code = 'RMT'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime	
from #tmpEVS
where service_category_code = 'RNU'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime	
from #tmpEVS
where service_category_code = 'ROT'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime
from #tmpEVS
where service_category_code = 'RPT'
order by area_name, district_board, district, enrolment_ref_no

select 	
	area_name,
	district_board,
	district_name,
	enrolment_ref_no,
	sp_id,
	effective_dtm,
	sp_eng_name,
	display_seq,
	practice_name,
	isnull('Room '+ room + ', ', '') + isnull('Floor ' + [floor] + ', ', '') + isnull('Block ' + block + ', ', '') + isnull(building,''),
	service_category,
	area_chi,
	district_board_chi,
	district_chi,
	sp_chi_name,
	case isnull(address_code,'')
			when '' then ''
			else building_chi + isnull(block + '座', '') + isnull([floor] + '樓', '') + isnull(room + '室', '')
	end collate database_default,
	service_category_chi,
	phone_daytime	
from #tmpEVS
where service_category_code = 'RRD'
order by area_name, district_board, district, enrolment_ref_no

drop table #tmpEVS
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_EnrolledPractice_Stat] TO HCVU
GO
