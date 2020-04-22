IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemProfile_get_byProfileID_withOutputParm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemProfile_get_byProfileID_withOutputParm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Tommy Cheung
-- Create date: 08 Oct 2008
-- Description:	Retrieve the sequence number from 
--              database according to different
--				type of number
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	10 Oct 2009
-- Description:		Add [Scheme_Code] = 'ALL'
-- =============================================
CREATE PROCEDURE [dbo].[proc_SystemProfile_get_byProfileID_withOutputParm]
	@Profile_ID char(10),
	@Message_ID char(12) output
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
-- =============================================
-- Validation 
-- =============================================
    select @Profile_Type = Profile_Type, @Last_Reset_Dtm = convert(varchar(8), Last_Reset_Dtm, 112) 
	from dbo.SystemProfile
	where Profile_ID = @Profile_ID AND [Scheme_Code] = 'ALL'
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	If (@Profile_Type = 'D' )
	begin
		if (datediff("d",@Last_Reset_dtm, getdate()) > 0 )
		begin 
			update dbo.SystemProfile
			set Profile_Num = isNull(Reset_Offset,0), Last_Reset_Dtm = getdate()
			where Profile_Type = 'D' and 
			Profile_ID = @Profile_ID and 
			datediff("d",convert(varchar(8),Last_Reset_Dtm, 112), getdate()) > 0 AND
			[Scheme_Code] = 'ALL'	
		end

		update dbo.SystemProfile 
		set Profile_Num = Profile_Num + 1
		where Profile_ID = @Profile_ID AND [Scheme_Code] = 'ALL'

		select @Message_ID=Profile_Num
		from dbo.SystemProfile
		where Profile_ID = @Profile_ID AND [Scheme_Code] = 'ALL'
	end
	else if (@Profile_Type = 'Y' )
	begin
	  select @Profile_Year = datepart("yyyy", getdate())
	  
  	  select @pid = isnull(Profile_ID,'')
	  from systemprofile
	  where Profile_ID = @Profile_ID and Profile_year = @Profile_Year AND [Scheme_Code] = 'ALL'

	  if (@pid = '' or @pid is null) 
	  begin
		insert into SystemProfile 
		(profile_id, profile_year, profile_type, profile_num, description, last_reset_dtm, reset_offset)
        select profile_id, @profile_year, profile_type, isnull(reset_offset,0), description, getdate(), reset_offset
		from systemprofile
		where Profile_ID = @Profile_ID and Profile_year = @Profile_Year - 1 AND [Scheme_Code] = 'ALL'
	  end 

	  
	  update dbo.SystemProfile 
	  set Profile_Num = Profile_Num + 1
	  where Profile_ID = @Profile_ID and Profile_year = @Profile_Year AND [Scheme_Code] = 'ALL'

	  select @Message_ID=Profile_Num
	  from dbo.SystemProfile
	  where Profile_ID = @Profile_ID and Profile_year = @Profile_Year AND [Scheme_Code] = 'ALL'
	end
	else if (@Profile_Type = 'N')
	begin
	  update dbo.SystemProfile 
	  set Profile_Num = Profile_Num + 1
	  where Profile_ID = @Profile_ID AND [Scheme_Code] = 'ALL'

	  select @Message_ID=Profile_Num
	  from dbo.SystemProfile
	  where Profile_ID = @Profile_ID AND [Scheme_Code] = 'ALL'
	end 

END
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_get_byProfileID_withOutputParm] TO HCVU
GO
