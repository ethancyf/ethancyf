IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0023_EHAPP_Stat_Schedule_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0023_EHAPP_Stat_Schedule_get]
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
-- Modification History
-- CR No.:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:		Tommy LAM
-- Modified date:	29 Jan 2014
-- Description:		Rename Table:
--						1. [_TransactionSummaryCopaymentEHAPP] to [RpteHSD0023TransactionSummaryCopaymentEHAPP]
--						2. [_TransactionSummaryEHAPP] to [RpteHSD0023TransactionSummaryEHAPP]
--						3. [_VoucherAccSummaryEHAPP] to [RpteHSD0023VoucherAccSummaryEHAPP]
--					Re-structure of Table - [RpteHSD0023TransactionSummaryCopaymentEHAPP]:
--					For Sub-report - "eHSD0023-02":
--						1. Merge Columns "1 HCV + $50" & "2HCVs" to "Use Voucher"
--						2. Add new Field "Total Voucher Amount Used For Co-payment ($)"
--					For Sub-report - "eHSD0023-03":
--						1. Migrate value "1 HCV + $50" & "2HCVs" of Column "Co-payment" to value "Use Voucher"
--						2. Add new Column "Use HCV Amount ($)"
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0020
-- Modified by:		Koala CHENG
-- Modified date:	30 Aug 2013
-- Description:		Fix to use nvarchar to store chinese data
-- =============================================
-- =============================================
-- Author:		Karl LAM
-- Create date: 16 Apr 2013
-- Description:	Get Stat for report eHSD0023
-- =============================================
--exec proc_EHS_eHSD0023_EHAPP_Stat_Schedule_get '2013-05-01'
/*
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0023_EHAPP_Stat_Schedule_get]  
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
  
declare @wsContent_ct int		
declare @wsSummary_ct int		
declare @ws01_ct int			
declare @ws02_ct int			
declare @ws03_ct int			
declare @ws04_ct int			


---- init worksheet key  
set @wsContent = 'Content'  
set @ws01 = '01'  
set @ws02 = '02'  
set @ws03 = '03'  


set @wsContent_ct = 1
set @ws01_ct = 1
set @ws02_ct = 1
set @ws03_ct = 1

---- Prepare ResultSet ----   
declare @WorkBook table
(  
WorkSheetID varchar(30),  
Result01 nvarchar(300) default '',  
Result02 nvarchar(200) default '',  
Result03 nvarchar(200) default '',  
Result04 nvarchar(200) default '',  
Result05 nvarchar(200) default '',  
Result06 nvarchar(200) default '',  
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
DisplaySeq int					
)  

-- =============================================
-- Build frame
-- =============================================
 
---- Generate static layout ----    
---- Content  

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'Sub Report ID','Sub Report Name',@wsContent_ct) 
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0023-01','Report on Number of eHealth (Subsidies) Accounts (for EHAPP-eligibile only)',@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0023-02','Report on EHAPP transaction',@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)D0023-03','Raw data of EHAPP transaction',@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, '','',@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@wsContent, 'Report Generation Time: ' + CONVERT(VARCHAR(10), getdate(), 111) + ' ' + CONVERT(VARCHAR(5), getdate(), 114),@wsContent_ct)  
SELECT @wsContent_ct=@wsContent_ct+1

--01 sub Report
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws01, 'eHS(S)D0023-01' + ': ' + 'Report on Number of eHealth (Subsidies) Accounts (for EHAPP-eligibile only)', @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws01, @reporting_period, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)
VALUES (@ws01, 'eHealth (Subsidies) Account Summary (Exclude Terminated Accounts)', @ws01_ct)
SELECT @ws01_ct=@ws01_ct + 1  

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq)   
VALUES (@ws01, 'With EHAPP transaction','','Without EHAPP transaction','Total','','Total No. of Validated Account', @ws01_ct) 
SELECT @ws01_ct=@ws01_ct + 1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)   
VALUES (@ws01, 'Validated Account','Temporary account', @ws01_ct) 
SELECT @ws01_ct=@ws01_ct + 1

---- 02
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws02, 'eHS(S)D0023-02' + ': ' +'Report on EHAPP transaction',@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws02,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws02, @reporting_period,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws02,@ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@ws02,'No. of EHAPP transaction', 'No. of involved SP', @ws02_ct)  
SELECT @ws02_ct=@ws02_ct + 1

---- 03  
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)   
VALUES (@ws03, 'eHS(S)D0023-03: Raw data of EHAPP transaction', @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws03, @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws03, 'Reporting period: 2 weeks ending at ' + CONVERT(VARCHAR(10), @reportDtm, 111), @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws03, @ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08,Result09,Result10,Result11,Result12,Result13,Result14,Result15, Result16, Result17, DisplaySeq)  
VALUES (@ws03, 'Transaction ID','Transaction Time','SPID', 'Practice No.', 'Practice Name (In English)', 'Practice Name (In Chinese)','Profession','District','District Board','Area', 'Service Date', 'Doc Type', 'Co-payment', 'Use HCV Amount ($)', 'Transaction Status', 'Means of input', 'With HCVS Claim for EHAPP Co-payment',@ws03_ct)  
SELECT @ws03_ct=@ws03_ct+1

-- =================================================================
-- eHSD0023-01: Report on Number of eHealth Accounts (for HCVS-eligible only)
-- ================================================================= 
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq)  
 select @ws01, ValidateAccountWithEHAPPExcludeTeminatedAcc, TempAccountWithEHAPPExcludeTeminatedAcc, WithoutEHAPPExcludeTeminatedAcc, TotalAcc, '', ValidAcc, @ws01_ct from RpteHSD0023VoucherAccSummaryEHAPP 
 where report_dtm = @reportDtm  
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, DisplaySeq)
VALUES (@ws01, @ws01_ct)
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq)
VALUES (@ws01, 'Validated Account Breakdown by Status', @ws01_ct) 
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq)   
VALUES (@ws01, 'Active', 'Suspended', 'Terminated', '', '', 'Total', @ws01_ct)
SELECT @ws01_ct=@ws01_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, DisplaySeq)   
SELECT
	@ws01, ValidateAccountActive, ValidateAccountSuspend, ValidateAccountTerminate, '', '', ValidateAccountTotal,  @ws01_ct
FROM
	RpteHSD0023VoucherAccSummaryEHAPP
WHERE
	report_dtm = @ReportDtm  
SELECT @ws01_ct=@ws01_ct+1


-- =================================================================
-- eHSD0023-02: Report on EHAPP transaction
-- =================================================================
declare @no_of_trans as varchar(50)
declare @no_of_involved_sp as varchar(50)

SELECT @no_of_trans = Field_Value FROM RpteHSD0023TransactionSummaryCopaymentEHAPP
	WHERE Report_Dtm = @reportDtm AND Field_Name = 'No_Of_Trans'

SELECT @no_of_involved_sp = Field_Value FROM RpteHSD0023TransactionSummaryCopaymentEHAPP
	WHERE Report_Dtm = @reportDtm AND Field_Name = 'No_Of_Involved_SP'

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)
	SELECT	@ws02, @no_of_trans, @no_of_involved_sp, @ws02_ct
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws02, @ws02_ct)
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws02, 'Co-payment Breakdown', @ws02_ct)
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01,Result03, Result04,Result05, Result06, DisplaySeq) VALUES 
(@ws02, 'Use Voucher','''$100', 'CSSA','Medical Wavier', 'Total', @ws02_ct )
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01,Result02, DisplaySeq) VALUES 
(@ws02, 'With HCVS Claim for EHAPP Co-payment', 'Without HCVS Claim for EHAPP Co-payment',@ws02_ct )
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01,Result02, Result03, Result04,Result05,Result06, DisplaySeq) 
SELECT @ws02,
(SELECT Field_Value from RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE Field_Name = 'HCV_With_HCVSClaim' and report_dtm = @reportDtm  ),
(SELECT Field_Value from RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE Field_Name = 'HCV_Without_HCVSClaim' and report_dtm = @reportDtm  ),
(SELECT Field_Value from RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE Field_Name = 'F100'and report_dtm = @reportDtm  ),
(SELECT Field_Value from RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE Field_Name = 'CSSA'and report_dtm = @reportDtm  ),
(SELECT Field_Value from RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE Field_Name = 'MED_WAIVE'and report_dtm = @reportDtm  ),
@no_of_trans, @ws02_ct
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws02, @ws02_ct)
SELECT @ws02_ct=@ws02_ct+1

INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)
SELECT
	@ws02,
	'Total Voucher Amount Used for Co-payment ($)',
	(SELECT Field_Value FROM RpteHSD0023TransactionSummaryCopaymentEHAPP WHERE Field_Name = 'Total_HCVAmount' and report_dtm = @reportDtm),
	@ws02_ct
SELECT @ws02_ct=@ws02_ct+1

-- =================================================================
-- eHSD0023-03: Raw Data
-- =================================================================
INSERT INTO @WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08,Result09,Result10,Result11,Result12,Result13,Result14, Result15, Result16, Result17, DisplaySeq)
 select @ws03,  
   tx.Transaction_ID,  
   CONVERT(VARCHAR(11), tx.transaction_dtm, 106) + ' ' + CONVERT(VARCHAR(8), tx.transaction_dtm, 108), tx.SP_ID,  
   tx.Practice_No , tx.Practice_Name, tx.Practice_Name_Chi, tx.service_type,tx.district_name, tx.district_board, tx.area_name ,  CONVERT(VARCHAR(11), tx.Service_Receive_Dtm, 106),
	dt.Doc_Display_Code, sd.Data_Value,
	Case tx.Copayment 
		when 'HCV' then tx.HCVAmount
		Else 'N/A' end,
	ssd.Status_Description, isnull(Create_By_SmartID,'Manual'),
	Case tx.Copayment 
		when 'HCV' then 
			case tx.WithHCVS when 1 then 'Y' else 'N' end
		Else 'N/A' end
	,@ws03_ct  
 from RpteHSD0023TransactionSummaryEHAPP tx INNER JOIN StaticData sd on sd.Column_Name = 'EHAPP_COPAYMENT' and sd.Item_No = tx.CoPayment
	INNER JOIN StatusData ssd on ssd.Enum_Class = 'ClaimTransStatus' and tx.Record_status = ssd.Status_Value 
	INNER JOIN DocType dt on dt.Doc_Code = tx.Doc_Code
 where tx.report_dtm = @reportDtm  
 order by tx.transaction_dtm


---- Get the resultset for whole workbook  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @wscontent  
ORDER BY DisplaySeq			
  
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20   
FROM @WorkBook WHERE WorkSheetID = @ws01  
ORDER BY DisplaySeq			
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20
FROM @WorkBook WHERE WorkSheetID = @ws02  
ORDER BY DisplaySeq			-- added CRP11-009: preserve ordering	
  
SELECT   
Result01, Result02, Result03, Result04, Result05,  
Result06, Result07, Result08, Result09, Result10,  
Result11, Result12, Result13, Result14, Result15,  
Result16, Result17, Result18, Result19, Result20
FROM @WorkBook WHERE WorkSheetID = @ws03  
ORDER BY DisplaySeq			
  
END   
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0023_EHAPP_Stat_Schedule_get] TO HCVU
GO
*/