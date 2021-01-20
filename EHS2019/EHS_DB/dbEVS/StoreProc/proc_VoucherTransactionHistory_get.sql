IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionHistory_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionHistory_get]
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
-- Created by:		Winnie SUEN
-- Created date:	29 Jan 2020
-- CR No.:			CRE19-026 (HCVS Hotline Service)
-- Description:		Get the latest transaction for eHA Enquiry (Call Centre)
-- =============================================  
 
  
CREATE PROCEDURE [dbo].[proc_VoucherTransactionHistory_get]   
	@Doc_Code		CHAR(20),  
	@identity		VARCHAR(20),  
	@Scheme_Code	CHAR(10),  
	@Subsidize_Code CHAR(10),
	@Acc_Type		CHAR(1),
	@Period_From	DATETIME,	
	@Period_To		DATETIME	-- Inclusive

AS  
BEGIN  
  
-- Performance Issue: Do not count Temporary / Special Account with status = 'D'  
  
-- =============================================  
-- Declaration  
-- =============================================  
	DECLARE @Performance_Start_Dtm DATETIME

	DECLARE @In_Doc_Code		CHAR(20)
	DECLARE @In_Identity		VARCHAR(20)
	DECLARE @In_Scheme_Code		CHAR(10)
	DECLARE @In_Subsidize_Code	CHAR(10)
	DECLARE @In_Acc_Type		CHAR(1)

	DECLARE @ServiceDtmStartYear	INT			
	DECLARE @LatestHistoryByYear	INT	
	DECLARE @CurrentDtm				Datetime
	DECLARE @ServiceDtmStart	DATETIME
	DECLARE @ServiceDtmEnd		DATETIME
	
	DECLARE @blnOtherDoc_Code	TINYINT  
	DECLARE @OtherDoc_Code		CHAR(20)  
  
	DECLARE @tblVoucherAcc Table(  
		Voucher_Acc_ID CHAR(15)  
	)  
  
	DECLARE @tblTempVoucherAcc Table(  
		Voucher_Acc_ID CHAR(15)  
	)  

	DECLARE @tblSpecialAcc Table(  
		Voucher_Acc_ID CHAR(15)  
	)  
  
	DECLARE @tblInvalidAcc Table(  
		Voucher_Acc_ID CHAR(15)  
	)  
  
	DECLARE @tblVoucherTransaction Table(  
		Transaction_ID CHAR(20)  
	)  
  
-- =============================================  
-- Validation   
-- =============================================  
	IF @Period_From IS NULL
	BEGIN
		BEGIN TRY
			SET @LatestHistoryByYear = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'VoucherTransactionHistory_LatestYear')
		END TRY

		BEGIN CATCH    	    
	
		IF @LatestHistoryByYear IS NULL
			BEGIN
				RAISERROR ('The value of "VoucherTransactionHistory_LatestYear" in DB Table [SystemParameters] is invalid.',16,1)    
				RETURN
			END
		END CATCH   
	END
	
-- =============================================  
-- Initialization  
-- ============================================= 
	SET @Performance_Start_Dtm = GETDATE() 
   
	SET @In_Doc_Code = @Doc_Code
	SET @In_Identity = @identity
	SET @In_Scheme_Code = @Scheme_Code
	SET @In_Subsidize_Code = @Subsidize_Code
	SET @In_Acc_Type = @Acc_Type

	SET @blnOtherDoc_Code = 0   
	SET @OtherDoc_Code = @In_Doc_Code   
  
	IF LTRIM(RTRIM(@In_Doc_Code)) = 'HKIC'   
		BEGIN  
			 SET @blnOtherDoc_Code = 1  
			 SET @OtherDoc_Code = 'HKBC'  
		END  
  
	IF LTRIM(RTRIM(@In_Doc_Code)) = 'HKBC'   
		BEGIN  
			 SET @blnOtherDoc_Code = 1  
			 SET @OtherDoc_Code = 'HKIC'  
		END  

	
	-- Calculate the period 
	SET @CurrentDtm = CONVERT(DATETIME,CONVERT(VARCHAR(10),GETDATE(),121))

	IF @Period_From IS NULL
	BEGIN
		SET @ServiceDtmStartYear = YEAR(DATEADD(YYYY, (@LatestHistoryByYear * -1) + 1, @CurrentDtm))
		SET @ServiceDtmStart = DATETIMEFROMPARTS(@ServiceDtmStartYear, 1, 1, 0, 0, 0, 0)		
	END
	ELSE
	BEGIN
		SET @ServiceDtmStart = @Period_From
	END

	IF @Period_To IS NULL	
		SET @ServiceDtmEnd = DATEADD(DD, 1, @CurrentDtm)
	ELSE
		SET @ServiceDtmEnd = DATEADD(DD, 1, @Period_To)
	  
-- =============================================  
-- Preparing Data 
-- =============================================  
  
	EXEC [proc_SymmetricKey_open] 
   
	-- Retrieve VoucherAccount By Identity in different PersonalInformation Tables  

	IF @In_Acc_Type IS NULL OR @In_Acc_Type = 'V'
	BEGIN
		INSERT INTO @tblVoucherAcc  
			SELECT [Voucher_Acc_ID]  
			FROM [PersonalInformation] WITH (NOLOCK) 
			WHERE 
				Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity) 
				AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
	END

	IF @In_Acc_Type IS NULL OR @In_Acc_Type = 'T'
	BEGIN     
		INSERT INTO @tblTempVoucherAcc 
			SELECT TPI.[Voucher_Acc_ID]  
			FROM [TempPersonalInformation] TPI WITH (NOLOCK) 
				INNER JOIN [TempVoucherAccount] TVA WITH (NOLOCK)  
					ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]  
			WHERE   
				Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
				AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
				--AND TVA.[Record_Status] <> 'D'
	END
 
	IF @In_Acc_Type IS NULL OR @In_Acc_Type = 'S'
	BEGIN     
		INSERT INTO @tblSpecialAcc   
			SELECT SPI.[Special_Acc_ID]  
			FROM [SpecialPersonalInformation] SPI WITH (NOLOCK)  
				INNER JOIN [SpecialAccount] SVA WITH (NOLOCK)  
					ON SPI.[Special_Acc_ID] = SVA.[Special_Acc_ID]    
			WHERE   
				Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
				AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
				--AND SVA.[Record_Status] <> 'D' 
	END
    
	IF @In_Acc_Type IS NULL OR @In_Acc_Type = 'I' 
	BEGIN 
		INSERT INTO @tblInvalidAcc  
			SELECT IPI.[Invalid_Acc_ID]  
			FROM [InvalidPersonalInformation] IPI WITH (NOLOCK)   
				INNER JOIN [InvalidAccount] IV WITH (NOLOCK)  
					ON IPI.[Invalid_Acc_ID] = IV.[Invalid_Acc_ID]  
			WHERE   
				Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
				AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
				--AND IV.[Count_Benefit] = 'Y'  
	END
  
	EXEC [proc_SymmetricKey_close]
   
	-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)  
   
	INSERT INTO @tblVoucherTransaction  
		SELECT Distinct(tmp.[Transaction_ID])  
		FROM 
			(
			SELECT VT.[Transaction_ID]  
			FROM @tblVoucherAcc tmp 
				INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
					ON VT.[Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  

			UNION  

			SELECT VT.[Transaction_ID]  
			FROM @tblTempVoucherAcc tmp 
				INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
					ON VT.[Temp_Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  

			UNION  

			SELECT VT.[Transaction_ID]  
			FROM @tblSpecialAcc tmp 
				INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
					ON VT.[Special_Acc_ID] = tmp.[Voucher_Acc_ID]  

			UNION  

			SELECT VT.[Transaction_ID]  
			FROM @tblInvalidAcc tmp 
				INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
					ON VT.[Invalid_Acc_ID] = tmp.[Voucher_Acc_ID]  
			) tmp
 
-- =============================================  
-- Return results  
-- =============================================   
    
	EXEC [proc_SymmetricKey_open]

	SELECT
		TD.[Transaction_ID],
		VT.[Service_Receive_Dtm],		  
		TD.[Scheme_Code],  
		TD.[Scheme_Seq],  
		TD.[Subsidize_Code],  
		TD.[Subsidize_Item_Code],  
		TD.[Available_item_Code],  
		TD.[Unit],  
		TD.[Per_Unit_Value],  
		TD.[Total_Amount],
		TD.[Remark],  
		TD.[ExchangeRate_Value],
		TD.[Total_Amount_RMB],
		VT.[Record_Status],		
		ISNULL(VT.Invalidation, '') AS [Invalidation],
		SD.[Status_Description] AS [Transaction_Status],
		SD2.[Status_Description] AS [Invalidation_Status],
		IIF(VT.[Record_Status] IN ('I','D','W') OR ISNULL(VT.Invalidation, '') = 'I', 'Y', 'N') AS [Cancelled],
		VT.[SP_ID],		  
		VT.[Service_Type],
		Prof.[Service_Category_Desc],  
		Prof.[Service_Category_Desc_Chi], 
		[SP_Name] = CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)),
		[SP_Name_Chi] = CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)),
		P.[Practice_Name],
		P.[Practice_Name_Chi],
		[Practice_Address] = (SELECT [dbo].[func_formatEngAddress](P.[Room], P.[Floor], P.[Block], P.[Building], P.[District])),
		[Practice_Address_Chi] = (SELECT [dbo].[func_format_Address_Chi](P.[Room], P.[Floor], P.[Block], P.Building_Chi, P.[District])),
		P.Phone_Daytime
	FROM  
		@tblVoucherTransaction tblVT 
			INNER JOIN [TransactionDetail] TD WITH (NOLOCK)
				ON tblVT.[Transaction_ID] = TD.[Transaction_ID]  
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
				ON TD.[Transaction_ID] = VT.[Transaction_ID]  
			INNER JOIN [Profession] Prof WITH (NOLOCK)
				ON LTRIM(RTRIM(VT.[Service_Type])) = Prof.[Service_Category_Code] 
			INNER JOIN [ServiceProvider] SP WITH (NOLOCK)
				ON VT.[SP_ID] = SP.[SP_ID] 
			INNER JOIN [Practice] P WITH (NOLOCK)
				ON VT.[SP_ID] = P.[SP_ID] AND VT.[Practice_Display_Seq] = P.[Display_Seq]
			INNER JOIN StatusData SD WITH (NOLOCK)
				ON SD.[Enum_Class] = 'ClaimTransStatus' AND SD.[Status_Value] = VT.[Record_Status]
			INNER JOIN StatusData SD2 WITH (NOLOCK)
				ON SD2.[Enum_Class] = 'TransactionInvalidationStatus' AND SD2.[Status_Value] = ISNULL(VT.[Invalidation],'')
	WHERE    
		((RTRIM(ISNULL(@In_Scheme_Code,'')) = '') OR (TD.[Scheme_Code] = @In_Scheme_Code)) 
		AND
		((RTRIM(ISNULL(@In_Subsidize_Code,'')) = '') OR (TD.[Subsidize_Code] = @In_Subsidize_Code)) 
		AND
		VT.Service_Receive_Dtm >= @ServiceDtmStart AND VT.Service_Receive_Dtm < @ServiceDtmEnd

	ORDER BY 
		[Service_Receive_Dtm] DESC

-- =============================================  
-- (Optional) Performance Capture 
-- =============================================  

	IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
		DECLARE @Performance_End_Dtm datetime
		SET @Performance_End_Dtm = GETDATE()
		DECLARE @Parameter varchar(255)
		SET @Parameter = @In_Doc_Code + ',' + @In_Identity + ',' + @In_Scheme_Code + ',' + @In_Subsidize_Code + ',' + @In_Acc_Type
						 	
		EXEC proc_SProcPerformance_add 'proc_VoucherTransactionHistory_get',
									   @Parameter,
									   @Performance_Start_Dtm,
									   @Performance_End_Dtm
	END         
   
   EXEC [proc_SymmetricKey_close]
END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionHistory_get] TO HCVU
GO

