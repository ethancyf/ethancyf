IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_TempPersonalInformation_after_del')
	DROP TRIGGER [dbo].[tri_TempPersonalInformation_after_del]
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
-- Author:		Tommy Cheung
-- Create date: 21 August 2008
-- Description:	Trigger an insert statment into TempPersonalInformationLOG
--				when a row is deleted from TempPersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	18 August 2009
-- Description:		Add Doc_Code, Foreign_Passport_No & Permit_To_Remain_Until
-- =============================================
Create TRIGGER [dbo].[tri_TempPersonalInformation_after_del]
   ON  [dbo].[TempPersonalInformation]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO TempPersonalInformationLOG
		(System_Dtm,
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Check_Dtm,
		Validating,
		Record_Status,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field10,
		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		EC_Reference_No_Other_Format,
		Deceased,
		DOD,
		Exact_DOD,
		SmartID_Ver
		)
	SELECT getdate(),
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Check_Dtm,
		Validating,
		Record_Status,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		DataEntry_By,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		Encrypt_Field10,
		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		EC_Reference_No_Other_Format,
		Deceased,
		DOD,
		Exact_DOD,
		SmartID_Ver
	FROM deleted


END
GO
