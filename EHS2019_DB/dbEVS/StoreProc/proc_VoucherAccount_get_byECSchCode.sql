IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_get_byECSchCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_get_byECSchCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		PAKHO LEE
-- Create date: 22 April 2008
-- Description:	Retrieve the Voucher Recipient 
--			    Account information by EC Details and Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_VoucherAccount_get_byECSchCode]
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
			VA.Public_Enquiry_Status,VA.Remark, VA.Public_Enq_Status_Remark, VA.TSMP as VATSMP, P.TSMP as PITSMP,
			P.Record_Status as PIStatus, 
			isNull(P.HKID_Card, 'N') as HKID_Card,
			
			P.EC_Serial_No,
			P.EC_Reference_No,
			P.EC_Date,
			P.EC_Age,
			P.EC_Date_of_Registration
			
	From dbo.PersonalInformation P, dbo.VoucherAccount VA
	where 
			P.Voucher_Acc_ID = VA.Voucher_Acc_ID AND
			VA.Scheme_Code = @Scheme_Code AND
			P.EC_Serial_No = @EC_Serial_No AND
			P.EC_Reference_No = @EC_Reference_No			

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byECSchCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byECSchCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byECSchCode] TO HCVU
GO
