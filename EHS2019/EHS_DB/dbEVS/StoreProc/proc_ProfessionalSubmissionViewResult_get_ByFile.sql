IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionViewResult_get_ByFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionViewResult_get_ByFile]
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
-- CR No:			INT14-0023
-- Modified by:		Karl LAM  
-- Modified date:	07 Nov 2014  
-- Description:		Fix cannot join PVL.RecordStatus
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:		Karl LAM  
-- Modified date:	21 Oct 2013  
-- Description:		Change File Name to Varchar(50)  
-- =============================================  
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 17 Jun 2008
-- Description:	Retrieve Professional Submission & Result
--		For View Result
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionViewResult_get_ByFile]
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

--Reference No = Enrolment_Ref_No + Professional_Seq (5 Digit)

SELECT 
	PS.File_Name, PS.Reference_No, PS.Display_Seq, PS.Registration_Code,
	--PS.SP_HKID, PS.Surname, PS.Other_Name,
	convert(char,DecryptByKey(PS.[Encrypt_Field1])) as SP_HKID ,
	convert(varchar(40),DecryptByKey(PS.[Encrypt_Field10])) as Surname ,
	convert(varchar(40),DecryptByKey(PS.[Encrypt_Field11])) as Other_Name,
		
	PSR.Result, PSR.Remark, PVL.Record_Status
	
FROM
	[dbo].[ProfessionalSubmission] PS 
		LEFT OUTER JOIN [dbo].[ProfessionalSubmissionResult] PSR
			ON PS.File_Name = PSR.File_Name And PS.Reference_No = PSR.Reference_No
		LEFT OUTER JOIN [dbo].[ProfessionalVerificationLOG] PVL
			ON PS.Reference_No = PVL.Enrolment_Ref_No + RIGHT('00000' + CONVERT(Varchar, PVL.Professional_Seq), 5) AND
			PVL.System_Dtm = (Select Max (System_Dtm) From [dbo].[ProfessionalVerificationLOG] where PS.Reference_No = Enrolment_Ref_No + RIGHT('00000' + CONVERT(Varchar, Professional_Seq), 5))

WHERE
	PS.File_Name = @File_Name

ORDER BY PS.Display_Seq ASC

EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionViewResult_get_ByFile] TO HCVU
GO
