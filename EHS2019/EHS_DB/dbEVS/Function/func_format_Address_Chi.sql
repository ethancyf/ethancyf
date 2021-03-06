IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_format_Address_Chi]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[func_format_Address_Chi]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Struture
-- =============================================
-- =============================================
-- Author:			Tommy Lam
-- Create date:		27 Nov 2013
-- CR No.:			CRE13-008 - SP Amendment Report
-- Description:		Format Address in Chinese
-- =============================================

CREATE FUNCTION [dbo].[func_format_Address_Chi] (
	@room			nvarchar(5),
	@floor			nvarchar(3),
	@block			nvarchar(3),
	@building_chi	nvarchar(100),
	@district_code	char(5)
	)
RETURNS nvarchar(300)
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @result nvarchar(300)

	DECLARE @result_district	nvarchar(30)
	DECLARE @result_area		nvarchar(50)

-- =============================================
-- Validation
-- =============================================

	IF @room IS NULL
		SET @room = ''

	IF @floor IS NULL
		SET @floor = ''

	IF @block IS NULL
		SET @block = ''

	IF @building_chi IS NULL
		SET @building_chi = ''

	IF @district_code IS NULL
		SET @district_code = ''

	IF @building_chi = ''
		RETURN ''

-- =============================================
-- Initialization
-- =============================================

	SET @result = ''

-- =============================================
-- Process
-- =============================================

	IF @district_code <> ''
		BEGIN
			SELECT
				@result_district = ISNULL(LTRIM(RTRIM(D.district_chi)), ''),
				@result_area = ISNULL(LTRIM(RTRIM(DA.area_chi)), '')
			FROM
				district D WITH (NOLOCK)
					INNER JOIN DistrictBoard DB WITH (NOLOCK)
						ON D.District_Board = DB.District_Board
					INNER JOIN District_Area DA WITH (NOLOCK)
						ON DB.Area_Code = DA.Area_Code
			WHERE D.district_code = @district_code

			SET @result = @result + @result_area + @result_district
		END

	SET @result = @result + LTRIM(RTRIM(@building_chi))

	IF @block <> ''
		SET @result = @result + LTRIM(RTRIM(@block)) + N'座'

	IF @floor <> ''
		SET @result = @result + LTRIM(RTRIM(@floor)) + N'樓'

	IF @room <> ''
		SET @result = @result + LTRIM(RTRIM(@room)) + N'室'

-- =============================================
-- Return results
-- =============================================

	RETURN @result

END
GO

GRANT EXECUTE ON [dbo].[func_format_Address_Chi] TO HCVU
GO
