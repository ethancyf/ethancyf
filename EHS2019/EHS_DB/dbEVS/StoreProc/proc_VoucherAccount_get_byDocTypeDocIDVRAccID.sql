 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			INT21-0022 (HCVU Claim Transaction Performance Tuning)
-- Modified by:		Winnie SUEN
-- Modified date:	02 Sep 2021
-- Description:		(1) Search with Raw Doc No. to handle "Search by any doc type issue"
--					(2) Fine Tune performance with adding "OPTION (RECOMPILE)"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT21-0016
-- Modified by:		Martin Tang
-- Modified date:	06 Aug 2021
-- Description:		Roll back search raw doc no. to handle search account timeout issue
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	19 July 2021
-- Description:		Fix "Search by any doc type issue"
-- =============================================
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
	@VRAccID char(15),	
	@RawIdentityNum     VARCHAR(20)
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
	DECLARE @maxrow int
	DECLARE @rowcount int
	
	declare @IdentityNum2 varchar(20)
	DECLARE @IdentityNum3	VARCHAR(20)

	Declare @SameIDChecking int	
-- =============================================
-- Initialization
-- =============================================

	SELECT	@maxrow = Parm_Value1
	FROM	dbo.SystemParameters
	WHERE	Parameter_name = 'MaxRowRetrieve' and
			Record_Status = 'A'
			
	set @IdentityNum2 = ' ' + @IdentityNum
	SET @IdentityNum3 = @RawIdentityNum;
	
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
EXEC [proc_SymmetricKey_open]
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
		CONVERT(VARCHAR(100), DecryptByKey(P.[Encrypt_Field2])) as EName,  
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
		AND (	(	(@IdentityNum = ''
						OR P.[Encrypt_Field1] = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum)
						OR P.[Encrypt_Field1] = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum2))
					AND (@Adoption_Prefix_Num = ''
						OR P.[Encrypt_Field11] = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @Adoption_Prefix_Num)))
				OR (@IdentityNum3 = ''
					OR P.[Encrypt_Field1] = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @IdentityNum3)))		
		AND (@VRAccID = '' or P.Voucher_Acc_ID=@VRAccID)
	ORDER BY
		P.[Doc_Code]
	OPTION (RECOMPILE);
	
EXEC [proc_SymmetricKey_close] 
        
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccount_get_byDocTypeDocIDVRAccID] TO HCVU
GO

