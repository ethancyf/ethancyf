IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID]
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
-- Modified by:		Chris YIM
-- Modified date:	22 Jan 2018
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Description:		Add DOD and Exact_DOD
-- =============================================
-- =============================================
-- CR No.:		CRE13-006
-- Author:		Karl LAM
-- Create date: 31 Oct 2013
-- Description:	Retrieve All Related EHS Account By Document & Document Identity
-- =============================================

--exec proc_RelatedWriteOffAccount_get_byDocCodeDocID 'HKIC', ' A1111119'

CREATE PROCEDURE [dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID]
	@Doc_Code char(20),
	@identity varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Other_Doc_Code char(20)
	SET @Other_Doc_Code = ''

-- =============================================
-- Process
-- =============================================

--Handle HKIC AND HKBC
	IF	(@Doc_Code = 'HKIC') SET @Other_Doc_Code = 'HKBC'
	IF	(@Doc_Code = 'HKBC') SET @Other_Doc_Code = 'HKIC' 

EXEC [proc_SymmetricKey_open]
        
    --Get Validated Account
		SELECT
			[IdentityNum] = CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])),
			P.[Doc_Code],
			P.[DOB],
			P.[Exact_DOB],
			P.[DOD],
			P.[Exact_DOD]				
		FROM 
			[PersonalInformation] AS P 
		WHERE
			(P.[Doc_Code] = @Doc_Code AND P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity))
			OR 
			(@Other_Doc_Code <> '' AND P.[Doc_Code] = @Other_Doc_Code AND P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity))
	
	UNION 
	
	--Get Temp Acc
		SELECT 
			[IdentityNum] = CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])),  
			P.[Doc_Code],
			P.[DOB],  
			P.[Exact_DOB],
			P.[DOD],
			P.[Exact_DOD]
		 FROM   
		  [TempPersonalInformation] AS P  INNER JOIN [TempVoucherAccount] TVA 
				ON	P.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]
					AND	TVA.[Record_Status] not in ('D', 'V')							--Exclude removed account (D) and validated account (V)
					AND NOT (TVA.[Record_Status] = 'I' AND TVA.[Account_Purpose] = 'O') --Amendment original account 
		 WHERE  
		     (P.[Doc_Code] = @Doc_Code AND P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity))		     
		     OR
		     (@Other_Doc_Code <> '' AND P.[Doc_Code] = @Other_Doc_Code AND P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity))		     
		     

	UNION 
	
	--Get Special Acc
		SELECT  
			[IdentityNum] = CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])),  
			P.[Doc_Code],
			P.[DOB],  
			P.[Exact_DOB],
			P.[DOD],
			P.[Exact_DOD]
		 FROM   
		  [SpecialPersonalInformation] AS P  INNER JOIN [SpecialAccount] TVA 
				ON	P.[Special_Acc_ID] = TVA.[Special_Acc_ID]
					AND	TVA.[Record_Status] not in ('D', 'V')							--Exclude removed account (D) and validated account (V)
					AND NOT (TVA.[Record_Status] = 'I' AND TVA.[Account_Purpose] = 'O') --Amendment original account 
		 WHERE  
		     (P.[Doc_Code] = @Doc_Code AND P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity))		     
		     OR
		     (@Other_Doc_Code <> '' AND P.[Doc_Code] = @Other_Doc_Code AND P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity))		     
			
EXEC [proc_SymmetricKey_close]						

END
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocCodeDocID] TO WSEXT
GO
