﻿ IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	11 October 2018
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
-- =============================================
-- Author:		Kathy LEE
-- Create date: 5 July 2010
-- Description:	Retrieve the validated Voucher Account by using Doc Type,
--				Identity Num or Voucher Account ID
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID]
	-- Add the parameters for the stored procedure here
	@doc_Code char(20),
	@IdentityNum varchar(20),
	@Adoption_Prefix_Num char(7),
	@VRAccID char(15)
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
	DECLARE @maxrow int
	DECLARE @rowcount int
	
	declare @IdentityNum2 varchar(20)
	Declare @SameIDChecking int	
-- =============================================
-- Initialization
-- =============================================

	SELECT	@maxrow = Parm_Value1
	FROM	dbo.SystemParameters
	WHERE	Parameter_name = 'MaxRowRetrieve' and
			Record_Status = 'A'
			
	set @IdentityNum2 = ' ' + @IdentityNum
	
	set @SameIDChecking = 0
	
	if @doc_Code = 'HKIC'
	begin
		set @SameIDChecking = 1
	end
	else if @doc_code = 'HKBC'
	begin
		set @SameIDChecking = 1
	end

	SET NOCOUNT ON;
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
  
	SELECT  
		P.[Voucher_Acc_ID],  
		[DOB],  
		[Exact_DOB],  
		[Sex],  
		[Date_of_Issue],  
  
		CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field1])) as IdentityNum,  
		CONVERT(VARCHAR(40), DecryptByKey(P.[Encrypt_Field2])) as EName,  
		CONVERT(NVARCHAR, DecryptByKey(P.[Encrypt_Field3])) as CName,     
     
		[EC_Age],  
		[EC_Date_of_Registration],  
		[Doc_Code],  
  
		ISNULL(CONVERT(VARCHAR, DecryptByKey(P.[Encrypt_Field11])),'') as AdoptionPrefixNum,  
		[Other_Info],  
		VA.[Record_Status],
		VACL.[Create_By],
		VACL.[Create_Dtm],
		VACL.SP_Practice_Display_Seq AS SP_Practice_Display_Seq,
		VACL.[Create_By_BO],
		'N' AS [With_Transaction],
		'V' AS [Account_Source],
		'Validated' AS [Account_Type]
	FROM   
		[PersonalInformation] AS P WITH (NOLOCK)
			INNER JOIN [VoucherAccount] VA WITH (NOLOCK)
				ON P.Voucher_Acc_ID = VA.Voucher_Acc_ID
			INNER JOIN [VoucherAccountCreationLOG] VACL WITH (NOLOCK)
				ON P.Voucher_Acc_ID = VACL.Voucher_Acc_ID
	WHERE  
		(@doc_code = '' or P.[Doc_Code] = @doc_code or (@SameIDChecking = 1 and P.[Doc_Code] in ('HKIC', 'HKBC')))
		AND (@IdentityNum = '' or P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum)
			or P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNum2))
		AND (@Adoption_Prefix_Num = '' or P.Encrypt_Field11 = EncryptByKey(KEY_GUID('sym_Key'), @Adoption_Prefix_Num))
		AND (@VRAccID = '' or P.Voucher_Acc_ID=@VRAccID)
	ORDER BY
		P.[Doc_Code]
	
CLOSE SYMMETRIC KEY sym_Key  
        
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID] TO HCVU
GO
