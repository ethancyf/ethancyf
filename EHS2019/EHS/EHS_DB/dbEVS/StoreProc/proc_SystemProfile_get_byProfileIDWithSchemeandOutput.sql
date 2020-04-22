IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemProfile_get_byProfileIDWithSchemeandOutput]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemProfile_get_byProfileIDWithSchemeandOutput]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 20 Sep 2009
-- Description:	Retrieve the sequence number from 
--              database according to different
--				type of number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

create PROCEDURE [dbo].[proc_SystemProfile_get_byProfileIDWithSchemeandOutput]
	@Profile_ID char(10),
	@Scheme_Code char(10),
	@Seq char(10) output,
	@Prefix varchar(5) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	declare @Profile_Type as char(1)
	declare @Profile_Year as smallint
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
	ORDER BY Profile_Year DESC
	
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
	
	-- 3. Profile Type = 'N'
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
		SELECT	@Seq = [Profile_Num], @prefix = ISNULL([Profile_Prefix], '') 
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code
		
	end
	else if (@Profile_Type = 'Y' )
	begin
	
		SELECT @Profile_Year = datepart("yyyy", getdate())
	  
		SELECT @pid = isnull(Profile_ID,'')
		FROM [dbo].[SystemProfile]
		WHERE [Profile_ID] = @Profile_ID AND [Profile_Year] = @Profile_Year AND [Scheme_Code] = @Scheme_Code

		-- Auto Insert Year Entry
		if (@pid = '' or @pid is null) 
		BEGIN
			INSERT INTO [dbo].[SystemProfile]
			(
				[Profile_ID], [Profile_Year], [Profile_Type], [Profile_Num], [Description], [Last_reset_dtm], [reset_offset], [Scheme_Code], [Profile_Prefix]
			)
			SELECT TOP 1
				[Profile_ID], @Profile_Year, [Profile_Type], ISNULL([reset_offset],0), [Description], getdate(), [reset_offset], [Scheme_Code], [Profile_Prefix]
			FROM	[dbo].[SystemProfile]
			WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code --[Profile_year] = @Profile_Year - 1 
			ORDER BY [Profile_Year] DESC
		END
	  
		-- UPDATE Profile_Num + 1
		UPDATE	[dbo].[SystemProfile]
		SET		[Profile_Num] = Profile_Num + 1
		WHERE	[Profile_ID] = @Profile_ID AND [Profile_Year] = @Profile_Year AND [Scheme_Code] = @Scheme_Code

		-- SELECT Profile_Num & Profile_Prefix
		SELECT	@Seq = [Profile_Num], @prefix = ISNULL([Profile_Prefix], '') 
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID and [Profile_Year] = @Profile_Year AND [Scheme_Code] = @Scheme_Code
	end
	else if (@Profile_Type = 'N')
	begin
	
		-- UPDATE Profile_Num + 1
		UPDATE	[dbo].[SystemProfile]
		SET		[Profile_Num] = Profile_Num + 1
		WHERE	[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code

		-- SELECT Profile_Num & Profile_Prefix
		SELECT	@Seq = [Profile_Num], @prefix = ISNULL([Profile_Prefix], '') 
		FROM	[dbo].[SystemProfile]
		WHERE	[Profile_ID] = @Profile_ID
	end 
END
GO
