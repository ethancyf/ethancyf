IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0027_02_PIDVSSAgeReport_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0027_02_PIDVSSAgeReport_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	11 Sep 2017
-- CR No.:			CRE17-003 (Stop vaccine daily stat)
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================       
-- =============================================  
-- CR No.:		CRE15-005-04
-- Author:		Winnie SUEN
-- Create date: 26 Aug 2015
-- Description: Retrieve PIDVSS transaction by age group
--				Copy from proc_EHS_CIVSSAgeReport_Stat
-- =============================================  
/*    
CREATE Procedure [proc_EHS_eHSD0027_02_PIDVSSAgeReport_Stat]    
 @Cutoff_Dtm as DateTime    
AS    
BEGIN    
SET NOCOUNT ON;    

-- =============================================  
-- Declaration                                                           
-- =============================================  
DECLARE @TempPIDVSSTran table (    
  Transaction_ID		char(20),    
  Service_Receive_Dtm	datetime,    
  Account_Type			char(1),    
  DOB					datetime,    
  DOB_Adjust			datetime,    
  Exact_DOB				char(1),    
  Available_Item_Code	char(20),    
  SP_ID					char(8),
  Subsidize_Code		char(10) 
 )    
 
 
    
DECLARE @OutputTable table (    
  Display_Seq  smallint,    
  Col1   varchar(100) default '',    
  Col2   varchar(100) default '',    
  Col3   varchar(100) default '',    
  Col4   varchar(100) default '',    
  Col5   varchar(100) default '',    
  Col6   varchar(100) default '',    
  Col7   varchar(100) default '',    
  Col8   varchar(100) default '',    
  Col9   varchar(100) default '',    
  Col10   varchar(100) default '',    
  Col11   varchar(100) default ''    
 )    
--Determine Scheme seq    
Declare @current_scheme_Seq int    
-- =============================================  
--   Validation                                   
-- =============================================      
    
-- =============================================  
--   Initialization                               
-- =============================================  
--Validated    
--SELECT top 1  @current_scheme_Seq = scheme_seq FROM SubsidizeGroupClaim where scheme_code = 'PIDVSS' and ((Claim_Period_From <= @Cutoff_Dtm and @Cutoff_Dtm < Claim_Period_To)  
Declare @schemeDate Datetime  
set @schemeDate = DATEADD(dd, -1, @Cutoff_Dtm)  
EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'PIDVSS', @schemeDate  
    
Insert INTO @TempPIDVSSTran    
(      
 Transaction_ID,    
 Service_Receive_Dtm,    
 Account_Type,    
 DOB,    
 DOB_Adjust,    
 Exact_DOB,    
 Available_Item_Code,    
 SP_ID,
 Subsidize_Code
)    
SELECT    
 T.Transaction_ID,    
 T.Service_Receive_Dtm,    
 'V',    
 VR.DOB,    
 VR.DOB,    
 VR.Exact_DOB,    
 D.Available_Item_Code,    
 T.SP_ID,
 D.Subsidize_Code
FROM VoucherTransaction T     
 INNER JOIN TransactionDetail D on T.Transaction_ID = D.Transaction_ID     
 INNER JOIN PersonalInformation VR    
  ON T.Voucher_Acc_ID = VR.Voucher_Acc_ID     
   AND T.Doc_Code = VR.Doc_Code         
   AND T.Voucher_Acc_ID IS NOT NULL    
   AND T.Voucher_Acc_ID <> ''    
 INNER JOIN VoucherAccount A    
  ON T.Voucher_Acc_ID = A.Voucher_Acc_ID     
WHERE T.Scheme_Code='PIDVSS'     
AND T.Transaction_Dtm <= @Cutoff_Dtm    
AND T.Record_Status NOT In (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0027')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND (T.Invalidation IS NULL OR T.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0027')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
AND D.scheme_seq = @current_scheme_Seq    
 --AND T.SP_ID NOT IN (Select SP_ID FROM SPExceptionList)    
ORDER BY T.Transaction_ID    
    
--Temp    
Insert INTO @TempPIDVSSTran    
(      
 Transaction_ID,    
 Service_Receive_Dtm,    
 Account_Type,    
 DOB,    
 DOB_Adjust,    
 Exact_DOB,    
 Available_Item_Code,    
 SP_ID,
 Subsidize_Code
)    
SELECT    
 T.Transaction_ID,    
 T.Service_Receive_Dtm,    
 'T',    
 TVR.DOB,    
 TVR.DOB,    
 TVR.Exact_DOB,    
 D.Available_Item_Code,    
 T.SP_ID,
 D.Subsidize_Code
FROM VoucherTransaction T    
 INNER JOIN TransactionDetail D     
  ON T.Transaction_ID = D.Transaction_ID     
 INNER JOIN TempPersonalInformation TVR     
  ON T.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID     
   AND (T.Voucher_Acc_ID = '' OR T.Voucher_Acc_ID IS NULL)    
   AND T.Special_Acc_ID IS NULL    
   AND T.Invalid_Acc_ID IS NULL    
   AND T.Temp_Voucher_Acc_ID <> ''     
   AND T.Temp_Voucher_Acc_ID IS NOT NULL     
   AND T.Doc_Code = TVR.Doc_Code    
 INNER JOIN TempVoucherAccount A    
  ON T.Temp_Voucher_Acc_ID = A.Voucher_Acc_ID     
WHERE T.Scheme_Code='PIDVSS'     
AND T.Transaction_Dtm <= @Cutoff_Dtm    
AND T.Record_Status NOT In (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0027')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND (T.Invalidation IS NULL OR T.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0027')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
AND D.scheme_seq = @current_scheme_Seq     
 --AND T.SP_ID NOT IN (Select SP_ID FROM SPExceptionList)    
ORDER BY T.Transaction_ID    
    
--Special    
Insert INTO @TempPIDVSSTran    
(      
 Transaction_ID,    
 Service_Receive_Dtm,    
 Account_Type,    
 DOB,    
 DOB_Adjust,    
 Exact_DOB,    
 Available_Item_Code,    
 SP_ID,
 Subsidize_Code
)    
SELECT    
 T.Transaction_ID,    
 T.Service_Receive_Dtm,    
 'S',    
 TVR.DOB,    
 TVR.DOB,    
 TVR.Exact_DOB,    
 D.Available_Item_Code,    
 T.SP_ID,
 D.Subsidize_Code
FROM VoucherTransaction T    
 INNER JOIN TransactionDetail D     
  ON T.Transaction_ID = D.Transaction_ID    
 INNER JOIN SpecialPersonalInformation TVR ON T.Special_Acc_ID = TVR.Special_Acc_ID     
     AND T.Special_Acc_ID IS NOT NULL    
     AND (T.Voucher_Acc_ID IS NULL OR T.Voucher_Acc_ID = '')    
     AND T.Invalid_Acc_ID IS NULL    
     AND T.Doc_Code = TVR.Doc_Code     
 INNER JOIN SpecialAccount A    
  ON T.Special_Acc_ID = A.Special_Acc_ID     
WHERE T.Scheme_Code='PIDVSS'    
AND T.Transaction_Dtm <= @Cutoff_Dtm    
AND T.Record_Status NOT In (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0027')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND (T.Invalidation IS NULL OR T.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0027')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
AND D.scheme_seq = @current_scheme_Seq     
 --AND T.SP_ID NOT IN (Select SP_ID FROM SPExceptionList)    
Order by T.Transaction_ID    
    
--Format the Exact_DOB is month case, date value become the last day of that month    
Update @TempPIDVSSTran Set DOB = dateadd(m, datediff(m, 0, dateadd(m, 1, DOB)), -1)    
where Exact_DOB in ('M','U')    
    
Update @TempPIDVSSTran Set DOB_Adjust = dateadd(m, datediff(m, 0, dateadd(m, 1, DOB_Adjust)), -1)    
where Exact_DOB in ('M','U')    
    
UPDATE    
  @TempPIDVSSTran    
 SET    
  DOB_Adjust = DATEADD(yyyy, 1, DOB_Adjust)    
 WHERE     
  MONTH(DOB_Adjust) > MONTH(Service_receive_dtm)    
  OR     
  (     
   MONTH(DOB_Adjust) = MONTH(Service_receive_dtm) AND DAY(DOB_Adjust) > DAY(Service_receive_dtm)     
  )    
    
-- =============================================  
--   Return results                               
-- =============================================  
--Prepare the final output datatable    
Insert INTO @OutputTable VALUES    
(1, 'eHS(S)D0027-02: Report on PIDVSS transaction by age group and dose', '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable VALUES    
(2, '' , '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable VALUES    
(3, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111) , '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable VALUES    
(4, '' , '', '', '', '', '', '', '', '', '', '')    
    


Declare @displayCode nvarchar(255)  
SET @displayCode = (SELECT [Display_Code_For_Claim] + ' and '  
					FROM (  SELECT Display_Code_For_Claim FROM [SubsidizeGroupClaim]   
							INNER JOIN [Subsidize] ON [SubsidizeGroupClaim].[Subsidize_Code] = [Subsidize].[Subsidize_Code]  
							WHERE [SubsidizeGroupClaim].[Scheme_Code] = 'PIDVSS' 
							AND [SubsidizeGroupClaim].[Scheme_Seq] = @current_scheme_Seq 
							AND [Subsidize].[Subsidize_Item_Code] = 'SIV' ) tblDisplayCode  
					for xml path('') )  
SET @displayCode = SUBSTRING(@displayCode, 1, LEN(@displayCode)- 4) 

Insert INTO @OutputTable (Display_Seq, Col1,Col2) VALUES
(5, '', @displayCode)

Insert INTO @OutputTable VALUES    
(6, 'Dose', '6 months to less than 2 years', '2 years to less than 9 years', '>= 9 years', 'Total', '', 'No. of SP involved', '', '', '', '')    

Insert INTO @OutputTable VALUES    
(7, '' , '', '', '', '', '', '', '', '', '', '')   

--1st dose
Insert INTO @OutputTable    
SELECT 8, '1st Dose', '', '', '', '', '', COUNT(DISTINCT SP_ID), '','','',''    
FROM @TempPIDVSSTran 

Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(9, 'QIV')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(10, 'TIV')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES  
(11, 'Sub-total')
   
 
Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 9     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 9)    
where Display_Seq = 9 


Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 10     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 10)    
where Display_Seq = 10 


Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='1STDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 11 

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 11)    
where Display_Seq = 11 


--2nd dose 
Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(12, '')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES  
(13, '2nd Dose')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(14, 'QIV')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(15, 'TIV')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES  
(16, 'Sub-total')

    
Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 14     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 14)    
where Display_Seq = 14 


Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 15     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 15)    
where Display_Seq = 15 


Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='2NDDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 16     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 16)
where Display_Seq = 16 

--Only dose  
Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(17, '')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES  
(18, 'Only Dose')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(19, 'QIV')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES
(20, 'TIV')

Insert INTO @OutputTable (Display_Seq, Col1) VALUES  
(21, 'Sub-total')
    
Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code = 'PIDQIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 19     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 19)    
where Display_Seq = 19


Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code = 'PIDTIV'
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 20

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 20)    
where Display_Seq = 20


Update @OutputTable set     
Col2 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2) ,
Col3 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9) ,
Col4 = (SELECT count(1) FROM @TempPIDVSSTran Where Available_Item_Code='ONLYDOSE' AND Subsidize_Code IN ('PIDQIV', 'PIDTIV')
AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9)
WHERE Display_Seq = 21     

Update @OutputTable   
set Col5 = (select convert(int,Col2) + convert(int,Col3) + convert(int,Col4) from @OutputTable where Display_Seq = 21)    
where Display_Seq = 21

  
DELETE FROM RpteHSD0027PIDVSSAgeReportStat  
INSERT INTO RpteHSD0027PIDVSSAgeReportStat
( 
  Display_Seq,    
  Col1,    
  Col2,    
  Col3,    
  Col4,    
  Col5,    
  Col6,    
  Col7,   
  Col8,    
  Col9,    
  Col10,
  Col11 
)       
Select     
 Display_Seq,    
 Col1 ,    
 Col2 ,    
 Col3 ,    
 Col4 ,    
 Col5 ,    
 Col6 ,    
 Col7 ,    
 Col8 ,    
 Col9,    
 Col10,    
 Col11    
from @OutputTable    
order by    
 Display_Seq    
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0027_02_PIDVSSAgeReport_Stat] TO HCVU
GO
*/
