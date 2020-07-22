IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0001_HCVS_Stat_Schedule_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0001_HCVS_Stat_Schedule_get]
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
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Including scheme [HCVSDHC]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-020 (HKIC Symbol Others)
-- Modified by:		Winnie SUEN
-- Modified date:	25 Feb 2019
-- Description:		Show HKIC Symbol Description
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	14 May 2018
-- CR No.:			CRE17-010
-- Description:		OCSSS Integration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Mar 2018
-- CR No.:			INT17-0026
-- Description:		Fix CNY dollar sign
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	10 Jan 2018
-- CR No.:			CRE14-016
-- Description:		Deceased Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 April 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-018
-- Modified by:		Karl LAM
-- Modified date:	05 Feb 2014
-- Description:		enhance eHSD0001 for change voucher amount to $1
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-006
-- Modified by:		Karl LAM
-- Modified date:	12 Nov 2013
-- Description:		Update sub Report eHSD0001-02
--					Add new sub report eHSD0001-03	Report on Write Off Voucher Summary on eHealth Account (for HCVS-eligible only)
--					Add new sub report eHSD0001-04	Report on Given Voucher Summary on eHealth Account (for HCVS-eligible only)		
--					Rename report eHSD0001-04 to eHSD0001-05					
--					Rename report eHSD0001-05 to eHSD0001-06
--					Rename report eHSD0001-06 to eHSD0001-07
--					Rename report eHSD0001-07 to eHSD0001-08
--					Rename all physical temp tables to add prefix 'RpteHSD0001'
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0020
-- Modified by:		Koala CHENG
-- Modified date:	30 Aug 2013
-- Description:		Fix to use nvarchar to store chinese data
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0014
-- Modified by:		Karl LAM
-- Modified date:	04 Jul 2013
-- Description:		Fix sorting seq of raw data sub report 07
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-002 Adding 4 data fields "SPID", "Practice No.", "Practice Name (In English)" and "Practice Name (In Chinese)" in HCVS daily report
-- Modified by:		Tommy LAM
-- Modified date:	29 Apr 2013
-- Description:		Add additional data fields - [SP_ID], [Practice_Display_Seq], [Practice_Name] and [Practice_Name_Chi] for the report ¡V ¡§eHSD0001-07¡¨ (worksheet 07)
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	4 Jan 2013
-- Description:		1. Remove joining by SchemeClaim.Scheme_Seq
--					2. @MaxSubsidize is bounded by SubsidizeGroupClaim.Last_Service_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	7 October 2011
-- CR No.:		CRE11-024-02
-- Description:		(1) Modified code to cater the 'ROP' field, which affected the following reports:
--						(i) eHSD0001-04 Report on Number of Voucher Claimed by Profession
--						(ii) eHSD0001-05 Report on Number of Voucher Claim per Transaction by Profession
--						(iii) eHSD0001-06 Claim Traansactions by Profession and Reason For Visit Level 1
--					(2) Changed the existing eHSD0001-06 Claim Traansactions by Profession and Reason For Visit Level 1
--						(i) display 1 more line called Defer Input
--						(ii) added the Secondary Reason For Visit calculation, which needs the addition of the labels to @ws06
--					(3) No change, but need to mention that voucher extension will cause more available vouchers
--							for calculation to be considered for different ages.  This will change he display in:
--						(i) eHSD0001-02 Report on Balance Summary of eHealth Accounts (for HCVS-eligible only)
--						(ii) eHSD0001-03 Report on Usage of Zero Balance eHealth Accounts
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	9 September 2011
-- CR No.:			CRP11-009
-- Description:		Modifications to data retrieval for preserve ordering
--					for the following temp tables:
--						- _VoucherclaimPerVoucherSummary
--						- _VoucherclaimPerReasonForVisitSummary
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 June 2011
-- CR No.:			CRE11-007
-- Description:		Modify the report due to terminated accounts
-- =============================================
-- =============================================    
-- Modification History    
-- Modified by:		Lawrence TSANG
-- Modified date:	23 December 2010
-- Description:		Handle cross-year
-- =============================================  
-- =============================================    
-- Modification History    
-- Modified by:  Eric Tse    
-- Modified date: 25 October 2010    
-- Description: Modify the store procedure to fit with new report layout standard  
--    Change the name of store procedure from proc_EHS_VoucherAccClaim_Stat_Schedule_get  
--    (1) Build the content page  
-- =============================================    
-- =============================================  
-- Author:  Kathy LEE  
-- Create date: 03 Oct 2008  
-- Description: Statistics for getting voucher account and claim  
--    1. Get related data to temp table (_VoucherSummary,   
--     _VoucherAccSummary, _VoucherClaimByProfSummary, _VoucherclaimPerVoucherSummary,  
--     _VoucherclaimPerReasonForVisitSummary, _TransactionSummary)  
-- =============================================  
-- =============================================  
-- Author:  Kathy LEE  
-- Create date: 16 Oct 2009  
-- Description: Statistics for getting voucher account and claim  
--    1. Get related data to temp table (_VoucherSummary,   
--     _VoucherAccSummary, _VoucherClaimByProfSummary, _VoucherclaimPerVoucherSummary,  
--     _VoucherclaimPerReasonForVisitSummary, _TransactionSummary,   
--     _VoucherAccBalanceSummary, _UsageZeroBalanceVoucherAcc)  
-- =============================================  
-- =============================================    
-- Modification History    
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================  

--exec proc_EHS_eHSD0001_HCVS_Stat_Schedule_get

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0001_HCVS_Stat_Schedule_get]  
	@ReportDtm	datetime = NULL
AS BEGIN  

 SET NOCOUNT ON;  

-- =============================================
-- Report setting
-- =============================================
  
	IF @ReportDtm IS NULL BEGIN
		SET @ReportDtm = DATEADD(dd, -1, CONVERT(VARCHAR(11), GETDATE(), 106))	-- "106" gives "dd MMM yyyy"
	END

declare @reporting_period as varchar(50)  
  
set @reporting_period = 'Reporting period: as at ' + CONVERT(VARCHAR(10), @reportDtm, 111)  

DECLARE @Str_NA as varchar(10)
DECLARE @Str_Valid varchar(10)
DECLARE @Str_ConnectionFailed varchar(50)

SELECT @Str_NA = Description 				FROM SystemResource WITH (NOLOCK) WHERE ObjectType = 'Text' AND ObjectName='NA'
SELECT @Str_Valid = Description 			FROM SystemResource WITH (NOLOCK) WHERE ObjectType = 'Text' AND ObjectName='OCSSSResultValid'
SELECT @Str_ConnectionFailed = Description 	FROM SystemResource WITH (NOLOCK) WHERE ObjectType = 'Text' AND ObjectName='OCSSSResultConnectionFailed'



-- =============================================
-- Declaration
-- =============================================
	
---- declare worksheet key in eHSD0001-HCVS-claim  
declare @wsContent varchar(30)  
declare @wsSummary varchar(30)  
declare @ws01 varchar(30)  
declare @ws02 varchar(30)  
declare @ws03 varchar(30)  
declare @ws04 varchar(30)  
declare @ws05 varchar(30)  
declare @ws06a varchar(30)  
declare @ws06b varchar(30)  
declare @ws07a varchar(30)  
declare @ws07b varchar(30)  
declare @ws08a varchar(30)  
declare @ws08b varchar(30)  
declare @ws08c varchar(30)  

declare @wsContent_ct int		
declare @wsSummary_ct int		
declare @ws01_ct int			
declare @ws02_ct int			
declare @ws03_ct int			
declare @ws04_ct int			
declare @ws05_ct int			
declare @ws06a_ct int			
declare @ws06b_ct int			
declare @ws07a_ct int			
declare @ws07b_ct int			
declare @ws08a_ct int			
declare @ws08b_ct int			
declare @ws08c_ct int			

  
---- init worksheet key  
set @wsContent = 'Content'  
set @wsSummary = 'Summary'  
set @ws01 = '01-eHA'  
set @ws02 = '02-available voucher'  
set @ws03 = '03-write off voucher'  
set @ws04 = '04-given voucher'  
set @ws05 = '05-professional (HCVS)'  
set @ws06a = '06a-per tx by prof (HCVS)'  
set @ws06b = '06b-per tx by prof (HCVSDHC)'  
set @ws07a = '07a-reason (HCVS)'  
set @ws07b = '07b-reason (HCVSDHC)'  
set @ws08a = '08a-raw (HCVS)' 
set @ws08b = '08b-raw (HCVSDHC)' 
set @ws08c = '08c-raw (HCVSCHN)' 

---- initialize the counter	(CRP11-009)
set @wsContent_ct = 1
set @wsSummary_ct = 1
set @ws01_ct = 1
set @ws02_ct = 1
set @ws03_ct = 1
set @ws04_ct = 1
set @ws05_ct = 1
set @ws06a_ct = 1
set @ws06b_ct = 1
set @ws07a_ct = 1
set @ws07b_ct = 1
set @ws08a_ct = 1
set @ws08b_ct = 1
set @ws08c_ct = 1

---- Prepare ResultSet ----   
---- as of 1 Nov 2010 the max columns are less than 21   
declare @WorkBook table
(  
WorkSheetID varchar(30),  
Result01 nvarchar(200) default '',  
Result02 nvarchar(200) default '',  
Result03 nvarchar(100) default '',  
Result04 nvarchar(100) default '',  
Result05 nvarchar(100) default '',  
Result06 nvarchar(100) default '',  
Result07 nvarchar(200) default '',  
Result08 nvarchar(200) default '',  
Result09 nvarchar(200) default '',  
Result10 nvarchar(200) default '',  
Result11 nvarchar(200) default '',  
Result12 nvarchar(200) default '',  
Result13 nvarchar(200) default '',  
Result14 nvarchar(200) default '',  
Result15 nvarchar(200) default '',  
Result16 nvarchar(200) default '',  
Result17 nvarchar(200) default '',  
Result18 nvarchar(200) default '',  
Result19 nvarchar(200) default '',   
Result20 nvarchar(200) default '',
-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
Result21 nvarchar(200) default '',
Result22 nvarchar(200) default '',
Result23 nvarchar(200) default '',
Result24 nvarchar(200) default '',
Result25 nvarchar(200) default '',
Result26 nvarchar(200) default '',
Result27 nvarchar(200) default '',
Result28 nvarchar(200) default '',
Result29 nvarchar(200) default '',
Result30 nvarchar(200) default '',
Result31 nvarchar(200) default '',
Result32 nvarchar(200) default '',
Result33 nvarchar(200) default '',
Result34 nvarchar(200) default '',
Result35 nvarchar(200) default '',
Result36 nvarchar(200) default '',
Result37 nvarchar(200) default '',
Result38 nvarchar(200) default '',
Result39 nvarchar(200) default '',
Result40 nvarchar(200) default '',
Result41 nvarchar(200) default '',
Result42 nvarchar(200) default '',
Result43 nvarchar(200) default '',
Result44 nvarchar(200) default '',
Result45 nvarchar(200) default '',
Result46 nvarchar(200) default '',
Result47 nvarchar(200) default '',
Result48 nvarchar(200) default '',
Result49 nvarchar(200) default '',
Result50 nvarchar(200) default '',
Result51 nvarchar(200) default '',
Result52 nvarchar(200) default '',
Result53 nvarchar(200) default '',
Result54 nvarchar(200) default '',
Result55 nvarchar(200) default '',
Result56 nvarchar(200) default '',
Result57 nvarchar(200) default '',
Result58 nvarchar(200) default '',
Result59 nvarchar(200) default '',
Result60 nvarchar(200) default '',
Result61 nvarchar(200) default '',
Result62 nvarchar(200) default '',
Result63 nvarchar(200) default '',
Result64 nvarchar(200) default '',
Result65 nvarchar(200) default '',
Result66 nvarchar(200) default '',
Result67 nvarchar(200) default '',
Result68 nvarchar(200) default '',
Result69 nvarchar(200) default '',
Result70 nvarchar(200) default '',
Result71 nvarchar(200) default '',
Result72 nvarchar(200) default '',
Result73 nvarchar(200) default '',
Result74 nvarchar(200) default '',
Result75 nvarchar(200) default '',
Result76 nvarchar(200) default '',
Result77 nvarchar(200) default '',
Result78 nvarchar(200) default '',
Result79 nvarchar(200) default '',
Result80 nvarchar(200) default '',
Result81 nvarchar(200) default '',
Result82 nvarchar(200) default '',
Result83 nvarchar(200) default '',
Result84 nvarchar(200) default '',
Result85 nvarchar(200) default '',
Result86 nvarchar(200) default '',
Result87 nvarchar(200) default '',
Result88 nvarchar(200) default '',
Result89 nvarchar(200) default '',
Result90 nvarchar(200) default '',
Result91 nvarchar(200) default '',
Result92 nvarchar(200) default '',
Result93 nvarchar(200) default '',
Result94 nvarchar(200) default '',
Result95 nvarchar(200) default '',
Result96 nvarchar(200) default '',
Result97 nvarchar(200) default '',
Result98 nvarchar(200) default '',
Result99 nvarchar(200) default '',
Result100 nvarchar(200) default '',
-- CRE11-024-02 HCVS Pilot Extension Part 2 [End]
DisplaySeq int					-- added for CRP11-009		
)  


DECLARE @sub_rpt_name_01 nvarchar(200)
DECLARE @sub_rpt_name_02 nvarchar(200)
DECLARE @sub_rpt_name_03 nvarchar(200)
DECLARE @sub_rpt_name_04 nvarchar(200)
DECLARE @sub_rpt_name_05 nvarchar(200)
DECLARE @sub_rpt_name_06a nvarchar(200)
DECLARE @sub_rpt_name_06b nvarchar(200)
DECLARE @sub_rpt_name_07a nvarchar(200)
DECLARE @sub_rpt_name_07b nvarchar(200)
DECLARE @sub_rpt_name_08a nvarchar(200)
DECLARE @sub_rpt_name_08b nvarchar(200)
DECLARE @sub_rpt_name_08c nvarchar(200)

-- =============================================
-- Build frame
-- =============================================
 
SET  @sub_rpt_name_01 = 'Report on Number of eHealth (Subsidies) Accounts (covering Voucher Scheme only)'
SET  @sub_rpt_name_02 = 'Report on Available Voucher Summary on eHealth (Subsidies) Account (covering Voucher Scheme only)'
SET  @sub_rpt_name_03 = 'Report on Write Off Voucher Summary on eHealth (Subsidies) Account (covering Voucher Scheme only)'
SET  @sub_rpt_name_04 = 'Report on Total Entitled Voucher Summary on eHealth (Subsidies) Account (covering Voucher Scheme only)'
SET  @sub_rpt_name_05 = 'Report on Voucher Amount Claimed by Profession (HCVS)'
SET  @sub_rpt_name_06a = 'Report on Voucher Amount Claim per Transaction by Profession (HCVS)'
SET  @sub_rpt_name_06b = 'Report on Voucher Amount Claim per Transaction by Profession (HCVSDHC)'
SET  @sub_rpt_name_07a = 'Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVS)'
SET  @sub_rpt_name_07b = 'Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVSDHC)'
SET  @sub_rpt_name_08a = 'Raw data of Voucher Claim Transactions (HCVS)'
SET  @sub_rpt_name_08b = 'Raw data of Voucher Claim Transactions (HCVSDHC)'
SET  @sub_rpt_name_08c = 'Raw data of Voucher Claim Transactions (HCVSCHN)'
 
DECLARE @WriteOffDisclaimer varchar(200)
SET @WriteOffDisclaimer = '* Include eHealth (Subsidies) Account with current year write off record only'
 
---- Generate static layout ----    
---- Content  
-- (changed by CRP11-009)
/*
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'Sub Report ID','Sub Report Name')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-01','Report on Number of eHealth Accounts (for HCVS-eligible only)')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-02','Report on Balance Summary of eHealth Accounts (for HCVS-eligible only)')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-03','Report on Usage of Zero Balance eHealth Accounts (for HCVS-eligible only)')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-04','Report on Number of Voucher Claimed by Profession')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-05','Report on Number of Voucher Claim per Transaction by Profession')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-06','Report on Number of Claim Transactions by Profession and Reason for Visit Level 1')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, 'eHSD0001-07','Raw data of Voucher Claim Transactions')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02) VALUES (@wsContent, '','')  
INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@wsContent, 'Report Generation Time: ' + CONVERT(VARCHAR(10), getdate(), 111) + ' ' + CONVERT(VARCHAR(5), getdate(), 114))  
*/
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'Sub Report ID','Sub Report Name',@wsContent_ct) 
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-01',@sub_rpt_name_01,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-02',@sub_rpt_name_02,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-03',@sub_rpt_name_03,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-04',@sub_rpt_name_04,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-05',@sub_rpt_name_05,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-06a',@sub_rpt_name_06a,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-06b',@sub_rpt_name_06b,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-07a',@sub_rpt_name_07a,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-07b',@sub_rpt_name_07b,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-08a',@sub_rpt_name_08a,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-08b',@sub_rpt_name_08b,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0001-08c',@sub_rpt_name_08c,@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, '','',@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@wsContent, 'Report Generation Time: ' + CONVERT(VARCHAR(10), getdate(), 111) + ' ' + CONVERT(VARCHAR(5), getdate(), 114),@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1

---- Summary  
-- (changed by CRP11-009)
/*
INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@wsSummary, 'Summary of cumulative totals:')  
INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@wsSummary, '')  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03) VALUES (@wsSummary, 'No. of Vouchers Claimed','No. of Transaction','No. of SP')  
*/
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@wsSummary, 'Summary of cumulative totals:',@wsSummary_ct)  
SELECT @wsSummary_ct=@wsSummary_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@wsSummary, '',@wsSummary_ct)  
SELECT @wsSummary_ct=@wsSummary_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq) VALUES (@wsSummary, '', 'Total Voucher Amount Claimed ($)','No. of Transaction','No. of SP',@wsSummary_ct)  
SELECT @wsSummary_ct=@wsSummary_ct + 1
---- 01-eHA  
-- (changed by CRP11-009)
/*
INSERT INTO @WorkBook (WorkSheetID, Result01)   
VALUES (@ws01, 'eHSD0001-01' + ': ' + 'Report on Number of eHealth Accounts (for HCVS-eligible only)')  
INSERT INTO @WorkBook (WorkSheetID) VALUES (@ws01)  
INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@ws01, @reporting_period)  
INSERT INTO @WorkBook (WorkSheetID) VALUES (@ws01)  

INSERT INTO @WorkBook (WorkSheetID, Result01)
VALUES (@ws01, 'eHealth Account Summary (Exclude Terminated Accounts)')  

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05)   
VALUES (@ws01, 'Without Voucher Claim','With Voucher Claim','Total','','Total No. of Validated Account')  
*/

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws01, 'eHS(S)D0001-01' + ': ' + @sub_rpt_name_01, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws01, @reporting_period, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)
VALUES (@ws01, '', 'eHealth (Subsidies) Account Summary (Exclude Terminated Accounts)', @ws01_ct)
SELECT @ws01_ct=@ws01_ct + 1  

--INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq)   
--VALUES (@ws01, '', 'Without Voucher Claim','With Voucher Claim','Total','','Total No. of Validated Account', @ws01_ct) 
--SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, DisplaySeq)   
VALUES (@ws01, '', 'Without Voucher Claim', '', 'With Voucher Claim', '', 'Total', '', '','Total No. of Validated Account', @ws01_ct) 
SELECT @ws01_ct=@ws01_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, DisplaySeq)   
VALUES (@ws01, '', 'Alive', 'Deceased', 'Alive', 'Deceased', 'Alive', 'Deceased', @ws01_ct) 
SELECT @ws01_ct=@ws01_ct + 1

---- 02-eHA  
-- (changed by CRP11-009)
/*
INSERT INTO @WorkBook (WorkSheetID, Result01)   
VALUES (@ws02, 'eHSD0001-02' + ': ' +'Report on Balance Summary of eHealth Accounts (for HCVS-eligible only)')  
INSERT INTO @WorkBook (WorkSheetID) VALUES (@ws02)  
INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@ws02, @reporting_period)  
INSERT INTO @WorkBook (WorkSheetID) VALUES (@ws02)  

INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@ws02, 'Voucher Balance Summary (Exclude Terminated Accounts)')
*/
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws02, 'eHS(S)D0001-02' + ': ' +@sub_rpt_name_02,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws02,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws02, @reporting_period,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws02,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws02, 'Available Voucher Summary (Exclude Terminated Accounts)',@ws02_ct)
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq) VALUES 
(@ws02, '', 'Alive + Deceased', '', 'Alive', '', 'Deceased',@ws02_ct)
SELECT @ws02_ct=@ws02_ct + 1
--INSERT INTO @WorkBook (WorkSheetID, Result01,Result02, Result03, DisplaySeq) VALUES 
--			(@ws02, 'Available Voucher Amount Range ($)','No. of eHealth (Subsidies) Account*','Total Voucher Amount ($)', @ws02_ct)
--SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, DisplaySeq) VALUES 
			(@ws02, 'Available Voucher Amount Range ($)',
			'No. of eHealth (Subsidies) Account*','Total Voucher Amount ($)', 
			'No. of eHealth (Subsidies) Account*','Total Voucher Amount ($)', 
			'No. of eHealth (Subsidies) Account*','Total Voucher Amount ($)', 
			@ws02_ct)
SELECT @ws02_ct=@ws02_ct + 1

---- 03-eHA  
-- (changed by CRP11-009)
/*
INSERT INTO @WorkBook (WorkSheetID, Result01)   
VALUES (@ws03, 'eHSD0001-03: Report on Usage of Zero Balance eHealth Accounts (for HCVS-eligible only)')  
INSERT INTO @WorkBook (WorkSheetID) VALUES (@ws03)  
INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@ws03, @reporting_period)  
INSERT INTO @WorkBook (WorkSheetID) VALUES (@ws03)  

INSERT INTO @WorkBook (WorkSheetID, Result01) VALUES (@ws03, 'Usage of Zero Balance Summary (Exclude Terminated Accounts)')  
*/
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws03, 'eHS(S)D0001-03' + ': ' +@sub_rpt_name_03, @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws03, @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws03, @reporting_period, @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws03, @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws03, 'Write Off Voucher Summary (Exclude Terminated Accounts)', @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq) VALUES 
(@ws03, '', 'Alive + Deceased', '', 'Alive', '', 'Deceased', @ws03_ct)
SELECT @ws03_ct=@ws03_ct + 1
--INSERT INTO @WorkBook (WorkSheetID, Result01, Result02,Result03, DisplaySeq) VALUES 
INSERT INTO @WorkBook (WorkSheetID, Result01, 
Result02, Result03, 
Result04, Result05, 
Result06, Result07, 
DisplaySeq) VALUES 
(@ws03, 
'Write Off Voucher Amount Range ($)', 'No. of eHealth (Subsidies) Account*',
'Write Off Voucher Amount Range ($)', 'No. of eHealth (Subsidies) Account*',
'Write Off Voucher Amount Range ($)', 'No. of eHealth (Subsidies) Account*',
'Total Voucher Amount ($)', @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1

--04

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws04, 'eHS(S)D0001-04' + ': ' +@sub_rpt_name_04, @ws04_ct)  
SELECT @ws04_ct=@ws04_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws04, @ws04_ct)  
SELECT @ws04_ct=@ws04_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws04, @reporting_period, @ws04_ct)  
SELECT @ws04_ct=@ws04_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws04, @ws04_ct)  
SELECT @ws04_ct=@ws04_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws04, 'Total Entitled Voucher Summary (Exclude Terminated Accounts)', @ws04_ct)  
SELECT @ws04_ct=@ws04_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq) VALUES 
(@ws04, '', 'Alive + Deceased', '', 'Alive', '', 'Deceased', @ws04_ct)
SELECT @ws04_ct=@ws04_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, DisplaySeq) VALUES 
(@ws04, 'Entitled Voucher Amount ($)*', 
'No. of eHealth (Subsidies) Account','Total Voucher Amount ($)', 
'No. of eHealth (Subsidies) Account','Total Voucher Amount ($)', 
'No. of eHealth (Subsidies) Account','Total Voucher Amount ($)', 
@ws04_ct)  
SELECT @ws04_ct=@ws04_ct+1

---- 05-Prof (HCVS)

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws05, 'eHS(S)D0001-05' + ': ' +@sub_rpt_name_05, @ws05_ct)  
SELECT @ws05_ct=@ws05_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws05, @ws05_ct)  
SELECT @ws05_ct=@ws05_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws05, @reporting_period, @ws05_ct)  
SELECT @ws05_ct=@ws05_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws05, @ws05_ct)   
SELECT @ws05_ct=@ws05_ct+1


INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10, Result11, Result12, DisplaySeq)  
VALUES (@ws05, 'ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','Total',@ws05_ct)  
SELECT @ws05_ct=@ws05_ct+1


---- 06a-Per Tx by Prof (HCVS)
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws06a, 'eHS(S)D0001-06a' + ': ' +@sub_rpt_name_06a,@ws06a_ct) 
SELECT @ws06a_ct=@ws06a_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws06a,@ws06a_ct)  
SELECT @ws06a_ct=@ws06a_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws06a, @reporting_period,@ws06a_ct)  
SELECT @ws06a_ct=@ws06a_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws06a,@ws06a_ct)  
SELECT @ws06a_ct=@ws06a_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10, Result11, Result12, Result13, DisplaySeq)  
VALUES (@ws06a, 'Voucher amount claimed per transaction ($)','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','Total',@ws06a_ct)  
SELECT @ws06a_ct=@ws06a_ct+1


---- 06b-Per Tx by Prof (HCVSDHC)
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws06b, 'eHS(S)D0001-06b' + ': ' +@sub_rpt_name_06b,@ws06b_ct) 
SELECT @ws06b_ct=@ws06b_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws06b,@ws06b_ct)  
SELECT @ws06b_ct=@ws06b_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws06b, @reporting_period,@ws06b_ct)  
SELECT @ws06b_ct=@ws06b_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws06b,@ws06b_ct)  
SELECT @ws06b_ct=@ws06b_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
VALUES (@ws06b, 'Voucher amount claimed per transaction ($)','DIT', 'POD', 'SPT','Total',@ws06b_ct)  
SELECT @ws06b_ct=@ws06b_ct+1


---- 07a-Reason (HCVS)

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws07a, 'eHS(S)D0001-07a' + ': ' +@sub_rpt_name_07a, @ws07a_ct)  
SELECT @ws07a_ct=@ws07a_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws07a, @ws07a_ct)  
SELECT @ws07a_ct=@ws07a_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws07a, @reporting_period, @ws07a_ct)  
SELECT @ws07a_ct=@ws07a_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws07a, @ws07a_ct)  
SELECT @ws07a_ct=@ws07a_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10, Result11, Result12, Result13, DisplaySeq)  
VALUES (@ws07a, 'Principal Reason for Visit (Level 1)','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','Total',@ws07a_ct)   
SELECT @ws07a_ct=@ws07a_ct+1


---- 07b-Reason (HCVSDHC)

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws07b, 'eHS(S)D0001-07b' + ': ' +@sub_rpt_name_07b, @ws07b_ct)  
SELECT @ws07b_ct=@ws07b_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws07b, @ws07b_ct)  
SELECT @ws07b_ct=@ws07b_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws07b, @reporting_period, @ws07b_ct)  
SELECT @ws07b_ct=@ws07b_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws07b, @ws07b_ct)  
SELECT @ws07b_ct=@ws07b_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
VALUES (@ws07b, 'Principal Reason for Visit (Level 1)','DIT', 'POD', 'SPT','Total',@ws07b_ct)   
SELECT @ws07b_ct=@ws07b_ct+1



---- 08a-Raw (HCVS)
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws08a, 'eHS(S)D0001-08a' + ': ' +@sub_rpt_name_08a, @ws08a_ct)  
SELECT @ws08a_ct=@ws08a_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws08a, @ws08a_ct)  
SELECT @ws08a_ct=@ws08a_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws08a, 'Reporting period: the week ending ' + CONVERT(VARCHAR(10), @reportDtm, 111), @ws08a_ct)  
SELECT @ws08a_ct=@ws08a_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws08a, @ws08a_ct)  
SELECT @ws08a_ct=@ws08a_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, 
						Result08, Result09, Result10, Result11, Result12, Result13, Result14, Result15, 
						Result16, Result17, Result18, DisplaySeq)  
VALUES (@ws08a, 'Transaction ID','Transaction Time','SPID','Practice No.','Practice Name (In English)','Practice Name (In Chinese)','Voucher Amount Claimed ($)',
				'Net Service Fee Charged ($)','Profession','District','District Board','Area','Transaction Status','Reimbursement Status','Means of Input', 
				'HKIC Symbol', 'OCSSS Checking Result', 'DHC-related Services', @ws08a_ct)  
SELECT @ws08a_ct=@ws08a_ct+1


---- 08b-Raw (HCVSDHC)
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws08b, 'eHS(S)D0001-08b' + ': ' +@sub_rpt_name_08b, @ws08b_ct)  
SELECT @ws08b_ct=@ws08b_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws08b, @ws08b_ct)  
SELECT @ws08b_ct=@ws08b_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws08b, 'Reporting period: the week ending ' + CONVERT(VARCHAR(10), @reportDtm, 111), @ws08b_ct)  
SELECT @ws08b_ct=@ws08b_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws08b, @ws08b_ct)  
SELECT @ws08b_ct=@ws08b_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, 
						Result08, Result09, Result10, Result11, Result12, Result13, Result14, Result15, 
						Result16, Result17, DisplaySeq)  
VALUES (@ws08b, 'Transaction ID','Transaction Time','SPID','Practice No.','Practice Name (In English)','Practice Name (In Chinese)','Voucher Amount Claimed ($)',
				'Net Service Fee Charged ($)','Profession','District','District Board','Area','Transaction Status','Reimbursement Status','Means of Input', 
				'HKIC Symbol', 'OCSSS Checking Result', @ws08b_ct)  
SELECT @ws08b_ct=@ws08b_ct+1


---- 08c-Raw (HCVSCHN)
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws08c, 'eHS(S)D0001-08c' + ': ' +@sub_rpt_name_08c, @ws08c_ct)  
SELECT @ws08c_ct=@ws08c_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws08c, @ws08c_ct)  
SELECT @ws08c_ct=@ws08c_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws08c, 'Reporting period: the week ending ' + CONVERT(VARCHAR(10), @reportDtm, 111), @ws08c_ct)  
SELECT @ws08c_ct=@ws08c_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws08c, @ws08c_ct)  
SELECT @ws08c_ct=@ws08c_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, 
						Result08, Result09, Result10, Result11, Result12, Result13, Result14, Result15, 
						Result16, Result17, Result18, Result19, Result20, DisplaySeq)  
VALUES (@ws08c, 'Transaction ID','Transaction Time','SPID','Practice No.','Practice Name (In English)','Practice Name (In Chinese)','Voucher Amount Claimed ($)',
				N'Voucher Amount Claimed (¢D)','Conversion Rate',N'Net Service Fee Charged (¢D)','Payment Type','Profession','District','District Board','Area',
				'Transaction Status','Reimbursement Status','Means of Input','HKIC Symbol', 'OCSSS Checking Result' , @ws08c_ct)  
SELECT @ws08c_ct=@ws08c_ct+1


-- =================================================================
-- Summary of cumulative totals
-- ================================================================= 
-- (changed by CRP11-009)
/*
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03)  
 select @wsSummary, noOfVoucherClaimed, noOfTransaction , noOfSP  from RpteHSD0001VoucherSummary  
 where report_dtm = @reportDtm  
  */
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq)  
 select @wsSummary, 'Any Voucher Scheme', noOfVoucherClaimed, noOfTransaction , noOfSP, @wsSummary_ct  from RpteHSD0001VoucherSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm AND Scheme_Code = 'ALL'
SELECT @wsSummary_ct=@wsSummary_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq)  
 select @wsSummary, Scheme_Code, noOfVoucherClaimed, noOfTransaction , noOfSP, @wsSummary_ct  from RpteHSD0001VoucherSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm AND Scheme_Code = 'HCVS'
SELECT @wsSummary_ct=@wsSummary_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq)  
 select @wsSummary, Scheme_Code, noOfVoucherClaimed, noOfTransaction , noOfSP, @wsSummary_ct  from RpteHSD0001VoucherSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm AND Scheme_Code = 'HCVSCHN'
SELECT @wsSummary_ct=@wsSummary_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq)  
 select @wsSummary, Scheme_Code, noOfVoucherClaimed, noOfTransaction , noOfSP, @wsSummary_ct  from RpteHSD0001VoucherSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm AND Scheme_Code = 'HCVSDHC'
SELECT @wsSummary_ct=@wsSummary_ct+1

-- =================================================================
-- eHSD0001-01: Report on Number of eHealth Accounts (for HCVS-eligible only)
-- ================================================================= 

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, DisplaySeq)  
select @ws01, 'Any Voucher Scheme', 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		AliveTotalAcc, DeceasedTotalAcc, 
		'', ValidAcc, @ws01_ct 
from RpteHSD0001_01_eHA_Summary WITH (NOLOCK)  
where report_dtm = @reportDtm AND Scheme_Code = 'ALL'
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
select @ws01, Scheme_Code, 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		@ws01_ct 
from RpteHSD0001_01_eHA_Summary WITH (NOLOCK)  
where report_dtm = @reportDtm AND Scheme_Code = 'HCVS'
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
select @ws01, Scheme_Code, 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		@ws01_ct 
from RpteHSD0001_01_eHA_Summary WITH (NOLOCK)  
where report_dtm = @reportDtm AND Scheme_Code = 'HCVSCHN'
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
select @ws01, Scheme_Code, 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		@ws01_ct 
from RpteHSD0001_01_eHA_Summary WITH (NOLOCK)  
where report_dtm = @reportDtm AND Scheme_Code = 'HCVSDHC'
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, DisplaySeq)
VALUES (@ws01, @ws01_ct)
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)
VALUES (@ws01, 'Validated Account Breakdown by Status', @ws01_ct) 
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq)   
VALUES (@ws01, 'Active', 'Suspended', 'Terminated', 'Total', @ws01_ct)
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, DisplaySeq)   
SELECT
	@ws01, ValidateAccountActive, ValidateAccountSuspend, ValidateAccountTerminate, ValidateAccountTotal, @ws01_ct
FROM
	RpteHSD0001_01_eHA_Summary WITH (NOLOCK)  
WHERE
	report_dtm = @ReportDtm AND Scheme_Code = 'ALL'
SELECT @ws01_ct=@ws01_ct+1


-- =================================================================
-- eHSD0001-02: Report on Available Voucher Summary on eHealth Account (for HCVS-eligible only)
-- =================================================================

INSERT INTO @WorkBook (WorkSheetID, Result01, 
						Result02, Result03,
						Result04, Result05,
						Result06, Result07,
						 DisplaySeq) 
SELECT @ws02, voucherBalance, 
		noOfVoucherAC, 
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased, 
		cast(@ws02_ct as int) + (Display_Seq + 1) 
FROM RpteHSD0001_02_AvailableVoucher_Summary WITH (NOLOCK)   
WHERE report_dtm = @reportDtm 
--AND SubReport_ID = '02'
ORDER BY Display_Seq

SELECT @ws02_ct= cast(max(DisplaySeq) as int) + 1 from @WorkBook where WorkSheetID = @ws02

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES
(@ws02, @WriteOffDisclaimer, @ws02_ct)
SELECT @ws02_ct=@ws02_ct + 1

INSERT INTO @WorkBook (WorkSheetID, DisplaySeq)
VALUES (@ws02,@ws02_ct)
SELECT @ws02_ct = @ws02_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)
VALUES (@ws02, 'Voucher Usage Summary of Terminated Accounts',@ws02_ct)
SELECT @ws02_ct = @ws02_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)
SELECT
	@ws02, 'No. of Voucher Account', ValidateAccountTerminate,@ws02_ct
FROM
	--RpteHSD0001VoucherAccSummary 
	RpteHSD0001_01_eHA_Summary WITH (NOLOCK) 
WHERE
	report_dtm = @ReportDtm
		AND Scheme_Code = 'ALL'

SELECT @ws02_ct = @ws02_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)
SELECT
	@ws02, 'Total Voucher Amount ($)', TerminateVoucher,@ws02_ct
FROM
	--RpteHSD0001VoucherAccSummary 
	RpteHSD0001_01_eHA_Summary WITH (NOLOCK)  
WHERE
	report_dtm = @ReportDtm
			AND Scheme_Code = 'ALL'

SELECT @ws02_ct = @ws02_ct + 1


-- =================================================================
-- eHSD0001-03: eHSD0001-03: Report on Write Off Voucher Summary of eHealth Accounts (for HCVS-eligible only)
-- =================================================================

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, 
		Result04, Result05, 
		Result06, Result07, 
		DisplaySeq) 
SELECT @ws03, voucherBalance, noOfVoucherAC, noOfVoucher, 
	noOfVoucherAC_Alive, noOfVoucher_Alive,  
	noOfVoucherAC_Deceased, noOfVoucher_Deceased, 
	cast(@ws03_ct as int) + (Display_Seq + 1) 
from RpteHSD0001_03_WriteOffVoucher_Summary WITH (NOLOCK)   
WHERE report_dtm = @reportDtm 
--AND SubReport_ID = '03'
ORDER BY Display_Seq 
SELECT @ws03_ct= cast(max(DisplaySeq) as int) + 1 from @WorkBook where WorkSheetID = @ws03

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES
(@ws03, @WriteOffDisclaimer, @ws03_ct)
SELECT @ws03_ct=@ws03_ct + 1


-- =================================================================
-- eHSD0001-04: Report on Given Voucher Summary on eHealth Account (for HCVS-eligible only)
-- =================================================================
  
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, 
		Result04, Result05, 
		Result06, Result07, 
		DisplaySeq) 
SELECT @ws04, voucherBalance, noOfVoucherAC, noOfVoucher, 
	noOfVoucherAC_Alive, noOfVoucher_Alive,  
	noOfVoucherAC_Deceased, noOfVoucher_Deceased, 
	cast(@ws04_ct as int) + (Display_Seq + 1) 
from RpteHSD0001_04_TotalEntitledVoucher_Summary WITH (NOLOCK)   
WHERE report_dtm = @reportDtm 
--AND SubReport_ID = '04'
ORDER BY Display_Seq
SELECT @ws04_ct= cast(max(DisplaySeq) as int) + 1 from @WorkBook where WorkSheetID = @ws04

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES
(@ws04, '*Total entitled voucher is the entitled voucher before discounting of write-off', @ws04_ct)
SELECT @ws04_ct=@ws04_ct + 1

-- =================================================================
-- eHSD0001-05: Report on Voucher Amount Claimed by Profession (HCVS)
-- =================================================================
INSERT INTO @WorkBook (	WorkSheetID, Result01, Result02, Result03, Result04, Result05,  
						Result06, Result07, Result08, Result09, Result10, Result11, Result12, DisplaySeq)  
 select @ws05, ENU, RCM, RCP, RDT, RMP, RMT, RNU, ROP, ROT, RPT, RRD, Total, @ws05_ct from RpteHSD0001_05_VoucherClaimByProfSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm
SELECT @ws05_ct=@ws05_ct+1
 

-- =================================================================
-- eHSD0001-06a: Report on Voucher Amount Claim per Transaction by Profession (HCVS)
-- =================================================================
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10, Result11, Result12, Result13, DisplaySeq)  
 select @ws06a, noOfClaim, ENU, RCM, RCP, RDT, RMP, RMT, RNU, ROP, ROT, RPT, RRD, Total, @ws06a_ct+SortOrder from RpteHSD0001_06a_VoucherClaimPerVoucherSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm  
  order by SortOrder
SELECT @ws06a_ct=@ws06a_ct+(SELECT ISNULL((SELECT IsNULL(max(SortOrder),0) from RpteHSD0001_06a_VoucherClaimPerVoucherSummary WITH (NOLOCK) where report_dtm = @reportDtm),0))


-- =================================================================
-- eHSD0001-06b: Report on Voucher Amount Claim per Transaction by Profession (HCVSDHC)
-- =================================================================
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
 select @ws06b, noOfClaim, DIT, POD, SPT, Total, @ws06b_ct+SortOrder from RpteHSD0001_06b_VoucherClaimPerVoucherSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm  
  order by SortOrder
SELECT @ws06b_ct=@ws06b_ct+(SELECT ISNULL((SELECT IsNULL(max(SortOrder),0) from RpteHSD0001_06b_VoucherClaimPerVoucherSummary WITH (NOLOCK) where report_dtm = @reportDtm),0))



-- =================================================================
-- eHSD0001-07a: Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVS)
-- =================================================================

-- CRE11-024-02 HCVS Pilot Extension Part 2 -- [Start]
-- --------------------------------------------------- --
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10, Result11, Result12, Result13, DisplaySeq)  
 select  @ws07a, reason, ENU, RCM, RCP, RDT, RMP,   
     case isnull(RMT, '')  
     when '' then '0'  
     when -1 then 'N/A'  
     else CONVERT(VARCHAR(10), RMT)  
  end,  
  RNU, ROP, ROT, RPT,RRD, Total, SortOrder+@ws07a_ct from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm and Secondary='N'
 order by SortOrder
SELECT @ws07a_ct=@ws07a_ct+(SELECT ISNULL((SELECT IsNULL(max(SortOrder),0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK) where report_dtm = @reportDtm and Secondary='N'),0))

-- insert an empty row
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws07a, @ws07a_ct)  
SELECT @ws07a_ct=@ws07a_ct+1

-- insert labels for Secondary Reason For Visit
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10, Result11, Result12, Result13, DisplaySeq)  
VALUES (@ws07a, 'Secondary Reason for Visit (Level 1)','ENU','RCM','RCP','RDT','RMP','RMT','RNU','ROP','ROT','RPT','RRD','Total',@ws07a_ct)   
SELECT @ws07a_ct=@ws07a_ct+1


-- insert the Secondary Reason For Visit Data
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10, Result11, Result12, Result13, DisplaySeq)  
 select  @ws07a, reason, ENU, RCM, RCP, RDT, RMP,   
     case isnull(RMT, '')  
     when '' then '0'  
     when -1 then 'N/A'  
     else CONVERT(VARCHAR(10), RMT)  
  end,  
  RNU, ROP, ROT, RPT,RRD, Total, SortOrder+@ws07a_ct from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm and Secondary='Y'
 order by SortOrder
SELECT @ws07a_ct=@ws07a_ct+(SELECT ISNULL((SELECT IsNULL(max(SortOrder),0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK) where report_dtm = @reportDtm and Secondary='Y'),0))


-- =================================================================
-- eHSD0001-07b: Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVS)
-- =================================================================

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
 select  @ws07b, reason, DIT, POD, SPT, Total, SortOrder+@ws07b_ct from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm and Secondary='N'
 order by SortOrder
SELECT @ws07b_ct=@ws07b_ct+(SELECT ISNULL((SELECT IsNULL(max(SortOrder),0) from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK) where report_dtm = @reportDtm and Secondary='N'),0))

-- insert an empty row
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws07b, @ws07b_ct)  
SELECT @ws07b_ct=@ws07b_ct+1

-- insert labels for Secondary Reason For Visit
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
VALUES (@ws07b, 'Secondary Reason for Visit (Level 1)','DIT', 'POD', 'SPT','Total',@ws07b_ct)   
SELECT @ws07b_ct=@ws07b_ct+1


-- insert the Secondary Reason For Visit Data
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, DisplaySeq)  
 select  @ws07b, reason, DIT, POD, SPT, Total, SortOrder+@ws07b_ct from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK)  
 where report_dtm = @reportDtm and Secondary='Y'
 order by SortOrder
SELECT @ws07b_ct=@ws07b_ct+(SELECT ISNULL((SELECT IsNULL(max(SortOrder),0) from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary WITH (NOLOCK) where report_dtm = @reportDtm and Secondary='Y'),0))


-- =================================================================
-- eHSD0001-08a: Raw data of Voucher Claim Transactions (HCVS)
-- =================================================================
INSERT INTO @WorkBook (WorkSheetID, 
						Result01, Result02, Result03, Result04, Result05, 
						Result06, Result07, Result08, Result09, Result10, 
						Result11, Result12, Result13, Result14, Result15, 
						Result16, Result17, Result18, Result19,
						DisplaySeq)  
SELECT  @ws08a,  
	transaction_id,  
	CONVERT(VARCHAR(11), transaction_dtm, 106) + ' ' + CONVERT(VARCHAR(8), transaction_dtm, 108),
	SP_ID, Practice_Display_Seq, Practice_Name, Practice_Name_Chi,
	voucher_claim, Co_Payment, service_type, district_name, district_board, area_name, Transaction_Status, Reimbursement_Status, 
	isnull(Create_By_SmartID,'Manual'), 
	ISNULL(HKIC_Symbol, @Str_NA),
	CASE 
		-- SP Claim and HKIC Symbol C or U
		WHEN  ISNULL(Manual_Reimburse, '') = 'N' AND ISNULL(HKIC_Symbol, '') IN ('C','U') THEN
		CASE 
			WHEN OCSSS_Ref_Status = 'V' THEN @Str_Valid 
			WHEN OCSSS_Ref_Status = 'C' THEN @Str_ConnectionFailed
			WHEN OCSSS_Ref_Status = 'N' THEN @Str_ConnectionFailed
			ELSE @Str_NA
		END
	ELSE 
		-- SP Claim and HKIC Symbol A, R or Others / VU claim / IVRS claim / old claim record
		@Str_NA                                             
	END AS [OCSSS_Ref_Status],   
	
	ISNULL(DHC_Service,'N') AS [DHC_Service],
	row_number() over (order by transaction_dtm),
	@ws08a_ct 	
FROM
	RpteHSD0001_08a_TransactionSummary R WITH (NOLOCK)
WHERE
	report_dtm = @reportDtm  
ORDER BY
	transaction_dtm

SELECT @ws08a_ct=@ws08a_ct+1


-- =================================================================
-- eHSD0001-08b: Raw data of Voucher Claim Transactions (HCVSDHC)
-- =================================================================
INSERT INTO @WorkBook (WorkSheetID, 
						Result01, Result02, Result03, Result04, Result05, 
						Result06, Result07, Result08, Result09, Result10, 
						Result11, Result12, Result13, Result14, Result15, 
						Result16, Result17, Result18,
						DisplaySeq)  
SELECT  @ws08b,  
	transaction_id,  
	CONVERT(VARCHAR(11), transaction_dtm, 106) + ' ' + CONVERT(VARCHAR(8), transaction_dtm, 108),
	SP_ID, Practice_Display_Seq, Practice_Name, Practice_Name_Chi,
	voucher_claim, Co_Payment, service_type, district_name, district_board, area_name, Transaction_Status, Reimbursement_Status, 
	isnull(Create_By_SmartID,'Manual'), 
	ISNULL(HKIC_Symbol, @Str_NA),
	CASE 
		-- SP Claim and HKIC Symbol C or U
		WHEN  ISNULL(Manual_Reimburse, '') = 'N' AND ISNULL(HKIC_Symbol, '') IN ('C','U') THEN
		CASE 
			WHEN OCSSS_Ref_Status = 'V' THEN @Str_Valid 
			WHEN OCSSS_Ref_Status = 'C' THEN @Str_ConnectionFailed
			WHEN OCSSS_Ref_Status = 'N' THEN @Str_ConnectionFailed
			ELSE @Str_NA
		END
	ELSE 
		-- SP Claim and HKIC Symbol A, R or Others / VU claim / IVRS claim / old claim record
		@Str_NA                                             
	END AS [OCSSS_Ref_Status],   
	row_number() over (order by transaction_dtm),
	@ws08b_ct 	
FROM
	RpteHSD0001_08b_HCVSDHCTransactionSummary R WITH (NOLOCK)
WHERE
	report_dtm = @reportDtm  
ORDER BY
	transaction_dtm

SELECT @ws08b_ct=@ws08b_ct+1


-- =================================================================
-- eHSD0001-08c: Raw data of Voucher Claim Transactions (HCVSCHN)
-- =================================================================
INSERT INTO @WorkBook (WorkSheetID,
					   Result01, Result02, Result03, Result04, Result05,
					   Result06, Result07, Result08, Result09, Result10,
					   Result11, Result12, Result13, Result14, Result15,
					   Result16, Result17, Result18, Result19, Result20, 
					   Result21, 
					   DisplaySeq)  
SELECT
	@ws08c,  
	transaction_id, CONVERT(varchar(11), transaction_dtm, 106) + ' ' + CONVERT(varchar(8), transaction_dtm, 108), SP_ID, Practice_Display_Seq, Practice_Name,
	Practice_Name_Chi, Total_Amount_HKD, Total_Amount_RMB, Conversion_Rate, Co_Payment_RMB, Payment_Type,
	service_type, district_name, district_board, area_name, Transaction_Status,
	Reimbursement_Status, isnull(Create_By_SmartID,'Manual'), 
	ISNULL(HKIC_Symbol, @Str_NA),
	CASE 
		-- SP Claim and HKIC Symbol C or U
		WHEN  ISNULL(Manual_Reimburse, '') = 'N' AND ISNULL(HKIC_Symbol, '') IN ('C','U') THEN
		CASE 
			WHEN OCSSS_Ref_Status = 'V' THEN @Str_Valid 
			WHEN OCSSS_Ref_Status = 'C' THEN @Str_ConnectionFailed
			WHEN OCSSS_Ref_Status = 'N' THEN @Str_ConnectionFailed
			ELSE @Str_NA
		END
	ELSE 
	-- SP Claim and HKIC Symbol A, R or Others / VU claim / IVRS claim / old claim record
	@Str_NA                                             
	END AS [OCSSS_Ref_Status],   
	row_number() over (order by transaction_dtm), @ws08c_ct 	
FROM
	RpteHSD0001_08c_HCVSCHNTransactionSummary R WITH (NOLOCK)
WHERE
	report_dtm = @reportDtm  
ORDER BY
	transaction_dtm

SELECT @ws08c_ct=@ws08c_ct+1


---- Get the resultset for whole workbook  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @wscontent  
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @wssummary 
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	 
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws01  
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20,
Result21, Result22, Result23, Result24, Result25,
Result26, Result27, Result28, Result29, Result30,
Result31, Result32, Result33, Result34, Result35,
Result36, Result37, Result38, Result39, Result40,
Result41, Result42, Result43, Result44, Result45,
Result46, Result47, Result48, Result49, Result50 
FROM @WorkBook WHERE WorkSheetID = @ws02  
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20,
Result21, Result22, Result23, Result24, Result25,
Result26, Result27, Result28, Result29, Result30,
Result31, Result32, Result33, Result34, Result35,
Result36, Result37, Result38, Result39, Result40,
Result41, Result42, Result43, Result44, Result45,
Result46, Result47, Result48, Result49, Result50   
FROM @WorkBook WHERE WorkSheetID = @ws03  
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20,
Result21, Result22, Result23, Result24, Result25,
Result26, Result27, Result28, Result29, Result30,
Result31, Result32, Result33, Result34, Result35,
Result36, Result37, Result38, Result39, Result40,
Result41, Result42, Result43, Result44, Result45,
Result46, Result47, Result48, Result49, Result50   
FROM @WorkBook WHERE WorkSheetID = @ws04  
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	  
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws05 
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
 

SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws06a
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	

SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws06b
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws07a 
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws07b
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	

SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,
Result16, Result17, Result18
FROM @WorkBook WHERE WorkSheetID = @ws08a  
ORDER BY DisplaySeq, cast(Result19 as int)	-- added CRP11-009: preserve ordering	( helen fix crp11-009 ordering)

SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,
Result16, Result17
FROM @WorkBook WHERE WorkSheetID = @ws08b 
ORDER BY DisplaySeq, cast(Result18 as int)	-- added CRP11-009: preserve ordering	( helen fix crp11-009 ordering)

SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,
Result16, Result17, Result18, Result19, Result20
FROM @WorkBook WHERE WorkSheetID = @ws08c  
ORDER BY DisplaySeq, cast(Result21 as int)

END   
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0001_HCVS_Stat_Schedule_get] TO HCVU
GO

