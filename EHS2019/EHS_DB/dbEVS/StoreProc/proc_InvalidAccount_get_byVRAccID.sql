IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InvalidAccount_get_byVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InvalidAccount_get_byVRAccID]
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
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	31 Dec 2015
-- Description:		(1) Remove [CCValue]
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
-- Modified by:	  Paul Yip
-- Modified date: 29 March 2010
-- Description:	  Ouput 'Count_Benefit','Original_Acc_Type'
--					add 'Create_By_SmartID'
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 18 Sep 2009
-- Description:	Retrieve the Invalid Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 21 Sep 2009
-- Description:	  Remove the Check_Dtm, Validating field, Create_By_SmartID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Paul Yip	
-- Modified date: 19 March 2010
-- Description:	 Remove unused fields 'Confirmed_Dtm','Validated_Acc_ID','Last_Fail_Validate_Dtm','DataEntry_By'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_InvalidAccount_get_byVRAccID]
	@VRAccID char(15)
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
	--Check_Dtm			datetime,
	--Validating			char(1),	
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
	
	--CCValue1			int,
	--CCValue2			int,
	--CCValue3			int,
	--CCValue4			int,
	--CCValue5			int,
	--CCValue6			int,
	
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
	SmartID_Ver			varchar(5)
)

DECLARE  @tmpVoucherTransaction TABLE
(
	Transaction_ID char(20),
	Invalid_Acc_ID char(15),
	Scheme_Code char(10)
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

	INSERT INTO @tmpVoucherTransaction
	SELECT Transaction_ID, Invalid_Acc_ID, Scheme_Code
	FROM VoucherTransaction
	WHERE [Invalid_Acc_ID] = @VRAccID -- and Scheme_Code = @Scheme_Code
	
	INSERT INTO @tmpPersonalInformation 
	(
		Voucher_Acc_ID,
		DOB,
		Exact_DOB,
		Sex,			
		Date_of_Issue,
		--Check_Dtm,
		--Validating,
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
		SmartID_Ver
	) 
	SELECT
		P.[Invalid_Acc_ID],
		P.[DOB],
		P.[Exact_DOB],
		P.[Sex],
		P.[Date_of_Issue],
		--[HKID_Card],
		--P.[Check_Dtm],
		--P.[Validating],
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
		P.[SmartID_Ver]
	FROM 
		[InvalidPersonalInformation] AS P
	WHERE
		P.[Invalid_Acc_ID] = @VRAccID

/*	
	UPDATE @tmpPersonalInformation SET CCValue1 = CC1.[UniCode_Int] FROM @tmpPersonalInformation tmpPI, [CCC_BIG5] CC1
	WHERE (tmpPI.CCcode1 IS NOT NULL OR RTRIM(LTRIM(CCcode1)) <> '') AND LEFT(CCcode1,4) = CC1.[ccc_head] AND RIGHT(CCcode1,1) = CC1.[ccc_tail]
	
	UPDATE @tmpPersonalInformation SET CCValue2 = CC2.[UniCode_Int] FROM @tmpPersonalInformation tmpPI, [CCC_BIG5] CC2
	WHERE (tmpPI.CCcode2 IS NOT NULL OR RTRIM(LTRIM(CCcode2)) <> '') AND LEFT(CCcode2,4) = CC2.[ccc_head] AND RIGHT(CCcode2,1) = CC2.[ccc_tail]
	
	UPDATE @tmpPersonalInformation SET CCValue3 = CC3.[UniCode_Int] FROM @tmpPersonalInformation tmpPI, [CCC_BIG5] CC3
	WHERE (tmpPI.CCcode3 IS NOT NULL OR RTRIM(LTRIM(CCcode3)) <> '') AND LEFT(CCcode3,4) = CC3.[ccc_head] AND RIGHT(CCcode3,1) = CC3.[ccc_tail]
	
	UPDATE @tmpPersonalInformation SET CCValue4 = CC4.[UniCode_Int] FROM @tmpPersonalInformation tmpPI, [CCC_BIG5] CC4
	WHERE (tmpPI.CCcode4 IS NOT NULL OR RTRIM(LTRIM(CCcode4)) <> '') AND LEFT(CCcode4,4) = CC4.[ccc_head] AND RIGHT(CCcode4,1) = CC4.[ccc_tail]	
	
	UPDATE @tmpPersonalInformation SET CCValue5 = CC5.[UniCode_Int] FROM @tmpPersonalInformation tmpPI, [CCC_BIG5] CC5
	WHERE (tmpPI.CCcode5 IS NOT NULL OR RTRIM(LTRIM(CCcode5)) <> '') AND LEFT(CCcode5,4) = CC5.[ccc_head] AND RIGHT(CCcode5,1) = CC5.[ccc_tail]
	
	UPDATE @tmpPersonalInformation SET CCValue6 = CC6.[UniCode_Int] FROM @tmpPersonalInformation tmpPI, [CCC_BIG5] CC6
	WHERE (tmpPI.CCcode6 IS NOT NULL OR RTRIM(LTRIM(CCcode6)) <> '') AND LEFT(CCcode6,4) = CC6.[ccc_head] AND RIGHT(CCcode6,1) = CC6.[ccc_tail]
*/

	SELECT
		--[SpecialAccount]
		VA.[Invalid_Acc_ID] as [Voucher_Acc_ID],		
		VA.[Scheme_Code],
		VA.[Record_Status],
		VA.[Account_Purpose],
		VA.[Create_Dtm],
		VA.[Create_By],
		VA.[Update_Dtm],
		VA.[Update_By],
		VA.[TSMP],
		VA.[Original_Acc_ID],
		
		--VoucherAccountCreationLog
		ISNULL(L.SP_ID, '') as SP_ID,
		ISNULL(L.SP_Practice_Display_Seq, 0) as SP_Practice_Display_Seq,
				
		--TempVoucherAccPendingVerify
		PV.First_Validate_Dtm,
		
		--@tmpVoucherTransaction
		isNull(VT.Transaction_ID, '') Transaction_ID,
		
		VA.[Count_Benefit],
		VA.[Original_Acc_Type]
		
	FROM
		[InvalidAccount] VA
		
		LEFT OUTER JOIN VoucherAccountCreationLog L on 
			VA.[Invalid_Acc_ID] = L.[Voucher_Acc_ID]
			
		left outer join TempVoucherAccPendingVerify PV on 
			VA.[Invalid_Acc_ID] = PV.[Voucher_Acc_ID] and 
			VA.[Scheme_Code] = PV.[Scheme_Code]
			
		left outer join @tmpVoucherTransaction VT on 
			VT.[Invalid_Acc_ID] = @VRAccID and 
			VA.[Invalid_Acc_ID] = VT.[Invalid_Acc_ID] 
			--and VA.[Scheme_Code] = VT.[Scheme_Code]
	WHERE
		VA.[Invalid_Acc_ID] = @VRAccID
		
	SELECT
		[Voucher_Acc_ID],
		[DOB],
		[Exact_DOB],
		[Sex],
		[Date_of_Issue],
		--[HKID_Card],
		--[Check_Dtm],
		--[Validating],
		[Create_By_SmartID],
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
		--CCValue1,
		--CCValue2,
		--CCValue3,
		--CCValue4,
		--CCValue5,
		--CCValue6,
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
		[EC_Reference_No_Other_Format],
		[SmartID_Ver]
	FROM
		@tmpPersonalInformation 
	
EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_InvalidAccount_get_byVRAccID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InvalidAccount_get_byVRAccID] TO HCVU
GO
