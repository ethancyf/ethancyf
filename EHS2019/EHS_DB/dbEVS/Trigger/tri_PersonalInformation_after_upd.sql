IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_PersonalInformation_after_upd')
	DROP TRIGGER [dbo].[tri_PersonalInformation_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (COVID19)
-- Modified by:		Winnie SUEN
-- Modified date:	27 May 2021
-- Description:		Add [PASS_Issue_Region]
-- =============================================
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
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	09 Feb 2010
-- Description:		Add 'Verify_By_SmartID'
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 21 August 2008
-- Description:	Trigger an insert statment into PersonalInformationLOG
--				when a row is updated / inserted into PersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	18 August 2009
-- Description:		Add Doc_Code, Foreign_Passport_No & Permit_To_Remain_Until
-- =============================================
Create TRIGGER [dbo].[tri_PersonalInformation_after_upd]
   ON  [dbo].[PersonalInformation]
   AFTER INSERT, UPDATE
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO PersonalInformationLOG
		(System_Dtm,
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Create_By_SmartID,
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
		Verify_By_SmartID,
		EC_Reference_No_Other_Format,
		Deceased,
		DOD,
		Exact_DOD,
		SmartID_Ver,
	  	[PASS_Issue_Region]	
		)
	SELECT getdate(),
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Create_By_SmartID,
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
		Verify_By_SmartID,
		EC_Reference_No_Other_Format,
		Deceased,
		DOD,
		Exact_DOD,
		SmartID_Ver,
	  	[PASS_Issue_Region]	
	FROM inserted

END
GO
