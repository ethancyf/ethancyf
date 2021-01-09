IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_get_byVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_get_byVRAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Modified by:	    Winnie SUEN
-- Modified date:	26 Aug 2018
-- Description:		Add [TempVoucherAccount] - [SourceApp]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		(1) Add TempVoucherAccount - [Deceased]
--					(2) Add TempPersonalInformation - [Deceased], [DOD], [Exact_DOD]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	3 May 2010
-- Description:		(1) Change [EC_Reference_No] varchar(15) -> varchar(40)
--					(2) Get [EC_Reference_No_Other_Format]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	29 Dec 2009
-- Description:		Get 'Create_By_SmartID', 'Orginial_Amend_Acc_ID', 'Create_By_BO'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	29 Dec 2009
-- Description:		Get 'Create_By_SmartID', 'Orginial_Amend_Acc_ID', 'Create_By_BO'
-- =============================================
-- =============================================
-- Author:		Clark Yip
-- Create date: 29 April 2008
-- Description:	Retrieve the Temp Voucher Recipient 
--			    Account information by Voucher Account ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Timothy LEUNG
-- Modified date: 29 May 2008 
-- Description:	Add Scheme as the retrieve parameter to search Voucher Acocunt
--
-- Modified by:	 Timothy LEUNG
-- Modified date: 20 Oct 2008
-- Description:	Add Certification of Exemption information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 18 Dec 2008
-- Description:	Get the field Original_Acc_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Stanley Chan
-- Modified date: 19 Aug 2008
-- Description:	1) Return 2 table 
--					i) Personal information
--					ii) TempVoucherAccount
--				2) assign updated chineses name
-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_get_byVRAccID]
	@VRAccID char(15)
	--@Scheme_Code char(10)
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
	Eng_Name			varchar(40),
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
	EC_Reference_No_Other_Format		char(1),
	Deceased			char(1),
	DOD					datetime,
	Exact_DOD			char(1),
	SmartID_Ver			varchar(5)
)


EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	declare  @tmpVoucherTransaction table (Transaction_ID char(20), Temp_Voucher_Acc_ID char(15), Scheme_Code char(10))
	
	insert into @tmpVoucherTransaction
	select Transaction_ID, Temp_Voucher_Acc_ID, Scheme_Code
	from VoucherTransaction
	where Temp_Voucher_Acc_ID = @VRAccID -- and Scheme_Code = @Scheme_Code
	

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
		P.[Voucher_Acc_ID],
		P.[DOB],
		P.[Exact_DOB],
		P.[Sex],
		P.[Date_of_Issue],
		--[HKID_Card],
		P.[Check_Dtm],
		P.[Validating],
		P.[Create_By_SmartID],
		P.[Record_Status],
		P.[Create_Dtm],
		P.[Create_By],
		P.[Update_Dtm],
		P.[Update_By],
		P.[DataEntry_By],
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])) as [IdentityNum],
		CONVERT(VARCHAR(40), DecryptByKey(P.[Encrypt_Field2])) as [Eng_Name],
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
		[TempPersonalInformation] AS P
	WHERE
		P.[Voucher_Acc_ID] = @VRAccID
	

	SELECT
		--[TempVoucherAccount]
		VA.[Voucher_Acc_ID],		
		VA.[Scheme_Code],
		VA.[Validated_Acc_ID],
		VA.[Record_Status],
		VA.[Account_Purpose],
		VA.[Confirm_Dtm],
		VA.[Last_Fail_Validate_Dtm],
		VA.[Create_Dtm],
		VA.[Create_By],
		VA.[Update_Dtm],
		VA.[Update_By],
		VA.[DataEntry_By],
		VA.[TSMP],
		VA.[Original_Acc_ID],
		VA.[Original_Amend_Acc_ID],
		VA.[Create_By_BO],
		
		--VoucherAccountCreationLog
		ISNULL(L.SP_ID, '') as SP_ID,
		ISNULL(L.SP_Practice_Display_Seq, 0) as SP_Practice_Display_Seq,
				
		--TempVoucherAccPendingVerify
		PV.First_Validate_Dtm,
		
		--@tmpVoucherTransaction
		isNull(VT.Transaction_ID, '') Transaction_ID,
		VA.[Deceased],
		VA.[SourceApp]
	FROM
		TempVoucherAccount VA
		
		LEFT OUTER JOIN VoucherAccountCreationLog L on 
			VA.[Voucher_Acc_ID] = L.[Voucher_Acc_ID]
			
		left outer join TempVoucherAccPendingVerify PV on 
			VA.[Voucher_Acc_ID] = PV.[Voucher_Acc_ID]
			--and VA.[Scheme_Code] = PV.[Scheme_Code]
			
		left outer join @tmpVoucherTransaction VT on 
			VT.[Temp_Voucher_Acc_ID] = @VRAccID and 
			VA.[Voucher_Acc_ID] = VT.[Temp_Voucher_Acc_ID] 
			--and VA.[Scheme_Code] = VT.[Scheme_Code]
	WHERE
		VA.[Voucher_Acc_ID] = @VRAccID
		
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

	
EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byVRAccID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byVRAccID] TO HCVU
GO
