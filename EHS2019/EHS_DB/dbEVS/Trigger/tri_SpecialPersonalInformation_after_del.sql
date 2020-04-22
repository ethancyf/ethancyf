IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SpecialPersonalInformation_after_del')
	DROP TRIGGER [dbo].[tri_SpecialPersonalInformation_after_del]
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
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		Add [Deceased], [DOD], [Exact_DOD]
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
-- Description:		Trigger an insert statement into SpecialPersonalInformationLOG when a row is deleted from SpecialPersonalInformation
-- =============================================

Create TRIGGER [dbo].[tri_SpecialPersonalInformation_after_del]
   ON		[dbo].[SpecialPersonalInformation]
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
	INSERT INTO [dbo].[SpecialPersonalInformationLOG]
		([System_Dtm]
		,[Special_Acc_ID]
		,[Doc_Code]
		,[DOB]
		,[Exact_DOB]
		,[Sex]
		,[Date_of_Issue]
		,[Check_Dtm]
		,[Validating]
		,[EC_Serial_No]
		,[EC_Reference_No]
		,[EC_Age]
		,[EC_Date_of_Registration]
		,[Foreign_Passport_No]
		,[Record_Status]
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
		,[Other_Info]
		,[Permit_To_Remain_Until]
		,[EC_Reference_No_Other_Format]
		,[Deceased]
		,[DOD]
		,[Exact_DOD]
		,[SmartID_Ver]
	)
    SELECT 
		GETDATE(),
		[Special_Acc_ID],
		[Doc_Code],
		[DOB],
		[Exact_DOB],
		[Sex],
		[Date_of_Issue],
		[Check_Dtm],
		[Validating],
		[EC_Serial_No],
		[EC_Reference_No],
		[EC_Age],
		[EC_Date_of_Registration],
		[Foreign_Passport_No],
		[Record_Status],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By],
		[DataEntry_By],
		[Encrypt_Field1],
		[Encrypt_Field2],
		[Encrypt_Field3],
		[Encrypt_Field4],
		[Encrypt_Field5],
		[Encrypt_Field6],
		[Encrypt_Field7],
		[Encrypt_Field8],
		[Encrypt_Field9],
		[Encrypt_Field10],
		[Encrypt_Field11],
		[Other_Info],
		[Permit_To_Remain_Until],
		[EC_Reference_No_Other_Format],
		[Deceased],
		[DOD],
		[Exact_DOD],
		[SmartID_Ver]
	FROM
		Deleted

END
GO
