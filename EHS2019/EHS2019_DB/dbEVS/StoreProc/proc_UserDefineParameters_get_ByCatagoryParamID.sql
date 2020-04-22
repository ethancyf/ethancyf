IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_UserDefineParameters_get_ByCatagoryParamID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_UserDefineParameters_get_ByCatagoryParamID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Stanley Chan
-- Create date: 10 Sep 2008
-- Description:	Retrieve User Define Parameter by specific Category and Parameter ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

CREATE PROCEDURE proc_UserDefineParameters_get_ByCatagoryParamID
	-- Add the parameters for the stored procedure here
	@Category varchar(20),
	@Parameter_ID varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Parm_Value1]
	FROM 
		[dbo].[SystemParameters]
	WHERE 
		UPPER(Category) = UPPER(@Category) 
		AND UPPER(Parameter_Name) = UPPER(@Parameter_ID)
		AND [Record_Status] = 'A'
		AND [External_Use] = 'Y'
		AND [Scheme_Code] = 'ALL'
END
GO

GRANT EXECUTE ON [dbo].[proc_UserDefineParameters_get_ByCatagoryParamID] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_UserDefineParameters_get_ByCatagoryParamID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_UserDefineParameters_get_ByCatagoryParamID] TO HCVU
GO
