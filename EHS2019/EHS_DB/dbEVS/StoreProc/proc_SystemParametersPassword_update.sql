IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParametersPassword_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParametersPassword_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 Aug 2014
-- CR No.:			CRE13-029
-- Description:		RSA Server Upgrade to 7.1 (Interface Control)
-- =============================================
-- =============================================
-- Author:			Timothy LEUNG
-- Create date:		26-09-2008
-- Description:		Update Password in SystemParameters
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	26-05-2009
-- Description:		Add Scheme Code: If Get By Empty Scheme Code, Retrieve 'All'
-- =============================================

CREATE  procedure [dbo].[proc_SystemParametersPassword_update]
@Parameter_Name char(50),
@Scheme_Code char(10),
@Password varchar(50),
@Update_By varchar(20)
AS

EXEC [proc_SymmetricKey_open]

-- =============================================
-- Initialization
-- =============================================

IF LTRIM(RTRIM(@Scheme_Code)) = '' 
	SET @Scheme_Code = 'ALL'

	
-- =============================================
-- Return results
-- =============================================

UPDATE
	[dbo].[SystemParameters]

SET 
	[Encrypt_Field] = EncryptByKey(KEY_GUID('sym_Key'), @Password),
	[Update_Dtm] = GETDATE(),
	[Update_By] = @Update_By
	
WHERE
	[Parameter_name] = @Parameter_Name
	AND [Scheme_Code] = @Scheme_Code

EXEC [proc_SymmetricKey_close]
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_update] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_update] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemParametersPassword_update] TO HCVU
GO
