IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_get_byDocCodeDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_get_byDocCodeDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR# :		
-- Modified by:	Koala CHENG
-- Modified date:	09 Oct 2020
-- Description:	Fine tune
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
-- CR No:			CRE13-001
-- Modified by:		Karl LAM
-- Modified date:	9 Apr 2013
-- Description:		Match the column definition of the result temp table variable @result ([Subsidize_Item_Code], [Available_item_Code_Desc]) to real table
--					Bug fix
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	4 Jan 2012
-- Description:		Remove joining by SchemeClaim.Scheme_Seq
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-01 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Koala CHENG
-- Modified date:	20 Jun 2012
-- Description:		Modify to return more column for build object in code behide
--					Grant permission to WSEXT
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	20 August 2010
-- Description:	    Fix the problem to get the "Subsidize Item Code"
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:     Derek LEUNG  
-- Modified date: 5 Aug 2010  
-- Description:     Exclude VT.record_status = 'D' (removed from backoffice) records  
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:	14 May 2010
-- Description:	    Performance Tuning 3: Union
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Paul Yip	
-- Modified date:	29 Mar 2010		
-- Description:	    Do not count those transactions linked with invalid accounts 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:	07 Dec 2009
-- Description:	    Arrange the open & close SYMMETRIC KEY
-- =============================================
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		01 Sep 2009
-- Description:		Get the TransactionDetail (Benefit) By 
--					DocCode, Identity For VU Transaction Detail Display
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:	7 Oct 2009
-- Description:	    Performance Tuning
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:	23 Oct 2009
-- Description:	    Performance Tuning 2
-- =============================================

CREATE PROCEDURE [dbo].[proc_TransactionDetail_get_byDocCodeDocID]     
 @Doc_Code   char(20),    
 @identity   varchar(20)  
WITH RECOMPILE
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
 Exact_DOB char(1)      
)    
    
    
declare @result table    
(    
 [Service_Receive_Dtm] datetime,    
 [Scheme_Code] char(10),    
 [Display_Code] char(25),    
 [Scheme_Seq] smallint,    
 [Subsidize_Code] char(10),    
 [Subsidize_Item_Code] char(25),    
 [Available_item_Code] char(20),    
 [Available_item_Code_Desc] varchar(100),    
 [Unit] smallint,    
 [Source] char(10),    
 [Transaction_ID] char(20),  
 [Per_Unit_Value] money,      
 [Total_Amount] money,     
 [DOB] datetime, 
 [Exact_DOB] char(1),  
 [Remark] nvarchar(255),
 [ExchangeRate_Value] decimal(9,3),      
 [Total_Amount_RMB] money,
 [Available_Item_Desc] varchar(100),      
 [Available_Item_Desc_Chi] nvarchar(100),
 [Available_Item_Desc_CN] nvarchar(100),
 [Display_Code_For_Claim] varchar(25)   
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
    
    
OPEN SYMMETRIC KEY sym_Key     
 DECRYPTION BY ASYMMETRIC KEY asym_Key    
     
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
       
CLOSE SYMMETRIC KEY sym_Key    
    
-- Retrieve Transaction Related to the [Voucher_Acc_ID](s)    
    
 INSERT INTO @tmpVoucherTransaction     
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB]    
 FROM    
  @tmpVoucherAcct tmp INNER JOIN [VoucherTransaction] VT    
   ON VT.[Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]    
 WHERE    
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null    
     
 UNION      
 --INSERT INTO @tmpVoucherTransaction    
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB]    
 FROM    
  @tmpTempVoucherAcct tmp INNER JOIN [VoucherTransaction] VT    
   ON VT.[Temp_Voucher_Acc_ID] = tmp.[Voucher_Acc_ID]    
 WHERE    
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null    
        
 UNION    
 --INSERT INTO @tmpVoucherTransaction    
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB]    
 FROM    
  @tmpSpecialAcct tmp INNER JOIN [VoucherTransaction] VT    
   ON VT.[Special_Acc_ID] = tmp.[Voucher_Acc_ID]    
 WHERE    
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D' AND VT.[Invalid_acc_id] is null    
     
 UNION    
 --INSERT INTO @tmpVoucherTransaction    
 SELECT VT.[Transaction_ID], [DOB], [Exact_DOB]    
 FROM    
  @tmpInvalidAcct tmp INNER JOIN [VoucherTransaction] VT    
   ON VT.[Invalid_Acc_ID] = tmp.[Voucher_Acc_ID]    
 WHERE    
  VT.[Record_Status] <> 'I' AND VT.[Record_Status] <> 'W' AND VT.[Record_Status] <> 'D'     
      
-- Distinct the Transaction     
    
 INSERT INTO @tmpVoucherTransactionDistinct    
 SELECT Distinct([Transaction_ID]), [DOB], [Exact_DOB]    
 FROM    
  @tmpVoucherTransaction    
        
 insert into @result    
 (    
  [Service_Receive_Dtm],    
  [Scheme_Code],    
  [Scheme_Seq],    
  [Subsidize_Code],    
  [Subsidize_Item_Code],    
  [Available_item_Code],    
 [Unit],    
  [Source],  
  [Transaction_ID],  
  [Per_Unit_Value],      
  [Total_Amount],   
  [DOB],
  [Exact_DOB],    
  [Remark],    
  [ExchangeRate_Value],      
  [Total_Amount_RMB] ,
  [Available_Item_Desc],      
  [Available_Item_Desc_Chi],
  [Available_Item_Desc_CN]
 )    
 SELECT    
  --Distinct    
  VT.[Service_Receive_Dtm],    
  TD.[Scheme_Code],    
  TD.[Scheme_Seq],    
  TD.[Subsidize_Code],    
  TD.[Subsidize_Item_Code],    
  TD.[Available_item_Code],    
  TD.[Unit],    
  TD.[Scheme_Code],    
  TD.[Transaction_ID],  
  TD.[Per_Unit_Value],      
  TD.[Total_Amount],
  tmp.[DOB],    
  tmp.[Exact_DOB],   
  '' as [Remark],   
 TD.[ExchangeRate_Value],      
 TD.[Total_Amount_RMB],   
  '' as [Available_Item_Desc],      
  '' as [Available_Item_Desc_Chi],
  '' as [Available_Item_Desc_CN] 
 FROM    
  @tmpVoucherTransactionDistinct tmp     
   INNER JOIN [TransactionDetail] TD     
    ON tmp.[Transaction_ID] = TD.[Transaction_ID]    
   INNER JOIN [VoucherTransaction] VT     
    ON TD.[Transaction_ID] = VT.[Transaction_ID]       
    
         
 update @Result    
 set [Display_Code] = SC.Display_Code    
 from @Result R, SchemeClaim SC    
 where R.Scheme_Code = SC.Scheme_Code    
-- and R.scheme_seq = SC.scheme_seq    
     
 update @Result    
 set [Source] = SC.Display_Code    
 from @Result R, SchemeClaim SC    
 where R.[Source] = SC.Scheme_Code    
-- and R.scheme_seq = SC.scheme_seq    

 update @Result  
 set [Available_item_Desc] = sid.available_Item_Desc  ,  
  [Available_item_Desc_Chi] =  sid.available_Item_Desc_Chi,
  [Available_item_Desc_CN] =  sid.available_Item_Desc_CN
 from @Result R, subsidizeitemdetails sid
 where R.[Subsidize_Item_Code] = sid.Subsidize_item_code  
 and R.Available_item_Code = sid.Available_item_Code  
   
 update @Result    
 set [Available_item_Code_Desc] = sid.available_Item_Desc    
 from @Result R, subsidizeitemdetails sid    
 where R.[Subsidize_Item_Code] = sid.Subsidize_item_code    
 and R.Available_item_Code = sid.Available_item_Code    

 update @Result    
 set [Display_Code_For_Claim] = SGC.[Display_Code_For_Claim]    
 from @Result R, SubsidizeGroupClaim SGC    
 where R.Scheme_Code = SGC.Scheme_Code    
 and R.Scheme_Seq = SGC.Scheme_Seq    
 and R.Subsidize_Code = SGC.Subsidize_code    

 select convert(varchar(11), R.[Service_Receive_Dtm], 113) as Service_Receive_Dtm,    
  R.[Display_Code] as Scheme_Code,    
  R.[Scheme_Seq],    
  R.[Subsidize_Code],    
  R.[Subsidize_Item_Code],    
  R.[Available_item_Code] as Available_item_Code,    
  R.[Unit],    
  R.[Source],    
  R.[Transaction_ID],  
  R.[Per_Unit_Value],      
  R.[Total_Amount],   
  R.[DOB],
  R.[Exact_DOB],
  R.[Remark],    
  R.[ExchangeRate_Value],      
  R.[Total_Amount_RMB] ,  
  R.[Available_Item_Desc],      
  R.[Available_Item_Desc_Chi],
  R.[Available_Item_Desc_CN],
  R.[Display_Code_For_Claim]
  from @result R    
  order by R.[Service_Receive_Dtm]    
      
     
END  
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byDocCodeDocID] TO WSEXT
GO
