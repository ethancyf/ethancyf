IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_search]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_search]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************************/
/* Server:    HATCMSD02\DEV2															  */
/* Database:  dbTCM_CORP																  */
/* Created Date/Time: 20-Jul-2005														  */
/* Created by: PAS				  														  */
/* Description : The source code is from PAS team. The program helps to find the matching */
/*				 address code of the given details										  */
/* Expected Results: The records will be retrieved from serveral tables: address_detail	  */
/*					 district, district_area, elderly_home_table and address_key_new	  */
/*					 The following sub-stored procedures will be called:				  */
/*					- dbo.cpi_address_cut_head											  */
/*					- dbo.cpi_address_cut_type											  */
/*					- dbo.cpi_address_phonetic_word										  */
/*					- dbo.cpi_address_build_long_key									  */
/*					- dbo.cpi_address_build_short_key									  */
/*					- dbo.cpi_address_build_soundex_key									  */
/* Sorting Order: NA																	  */
/******************************************************************************************/ 
/* Modification History																	  */
/* Modified By:	Maggie Lee																  */
/* Modified Date/Time: 05-May-2008														  */
/* Details:	Rewrite SQL statement to make compatible with SQL2005 syntax				  */
/******************************************************************************************/
/* Modified By:	Maggie Lee																  */
/* Modified Date/Time: 09-May-2008														  */
/* Details:	Restrict the collation used in table #temp_result							  */
/******************************************************************************************/
/* Modified By:	Maggie Lee																  */
/* Modified Date/Time: 20-Aug-2008														  */
/* Details:	If either Address_Detail.estate_eng or Address_Detail.street_eng is NULL, the */
/*			Comma will be missing shown in english address field						  */
/******************************************************************************************/

-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 08 May 2009
-- Description:	1. Remove duplicated records return
--				2. Make SP logic consistence with Orginal Copy (from CS8)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Struture
-- =============================================

CREATE PROCEDURE [dbo].[cpi_address_search] (   @region varchar(2)
					,@street_in varchar(50)
					,@build_land_in varchar(50)
					,@location_in varchar(50)
					,@location_all varchar(255)
					,@district_code as varchar(5)
					
				     )
AS
begin

declare @long_key varchar(24)
declare @short_key varchar(7)
declare @soundex_key varchar(8)
declare @org_location_all varchar(255)
declare @org_build_land_in varchar(255)
declare @temp_location_all varchar(255)
declare @district_name char(15)
declare @pos1 int
declare @pos2 int

if ltrim(rtrim(@district_code)) = ''
   select @district_code = NULL
   
create table #temp_all (record_id int, record_type char(1), search_type int)
create table #temp_final (record_id int, search_type int)
create table #temp_building (record_id int, record_type char(1), search_type int)
create table #temp_location (record_id int, record_type char(1), search_type int)
create table #temp_street (record_id int, record_type char(1) collate database_default, search_type int)
create table #temp_result 
( record_id int,
  address_eng varchar(255), 
  address_chi varchar(255) COLLATE DATABASE_DEFAULT,
  district_code char(5) collate database_default,
  district_name char(15) collate database_default,
  search_type int )

if @street_in is null and @build_land_in is null 
  and @location_in is null and @location_all is null
   goto final_result 

select @street_in = upper(ltrim(rtrim(@street_in)))
select @build_land_in = upper(ltrim(rtrim(@build_land_in)))
select @location_in = upper(ltrim(rtrim(@location_in)))
select @location_all = upper(ltrim(rtrim(@location_all)))
   
if substring(@location_all,1, 5) = 'BLOCK' or
   substring(@location_all,1, 5) = 'TOWER' or
   substring(@location_all,1, 5) = 'HOUSE'
begin
  select @temp_location_all = substring(@location_all, 7, len(@location_all)-6)
   while charindex(',', rtrim(@temp_location_all)) != 0
   select @temp_location_all = 
      stuff(@temp_location_all, charindex(',', @temp_location_all), 1, space(1))
	select @pos2 = charindex(' ', @temp_location_all)
	if @pos2 > 1
		select @org_location_all = substring(@temp_location_all, 1, 
														@pos2 - 1)
	else
	   select @org_location_all = @temp_location_all

end
else
begin
	if substring(@location_all,1,1) in ('0','1','2','3','4','5','6','7','8','9') or
		substring(@location_all,2,1) in ('0','1','2','3','4','5','6','7','8','9')
	begin
		select @pos1 = charindex(',', @location_all)
		select @pos2 = charindex(' ', @location_all)
		if @pos1 > 1 and @pos2 > 1
			if @pos1 > @pos2
				select @org_location_all = substring(@location_all, 1, @pos2 - 1)
			else
				select @org_location_all = substring(@location_all, 1, @pos1 - 1)
		else
			select @org_location_all = @location_all
	end
	else
		select @org_location_all = @location_all
end

/* print  @org_location_all */

if substring(@build_land_in,1,1) in ('0','1','2','3','4','5','6','7','8','9') or
	substring(@build_land_in,2,1) in ('0','1','2','3','4','5','6','7','8','9') or
	@build_land_in like 'BLOCK%' or
	@build_land_in like 'BLK%' or
	@build_land_in like 'HOUSE%' or
	@build_land_in like 'NO%' 
begin
   if @build_land_in like 'BLK%'
      select @org_build_land_in = 'BLOCK' + substring(@build_land_in,4,len(@build_land_in))
   else
      select @org_build_land_in = @build_land_in
	select @build_land_in = null
end

/* print @org_location_all */
select @district_name = null
select @district_name = ltrim(rtrim(district_name)) from district
			  where rtrim(@org_location_all) like '%'+rtrim(district_name)

/* print @district_name */
if @district_name is not null and @district_name != ''
   if len(rtrim(@org_location_all)) != len(rtrim(@district_name))
      select @location_all = substring(@org_location_all, 1, len(rtrim(@org_location_all))
	                     - len(rtrim(@district_name)))

if @region = 'AL'
   select @region = '%'

select @long_key = ''
select @short_key = ''
select @soundex_key = ''

-- Initialize NULL value
if @street_in = ''
    select @street_in = NULL
    
if @build_land_in = ''
    select @build_land_in = NULL

if @location_in = ''
    select @location_in = NULL
    
if @location_all = ''
    select @location_all = NULL    
   
if ltrim(rtrim(@org_location_all) )= ''
    select @org_location_all = NULL    
    
if ltrim(rtrim(@district_name) )= ''
    select @district_name = NULL       
    
exec cpi_address_phonetic_word @build_land_in, @build_land_in output
exec cpi_address_phonetic_word @street_in, @street_in output
exec cpi_address_phonetic_word @location_in, @location_in output
exec cpi_address_phonetic_word @location_all, @location_all output

exec cpi_address_cut_type @build_land_in, @build_land_in output
exec cpi_address_cut_type @street_in, @street_in output
exec cpi_address_cut_type @location_in, @location_in output
exec cpi_address_cut_head @location_all, @location_all output

-----------------------------------------
/* For all */
-----------------------------------------
if @location_all is not null 
begin     
	/*  --Use long key */
	exec cpi_address_build_long_key @location_all, @long_key output
	
	/*  --print @long_key */
	/* For OAH */
	if @long_key like 'OAH%'
	  if @long_key = 'OAH'
		select @long_key = rtrim(@long_key) + '%'
	  else
		select @long_key = rtrim(@long_key)
	else
		select @long_key = rtrim(@long_key) + '%'

   	insert into #temp_all
	   select distinct record_id, record_type, 9 
	    from address_key_new 
	         where long_key like @long_key
	           and record_type in ('L', 'V')
	           and region like @region
	           and record_id not in (select record_id from #temp_all)

	   insert into #temp_all
	   select distinct record_id, record_type, 8 from address_key_new where long_key like @long_key
	                                             and record_type = 'B'
	                                             and region like @region
	                                             and record_id not in (select record_id from #temp_all)
	   insert into #temp_all
	   select distinct record_id, record_type, 7 from address_key_new where long_key like @long_key
	                                             and record_type in ('S','I')
	                                             and region like @region
	                                             and record_id not in (select record_id from #temp_all)
	   insert into #temp_all
	   select distinct record_id, record_type, 7 from address_key_new where long_key like @long_key
	                                             and record_type in ('U','O','T','A','G')
	                                             and region like @region
	                                             and record_id not in (select record_id from #temp_all)
      /*  Use short key */
	exec cpi_address_build_short_key @location_all, @short_key output
	
      /* --print @short_key */
	select @short_key = ltrim(rtrim(@short_key)) + '%'

      insert into #temp_all   
         select distinct record_id, record_type, 6 
           from address_key_new 
          where short_key like @short_key
             and record_type in ('L','V')
             and region like @region
             and record_id not in (select record_id from #temp_all)
      
      insert into #temp_all
    	  select distinct record_id, record_type, 5 
    	   from address_key_new  
             where short_key like @short_key
               and record_type = 'B'
               and region like @region
               and record_id not in (select record_id from #temp_all)
      
      insert into #temp_all
          select distinct record_id, record_type, 4 
          from address_key_new   
             where short_key like @short_key
              and record_type in ('S','I')
              and region like @region
              and record_id not in (select record_id from #temp_all)

      insert into #temp_all
      select distinct record_id, record_type, 4 
        from address_key_new  
           where short_key like @short_key
             and record_type in ('U','O','T','A','G')
             and region like @region
             and record_id not in (select record_id from #temp_all)
     
     /* Use soundex key */
     exec cpi_address_build_soundex_key @location_all, @soundex_key output

     /* --print @soundex_key */
     insert into #temp_all
         select distinct record_id, record_type, 3 from address_key_new 
		where soundex_key = @soundex_key
                     and record_type in ('L','V')
                     and region like @region
                     and record_id not in (select record_id from #temp_all)
     
     insert into #temp_all
         select distinct record_id, record_type, 2 from address_key_new 
		where soundex_key = @soundex_key
                     and record_type = 'B'
                     and region like @region
                     and record_id not in (select record_id from #temp_all)
  
     insert into #temp_all
         select distinct record_id, record_type, 1 from address_key_new 
		where soundex_key = @soundex_key
                     and record_type in ('S','I')
                     and region like @region
                     and record_id not in (select record_id from #temp_all)

     insert into #temp_all
         select distinct record_id, record_type, 1 from address_key_new
		where soundex_key = @soundex_key
                     and record_type in ('U','O','T','A','G')
                     and region like @region
                     and record_id not in (select record_id from #temp_all)

     goto return_result

end

search_location:
-----------------------------------------
/* For location */
-----------------------------------------
if @location_in is not null
begin
	/* Use long key */
	exec cpi_address_build_long_key @location_in, @long_key output
	select @long_key = rtrim(@long_key) + '%'
	
	insert into #temp_location
	select distinct record_id, record_type, 48 from address_key_new 
	   where long_key like @long_key 
		and record_type in ('L','V')
		and region like @region
	
	insert into #temp_location
	  select distinct record_id, record_type, 16 from address_key_new 
	   where long_key like @long_key 
		and record_type in ('O','A')
		and region like @region
		and record_id not in (select record_id from #temp_location)
	
	/* Use short key */
	exec cpi_address_build_short_key @location_in, @short_key output
	select @short_key = ltrim(rtrim(@short_key)) + '%'
	insert into #temp_location	
	select distinct record_id, record_type, 32
	 from address_key_new 
	  where short_key like @short_key
		and record_type in ('L','V')
		and region like @region
		and record_id not in (select record_id from #temp_location)
	
	insert into #temp_location	
	   select distinct record_id, record_type, 15 
	   from address_key_new   
	      where short_key like @short_key
		and record_type in ('O','A')
		and region like @region
		and record_id not in (select record_id from #temp_location)

	-- Use soundex key
	exec cpi_address_build_soundex_key @location_in, @soundex_key output

	insert into #temp_location	
 	  select distinct record_id, record_type, 16 from address_key_new 
	    where soundex_key = @soundex_key 
	      and record_type in ('L','V')
	      and region like @region
	      and record_id not in (select record_id from #temp_location)

	insert into #temp_location	
  	  select distinct record_id, record_type, 14 from address_key_new 
	    where soundex_key = @soundex_key 
		and record_type in ('O','A')
		and region like @region
		and record_id not in (select record_id from #temp_location)
end

search_building:
-----------------------------------------
/* For building                        */
-----------------------------------------
if  @build_land_in is not null
begin
	/* Use long key */
	exec cpi_address_build_long_key @build_land_in, @long_key output
	select @long_key = rtrim(@long_key) + '%'

	insert into #temp_building
	select distinct record_id, record_type, 12 from address_key_new 
		where long_key like @long_key and record_type = 'B'
		   and region like @region
			   
	insert into #temp_building
	select distinct record_id, record_type, 4 from address_key_new 
		where long_key like @long_key and record_type = 'U'
			and region like @region
			and record_id not in (select record_id from #temp_building)
			and region like @region
	/* Use short key */
	exec cpi_address_build_short_key @build_land_in, @short_key output
	select @short_key = ltrim(rtrim(@short_key)) + '%'
	if (@location_in is null) or ltrim(rtrim(@location_in)) = ''
	begin
		insert into #temp_building	
		select distinct record_id, record_type, 8 from address_key_new   
		  where short_key like @short_key
			and record_type = 'B'
			and region like @region
			and record_id not in (select record_id from #temp_building)
	
		insert into #temp_building	
		select distinct record_id, record_type, 3 from address_key_new   
		  where short_key like @short_key
			and record_type = 'U'
			and region like @region
			and record_id not in (select record_id from #temp_building)
	end
	else
	       begin
			insert into #temp_building	
			select distinct record_id, record_type, 8 
			from address_key_new 
			  where short_key like @short_key
				and record_type = 'B'
				and region like @region
				and record_id in (select record_id from #temp_location)
				and record_id not in (select record_id from #temp_building)
			
			insert into #temp_building	
			select distinct record_id, record_type, 3 
			 from address_key_new 
			   where short_key like @short_key
				and record_type = 'U'
				and region like @region
				and record_id in (select record_id from #temp_location)
				and record_id not in (select record_id from #temp_building)
		end

		/* Use soundex key */
		exec cpi_address_build_soundex_key @build_land_in, @soundex_key output
		if (@location_in is null)
		begin
			insert into #temp_building	
			select distinct record_id, record_type, 4 from address_key_new 
			  where soundex_key = @soundex_key and record_type = 'B'
				and region like @region
				and record_id not in (select record_id from #temp_building)
			
			insert into #temp_building	
			select distinct record_id, record_type, 2 from address_key_new 
			  where soundex_key = @soundex_key and record_type = 'U'
				and region like @region
				and record_id not in (select record_id from #temp_building)
		end
		else
		begin
			insert into #temp_building	
			select distinct record_id, record_type, 4 from address_key_new 
			  where soundex_key = @soundex_key and record_type = 'B'
				and region like @region
				and record_id in (select record_id from #temp_location)
				and record_id not in (select record_id from #temp_building)
			
			insert into #temp_building	
			select distinct record_id, record_type, 2 from address_key_new 
			  where soundex_key = @soundex_key and record_type = 'U'
				and region like @region
				and record_id in (select record_id from #temp_location)
				and record_id not in (select record_id from #temp_building)
		end
end	

search_street:
-----------------------------------------
/* For street */
-----------------------------------------
if @street_in is not null 
begin	

	/* Use long key */
	exec cpi_address_build_long_key @street_in, @long_key output

	/* print @long_key */
	select @long_key = @long_key + '%'
	
	insert into #temp_street
	select distinct record_id, record_type, 3 from address_key_new 
	  where long_key like @long_key 
		and record_type in ('S','I')
		and region like @region
	
	insert into #temp_street
	select distinct record_id, record_type, 1 from address_key_new 
	  where long_key like @long_key 
		and record_type in ('T','G')
		and region like @region
		and record_id not in (select record_id from #temp_street)

	/* Use short key */
	exec cpi_address_build_short_key @street_in, @short_key output
	select @short_key = ltrim(rtrim(@short_key)) + '%'
	
	insert into #temp_street	
	select distinct record_id, record_type, 2 
	 from address_key_new 
	  where short_key like  @short_key
		and record_type in ('S','I')
		and region like @region
		and record_id not in (select record_id from #temp_street)
	
	insert into #temp_street	
	select distinct record_id, record_type, 1 
	  from address_key_new   
	  where short_key like  @short_key
		and record_type in ('T','G')
		and region like @region
		and record_id not in (select record_id from #temp_street)

	/* Use soundex key */
	exec cpi_address_build_soundex_key @street_in, @soundex_key output
	
	insert into #temp_street	
	  select distinct record_id, record_type, 1 from address_key_new 
		   where soundex_key = @soundex_key 
		and record_type in ('S','I')
		and region like @region
		and record_id not in (select record_id from #temp_street)
		
	insert into #temp_street	
	  select distinct record_id, record_type, 1 from address_key_new 
	    where soundex_key = @soundex_key 
		and record_type in ('T','G')
		and region like @region
		and record_id not in (select record_id from #temp_street)
	
	goto return_result

end

return_result:
if (@build_land_in is null)
begin
	insert into #temp_building select record_id, record_type, 0 from #temp_location
	insert into #temp_building select record_id, record_type, 0 from #temp_street
end
if (@location_in is null)
begin
	insert into #temp_location select record_id, record_type, 0 from #temp_building
	insert into #temp_location select record_id, record_type, 0 from #temp_street
end
if (@street_in is null)
begin
	insert into #temp_street select record_id, record_type, 0 from #temp_location
	insert into #temp_street select record_id, record_type, 0 from #temp_building
end

/*
----------------------------
 Get the final result
----------------------------
*/
-- Modified by Kathy @20090508: Make SP logic consistence with Orginal Copy (from CS8)
-- if (@build_land_in is null)
if (@location_all is null)
begin
	insert into #temp_all
	  select * from #temp_building where record_type not in ('V','I','A','G')
	
	/* convert village key */
	insert into #temp_all
	  select a.record_id, a.record_type, b.search_type 
	    from address_key_new a, #temp_building b
	      where b.record_type in ('V','I','A','G')
		and a.short_key = convert(char(7), b.record_id)

	/* select * from #temp_building */
	insert into #temp_all
	  select * from #temp_location where record_type not in ('V','I','A','G')
	
	/* convert village key */
	insert into #temp_all
	  select a.record_id, a.record_type, b.search_type 
	    from address_key_new a, #temp_location b
	      where b.record_type in ('V','I','A','G')
   		and a.short_key = convert(char(7), b.record_id)

	/* select * from #temp_location */
	insert into #temp_all
	  select * from #temp_street where record_type not in ('V','I','A','G')
	
	/* convert village key */
	insert into #temp_all
	  select a.record_id, a.record_type, b.search_type 
	    from address_key_new a, #temp_street b
	      where b.record_type in ('V','I','A','G')
	          and a.short_key = convert(char(7), b.record_id)

	/* select * from #temp_street */
	insert into #temp_final
	  select distinct record_id, sum(search_type) from #temp_all
   	    group by record_id 
		having count(*) >= 3
end
else
begin

	insert into #temp_final
	    select distinct record_id, sum(search_type) from #temp_all
		group by record_id 

	/* Needed to replace the village key record with the address record */
	/* select count(*) from #temp_final */
	insert into #temp_final
	select a.record_id, b.search_type from address_key_new a, #temp_final b
		where a.record_type = 'E'
		and a.short_key = convert(char(7), b.record_id)
		-- Added by Kathy @20090508 Removed duplicated records
		and a.record_id not in (select record_id from #temp_final)

end

/* return result set */
insert into #temp_result
  select aa.record_id, 
	(CASE WHEN bldg_eng is null THEN '' ELSE rtrim(bldg_eng)+
	--(CASE WHEN estate_eng+street_eng is null THEN '' ELSE ', ' END) END)+
	(CASE WHEN isnull(estate_eng,'') + isnull(street_eng,'') = '' THEN '' ELSE ', ' END) END)+ -- Modified by Maggie @20080820
	(CASE WHEN estate_eng is null THEN '' ELSE rtrim(estate_eng)+
		(CASE WHEN street_eng is null THEN '' ELSE ', ' END) END)+
         (CASE WHEN house_no is null then '' ELSE rtrim(house_no) +' ' END) +
         (CASE WHEN street_eng is null then '' ELSE rtrim(street_eng) END) as address_eng,
         (CASE WHEN d.eh_chinese_name is null then '' ELSE rtrim(d.eh_chinese_name)+'--' END) +
                rtrim(area_chi) + rtrim(district_chi) +
         (CASE WHEN street_chi is null then '' ELSE rtrim(street_chi) END) +
         (CASE WHEN house_no is null then '' ELSE rtrim(house_no)+char(184)+char(185) END) +
         (CASE WHEN estate_chi is null then '' ELSE rtrim(estate_chi) END) +
         (CASE WHEN bldg_chi is null THEN '' ELSE rtrim(bldg_chi) END) as address_chi, 
            aa.district_code, ad.district_name
	, search_type 
   from address_detail aa -- Modified by Maggie @20080506:Rewrite SQL statement to make compatible with SQL2005
     JOIN District ad on aa.District_Code = ad.District_Code
     JOIN DistrictBoard db ON ad.District_Board = db.District_Board
     JOIN District_Area ae on db.Area_Code = ae.Area_Code
     left outer join elderly_home_table d on aa.record_id  = d.eh_address_id 
     join #temp_final ab on aa.record_id = ab.record_id
     WHERE ae.BO_Input_Avail = 'Y'


/* add mark to these address that match house no or BLOCK, TOWER and NO*/
-- Modified by Kathy @20090508: Make SP logic consistence with Orginal Copy (from CS8)
-- if (@location_all is null) or ltrim(rtrim(@street_in)) = ''
if (@location_all is null)
begin 
	if @org_build_land_in is not null 
	begin
		update #temp_result
		set search_type = search_type + 1
		from #temp_result a, address_detail b
		where a.record_id = b.record_id
		and 
		  (( b.bldg_eng is null
		     and (rtrim(b.house_no)) like rtrim(@org_build_land_in)+'%')
		   or ( b.bldg_eng is not null
		           and (rtrim(b.bldg_eng)) like '%'+ @org_build_land_in)
		)
	end
end
else
begin
	/* add mark to these address with district info that matched */
	update #temp_result
	set search_type = search_type * 2
	where @org_location_all like '%'+ district_name 

	/* add mark to these address building match house no , block, tower or house of location_all */
	update #temp_result
	set search_type = search_type + 1
	from #temp_result a, address_detail b
	where a.record_id = b.record_id
	and 
	(
		right(rtrim(b.bldg_eng),1) = ')'
		and rtrim(b.bldg_eng) like '%'+rtrim(substring(@org_location_all,1,2))+')'
		or
		right(rtrim(b.bldg_eng),1) <> ')' and b.bldg_eng is not null
		and rtrim(b.bldg_eng) like '%'+substring(@org_location_all,1,2)
		or
		b.bldg_eng is null
		and (rtrim(b.house_no)) like substring(@org_location_all,1,3)+'%'
		or
		b.bldg_eng is not null
		and (rtrim(b.bldg_eng)) like '%'+substring(@org_location_all,1,3)
	)
end

DECLARE @maxrow int
declare @rowcount int

select @rowcount = count(1)       
from #temp_result  t -- Modified by Maggie @20080506:Rewrite SQL statement to make compatible with SQL2005
        left outer join district d on t.district_code  = d.district_code
           where ((@district_code is not null and t.district_code = @district_code)
                   or (@district_code is null))

SELECT	@maxrow =  Parm_Value1
FROM	dbo.SystemParameters
WHERE	Parameter_name = 'MaxRowRetrieveStructureAddress' and 
		Record_Status = 'A' AND Scheme_Code = 'ALL'

if @rowcount > @maxrow
begin
		Raiserror('00002', 16, 1)
		return @@error
end


final_result:
select record_id, isnull(address_eng,'') as address_eng
     , isnull(address_chi,'') as address_chi, t.district_code 
     , d.district_area as area_code
      from #temp_result  t -- Modified by Maggie @20080506:Rewrite SQL statement to make compatible with SQL2005
        left outer join district d on t.district_code  = d.district_code
           where ((@district_code is not null and t.district_code = @district_code)
                   or (@district_code is null))
	     order by search_type desc, address_eng asc

drop table #temp_all
drop table #temp_final
drop table #temp_building
drop table #temp_location
drop table #temp_street
drop table #temp_result
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_search] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_search] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_search] TO HCVU
GO
