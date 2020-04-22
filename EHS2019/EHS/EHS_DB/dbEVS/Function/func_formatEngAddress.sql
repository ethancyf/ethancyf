IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[func_formatEngAddress]'))
	DROP FUNCTION [dbo].[func_formatEngAddress]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Struture
-- =============================================
-- =============================================
-- Author:			Karl Lam
-- Create date:	18 Jun 2013
-- Description:	Return formatted address
-- =============================================

CREATE FUNCTION [dbo].[func_formatEngAddress]
(
	@Room		nvarchar(5),
	@Floor		nvarchar(3),
	@Block		nvarchar(3),
	@Building	varchar(100),
	@District	char(4)

)
RETURNS nvarchar(300)
AS
BEGIN
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @DistrictName char(15)
	DECLARE @AreaName char(50)
	DECLARE @Result nVarchar(300)
	
	SET @Result = ''
	-- =============================================
	-- Process
	-- =============================================
	IF rtrim(ltrim(isnull(@Room,''))) <> ''
	SELECT @Result = @Result + 'ROOM ' + rtrim(ltrim(isnull(@Room,''))) + ', '
	
	IF rtrim(ltrim(isnull(@Floor,''))) <> ''
	SELECT @Result = @Result + 'FLOOR ' + rtrim(ltrim(isnull(@Floor,''))) + ', '
	
	IF rtrim(ltrim(isnull(@Block,''))) <> ''
	SELECT @Result = @Result + 'BLOCK ' + rtrim(ltrim(isnull(@Block,''))) + ', '
	
	IF rtrim(ltrim(isnull(@Building,''))) <> ''
	SELECT @Result = @Result + rtrim(ltrim(isnull(@Building,'')))
	
	
	if rtrim(ltrim(isnull(@District,''))) <> ''
	BEGIN
		SELECT	@DistrictName = D.district_name,
				@AreaName = DA.area_name
		FROM District D 
		INNER JOIN DistrictBoard DB on D.District_Board = DB.District_Board
		INNER JOIN District_Area DA on DB.Area_Code = DA.Area_Code   
		WHERE D.district_code = @District
		
		
		IF rtrim(ltrim(isnull(@DistrictName,''))) <> ''
		SELECT @Result = @Result + ', ' + rtrim(ltrim(@DistrictName)) + ', ' + rtrim(ltrim(@AreaName))

	END
	-- =============================================
	-- Return results
	-- =============================================
	RETURN @Result

END
GO

Grant execute on [dbo].[func_formatEngAddress] to HCVU
GO
