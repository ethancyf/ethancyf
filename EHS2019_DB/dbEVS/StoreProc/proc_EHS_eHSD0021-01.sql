IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-01]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-01]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Prepare new temp table #tempAccount and #tempTransaction
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
-- CR No.:			CRE13-019-02
-- Modified by:		Winnie SUEN
-- Modified date:	09 Mar 2015
-- Description:		Include HCVSC for voucher
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	8 May 2013
-- Description:		Add EHAPP total
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	05 APR 2012
-- Description:		CRP11-005 - Summary of transactions
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-01]
@Report_Dtm		datetime = NULL
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	--DECLARE @strYear char(4)
	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, DATEADD(dd, 1, @Report_Dtm), 106)
	END ELSE BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END

	DECLARE @eligibileAge int
	declare @totalTranBySP as int
	declare @totalTranByBackOffice as int
	declare @totalTran as int

	declare @totalTranByVoucherScheme as int
	declare @totalTranByVaccineScheme as int
	declare @totalTranByEHAPPScheme as int
	declare @totalSchemeTran as int

	declare @totalAccountByVoucherScheme as int
	declare @totalAccountByVaccineScheme as int
	declare @totalAccountByEHAPPScheme as int
	

	DECLARE @ResultTable1 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT '',
		Result_Value2	varchar(100) DEFAULT ''
		

	)


-- =============================================
-- Retrieve data
-- =============================================

---No of transactions input by SP


select @totalTranBySP = ISNULL(SUM(TxCount),0) from #tempTransaction where  manual_reimburse='N' 
	And Record_Status Not in ('I', 'D','W') And (Invalidation <> 'I')


---No of transactions input by Back Office
select @totalTranByBackOffice = ISNULL(SUM(TxCount),0) from #tempTransaction  WHERE manual_reimburse='Y'
	And Record_Status Not in ('I', 'D','W') And (Invalidation <>('I'))

set @totalTran =@totalTranBySP+@totalTranByBackOffice

---No of transactions made by voucher/vaccination scheme
select @totalTranByVoucherScheme = ISNULL(SUM(TxCount),0) FROM #tempTransaction where Record_Status Not in ('I','D','W') 
And Invalidation <> 'I' and Subsidize_Type = 'VOUCHER'

select @totalTranByVaccineScheme = ISNULL(SUM(TxCount),0) FROM #tempTransaction where Record_Status Not in ('I','D','W') 
And Invalidation <> 'I' and Subsidize_Type = 'VACCINE'

select  @totalTranByEHAPPScheme =ISNULL(SUM(TxCount),0)  from #tempTransaction where Record_Status Not in ('I','D','W') 
And Invalidation <> 'I' and scheme_code = 'EHAPP'

set @totalSchemeTran =@totalTranByVoucherScheme+@totalTranByVaccineScheme+@totalTranByEHAPPScheme
-- ---------------------------------------------
-- eHealth account
-- ---------------------------------------------
--insert into @tempAccount
--select distinct encrypt_field1, case rtrim(doc_code) when 'HKBC' then 'HKIC' else doc_code end, rtrim(scheme_code) from View_VoucherTranAcc where Transaction_Dtm < @Report_Dtm
--
select @totalAccountByVoucherScheme=count(1) from (select  encrypt_field1, doc_code from #tempAccount where subsidize_item_code='EHCVS' group by encrypt_field1, doc_code) as g
select @totalAccountByVaccineScheme=count(1) from (select  encrypt_field1, doc_code from #tempAccount TA
																						 INNER JOIN SubsidizeItem SI WITH (NOLOCK)
																						 ON TA.Subsidize_Item_Code = SI.Subsidize_Item_Code
													where SI.Subsidize_Type = 'VACCINE' group by encrypt_field1, doc_code) as g
select @totalAccountByEHAPPScheme=count(1) from (select  encrypt_field1, doc_code from #tempAccount where scheme_code='EHAPP' group by encrypt_field1, doc_code) as g

-- =============================================
-- Construct layout
-- =============================================
	

	

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(0, 'eHS(S)D0021-01: Total summary of transactions')
	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(2,  'No. of transactions input by Service Provider',@totalTranBySP)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(3,  'No. of transactions input by Back Office',@totalTranByBackOffice)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(4, 'Cumulative Total',@totalTran)

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(5)
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(6)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(7,  'No. of transactions made in Voucher Scheme',@totalTranByVoucherScheme)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(8,  'No. of transactions made in Vaccination Scheme',@totalTranByVaccineScheme)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(9,  'No. of transactions made in EHAPP Scheme',@totalTranByEHAPPScheme)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(10,  'Cumulative Total',@totalSchemeTran)

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(11)
	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(12)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(13,  'No. of eHealth (Subsidies) Account with subsidy claim in Voucher Scheme',@totalAccountByVoucherScheme)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(14,  'No. of eHealth (Subsidies) Account with subsidy claim in Vaccination Scheme',@totalAccountByVaccineScheme)
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1,Result_Value2 ) VALUES
	(15,  'No. of eHealth (Subsidies) Account with subsidy claim in EHAPP Scheme',@totalAccountByEHAPPScheme)
	

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(16)
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(17)	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(18)
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(19)
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(20)
	INSERT INTO @ResultTable1 (Result_Seq,Result_Value1,Result_Value2 ) VALUES
	(21, 'Notes:','')
	INSERT INTO @ResultTable1 (Result_Seq,Result_Value1,Result_Value2 ) VALUES
	(22, 'i) Invalidated transactions, removed and voided transactions are excluded','')

-- ---------------------------------------------
-- Select Data
-- ---------------------------------------------



	SELECT 
		Result_Value1,
		Result_Value2
	FROM
		@ResultTable1
	ORDER BY
		Result_Seq


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-01] TO HCVU
GO

