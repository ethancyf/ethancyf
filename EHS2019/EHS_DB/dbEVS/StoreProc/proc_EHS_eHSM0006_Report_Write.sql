IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0006_Report_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_EHS_eHSM0006_Report_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	1 Dec 2020
-- CR. No			INT20-0053
-- Description:		Hide SSSCMC Practice
-- =============================================
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
-- Modified by:		Koala CHENG
-- CR No.:			INT18-0004
-- Modified date:	01 Jun 2018
-- Description:		Fix the calculation of aged 70 stat
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- CR No.:			INT18-0002
-- Modified date:	11 May 2018
-- Description:		Add aged 70 stat for internal use, until implemented complete solution for CRE17-017
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- CR No.:			INT17-0027
-- Modified date:	03 Apr 2018
-- Description:		Fix HCVSCHN sub report Deceased Status conversion error
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- CR No.:			CRE16-014 (Add Deceased Status)
-- Modified date:	23 Jan 2018
-- Description:		Align RptAccount SFC logic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- CR No.:			CRE17-002 (Fine-tunning of eHSM0006 Report)
-- Modified date:	04 Jul 2017
-- Description:		Add sub Report [Summary], [04]
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- CR No.:			CRE16-025 (Lowering voucher eligibility age)
-- Create date:		25 May 2017
-- Description:		New report eHS(S)M0006 - Report of Voucher Claim - Monthly Basis
-- =============================================


CREATE PROCEDURE [dbo].[proc_EHS_eHSM0006_Report_Write]
	@cutOffDtm datetime = NULL	-- The date to gen report
AS BEGIN

	SET NOCOUNT ON;
	
--	DECLARE @cutoffdtm as datetime
--	set @cutoffdtm='2017-07-03'
-- =============================================
-- Report setting
-- =============================================

	IF @cutOffDtm IS NULL BEGIN
		SET @cutOffDtm = CONVERT(varchar(11), GETDATE(), 106) + ' 00:00'
	END

	DECLARE @ReportDtm datetime
	SET @ReportDtm = DATEADD(day, -1, @cutOffDtm) -- The date report data as at
	
	-- Voucher Acc created by Back Office
	DECLARE @BO_Voucher_Acc_ID AS VARCHAR(5000) = '01076933,01076935,01076938,01076947'
	
	DECLARE @HKUSZH_SP_ID varchar(8)
	SET @HKUSZH_SP_ID = '00620498'  -- SP under HCVSCHN
	
	--SELECT 
	--	  @HKUSZH_SP_ID = SP_ID
	--FROM 
	--	  SchemeInformation
	--WHERE
	--	  Scheme_Code = @scheme_code_mainland
	--	  AND SP_ID NOT IN (SELECT SP_ID FROM SPExceptionList)


-- =============================================
-- Declaration
-- =============================================

	CREATE TABLE #VoucherTransaction (
		TxCnt					int,
		Scheme_Code				char(10),
		Identity_Num			varbinary(100),
		Age						int,
		Deceased				BIT,
		service_type			char(5),		
		Claim_Amount			int,
		Claim_Amount_RMB		Money,
		Reason_for_visit_L1		smallint default 0,
		ReasonforVisit_S1_L1	smallint default 0,
		ReasonforVisit_S2_L1	smallint default 0,
		ReasonforVisit_S3_L1	smallint default 0,
		Record_Status			char(1),
		Invalidation			char(1),
		SP_ID					char(8),
		Practice_Display_Seq	smallint,
		SFC_Logical_DOD			datetime,
		DOB						datetime
	)



	-- For 01 tab Part 3
	CREATE table #VoucherAccount (
		Identity_Num			VARBINARY(100),
		Age						INT,
		Deceased				BIT
	)

	---- For 01 tab Part 4
	--DECLARE @VA_CreatedByBO AS TABLE (
	--	Voucher_Acc_ID			CHAR(15),
	--	Identity_Num			VARBINARY(100),
	--	--Deceased				CHAR(1)
	--	Deceased				BIT
	--)

	
	-- For 03 tab - Seperate a transaction into Several reason
	CREATE TABLE #VT_ReasonForVisit (
		Scheme_Code				char(10),
		TxCnt					int,
		age						int,
		service_type			char(5),		
		Reason_Code				INT,
		Secondary				CHAR(1)
	)


	--Result Table
	DECLARE @ResultTableSummary AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable01 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT '',	
		Result_Value6 varchar(100) DEFAULT '',	
		Result_Value7 varchar(100) DEFAULT '',	
		Result_Value8 varchar(100) DEFAULT '',	
		Result_Value9 varchar(100) DEFAULT '',	
		Result_Value10 varchar(100) DEFAULT '',
		Result_Value11 varchar(100) DEFAULT '',
		Result_Value12 varchar(100) DEFAULT '',
		Result_Value13 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable02 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT ''
	)


	DECLARE @ResultTable03 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT '',	
		Result_Value6 varchar(100) DEFAULT '',	
		Result_Value7 varchar(100) DEFAULT '',	
		Result_Value8 varchar(100) DEFAULT '',	
		Result_Value9 varchar(100) DEFAULT '',	
		Result_Value10 varchar(100) DEFAULT '',
		Result_Value11 varchar(100) DEFAULT '',
		Result_Value12 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable04 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT ''
	)


	DECLARE @ResultTable05 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT '',	
		Result_Value6 varchar(100) DEFAULT '',	
		Result_Value7 varchar(100) DEFAULT '',	
		Result_Value8 varchar(100) DEFAULT '',	
		Result_Value9 varchar(100) DEFAULT '',	
		Result_Value10 varchar(100) DEFAULT '',
		Result_Value11 varchar(100) DEFAULT '',
		Result_Value12 varchar(100) DEFAULT '',
		Result_Value13 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable06 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable07 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT '',
		Result_Value6 varchar(100) DEFAULT '',
		Result_Value7 varchar(100) DEFAULT '',
		Result_Value8 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable08 AS TABLE (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 nvarchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',
		Result_Value5 varchar(100) DEFAULT '',	
		Result_Value6 varchar(100) DEFAULT '',	
		Result_Value7 varchar(100) DEFAULT '',	
		Result_Value8 varchar(100) DEFAULT '',	
		Result_Value9 varchar(100) DEFAULT '',	
		Result_Value10 varchar(100) DEFAULT '',
		Result_Value11 varchar(100) DEFAULT '',
		Result_Value12 varchar(100) DEFAULT '',
		Result_Value13 varchar(100) DEFAULT '',
		Result_Value14 varchar(100) DEFAULT '',
		Result_Value15 varchar(100) DEFAULT '',
		Result_Value16 varchar(100) DEFAULT '',
		Result_Value17 varchar(100) DEFAULT '',
		Result_Value18 varchar(100) DEFAULT '',
		Result_Value19 varchar(100) DEFAULT '',
		Result_Value20 varchar(100) DEFAULT '',
		Result_Value21 varchar(100) DEFAULT '',
		Result_Value22 varchar(100) DEFAULT '',
		Result_Value23 varchar(100) DEFAULT '',
		Result_Value24 varchar(100) DEFAULT '',
		Result_Value25 varchar(100) DEFAULT '',
		Result_Value26 varchar(100) DEFAULT '',
		Result_Value27 varchar(100) DEFAULT '',
		Result_Value28 varchar(100) DEFAULT '',
		Result_Value29 varchar(100) DEFAULT '',
		Result_Value30 varchar(100) DEFAULT '',
		Result_Value31 varchar(100) DEFAULT '',
		Result_Value32 varchar(100) DEFAULT '',
		Result_Value33 varchar(100) DEFAULT '',
		Result_Value34 varchar(100) DEFAULT '',
		Result_Value35 varchar(100) DEFAULT '',
		Result_Value36 varchar(100) DEFAULT '',
		Result_Value37 varchar(100) DEFAULT '',
		Result_Value38 varchar(100) DEFAULT '',
		Result_Value39 varchar(100) DEFAULT '',
		Result_Value40 varchar(100) DEFAULT '',
		Result_Value41 varchar(100) DEFAULT '',
		Result_Value42 varchar(100) DEFAULT '',
		Result_Value43 varchar(100) DEFAULT '',
		Result_Value44 varchar(100) DEFAULT '',
		Result_Value45 varchar(100) DEFAULT '',
		Result_Value46 varchar(100) DEFAULT '',
		Result_Value47 varchar(100) DEFAULT '',
		Result_Value48 varchar(100) DEFAULT '',
		Result_Value49 varchar(100) DEFAULT '',
		Result_Value50 varchar(100) DEFAULT ''
	)

	--
	DECLARE @Summary05 AS TABLE (
		AgeGroup		INT,
		Secondary		CHAR(1),
		Service_Type	CHAR(5),
		Reason_Code		INT,
		Tx_Count		INT
	)

	DECLARE @TempResultTable05 AS TABLE (
		AgeGroup		INT,
		Secondary		CHAR(1),
		Reason_Code		INT,
		Reason_Desc		VARCHAR(50),
		RMP				INT,
		RCM				INT,
		RDT				INT,
		ROT				INT,
		RPT				INT,
		RMT				INT,
		RRD				INT,
		ENU				INT,
		RNU				INT,
		RCP				INT,
		ROP				INT,
		Total			INT
	)

	DECLARE @Summary06 AS TABLE (
		AgeGroup		INT,
		Secondary		CHAR(1),
		Service_Type	CHAR(5),
		Reason_Code		INT,
		Tx_Count		INT
	)

	DECLARE @TempResultTable06 AS TABLE (
		AgeGroup		INT,
		Secondary		CHAR(1),
		Reason_Code		INT,
		Reason_Desc		VARCHAR(50),
		DIT				INT,
		POD				INT,
		SPT				INT,		
		Total			INT
	)

	DECLARE @Summary07 AS TABLE (
		SP_ID			CHAR(8),
		Display_Seq		SMALLINT,
		Practice_Name	NVARCHAR(200),
		Service_Type	CHAR(5),
		Amt_65to69		BIGINT,
		Tx_Cnt_65to69	INT,
		Amt_70Above		BIGINT,
		Tx_Cnt_70Above	INT,
		Amt_65Above		BIGINT,
		Tx_Cnt_65Above	INT
	)

	
	DECLARE @Profession AS TABLE (
		Service_Type			char(5)
	)


	DECLARE @Reason AS TABLE
	(
		Reason_Code		INT,
		Reason_Desc		VARCHAR(50),
		Secondary		CHAR(1)
	)

	DECLARE @Reason_HCVSDHC AS TABLE
	(
		Reason_Code		INT,
		Reason_Desc		VARCHAR(50),
		Secondary		CHAR(1)
	)

	DECLARE @scheme_code_HK			CHAR(10)
	DECLARE @scheme_code_mainland	CHAR(10)
	DECLARE @scheme_code_DHC		CHAR(10)

-- =============================================
-- Initialization
-- =============================================

	-- Profession
	INSERT INTO @Profession
	SELECT Service_Category_Code FROM profession WITH (NOLOCK)

	-- Reason for Visit
	INSERT INTO @Reason(Reason_Code, Reason_Desc, Secondary)
	SELECT DISTINCT Reason_L1_Code, Reason_L1, 'N' FROM ReasonForVisitL1 WITH (NOLOCK)
	UNION
	SELECT 5, 'Defer Input', 'N'
	UNION
	SELECT DISTINCT Reason_L1_Code, Reason_L1, 'Y' FROM ReasonForVisitL1 WITH (NOLOCK)


	INSERT INTO @Reason_HCVSDHC(Reason_Code, Reason_Desc, Secondary)
	SELECT DISTINCT Reason_L1_Code, Reason_L1, 'N' FROM ReasonForVisitL1 WITH (NOLOCK)
	UNION
	SELECT DISTINCT Reason_L1_Code, Reason_L1, 'Y' FROM ReasonForVisitL1 WITH (NOLOCK)	
	

	SET @scheme_code_HK			= 'HCVS'
	SET @scheme_code_mainland	= 'HCVSCHN'
	SET @scheme_code_DHC		= 'HCVSDHC'	

-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Retrieve Voucher transactions
-- ---------------------------------------------
	INSERT INTO #VoucherTransaction (
		TxCnt,
		Scheme_Code,
		identity_num,
		Age,
		Deceased,
		Service_Type,
		Claim_Amount,
		Claim_Amount_RMB,
		Reason_for_visit_L1,
		ReasonforVisit_S1_L1,
		ReasonforVisit_S2_L1,
		ReasonforVisit_S3_L1,
		Record_Status,
		Invalidation,
		SP_ID,
		Practice_Display_Seq,
		SFC_Logical_DOD,
		DOB
	)
		SELECT
		COUNT(Transaction_ID), 
		Scheme_Code, 
		identity_num, 
		Age, 
		Deceased, 
		Service_Type, 
		SUM(Claim_Amount),  
		SUM(Claim_Amount_RMB),
		Reason_for_Visit_L1,
		ReasonforVisit_S1_L1, 
		ReasonforVisit_S2_L1, 
		ReasonforVisit_S3_L1, 
		Record_Status, 
		Invalidation,
		SP_ID, 
		Practice_Display_Seq,
		SFC_Logical_DOD,
		DOB
	FROM (
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Scheme_Code,
			ISNULL(VT.Voucher_Acc_ID, '') AS [Voucher_Acc_ID] ,
			ISNULL(VT.Temp_Voucher_Acc_ID, '') AS [Temp_Voucher_Acc_ID],
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN TP.Encrypt_Field1 ELSE VP.Encrypt_Field1 END AS [Identity_Num],
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Age ELSE ACVP.Age END AS [Age],
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Deceased ELSE ACVP.Deceased END AS [Deceased],
			VT.Service_Type,
			VT.Claim_Amount,
			ISNULL(TD.Total_Amount_RMB, 0) AS [Claim_Amount_RMB],
			CONVERT(smallint, ISNULL(TAF1.AdditionalFieldValueCode, 5)) AS Reason_for_Visit_L1, 
			CONVERT(smallint, ISNULL(TAF2.AdditionalFieldValueCode, 0)) AS ReasonforVisit_S1_L1, 
			CONVERT(smallint, ISNULL(TAF3.AdditionalFieldValueCode, 0)) AS ReasonforVisit_S2_L1, 
			CONVERT(smallint, ISNULL(TAF4.AdditionalFieldValueCode, 0)) AS ReasonforVisit_S3_L1,
			VT.Record_Status,
			VT.Invalidation,
			VT.SP_ID,
			VT.Practice_Display_Seq,
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Logical_DOD ELSE ACVP.Logical_DOD END AS [SFC_Logical_DOD],
			CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.DOB ELSE ACVP.DOB END AS [DOB]
		FROM
			 VoucherTransaction  VT WITH (NOLOCK)
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			LEFT JOIN TransactionAdditionalField TAF1 WITH (NOLOCK)
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			LEFT JOIN TransactionAdditionalField TAF2 WITH (NOLOCK)
				ON VT.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S1_L1'
			LEFT JOIN TransactionAdditionalField TAF3 WITH (NOLOCK)
				ON VT.Transaction_ID = TAF3.Transaction_ID
					AND TAF3.AdditionalFieldID = 'ReasonforVisit_S2_L1'
			LEFT JOIN TransactionAdditionalField TAF4 WITH (NOLOCK)
				ON VT.Transaction_ID = TAF4.Transaction_ID
					AND TAF4.AdditionalFieldID = 'ReasonforVisit_S3_L1'
			LEFT JOIN PersonalInformation VP WITH (NOLOCK)
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
			LEFT JOIN RptAccountSFC ACVP WITH (NOLOCK) 
				ON VP.Encrypt_Field1 = ACVP.identity_num AND ACVP.Is_Terminate = 'N'
			LEFT JOIN TempPersonalInformation TP WITH (NOLOCK)
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
			LEFT JOIN RptAccountSFC ACTP WITH (NOLOCK)
				ON TP.Encrypt_Field1 = ACTP.identity_num AND ACTP.Is_Terminate = 'N'
		WHERE
				VT.Scheme_Code IN (@scheme_code_HK, @scheme_code_mainland, @scheme_code_DHC)
				AND VT.Transaction_Dtm < @CutOffDtm
	) a
	GROUP BY 
		Scheme_Code, 
		identity_num, 
		Age, 
		Deceased, 
		Service_Type,  
		Reason_for_Visit_L1,
		ReasonforVisit_S1_L1, 
		ReasonforVisit_S2_L1, 
		ReasonforVisit_S3_L1, 
		Record_Status, 
		Invalidation,
		SP_ID, 
		Practice_Display_Seq,
		SFC_Logical_DOD,
		DOB

--------------------------------------------------------------
	DELETE #VoucherTransaction WHERE Record_Status IN (
				SELECT
					Status_Value
				FROM
					StatStatusFilterMapping WITH (NOLOCK)
				WHERE
					(report_id = 'ALL' OR report_id = 'eHSD0001') 
						AND Table_Name = 'VoucherTransaction'
						AND Status_Name = 'Record_Status' 
						AND ((Effective_Date IS NULL OR Effective_Date <= @CutOffDtm) AND (Expiry_Date IS NULL OR @CutOffDtm < Expiry_Date))
				)


----------------------------------------------------------------
	
	DELETE #VoucherTransaction WHERE (Invalidation IS NOT NULL AND Invalidation IN (
				SELECT
					Status_Value
				FROM
					StatStatusFilterMapping WITH (NOLOCK)
				WHERE
					(report_id = 'ALL' OR report_id = 'eHSD0001') 
						AND Table_Name = 'VoucherTransaction'
						AND Status_Name = 'Invalidation'
						AND ((Effective_Date IS NULL OR Effective_Date <= @CutOffDtm) AND (Expiry_Date IS NULL OR @CutOffDtm < Expiry_Date))
				)
				)



---------------------------------------------------------------
-- Remove VT that cannot match with Account (Exlcude Terminated Account)
	DELETE #VoucherTransaction WHERE Age IS NULL

-- =============================================
-- Process data for statistics
-- =============================================
--------------------------------------------------------------
-- Start of Summary

------------------------------------------------------------
-- Result

	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @ReportDtm, 111))
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('Voucher claims for voucher recipients aged 65 to 69')
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2, Result_Value3) VALUES ('', 'Total Voucher Amount Claimed ($)', 'No. of Transaction')
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 

	SELECT 'Any Voucher Scheme', ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code IN (@scheme_code_HK, @scheme_code_mainland, @scheme_code_DHC) AND Age >=65 AND Age <70
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_HK, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK AND Age >=65 AND Age <70 
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_mainland, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_mainland AND Age >=65 AND Age <70
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_DHC, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Age >=65 AND Age <70
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')

	--

	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('Voucher claims for voucher recipients aged 70 or above')
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) VALUES ('', 'Total Voucher Amount Claimed ($)', 'No. of Transaction')
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 

	SELECT 'Any Voucher Scheme', ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code IN (@scheme_code_HK, @scheme_code_mainland, @scheme_code_DHC) AND Age >=70
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_HK, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK AND Age >=70 
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_mainland, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_mainland AND Age >=70
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_DHC, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Age >=70
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')

	--

	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('Voucher claims for all voucher recipients (i.e. voucher recipients aged 65 or above)')
	INSERT INTO @ResultTableSummary (Result_Value1) VALUES ('')
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) VALUES ('', 'Total Voucher Amount Claimed ($)', 'No. of Transaction')

	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT 'Any Voucher Scheme', ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code IN (@scheme_code_HK, @scheme_code_mainland, @scheme_code_DHC) AND Age >=65
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_HK, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK AND Age >=65 
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_mainland, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_mainland AND Age >=65
	INSERT INTO @ResultTableSummary (Result_Value1,Result_Value2,Result_Value3) 
	SELECT @scheme_code_DHC, ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0), ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Age >=65
		
-- End of Summary
------------------------------------------------------------
--------------------------------------------------------------
-- Start of 01

-- +-----------------------------------------------------------------------------------+
-- |                      eHSM0006-01 - Number of VR by age groups                     |
-- +-----------------------------------------------------------------------------------+

-- =============================================
-- Part 1: Used Voucher >= 0
-- =============================================


	DECLARE @NoRegElder_65to69 INT, @NoRegElder_70 INT
	DECLARE @NoRegElderDeath_65to69 INT, @NoRegElderDeath_70 INT

	SELECT	@NoRegElder_65to69 = COUNT(1) FROM	RptAccountSFC WITH (NOLOCK) WHERE Is_Terminate = 'N' AND Age BETWEEN 65 AND 69
	SELECT	@NoRegElder_70 = COUNT(1) FROM	RptAccountSFC WITH (NOLOCK) WHERE Is_Terminate = 'N' AND Age >= 70

	-- Match Death Record
	SELECT	@NoRegElderDeath_65to69 = COUNT(1) FROM	RptAccountSFC WITH (NOLOCK) WHERE Is_Terminate = 'N' AND Deceased = 1 AND Age BETWEEN 65 AND 69 
	SELECT	@NoRegElderDeath_70 = COUNT(1) FROM	RptAccountSFC WITH (NOLOCK) WHERE Is_Terminate = 'N' AND Deceased = 1 AND Age >= 70

------------------------------------------------------------
-- Result

	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @ReportDtm, 111))
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2) VALUES ('1) Used Voucher >=0', '')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2) VALUES ('', 'Any Voucher Scheme')
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4) VALUES ('', 'VR aged 65 - 69', 'VR aged >=70', 'Total (i.e. VR aged >=65)')

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4) 
	SELECT 'Cumulative no. of registered elders', @NoRegElder_65to69, @NoRegElder_70, @NoRegElder_65to69 + @NoRegElder_70

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4) 
	SELECT 'Cumulative no. of elders who match death record (excluded those deceased before the eligible age to use voucher)', @NoRegElderDeath_65to69, @NoRegElderDeath_70, @NoRegElderDeath_65to69 + @NoRegElderDeath_70


-- =============================================
-- Part 2: Used Voucher > 0
-- =============================================

-- Get Data

	DECLARE @NoRegElderWClaim_ALL_65to69 int
	DECLARE @NoRegElderDeathWClaim_ALL_65to69 int
	DECLARE @NoRegElderWClaim_ALL_70 int
	DECLARE @NoRegElderDeathWClaim_ALL_70 int

	DECLARE @NoRegElderWClaim_HCVS_65to69 int
	DECLARE @NoRegElderDeathWClaim_HCVS_65to69 int
	DECLARE @NoRegElderWClaim_HCVS_70 int
	DECLARE @NoRegElderDeathWClaim_HCVS_70 int

	DECLARE @NoRegElderWClaim_HCVSDHC_65to69 int
	DECLARE @NoRegElderDeathWClaim_HCVSDHC_65to69 int
	DECLARE @NoRegElderWClaim_HCVSDHC_70 int
	DECLARE @NoRegElderDeathWClaim_HCVSDHC_70 int

	DECLARE @NoRegElderWClaim_HCVSCHN_65to69 int
	DECLARE @NoRegElderDeathWClaim_HCVSCHN_65to69 int
	DECLARE @NoRegElderWClaim_HCVSCHN_70 int
	DECLARE @NoRegElderDeathWClaim_HCVSCHN_70 int

	-- Any Voucher Scheme
	SELECT  
		@NoRegElderWClaim_ALL_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Age BETWEEN 65 AND 69

	SELECT  @NoRegElderDeathWClaim_ALL_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Age BETWEEN 65 AND 69 AND Deceased = 1

	SELECT  @NoRegElderWClaim_ALL_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Age >= 70

	SELECT  @NoRegElderDeathWClaim_ALL_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Age >= 70 AND Deceased = 1


	-- HCVS

	SELECT  
		@NoRegElderWClaim_HCVS_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_HK) AND Age BETWEEN 65 AND 69

	SELECT  @NoRegElderDeathWClaim_HCVS_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_HK) AND Age BETWEEN 65 AND 69 AND Deceased = 1

	SELECT  @NoRegElderWClaim_HCVS_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_HK) AND Age >= 70

	SELECT  @NoRegElderDeathWClaim_HCVS_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_HK) AND Age >= 70 AND Deceased = 1


	-- HCVSDHC

	SELECT  
		@NoRegElderWClaim_HCVSDHC_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_DHC) AND Age BETWEEN 65 AND 69

	SELECT  @NoRegElderDeathWClaim_HCVSDHC_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_DHC) AND Age BETWEEN 65 AND 69 AND Deceased = 1

	SELECT  @NoRegElderWClaim_HCVSDHC_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_DHC) AND Age >= 70

	SELECT  @NoRegElderDeathWClaim_HCVSDHC_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_DHC) AND Age >= 70 AND Deceased = 1
		
			
	--HCVSCHN

	SELECT  
		@NoRegElderWClaim_HCVSCHN_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_mainland) AND Age BETWEEN 65 AND 69

	SELECT  @NoRegElderDeathWClaim_HCVSCHN_65to69 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_mainland) AND Age BETWEEN 65 AND 69 AND Deceased = 1

	SELECT  @NoRegElderWClaim_HCVSCHN_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_mainland) AND Age >= 70

	SELECT  @NoRegElderDeathWClaim_HCVSCHN_70 = COUNT(DISTINCT Identity_num)
	FROM	
		#VoucherTransaction 
	WHERE
		Scheme_Code in (@scheme_code_mainland) AND Age >= 70 AND Deceased = 1

------------------------------------------------------------
-- Result

	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('2) Used Voucher >0')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,Result_Value11,Result_Value12,Result_Value13) VALUES 
	('', 'Any Voucher Scheme','','',@scheme_code_HK,'','',@scheme_code_DHC,'','',@scheme_code_mainland,'','')

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,Result_Value11,Result_Value12,Result_Value13) VALUES 
	('', 'VR aged 65 - 69', 'VR aged >=70', 'Total (i.e. VR aged >=65)',
	'VR aged 65 - 69', 'VR aged >=70', 'Total (i.e. VR aged >=65)',
	'VR aged 65 - 69', 'VR aged >=70', 'Total (i.e. VR aged >=65)',
	'VR aged 65 - 69', 'VR aged >=70', 'Total (i.e. VR aged >=65)')

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,Result_Value11,Result_Value12,Result_Value13)
	SELECT 'Cumulative no. of registered elders', 
	@NoRegElderWClaim_ALL_65to69, @NoRegElderWClaim_ALL_70, @NoRegElderWClaim_ALL_65to69 + @NoRegElderWClaim_ALL_70,
	@NoRegElderWClaim_HCVS_65to69, @NoRegElderWClaim_HCVS_70, @NoRegElderWClaim_HCVS_65to69 + @NoRegElderWClaim_HCVS_70,
	@NoRegElderWClaim_HCVSDHC_65to69, @NoRegElderWClaim_HCVSDHC_70, @NoRegElderWClaim_HCVSDHC_65to69 + @NoRegElderWClaim_HCVSDHC_70,
	@NoRegElderWClaim_HCVSCHN_65to69, @NoRegElderWClaim_HCVSCHN_70, @NoRegElderWClaim_HCVSCHN_65to69 + @NoRegElderWClaim_HCVSCHN_70

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,Result_Value11,Result_Value12,Result_Value13)
	SELECT 'Cumulative no. of elders who match death record (excluded those deceased before the eligible age to use voucher)', 
	@NoRegElderDeathWClaim_ALL_65to69, @NoRegElderDeathWClaim_ALL_70, @NoRegElderDeathWClaim_ALL_65to69 + @NoRegElderDeathWClaim_ALL_70,
	@NoRegElderDeathWClaim_HCVS_65to69, @NoRegElderDeathWClaim_HCVS_70, @NoRegElderDeathWClaim_HCVS_65to69 + @NoRegElderDeathWClaim_HCVS_70,
	@NoRegElderDeathWClaim_HCVSDHC_65to69, @NoRegElderDeathWClaim_HCVSDHC_70, @NoRegElderDeathWClaim_HCVSDHC_65to69 + @NoRegElderDeathWClaim_HCVSDHC_70,
	@NoRegElderDeathWClaim_HCVSCHN_65to69, @NoRegElderDeathWClaim_HCVSCHN_70, @NoRegElderDeathWClaim_HCVSCHN_65to69 + @NoRegElderDeathWClaim_HCVSCHN_70


-- =============================================
-- Part 3: Validated account created by HKU-SZH (i.e. account created under SPID: 00620498)
-- ***  Include Terminate Account ***
-- =============================================

-- Get Data

	DECLARE @NoValidAcc_65to69		INT
	DECLARE @NoValidAcc_70			INT
	DECLARE @NoValidAccDeath_65to69	INT
	DECLARE @NoValidAccDeath_70		INT

	INSERT INTO #VoucherAccount (Identity_Num, Age, Deceased)
	SELECT 
		AC.Identity_Num,
		AC.Age,
		AC.Deceased
	FROM 
		VoucherAccount VA WITH (NOLOCK)
		INNER JOIN PersonalInformation PI WITH (NOLOCK) ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID
		INNER JOIN RptAccountSFC AC WITH (NOLOCK) ON PI.Encrypt_Field1 = AC.Identity_Num 
	WHERE
		VA.Create_By = @HKUSZH_SP_ID and VA.Effective_Dtm < @CutOffDtm


	SELECT @NoValidAcc_65to69 = COUNT(DISTINCT Identity_Num) FROM #VoucherAccount WHERE Age BETWEEN 65 AND 69
	SELECT @NoValidAccDeath_65to69 = COUNT(DISTINCT Identity_Num) FROM #VoucherAccount WHERE Age BETWEEN 65 AND 69 AND Deceased = 1

	SELECT @NoValidAcc_70 = COUNT(DISTINCT Identity_Num) FROM #VoucherAccount WHERE Age >= 70
	SELECT @NoValidAccDeath_70 = COUNT(DISTINCT Identity_Num) FROM #VoucherAccount WHERE Age >= 70 AND Deceased = 1


------------------------------------------------------------
-- Result

	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('3) Validated account created by HKU-SZH (i.e. account created under SPID: 00620498)')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2) VALUES ('', @scheme_code_mainland)
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4) VALUES ('', 'VR aged 65 - 69', 'VR aged >=70', 'Total (i.e. VR aged >=65)')

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4)
	SELECT 'Cumulative no. of validated account*', @NoValidAcc_65to69, @NoValidAcc_70, @NoValidAcc_65to69 + @NoValidAcc_70

	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2,Result_Value3,Result_Value4)
	SELECT 'Cumulative no. of validated account which match death record*', @NoValidAccDeath_65to69, @NoValidAccDeath_70, @NoValidAccDeath_65to69 + @NoValidAccDeath_70

	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('Remarks:')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('* Include terminated account and the date for temp. account converted to validated account is used for counting the as at date')

-- =============================================
-- Part 4: Validated account created by back office
-- =============================================

-- Get Data
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('4) Validated account created by back office for HCVSCHN')
	INSERT INTO @ResultTable01 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2) VALUES ('', 'Matched with death record')


	--INSERT INTO @VA_CreatedByBO (voucher_acc_id, Identity_Num, Deceased)
	--SELECT Voucher_Acc_ID, Encrypt_Field1, 'N'
	--FROM PersonalInformation
	--WHERE Voucher_Acc_ID IN (SELECT ITEM FROM func_Split_string(@BO_Voucher_Acc_ID,','))

	--UPDATE VB
	--SET Deceased = 'Y'
	--FROM  
	--	@VA_CreatedByBO VB
	--	INNER JOIN DeathRecordEntry de ON VB.Identity_Num = DE.Encrypt_Field1 AND DE.Record_Status = 'A'


	INSERT INTO @ResultTable01 (Result_Value1,Result_Value2)
	SELECT
		dbo.func_format_voucher_account_number('V', Voucher_Acc_ID) as 'back office voucher acc id', 
		CASE WHEN Deceased IS NULL THEN 'N' ELSE 
			CASE WHEN Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM
	--@VA_CreatedByBO
		PersonalInformation WITH (NOLOCK)
	WHERE Voucher_Acc_ID IN (SELECT ITEM FROM func_Split_string(@BO_Voucher_Acc_ID,','))
	ORDER BY 
		Voucher_Acc_ID

-- End of 01
------------------------------------------------------------
--------------------------------------------------------------
-- Start of 02

-- +-----------------------------------------------------------------------------------+
-- |           eHSM0006-02 - Number of VR who reachd age 70 since 2018                 |
-- +-----------------------------------------------------------------------------------+

	-- =============================================
	-- Year		Age
	-- =============================================
	-- 2018:	70		(Year of DOB 1948)
	-- 2019:	70-71	(Year of DOB 1948 to 1949)
	-- 2020:	70-72	(Year of DOB 1948 to 1950)
	-- 2021:	70-73	(Year of DOB 1948 to 1951)
	-- =============================================

	DECLARE @Age70_from_age			AS INT
	DECLARE @Age70_to_age			AS INT
	DECLARE @Age70_to_year			AS INT
	DECLARE @Age70_from_DOB			AS INT
	DECLARE @Age70_to_DOB			AS INT
	
	SET @Age70_to_year =  YEAR(@ReportDtm)
	SET @Age70_from_age = 70
	SET @Age70_to_age = @Age70_from_age + (YEAR(@ReportDtm) - 2018)
	SET @Age70_from_DOB = 1948
	SET @Age70_to_DOB = @Age70_from_DOB + (YEAR(@ReportDtm) - 2018)
	
-- =============================================
-- Part 1: Used Voucher >= 0
-- =============================================
	DECLARE @NoRegElder_70AndAbove_Since2018 INT
	DECLARE @NoRegElderDeath_70AndAbove_Since2018 INT

	--Deceased
	SELECT	@NoRegElderDeath_70AndAbove_Since2018 = COUNT(1) 
	FROM	RptAccountSFC R WITH (NOLOCK)
	WHERE Is_Terminate = 'N' 
		AND R.Deceased = 1
		AND YEAR(R.Logical_DOD) - YEAR(R.DOB) >= @Age70_from_age
		AND YEAR(R.DOB) BETWEEN @Age70_from_DOB AND @Age70_to_DOB
	
	--Alive
	SELECT	@NoRegElder_70AndAbove_Since2018 = COUNT(1) 
	FROM	RptAccountSFC R WITH (NOLOCK)
	WHERE Is_Terminate = 'N' 
		AND Deceased = 0 
		AND YEAR(R.DOB) BETWEEN @Age70_from_DOB AND @Age70_to_DOB

------------------------------------------------------------
-- Result

	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @ReportDtm, 111))
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) VALUES ('1) Used Voucher >=0', '')
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) VALUES ('', 'Any Voucher Scheme')
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) VALUES ('', 'VR aged 70 - ' + CONVERT(varchar(4), @Age70_to_age))
	
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) 
	SELECT 'Age 70 - ' + CONVERT(varchar(3), @Age70_to_age) + ' in ' + convert(varchar(4), @Age70_to_year) + ' and alive', 
			@NoRegElder_70AndAbove_Since2018

	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) 
	SELECT 'Age 70 - ' + CONVERT(varchar(3), @Age70_to_age) + ' in ' + convert(varchar(4), @Age70_to_year) + ' and deceased', 
			@NoRegElderDeath_70AndAbove_Since2018

	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) 
	SELECT 'Total', 
			@NoRegElder_70AndAbove_Since2018 + @NoRegElderDeath_70AndAbove_Since2018

-- =============================================
-- Part 2: Used Voucher > 0
-- =============================================

	DECLARE @NoRegElderWClaim_ALL_70AndAbove_Since2018 INT
	DECLARE @NoRegElderDeathWClaim_ALL_70AndAbove_Since2018 INT

	--Deceased
	SELECT  @NoRegElderDeathWClaim_ALL_70AndAbove_Since2018 = COUNT(DISTINCT Identity_num)
	FROM	#VoucherTransaction R
	WHERE	Deceased = 1 
			AND YEAR(R.SFC_Logical_DOD) - YEAR(R.DOB) >= @Age70_from_age
			AND YEAR(R.DOB) BETWEEN @Age70_from_DOB AND @Age70_to_DOB

	--Alive
	SELECT  @NoRegElderWClaim_ALL_70AndAbove_Since2018 = COUNT(DISTINCT Identity_num)
	FROM	#VoucherTransaction R
	WHERE	Deceased = 0 
			AND YEAR(R.DOB) BETWEEN @Age70_from_DOB AND @Age70_to_DOB

------------------------------------------------------------
-- Result

	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('2) Used Voucher >0')
	INSERT INTO @ResultTable02 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) VALUES ('', 'Any Voucher Scheme')
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) VALUES ('', 'VR aged 70 - ' + CONVERT(varchar(4), @Age70_to_age))
	
	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) 
	SELECT 'Age 70 - ' + CONVERT(varchar(3), @Age70_to_age) + ' in ' + convert(varchar(4), @Age70_to_year) + ' and alive', 
			@NoRegElderWClaim_ALL_70AndAbove_Since2018

	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) 
	SELECT 'Age 70 - ' + CONVERT(varchar(3), @Age70_to_age) + ' in ' + convert(varchar(4), @Age70_to_year) + ' and deceased',  
			@NoRegElderDeathWClaim_ALL_70AndAbove_Since2018

	INSERT INTO @ResultTable02 (Result_Value1,Result_Value2) 
	SELECT 'Total', 
			@NoRegElderWClaim_ALL_70AndAbove_Since2018 + @NoRegElderDeathWClaim_ALL_70AndAbove_Since2018
	
-- End of 02
------------------------------------------------------------
------------------------------------------------------------

-- Start of 03
-- +-----------------------------------------------------------------------------------+
-- |       eHSM0006-03 - Report on Voucher Amount Claimed by Profession (HCVS)         |
-- +-----------------------------------------------------------------------------------+

-- RMP | RCM | RDT | ROT | RPT | RMT | RRD | ENU | RNU | RCP | ROP | Total --

	DECLARE @ENU_Amt BIGINT
	DECLARE	@RCM_Amt BIGINT
	DECLARE	@RCP_Amt BIGINT
	DECLARE	@RDT_Amt BIGINT
	DECLARE	@RMP_Amt BIGINT
	DECLARE	@RMT_Amt BIGINT
	DECLARE	@RNU_Amt BIGINT
	DECLARE @ROP_Amt BIGINT
	DECLARE	@ROT_Amt BIGINT
	DECLARE	@RPT_Amt BIGINT
	DECLARE	@RRD_Amt BIGINT
	DECLARE @Total_Amt BIGINT

------------------------------------------------------------
-- Age 65 - 69
	SELECT 	@ENU_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction where Scheme_Code = @scheme_code_HK AND service_type = 'ENU' AND Age BETWEEN 65 AND 69
	SELECT	@RCM_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RCM' AND Age BETWEEN 65 AND 69
	SELECT	@RCP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RCP' AND Age BETWEEN 65 AND 69
	SELECT	@RDT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RDT' AND Age BETWEEN 65 AND 69
	SELECT	@RMP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RMP' AND Age BETWEEN 65 AND 69
	SELECT	@RMT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RMT' AND Age BETWEEN 65 AND 69
	SELECT	@RNU_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RNU' AND Age BETWEEN 65 AND 69
	SELECT	@ROP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ROP' AND Age BETWEEN 65 AND 69
	SELECT	@ROT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ROT' AND Age BETWEEN 65 AND 69
	SELECT	@RPT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RPT' AND Age BETWEEN 65 AND 69
	SELECT	@RRD_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RRD' AND Age BETWEEN 65 AND 69
	SELECT  @Total_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK AND Age BETWEEN 65 AND 69


	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('Voucher amount claimed for voucher recipients aged 65 to 69')
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	VALUES ('RMP','RCM','RDT','ROT','RPT','RMT','RRD','ENU','RNU','RCP','ROP','Total')

	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	SELECT @RMP_Amt, @RCM_Amt, @RDT_Amt, @ROT_Amt, @RPT_Amt, @RMT_Amt, @RRD_Amt, @ENU_Amt, @RNU_Amt, @RCP_Amt, @ROP_Amt, @Total_Amt
	

------------------------------------------------------------
-- Age >= 70

	SELECT 	@ENU_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ENU' AND Age >= 70
	SELECT	@RCM_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RCM' AND Age >= 70
	SELECT	@RCP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RCP' AND Age >= 70
	SELECT	@RDT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RDT' AND Age >= 70
	SELECT	@RMP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RMP' AND Age >= 70
	SELECT	@RMT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RMT' AND Age >= 70
	SELECT	@RNU_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RNU' AND Age >= 70
	SELECT	@ROP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ROP' AND Age >= 70
	SELECT	@ROT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ROT' AND Age >= 70
	SELECT	@RPT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RPT' AND Age >= 70
	SELECT	@RRD_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RRD' AND Age >= 70
	SELECT  @Total_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK AND Age >= 70


	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('Voucher amount claimed for voucher recipients aged 70 or above')
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	VALUES ('RMP','RCM','RDT','ROT','RPT','RMT','RRD','ENU','RNU','RCP','ROP','Total')

	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	SELECT @RMP_Amt, @RCM_Amt, @RDT_Amt, @ROT_Amt, @RPT_Amt, @RMT_Amt, @RRD_Amt, @ENU_Amt, @RNU_Amt, @RCP_Amt, @ROP_Amt, @Total_Amt


------------------------------------------------------------
-- Age >= 65

	SELECT 	@ENU_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ENU' AND Age >= 65
	SELECT	@RCM_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RCM' AND Age >= 65
	SELECT	@RCP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RCP' AND Age >= 65
	SELECT	@RDT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RDT' AND Age >= 65
	SELECT	@RMP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RMP' AND Age >= 65
	SELECT	@RMT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RMT' AND Age >= 65
	SELECT	@RNU_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RNU' AND Age >= 65
	SELECT	@ROP_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ROP' AND Age >= 65
	SELECT	@ROT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'ROT' AND Age >= 65
	SELECT	@RPT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RPT' AND Age >= 65
	SELECT	@RRD_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction Where Scheme_Code = @scheme_code_HK AND Service_type = 'RRD' AND Age >= 65
	SELECT  @Total_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) from #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK AND Age >= 65

	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('Voucher amount claimed for all voucher recipients (i.e. voucher recipients aged 65 or above)')
	INSERT INTO @ResultTable03 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	VALUES ('RMP','RCM','RDT','ROT','RPT','RMT','RRD','ENU','RNU','RCP','ROP','Total')

	INSERT INTO @ResultTable03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	SELECT @RMP_Amt, @RCM_Amt, @RDT_Amt, @ROT_Amt, @RPT_Amt, @RMT_Amt, @RRD_Amt, @ENU_Amt, @RNU_Amt, @RCP_Amt, @ROP_Amt, @Total_Amt

-- End of 03
------------------------------------------------------------
------------------------------------------------------------

-- Start of 04
-- +-----------------------------------------------------------------------------------+
-- |       eHSM0006-04 - Report on Voucher Amount Claimed by Profession (HCVSDHC)      |
-- +-----------------------------------------------------------------------------------+

-- DIT | POD | SPT | Total --

	DECLARE @DIT_Amt BIGINT
	DECLARE	@POD_Amt BIGINT
	DECLARE	@SPT_Amt BIGINT
	--DECLARE @Total_Amt BIGINT

------------------------------------------------------------
-- Age 65 - 69
	SELECT 	@DIT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND service_type = 'DIT' AND Age BETWEEN 65 AND 69
	SELECT	@POD_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Service_type = 'POD' AND Age BETWEEN 65 AND 69
	SELECT	@SPT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Service_type = 'SPT' AND Age BETWEEN 65 AND 69
	SELECT  @Total_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Age BETWEEN 65 AND 69


	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('Voucher amount claimed for voucher recipients aged 65 to 69')
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable04 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('DIT','POD','SPT','Total')

	INSERT INTO @ResultTable04 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	SELECT @DIT_Amt, @POD_Amt, @SPT_Amt, @Total_Amt
	

------------------------------------------------------------
-- Age >= 70

	SELECT 	@DIT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND service_type = 'DIT' AND Age >= 70
	SELECT	@POD_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Service_type = 'POD' AND Age >= 70
	SELECT	@SPT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Service_type = 'SPT' AND Age >= 70
	SELECT  @Total_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Age >= 70

	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('Voucher amount claimed for voucher recipients aged 70 or above')
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable04 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('DIT','POD','SPT','Total')

	INSERT INTO @ResultTable04 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	SELECT @DIT_Amt, @POD_Amt, @SPT_Amt, @Total_Amt


------------------------------------------------------------
-- Age >= 65

	SELECT 	@DIT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND service_type = 'DIT' AND Age >= 65
	SELECT	@POD_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Service_type = 'POD' AND Age >= 65
	SELECT	@SPT_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Service_type = 'SPT' AND Age >= 65
	SELECT  @Total_Amt = isnull(sum(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND Age >= 65

	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('')
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('Voucher amount claimed for all voucher recipients (i.e. voucher recipients aged 65 or above)')
	INSERT INTO @ResultTable04 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable04 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('DIT','POD','SPT','Total')

	INSERT INTO @ResultTable04 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	SELECT @DIT_Amt, @POD_Amt, @SPT_Amt, @Total_Amt	

-- End of 04
------------------------------------------------------------
------------------------------------------------------------
-- Prepare Reason For Visit data for 05 HCVS & 06 HCVSDHC

	-- HCVS
	INSERT INTO #VT_ReasonForVisit (Scheme_Code, TxCnt, age, service_type, Reason_Code, Secondary)
	-- Primary
	SELECT @scheme_code_HK, SUM(TxCnt) AS cnt, Age, service_type, Reason_for_visit_L1, 'N' AS sec 
	FROM #VoucherTransaction 
	WHERE Scheme_Code = @scheme_code_HK
	GROUP BY Age, service_type, Reason_for_visit_L1
	UNION ALL
	-- Secondary
	SELECT @scheme_code_HK, SUM(TxCnt), a.Age, a.service_type, a.ReasonSecond, 'Y' FROM (
		SELECT TxCnt, Age, Service_Type, ReasonforVisit_S1_L1 AS ReasonSecond FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK
		UNION ALL
		SELECT TxCnt, Age, Service_Type, ReasonforVisit_S2_L1 AS ReasonSecond FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK
		UNION ALL
		SELECT TxCnt, Age, Service_Type, ReasonforVisit_S3_L1 AS ReasonSecond FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_HK
	) a
	WHERE a.ReasonSecond <> 0
	GROUP BY a.Age, a.service_type, a.ReasonSecond

	-- HCVSDHC
	INSERT INTO #VT_ReasonForVisit (Scheme_Code, TxCnt, age, service_type, Reason_Code, Secondary)
	-- Primary
	SELECT @scheme_code_DHC, SUM(TxCnt) AS cnt, Age, service_type, Reason_for_visit_L1, 'N' AS sec 
	FROM #VoucherTransaction 
	WHERE Scheme_Code = @scheme_code_DHC
	GROUP BY Age, service_type, Reason_for_visit_L1
	UNION ALL
	-- Secondary
	SELECT @scheme_code_DHC, SUM(TxCnt), a.Age, a.service_type, a.ReasonSecond, 'Y' FROM (
		SELECT TxCnt, Age, Service_Type, ReasonforVisit_S1_L1 AS ReasonSecond FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC
		UNION ALL
		SELECT TxCnt, Age, Service_Type, ReasonforVisit_S2_L1 AS ReasonSecond FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC
		UNION ALL
		SELECT TxCnt, Age, Service_Type, ReasonforVisit_S3_L1 AS ReasonSecond FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC
	) a
	WHERE a.ReasonSecond <> 0
	GROUP BY a.Age, a.service_type, a.ReasonSecond


------------------------------------------------------------------------------------------
-- Start of 05
-- +-----------------------------------------------------------------------------------+
-- |   eHSM0006-05 -Number of Claim Transactions by Professions of Service Providers,  |
-- |                Reasons for Visit and Age Groups of Voucher Recipients (HCVS)      |
-- +-----------------------------------------------------------------------------------+

-- Reason for Visit (Level 1) | RMP | RCM | RDT | ROT | RPT | RMT | RRD | ENU | RNU | RCP | ROP | Total --
------------------------------------------------------------------------------------------
-- Get Data

	-- Age 65 - 69
	INSERT INTO @Summary05 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT 1, R.Secondary, P.Service_Type, R.Reason_Code, ISNULL(VT.Tx_Count,0)
	FROM @Profession P
	CROSS JOIN @Reason R
	LEFT JOIN
		(	SELECT Reason_Code, Secondary, Service_Type, SUM(TxCnt) AS [Tx_Count]
			FROM #VT_ReasonForVisit
			WHERE Scheme_Code = @scheme_code_HK 
				AND Age BETWEEN 65 AND 69
			GROUP BY Service_Type, Reason_Code, Secondary
		) VT ON VT.Reason_Code = R.reason_code AND VT.Secondary = R.Secondary
									AND VT.Service_Type = P.Service_Type

	-- Age >= 70
	INSERT INTO @Summary05 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT 2, R.Secondary, P.Service_Type, R.Reason_Code, ISNULL(VT.Tx_Count,0)
	FROM @Profession P
	CROSS JOIN @Reason R
	LEFT JOIN
		(	SELECT Reason_Code, Secondary, Service_Type, SUM(TxCnt) AS [Tx_Count]
			FROM #VT_ReasonForVisit
			WHERE Scheme_Code = @scheme_code_HK 
				AND Age >= 70
			GROUP BY Service_Type, Reason_Code, Secondary
		) VT ON VT.Reason_Code = R.reason_code AND VT.Secondary = R.Secondary
									AND VT.Service_Type = P.Service_Type


	-- Age >= 65
	INSERT INTO @Summary05 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT 3, R.Secondary, P.Service_Type, R.Reason_Code, ISNULL(VT.Tx_Count,0)
	FROM @Profession P
	CROSS JOIN @Reason R
	LEFT JOIN
		(	SELECT Reason_Code, Secondary, Service_Type, SUM(TxCnt) AS [Tx_Count]
			FROM #VT_ReasonForVisit
			WHERE Scheme_Code = @scheme_code_HK 
				AND Age >= 65
			GROUP BY Service_Type, Reason_Code, Secondary
		) VT ON VT.Reason_Code = R.reason_code AND VT.Secondary = R.Secondary
									AND VT.Service_Type = P.Service_Type


	-- Total
	INSERT INTO @Summary05 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT AgeGroup, Secondary, 'Total', Reason_Code, SUM(Tx_Count)
	FROM @Summary05
	GROUP BY AgeGroup, Secondary, Reason_Code

	INSERT INTO @Summary05 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT AgeGroup, Secondary, Service_Type, 99, SUM(Tx_Count)
	FROM @Summary05
	GROUP BY AgeGroup, Secondary, Service_Type

------------------------------------------------------------

	INSERT INTO @TempResultTable05 (AgeGroup, Secondary, Reason_Code, Reason_Desc, RMP, RCM, RDT, ROT, RPT, RMT, RRD, ENU, RNU, RCP, ROP, total)
	SELECT	PT.*
	FROM
	(
		SELECT	S.AgeGroup,
				S.Secondary,
				S.Reason_Code, 
				ISNULL(R.Reason_Desc, 'Total') AS [Reason_Desc],
				S.Service_Type,						
				ISNULL(S.Tx_Count, 0) AS [Count]
		FROM
			@Summary05 S
			LEFT JOIN @Reason R ON S.Reason_Code = R.Reason_Code AND S.Secondary = R.Secondary
	) SR
	pivot
	(
		MAX([Count])
		for Service_Type in (
					[RMP], [RCM], [RDT], [ROT], [RPT], [RMT], [RRD],
					[ENU], [RNU], [RCP], [ROP], [Total])
	) PT



------------------------------------------------------------
-- Result

-- Age 65 - 69

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('Number of claim transactions for voucher recipients aged 65 to 69')

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES ('Principal Reason for Visit (Level 1)', 'RMP', 'RCM', 'RDT', 'ROT', 'RPT', 'RMT', 'RRD', 'ENU', 'RNU', 'RCP', 'ROP', 'Total')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT 
		Reason_Desc, RMP, RCM, RDT, ROT, RPT, CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
		RRD, ENU, RNU, RCP, ROP, total 
	FROM @TempResultTable05
	WHERE
		AgeGroup = 1 AND Secondary = 'N'
	ORDER BY Reason_Code

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES ('Secondary Reason for Visit (Level 1)', 'RMP', 'RCM', 'RDT', 'ROT', 'RPT', 'RMT', 'RRD', 'ENU', 'RNU', 'RCP', 'ROP', 'Total')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT 
		Reason_Desc, RMP, RCM, RDT, ROT, RPT, CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
		RRD, ENU, RNU, RCP, ROP, total 
	FROM @TempResultTable05
	WHERE
		AgeGroup = 1 AND Secondary = 'Y'
	ORDER BY Reason_Code


------------------------------------------------------------
-- Age >= 70

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('Number of claim transactions for voucher recipients aged 70 or above')

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES ('Principal Reason for Visit (Level 1)', 'RMP', 'RCM', 'RDT', 'ROT', 'RPT', 'RMT', 'RRD', 'ENU', 'RNU', 'RCP', 'ROP', 'Total')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT 
		Reason_Desc, RMP, RCM, RDT, ROT, RPT, CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
		RRD, ENU, RNU, RCP, ROP, total 
	FROM @TempResultTable05
	WHERE
		AgeGroup = 2 AND Secondary = 'N'
	ORDER BY Reason_Code

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES ('Secondary Reason for Visit (Level 1)', 'RMP', 'RCM', 'RDT', 'ROT', 'RPT', 'RMT', 'RRD', 'ENU', 'RNU', 'RCP', 'ROP', 'Total')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT 
		Reason_Desc, RMP, RCM, RDT, ROT, RPT, CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
		RRD, ENU, RNU, RCP, ROP, total 
	FROM @TempResultTable05
	WHERE
		AgeGroup = 2 AND Secondary = 'Y'
	ORDER BY Reason_Code

------------------------------------------------------------
-- Age >= 65

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('Number of claim transactions for all voucher recipients (i.e. voucher recipients aged 65 or above)')

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES ('Principal Reason for Visit (Level 1)', 'RMP', 'RCM', 'RDT', 'ROT', 'RPT', 'RMT', 'RRD', 'ENU', 'RNU', 'RCP', 'ROP', 'Total')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT 
		Reason_Desc, RMP, RCM, RDT, ROT, RPT, CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
		RRD, ENU, RNU, RCP, ROP, total 
	FROM @TempResultTable05
	WHERE
		AgeGroup = 3 AND Secondary = 'N'
	ORDER BY Reason_Code

	INSERT INTO @ResultTable05 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	VALUES ('Secondary Reason for Visit (Level 1)', 'RMP', 'RCM', 'RDT', 'ROT', 'RPT', 'RMT', 'RRD', 'ENU', 'RNU', 'RCP', 'ROP', 'Total')

	INSERT INTO @ResultTable05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT 
		Reason_Desc, RMP, RCM, RDT, ROT, RPT, CASE WHEN Reason_Code = 4 THEN 'N/A' ELSE CAST(RMT AS VARCHAR(100)) END, 
		RRD, ENU, RNU, RCP, ROP, total 
	FROM @TempResultTable05
	WHERE
		AgeGroup = 3 AND Secondary = 'Y'
	ORDER BY Reason_Code


-- End of 05
------------------------------------------------------------
------------------------------------------------------------------------------------------
-- Start of 06
-- +-----------------------------------------------------------------------------------+
-- |   eHSM0006-06 -Number of Claim Transactions by Professions of Service Providers,  |
-- |                Reasons for Visit and Age Groups of Voucher Recipients (HCVSDHC)   |
-- +-----------------------------------------------------------------------------------+

-- Reason for Visit (Level 1) | DIT | POD | SPT | Total --
------------------------------------------------------------------------------------------
-- Get Data

	-- Age 65 - 69
	INSERT INTO @Summary06 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT 1, R.Secondary, P.Service_Type, R.Reason_Code, ISNULL(VT.Tx_Count,0)
	FROM @Profession P
	CROSS JOIN @Reason_HCVSDHC R
	LEFT JOIN
		(	SELECT Reason_Code, Secondary, Service_Type, SUM(TxCnt) AS [Tx_Count]
			FROM #VT_ReasonForVisit
			WHERE Scheme_Code = @scheme_code_DHC 
				AND Age BETWEEN 65 AND 69
			GROUP BY Service_Type, Reason_Code, Secondary
		) VT ON VT.Reason_Code = R.reason_code AND VT.Secondary = R.Secondary
									AND VT.Service_Type = P.Service_Type
	
	-- Age >= 70
	INSERT INTO @Summary06 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT 2, R.Secondary, P.Service_Type, R.Reason_Code, ISNULL(VT.Tx_Count,0)
	FROM @Profession P
	CROSS JOIN @Reason_HCVSDHC R
	LEFT JOIN
		(	SELECT Reason_Code, Secondary, Service_Type, SUM(TxCnt) AS [Tx_Count]
			FROM #VT_ReasonForVisit
			WHERE Scheme_Code = @scheme_code_DHC 
				AND Age >= 70
			GROUP BY Service_Type, Reason_Code, Secondary
		) VT ON VT.Reason_Code = R.reason_code AND VT.Secondary = R.Secondary
									AND VT.Service_Type = P.Service_Type

	-- Age >= 65
	INSERT INTO @Summary06 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT 3, R.Secondary, P.Service_Type, R.Reason_Code, ISNULL(VT.Tx_Count,0)
	FROM @Profession P
	CROSS JOIN @Reason_HCVSDHC R
	LEFT JOIN
		(	SELECT Reason_Code, Secondary, Service_Type, SUM(TxCnt) AS [Tx_Count]
			FROM #VT_ReasonForVisit
			WHERE Scheme_Code = @scheme_code_DHC 
				AND Age >= 65
			GROUP BY Service_Type, Reason_Code, Secondary
		) VT ON VT.Reason_Code = R.reason_code AND VT.Secondary = R.Secondary
									AND VT.Service_Type = P.Service_Type

	
	-- Total
	INSERT INTO @Summary06 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT AgeGroup, Secondary, 'Total', Reason_Code, SUM(Tx_Count)
	FROM @Summary06
	GROUP BY AgeGroup, Secondary, Reason_Code

	INSERT INTO @Summary06 (AgeGroup, Secondary, Service_Type, Reason_Code, Tx_Count)
	SELECT AgeGroup, Secondary, Service_Type, 99, SUM(Tx_Count)
	FROM @Summary06
	GROUP BY AgeGroup, Secondary, Service_Type

------------------------------------------------------------

	INSERT INTO @TempResultTable06 (AgeGroup, Secondary, Reason_Code, Reason_Desc, DIT, POD, SPT, total)
	SELECT	PT.*
	FROM
	(
		SELECT	S.AgeGroup,
				S.Secondary,
				S.Reason_Code, 
				ISNULL(R.Reason_Desc, 'Total') AS [Reason_Desc],
				S.Service_Type,						
				ISNULL(S.Tx_Count, 0) AS [Count]
		FROM
			@Summary06 S
			LEFT JOIN @Reason_HCVSDHC R ON S.Reason_Code = R.Reason_Code AND S.Secondary = R.Secondary									
	) SR
	pivot
	(
		MAX([Count])
		for Service_Type in (
					[DIT], [POD], [SPT], [Total])
	) PT



------------------------------------------------------------
-- Result

-- Age 65 - 69

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('Number of claim transactions for voucher recipients aged 65 to 69')

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES ('Principal Reason for Visit (Level 1)', 'DIT', 'POD', 'SPT', 'Total')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT 
		Reason_Desc, DIT, POD, SPT, total 
	FROM @TempResultTable06
	WHERE
		AgeGroup = 1 AND Secondary = 'N'
	ORDER BY Reason_Code

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES ('Secondary Reason for Visit (Level 1)', 'DIT', 'POD', 'SPT', 'Total')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT 
		Reason_Desc, DIT, POD, SPT, total 
	FROM @TempResultTable06
	WHERE
		AgeGroup = 1 AND Secondary = 'Y'
	ORDER BY Reason_Code


------------------------------------------------------------
-- Age >= 70

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('Number of claim transactions for voucher recipients aged 70 or above')

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES ('Principal Reason for Visit (Level 1)', 'DIT', 'POD', 'SPT', 'Total')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT 
		Reason_Desc, DIT, POD, SPT, total 
	FROM @TempResultTable06
	WHERE
		AgeGroup = 2 AND Secondary = 'N'
	ORDER BY Reason_Code

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES ('Secondary Reason for Visit (Level 1)', 'DIT', 'POD', 'SPT', 'Total')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT 
		Reason_Desc, DIT, POD, SPT, total 
	FROM @TempResultTable06
	WHERE
		AgeGroup = 2 AND Secondary = 'Y'
	ORDER BY Reason_Code

------------------------------------------------------------
-- Age >= 65

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('Number of claim transactions for all voucher recipients (i.e. voucher recipients aged 65 or above)')

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES ('Principal Reason for Visit (Level 1)', 'DIT', 'POD', 'SPT', 'Total')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT 
		Reason_Desc, DIT, POD, SPT, total 
	FROM @TempResultTable06
	WHERE
		AgeGroup = 3 AND Secondary = 'N'
	ORDER BY Reason_Code

	INSERT INTO @ResultTable06 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES ('Secondary Reason for Visit (Level 1)', 'DIT', 'POD', 'SPT', 'Total')

	INSERT INTO @ResultTable06 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT 
		Reason_Desc, DIT, POD, SPT, total 
	FROM @TempResultTable06
	WHERE
		AgeGroup = 3 AND Secondary = 'Y'
	ORDER BY Reason_Code


-- End of 06
------------------------------------------------------------
------------------------------------------------------------
-- Start of 07
-- +-----------------------------------------------------------------------------------+
-- |   eHSM0006-07 -Report on Voucher Amount Claimed by Age Groups of Voucher          |
-- |                Recipients and by Practice (HCVSDHC)                               |
-- +-----------------------------------------------------------------------------------+

-- Practice | Profession | Claim Amount ($) | No. of Voucher tx | Claim Amount ($) | No. of Voucher tx | Claim Amount ($) |No. of Voucher tx  --
------------------------------------------------------------------------------------------

	DECLARE @DHCPracticeList TABLE (
		SP_ID			CHAR(8),
        Display_Seq		SMALLINT,
        Practice_Name	NVARCHAR(500),
		Service_Type	CHAR(5)
    )


	INSERT INTO @DHCPracticeList (SP_ID, Display_Seq, Practice_Name, Service_Type)
	SELECT 
		P.SP_ID, P.Display_Seq, P.Practice_Name, PRO.Service_Category_Code
	FROM 
		ServiceProvider SP WITH (NOLOCK)
	INNER JOIN PracticeSchemeInfo PSI WITH (NOLOCK)
		ON SP.SP_ID = PSI.SP_ID AND PSI.Scheme_Code = @scheme_code_DHC
	INNER JOIN Practice P WITH (NOLOCK)
		ON SP.SP_ID = P.SP_ID AND PSI.Practice_Display_Seq = P.Display_Seq
	INNER JOIN Professional Pro WITH (NOLOCK)
		ON SP.SP_ID = Pro.SP_ID AND P.Professional_Seq = Pro.Professional_Seq
		
	-- 
	
	INSERT INTO @ResultTable07 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @reportDtm, 111))

	INSERT INTO @ResultTable07 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable07 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8) VALUES 
	('', '', 'Aged 65 to 69', '', 'Aged 70 or above', '', 'Aged 65 or above', '')

	INSERT INTO @ResultTable07 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8) VALUES 
	('Practice', 'Profession', 'Claim Amount ($)', 'No. of voucher transaction', 'Claim Amount ($)', 'No. of voucher transaction', 'Claim Amount ($)', 'No. of voucher transaction')

	--

	DECLARE @SP_ID			CHAR(8)
	DECLARE @Display_Seq	SMALLINT
	DECLARE @Practice_Name	NVARCHAR(200)
	DECLARE @Service_Type	CHAR(5)

	DECLARE @DHC_Amt_65to69		BIGINT
	DECLARE @DHC_Tx_Cnt_65to69	INT
	DECLARE @DHC_Amt_70Above	BIGINT
	DECLARE @DHC_Tx_Cnt_70Above	INT
	DECLARE @DHC_Amt_65Above	BIGINT
	DECLARE @DHC_Tx_Cnt_65Above	INT

	DECLARE myCursor CURSOR FOR
	SELECT SP_ID, Display_Seq, Practice_Name, Service_Type FROM @DHCPracticeList ORDER BY Practice_Name
	
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @SP_ID, @Display_Seq, @Practice_Name, @Service_Type
	WHILE @@FETCH_STATUS = 0 BEGIN

		SET @DHC_Amt_65to69 = 0
		SET @DHC_Tx_Cnt_65to69 = 0
		SET @DHC_Amt_70Above = 0
		SET @DHC_Tx_Cnt_70Above = 0
		SET @DHC_Amt_65Above = 0
		SET @DHC_Tx_Cnt_65Above = 0

		SELECT @DHC_Amt_65to69 = ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND SP_ID = @SP_ID AND Practice_Display_Seq = @Display_Seq AND Age BETWEEN 65 AND 69
		SELECT @DHC_Tx_Cnt_65to69 = ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND SP_ID = @SP_ID AND Practice_Display_Seq = @Display_Seq AND Age BETWEEN 65 AND 69

		SELECT @DHC_Amt_70Above = ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND SP_ID = @SP_ID AND Practice_Display_Seq = @Display_Seq AND Age >= 70
		SELECT @DHC_Tx_Cnt_70Above = ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND SP_ID = @SP_ID AND Practice_Display_Seq = @Display_Seq AND Age >= 70

		SELECT @DHC_Amt_65Above = ISNULL(SUM(CONVERT(BIGINT, Claim_Amount)),0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND SP_ID = @SP_ID AND Practice_Display_Seq = @Display_Seq AND Age >= 65
		SELECT @DHC_Tx_Cnt_65Above = ISNULL(SUM(TxCnt), 0) FROM #VoucherTransaction WHERE Scheme_Code = @scheme_code_DHC AND SP_ID = @SP_ID AND Practice_Display_Seq = @Display_Seq AND Age >= 65

		INSERT INTO @Summary07 (SP_ID, Display_Seq, Practice_Name, Service_Type, Amt_65to69, Tx_Cnt_65to69, Amt_70Above, Tx_Cnt_70Above, Amt_65Above, Tx_Cnt_65Above)
		VALUES (@SP_ID, @Display_Seq, @Practice_Name, @Service_Type,
				@DHC_Amt_65to69, @DHC_Tx_Cnt_65to69, 
				@DHC_Amt_70Above, @DHC_Tx_Cnt_70Above,
				@DHC_Amt_65Above, @DHC_Tx_Cnt_65Above
				)

		FETCH NEXT FROM myCursor INTO @SP_ID, @Display_Seq, @Practice_Name, @Service_Type
	END
	
	CLOSE myCursor
	DEALLOCATE myCursor

	INSERT INTO @ResultTable07 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	SELECT 
		Practice_Name, Service_Type, Amt_65to69, Tx_Cnt_65to69, Amt_70Above, Tx_Cnt_70Above, Amt_65Above, Tx_Cnt_65Above
	FROM
		@Summary07
	ORDER BY
		Practice_Name

	-- Total
	INSERT INTO @ResultTable07 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8)
	SELECT 
		'', 'Total', SUM(Amt_65to69), SUM(Tx_Cnt_65to69), SUM(Amt_70Above), SUM(Tx_Cnt_70Above), SUM(Amt_65Above), SUM(Tx_Cnt_65Above)
	FROM
		@Summary07
			
------------------------------------------------------------
-- Start of 08
-- +---------------------------------------------------------------------------+
-- |   eHSM0006-08 -Report on Voucher Amount Claimed by Age Groups of Voucher  |
-- |                Recipients and by Practice (HCVSCHN)		               |
-- +---------------------------------------------------------------------------+

	DECLARE @DataTable table (
		R			int,
		C			int,
		Txt			Nvarchar(500)
	)
	
	DECLARE @FrameTable table (
		C			int,
		ColName		varchar(5)
	)
		
	DECLARE @PracticeList TABLE (
            Display_Seq          SMALLINT,
            Practice_Name        NVARCHAR(500)
    )
	
	DECLARE @PracticeName1 NVARCHAR(500)
	DECLARE @PracticeName2 NVARCHAR(500)
	DECLARE @PracticeName3 NVARCHAR(500)
	DECLARE @Seq1 int
	DECLARE @Seq2 int
	DECLARE @Seq3 int

	DECLARE @r INT
	DECLARE @c INT
	DECLARE @i INT
	SET @r=1 
	SET @c=1 
	SET @i=1

	WHILE @i <= 50 BEGIN
		INSERT INTO @FrameTable (C, ColName) VALUES (@i, 'C' + CONVERT(varchar, @i))

		SET @i = @i + 1

	END

    INSERT INTO @PracticeList (Display_Seq, Practice_Name)
    SELECT 
		Display_Seq, 
		CAST(Display_Seq AS VARCHAR) + '.' + Practice_Name 
    FROM
		Practice P WITH (NOLOCK) 
    WHERE
		SP_ID = @HKUSZH_SP_ID
		AND EXISTS (SELECT SP_ID, Practice_Display_Seq 
					FROM PracticeSchemeInfo PSI WITH (NOLOCK)
					WHERE P.SP_ID = PSI.SP_ID AND P.Display_Seq = PSI.Practice_Display_Seq
						AND PSI.Scheme_Code = @scheme_code_mainland
					)
	
------------------------------------------------------------------------------------
--Age 65-69
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'Voucher claims for voucher recipients aged 65 to 69')
	SET @r = @r+1

	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	
		
	--Header
	---------------------------------------
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @c = @c+1

	DECLARE myCursor CURSOR LOCAL FAST_FORWARD FOR
		select Practice_Name
		from @PracticeList
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @PracticeName1
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO @DataTable(r,c,Txt)
		VALUES (@r, @c, @PracticeName1)

		SET @c = @c+1
		FETCH NEXT FROM myCursor INTO @PracticeName1
	END
	CLOSE myCursor
	DEALLOCATE myCursor
			
	INSERT INTO @DataTable(r,c,Txt)
	VALUES (@r, @c, 'Total')

	SET @c= 1
	SET @r= @r+1
	----------------------------
	

	--content
	-----------------------------
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'HKD$')
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r+1,@c,N'RMB¥')		
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r+2,@c,'No. of voucher transaction')		
	SET @c = @c+1

	DECLARE myCursor CURSOR LOCAL FAST_FORWARD FOR
		select Display_Seq
		from @PracticeList
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @Seq1
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r, @c, isnull(SUM(CONVERT(BIGINT, Claim_Amount)),0)
		FROM #VoucherTransaction
		WHERE Age >=65 AND Age <=69
		AND Scheme_Code = @scheme_code_mainland
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq1

		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r+1, @c, isnull(SUM(CONVERT(MONEY, Claim_Amount_RMB)),0)
		FROM #VoucherTransaction
		WHERE Age >=65 AND Age <=69
		AND Scheme_Code = @scheme_code_mainland
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq1

		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r+2, @c, ISNULL(SUM(TxCnt), 0)
		FROM #VoucherTransaction
		WHERE Age >=65 AND Age <=69
		AND (Scheme_Code = @scheme_code_mainland)
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq1

		SET @c = @c+1
		FETCH NEXT FROM myCursor INTO @Seq1
	END
	CLOSE myCursor
	DEALLOCATE myCursor
		
	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r, @c, isnull(SUM(CONVERT(BIGINT, Claim_Amount)),0)
	FROM #VoucherTransaction
	WHERE Age >=65 AND Age <=69
	AND SP_ID = @HKUSZH_SP_ID
	AND Scheme_Code = @scheme_code_mainland

	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r+1, @c, isnull(SUM(CONVERT(MONEY, Claim_Amount_RMB)),0)
	FROM #VoucherTransaction
	WHERE Age >=65 AND Age <=69
	AND SP_ID = @HKUSZH_SP_ID
	AND Scheme_Code = @scheme_code_mainland

	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r+2, @c, ISNULL(SUM(TxCnt), 0)
	FROM #VoucherTransaction
	WHERE Age >=65 AND Age <=69
	AND SP_ID = @HKUSZH_SP_ID
	AND (Scheme_Code = @scheme_code_mainland)

	SET @c= 1
	SET @r= @r+3
	--------------------------------------------------
	
		
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	
-----------------------------------------------------------------------------------
------------------------------------------------------------------------------------
--Age >=70
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'Voucher claims for voucher recipients aged 70 or above')
	SET @r = @r+1

	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	
		
	--Header
	---------------------------------------
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @c = @c+1

	DECLARE myCursor CURSOR LOCAL FAST_FORWARD FOR
		select Practice_Name
		from @PracticeList
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @PracticeName2
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO @DataTable(r,c,Txt)
		VALUES (@r, @c, @PracticeName2)

		SET @c = @c+1
		FETCH NEXT FROM myCursor INTO @PracticeName2
	END
	CLOSE myCursor
	DEALLOCATE myCursor
			
	INSERT INTO @DataTable(r,c,Txt)
	VALUES (@r, @c, 'Total')

	SET @c= 1
	SET @r= @r+1
	----------------------------
	

	--content
	-----------------------------
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'HKD$')
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r+1,@c,N'RMB¥')		
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r+2,@c,'No. of voucher transaction')		
	SET @c = @c+1

	DECLARE myCursor CURSOR LOCAL FAST_FORWARD FOR
		select Display_Seq
		from @PracticeList
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @Seq2
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r, @c, isnull(SUM(CONVERT(BIGINT, Claim_Amount)),0)
		FROM #VoucherTransaction
		WHERE Age >=70
		AND Scheme_Code = @scheme_code_mainland
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq2

		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r+1, @c, isnull(SUM(CONVERT(MONEY, Claim_Amount_RMB)),0)
		FROM #VoucherTransaction
		WHERE Age >=70
		AND Scheme_Code = @scheme_code_mainland
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq2

		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r+2, @c, ISNULL(SUM(TxCnt), 0)
		FROM #VoucherTransaction
		WHERE Age >=70
		AND (Scheme_Code = @scheme_code_mainland)
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq2

		SET @c = @c+1
		FETCH NEXT FROM myCursor INTO @Seq2
	END
	CLOSE myCursor
	DEALLOCATE myCursor
		
	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r, @c, isnull(SUM(CONVERT(BIGINT, Claim_Amount)),0)
	FROM #VoucherTransaction
	WHERE Age >=70
	AND Scheme_Code = @scheme_code_mainland
	AND SP_ID = @HKUSZH_SP_ID

	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r+1, @c, isnull(SUM(CONVERT(MONEY, Claim_Amount_RMB)),0)
	FROM #VoucherTransaction
	WHERE Age >=70
	AND Scheme_Code = @scheme_code_mainland
	AND SP_ID = @HKUSZH_SP_ID

	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r+2, @c, ISNULL(SUM(TxCnt), 0)
	FROM #VoucherTransaction
	WHERE Age >=70
	AND (Scheme_Code = @scheme_code_mainland)
	AND SP_ID = @HKUSZH_SP_ID

	SET @c= 1
	SET @r= @r+3
	--------------------------------------------------
	

	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	
-----------------------------------------------------------------------------------
------------------------------------------------------------------------------------
--Age >=65
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'Voucher claims for all voucher recipients (i.e. voucher recipients aged 65 or above)')
	SET @r = @r+1

	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	
		
	--Header
	---------------------------------------
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @c = @c+1

	DECLARE myCursor CURSOR LOCAL FAST_FORWARD FOR
		select Practice_Name
		from @PracticeList
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @PracticeName3
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO @DataTable(r,c,Txt)
		VALUES (@r, @c, @PracticeName3)

		SET @c = @c+1
		FETCH NEXT FROM myCursor INTO @PracticeName3
	END
	CLOSE myCursor
	DEALLOCATE myCursor
			
	INSERT INTO @DataTable(r,c,Txt)
	VALUES (@r, @c, 'Total')

	SET @c= 1
	SET @r= @r+1
	----------------------------
	

	--content
	-----------------------------
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'HKD$')
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r+1,@c,N'RMB¥')		
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r+2,@c,'No. of voucher transaction')		
	SET @c = @c+1

	DECLARE myCursor CURSOR LOCAL FAST_FORWARD FOR
		select Display_Seq
		from @PracticeList
	OPEN myCursor
	FETCH NEXT FROM myCursor INTO @Seq3
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r, @c, isnull(SUM(CONVERT(BIGINT, Claim_Amount)),0)
		FROM #VoucherTransaction
		WHERE Age >=65
		AND Scheme_Code = @scheme_code_mainland
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq3

		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r+1, @c, isnull(SUM(CONVERT(MONEY, Claim_Amount_RMB)),0)
		FROM #VoucherTransaction
		WHERE Age >=65
		AND Scheme_Code = @scheme_code_mainland
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq3

		INSERT INTO @DataTable(r,c,Txt)
		SELECT @r+2, @c, ISNULL(SUM(TxCnt), 0)
		FROM #VoucherTransaction
		WHERE Age >=65
		AND (Scheme_Code = @scheme_code_mainland)
		AND SP_ID = @HKUSZH_SP_ID
		AND Practice_Display_Seq = @Seq3

		SET @c = @c+1
		FETCH NEXT FROM myCursor INTO @Seq3
	END
	CLOSE myCursor
	DEALLOCATE myCursor
		
	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r, @c, isnull(SUM(CONVERT(BIGINT, Claim_Amount)),0)
	FROM #VoucherTransaction
	WHERE Age >=65
	AND Scheme_Code = @scheme_code_mainland
	AND SP_ID = @HKUSZH_SP_ID

	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r+1, @c, isnull(SUM(CONVERT(MONEY, Claim_Amount_RMB)),0)
	FROM #VoucherTransaction
	WHERE Age >=65
	AND Scheme_Code = @scheme_code_mainland
	AND SP_ID = @HKUSZH_SP_ID

	INSERT INTO @DataTable(r,c,Txt)
	SELECT @r+2, @c, ISNULL(SUM(TxCnt), 0)
	FROM #VoucherTransaction
	WHERE Age >=65
	AND (Scheme_Code = @scheme_code_mainland)
	AND SP_ID = @HKUSZH_SP_ID

	SET @c= 1
	SET @r= @r+3
	--------------------------------------------------
	

	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	INSERT INTO @DataTable(r,c,Txt) VALUES (@r,@c,'')	
	SET @r = @r+1
	
-----------------------------------------------------------------------------------
	INSERT INTO @ResultTable08 (Result_Value1) VALUES ('Reporting period: as at ' + CONVERT(varchar(10), @ReportDtm, 111))
	INSERT INTO @ResultTable08 (Result_Value1) VALUES ('')

	INSERT INTO @ResultTable08 (	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
		Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20,
		Result_Value21, Result_Value22, Result_Value23, Result_Value24, Result_Value25, 
		Result_Value26, Result_Value27, Result_Value28, Result_Value29, Result_Value30,
		Result_Value31, Result_Value32, Result_Value33, Result_Value34, Result_Value35, 
		Result_Value36, Result_Value37, Result_Value38, Result_Value39, Result_Value40,
		Result_Value41, Result_Value42, Result_Value43, Result_Value44, Result_Value45, 
		Result_Value46, Result_Value47, Result_Value48, Result_Value49, Result_Value50
	)
	SELECT
		ISNULL(C1, ''),  ISNULL(C2, ''),  ISNULL(C3, ''),  ISNULL(C4, ''),  ISNULL(C5, ''),  
		ISNULL(C6, ''),  ISNULL(C7, ''),  ISNULL(C8, ''),  ISNULL(C9, ''),  ISNULL(C10, ''),
		ISNULL(C11, ''), ISNULL(C12, ''), ISNULL(C13, ''), ISNULL(C14, ''), ISNULL(C15, ''), 
		ISNULL(C16, ''), ISNULL(C17, ''), ISNULL(C18, ''), ISNULL(C19, ''), ISNULL(C20, ''),
		ISNULL(C21, ''), ISNULL(C22, ''), ISNULL(C23, ''), ISNULL(C24, ''), ISNULL(C25, ''), 
		ISNULL(C26, ''), ISNULL(C27, ''), ISNULL(C28, ''), ISNULL(C29, ''), ISNULL(C30, ''),
		ISNULL(C31, ''), ISNULL(C32, ''), ISNULL(C33, ''), ISNULL(C34, ''), ISNULL(C35, ''), 
		ISNULL(C36, ''), ISNULL(C37, ''), ISNULL(C38, ''), ISNULL(C39, ''), ISNULL(C40, ''),
		ISNULL(C41, ''), ISNULL(C42, ''), ISNULL(C43, ''), ISNULL(C44, ''), ISNULL(C45, ''), 
		ISNULL(C46, ''), ISNULL(C47, ''), ISNULL(C48, ''), ISNULL(C49, ''), ISNULL(C50, '')
	FROM
		(
			SELECT
				D.R,
				F.ColName,
				D.Txt
			FROM
				@FrameTable F
					INNER JOIN @DataTable D
						ON F.C = D.C
		) P PIVOT (
			MAX(Txt)
			FOR ColName IN (C1,  C2,  C3,  C4,  C5,  
							C6,  C7,  C8,  C9,  C10,
							C11, C12, C13, C14, C15, 
							C16, C17, C18, C19, C20,
							C21, C22, C23, C24, C25, 
							C26, C27, C28, C29, C30,
							C31, C32, C33, C34, C35, 
							C36, C37, C38, C39, C40,
							C41, C42, C43, C44, C45, 
							C46, C47, C48, C49, C50)
		) AS PVT
	ORDER BY
		R

-- End of 08
------------------------------------------------------------
------------------------------------------------------------

-- =============================================
-- Insert to statistics table
-- =============================================
	DELETE FROM RpteHSM0006VoucherClaimByAge	
	DELETE FROM RpteHSM0006VRByAgeStat
	DELETE FROM RpteHSM0006VoucherClaimByAge_Over70_Since2018
	DELETE FROM RpteHSM0006VoucherClaimByProfStat
	DELETE FROM RpteHSM0006VoucherClaimByProfStat_HCVSDHC
	DELETE FROM RpteHSM0006VoucherClaimByReasonForVisitStat
	DELETE FROM RpteHSM0006VoucherClaimByReasonForVisitStat_HCVSDHC
	DELETE FROM RpteHSM0006VoucherClaimByAgeByPractice_HCVSDHC
	DELETE FROM RpteHSM0006VoucherClaimByAgeByPractice_HCVSCHN
	
	
	INSERT INTO RpteHSM0006VoucherClaimByAge (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4
	FROM	
		@ResultTableSummary
	ORDER BY	
		Result_Seq


	INSERT INTO RpteHSM0006VRByAgeStat (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,
		Col11, Col12, Col13
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13
	FROM	
		@ResultTable01
	ORDER BY	
		Result_Seq

	
	INSERT INTO RpteHSM0006VoucherClaimByAge_Over70_Since2018 (
		Display_Seq,
		Col1,
		Col2
	)
	SELECT		
		Result_Seq,
		Result_Value1,
		Result_Value2
	FROM	
		@ResultTable02
	ORDER BY	
		Result_Seq

	INSERT INTO RpteHSM0006VoucherClaimByProfStat (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,
		Col11, Col12
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12
	FROM	
		@ResultTable03
	ORDER BY	
		Result_Seq


	INSERT INTO RpteHSM0006VoucherClaimByProfStat_HCVSDHC (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4
	FROM	
		@ResultTable04
	ORDER BY	
		Result_Seq


	INSERT INTO RpteHSM0006VoucherClaimByReasonForVisitStat (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,
		Col11, Col12, Col13
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13
	FROM	
		@ResultTable05
	ORDER BY	
		Result_Seq


	INSERT INTO RpteHSM0006VoucherClaimByReasonForVisitStat_HCVSDHC (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4,  Col5
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5
	FROM	
		@ResultTable06
	ORDER BY	
		Result_Seq


	INSERT INTO RpteHSM0006VoucherClaimByAgeByPractice_HCVSDHC (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8
	FROM	
		@ResultTable07
	ORDER BY	
		Result_Seq


	INSERT INTO RpteHSM0006VoucherClaimByAgeByPractice_HCVSCHN (
		Display_Seq,
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,
		Col11, Col12, Col13, Col14, Col15, 
		Col16, Col17, Col18, Col19, Col20,
		Col21, Col22, Col23, Col24, Col25, 
		Col26, Col27, Col28, Col29, Col30,
		Col31, Col32, Col33, Col34, Col35, 
		Col36, Col37, Col38, Col39, Col40,
		Col41, Col42, Col43, Col44, Col45, 
		Col46, Col47, Col48, Col49, Col50
	)
	SELECT	
		Result_Seq,	
		Result_Value1,  Result_Value2,  Result_Value3,  Result_Value4,  Result_Value5,  
		Result_Value6,  Result_Value7,  Result_Value8,  Result_Value9,  Result_Value10,
		Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
		Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20,
		Result_Value21, Result_Value22, Result_Value23, Result_Value24, Result_Value25, 
		Result_Value26, Result_Value27, Result_Value28, Result_Value29, Result_Value30,
		Result_Value31, Result_Value32, Result_Value33, Result_Value34, Result_Value35, 
		Result_Value36, Result_Value37, Result_Value38, Result_Value39, Result_Value40,
		Result_Value41, Result_Value42, Result_Value43, Result_Value44, Result_Value45, 
		Result_Value46, Result_Value47, Result_Value48, Result_Value49, Result_Value50
	FROM	
		@ResultTable08
	ORDER BY	
		Result_Seq

-- ---------------------------------------------
-- Drop the temporary tables
-- ---------------------------------------------
	
	DROP TABLE #vouchertransaction
	DROP TABLE #VoucherAccount
	DROP TABLE #VT_ReasonForVisit
	
END
GO

GRANT EXECUTE ON proc_EHS_eHSM0006_Report_Write TO HCVU
GO


