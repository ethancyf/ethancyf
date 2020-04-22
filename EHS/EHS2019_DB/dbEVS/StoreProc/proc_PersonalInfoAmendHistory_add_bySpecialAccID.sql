IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_add_bySpecialAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_add_bySpecialAccID]
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
-- Author:		Dedrick Ng
-- Create date: 25 Sep 2009
-- Description:	Insert [PersonalInforAmendHistory]
-- =============================================

CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_add_bySpecialAccID]
	@Voucher_Acc_ID	char(15),
	@Special_Acc_ID	char(15),
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
		SmartID_Ver
	)
	SELECT 
		GetDate(),
		@Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		'N',
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
		@Special_Acc_ID,
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
		SmartID_Ver
	FROM [dbo].[SpecialPersonalInformation]
	WHERE Special_Acc_ID = @Special_Acc_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_add_bySpecialAccID] TO HCVU
GO
