IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_get_byVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_get_byVRAccID]
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
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
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
-- Description:		(1) Add VoucherAccount - [Deceased]
--					(2) Add PersonalInformation - [Deceased], [DOD], [Exact_DOD]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	4 Jan 2016
-- Description:		(1) Remove [CCValue]
--					(2) Remove Compare CCCode logic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	02 Feb 2011	
-- Description:		Check CCC code value against Chinese name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			Pak Ho LEE
-- Modified date:		03 Aug 2010
-- Description:			Join CreationLog with [Type = 'V']
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 May 2010
-- Description:		(1) [EC_Reference_No]: varchar(15) -> varchar(40)
--					(2) Retrieve [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Author:		Clark Yip
-- Create date: 29 April 2008
-- Description:	Retrieve the Voucher Recipient (ych480:Encryption)
--			    Account information by Voucher Account ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Timothy LEUNG
-- Modified date: 30 May 2008
-- Description:	Add SchemeCode as the input parameter 
--				to search VoucherAccount	
--
-- Modified by:	 Timothy LEUNG
-- Modified date: 20 Oct 2008
-- Description:	Add Certification of Exemption information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	19 Aug 2009
-- Description:		Oct19 Version new schema
--					Voucher Account + PersonalInformation (1 or More)
-- =============================================

-- =============================================
-- Remark:
--	Use Left Outer Join on CCCode Table have Performance Issue!
--	Create Temp Table, and update the CCCode Value accordingly
-- =============================================


CREATE PROCEDURE [dbo].[proc_VoucherAccount_get_byVRAccID]
	@VRAccID char(15)
	--@Scheme_Code char(10)
AS
BEGIN
	SET NOCOUNT ON;
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
	Permit_To_Remain_Until	datetime,
	AdoptionPrefixNum	char(7),
	Other_Info			varchar(10),
	EC_Reference_No_Other_Format	char(1),
	Deceased			char(1),
	DOD					datetime, 
	Exact_DOD			char(1),
	SmartID_Ver			varchar(5)
)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================               

	INSERT INTO @tmpPersonalInformation 
	(
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,			
		Date_of_Issue,
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
			[Voucher_Acc_ID],
			[DOB],
			[Exact_DOB],
			[Sex],
			[Date_of_Issue],
			--[HKID_Card],		
			[Create_By_SmartID],
			[Record_Status],
			[Create_Dtm],
			[Create_By],
			[Update_Dtm],
			[Update_By],
			[DataEntry_By],

			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])) as IdentityNum,
			CONVERT(VARCHAR(100), DecryptByKey(P.[Encrypt_Field2])) as Eng_Name,
			CONVERT(NVARCHAR, DecryptByKey(P.[Encrypt_Field3])) as Chi_Name,			
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field4])) as CCcode1,
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field5])) as CCcode2,
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field6])) as CCcode3,
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field7])) as CCcode4,
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field8])) as CCcode5,
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field9])) as CCcode6,	
			
			[TSMP],
			[EC_Serial_No],
			[EC_Reference_No],
			--[EC_Date],
			[EC_Age],
			[EC_Date_of_Registration],
			[Doc_Code],
			[Foreign_Passport_No],
			[Permit_To_Remain_Until],
			CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field11])) as [AdoptionPrefixNum],	
			[Other_Info],
			[EC_Reference_No_Other_Format],
			[Deceased],
			[DOD], 
			[Exact_DOD],
			[SmartID_Ver]
		FROM 
			[PersonalInformation] AS P
		WHERE
			[Voucher_Acc_ID] = @VRAccID
						

-- VoucherAccount

	SELECT
		VA.[Voucher_Acc_ID],		
		VA.[Scheme_Code],
		--VA.[Voucher_Used],
		--VA.[Total_Voucher_Amt_Used],
		VA.[Record_Status],
		VA.[Remark],
		VA.[Public_Enquiry_Status],
		VA.[Public_Enq_Status_Remark],
		VA.[Effective_Dtm],
		VA.[Terminate_Dtm],
		VA.[Create_Dtm],
		VA.[Create_By],
		VA.[Update_Dtm],
		VA.[Update_By],
		VA.[DataEntry_By],
		VA.[TSMP],
		ISNULL(L.SP_ID, '') as SP_ID,
		ISNULL(L.SP_Practice_Display_Seq, 0) as SP_Practice_Display_Seq,
		VA.[Deceased]
	FROM
		[VoucherAccount] VA		
			LEFT OUTER JOIN [VoucherAccountCreationLog] L
				ON VA.Voucher_Acc_ID = L.Voucher_Acc_ID And L.[Voucher_Acc_Type] = 'V'
	WHERE
		VA.[Voucher_Acc_ID] = @VRAccID
		
		
		
-- PersonalInformation
		
	SELECT
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,			
		Date_of_Issue,
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
	FROM
	@tmpPersonalInformation


EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byVRAccID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byVRAccID] TO HCVU
GO
