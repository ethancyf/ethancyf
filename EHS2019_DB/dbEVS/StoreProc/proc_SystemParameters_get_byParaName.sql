IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_get_byParaName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_get_byParaName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Stanely CHAN
-- Create date: 25 April 2008
-- Description:	Common procedure to retrieve the
--				System Parameter by parameter_name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	26-05-2009
-- Description:		Add Scheme Code: If Get By Empty Scheme Code, Retrieve 'All'
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemParameters_get_byParaName]
	@parameter_name as char(50),
	@Scheme_Code as char(10)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

IF LTRIM(RTRIM(@Scheme_Code)) = '' 
	SET @Scheme_Code = 'ALL'

-- =============================================
-- Return results
-- =============================================

    SELECT 
		[Parm_Value1], [Parm_Value2], [Scheme_Code]
	FROM
		[dbo].[SystemParameters]
	WHERE
		[Parameter_name] = @parameter_name
		AND [Record_Status] = 'A'
		AND [External_Use] = 'N'
		AND [Scheme_Code] = @Scheme_Code
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_byParaName] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_byParaName] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_byParaName] TO HCVU
GO
