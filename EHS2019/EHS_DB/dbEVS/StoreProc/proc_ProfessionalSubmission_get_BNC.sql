IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmission_get_BNC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmission_get_BNC]
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
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 September 2010
-- Description:		Add a single quote (') before the Registration_Code to avoid converting to integer while
--					writing to Excel
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 02 July 2008
-- Description:	Retrieve Professional Submission For BNC
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalSubmission_get_BNC]
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
		Reference_No as [Record Reference No.],
		--SP_HKID as [HKIC No.],
		convert(char,DecryptByKey([Encrypt_Field1])) as [HKIC No.] ,
		'''' + Registration_Code as [Professional Registration No.],
		--Surname as [Surname], 
		convert(varchar(40),DecryptByKey([Encrypt_Field10])) as [Surname],		
		--Other_Name as [Other Name]
		convert(varchar(40),DecryptByKey([Encrypt_Field11])) as [Other Name]

	FROM [dbo].[ProfessionalSubmission]
	
	WHERE
		[File_Name] = @File_Name
	
	ORDER BY Display_Seq ASC
	
	EXEC [proc_SymmetricKey_close]
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmission_get_BNC] TO HCVU
GO
