IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RelatedWriteOffAccount_get_byDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RelatedWriteOffAccount_get_byDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:	
-- Description:	    
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		11 Dec 2017
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Description:		Retrieve personal information by Doc ID 
-- =============================================

--exec proc_RelatedWriteOffAccount_get_byDocID ' A1111119'

CREATE PROCEDURE [dbo].[proc_RelatedWriteOffAccount_get_byDocID]
	@identity varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Process
-- =============================================

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
			P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity)
	
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
		     P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity)		     		     
		     
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
		     P.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @identity)	     
   			
EXEC [proc_SymmetricKey_close]				

END
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocID] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_RelatedWriteOffAccount_get_byDocID] TO WSEXT
GO