IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_get_byHKIDSchCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_get_byHKIDSchCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Koala CHENG
-- Modified date:	06 Jan 2021
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
--					Obsolete SProc
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 22 April 2008
-- Description:	Retrieve the Voucher Recipient 
--			    Account information by HKID and Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Timothy LEUNG
-- Modified date:	20 Oct 2008
-- Description:		Add Certification of Exemption information
-- =============================================

--CREATE PROCEDURE [dbo].[proc_VoucherAccount_get_byHKIDSchCode]
--	@HKID char(9),
--	@Scheme_Code varchar(10)
--AS
--BEGIN
--	SET NOCOUNT ON;
---- =============================================
---- Declaration
---- =============================================
---- =============================================
---- Validation 
---- =============================================
---- =============================================
---- Initialization
---- =============================================
---- =============================================
---- Return results
---- =============================================
--OPEN SYMMETRIC KEY sym_Key 
--	DECRYPTION BY ASYMMETRIC KEY asym_Key

--	SELECT 
--		convert(varchar, DecryptByKey(P.Encrypt_Field1)) as HKID, 
--		convert(varchar(40), DecryptByKey(P.Encrypt_Field2)) as Eng_Name, 
--		convert(nvarchar, DecryptByKey(P.Encrypt_Field3)) as Chi_Name, 
--		convert(varchar, DecryptByKey(P.Encrypt_Field4)) as CCcode1, 
--		convert(varchar, DecryptByKey(P.Encrypt_Field5)) as CCcode2, 
--		convert(varchar, DecryptByKey(P.Encrypt_Field6)) as CCcode3, 
--		convert(varchar, DecryptByKey(P.Encrypt_Field7)) as CCcode4, 
--		convert(varchar, DecryptByKey(P.Encrypt_Field8)) as CCcode5, 
--		convert(varchar, DecryptByKey(P.Encrypt_Field9)) as CCcode6, 
--			P.DOB, P.Exact_DOB, P.Sex,
--			P.Date_of_issue, P.Create_By, P.Create_Dtm, VA.Voucher_Acc_ID, 
--			VA.Scheme_Code, VA.Voucher_Used, VA.Total_Voucher_Amt_Used, VA.Record_Status, 
--			VA.Public_Enquiry_Status,VA.Remark, VA.Public_Enq_Status_Remark, VA.TSMP as VATSMP, P.TSMP as PITSMP,
--			P.Record_Status as PIStatus, 
--			isNull(P.HKID_Card, 'N') as HKID_Card,
--			P.EC_Serial_No,
--			P.EC_Reference_No,
--			P.EC_Date,
--			P.EC_Age,
--			P.EC_Date_of_Registration
--	From dbo.PersonalInformation P, dbo.VoucherAccount VA
--	where 
--	P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and
--			P.Voucher_Acc_ID = VA.Voucher_Acc_ID and
--			VA.Scheme_Code = @Scheme_Code

--CLOSE SYMMETRIC KEY sym_Key
--END
--GO

--GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byHKIDSchCode] TO HCPUBLIC
--GO

--GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byHKIDSchCode] TO HCSP
--GO

--GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byHKIDSchCode] TO HCVU
--GO
