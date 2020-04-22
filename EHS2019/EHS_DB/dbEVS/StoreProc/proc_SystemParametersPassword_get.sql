IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParametersPassword_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParametersPassword_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:			Timothy LEUNG
-- Create date:		26-09-2008
-- Description:		return Password in SystemParameters
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	26-05-2009
-- Description:		Add Scheme Code: If Get By Empty Scheme Code, Retrieve 'All'
-- =============================================

CREATE  procedure [dbo].[proc_SystemParametersPassword_get]
@Parameter_Name char(50),
@Scheme_Code char(10)
AS

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

-- =============================================
-- Initialization
-- =============================================

IF LTRIM(RTRIM(@Scheme_Code)) = '' 
	SET @Scheme_Code = 'ALL'
	
-- =============================================
-- Return results
-- =============================================

SELECT Parameter_Name, CONVERT(VarChar(50), DecryptByKey(Encrypt_Field)) as ParaValue

FROM [dbo].[SystemParameters]

WHERE
	[Parameter_Name] = @Parameter_Name
	AND [Record_Status] = 'A'
	AND [External_Use] = 'N'
	AND [Scheme_Code] = @Scheme_Code
	
CLOSE SYMMETRIC KEY sym_Key	

GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_get] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_get] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_get] TO WSEXT
GO