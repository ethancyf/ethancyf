IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-03]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-03]
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
-- Description:		CRP11-005 - Summary of transactions input by Back Office
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-03]
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

	DECLARE @Valid_Approval int
	declare @Valid_Reimburse as int
	declare @Valid_Joined as int
	declare @totalValidTrans as int

	
	declare @Invalid_Removed as int
	declare @Invalid_Pending as int
	declare @Invalided as int		
	declare @totalInValidTrans as int

	DECLARE @ResultTable3 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT '',
		Result_Value2	varchar(100) DEFAULT '',
		Result_Value3	varchar(100) DEFAULT '',
		Result_Value4	varchar(100) DEFAULT '',
		Result_Value5	varchar(100) DEFAULT ''

	)

		
-- =============================================
-- Retrieve data
-- =============================================

---No of validated transactions input by Back office

--Pending Approval
select @Valid_Approval = ISNULL(SUM(TxCount),0) from #tempTransaction where 
manual_reimburse='Y' and  Record_Status ='B' And Invalidation <> 'I'

--Reimbursed
select @Valid_Reimburse= ISNULL(SUM(TxCount),0) from #tempTransaction where 
manual_reimburse='Y' and  Record_Status ='R' And Invalidation <> 'I'

--Joined
select @Valid_Joined= ISNULL(SUM(TxCount),0) from #tempTransaction where 
manual_reimburse='Y' and  Record_Status ='J' And Invalidation <> 'I' -- (Joined status is no invalidation)

set @totalValidTrans =@Valid_Approval+@Valid_Reimburse+@Valid_Joined


---No of Invalidated transactions input by Back office


--Removed
select @Invalid_Removed =  ISNULL(SUM(TxCount),0) from #tempTransaction where 
manual_reimburse='Y' and  Record_Status ='D' And Invalidation <> 'I'

--Invalidated
select @Invalided = ISNULL(SUM(TxCount),0) from #tempTransaction where 
manual_reimburse='Y' and  Record_Status ='R' And ( Invalidation = ('I'))


--Pending Invalidation
select @Invalid_Pending =ISNULL(SUM(TxCount),0) from #tempTransaction where 
manual_reimburse='Y' and  Record_Status ='R' And ( Invalidation = ('P'))




set @totalInValidTrans=@Invalid_Pending+@Invalided+@Invalid_Removed

-- =============================================
-- Construct layout
-- =============================================


	

	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1) VALUES
	(0, 'eHS(S)D0021-03: Summary of transactions input by Back Office')
	
	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(1)
		
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1) VALUES
	(2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )
	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(3)
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1 ) VALUES
	(4,  'Break down by Validated Status')
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4 ) VALUES
	(5,  'Pending Approval', 'Reimbursed', 'Joined','Cumulative Total')
	
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4) VALUES
	(6, @Valid_Approval,@Valid_Reimburse,@Valid_Joined,@totalValidTrans)


	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(7)
	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(8)
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1 ) VALUES
	(9,  'Break down by Invalidated Status')
	
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4 ) VALUES
	(10,  'Removed', 'Pending Invalidation','Invalidated','Cumulative Total')
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4 ) VALUES
	(11, @Invalid_Removed,@Invalid_Pending,@Invalided,@totalInValidTrans)




	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(12)
	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(13)	

-- ---------------------------------------------
-- Select Data
-- ---------------------------------------------



	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4
	FROM
		@ResultTable3
	ORDER BY
		Result_Seq


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-03] TO HCVU
GO

