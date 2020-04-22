IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Philip Chau
-- Create date: 29 May 2015
-- Description:	Retrieve the Profile_Prefix 
-- according to the profile_ID and scheme_Code
-- =============================================

CREATE PROCEDURE dbo.proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode
	@Profile_ID char(10),
	@Scheme_Code char(10)
AS
BEGIN

-- =============================================
-- Return results
-- =============================================
	SELECT TOP 1 
			ISNULL([Profile_Prefix], '') as Profile_Prefix
		FROM [dbo].[SystemProfile] WITH (NOLOCK)
		WHERE
			[Profile_ID] = @Profile_ID AND [Scheme_Code] = @Scheme_Code
		ORDER BY Profile_Year DESC
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemProfile_ProfilePrefix_get_byProfileIDSchemeCode] TO WSEXT
GO

