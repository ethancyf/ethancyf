IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_get_byDocCodeDocIDScheme]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_get_byDocCodeDocIDScheme]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	25 August 2016
-- CR No.:			CRE16-002
-- Description:		Change table [EqvSubsidizeMap] to view [ViewEqvSubsidizeMap]
-- =============================================
-- =============================================
-- Modification History
-- CR# : CRE13-019-02
-- Modified by:	Karl LAM
-- Modified date:	05 Jan 2015
-- Description:	Add @ExchangeRate_Value, @Total_Amount_RMB
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
-- Modified by:		Twinsen CHAN
-- Modified date:	04 Dec 2012
-- Description:		Remove parameter @Scheme_seq
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
-- Modified date: 30 Nov 2009  
-- Description:     Retrieve DOB, and arrange the open & close SYMMETRIC KEY  
-- =============================================  
-- =============================================  
-- Author:   Pak Ho LEE  
-- Create date:  01 Sep 2009  
-- Description:  Get the TransactionDetail (Benefit) By   
--     DocCode, Identity, Scheme Code, Scheme Seq  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Pak Ho LEE  
-- Modified date: 19 Sep 2009  
-- Description:     Handle Cross Dose checking  
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
  
  
CREATE PROCEDURE [dbo].[proc_TransactionDetail_get_byDocCodeDocIDScheme]   
 @Doc_Code   char(20),  
 @identity   varchar(20),  
 @Scheme_Code  char(10)  
-- ,@Scheme_seq   smallint  
AS  
BEGIN  
  
-- Performance Issue: Do not count Temporary / Special Account with status = 'D'  
  
-- =============================================  
-- Declaration  
-- =============================================  
  
DECLARE @blnOtherDoc_Code tinyint  
DECLARE @OtherDoc_Code char(20)  
  
DECLARE @tmpTempVoucherAcct Table  
(  
 Voucher_Acc_ID char(15),  
 DOB datetime,  
 Exact_DOB char(1)  
)  
  
DECLARE @tmpVoucherAcct Table  
(  
 Voucher_Acc_ID char(15),  
 DOB datetime,  
 Exact_DOB char(1)  
)  
  
DECLARE @tmpSpecialAcct Table  
(  
 Voucher_Acc_ID char(15),  
 DOB datetime,  
 Exact_DOB char(1)  
)  
  
DECLARE @tmpInvalidAcct Table  
(  
 Voucher_Acc_ID char(15),  
 DOB datetime,  
 Exact_DOB char(1)  
)  
  
DECLARE @tmpVoucherTransactionDistinct Table  
(  
 Transaction_ID char(20),  
 DOB datetime,  
 Exact_DOB char(1)  
)  
  
DECLARE @tmpVoucherTransaction Table  
(  
 Transaction_ID char(20),  
 DOB datetime,  
 Exact_DOB char(1),  
 Type tinyint  
)  
  
  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
   
SET @blnOtherDoc_Code = 0   
SET @OtherDoc_Code = @Doc_Code   
  
IF LTRIM(RTRIM(@Doc_Code)) = 'HKIC'   
BEGIN  
 Set @blnOtherDoc_Code = 1  
 Set @OtherDoc_Code = 'HKBC'  
END  
  
IF LTRIM(RTRIM(@Doc_Code)) = 'HKBC'   
BEGIN  
 Set @blnOtherDoc_Code = 1  
 Set @OtherDoc_Code = 'HKIC'  
END  
  
-- =============================================  
-- Return results  
-- =============================================  
  
EXEC [proc_SymmetricKey_open]
   
-- Retrieve VoucherAccount By Identity in different PersonalInformation Tables  
  
 INSERT INTO @tmpVoucherAcct  
 SELECT [Voucher_Acc_ID], [DOB], [Exact_DOB]  
 FROM [PersonalInformation]  
 WHERE Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND  
   (  
    [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code)  
   )  
     
 INSERT INTO @tmpTempVoucherAcct   
 SELECT TPI.[Voucher_Acc_ID], TPI.[DOB], TPI.[Exact_DOB]  
 FROM [TempPersonalInformation] TPI  
  INNER JOIN [TempVoucherAccount] TVA  
   ON TPI.[Voucher_Acc_ID] = TVA.[Voucher_Acc_ID]  
 WHERE   
   Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND  
   (  
    [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code)  
   )  
   AND TVA.[Record_Status] <> 'D'  
     
 INSERT INTO @tmpSpecialAcct   
 SELECT SPI.[Special_Acc_ID], SPI.[DOB], SPI.[Exact_DOB]  
 FROM [SpecialPersonalInformation] SPI  
  INNER JOIN [SpecialAccount] SVA  
   ON SPI.[Special_Acc_ID] = SVA.[Special_Acc_ID]    
 WHERE   
   Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND  
   (  
    [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code)  
   )  
   AND SVA.[Record_Status] <> 'D'  
     
 INSERT INTO @tmpInvalidAcct  
 SELECT IPI.[Invalid_Acc_ID], IPI.[DOB], IPI.[Exact_DOB]  
 FROM [InvalidPersonalInformation] IPI   
  INNER JOIN [InvalidAccount] IV  
   ON IPI.[Invalid_Acc_ID] = IV.[Invalid_Acc_ID]  
 WHERE   
   Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) AND  
   (  
    [Doc_Code] = @Doc_Code OR (@blnOtherDoc_Code = 1 AND [Doc_Code] = @OtherDoc_Code)  
   )   
   AND IV.[Count_Benefit] = 'Y'  
     
  
EXEC [proc_SymmetricKey_close]  
  
-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)  
   
 INSERT INTO @tmpVoucherTransaction  
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB], 1  
 FROM  
  @tmpVoucherAcct tmp INNER JOIN [VoucherTransaction] VT  
   ON VT.[Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  
 WHERE  
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null  
   
 UNION   
 --INSERT INTO @tmpVoucherTransaction  
 SELECT  VT.[Transaction_ID], [DOB], [Exact_DOB], 3  
 FROM  
  @tmpTempVoucherAcct tmp INNER JOIN [VoucherTransaction] VT  
   ON VT.[Temp_Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]  
 WHERE  
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null  
      
 UNION     
 --INSERT INTO @tmpVoucherTransaction  
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB], 2  
 FROM  
  @tmpSpecialAcct tmp INNER JOIN [VoucherTransaction] VT  
   ON VT.[Special_Acc_ID] = tmp.[Voucher_Acc_ID]  
 WHERE  
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null  
    
 UNION  
 --INSERT INTO @tmpVoucherTransaction  
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB], 4   
 FROM  
  @tmpInvalidAcct tmp INNER JOIN [VoucherTransaction] VT  
   ON VT.[Invalid_Acc_ID] = tmp.[Voucher_Acc_ID]  
 WHERE  
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D'    
      
  
-- Distinct the Transaction   
  
 INSERT INTO @tmpVoucherTransactionDistinct  
 SELECT [Transaction_ID], [DOB], [Exact_DOB] FROM @tmpVoucherTransaction as tmpV WHERE  
 EXISTS  
 (  
  SELECT [Transaction_ID] FROM  
  (  
   SELECT [Transaction_ID] , Min(Type) Type FROM @tmpVoucherTransaction GROUP BY Transaction_ID  
  ) tmp  
  WHERE tmp.[Transaction_ID] = tmpV.[Transaction_ID] AND tmp.[Type] = tmpV.[Type]  
 )  
    
   
 /*  
 INSERT INTO @tmpVoucherTransactionDistinct  
 SELECT Distinct [Transaction_ID], [DOB], [Exact_DOB]   
 FROM  
  @tmpVoucherTransaction  
 */  
    
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
  tmp.[DOB],  
  tmp.[Exact_DOB]  
 FROM  
  @tmpVoucherTransactionDistinct tmp  
  INNER JOIN [TransactionDetail] TD  
   ON tmp.[Transaction_ID] = TD.[Transaction_ID]  
  INNER JOIN [VoucherTransaction] VT   
   ON TD.[Transaction_ID] = VT.[Transaction_ID]  
 WHERE  
  TD.[Scheme_Code] = @Scheme_Code
  OR  
  EXISTS (  
   SELECT [Scheme_Code] FROM [ViewEqvSubsidizeMap] ESM WHERE   
    ESM.[Scheme_Code] = @Scheme_Code AND   
    ESM.[Eqv_Scheme_Code] = TD.[Scheme_Code] AND  
    ESM.[Eqv_Scheme_Seq] = TD.[Scheme_Seq] AND  
    ESM.[Eqv_Subsidize_Item_Code] = TD.[Subsidize_Item_Code]  
  )  
  
   
END  
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDScheme] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDScheme] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocIDScheme] TO WSEXT
GO