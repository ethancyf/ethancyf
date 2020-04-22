IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_get_byECSchCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_get_byECSchCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 08 May 2008
-- Description:	Retrieve the Temporary Voucher Recipient 
--			    Account information by EC Details and Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 PAK HO LEE
-- Modified date: 22 Dec 2008
-- Description:	Get the field Original_Acc_ID
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_get_byECSchCode]
	@EC_Serial_No varchar(10),
	@EC_Reference_No varchar(15),
	@Scheme_Code varchar(10)
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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

declare @tmpVoucherAcct table (Voucher_Acc_ID char(15)
									)
									
	insert into @tmpVoucherAcct
	select Voucher_Acc_ID
	from TempPersonalInformation 
	where EC_Serial_No = @EC_Serial_No AND
		EC_Reference_No = @EC_Reference_No
	
	declare @tmpVoucherTransaction table (Transaction_ID char(15),
									Temp_Voucher_Acc_ID char(15),
									Scheme_Code char(10)
								    )
									
	insert into @tmpVoucherTransaction
	select Transaction_ID, Temp_Voucher_Acc_ID, Scheme_Code
	from VoucherTransaction
	where Temp_Voucher_Acc_ID  in 
	(select Voucher_Acc_ID from @tmpVoucherAcct) and
	Scheme_Code = @Scheme_Code
	

	SELECT 
		convert(varchar, DecryptByKey(P.Encrypt_Field1)) as HKID, 
		convert(varchar(40), DecryptByKey(P.Encrypt_Field2)) as Eng_Name, 
		convert(nvarchar, DecryptByKey(P.Encrypt_Field3)) as Chi_Name, 
		convert(varchar, DecryptByKey(P.Encrypt_Field4)) as CCcode1, 
		convert(varchar, DecryptByKey(P.Encrypt_Field5)) as CCcode2, 
		convert(varchar, DecryptByKey(P.Encrypt_Field6)) as CCcode3, 
		convert(varchar, DecryptByKey(P.Encrypt_Field7)) as CCcode4, 
		convert(varchar, DecryptByKey(P.Encrypt_Field8)) as CCcode5, 
		convert(varchar, DecryptByKey(P.Encrypt_Field9)) as CCcode6, 
		P.DOB, P.Exact_DOB, P.Sex,
			P.Date_of_issue, P.Create_By, P.Create_Dtm, VA.Voucher_Acc_ID, 
			VA.Scheme_Code, VA.Voucher_Used, VA.Total_Voucher_Amt_Used, VA.Record_Status, 
			VA.TSMP as VATSMP, P.TSMP as PITSMP,
			P.Record_Status as PIStatus,
			isNull(VT.Transaction_ID, '') Transaction_ID,
			VA.Account_Purpose,
			isNull(P.Validating, '') Validating, 
			isNull(P.HKID_Card, 'N') as HKID_Card ,
			P.EC_Serial_No,
			P.EC_Reference_No,
			P.EC_Date,
			P.EC_Age,
			P.EC_Date_of_Registration,
			VA.Original_Acc_ID
		
	from
		TempVoucherAccount VA inner join TempPersonalInformation P on
		P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
		VA.Scheme_Code = @Scheme_Code AND
		P.EC_Serial_No = @EC_Serial_No AND
		P.EC_Reference_No = @EC_Reference_No
		left outer join @tmpVoucherTransaction VT on 
		VA.Voucher_Acc_ID = VT.Temp_Voucher_Acc_ID
	where 
		VA.Scheme_Code = @Scheme_Code AND
		P.EC_Serial_No = @EC_Serial_No AND
		P.EC_Reference_No = @EC_Reference_No

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byECSchCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byECSchCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byECSchCode] TO HCVU
GO
