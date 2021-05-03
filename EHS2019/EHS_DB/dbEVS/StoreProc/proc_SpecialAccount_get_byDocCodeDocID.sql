IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_get_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_get_byDocCodeDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
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
-- Description:		(1) Add SpecialAccount - [Deceased]
--					(2) Add SpecialPersonalInformation - [Deceased], [DOD], [Exact_DOD]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	31 Dec 2015
-- Description:		Remove [CCValue]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 May 2010
-- Description:		(1) [EC_Reference_No]: varchar(15) -> varchar(40)
--					(2) Retrieve [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	10 Feb 2010
-- Description:		Get 'Create_By_SmartID',
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:	07 Dec 2009
-- Description:	    Arrange the open & close SYMMETRIC KEY
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 08 Sep 2009
-- Description:	Retrieve the Special Account By Document & Document Identity
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_SpecialAccount_get_byDocCodeDocID]
	@Doc_Code char(20),
	@identity varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	
-- Performance Issue: Do not Retrieve Temporary Account with Status = 'D'
	
-- =============================================
-- Declaration
-- =============================================

DECLARE @tmpPersonalInformation Table 
(
	Voucher_Acc_ID		char(15),
	DOB					datetime,
	Exact_DOB			char(1),
	Sex					char(1),
	Date_of_Issue		datetime,
	Check_Dtm			datetime,
	Validating			char(1),
	---HKID_Card		char(1),
	Create_By_SmartID	char(1),
	Record_Status		char(1),
	Create_Dtm			datetime,
	Create_By			varchar(20),
	Update_Dtm			datetime,
	Update_By			varchar(20),
	DataEntry_By		varchar(20),	
		
	IdentityNum			varchar(20),
	Eng_Name			varchar(100),
	Chi_Name			nvarchar(20),
	CCcode1				char(5),
	CCcode2				char(5),
	CCcode3				char(5),
	CCcode4				char(5),
	CCcode5				char(5),
	CCcode6				char(5),
	TSMP				varbinary(100),
	EC_Serial_No		varchar(10),
	EC_Reference_No		varchar(40),
	---EC_Date	datetime,
	EC_Age				smallint,
	EC_Date_of_Registration	datetime,
	--Encrypt_Field10		varbinary(100),
	Doc_Code			char(20),
	Foreign_Passport_No	char(20),
	Permit_To_Remain_Until datetime,
	AdoptionPrefixNum	char(7),
	Other_Info			varchar(10),
	EC_Reference_No_Other_Format	char(1),
	Deceased			char(1),
	DOD					datetime,
	Exact_DOD			char(1),
	SmartID_Ver			varchar(5)
)

DECLARE @tmpVoucherAcct Table
(
	Voucher_Acc_ID char(15)
)

DECLARE @tmpVoucherTransaction Table 
(
	Transaction_ID char(20),
	Special_Acc_ID char(15),
	Scheme_Code char(10)
)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

	INSERT INTO @tmpVoucherAcct
	SELECT	SPI.[Special_Acc_ID]
	FROM	[SpecialPersonalInformation] SPI
		INNER JOIN [SpecialAccount] SVA 
			ON SPI.[Special_Acc_ID] = SVA.[Special_Acc_ID]			
	WHERE	SVA.[Record_Status] <> 'D' AND
			[Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) 
		
									
	INSERT INTO @tmpVoucherTransaction	
	SELECT
		[Transaction_ID], [Special_Acc_ID], [Scheme_Code]
	FROM [VoucherTransaction]
	WHERE 
		[Special_Acc_ID] IN 
			(SELECT Voucher_Acc_ID FROM @tmpVoucherAcct) --and Scheme_Code = @Scheme_Code
		
	INSERT INTO @tmpPersonalInformation 
	(
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,			
		Date_of_Issue,
		Check_Dtm,
		Validating,
		---HKID_Card,
		Create_By_SmartID,
		Record_Status,	
		Create_Dtm,		
		Create_By,			
		Update_Dtm,			
		Update_By,			
		DataEntry_By,

		IdentityNum,
		Eng_Name,
		Chi_Name,
		CCcode1,
		CCcode2,
		CCcode3,
		CCcode4,
		CCcode5,
		CCcode6,
				
		TSMP,
		EC_Serial_No,
		EC_Reference_No,
		---EC_Date	datetime,
		EC_Age,
		EC_Date_of_Registration,
		--Encrypt_Field10,
		Doc_Code,
		Foreign_Passport_No,
		Permit_To_Remain_Until,
		AdoptionPrefixNum,
		Other_Info,
		EC_Reference_No_Other_Format,
		Deceased,
		DOD,
		Exact_DOD,
		SmartID_Ver
	) 
	
	SELECT
		P.[Special_Acc_ID],
		P.[DOB],
		P.[Exact_DOB],
		P.[Sex],
		P.[Date_of_Issue],
		--[HKID_Card],
		P.[Check_Dtm],
		P.[Create_By_SmartID],
		P.[Validating],
		P.[Record_Status],
		P.[Create_Dtm],
		P.[Create_By],
		P.[Update_Dtm],
		P.[Update_By],
		P.[DataEntry_By],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])) as [IdentityNum],
		CONVERT(VARCHAR(100), DecryptByKey(P.[Encrypt_Field2])) as [Eng_Name],
		CONVERT(NVARCHAR, DecryptByKey(P.[Encrypt_Field3])) as [Chi_Name],			
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field4])) as [CCcode1],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field5])) as [CCcode2],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field6])) as [CCcode3],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field7])) as [CCcode4],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field8])) as [CCcode5],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field9])) as [CCcode6],
		P.[TSMP],		
		P.[EC_Serial_No],
		P.[EC_Reference_No],
		P.[EC_Age],
		P.[EC_Date_of_Registration],
		P.[Doc_Code],
		P.[Foreign_Passport_No],
		P.[Permit_To_Remain_Until],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field11])) as [AdoptionPrefixNum],
		P.[Other_Info],
		P.[EC_Reference_No_Other_Format],
		P.[Deceased],
		P.[DOD],
		P.[Exact_DOD],
		P.[SmartID_Ver]
	FROM 
		[SpecialPersonalInformation] AS P
	WHERE
		P.[Special_Acc_ID] In (SELECT [Voucher_Acc_ID] FROM @tmpVoucherAcct)		
		
CLOSE SYMMETRIC KEY sym_Key
	
	SELECT
		--[TempVoucherAccount]
		VA.[Special_Acc_ID] as [Voucher_Acc_ID],		
		VA.[Scheme_Code],
		VA.[Validated_Acc_ID],
		VA.[Record_Status],
		VA.[Account_Purpose],
		VA.[Confirmed_Dtm] as [Confirm_Dtm],
		VA.[Last_Fail_Validate_Dtm],
		VA.[Create_Dtm],
		VA.[Create_By],
		VA.[Update_Dtm],
		VA.[Update_By],
		VA.[DataEntry_By],
		VA.[TSMP],
		VA.[Original_Acc_ID],
		
		ISNULL(L.SP_ID, '') as SP_ID,
		ISNULL(L.SP_Practice_Display_Seq, 0) as SP_Practice_Display_Seq,
		
		--TempVoucherAccPendingVerify
		PV.First_Validate_Dtm,
		
		--@tmpVoucherTransaction
		ISNULL(VT.Transaction_ID, '') Transaction_ID,
		
		VA.[Temp_Voucher_Acc_ID],	--Only Special Account Contain this field
		VA.[Deceased]
	FROM
		[SpecialAccount] VA
		
			LEFT OUTER JOIN VoucherAccountCreationLog L on 
				VA.[Special_Acc_ID] = L.[Voucher_Acc_ID]
				
			LEFT OUTER JOIN TempVoucherAccPendingVerify PV on 
			VA.[Special_Acc_ID] = PV.[Voucher_Acc_ID] 
			--and VA.[Scheme_Code] = PV.[Scheme_Code]
			
			LEFT OUTER JOIN @tmpVoucherTransaction VT 
				ON VA.[Special_Acc_ID] = VT.[Special_Acc_ID]			
	WHERE
		VA.[Special_Acc_ID] IN
			(SELECT Voucher_Acc_ID FROM @tmpVoucherAcct)		
	
	SELECT
		[Voucher_Acc_ID],
		[DOB],
		[Exact_DOB],
		[Sex],
		[Date_of_Issue],
		--[HKID_Card],
		[Check_Dtm],
		[Validating],
		[Record_Status],
		[Create_Dtm],
		[Create_By],
		[Update_Dtm],
		[Update_By],
		[DataEntry_By],
		[IdentityNum],
		[Eng_Name],
		[Chi_Name],			
		[CCcode1],
		[CCcode2],
		[CCcode3],
		[CCcode4],
		[CCcode5],
		[CCcode6],
		[TSMP],		
		[EC_Serial_No],
		[EC_Reference_No],
		[EC_Serial_No],
		[EC_Reference_No],
		[EC_Age],
		[EC_Date_of_Registration],
		[Doc_Code],
		[Foreign_Passport_No],
		[Permit_To_Remain_Until],
		[AdoptionPrefixNum],
		[Other_Info],
		[Create_By_SmartID],
		[EC_Reference_No_Other_Format],
		[Deceased],
		[DOD],
		[Exact_DOD],
		[SmartID_Ver]
	FROM
		@tmpPersonalInformation 
		
END
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_get_byDocCodeDocID] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_get_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_get_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SpecialAccount_get_byDocCodeDocID] TO WSEXT
Go