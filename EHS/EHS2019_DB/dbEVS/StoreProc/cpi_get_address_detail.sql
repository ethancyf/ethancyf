IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_get_address_detail]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_get_address_detail]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	19 April 2017
-- Description:		Rename table District_Board to DistrictBoard
-- =============================================
/******************************************************************************************/
/* Server:    HATCMSD02\DEV2															  */
/* Database:  dbTCM_CORP																  */
/* Created Date/Time: 20-Jul-2005														  */
/* Created by: PAS				  														  */
/* Description : The source code is from PAS team. The program helps to find the matching */
/*				 address code of the given details										  */
/* Expected Results: The records will be retrieved from serveral tables: address_detail	  */
/*					 district, district_area, elderly_home_table 						  */
/*					 Such program will be called by the following SP:					  */
/*					 - sp_tcm_get_patientdetail											  */
/*					 - sp_tcm_get_patientdetail_appointed								  */
/*					 - sp_tcm_get_patientdetail_by_patient_id							  */
/*					 - sp_tcm_rpt_0302													  */
/* Sorting Order: NA																	  */
/******************************************************************************************/ 
/* Modification History																	  */
/* Modified By:	Maggie Lee																  */
/* Modified Date/Time: 05-May-2008														  */
/* Details:	Rewrite SQL statement to make compatible with SQL2005 syntax				  */
/******************************************************************************************/
/* Modified By:	Maggie Lee																  */
/* Modified Date/Time: 27-Aug-2008														  */
/* Details:	If either Address_Detail.estate_eng or Address_Detail.street_eng is NULL, the */
/*			Comma will be missing shown in english address field						  */
/******************************************************************************************/
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Struture
-- =============================================

CREATE PROCEDURE dbo.cpi_get_address_detail (@record_id int, @address_eng varchar(255) output, 
											@address_chi varchar(255) output, @district_code char(5) output, 
											@eh_eng varchar(255) output, @eh_chi varchar(255) output
										)
AS
begin
	select @address_eng =
         (CASE WHEN bldg_eng is null THEN ''  ELSE rtrim(bldg_eng)+
       --(CASE WHEN estate_eng+street_eng is null THEN '' ELSE ', ' END) END)+
         (CASE WHEN isnull(estate_eng,'') + isnull(street_eng,'') = '' THEN '' ELSE ', ' END) END)+ -- Modified by Maggie @20080827
         (CASE WHEN estate_eng is null THEN ''  ELSE rtrim(estate_eng)+
         (CASE WHEN street_eng is null THEN ''  ELSE ', ' END) END)+
         (CASE WHEN house_no is null then '' ELSE rtrim(house_no) +' ' END) +
         (CASE WHEN street_eng is null then '' ELSE rtrim(street_eng) END),
			@address_chi = 
         rtrim(area_chi) + rtrim(district_chi) +
         (CASE WHEN street_chi is null then '' ELSE rtrim(street_chi) END) +
         (CASE WHEN house_no is null then '' ELSE rtrim(house_no) END) +
         (CASE WHEN estate_chi is null then '' ELSE rtrim(estate_chi) END) +
         (CASE WHEN bldg_chi is null THEN '' ELSE rtrim(bldg_chi) END),
	 @district_code = a.district_code,
	 @eh_eng = d.eh_name,
	 @eh_chi = d.eh_chinese_name
	from address_detail a -- Modified by Maggie @20080505
	  left outer join elderly_home_table d on  a.record_id = d.eh_address_id 
	  JOIN district b on a.district_code = b.district_code 
	  JOIN DistrictBoard db ON b.District_Board = db.District_Board
	  JOIN district_area c on db.Area_Code = c.area_code
	 where a.record_id = @record_id 				
				
end
GO

GRANT EXECUTE ON [dbo].[cpi_get_address_detail] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_get_address_detail] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_get_address_detail] TO HCVU
GO
