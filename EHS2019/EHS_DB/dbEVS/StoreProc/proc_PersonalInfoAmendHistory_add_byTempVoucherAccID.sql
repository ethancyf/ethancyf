IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_add_byTempVoucherAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_add_byTempVoucherAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (COVID19)
-- Modified by:		Winnie SUEN
-- Modified date:	28 May 2021
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
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	11 Feb 2010
-- Description:		Add 'Create_by_SmartID' from
--					TempPersonalInformation to PersonalInformation
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Insert [PersonalInforAmendHistory]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 17 Sep 2009
-- Description:	Add new fields and remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_add_byTempVoucherAccID]
	@Voucher_Acc_ID	char(15),
	@Temp_Voucher_Acc_ID	char(15),
	@Action_Type char(1)
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


	INSERT INTO [PersonalInfoAmendHistory]
	(
		System_Dtm,
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		--HKID_Card,
		Create_By_SmartID,
		Update_By,
		Record_Status,
		SubmitToVerify,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		Temp_Voucher_Acc_ID,
		Action_type,
		Cancel_By,
		Cancel_dtm,
		EC_Serial_No,
		EC_Reference_No,
		EC_Reference_No_Other_Format,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		
		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		SmartID_Ver,
		PASS_Issue_Region
	)
	SELECT 
		GetDate(),
		@Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Create_By_SmartID,
		--HKID_Card,
		--'N',
		Update_By,
		'A',
		'N',
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Encrypt_Field4,
		Encrypt_Field5,
		Encrypt_Field6,
		Encrypt_Field7,
		Encrypt_Field8,
		Encrypt_Field9,
		@Temp_Voucher_Acc_ID,
		@Action_Type,
		NULL,
		NULL,
		EC_Serial_No,
		EC_Reference_No,
		EC_Reference_No_Other_Format,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		
		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		Encrypt_Field11,
		Other_Info,
		SmartID_Ver,
		PASS_Issue_Region
	FROM [dbo].[TempPersonalInformation]
	WHERE Voucher_Acc_ID = @Temp_Voucher_Acc_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_add_byTempVoucherAccID] TO HCVU
GO
