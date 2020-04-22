IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_upd_ParameterValue]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_upd_ParameterValue]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Stanley Chan>
-- Create date: <10 Sep 2008>
-- Description:	<update System Parameter Value>
-- =============================================
CREATE PROCEDURE [dbo].[proc_SystemParameters_upd_ParameterValue]
	-- Add the parameters for the stored procedure here
	@Parameter_ID char(50), 
	@Category varchar(20), 
	@Parameter_Value nvarchar(255),
	@Update_By varchar(20),
	@tsmp timestamp
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if (select tsmp from SystemParameters
		where Parameter_Name = @Parameter_ID and
			  Category = @Category) != @tsmp
begin
		RAISERROR('00011', 16, 1)
		return @@error
end

    -- Insert statements for procedure here
	UPDATE SystemParameters
	set
	[Parm_Value1] = @Parameter_Value,
	[Update_By] = @Update_By,
	[Update_dtm] = getdate()
	where 
	[Parameter_Name] = @Parameter_ID and 
	[Category] = @Category
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_upd_ParameterValue] TO HCVU
GO
