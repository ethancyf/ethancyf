IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-07]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-07]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Use new temp table #tempAccount and #tempTransaction
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
-- CR No:			INT13-0019 - Fix eHSD0021 scheme list
-- Modified by:		Tommy Lam  
-- Modified date:	15 Aug 2013  
-- Description:		Include the record with 0 value even if there is no record in the Report of eHSD0021-07
-- =============================================  
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	9 May 2013
-- Description:		eHSD0021-07: Summary of EHAPP claim break down by period
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-07]
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
	

	DECLARE @ResultTable4 table (
		Result_Seq		smallint,	
		Result_Value2	varchar(100) DEFAULT '',
		Result_Value3	varchar(100) DEFAULT '',
		Result_Value4	varchar(100) DEFAULT ''
	)

	DECLARE @AllActiveScheme_EHAPP TABLE (
		Scheme_Seq		smallint
	)

	DECLARE @TempResultTable4 TABLE (
		Scheme_Seq				smallint,
		Subsidize_item_code		varchar(10),
		Result_Value4			int
	)

-- =============================================
-- Initialization
-- =============================================

	INSERT INTO @AllActiveScheme_EHAPP (
		Scheme_Seq
	)
	SELECT Scheme_Seq FROM SchemeBackOffice
		WHERE Scheme_Code = 'EHAPP'
			AND Record_Status = 'A'
			AND @Report_Dtm >= Effective_Dtm

-- =============================================
-- Construct layout
-- =============================================

	INSERT INTO @ResultTable4 (Result_Seq, Result_Value2) VALUES
	(0, 'eHS(S)D0021-07: Summary of EHAPP claim break down by period')
	
	INSERT INTO @ResultTable4 (Result_Seq) VALUES
	(1)
		
	INSERT INTO @ResultTable4 (Result_Seq, Result_Value2) VALUES
	(2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )
	INSERT INTO @ResultTable4 (Result_Seq) VALUES
	(3)
	insert into @ResultTable4 (Result_Seq, Result_Value2,Result_Value3,Result_Value4) 
	values( 4 , 'Period' ,'No. of Transaction','No. of eHealth (Subsidies) Account involve')

	insert into @TempResultTable4 (Scheme_Seq, Subsidize_item_code, Result_Value4)
	select Scheme_seq, subsidize_item_code, count(1) from  #tempAccount
	where scheme_code='EHAPP' Group by subsidize_item_code, Scheme_seq
 
	insert into @ResultTable4 (Result_Seq, Result_Value2, Result_Value3, Result_Value4) 
	select 5 , AAS_EHAPP.Scheme_Seq , 0, ISNULL(TRT4.Result_Value4, 0)
	from @AllActiveScheme_EHAPP AAS_EHAPP LEFT JOIN @TempResultTable4 TRT4
		on AAS_EHAPP.Scheme_Seq = TRT4.Scheme_Seq
	order by AAS_EHAPP.Scheme_Seq, TRT4.Subsidize_item_code

	update @ResultTable4 set Result_Value3=  T.cnt from @ResultTable4 r, 
	(select ISNULL(SUM(TxCount),0) as cnt, 1 AS Scheme_Seq from #tempTransaction
	where Record_Status Not in ('I', 'D','W') 
	And Invalidation <> 'I' and scheme_code='EHAPP'
	group by scheme_code
	) as T
	where  T.scheme_seq=r.Result_Value2 and r.Result_seq>=5


	update @ResultTable4 set Result_Value2= cast(year(sg.claim_period_from) as varchar(4)) + ' - ' + cast(year(sg.last_service_dtm) as varchar(4))
	 from  subsidizegroupclaim sg,@ResultTable4 r
	 where sg.scheme_seq=r.Result_Value2
	 and sg.scheme_code='EHAPP' and r.Result_seq>=5



	INSERT INTO @ResultTable4 (Result_Seq) VALUES
	(12)
	INSERT INTO @ResultTable4 (Result_Seq) VALUES
	(13)	

INSERT INTO @ResultTable4 (Result_Seq,Result_Value2,Result_Value3 ) VALUES
	(19, 'Notes:','')
	INSERT INTO @ResultTable4 (Result_Seq,Result_Value2,Result_Value3 ) VALUES
	(20, 'i) Invalidated transactions, removed and voided transactions are excluded','')

-- ---------------------------------------------
-- Select Data
-- ---------------------------------------------



	SELECT 		
		Result_Value2,
		Result_Value3,
		Result_Value4
	FROM
		@ResultTable4
	ORDER BY
		Result_Seq


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-07] TO HCVU
GO

