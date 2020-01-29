IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-04]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-04]
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
-- CR No:   INT13-0019 - Fix eHSD0021 scheme list
-- Modified by:  Tommy Lam
-- Modified date: 20 Aug 2013
-- Description:  Include the Scheme record with 0 value even if there is no record in the Report of eHSD0021-04
-- =============================================    
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	29 May 2013
-- Description:		Change to content of eHSD0021-06
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	05 APR 2012
-- Description:		CRP11-005 - Summary of Voucher claim break down by year
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-04]
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

	

	DECLARE @ResultTable6 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT ''   ,
		Result_Value2	varchar(100) DEFAULT ''  ,
		Result_Value3	varchar(100) DEFAULT '' 

	)

	DECLARE @TempResultTable6 TABLE (
		Scheme_Code		char(10),
		Result_Value2	int
	)	

	DECLARE @AllActiveScheme TABLE (  
		Scheme_Code char(10)  
	)

-- =============================================
-- Initialization
-- =============================================

	INSERT INTO @AllActiveScheme (  
		Scheme_Code  
	)  
	SELECT DISTINCT SECM.Scheme_Code_Claim
		FROM SchemeBackOffice SBO
			INNER JOIN SchemeEnrolClaimMap SECM
				ON SBO.Scheme_Code = SECM.Scheme_Code_Enrol
		WHERE @Report_Dtm >= SBO.Effective_Dtm
			AND SBO.Record_Status = 'A'
			AND SECM.Record_Status = 'A'

--declare  @VaccineClaim as table
--( 
-- totalcount   int,
-- scheme_code   varchar(10)
--)   
--declare  @VaccineAccount as table
--( 
-- encrypt_field1		varbinary(100),
--	Doc_code			varchar(20),
--	scheme_code			varchar(10)
--)


-- =============================================
-- Retrieve data
-- =============================================
--insert into @VaccineClaim
--select  count(1),scheme_code from view_vouchertranacc where transaction_dtm<@Report_Dtm 
--group by scheme_code , scheme_seq, subsidize_code
--
--insert into @VaccineAccount
--select distinct encrypt_field1, doc_code,scheme_code  from view_vouchertranacc group by scheme_code, encrypt_field1, doc_code



-- =============================================
-- Construct layout
-- =============================================


	INSERT INTO @ResultTable6 (Result_Seq, Result_Value1) VALUES
	(0, 'eHS(S)D0021-04: Summary of claim break down by Scheme')
	
	INSERT INTO @ResultTable6 (Result_Seq) VALUES
	(1)
		
	INSERT INTO @ResultTable6 (Result_Seq, Result_Value1) VALUES
	(2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )
	INSERT INTO @ResultTable6 (Result_Seq) VALUES
	(3)
	insert into @ResultTable6 (Result_Seq, Result_Value1,Result_Value2,Result_Value3) 
	values( 4 ,'Scheme Code', 'No. of Transaction','No. of eHealth (Subsidies) Account involve')

	INSERT INTO @TempResultTable6 (Scheme_Code, Result_Value2)
	SELECT Scheme_Code, ISNULL(SUM(TxCount),0) FROM #tempTransaction
		WHERE Record_Status NOT IN ('I', 'D', 'W')
			AND Invalidation <> 'I'
		GROUP BY Scheme_Code
	
	INSERT INTO @ResultTable6 (Result_Seq, Result_Value1, Result_Value2, Result_Value3) 
	SELECT 5, AAS.Scheme_Code, ISNULL(TRT6.Result_Value2, 0), 0
		FROM @AllActiveScheme AAS LEFT JOIN @TempResultTable6 TRT6
			ON AAS.Scheme_Code = TRT6.Scheme_Code COLLATE DATABASE_DEFAULT
		ORDER BY AAS.Scheme_Code

	update @ResultTable6 set Result_Value3= b.cnt from @ResultTable6 r, 
	( select a.scheme_code, count(1) as cnt from 
	(	select va.scheme_code from #tempAccount  va group by scheme_code, encrypt_field1, doc_code ) as a
	group by a.scheme_code
	) as b
	where b.scheme_code=r.Result_Value1 collate database_default  and  r.Result_seq>=5

	






	INSERT INTO @ResultTable6 (Result_Seq) VALUES
	(12)
	INSERT INTO @ResultTable6 (Result_Seq) VALUES
	(13)	

	INSERT INTO @ResultTable6 (Result_Seq,Result_Value1,Result_Value2,Result_Value3 ) VALUES
	(19, 'Notes:','','')
	INSERT INTO @ResultTable6 (Result_Seq,Result_Value1,Result_Value2,Result_Value3 ) VALUES
	(20, 'i) Invalidated transactions, removed and voided transactions are excluded','','')

-- ---------------------------------------------
-- Select Data
-- ---------------------------------------------



	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3
	FROM
		@ResultTable6
	ORDER BY
		Result_Seq


set nocount off

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-04] TO HCVU
GO

