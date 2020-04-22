IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00004_GetData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00004_GetData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			INT15-0003
-- Modified by:		Lawrence TSANG
-- Modified date:	8 May 2015
-- Description:		Fix the filtering criteria on VoucherTransaction.Record_Status
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			INT13-0011
-- Modified by:		Koala CHENG  
-- Modified date:	14 May 2013  
-- Description:		Fix database collation problem
-- =============================================  
-- =============================================
-- Author:		Tommy Tse
-- Create date:		19 Nov 2012
-- Description:		Get Data for Statistics - STAT00004
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00004_GetData] 
	@submission_method  VARCHAR(5000), -- ExternalWS, IVRS, WEB-FULLS, WEB-FULLM, WEB-TEXTS, WEB-TEXTM [Delimiter ',']
	@period_from		DATETIME,
	@period_to			DATETIME,
	@scheme_code		VARCHAR(10),
	@period_type		VARCHAR(1) --T, S
AS BEGIN

-- =============================================  
-- Declaration  
-- =============================================  
  	 SET NOCOUNT ON  
  
	 DECLARE @delimiter varchar(5)  

	 DECLARE @SubmissionMethod TABLE (  
	  SubmissionMethod  VARCHAR(10) COLLATE Chinese_Taiwan_Stroke_CI_AS 
	 )  

-- =============================================  
-- Validation  
-- ============================================= 

	IF @submission_method IS NULL  
	  SET @submission_method = ''  

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL

	IF @period_from > @period_to
		RETURN

	IF @scheme_code IS NULL
		SET @scheme_code = ''

	IF @period_type IS NULL
		SET @period_type = ''

-- =============================================  
-- Initialization  
-- =============================================  
  	SET @delimiter = ','  

	IF @submission_method <> ''  
	BEGIN  
		INSERT INTO @SubmissionMethod (  
			SubmissionMethod  
		)  
		SELECT Item FROM func_split_string(@submission_method, @delimiter)  
	END  

-- =============================================  
-- Retrieve Data  
-- =============================================  
	--Logic to retrieve the related sp and practice information from voucher transaction
	CREATE TABLE #tempsppractice
	(
		SP_ID VARCHAR(8) COLLATE Chinese_Taiwan_Stroke_CI_AS, 
		Practice_Seq INT, 
		Professional VARCHAR(10) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	INSERT INTO #tempsppractice
	(SP_ID, Practice_Seq)
	SELECT sp_id, MIN(practice_display_seq) AS practice_seq
	FROM vouchertransaction VT
	WHERE 
	scheme_code = @scheme_code COLLATE Chinese_Taiwan_Stroke_CI_AS AND 
	record_status NOT IN ('I', 'D') AND
	ISNULL(invalidation, '') <> 'I'
	AND (@submission_method = ''
		OR (SourceApp = 'WEB' AND 'WEB-FULL' + isNull(create_by_smartid,'N') IN (SELECT * FROM @SubmissionMethod)) 
		OR SourceApp IN (SELECT * FROM @SubmissionMethod) 
		OR SourceApp + isNull(create_by_smartid,'N') IN (SELECT * FROM @SubmissionMethod))  
	--AND (@create_by_smartid IS NULL OR (@create_by_smartid = 'Y' and create_by_smartid = 'Y') OR (@create_by_smartid = 'N' AND (create_by_smartid = 'N' OR create_by_smartid IS NULL)) )
	AND (@period_from IS NULL OR ((@period_type = 'T' AND 0<=DATEDIFF(day, @period_from, VT.Transaction_Dtm)) OR (@period_type = 'S' AND 0<=DATEDIFF(day, @period_from, VT.Service_Receive_Dtm))) ) 
	AND (@period_to IS NULL OR ((@period_type = 'T' AND 0<=DATEDIFF(day, VT.Transaction_Dtm, @period_to)) OR (@period_type = 'S' AND 0<=DATEDIFF(day, VT.Service_Receive_Dtm, @period_to))) )
	GROUP BY sp_id

	--Calculate the statistics
	UPDATE #tempsppractice
	SET professional = pf.Service_Category_Code
	FROM #tempsppractice tp, practice p, professional pf
	WHERE tp.sp_id = p.sp_id AND tp.practice_seq = p.display_seq AND
	p.sp_id = pf.sp_id AND p.professional_seq = pf.Professional_seq

	SELECT professional AS 'ProfessionCode', sp_id AS 'SPID'
	FROM #tempsppractice


	DROP TABLE #tempsppractice

	SET NOCOUNT OFF

END

GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00004_GetData] TO HCVU
GO
