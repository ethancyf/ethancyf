IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParametersExternal_get_byParaName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParametersExternal_get_byParaName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	07 Mar 2016
-- CR No.			CRE15-022
-- Description:		Include 'R' status for [External_Use]
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 7 Oct 2009
-- Description:	Retrieve External Use System Parameter
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE [dbo].proc_SystemParametersExternal_get_byParaName
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
		[Parm_Value1], [Parm_Value2], [Scheme_Code], [Category]
	FROM
		[dbo].[SystemParameters]
	WHERE
		[Parameter_name] = @parameter_name
		AND [Record_Status] = 'A'
		AND [External_Use] IN ('Y','R')
		AND [Scheme_Code] = @Scheme_Code
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersExternal_get_byParaName] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersExternal_get_byParaName] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersExternal_get_byParaName] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersExternal_get_byParaName] TO WSEXT
GO