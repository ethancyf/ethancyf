IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027-Content]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027-Content]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Helen Lam
-- Modified date:	27 Jan 2012
-- Description:		eHSA0027 - FHB statistics for 2011 (CRD12-002)
-- =============================================
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027-Content]
@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	Declare @strYear char(4)
	Declare @strValue varchar(400)

	set @strYear =cast(@Year as char(4))


	DECLARE @ResultTable table (
		Result_Seq		smallint,
		Result_Value1	varchar(100) DEFAULT '',
		Result_Value2	varchar(500) DEFAULT ''
	)


-- =============================================
-- Retrieve data
-- =============================================
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (1, 'Sub Report ID', 'Sub Report Name')
	set @strValue='Monthly statistics of validated HCVS-eligible eHealth accounts created in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (11, 'eHSA0027-01', @strValue)
	set @strValue='Validated HCVS-eligible eHealth accounts as at 31 Dec ' + @strYear+', broken down by ages'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (12, 'eHSA0027-02', @strValue)
	set @strValue='Monthly statistics of transactions claimed in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (13, 'eHSA0027-03', @strValue)
	set @strValue='Monthly statistics of vouchers claimed in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (14, 'eHSA0027-04', @strValue)
	set @strValue='Monthly statistics of transactions claimed in ' + @strYear+', broken down by 18 districts'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (15, 'eHSA0027-05',  @strValue)
	set @strValue='Monthly statistics of vouchers claimed in ' + @strYear+', broken down by 18 districts'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (16, 'eHSA0027-06', @strValue)
	set @strValue='Monthly statistics of transactions and vouchers claimed in ' + @strYear+', broken down by professions and reasons of visit'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (17, 'eHSA0027-07', @strValue)
	set @strValue='Cumulative monthly statistics of transactions submitted through Internet (Text-only version) in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (18, 'eHSA0027-08', @strValue)
	set @strValue='Cumulative monthly statistics of transactions submitted through Internet (Full version) in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (19, 'eHSA0027-09',  @strValue)
	set @strValue='Cumulative monthly statistics of transactions submitted through IVRS in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (20, 'eHSA0027-10', @strValue)
	set @strValue='Cumulative monthly statistics of transactions submitted through PCS in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (20, 'eHSA0027-11', @strValue)
	set @strValue='Statistics of EHCPs with claimed vouchers in ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (21, 'eHSA0027-12', @strValue)
	set @strValue='Statistics of EHCPs used Smart ICs for making claims as at 31 Dec ' + @strYear+', broken down by professions'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (22, 'eHSA0027-13', @strValue)
	set @strValue='Statistics of transactions and vouchers of each EHCP in  ' + @strYear
--	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (23, 'eHSA0027-14', @strValue)
--	set @strValue='Statistics of transactions and vouchers of each EHCP in  ' + @strYear +', broken down by means of input'
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES (24, 'eHSA0027-14', @strValue)

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


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027-Content] TO HCVU
GO

