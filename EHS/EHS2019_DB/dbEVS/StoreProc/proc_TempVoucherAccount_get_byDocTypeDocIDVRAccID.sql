﻿IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_get_byDocTypeDocIDVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_get_byDocTypeDocIDVRAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		11 October 2018
-- CR No.:			CRE17-018-07
-- Description:		Add Column "Record_Status"
--							   "Create_By]"
--							   "Create_Dtm]"
--							   "SP_Practice_Display_Seq"
--							   "Create_By_BO"
--							   "With_Transaction"
--							   "Account_Source"
--							   "Account_Type"
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_get_byDocTypeDocIDVRAccID]
	@IdentityNum		VARCHAR(20),
	@AdoptionPrefixNum	CHAR(7),
	@DocCode			CHAR(20),
	@VoucherAccID		CHAR(15)
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
	DECLARE @maxrow			INT
	DECLARE @rowcount		INT
	
	DECLARE @IdentityNum2	VARCHAR(20)
	DECLARE @SameIDChecking INT	
-- =============================================
-- Initialization
-- =============================================

	SELECT	@maxrow = Parm_Value1
	FROM	dbo.SystemParameters
	WHERE	Parameter_name = 'MaxRowRetrieve' and
			Record_Status = 'A'
			
	SET @IdentityNum2 = ' ' + @IdentityNum

		set @SameIDChecking = 0
	
	IF @DocCode = 'HKIC'
		BEGIN
			SET @SameIDChecking = 1
		END
	ELSE IF @DocCode = 'HKBC'
		BEGIN
			SET @SameIDChecking = 1
		END

	SET NOCOUNT ON;
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
		TP.[Voucher_Acc_ID],  
		TP.[DOB],  
		TP.[Exact_DOB],  
		TP.[Sex],  
		TP.[Date_of_Issue],  
  
		CONVERT(VARCHAR, DecryptByKey(TP.[Encrypt_Field1])) AS IdentityNum,  
		CONVERT(VARCHAR(40), DecryptByKey(TP.[Encrypt_Field2])) AS EName,  
		CONVERT(NVARCHAR, DecryptByKey(TP.[Encrypt_Field3])) AS CName,     
     
		TP.[EC_Age],  
		TP.[EC_Date_of_Registration],  
		TP.[Doc_Code],  
  
		ISNULL(CONVERT(VARCHAR, DecryptByKey(TP.[Encrypt_Field11])),'') AS AdoptionPrefixNum,  
		TP.[Other_Info],
		TVA.[Record_Status],
		VACL.[Create_By],
		VACL.[Create_Dtm],
		VACL.SP_Practice_Display_Seq AS SP_Practice_Display_Seq,
		TVA.[Create_By_BO],
		[With_Transaction] = Case 
			WHEN (SELECT COUNT(1) FROM VoucherTransaction WITH (NOLOCK) WHERE Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID AND Record_Status NOT IN ('I','D')) > 0 THEN 'Y'
			ELSE 'N'
		END,
		'T' AS [Account_Source],
		'Temporary' AS [Account_Type]
	FROM   
		[TempPersonalInformation] TP WITH (NOLOCK)
			INNER JOIN [TempVoucherAccount] TVA WITH (NOLOCK)
				ON TP.Voucher_Acc_ID = TVA.Voucher_Acc_ID  
			INNER JOIN [VoucherAccountCreationLOG] VACL WITH (NOLOCK)
				ON TP.Voucher_Acc_ID = VACL.Voucher_Acc_ID
	WHERE  
		(@IdentityNum = '' OR TP.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum) OR TP.[Encrypt_Field1] = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))
		AND (@AdoptionPrefixNum = '' OR TP.[Encrypt_Field11] = EncryptByKey(KEY_GUID('sym_Key'), @AdoptionPrefixNum))
		AND (@DocCode = '' OR TP.[Doc_Code] = @DocCode OR (@SameIDChecking = 1 AND TP.[Doc_Code] in ('HKIC', 'HKBC')))
		AND (@VoucherAccID = '' OR TP.[Voucher_Acc_ID] = @VoucherAccID)
		AND TVA.[Record_Status] NOT IN ('D','V')
	ORDER BY
		TP.[Doc_Code]
	
	CLOSE SYMMETRIC KEY sym_Key  
        
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_get_byDocTypeDocIDVRAccID] TO HCVU
GO
