IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemProfile_get_byProfileID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemProfile_get_byProfileID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			CRE19-022
-- Modified by:		Winnie SUEN
-- Modified date:	13 Jul 2020
-- Description:		1. Add handling on @Profile_Type = 'M' (Month)
--					2. User can define the specify year and month
--					3. Add colummn [Profile_Month] for SystemProfile table
-- =============================================  
-- =============================================
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 28 April 2008
-- Description:	Retrieve the sequence number from 
--              database according to different
--				type of number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Profile Add Scheme Code & Profile Prefix
--					Return Profile Prefix & Profile Num
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemProfile_get_byProfileID]
	@Profile_ID char(10),
	@Scheme_Code char(10),
	@Profile_Year SMALLINT = NULL,
	@Profile_Month SMALLINT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	declare @Profile_Type as char(1)
	declare @Last_Reset_Dtm as datetime
	declare @pid as char(10)
	
	IF LTRIM(RTRIM(@Scheme_Code)) = '' 
	SET @Scheme_Code = 'ALL'
	
-- =============================================
-- Validation 
-- =============================================
	
    SELECT TOP 1 
		@Profile_Type = Profile_Type, @Last_Reset_Dtm = convert(varchar(8), Last_Reset_Dtm, 112) 
	FROM [dbo].[SystemProfile]
	WHERE
		[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code
	ORDER BY Profile_Year DESC, Profile_Month DESC
	
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	-- 1. Profile Type = 'D'
	--		The Profile number generated will be reset per day
	
	-- 2. Profile Type = 'Y'
	--		The Profile number generated will be reset per year
	--		Each Year, will contain one entry, and	
	--		year entry will be auto inserted when enter new year
	
	-- 3. Profile Type = 'M'
	--		The Profile number generated will be reset per year and month
	--		Each Year and Month, will contain one entry, and	
	--		month entry will be auto inserted when enter new year and month

	-- 4. Profile Type = 'N'
	--		The Profile number generated will not be reset

	If (@Profile_Type = 'D' )
	begin
		if (datediff("d",@Last_Reset_dtm, getdate()) > 0 )
		BEGIN
			UPDATE	[dbo].[SystemProfile]
			SET		[Profile_Num] = isNull(Reset_Offset,0), Last_Reset_Dtm = getdate()
			WHERE	[Profile_Type] = 'D' and [Profile_ID] = @Profile_ID and 
					[Scheme_Code] = @Scheme_Code and
					datediff("d",convert(varchar(8),Last_Reset_Dtm, 112), getdate()) > 0
		END
		
		-- UPDATE Profile_Num + 1
		UPDATE	[dbo].[SystemProfile]
		SET		[Profile_Num] = Profile_Num + 1
		WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code

		-- SELECT Profile_Num & Profile_Prefix
		SELECT	[Profile_Num], ISNULL([Profile_Prefix], '') as Profile_Prefix
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code
		
	end
	else if (@Profile_Type = 'Y' )
	begin
	
		IF @Profile_Year IS NULL 
			SET @Profile_Year = datepart("yyyy", getdate())

		SELECT @pid = isnull(Profile_ID,'')
		FROM [dbo].[SystemProfile]
		WHERE [Profile_ID] = @Profile_ID AND [Profile_Year] = @Profile_Year AND [Scheme_Code] = @Scheme_Code

		-- Auto Insert Year Entry
		if (@pid = '' or @pid is null) 
		BEGIN
			INSERT INTO [dbo].[SystemProfile]
			(
				[Profile_ID], [Profile_Year], [Profile_Month], [Profile_Type], [Profile_Num], [Description], [Last_reset_dtm], [reset_offset], [Scheme_Code], [Profile_Prefix]
			)
			SELECT TOP 1
				[Profile_ID], @Profile_Year, 0, [Profile_Type], ISNULL([reset_offset],0), [Description], getdate(), [reset_offset], [Scheme_Code], [Profile_Prefix]
			FROM	[dbo].[SystemProfile]
			WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code --[Profile_year] = @Profile_Year - 1 
			ORDER BY [Profile_Year] DESC
		END
	  
		-- UPDATE Profile_Num + 1
		UPDATE	[dbo].[SystemProfile]
		SET		[Profile_Num] = Profile_Num + 1
		WHERE	[Profile_ID] = @Profile_ID AND [Profile_Year] = @Profile_Year AND [Scheme_Code] = @Scheme_Code

		-- SELECT Profile_Num & Profile_Prefix
		SELECT	[Profile_Num], ISNULL([Profile_Prefix], '') as Profile_Prefix
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID and [Profile_Year] = @Profile_Year AND [Scheme_Code] = @Scheme_Code
	end

	else if (@Profile_Type = 'M' )  -- Month
	begin
	
		IF @Profile_Year IS NULL 
			SET @Profile_Year = datepart("yyyy", getdate())

		IF @Profile_Month IS NULL 
			SET @Profile_Month = datepart(MONTH, getdate())

		SELECT @pid = isnull(Profile_ID,'')
		FROM [dbo].[SystemProfile]
		WHERE [Profile_ID] = @Profile_ID 
			AND [Profile_Year] = @Profile_Year AND [Profile_Month] = @Profile_Month
			AND [Scheme_Code] = @Scheme_Code

		-- Auto Insert Year and Month Entry
		if (@pid = '' or @pid is null) 
		BEGIN
			INSERT INTO [dbo].[SystemProfile]
			(
				[Profile_ID], [Profile_Year], [Profile_Month], [Profile_Type], [Profile_Num], [Description], [Last_reset_dtm], [reset_offset], [Scheme_Code], [Profile_Prefix]
			)
			SELECT TOP 1
				[Profile_ID], @Profile_Year, @Profile_Month, [Profile_Type], ISNULL([reset_offset],0), [Description], getdate(), [reset_offset], [Scheme_Code], [Profile_Prefix]
			FROM	[dbo].[SystemProfile]
			WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code --[Profile_year] = @Profile_Year - 1 
			ORDER BY Profile_Year DESC, Profile_Month DESC
		END
	  
		-- UPDATE Profile_Num + 1
		UPDATE	[dbo].[SystemProfile]
		SET		[Profile_Num] = Profile_Num + 1
		WHERE	[Profile_ID] = @Profile_ID AND [Profile_Year] = @Profile_Year AND [Profile_Month] = @Profile_Month AND [Scheme_Code] = @Scheme_Code

		-- SELECT Profile_Num & Profile_Prefix
		SELECT	[Profile_Num], ISNULL([Profile_Prefix], '') as Profile_Prefix
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID and [Profile_Year] = @Profile_Year AND [Profile_Month] = @Profile_Month AND [Scheme_Code] = @Scheme_Code
	end

	else if (@Profile_Type = 'N')
	begin
	
		-- UPDATE Profile_Num + 1
		UPDATE	[dbo].[SystemProfile]
		SET		[Profile_Num] = Profile_Num + 1
		WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code

		-- SELECT Profile_Num & Profile_Prefix
		SELECT	[Profile_Num], ISNULL([Profile_Prefix], '') as Profile_Prefix
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID
	end 
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_get_byProfileID] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_get_byProfileID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_get_byProfileID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_get_byProfileID] TO WSEXT
GO
