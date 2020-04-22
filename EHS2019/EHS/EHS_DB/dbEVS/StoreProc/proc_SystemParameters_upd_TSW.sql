IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_upd_TSW]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_upd_TSW]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Clark Yip
-- Create date:		3 Oct 2008
-- Description:		Update System Paramenters for TSW
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SystemParameters_upd_TSW]
	-- Add the parameters for the stored procedure here
	@Parameter_ID char(50), 	
	@Parameter_Value nvarchar(255)	
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE SystemParameters
	set
	[Parm_Value1] = @Parameter_Value	
	where 
	[Parameter_Name] = @Parameter_ID 	
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_upd_TSW] TO HCVU
GO
