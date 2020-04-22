IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_get_bySchemeCodeExternalUseLikeParaName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_get_bySchemeCodeExternalUseLikeParaName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Karl LAM
-- Create date: 19 Sep 2013
-- Description:	Common procedure to retrieve the
--				System Parameter by parameter_name in like clause
-- =============================================


CREATE PROCEDURE [dbo].[proc_SystemParameters_get_bySchemeCodeExternalUseLikeParaName]
	@Parameter_Name as char(50),
	@External_Use char(1),
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
		[Parameter_name] like @Parameter_Name
		AND [Record_Status] = 'A'
		AND [External_Use] = @External_Use
		AND [Scheme_Code] = @Scheme_Code
	ORDER BY [Parameter_name] ASC
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_bySchemeCodeExternalUseLikeParaName] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_bySchemeCodeExternalUseLikeParaName] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_bySchemeCodeExternalUseLikeParaName] TO HCVU
GO
