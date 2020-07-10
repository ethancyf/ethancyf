IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInformation_upd_fromTemp]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInformation_upd_fromTemp]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			INT20-0022 (Fix IMMBValidation on IDEAS Combo)
-- Modified by:		Koala CHENG
-- Modified date:	2 Jul 2020
-- Description:		Fix checking on [SmartID_Ver]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
--					Update Merge Priority: Smart ID (New) > Smart ID (Old) > Manual Input
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	16 Aug 2010
-- Description:		Fix the problem when handling merge case other than HKIC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	19 May 2010
-- Description:		(1) [EC_Reference_No]: varchar(15) -> varchar(40)
--					(2) Add [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	9 Apr 2010
-- Description:		Merge case for Temporary account (non-amendment case)
--
--					Summary
--					------------------------
--					Amendment Case	| Create Temporary account Merge Case
--					------------------------
--					Always overwrite| Create_By-SmartID has higher pirority 
--									| (3 key values the same)
--					
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	23 Mar 2010
-- Description:		Merge case for Temporary account (non-amendment case)
--
--					Summary
--					------------------------
--					Amendment Case	| Create Temporary account Merge Case
--					------------------------
--					Always overwrite| Create_By-SmartID has higher pirority 
--					
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Update PersonalInformation Fields From TempPersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 8 Oct 2009
-- Description:	Add new fields and remove obsolete fields
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Pak Ho LEE
-- Modified date: 6 Nov 2009
-- Description:	Fix EC Serial No + EC Reference No Update for Amendment Case Only
-- =============================================
CREATE PROCEDURE [dbo].[proc_PersonalInformation_upd_fromTemp]
	@Voucher_Acc_ID	char(15),
	@Temp_Voucher_Acc_ID char(15),
	@blnAmend tinyint
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

Declare @DOB datetime
Declare @Exact_DOB char(1)
Declare @Sex char(1)
Declare @Date_of_Issue datetime
Declare @Encrypt_Field2	varbinary(100)
Declare @Encrypt_Field3	varbinary(100)
Declare @Encrypt_Field4	varbinary(50)
Declare @Encrypt_Field5	varbinary(50)
Declare @Encrypt_Field6	varbinary(50)
Declare @Encrypt_Field7	varbinary(50)
Declare @Encrypt_Field8	varbinary(50)
Declare @Encrypt_Field9	varbinary(50)
--Declare @EC_Date datetime
Declare @EC_Age smallint
Declare @EC_Date_of_Registration datetime

Declare @Doc_Code char(20)
Declare @Foreign_Passport_No char(20)
Declare @Permit_to_Remain_Until datetime
Declare @Other_Info char(10)

Declare @EC_Serial_No varchar(10)
Declare @EC_Reference_No varchar(40)
Declare @EC_Reference_No_Other_Format char(1)

Declare @Create_By_SmartID char(1)
Declare @SmartID_Ver	varchar(5)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

IF @blnAmend = 0
BEGIN
	
	SELECT 
		@DOB = DOB,
		@Exact_DOB = Exact_DOB,
		@Sex = Sex,
		@Date_of_Issue = Date_of_Issue,
		@Encrypt_Field2 = Encrypt_Field2,
		@Encrypt_Field3 = Encrypt_Field3,
		@Encrypt_Field4 = Encrypt_Field4,
		@Encrypt_Field5 = Encrypt_Field5,
		@Encrypt_Field6 = Encrypt_Field6,
		@Encrypt_Field7 = Encrypt_Field7,
		@Encrypt_Field8 = Encrypt_Field8,
		@Encrypt_Field9 = Encrypt_Field9,
		--@EC_Date = EC_Date,
		@EC_Age = EC_Age,
		@EC_Date_of_Registration = EC_Date_of_Registration,
		@Doc_Code = Doc_Code,
		@Foreign_Passport_No = Foreign_Passport_No,
		@Permit_to_Remain_Until = Permit_to_Remain_Until,
		@Other_Info = Other_Info,
		@Create_By_SmartID = Create_By_SmartID,
		@SmartID_Ver = SmartID_Ver

	FROM [dbo].[TempPersonalInformation]	
	WHERE 
		Voucher_Acc_ID = @Temp_Voucher_Acc_ID
		
	
	UPDATE [dbo].[PersonalInformation]
	SET
		DOB = @DOB,
		Exact_DOB = @Exact_DOB,
		Sex = @Sex,
		Date_of_Issue = @Date_of_Issue,
		Encrypt_Field2 = @Encrypt_Field2,
		Encrypt_Field3 = @Encrypt_Field3,
		Encrypt_Field4 = @Encrypt_Field4,
		Encrypt_Field5 = @Encrypt_Field5,
		Encrypt_Field6 = @Encrypt_Field6,
		Encrypt_Field7 = @Encrypt_Field7,
		Encrypt_Field8 = @Encrypt_Field8,
		Encrypt_Field9 = @Encrypt_Field9,
		--EC_Date = @EC_Date,
		EC_Age = @EC_Age,
		EC_Date_of_Registration = @EC_Date_of_Registration,
		Doc_Code = @Doc_Code,
		Foreign_Passport_No = @Foreign_Passport_No,
		Permit_to_Remain_Until = @Permit_to_Remain_Until,
		Other_Info = @Other_Info,
		Create_By_SmartID = @Create_By_SmartID,
		SmartID_Ver = @SmartID_Ver
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Doc_Code = @Doc_Code AND --Create_By_SmartID <> 'Y'
		(
			-- Priority: Smart ID (New) > Smart ID (Old) > Manual Input
			-- NOT Overwrite the Personal Info for below cases:
			-- 1. Same DOB & DOI and 
			--		a. If Validated Acct is created by Smart IC but Temp acct is created by Manual Input
			--		b. If Validated Acct Smart ID version is newer than Temp Acct Smart ID version
			NOT (
				DOB = @DOB AND Exact_DOB = @Exact_DOB 
				AND Date_of_Issue = @Date_of_Issue
				AND Create_by_SmartID is not null AND Create_by_SmartID = 'Y'
				AND (
						@Create_By_SmartID = 'N'
						OR (
							@Create_By_SmartID = 'Y' 
							AND CONVERT(INT, ISNULL(LEFT(SmartID_Ver,2), 10)) > CONVERT(INT, ISNULL(LEFT(@SmartID_Ver,2), 10))   
						)
					)
				
			)			
		)
END	
ELSE
BEGIN

SELECT 
		@DOB = DOB,
		@Exact_DOB = Exact_DOB,
		@Sex = Sex,
		@Date_of_Issue = Date_of_Issue,
		@Encrypt_Field2 = Encrypt_Field2,
		@Encrypt_Field3 = Encrypt_Field3,
		@Encrypt_Field4 = Encrypt_Field4,
		@Encrypt_Field5 = Encrypt_Field5,
		@Encrypt_Field6 = Encrypt_Field6,
		@Encrypt_Field7 = Encrypt_Field7,
		@Encrypt_Field8 = Encrypt_Field8,
		@Encrypt_Field9 = Encrypt_Field9,
		--@EC_Date = EC_Date,
		@EC_Age = EC_Age,
		@EC_Date_of_Registration = EC_Date_of_Registration,
		@Doc_Code = Doc_Code,
		@Foreign_Passport_No = Foreign_Passport_No,
		@Permit_to_Remain_Until = Permit_to_Remain_Until,
		@Other_Info = Other_Info,
		--@DOB = DOB,
		--@Exact_DOB = Exact_DOB,
		--@Date_of_Issue = Date_of_Issue
		@EC_Serial_No = EC_Serial_No,
		@EC_Reference_No = EC_Reference_No,
		@EC_Reference_No_Other_Format = EC_Reference_No_Other_Format,
		@Create_By_SmartID = Create_By_SmartID,
		@SmartID_Ver = SmartID_Ver
		
	FROM [dbo].[TempPersonalInformation]	
	WHERE 
		Voucher_Acc_ID = @Temp_Voucher_Acc_ID
		
	
	UPDATE [dbo].[PersonalInformation]
	SET
		DOB = @DOB,
		Exact_DOB = @Exact_DOB,
		Sex = @Sex,
		Date_of_Issue = @Date_of_Issue,
		Encrypt_Field2 = @Encrypt_Field2,
		Encrypt_Field3 = @Encrypt_Field3,
		Encrypt_Field4 = @Encrypt_Field4,
		Encrypt_Field5 = @Encrypt_Field5,
		Encrypt_Field6 = @Encrypt_Field6,
		Encrypt_Field7 = @Encrypt_Field7,
		Encrypt_Field8 = @Encrypt_Field8,
		Encrypt_Field9 = @Encrypt_Field9,
		
		--EC_Date = @EC_Date,
		EC_Age = @EC_Age,
		EC_Date_of_Registration = @EC_Date_of_Registration,
		--DOB = @DOB,
		--Exact_DOB = @Exact_DOB,
		--Date_of_Issue = @Date_of_Issue,
		Doc_Code = @Doc_Code,
		Foreign_Passport_No = @Foreign_Passport_No,
		Permit_to_Remain_Until = @Permit_to_Remain_Until,
		Other_Info = @Other_Info,
		--Create_By_SmartID = 'N',
		Record_Status = 'A',		
		EC_Serial_No = @EC_Serial_No,
		EC_Reference_No = @EC_Reference_No,
		EC_Reference_No_Other_Format = @EC_Reference_No_Other_Format,
		Create_By_SmartID = @Create_By_SmartID,
		SmartID_Ver = @SmartID_Ver
		
	WHERE 
		Voucher_Acc_ID = @Voucher_Acc_ID AND Doc_Code = @Doc_Code

END


END

GO

GRANT EXECUTE ON [dbo].[proc_PersonalInformation_upd_fromTemp] TO HCVU
GO
