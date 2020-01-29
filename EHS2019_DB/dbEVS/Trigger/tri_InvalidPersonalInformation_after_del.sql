IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_InvalidPersonalInformation_after_del')
	DROP TRIGGER [dbo].[tri_InvalidPersonalInformation_after_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2009
-- Description:		Trigger an insert statement into InvalidPersonalInformationLOG when a row is deleted from InvalidPersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	5 Oct 2009
-- Description:		Update the table schema
-- =============================================

Create TRIGGER [dbo].[tri_InvalidPersonalInformation_after_del]
   ON		[dbo].[InvalidPersonalInformation]
   AFTER	DELETE
AS BEGIN

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
-- =============================================
-- Return results
-- =============================================
	INSERT INTO [dbo].[InvalidPersonalInformationLOG]
		([System_Dtm]
		,[Invalid_Acc_ID]
		,[Doc_Code]
		,[DOB]
		,[Exact_DOB]
		,[Sex]
		,[Date_of_Issue]
		,[Create_By_SmartID]
		,[EC_Serial_No]
		,[EC_Reference_No]
		,[EC_Age]
		,[EC_Date_of_Registration]
		,[Foreign_Passport_No]
		,[Permit_To_Remain_Until]
		,[Record_Status]
		,[Other_Info]
		,[Create_Dtm]
		,[Create_By]
		,[Update_Dtm]
		,[Update_By]
		,[DataEntry_By]
		,[Encrypt_Field1]
		,[Encrypt_Field2]
		,[Encrypt_Field3]
		,[Encrypt_Field4]
		,[Encrypt_Field5]
		,[Encrypt_Field6]
		,[Encrypt_Field7]
		,[Encrypt_Field8]
		,[Encrypt_Field9]
		,[Encrypt_Field10]
		,[Encrypt_Field11]
		,[EC_Reference_No_Other_Format]
		,[SmartID_Ver]
	)
    SELECT 
	   GETDATE()
	  ,[Invalid_Acc_ID]
      ,[Doc_Code]
      ,[DOB]
      ,[Exact_DOB]
      ,[Sex]
      ,[Date_of_Issue]
      ,[Create_By_SmartID]
      ,[EC_Serial_No]
      ,[EC_Reference_No]
      ,[EC_Age]
      ,[EC_Date_of_Registration]
      ,[Foreign_Passport_No]
      ,[Permit_To_Remain_Until]
      ,[Record_Status]
      ,[Other_Info]
      ,[Create_Dtm]
      ,[Create_By]
      ,[Update_Dtm]
      ,[Update_By]
      ,[DataEntry_By]
      ,[Encrypt_Field1]
      ,[Encrypt_Field2]
      ,[Encrypt_Field3]
      ,[Encrypt_Field4]
      ,[Encrypt_Field5]
      ,[Encrypt_Field6]
      ,[Encrypt_Field7]
      ,[Encrypt_Field8]
      ,[Encrypt_Field9]
      ,[Encrypt_Field10]
      ,[Encrypt_Field11]
      ,[EC_Reference_No_Other_Format]
	  ,[SmartID_Ver]
	FROM
		Deleted

END
GO
