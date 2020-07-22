IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0006_Report_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0006_Report_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	17 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Add sub Report [03],[05],[06] for DHC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	5 July 2017
-- CR No.:			CRE17-002 (Fine-tunning of eHSM0006 Report)
-- Description:		Add sub Report [Summary], [04]
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 July 2017
-- CR No.:			INT17-0009 (Follow up action on Voucher 65)
-- Description:		- Fix the unicode character in remark
--					- Change the file encoding to 65001
-- =============================================  
-- =============================================    
-- CR No.:			CRE16-025 (Lowering voucher eligibility age)
-- Author:			Winnie SUEN
-- Create date:		31 May 2016
-- Description:		New monthly report
-- =============================================  

-- exec [proc_EHS_eHSM0006_Report_Read]

Create PROCEDURE [dbo].[proc_EHS_eHSM0006_Report_Read]     
AS BEGIN  
-- =============================================  
-- Declaration  
-- =============================================  
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
-- =============================================  
-- Return results  
-- =============================================  
Declare @strGenDtm varchar(50)    
SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)    
SELECT 'Report Generation Time: ' + @strGenDtm  

-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSM0006_Report_Write
-- To Excel sheet: eHSM0006-Summary
-- --------------------------------------------------    
 SELECT    
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,'')
 FROM         
  RpteHSM0006VoucherClaimByAge WITH (NOLOCK)    
 ORDER BY    
  Display_Seq   

-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSM0006_Report_Write
-- To Excel sheet: eHSM0006-01: Report on Number of Voucher Recipients (VR) by age groups (covering HCVS and HCVSCHN only)
-- --------------------------------------------------    
 SELECT    
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,''),
  isnull(Col11,''),
  isnull(Col12,''),
  isnull(Col13,'')
 FROM         
  RpteHSM0006VRByAgeStat WITH (NOLOCK)    
 ORDER BY    
  Display_Seq    

    
-- --------------------------------------------------  
-- To Excel sheet: eHSM0006-03:Report on Voucher Amount Claimed by Profession (HCVS)
-- --------------------------------------------------  
 SELECT   
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,''),
  isnull(Col11,''),
  isnull(Col12,'')
 FROM  
  RpteHSM0006VoucherClaimByProfStat WITH (NOLOCK)  
 ORDER BY  
  Display_Seq   
-- --------------------------------------------------  
-- To Excel sheet: eHSM0006-04:Report on Voucher Amount Claimed by Profession (HCVSDHC)
-- --------------------------------------------------  
 SELECT   
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,'')
 FROM  
  RpteHSM0006VoucherClaimByProfStat_HCVSDHC WITH (NOLOCK)
 ORDER BY  
  Display_Seq   
-- --------------------------------------------------  
-- To Excel sheet: eHSM0006-05: Number of Claim Transactions by Professions of Service Providers, Reasons for Visit and Age Groups of Voucher Recipients (HCVS)
-- --------------------------------------------------  
 SELECT    
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,''),
  isnull(Col11,''),
  isnull(Col12,''),
  isnull(Col13,'') 
 FROM    
  RpteHSM0006VoucherClaimByReasonForVisitStat WITH (NOLOCK)  
 ORDER BY  
  Display_Seq   
-- --------------------------------------------------  
-- To Excel sheet: eHSM0006-06: Number of Claim Transactions by Professions of Service Providers, Reasons for Visit and Age Groups of Voucher Recipients (HCVSDHC)
-- --------------------------------------------------  
 SELECT    
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,'')
 FROM    
  RpteHSM0006VoucherClaimByReasonForVisitStat_HCVSDHC WITH (NOLOCK)
 ORDER BY  
  Display_Seq
-- --------------------------------------------------  
-- To Excel sheet: eHSM0006-07: Report on Voucher Amount Claimed by Age Groups of Voucher Recipients and by Practice (HCVSDHC)
-- --------------------------------------------------  
 SELECT    
  isnull(Col1,''), isnull(Col2,''), isnull(Col3,''), isnull(Col4,''), isnull(Col5,''),  
  isnull(Col6,''), isnull(Col7,''), isnull(Col8,''), isnull(Col9,''), isnull(Col10,'')
 FROM    
  RpteHSM0006VoucherClaimByAgeByPractice_HCVSDHC WITH (NOLOCK) 
 ORDER BY  
  Display_Seq  

-- --------------------------------------------------  
-- To Excel sheet: eHSM0006-08: Report on Voucher Amount Claimed by Age Groups of Voucher Recipients and by Practice (HCVSCHN)
-- --------------------------------------------------  
 SELECT    
  isnull(Col1,''), isnull(Col2,''), isnull(Col3,''), isnull(Col4,''), isnull(Col5,''),  
  isnull(Col6,''), isnull(Col7,''), isnull(Col8,''), isnull(Col9,''), isnull(Col10,''),    
  isnull(Col11,''), isnull(Col12,''), isnull(Col13,''), isnull(Col14,''), isnull(Col15,''),  
  isnull(Col16,''), isnull(Col17,''), isnull(Col18,''), isnull(Col19,''), isnull(Col20,''),    
  isnull(Col21,''), isnull(Col22,''), isnull(Col23,''), isnull(Col24,''), isnull(Col25,''),  
  isnull(Col26,''), isnull(Col27,''), isnull(Col28,''), isnull(Col29,''), isnull(Col30,''),    
  isnull(Col31,''), isnull(Col32,''), isnull(Col33,''), isnull(Col34,''), isnull(Col35,''),  
  isnull(Col36,''), isnull(Col37,''), isnull(Col38,''), isnull(Col39,''), isnull(Col40,''),    
  isnull(Col41,''), isnull(Col42,''), isnull(Col43,''), isnull(Col44,''), isnull(Col45,''),  
  isnull(Col46,''), isnull(Col47,''), isnull(Col48,''), isnull(Col49,''), isnull(Col50,'')
 FROM    
  RpteHSM0006VoucherClaimByAgeByPractice_HCVSCHN WITH (NOLOCK)  
 ORDER BY  
  Display_Seq  


-- --------------------------------------------------  
-- To Excel sheet:   eHSM0006-Remarks: Remarks  
-- --------------------------------------------------  
DECLARE @tblRemark AS TABLE (
	Seq	INT identity(1,1),
	Result_Value1 NVARCHAR(MAX),    
	Result_Value2 NVARCHAR(MAX)  
)

-- Lengend

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '(A) Legend', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '1.Profession Type Legend', ''


INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT Service_Category_Code, Service_Category_Desc 
FROM Profession WITH (NOLOCK)
ORDER BY Service_Category_Code


INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '', ''


-- Common Note

-- eHealth Accounts

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '(B) Common Note(s) for the report', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '1. eHealth (Subsidies) Accounts:', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   a. All terminated account are excluded except where otherwise specified', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   b. Firstly registered account will be chosen if there is more than one eHealth (Subsidies) Accounts for the same person (same identity document no.)', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   c. Registered date is the account create date if the elderly is eligible for the voucher scheme at the time of account creation', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   d. Registered date is the eligible date if the elderly is ineligible for the voucher scheme at the time of account creation', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   e. Eligible date is the first day of the year reached Aged 70 year-old if the elderly aged >= 70 on 2017', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '    f. Eligible date is the first effective day of lowering the voucher eligibility age to 65 (i.e. 01 Jul 2017) if the elderly aged between 65 and 69 on 2017', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   g. Eligible date is the first day of the year reached Aged 65 year-old if the elderly aged < 65 on 2017', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   h. For registered account with death date before being eligible in HCVS/HCVSDHC/HCVSCHN (i.e. ineligible at the time of account creation and with death date before the year eligible for HCVS/HCVSDHC/HCVSCHN), his account will NOT be counted in the statistics', ''


INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '', ''

-- Transactions

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '2. Transactions:', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   c. Exclude voided/deleted transactions.', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '', ''

 -- Age Group

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '3. Age group (refer to a specific date):', ''
INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   a. Age = year of the specific date – year of DOB', ''

INSERT INTO @tblRemark (Result_Value1, Result_Value2)
SELECT '   b. The age of VR and the related accounts are refer to the age of firstly registered account', ''

    
SELECT Result_Value1, Result_Value2 FROM @tblRemark ORDER BY Seq
END  
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0006_Report_Read] TO HCVU
GO

