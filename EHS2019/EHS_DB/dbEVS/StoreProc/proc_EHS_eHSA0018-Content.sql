IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0018-Content]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0018-Content]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 February 2011
-- Description:		eHSA0018 - FHB statistics for 2010
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0018-Content]
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @ResultTable table (
		Result_Seq		smallint,
		Result_Value1	varchar(100) DEFAULT '',
		Result_Value2	varchar(500) DEFAULT ''
	)


-- =============================================
-- Retrieve data
-- =============================================
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (1, 'Sub Report ID', 'Sub Report Name')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (11, 'eHSA0018-01', 'Monthly statistics of validated HCVS-eligible eHealth accounts created in 2010, broken down by professions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (12, 'eHSA0018-02', 'Validated HCVS-eligible eHealth accounts as at 31 Dec 2010, broken down by ages')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (13, 'eHSA0018-03', 'Monthly statistics of transactions claimed in 2010, broken down by professions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (14, 'eHSA0018-04', 'Monthly statistics of vouchers claimed in 2010, broken down by professions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (15, 'eHSA0018-05', 'Monthly statistics of transactions claimed in 2010, broken down by 18 districts')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (16, 'eHSA0018-06', 'Monthly statistics of vouchers claimed in 2010, broken down by 18 districts')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (17, 'eHSA0018-07', 'Monthly statistics of transactions and vouchers claimed in 2010, broken down by professions and reasons of visit')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (18, 'eHSA0018-08', 'Cumulative monthly statistics of transactions submitted through Internet (Full version) in 2010, broken down by professions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (19, 'eHSA0018-09', 'Cumulative monthly statistics of transactions submitted through Internet (Text-only version) in 2010, broken down by professions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (20, 'eHSA0018-10', 'Cumulative monthly statistics of transactions submitted through IVRS in 2010, broken down by professions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (21, 'eHSA0018-11', 'Statistics of EHCPs with claimed vouchers in 2010, broken down by professions ')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (22, 'eHSA0018-12', 'Statistics of transactions and vouchers of EHCP in 2010')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (30, '', '')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (31, '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (40, 'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114), '')


-- =============================================
-- Return result
-- =============================================

	SELECT 
		Result_Value1,
		Result_Value2
	FROM
		@ResultTable
	ORDER BY
		Result_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0018-Content] TO HCVU
GO

