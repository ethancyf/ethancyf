IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmission_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmission_get]
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
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:  Karl LAM  
-- Modified date: 21 Oct 2013  
-- Description:  Change File Name to Varchar(50)  
-- =============================================  
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 29 May 2008
-- Description:	Retrieve Professional Submission Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalSubmission_get]
	@File_Name varchar(50)
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

EXEC [proc_SymmetricKey_open]
	
-- =============================================
-- Return results
-- =============================================

	SELECT 
		[File_Name], [Reference_No], [Display_Seq], [Registration_Code],
		--SP_HKID, Surname, Other_Name
		convert(char,DecryptByKey([Encrypt_Field1])) as SP_HKID ,
		convert(varchar(40),DecryptByKey([Encrypt_Field10])) as Surname ,
		convert(varchar(40),DecryptByKey([Encrypt_Field11])) as Other_Name
		
	FROM [dbo].[ProfessionalSubmission]
	
	WHERE 
		[File_Name] = @File_Name
	
	ORDER BY Display_Seq ASC
	
	EXEC [proc_SymmetricKey_close]
	
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmission_get] TO HCVU
GO
