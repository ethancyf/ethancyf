IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Koala CHENG
-- Modified date:	22 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	2 May 2019
-- CR No.:			CRE18-021 (Voucher balance Enquiry show forfeited)
-- Description:		Add output [Service_Type]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	29 Nov 2018
-- CR No.:			CRE19-003 (Opt voucher capping)
-- Description:		Add input parm [Period_From], [Period_To], [Service_Category_Code]
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			I-CRE17-007
-- Modified by:		Chris YIM
-- Modified date:	21 February 2018
-- Description:		Tune Performance
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2018
-- CR No.:			INT17-0020
-- Description:		Add NOLOCK to calculate voucher entitlement
-- =============================================
-- =============================================
-- Modification History
-- CR# : CRE13-019-02
-- Modified by:	Karl LAM
-- Modified date:	05 Jan 2015 (promote after CRE14-019)
-- Description:	1. Add @ExchangeRate_Value, @Total_Amount_RMB
--				2. Allow empty scheme code/subsidize code
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRP12-007
-- Modified by:		Koala CHENG
-- Modified date:	08 Jan 2013
-- Description:		Performance Tuning
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:     Derek LEUNG  
-- Modified date: 5 Aug 2010  
-- Description:     Exclude VT.record_status = 'D' (removed from backoffice) records  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Pak Ho LEE  
-- Modified date: 14 May 2010  
-- Description:     Performance Tuning 3: Union  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Paul Yip   
-- Modified date: 29 Mar 2010    
-- Description:     Do not count those transactions linked with invalid accounts   
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Pak Ho LEE  
-- Modified date: 07 Dec 2009  
-- Description:     Arrange the open & close SYMMETRIC KEY  
-- =============================================  
-- =============================================  
-- Author:   Pak Ho LEE  
-- Create date:  01 Sep 2009  
-- Description:  Get the TransactionDetail (Benefit) By   
--     DocCode, Identity, Scheme Code, SubsidizeCode  
--     Mainly for Voucher Carry Forward   
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 21 September 2009  
-- Description:  Grant execute to HCPUBLIC  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Pak Ho LEE  
-- Modified date: 7 Oct 2009  
-- Description:     Performance Tuning  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Pak Ho LEE  
-- Modified date: 23 Oct 2009  
-- Description:     Performance Tuning 2  
-- =============================================  
  
  
CREATE PROCEDURE [dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll]   
	@Doc_Code		CHAR(20),  
	@identity		VARCHAR(20),  
	@Scheme_Code	CHAR(10),  
	@Subsidize_Code CHAR(10),
	@Period_From	DATETIME,
	@Period_To		DATETIME,
	@Service_Category_Code	VARCHAR(3)
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

DECLARE @In_Period_From		DATETIME
DECLARE @In_Period_To		DATETIME
DECLARE @In_Service_Category_Code	VARCHAR(3)
		
DECLARE @blnOtherDoc_Code	TINYINT  
DECLARE @OtherDoc_Code		CHAR(20)  
  
DECLARE @tmpTempVoucherAcct Table(  
	Voucher_Acc_ID CHAR(15)  
)  
  
DECLARE @tmpVoucherAcct Table(  
	Voucher_Acc_ID CHAR(15)  
)  
  
DECLARE @tmpSpecialAcct Table(  
	Voucher_Acc_ID CHAR(15)  
)  
  
DECLARE @tmpInvalidAcct Table(  
	Voucher_Acc_ID CHAR(15)  
)  
  
DECLARE @tmpVoucherTransaction Table(  
	Transaction_ID CHAR(20)  
)  
  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- ============================================= 
SET @Performance_Start_Dtm = GETDATE() 
   
SET @In_Doc_Code = @Doc_Code
SET @In_Identity = @identity
SET @In_Scheme_Code = @Scheme_Code
SET @In_Subsidize_Code = @Subsidize_Code

SET @In_Period_From	= @Period_From
SET @In_Period_To = @Period_To
SET @In_Service_Category_Code = @Service_Category_Code

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
  
-- =============================================  
-- Return results  
-- =============================================  
  
EXEC [proc_SymmetricKey_open]
   
-- Retrieve VoucherAccount By Identity in different PersonalInformation Tables  
   
INSERT INTO @tmpVoucherAcct  
	SELECT [Voucher_Acc_ID]  
	FROM [PersonalInformation] WITH (NOLOCK) 
	WHERE 
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity) 
		AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
     
INSERT INTO @tmpTempVoucherAcct   
	SELECT TPI.[Voucher_Acc_ID]  
	FROM [TempPersonalInformation] TPI WITH (NOLOCK) 
		INNER JOIN [TempVoucherAccount] TVA WITH (NOLOCK)  
			ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]  
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
		AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
		AND TVA.[Record_Status] <> 'D'  
     
INSERT INTO @tmpSpecialAcct   
	SELECT SPI.[Special_Acc_ID]  
	FROM [SpecialPersonalInformation] SPI WITH (NOLOCK)  
		INNER JOIN [SpecialAccount] SVA WITH (NOLOCK)  
			ON SPI.[Special_Acc_ID] = SVA.[Special_Acc_ID]    
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
		AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
		AND SVA.[Record_Status] <> 'D'  
     
INSERT INTO @tmpInvalidAcct  
	SELECT IPI.[Invalid_Acc_ID]  
	FROM [InvalidPersonalInformation] IPI WITH (NOLOCK)   
		INNER JOIN [InvalidAccount] IV WITH (NOLOCK)  
			ON IPI.[Invalid_Acc_ID] = IV.[Invalid_Acc_ID]  
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @In_Identity)
		AND ([Doc_Code] = @In_Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code))  
		AND IV.[Count_Benefit] = 'Y'  
  
EXEC [proc_SymmetricKey_close] 
   
-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)  
   
INSERT INTO @tmpVoucherTransaction  
	SELECT Distinct(tmp.[Transaction_ID])  
	FROM 
		(
		SELECT VT.[Transaction_ID]  
		FROM @tmpVoucherAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
				ON VT.[Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  
		WHERE  
			VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] IS NULL  
   
		UNION  

		SELECT VT.[Transaction_ID]  
		FROM @tmpTempVoucherAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
				ON VT.[Temp_Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  
		WHERE  
			VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] IS NULL
      
		UNION  

		SELECT VT.[Transaction_ID]  
		FROM @tmpSpecialAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
				ON VT.[Special_Acc_ID] = tmp.[Voucher_Acc_ID]  
		WHERE  
			VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] IS NULL 
    
		UNION  

		SELECT VT.[Transaction_ID]  
		FROM @tmpInvalidAcct tmp 
			INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
				ON VT.[Invalid_Acc_ID] = tmp.[Voucher_Acc_ID]  
		WHERE  
			VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D'   
		) tmp
    
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
	'' AS [Remark],  
	TD.[ExchangeRate_Value],
	TD.[Total_Amount_RMB],
	'' AS [Available_Item_Desc],  
	'' AS [Available_Item_Desc_Chi],  
	'' AS [Available_Item_Desc_CN],  
	VT.[Service_Receive_Dtm],
	VT.[Service_Type]
FROM  
	@tmpVoucherTransaction tmp 
		INNER JOIN [TransactionDetail] TD WITH (NOLOCK)
			ON tmp.[Transaction_ID] = TD.[Transaction_ID]  
		INNER JOIN [VoucherTransaction] VT WITH (NOLOCK)
			ON TD.[Transaction_ID] = VT.[Transaction_ID]  
WHERE    
	((RTRIM(ISNULL(@In_Scheme_Code,'')) = '') OR (TD.[Scheme_Code] = @In_Scheme_Code)) 
	AND
	((RTRIM(ISNULL(@In_Subsidize_Code,'')) = '') OR (TD.[Subsidize_Code] = @In_Subsidize_Code)) 
	AND
	(@In_Period_From IS NULL OR (VT.[Service_Receive_Dtm] >= @In_Period_From)) 
	AND
	(@In_Period_To IS NULL OR (VT.[Service_Receive_Dtm] < @In_Period_To)) 
	AND
	((RTRIM(ISNULL(@In_Service_Category_Code,'')) = '') OR (VT.[Service_Type] = @In_Service_Category_Code)) 

ORDER BY 
	Scheme_Seq, Scheme_Code



IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
	DECLARE @Performance_End_Dtm datetime
	SET @Performance_End_Dtm = GETDATE()
	DECLARE @Parameter varchar(255)
	SET @Parameter = @In_Doc_Code + ',' + @In_Identity + ',' + @In_Scheme_Code + ',' + @In_Subsidize_Code
					 + ',' + @In_Period_From + ',' + @In_Period_To + ',' + @In_Service_Category_Code
	
	EXEC proc_SProcPerformance_add 'proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll',
								   @Parameter,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
END         
   
END  
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDSchemeSubsidizeAll] TO WSEXT
Go