IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_PracticeList_withoutFee]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_PracticeList_withoutFee]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	22 Aug 2016
-- CR No.:			CRE16-002
-- Description:		Obsolete sproc
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004 (TIV and QIV)
-- Modified by:		Philip Chau
-- Modified date:	08 July 2015
-- Description:		Bug fix for filtering out the expiry subsidies
-- =============================================
-- =============================================
-- Author:		Mattie LO
-- Create date: 30 August 2009
-- Description:	 Retrieve the Practice List without Service Fee and joined specific scheme
-- =============================================

-- =============================================
-- Author:		Mattie LO
-- Update date: 21 October 2009
-- Description:	 Bug fix for truncating the English Name of the SP when Chinese Name is NULL
-- =============================================
/*
CREATE PROCEDURE [dbo].[proc_HCSD_get_PracticeList_withoutFee] 
	@Professional varchar(5)
	,@DistrictList varchar(200)
	,@subsidize_item_1 varchar(20)
	,@subsidize_item_2 varchar(20)
	,@subsidize_item_3 varchar(20)
	,@subsidize_item_4 varchar(20)
	,@subsidize_item_5 varchar(20)
	,@subsidize_item_6 varchar(20)
	,@subsidize_item_7 varchar(20)
	,@subsidize_item_8 varchar(20)
	,@subsidize_item_9 varchar(20)
	,@subsidize_item_10 varchar(20)
	,@language varchar(10)
AS
BEGIN

CREATE TABLE #DistrictList(DistrictBoard varchar(5) COLLATE Chinese_Taiwan_Stroke_CI_AS)

IF @DistrictList IS NOT NULL
   INSERT #DistrictList EXECUTE proc_HCSDUtilStringToTable @DistrictList

IF @language = 'ZH-TW' BEGIN
SELECT top 801 [sp_id]
      ,[practice_display_seq]
      ,[sp_name]
      ,isnull([sp_chi_name], '') as [sp_chi_name]
      ,[practice_name]
      ,isnull([practice_name_chi], '') as [practice_name_chi]
      ,isnull([phone_daytime], '') as [phone_daytime]
      ,[service_category_code_SD]
      ,[district_code]
      ,[district_board_shortname_SD]
      ,[area_code]
      ,[address_eng]
      ,[district_name]
      ,[district_board]
      ,[area_name]
      ,isnull([address_chi], '') as [address_chi]
      ,[district_name_chi]
      ,[district_board_chi]
      ,[area_name_chi]
      ,isnull([joined_scheme_1], '') as [joined_scheme_1]
      ,isnull([joined_scheme_2], '') as [joined_scheme_2]
      ,isnull([joined_scheme_3], '') as [joined_scheme_3]
      ,isnull([joined_scheme_4], '') as [joined_scheme_4]
      ,isnull([joined_scheme_5], '') as [joined_scheme_5]
      ,isnull([joined_scheme_6], '') as [joined_scheme_6]
      ,isnull([joined_scheme_7], '') as [joined_scheme_7]
      ,isnull([joined_scheme_8], '') as [joined_scheme_8]
      ,isnull([joined_scheme_9], '') as [joined_scheme_9]
      ,isnull([joined_scheme_10], '') as [joined_scheme_10]
  FROM [dbo].[SDSPPracticeFee]
  WHERE [service_category_code_SD] = @Professional
	and EXISTS (SELECT 1 FROM #DistrictList WHERE DistrictBoard = district_board_shortname_SD)
	and (([subsidize_item_1] is not null and @subsidize_item_1 = 'subsidize_item_1') OR @subsidize_item_1 IS NULL)
    and (([subsidize_item_2] is not null and @subsidize_item_2 = 'subsidize_item_2') OR @subsidize_item_2 IS NULL)
    and (([subsidize_item_3] is not null and @subsidize_item_3 = 'subsidize_item_3') OR @subsidize_item_3 IS NULL)
    and (([subsidize_item_4] is not null and @subsidize_item_4 = 'subsidize_item_4') OR @subsidize_item_4 IS NULL)
    and (([subsidize_item_5] is not null and @subsidize_item_5 = 'subsidize_item_5') OR @subsidize_item_5 IS NULL)
    and (([subsidize_item_6] is not null and @subsidize_item_6 = 'subsidize_item_6') OR @subsidize_item_6 IS NULL)
    and (([subsidize_item_7] is not null and @subsidize_item_7 = 'subsidize_item_7') OR @subsidize_item_7 IS NULL)
    and (([subsidize_item_8] is not null and @subsidize_item_8 = 'subsidize_item_8') OR @subsidize_item_8 IS NULL)
    and (([subsidize_item_9] is not null and @subsidize_item_9 = 'subsidize_item_9') OR @subsidize_item_9 IS NULL)
    and (([subsidize_item_10] is not null and @subsidize_item_10 = 'subsidize_item_10') OR @subsidize_item_10 IS NULL)
  ORDER BY [district_board_chi] ASC, [sp_chi_name] ASC
END
ELSE BEGIN
SELECT top 801 [sp_id]
      ,[practice_display_seq]
      ,[sp_name]
      ,isnull([sp_chi_name], '') as [sp_chi_name]
      ,[practice_name]
      ,isnull([practice_name_chi], '') as [practice_name_chi]
      ,isnull([phone_daytime], '') as [phone_daytime]
      ,[service_category_code_SD]
      ,[district_code]
      ,[district_board_shortname_SD]
      ,[area_code]
      ,[address_eng]
      ,[district_name]
      ,[district_board]
      ,[area_name]
      ,isnull([address_chi], '') as [address_chi]
      ,[district_name_chi]
      ,[district_board_chi]
      ,[area_name_chi]
      ,isnull([joined_scheme_1], '') as [joined_scheme_1]
      ,isnull([joined_scheme_2], '') as [joined_scheme_2]
      ,isnull([joined_scheme_3], '') as [joined_scheme_3]
      ,isnull([joined_scheme_4], '') as [joined_scheme_4]
      ,isnull([joined_scheme_5], '') as [joined_scheme_5]
      ,isnull([joined_scheme_6], '') as [joined_scheme_6]
      ,isnull([joined_scheme_7], '') as [joined_scheme_7]
      ,isnull([joined_scheme_8], '') as [joined_scheme_8]
      ,isnull([joined_scheme_9], '') as [joined_scheme_9]
      ,isnull([joined_scheme_10], '') as [joined_scheme_10]
  FROM [dbo].[SDSPPracticeFee]
  WHERE [service_category_code_SD] = @Professional
	and EXISTS (SELECT 1 FROM #DistrictList WHERE DistrictBoard = district_board_shortname_SD)
	and (([subsidize_item_1] is not null and @subsidize_item_1 = 'subsidize_item_1') OR @subsidize_item_1 IS NULL)
    and (([subsidize_item_2] is not null and @subsidize_item_2 = 'subsidize_item_2') OR @subsidize_item_2 IS NULL)
    and (([subsidize_item_3] is not null and @subsidize_item_3 = 'subsidize_item_3') OR @subsidize_item_3 IS NULL)
    and (([subsidize_item_4] is not null and @subsidize_item_4 = 'subsidize_item_4') OR @subsidize_item_4 IS NULL)
    and (([subsidize_item_5] is not null and @subsidize_item_5 = 'subsidize_item_5') OR @subsidize_item_5 IS NULL)
    and (([subsidize_item_6] is not null and @subsidize_item_6 = 'subsidize_item_6') OR @subsidize_item_6 IS NULL)
    and (([subsidize_item_7] is not null and @subsidize_item_7 = 'subsidize_item_7') OR @subsidize_item_7 IS NULL)
    and (([subsidize_item_8] is not null and @subsidize_item_8 = 'subsidize_item_8') OR @subsidize_item_8 IS NULL)
    and (([subsidize_item_9] is not null and @subsidize_item_9 = 'subsidize_item_9') OR @subsidize_item_9 IS NULL)
    and (([subsidize_item_10] is not null and @subsidize_item_10 = 'subsidize_item_10') OR @subsidize_item_10 IS NULL)
	ORDER BY [district_board_shortname_SD] ASC, [sp_name] ASC
END

--SELECT [scheme_column]
--      ,[scheme_code]
--  FROM [dbo].[SDSchemeColumnMapping]
--ORDER BY [scheme_column]

SELECT distinct ssColMap.scheme_column 
		,SDSS.scheme_code_claim as scheme_code
FROM [dbo].[SDSchemeSubsidize] SDSS, [dbo].[SDSchemeColumnMapping] ssColMap
WHERE ssColMap.scheme_code = SDSS.scheme_code_claim
and (SDSS.[Period_From] IS NULL OR DATEDIFF(Day, SDSS.[Period_From], GETDATE()) >= 0 )
and (SDSS.[Period_To] IS NULL OR DATEDIFF(Day, GETDATE(), SDSS.[Period_To]) > 0 )
ORDER BY ssColMap.scheme_column 

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_PracticeList_withoutFee] TO HCPUBLIC
GO
*/
