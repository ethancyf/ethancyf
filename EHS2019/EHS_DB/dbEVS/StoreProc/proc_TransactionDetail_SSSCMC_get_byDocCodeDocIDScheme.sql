IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme]
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
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	21 Oct 2020
-- CR No.:			CRE20-015
-- Description:		HA Scheme - Select all subsidies (HAS_A, HAS_B) of SSSCMC for entitlement checking
-- ============================================= 
  
CREATE PROCEDURE [dbo].[proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme]   
	@Doc_Code		CHAR(20),  
	@identity		VARCHAR(20),  
	@Scheme_Code	CHAR(10)  
AS  
BEGIN  
  
-- =============================================  
-- Declaration  
-- =============================================  
  
	DECLARE @blnOtherDoc_Code TINYINT  

	DECLARE @OtherDoc_Code table (
		Doc_Code	char(20)
	)
  
	DECLARE @tmpTempVoucherAcct Table(  
		Voucher_Acc_ID	CHAR(15),  
		DOB				DATETIME,  
		Exact_DOB		CHAR(1)  
	)  
  
	DECLARE @tmpVoucherAcct Table(  
		Voucher_Acc_ID	CHAR(15),  
		DOB				DATETIME,  
		Exact_DOB		CHAR(1)  
	)  
  
	DECLARE @tmpSpecialAcct Table(  
		Voucher_Acc_ID	CHAR(15),  
		DOB				DATETIME,  
		Exact_DOB		CHAR(1)  
	)  
  
	DECLARE @tmpVoucherTransactionDistinct Table(  
		Transaction_ID	CHAR(20)  
		--,DOB			DATETIME  
		--,Exact_DOB		CHAR(1)  
	)  
  
	DECLARE @tmpVoucherTransaction Table(  
		Transaction_ID	CHAR(20),  
		DOB				DATETIME,  
		Exact_DOB		CHAR(1),  
		TypeOfTran		TINYINT  
	)  
 
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
   
	SET @blnOtherDoc_Code = 0   
  
	IF LTRIM(RTRIM(@Doc_Code)) = 'HKIC' 
	BEGIN
		Set @blnOtherDoc_Code = 1
		INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKBC')
		INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('EC')
	END

	IF LTRIM(RTRIM(@Doc_Code)) = 'HKBC' 
	BEGIN
		Set @blnOtherDoc_Code = 1
		INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKIC')
		INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('EC')
	END

	IF LTRIM(RTRIM(@Doc_Code)) = 'EC' 
	BEGIN
		Set @blnOtherDoc_Code = 1
		INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKIC')
		INSERT INTO @OtherDoc_Code (Doc_Code) VALUES ('HKBC')
	END

	-- Retrieve VoucherAccount By Identity in different PersonalInformation Tables  
	EXEC [proc_SymmetricKey_open]
   
	-- Validated Account
	INSERT INTO @tmpVoucherAcct  
	SELECT 
		[Voucher_Acc_ID], [DOB], [Exact_DOB]  
	FROM 
		[PersonalInformation] WITH (NOLOCK)
	WHERE 
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity)
		AND ( [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code) ) )  
     
	-- Temporary Account
	INSERT INTO @tmpTempVoucherAcct
	SELECT 
		TPI.[Voucher_Acc_ID], TPI.[DOB], TPI.[Exact_DOB]  
	FROM 
		[TempPersonalInformation] TPI WITH (NOLOCK)
			INNER JOIN [TempVoucherAccount] TVA WITH (NOLOCK)
				ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]  
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) 
		AND ( [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code) ) )
		AND TVA.[Record_Status] <> 'D'  
     
	-- Special Account
	INSERT INTO @tmpSpecialAcct   
	SELECT 
		SPI.[Special_Acc_ID], SPI.[DOB], SPI.[Exact_DOB]  
	FROM 
		[SpecialPersonalInformation] SPI WITH (NOLOCK)
			INNER JOIN [SpecialAccount] SVA WITH (NOLOCK)
				ON SPI.[Special_Acc_ID] = SVA.[Special_Acc_ID]    
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) 
		AND ( [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] IN (SELECT Doc_Code FROM @OtherDoc_Code) ) )
		AND SVA.[Record_Status] <> 'D'  

     
	EXEC [proc_SymmetricKey_close]
  

	-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)  
   
	INSERT INTO @tmpVoucherTransaction  
	SELECT 
		VT.[Transaction_ID], [DOB], [Exact_DOB], 1  
	FROM  
		@tmpVoucherAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)  
				ON VT.[Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  
	WHERE  
		--VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null  
		NOT EXISTS(SELECT 1 FROM [VoucherTransaction] WITH(NOLOCK) WHERE [Record_Status] IN ('I','D') AND VT.Transaction_ID = Transaction_ID)
		AND VT.[Invalid_acc_id] IS NULL
   
	UNION   
	--INSERT INTO @tmpVoucherTransaction  
	SELECT  
		VT.[Transaction_ID], [DOB], [Exact_DOB], 3  
	FROM  
		@tmpTempVoucherAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)  
				ON VT.[Temp_Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  
	WHERE  
		--VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null  
		NOT EXISTS(SELECT 1 FROM [VoucherTransaction] WITH(NOLOCK) WHERE [Record_Status] IN ('I','D') AND VT.Transaction_ID = Transaction_ID)
		AND VT.[Invalid_acc_id] IS NULL
		
	UNION     
	--INSERT INTO @tmpVoucherTransaction  
	SELECT 
		VT.[Transaction_ID], [DOB], [Exact_DOB], 2  
	FROM  
		@tmpSpecialAcct tmp INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)  
			ON VT.[Special_Acc_ID] = tmp.[Voucher_Acc_ID]  
	WHERE  
		--VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null  
		NOT EXISTS(SELECT 1 FROM [VoucherTransaction] WITH(NOLOCK) WHERE [Record_Status] IN ('I','D') AND VT.Transaction_ID = Transaction_ID)
		AND VT.[Invalid_acc_id] IS NULL
    
	     
  
	-- Distinct the Transaction   
  
	INSERT INTO @tmpVoucherTransactionDistinct  
	SELECT DISTINCT
		[Transaction_ID]
	FROM 
		@tmpVoucherTransaction as tmpV 
	WHERE  
		EXISTS(  
			SELECT 
				[Transaction_ID] 
			FROM  
				(SELECT [Transaction_ID] , Min(TypeOfTran) AS TypeOfTran FROM @tmpVoucherTransaction GROUP BY Transaction_ID) tmp  
			WHERE 
				tmp.[Transaction_ID] = tmpV.[Transaction_ID] 
				AND tmp.TypeOfTran = tmpV.TypeOfTran  
		)  
     
-- =============================================  
-- Return results  
-- =============================================    

	SELECT  
		TD.[Transaction_ID],  
		TD.[Scheme_Code],  
		TD.[Scheme_Seq],  
		TD.[Subsidize_Code],  
		TD.[Subsidize_Item_Code],  
		TD.[Available_item_Code],  
		TD.[Unit],  
		TD.[Per_Unit_Value],  
		TD.[Total_Amount],  
		'' as [Remark],  
		TD.[ExchangeRate_Value],
		TD.[Total_Amount_RMB],
		'' as [Available_Item_Desc],  
		'' as [Available_Item_Desc_Chi],  
		'' as [Available_Item_Desc_CN],  
		VT.[Service_Receive_Dtm],  
		[DOB] = NULL,  
		[Exact_DOB] = NULL  
	FROM  
		@tmpVoucherTransactionDistinct tmp  
			INNER JOIN [TransactionDetail] TD WITH (NOLOCK)  
				ON tmp.[Transaction_ID] = TD.[Transaction_ID]  
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)   
				ON TD.[Transaction_ID] = VT.[Transaction_ID]  
	WHERE  
		TD.[Scheme_Code] = @Scheme_Code
 
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_SSSCMC_get_byDocCodeDocIDScheme] TO WSEXT
GO