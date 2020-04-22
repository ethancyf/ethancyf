 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

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
-- CR No.: CRE13-017-05
-- Author:  Karl LAM
-- Create date: 03 DEC 2013  
-- Description: Retrieve age report for CVSS PCV13
--				Copy from proc_EHS_CIVSSAgeReport_Stat
-- =============================================  

--exec [proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat] '2013-12-05'; select * from [RpteHSD0025CVSSPCV13AgeReportStat]
/*
CREATE PROCEDURE  [dbo].[proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat]     
 @Cutoff_Dtm  datetime    
    
as    
BEGIN
    
-- =============================================
-- Declaration                                                             
-- =============================================
DECLARE @Scheme_Code as varchar(10)
DECLARE @Report_ID as varchar(30)
DECLARE @EligibleClaimStartDOB as Datetime
DECLARE @EligibleClaimEndDOB as Datetime
DECLARE @EligibleClaimStartOperator as Varchar(20)
DECLARE @EligibleClaimEndOperator  as Varchar(20)

CREATE TABLE #TempCVSSPCV13Tran (    
  Transaction_ID char(20),    
  Service_Receive_Dtm  datetime,    
  Account_Type char(1),    
  DOB    datetime,    
  DOB_Adjust  datetime,    
  Exact_DOB  char(1),    
  Available_Item_Code char(20),    
  SP_ID   char(8),
  Eligible_Claim bit default 0
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
SET @Scheme_Code = 'CVSSPCV13'
SET @Report_ID = 'eHSD0025'

SELECT @EligibleClaimStartDOB = Value, @EligibleClaimStartOperator = Operator 
FROM EligibilityRule 
WHERE Scheme_Code = @Scheme_Code and Scheme_Seq = 1 and Subsidize_Code = 'CPV13' and Rule_Name = 'MinAge' and [Type] = 'DOB' and Handling_Method = 'Normal'

SELECT @EligibleClaimEndDOB = Value, @EligibleClaimEndOperator = Operator 
FROM EligibilityRule 
WHERE Scheme_Code = @Scheme_Code and Scheme_Seq = 1 and Subsidize_Code = 'CPV13' and Rule_Name = 'MaxAge' and [Type] = 'DOB' and Handling_Method = 'Normal'

--Validated    

Declare @schemeDate Datetime  
set @schemeDate = DATEADD(dd, -1, @Cutoff_Dtm)  
EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @schemeDate  
    
Insert INTO #TempCVSSPCV13Tran    
(      
 Transaction_ID,    
 Service_Receive_Dtm,    
 Account_Type,    
 DOB,    
 DOB_Adjust,    
 Exact_DOB,    
 Available_Item_Code,    
 SP_ID    
)    
SELECT    
 T.Transaction_ID,    
 T.Service_Receive_Dtm,    
 'V',    
 VR.DOB,    
 VR.DOB,    
 VR.Exact_DOB,    
 D.Available_Item_Code,    
 T.SP_ID    
FROM VoucherTransaction T     
 INNER JOIN TransactionDetail D on T.Transaction_ID COLLATE DATABASE_DEFAULT = D.Transaction_ID COLLATE DATABASE_DEFAULT   
 INNER JOIN PersonalInformation VR    
  ON T.Voucher_Acc_ID  COLLATE DATABASE_DEFAULT = VR.Voucher_Acc_ID   COLLATE DATABASE_DEFAULT    
   AND T.Doc_Code  COLLATE DATABASE_DEFAULT  = VR.Doc_Code   COLLATE DATABASE_DEFAULT        
   AND T.Voucher_Acc_ID IS NOT NULL    
   AND T.Voucher_Acc_ID <> ''    
 INNER JOIN VoucherAccount A    
  ON T.Voucher_Acc_ID  COLLATE DATABASE_DEFAULT = A.Voucher_Acc_ID   COLLATE DATABASE_DEFAULT    
WHERE T.Scheme_Code= @Scheme_Code
AND T.Transaction_Dtm <= @Cutoff_Dtm    
AND T.Record_Status NOT In (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND (T.Invalidation IS NULL OR T.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
AND D.scheme_seq = @current_scheme_Seq    
ORDER BY T.Transaction_ID    
    
--Temp    
Insert INTO #TempCVSSPCV13Tran    
(      
 Transaction_ID,    
 Service_Receive_Dtm,    
 Account_Type,    
 DOB,    
 DOB_Adjust,    
 Exact_DOB,    
 Available_Item_Code,    
 SP_ID    
)    
SELECT    
 T.Transaction_ID,    
 T.Service_Receive_Dtm,    
 'T',    
 TVR.DOB,    
 TVR.DOB,    
 TVR.Exact_DOB,    
 D.Available_Item_Code,    
 T.SP_ID    
FROM VoucherTransaction T    
 INNER JOIN TransactionDetail D     
  ON T.Transaction_ID  COLLATE DATABASE_DEFAULT = D.Transaction_ID    COLLATE DATABASE_DEFAULT   
 INNER JOIN TempPersonalInformation TVR     
  ON T.Temp_Voucher_Acc_ID  COLLATE DATABASE_DEFAULT = TVR.Voucher_Acc_ID      COLLATE DATABASE_DEFAULT 
   AND (T.Voucher_Acc_ID = '' OR T.Voucher_Acc_ID IS NULL)    
   AND T.Special_Acc_ID IS NULL    
   AND T.Invalid_Acc_ID IS NULL    
   AND T.Temp_Voucher_Acc_ID <> ''     
   AND T.Temp_Voucher_Acc_ID IS NOT NULL     
   AND T.Doc_Code  COLLATE DATABASE_DEFAULT = TVR.Doc_Code   COLLATE DATABASE_DEFAULT   
 INNER JOIN TempVoucherAccount A    
  ON T.Temp_Voucher_Acc_ID  COLLATE DATABASE_DEFAULT = A.Voucher_Acc_ID    COLLATE DATABASE_DEFAULT   
WHERE T.Scheme_Code=@Scheme_Code
AND T.Transaction_Dtm <= @Cutoff_Dtm    
AND T.Record_Status NOT In (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND (T.Invalidation IS NULL OR T.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
AND D.scheme_seq = @current_scheme_Seq      
ORDER BY T.Transaction_ID    
  
--Special    
Insert INTO #TempCVSSPCV13Tran    
(      
 Transaction_ID,    
 Service_Receive_Dtm,    
 Account_Type,    
 DOB,    
 DOB_Adjust,    
 Exact_DOB,    
 Available_Item_Code,    
 SP_ID    
)    
SELECT    
 T.Transaction_ID,    
 T.Service_Receive_Dtm,    
 'S',    
 TVR.DOB,    
 TVR.DOB,    
 TVR.Exact_DOB,    
 D.Available_Item_Code,    
 T.SP_ID    
FROM VoucherTransaction T    
 INNER JOIN TransactionDetail D     
  ON T.Transaction_ID  COLLATE DATABASE_DEFAULT = D.Transaction_ID     COLLATE DATABASE_DEFAULT 
 INNER JOIN SpecialPersonalInformation TVR ON T.Special_Acc_ID  COLLATE DATABASE_DEFAULT = TVR.Special_Acc_ID    COLLATE DATABASE_DEFAULT   
     AND T.Special_Acc_ID IS NOT NULL    
     AND (T.Voucher_Acc_ID IS NULL OR T.Voucher_Acc_ID = '')    
     AND T.Invalid_Acc_ID IS NULL    
     AND T.Doc_Code  COLLATE DATABASE_DEFAULT = TVR.Doc_Code  COLLATE DATABASE_DEFAULT     
 INNER JOIN SpecialAccount A    
  ON T.Special_Acc_ID  COLLATE DATABASE_DEFAULT = A.Special_Acc_ID    COLLATE DATABASE_DEFAULT   
WHERE T.Scheme_Code=@Scheme_Code
AND T.Transaction_Dtm <= @Cutoff_Dtm    
AND T.Record_Status NOT In (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND (T.Invalidation IS NULL OR T.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
AND D.scheme_seq = @current_scheme_Seq      
Order by T.Transaction_ID    
    
--Format the Exact_DOB is month case, date value become the last day of that month    
Update #TempCVSSPCV13Tran Set DOB = dateadd(m, datediff(m, 0, dateadd(m, 1, DOB)), -1)    
where Exact_DOB in ('M','U')    
    
Update #TempCVSSPCV13Tran Set DOB_Adjust = dateadd(m, datediff(m, 0, dateadd(m, 1, DOB_Adjust)), -1)    
where Exact_DOB in ('M','U')    
    
UPDATE    
  #TempCVSSPCV13Tran    
 SET    
  DOB_Adjust = DATEADD(yyyy, 1, DOB_Adjust)    
 WHERE     
  MONTH(DOB_Adjust) > MONTH(Service_receive_dtm)    
  OR     
  (     
   MONTH(DOB_Adjust) = MONTH(Service_receive_dtm) AND DAY(DOB_Adjust) > DAY(Service_receive_dtm)     
  ) 
  
  
--Find out eligible claims
DECLARE @sql_Find_Not_Eligible varchar(1000)

SET @sql_Find_Not_Eligible = 'UPDATE #TempCVSSPCV13Tran SET Eligible_Claim = 1 WHERE DOB ' + 
@EligibleClaimStartOperator + '''' + Convert(Varchar(11),@EligibleClaimStartDOB, 106) + '''' +
' AND DOB ' + @EligibleClaimEndOperator + '''' + Convert(Varchar(11),@EligibleClaimEndDOB, 106) + '''' 

EXECUTE (@sql_Find_Not_Eligible)    
    
   
-- =============================================
--   Return results                            
-- =============================================
--Prepare the final output datatable    
Insert INTO @OutputTable VALUES    
(1, 'eHS(S)D0025-02: Report on CVSSPCV13 transaction by age group', '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable VALUES    
(2, '' , '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable VALUES    
(3, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111) , '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable VALUES    
(4, '' , '', '', '', '', '', '', '', '', '', '')    
    
Insert INTO @OutputTable (Display_Seq, Col1)    

SELECT TOP 1 5, rtrim([Display_Code_For_Claim] )
FROM [SubsidizeGroupClaim] 
INNER JOIN [Subsidize]
ON [SubsidizeGroupClaim].[Subsidize_Code]  COLLATE DATABASE_DEFAULT = [Subsidize].[Subsidize_Code] COLLATE DATABASE_DEFAULT 
WHERE [SubsidizeGroupClaim].[Scheme_Code]  COLLATE DATABASE_DEFAULT = @Scheme_Code  COLLATE DATABASE_DEFAULT 
AND [SubsidizeGroupClaim].[Scheme_Seq] = @current_scheme_Seq 
AND [Subsidize].[Subsidize_Item_Code]  COLLATE DATABASE_DEFAULT = 'PV13'   COLLATE DATABASE_DEFAULT 
    
Insert INTO @OutputTable VALUES    
(6, 'Less than 2 years', '2 to less than 3 years', '3 to less than 4 years', '4 to less than 5 years' ,'5 years and above','Non Eligible Claim*', '', 'Total', '', 'No. of SP involved', '')    
    
Insert INTO @OutputTable    
SELECT 7, sum([less 2 Year]), sum([2 Year]), sum([3 YEAR]), sum([4 YEAR]),sum([5 YEAR]), sum([Non_Eligible]), '', '=sum(A7:F7)', '','',''
FROM (     
select count(1) [less 2 Year], 0 [2 YEAR], 0 [3 YEAR], 0 [4 YEAR], 0 [5 Year], 0 [Non_Eligible] from #TempCVSSPCV13Tran    
Where 
 DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 2   
 AND Eligible_Claim = 1 
union all    
select 0 [less 2 Year], count(1) [2 Year], 0 [3 YEAR], 0 [4 YEAR], 0 [5 Year], 0 [Non_Eligible] from #TempCVSSPCV13Tran    
Where DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 2    
 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 3  
 AND Eligible_Claim = 1
union all    
select 0 [less 2 Year],0 [2 Year], count(1) [3 YEAR], 0 [4 YEAR], 0 [5 Year], 0 [Non_Eligible] from #TempCVSSPCV13Tran    
Where 
 DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 3 
 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 4
 AND Eligible_Claim = 1    
union all    
select 0 [less 2 Year],0 [2 Year], 0 [3 YEAR], count(1) [4 YEAR], 0 [5 Year], 0 [Non_Eligible] from #TempCVSSPCV13Tran    
Where 
 DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 4 
 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 5
 AND Eligible_Claim = 1
union all    
select 0 [less 2 Year],0 [2 Year], 0 [3 YEAR], 0 [4 YEAR], count(1) [5 YEAR], 0 [Non_Eligible] from #TempCVSSPCV13Tran    
Where  
 DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 5   
 AND Eligible_Claim = 1
union all    
select 0 [less 2 Year],0 [2 Year], 0 [3 YEAR], 0 [4 YEAR], 0 [5 YEAR], count(1) [Non_Eligible] from #TempCVSSPCV13Tran    
Where  
Eligible_Claim = 0
) AS onlydose    
    
UPDATE @OutputTable   
set Col8 = (select convert(int,col1) + convert(int,col2) + convert(int,col3) + convert(int,col4) + convert(int,col5) + convert(int,col6) from @OutputTable where Display_Seq = 7),  
 Col10 = (select COUNT(DISTINCT SP_ID) [Sp] from #TempCVSSPCV13Tran)   
where Display_Seq = 7    

--Remarks
Insert INTO @OutputTable VALUES    
(8, '' , '', '', '', '', '', '', '', '', '', '')    

Insert INTO @OutputTable VALUES    
(9, '*Non-Eligible Claim: eHealth (Subsidies) Account with DOB outside ' + convert(Varchar(10), @EligibleClaimStartDOB, 111) + ' and ' + convert(Varchar(10), @EligibleClaimEndDOB, 111)  , '', '', '', '', '', '', '', '', '', '')      
  
Delete FROM [RpteHSD0025CVSSPCV13AgeReportStat] 
    
Insert INTO [RpteHSD0025CVSSPCV13AgeReportStat]    
(     
 Display_Seq,    
 Col1 ,    
 Col2 ,    
 Col3 ,    
 Col4 ,    
 Col5 ,    
 Col6 ,    
 Col7 ,    
 Col8 ,    
 Col9 ,    
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
    
    
--House Keeping
DROP TABLE #TempCVSSPCV13Tran

    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat] TO HCVU
GO
*/
