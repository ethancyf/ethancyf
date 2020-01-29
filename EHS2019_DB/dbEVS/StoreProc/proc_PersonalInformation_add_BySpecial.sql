   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_PersonalInformation_add_BySpecial]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_PersonalInformation_add_BySpecial]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

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
-- Author:		Dedrick Ng
-- Create date: 1 Feb 2010
-- Description:	Insert PersonalInformation By SpecialPersonalInformation
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformation_add_BySpecial]
	@Voucher_Acc_ID	char(15),
	@Special_Acc_ID	char(15)
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
-- =============================================
-- Return results
-- =============================================

	INSERT INTO [dbo].[PersonalInformation]
	(
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		--HKID_Card,
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
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		
		Encrypt_Field10,

		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		Deceased, 
		DOD, 
		Exact_DOD,
		SmartID_Ver
	)
	SELECT 
		@Voucher_Acc_ID,
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
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		
		Encrypt_Field10,

		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		Deceased, 
		DOD, 
		Exact_DOD,
		SmartID_Ver

	FROM [dbo].[SpecialPersonalInformation]
	WHERE 
		Special_Acc_ID = @Special_Acc_ID

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_PersonalInformation_add_BySpecial] to HCVU
GO       