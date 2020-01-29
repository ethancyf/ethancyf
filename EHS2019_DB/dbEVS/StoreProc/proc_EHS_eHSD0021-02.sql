IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-02]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-02]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Prepare new temp table #tempTransaction
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
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	9 May 2013
-- Description:		Add Joined status
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	05 APR 2012
-- Description:		CRP11-005 - Summary of transactions input by SP
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-02]
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

	DECLARE @Valid_Incomplete int
	declare @Valid_PendConfirm as int
	declare @Valid_PendeHealth as int
	declare @Valid_ReadyReimburse as int
	declare @Valid_Reimburse as int
	declare @totalValidTrans as int
	declare @Valid_Suspended as int
	declare @Valid_Joined as int

declare @Invalid_Pending as int
	declare @Invalided as int
	
	declare @Invalid_Voided as int
	declare @totalInValidTrans as int

	DECLARE @ResultTable2 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT '',
		Result_Value2	varchar(100) DEFAULT '',
		Result_Value3	varchar(100) DEFAULT '',
		Result_Value4	varchar(100) DEFAULT '',
		Result_Value5	varchar(100) DEFAULT '',
		Result_Value6	varchar(100) DEFAULT '',
		Result_Value7	varchar(100) DEFAULT '',
		Result_Value8	varchar(100) DEFAULT ''

	)

--	DECLARE @tempAccount table
--	(
--		encrypt_field1		varbinary(100),
--		Doc_code			varchar(20),
--		Scheme_code			varchar(10)
--		
--	)

-- =============================================
-- Retrieve data
-- =============================================

---No of validated transactions input by SP

--Incomplete
select @Valid_Incomplete = ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='U' And  Invalidation <> 'I'

--Pending Confirm
select @Valid_PendConfirm= ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='P' And Invalidation <> 'I'
--Pending eHealth
select @Valid_PendeHealth= ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='V' And Invalidation <> 'I'

--Ready to Reimburse
select @Valid_ReadyReimburse= ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='A' And Invalidation <> 'I'

--Reimbursed
select @Valid_Reimburse= ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='R' And Invalidation <> 'I'

--Suspended
select @Valid_Suspended = ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='S' And Invalidation <> 'I'

--Joined
select @Valid_Joined = ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='J' And Invalidation <> 'I'

set @totalValidTrans =@Valid_Incomplete+@Valid_PendConfirm+@Valid_PendeHealth+@Valid_ReadyReimburse+@Valid_Reimburse+@Valid_Suspended+@Valid_Joined




---No of Invalidated transactions input by SP
--Reimbursed Pending Invalidation
select @Invalid_Pending = ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And Record_Status ='R' and  ( Invalidation ='P')

--Reimbursed Invalidated
select @Invalided = ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And   Record_Status ='R' and  ( Invalidation ='I')



--Voided
select @Invalid_Voided = ISNULL(SUM(TxCount),0) from #tempTransaction where manual_reimburse='N' 
	And   Record_Status ='I'

set @totalInValidTrans=@Invalid_Pending+@Invalided+@Invalid_Voided

-- =============================================
-- Construct layout
-- =============================================


	

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1) VALUES
	(0, 'eHS(S)D0021-02: Summary of transactions input by Service Provider')
	
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(1)
		
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1) VALUES
	(2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(3)
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1 ) VALUES
	(4,  'Break down by Validated Status')
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8 ) VALUES
	(5,  'Incomplete', 'Pending Confirmation','Pending eHealth (Subsidies) Account Validation','Suspended','Ready to Reimburse','Reimbursed','Joined','Cumulative Total')
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8) VALUES
	(6, @Valid_Incomplete,@Valid_PendConfirm,@Valid_PendeHealth,@Valid_Suspended, @Valid_ReadyReimburse,@Valid_Reimburse,@Valid_Joined,@totalValidTrans)


	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(7)
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(8)
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1 ) VALUES
	(9,  'Break down by Invalidated Status')
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4 ) VALUES
	(10,  'Reimbursed Pending Invalidation', 'Invalidated','Voided','Cumulative Total')
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4 ) VALUES
	(11, @Invalid_Pending,@Invalided, @Invalid_Voided,@totalInValidTrans)




	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(12)
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(13)	

-- ---------------------------------------------
-- Select Data
-- ---------------------------------------------



	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8
	FROM
		@ResultTable2
	ORDER BY
		Result_Seq


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-02] TO HCVU
GO

