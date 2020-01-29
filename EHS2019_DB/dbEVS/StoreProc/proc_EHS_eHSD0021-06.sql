IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-06]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-06]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Use new temp table #tempAccount and #tempTransactionVaccineSeason
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-001
-- Modified by:		Winnie SUEN
-- Modified date:	28 Apr 2016
-- Description:		Replace table [StatVaccineMapping] with new table [VaccineSeason]
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
-- CR No.:			CRE15-004 (TIV and QIV)
-- Modified by:		Chris YIM
-- Modified date:	31 July 2015
-- Description:		Change grouping by scheme sequence to season
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-021-02
-- Modified by:		Tommy LAM
-- Modified date:	03 Jan 2014
-- Description:		Remove the special handling of Vaccine - "23vPPV"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	29 May 2013
-- Description:		Rectify vaccine filter by [SubsidizeItem].[Subsidize_Type] = 'VACCINE'
--					Change to content of eHSD0021-05
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	05 APR 2012
-- Description:		CRP11-005 - Summary of claims break down by Scheme
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-06]
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

	

	DECLARE @ResultTable5 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT '',
		Result_Value2	varchar(100) DEFAULT '',
		Result_Value3	varchar(100) DEFAULT '',
		Result_Value4	varchar(100) DEFAULT ''

	)

	DECLARE @preTempAccount TABLE (
		encrypt_field1			varbinary(100),
		Doc_code				varchar(20),
		scheme_code				varchar(10),
		scheme_seq				int,
		Subsidize_item_code		varchar(10),
		Subsidize_type			varchar(20),
		Season					varchar(100)
	)		

--	DECLARE @TempAccount TABLE
--	(
--		encrypt_field1			varbinary(100),
--		Doc_code				varchar(20),
--		Subsidize_item_code		varchar(10),
--		scheme_seq				int
--	)

	DECLARE @TempAccount TABLE
	(
		encrypt_field1			varbinary(100),
		Doc_code				varchar(20),
		Subsidize_item_code		varchar(10),
		Season					varchar(100)		
	)

	DECLARE @SubsidizeGroupClaim TABLE (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)

--	DECLARE @AllVaccineByPeriod TABLE (
--		Subsidize_Item_Code		char(10),
--		Scheme_Seq				smallint
--	)

	DECLARE @AllVaccineByPeriod TABLE (
		Subsidize_Item_Code		char(10),
		Season					varchar(100)
	)

--	DECLARE @ReportTempResult TABLE (
--		Subsidize_Item_Code		char(10),
--		Scheme_Seq				smallint,
--		No_Of_Trans				int
--	)

	DECLARE @ReportTempResult TABLE (
		Subsidize_Item_Code		char(10),
		Season					varchar(100),
		No_Of_Trans				int
	)

	INSERT INTO @SubsidizeGroupClaim (Scheme_Code, Scheme_Seq)
	SELECT Scheme_Code, Scheme_Seq
	FROM SubsidizeGroupClaim WITH (NOLOCK)
	WHERE Record_Status = 'A'
	GROUP BY Scheme_Code, Scheme_Seq
	HAVING MIN(Claim_Period_From) <= @Report_Dtm

--	INSERT INTO @AllVaccineByPeriod (Subsidize_Item_Code, Scheme_Seq)
--	SELECT SVM.Subsidize_Item_Code, SVM.Scheme_Seq
--	FROM StatVaccineMapping SVM
--		INNER JOIN @SubsidizeGroupClaim SGC
--			ON SVM.Scheme_Code = SGC.Scheme_Code COLLATE DATABASE_DEFAULT AND SVM.Scheme_Seq = SGC.Scheme_Seq
--	GROUP BY SVM.Subsidize_Item_Code, SVM.Scheme_Seq
	INSERT INTO @AllVaccineByPeriod (Subsidize_Item_Code, Season)
	SELECT VS.Subsidize_Item_Code, VS.Season_Desc
	FROM VaccineSeason VS WITH (NOLOCK)
		INNER JOIN @SubsidizeGroupClaim SGC 
			ON VS.Scheme_Code = SGC.Scheme_Code COLLATE DATABASE_DEFAULT AND VS.Scheme_Seq = SGC.Scheme_Seq
	GROUP BY VS.Subsidize_Item_Code, VS.Season_Desc

-- =============================================
-- Retrieve data
-- =============================================
----Mapping the season with eHealth Account 
--	INSERT INTO @preTempAccount
--		select ta.encrypt_field1, ta.doc_code, ta.scheme_code, ta.scheme_seq, ta.subsidize_item_code, ta.Subsidize_type, VS.season_Desc
--		from #tempAccount ta
--			LEFT OUTER JOIN VaccineSeason VS WITH (NOLOCK)
--				ON ta.scheme_code = VS.scheme_code and ta.scheme_seq = VS.scheme_seq and ta.subsidize_item_code = VS.subsidize_item_code
----Got Voucher eHealth Account 
----
----	INSERT INTO @TempAccount
----	select  encrypt_field1,doc_code,subsidize_item_code,scheme_seq from #tempAccount
----	 where subsidize_type = 'VACCINE' group by encrypt_field1,doc_code,subsidize_item_code,scheme_seq
--	INSERT INTO @TempAccount
--	select  encrypt_field1,doc_code,subsidize_item_code,season from @preTempAccount
--	 where subsidize_type = 'VACCINE' group by encrypt_field1,doc_code,subsidize_item_code,season

----	INSERT INTO @ReportTempResult (Subsidize_Item_Code, Scheme_Seq, No_Of_Trans)
----	select subsidize_item_code, Scheme_seq ,count(1) from  
----	@TempAccount group by subsidize_item_code, Scheme_seq

--	INSERT INTO @ReportTempResult (Subsidize_Item_Code, Season, No_Of_Trans)
--	select subsidize_item_code, Season ,count(1) from  
--	@TempAccount group by subsidize_item_code, Season

	INSERT INTO @ReportTempResult (Subsidize_Item_Code, Season, No_Of_Trans)
	SELECT Subsidize_Item_Code, Season_Desc, COUNT(1) FROM
			(
				SELECT DISTINCT TA.Subsidize_Item_Code, VS.Season_Desc, TA.Encrypt_Field1, TA.Doc_code
				FROM #tempAccount TA
					INNER JOIN SubsidizeItem SI WITH (NOLOCK) 
						ON SI.Subsidize_Item_Code = TA.Subsidize_Item_Code
					INNER JOIN VaccineSeason VS WITH (NOLOCK)
						ON TA.scheme_code = VS.scheme_code and TA.scheme_seq = VS.scheme_seq and TA.subsidize_item_code = VS.subsidize_item_code
				WHERE  SI.Subsidize_type = 'VACCINE'
			) A
	GROUP BY Subsidize_Item_Code, Season_Desc
-- =============================================
-- Construct layout
-- =============================================


	INSERT INTO @ResultTable5 (Result_Seq, Result_Value1) VALUES
	(0, 'eHS(S)D0021-06: Summary of vaccination claim break down by Vaccination Season')
	
	INSERT INTO @ResultTable5 (Result_Seq) VALUES
	(1)
		
	INSERT INTO @ResultTable5 (Result_Seq, Result_Value1) VALUES
	(2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )
	INSERT INTO @ResultTable5 (Result_Seq) VALUES
	(3)
	insert into @ResultTable5 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4) 
	values( 4 ,'Vaccine Code', 'Vaccination Season' ,'No. of Transaction','No. of eHealth (Subsidies) Account involve')

	
--	insert into @ResultTable5 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4) 
--	SELECT 5, AVP.Subsidize_Item_Code, AVP.Scheme_Seq, 0, ISNULL(RTR.No_Of_Trans, 0)
--	FROM @AllVaccineByPeriod AVP
--		LEFT JOIN @ReportTempResult RTR
--			ON AVP.Subsidize_Item_Code = RTR.Subsidize_Item_Code AND AVP.Scheme_Seq = RTR.Scheme_Seq
--	order by AVP.Subsidize_Item_Code, AVP.Scheme_Seq
	INSERT INTO @ResultTable5 (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4) 
	SELECT 5, AVP.Subsidize_Item_Code, AVP.Season, 0, ISNULL(RTR.No_Of_Trans, 0)
	FROM @AllVaccineByPeriod AVP
		LEFT JOIN @ReportTempResult RTR
			ON AVP.Subsidize_Item_Code = RTR.Subsidize_Item_Code AND AVP.Season = RTR.Season
	order by AVP.Subsidize_Item_Code, AVP.Season

--	UPDATE @ResultTable5 SET Result_Value3=  T.cnt FROM @ResultTable5 r, 
--	(SELECT COUNT(1) AS cnt, vd.subsidize_item_code, vd.scheme_seq 
--	 FROM vouchertransaction vt,transactiondetail vd  
--		  INNER JOIN subsidizeItem si ON vd.Subsidize_item_Code = si.Subsidize_item_Code
--	 WHERE transaction_dtm<@Report_Dtm AND vt.Record_Status NOT IN ('I', 'D','W')   
--			AND (Invalidation IS NULL OR Invalidation NOT IN ('I')) AND si.Subsidize_Type = 'VACCINE' 
--			AND vt.transaction_id=vd.transaction_id AND vt.scheme_code=vd.scheme_code  
--	 GROUP BY vd.scheme_seq, vd.subsidize_item_code  
--	) AS T
--	WHERE T.subsidize_item_code=r.Result_Value1 AND T.scheme_seq=r.Result_Value2 AND r.Result_seq>=5

	UPDATE @ResultTable5 SET Result_Value3=  T.TxCount FROM @ResultTable5 r, 
		#tempTransactionVaccineSeason T
		--(SELECT COUNT(1) AS cnt, vd.subsidize_item_code, VS.Season_Desc
		--FROM vouchertransaction vt WITH (NOLOCK),transactiondetail vd WITH (NOLOCK)  
		--	INNER JOIN subsidizeItem si WITH (NOLOCK)
		--		ON vd.Subsidize_item_Code = si.Subsidize_item_Code
		--	LEFT JOIN VaccineSeason VS WITH (NOLOCK)
		--		ON vd.scheme_code = VS.scheme_code and vd.scheme_seq = VS.scheme_seq and vd.subsidize_item_code = VS.subsidize_item_code
		-- WHERE transaction_dtm<@Report_Dtm AND vt.Record_Status NOT IN ('I', 'D','W')   
		--		AND (Invalidation IS NULL OR Invalidation NOT IN ('I')) AND si.Subsidize_Type = 'VACCINE' 
		--		AND vt.transaction_id=vd.transaction_id AND vt.scheme_code=vd.scheme_code  
		-- GROUP BY VS.Season_Desc, vd.subsidize_item_code  
		--) AS T
	WHERE T.subsidize_item_code=r.Result_Value1 AND T.Season_Desc=r.Result_Value2 AND r.Result_seq>=5

--	update @ResultTable5 set Result_Value2=  a.strYear from
--	(
--select distinct s.subsidize_item_code, sg.scheme_seq , cast(year(sg.claim_period_from) as varchar(4))+'/'+cast(year(sg.claim_period_to) as varchar(4)) as strYear
--	 from subsidize s, subsidizegroupclaim sg
--	 where s.subsidize_code=sg.subsidize_code
--	 and  s.subsidize_item_code not in ('PV', 'EHCVS')
--	) as a, @ResultTable5 r
--	where a.subsidize_item_code=r.Result_Value1 and r.Result_Value1<>'PV'
--	and rtrim(a.scheme_seq)=rtrim(r.Result_Value2)

--	update @ResultTable5 set Result_Value2 =sm.season from 
--	 StatVaccineMapping  sm, @ResultTable5 r
--	where sm.subsidize_item_code=r.Result_Value1 and sm.scheme_seq=r.Result_Value2
--	and Result_Seq>=5
--select * from @ResultTable5

	INSERT INTO @ResultTable5 (Result_Seq) VALUES
	(12)
	INSERT INTO @ResultTable5 (Result_Seq) VALUES
	(13)	

INSERT INTO @ResultTable5 (Result_Seq,Result_Value1,Result_Value2,Result_Value3 ) VALUES
	(19, 'Notes:','','')
	INSERT INTO @ResultTable5 (Result_Seq,Result_Value1,Result_Value2,Result_Value3 ) VALUES
	(20, 'i) Invalidated transactions, removed and voided transactions are excluded','','')

-- ---------------------------------------------
-- Select Data
-- ---------------------------------------------



	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4
	FROM
		@ResultTable5
	ORDER BY
		Result_Seq


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-06] TO HCVU
GO
