IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-Content]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-Content]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-021-02
-- Modified by:		Tommy LAM
-- Modified date:	03 Jan 2014
-- Description:		Change the Sub Report Name of "eHSD0021-05"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	8 May 2013
-- Description:		Add EHAPP Break Down
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	20 March 2012
-- Description:		CRP11-005 - Content worksheet
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-Content]

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
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (2, 'eHS(S)D0021-01', 'Total summary of transactions')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (3, 'eHS(S)D0021-02', 'Summary of transactions input by Service Provider')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (4, 'eHS(S)D0021-03', 'Summary of transactions input by Back Office')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (5, 'eHS(S)D0021-04', 'Summary of claim break down by Scheme')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (6, 'eHS(S)D0021-05', 'Summary of voucher claim break down by period')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (7, 'eHS(S)D0021-06', 'Summary of vaccination claim break down by Vaccination Season')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (8, 'eHS(S)D0021-07', 'Summary of EHAPP claim break down by period')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (30, '', '')	
	
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


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-Content] TO HCVU
GO
