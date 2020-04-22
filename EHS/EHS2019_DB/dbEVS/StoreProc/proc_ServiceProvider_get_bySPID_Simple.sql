IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_bySPID_Simple]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_bySPID_Simple]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Jun 2017
-- Description:		Add field "Activation_Code_Level"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	16 May 2017
-- CR No.			INT17-0007
-- Description:		Change PPI-ePR to eHR
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 June 2016
-- CR No.			CRE15-016
-- Description:		Retrieve Service Provider informaion by SPID in a simple way (no address search)
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_bySPID_Simple]
	@SP_ID	char(8)
AS BEGIN

	SET NOCOUNT ON;

	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT 
		SP_ID,
		Enrolment_Ref_No,
		Room,
		Floor,
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Phone_Daytime,
		Fax,
		Email,
		Tentative_Email,
		Activation_Code,
		Record_Status,
		Remark,
		Submission_Method,
		Already_Joined_EHR,
		Join_EHR,
		UnderModification,
		Enrolment_Dtm,
		Application_Printed,
		Effective_Dtm,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS [SP_HKID], 
		CONVERT(varchar(40), DecryptByKey(Encrypt_Field2)) AS [SP_Eng_Name], 
		CONVERT(nvarchar, DecryptByKey(Encrypt_Field3)) AS [SP_Chi_Name],
		TSMP,
		Token_Return_dtm,
		Tentative_Email_Input_By,
		Data_Input_By,
		Data_Input_Effective_Dtm,
		Activation_Code_Level
	FROM
		ServiceProvider
	WHERE
		SP_ID = @SP_ID

	CLOSE SYMMETRIC KEY sym_Key


END
GO


GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_bySPID_Simple] TO HCVU
GO
