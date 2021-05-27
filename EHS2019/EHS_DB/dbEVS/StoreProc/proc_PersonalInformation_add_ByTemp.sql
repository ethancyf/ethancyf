IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformation_add_ByTemp]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformation_add_ByTemp]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (COVID19)
-- Modified by:		Winnie SUEN
-- Modified date:	26 May 2021
-- Description:		Update PersonalInfo.[Update_Dtm] when merge temp account
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
-- Modified date:	23 Mar 2010
-- Description:		Insert the field 'Create_By_SmartID'
--					from TempPersonalInformation to PersonalInformation
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Insert PersonalInformation By TempPersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 8 Oct 2009
-- Description:	Add new fields and remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformation_add_ByTemp]
	@Voucher_Acc_ID	char(15),
	@Temp_Voucher_Acc_ID	char(15)
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
		EC_Reference_No_Other_Format,
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
		--HKID_Card,
		Create_By_SmartID,
		Record_Status,
		Create_Dtm,
		Create_By,
		GETDATE(), -- Update_Dtm,  -- CRE20-023
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
		EC_Reference_No_Other_Format,
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

	FROM [dbo].[TempPersonalInformation]
	WHERE 
		Voucher_Acc_ID = @Temp_Voucher_Acc_ID
	
END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_add_ByTemp] TO HCVU
GO
