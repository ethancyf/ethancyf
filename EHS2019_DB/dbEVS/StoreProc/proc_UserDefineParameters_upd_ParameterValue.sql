IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_UserDefineParameters_upd_ParameterValue]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_UserDefineParameters_upd_ParameterValue]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Stanley Chan>
-- Create date: <17/6/2008>
-- Description:	<update Parameter Value>
-- =============================================
CREATE PROCEDURE [dbo].[proc_UserDefineParameters_upd_ParameterValue]
	-- Add the parameters for the stored procedure here
	@Parameter_ID varchar(50), 
	@Category varchar(20), 
	@Parameter_Value nvarchar(255),
	@Update_By char(20),
	@tsmp timestamp
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if (select tsmp from UserDefineParameters
		where Parameter_ID = @Parameter_ID and
			  Category = @Category) != @tsmp
begin
		RAISERROR('00011', 16, 1)
		return @@error
end

    -- Insert statements for procedure here
	UPDATE UserDefineParameters
	set
	[Parameter_Value] = @Parameter_Value,
	[Update_By] = @Update_By,
	[Update_dtm] = getdate()
	where 
	[Parameter_ID] = @Parameter_ID and 
	[Category] = @Category
END
GO

GRANT EXECUTE ON [dbo].[proc_UserDefineParameters_upd_ParameterValue] TO HCVU
GO
