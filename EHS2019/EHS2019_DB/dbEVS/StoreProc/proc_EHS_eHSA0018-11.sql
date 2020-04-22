IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0018-11]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0018-11]
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0018-11]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @SP table (
		SP_ID					char(8),
		Practice_Display_Seq	smallint,
		Service_Type			char(5),
		No_Of_Transaction		int,
		No_Of_Voucher			int
	)
	
	DECLARE @ResultTable table(
		Result_Seq				smallint,
		Result_Value1			varchar(100) DEFAULT '',
		Result_Value2			varchar(30) DEFAULT '',
		Result_Value3			varchar(30) DEFAULT '',
		Result_Value4			varchar(30) DEFAULT '',
		Result_Value5			varchar(30) DEFAULT '',
		Result_Value6			varchar(30) DEFAULT '',
		Result_Value7			varchar(30) DEFAULT '',
		Result_Value8			varchar(30) DEFAULT '',
		Result_Value9			varchar(30) DEFAULT '',
		Result_Value10			varchar(30) DEFAULT '',
		Result_Value11			varchar(30) DEFAULT '',
		Result_Value12			varchar(30) DEFAULT ''
	)

	DECLARE @ENU	int
	DECLARE @RCM	int
	DECLARE @RCP	int
	DECLARE @RDT	int
	DECLARE @RMP	int
	DECLARE @RMT	int
	DECLARE @RNU	int
	DECLARE @ROT	int
	DECLARE @RPT	int
	DECLARE @RRD	int
	DECLARE @Total	int


-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Claimed SP
-- ---------------------------------------------

	INSERT INTO @SP (
		SP_ID,
		Practice_Display_Seq,
		Service_Type,
		No_Of_Transaction,
		No_Of_Voucher
	)
	SELECT DISTINCT
		VT.SP_ID,
		MIN(VT.Practice_Display_Seq) AS [Practice_Display_Seq],
		NULL AS [Service_Type],
		COUNT(1) AS [No_Of_Transaction],
		SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim) AS [No_Of_Voucher]
	FROM
		VoucherTransaction VT
	WHERE
		VT.Scheme_Code = 'HCVS'
			AND YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Record_Status NOT IN ('I', 'D')
			AND ISNULL(VT.Invalidation, '') <> 'I'
	GROUP BY
		VT.SP_ID


-- ---------------------------------------------
-- Patch the Service_Type
-- ---------------------------------------------

	UPDATE
		@SP
	SET
		Service_Type = PR.Service_Category_Code
	FROM
		@SP S
			INNER JOIN Practice P
				ON S.SP_ID = P.SP_ID
					AND S.Practice_Display_Seq = P.Display_Seq
			INNER JOIN Professional PR
				ON P.SP_ID = PR.SP_ID
					AND P.Professional_Seq = PR.Professional_Seq


-- =============================================
-- Build frame
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-11: Statistics of EHCPs with claimed vouchers in 2010, broken down by professions')
	
	INSERT INTO @ResultTable (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(2, 'No. of service providers')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(10, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------
					
	SELECT @ENU = COUNT(1) FROM @SP WHERE Service_Type = 'ENU'
	SELECT @RCM = COUNT(1) FROM @SP WHERE Service_Type = 'RCM' 
	SELECT @RCP = COUNT(1) FROM @SP WHERE Service_Type = 'RCP'
	SELECT @RDT = COUNT(1) FROM @SP WHERE Service_Type = 'RDT' 
	SELECT @RMP = COUNT(1) FROM @SP WHERE Service_Type = 'RMP' 
	SELECT @RMT = COUNT(1) FROM @SP WHERE Service_Type = 'RMT' 
	SELECT @RNU = COUNT(1) FROM @SP WHERE Service_Type = 'RNU' 
	SELECT @ROT = COUNT(1) FROM @SP WHERE Service_Type = 'ROT' 
	SELECT @RPT = COUNT(1) FROM @SP WHERE Service_Type = 'RPT' 
	SELECT @RRD = COUNT(1) FROM @SP WHERE Service_Type = 'RRD' 
	SELECT @Total = COUNT(1) FROM @SP
		
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	SELECT
		11,
		@ENU, 
		@RCM, 
		@RCP, 
		@RDT, 
		@RMP, 
		@RMT, 
		@RNU, 
		@ROT, 
		@RPT, 
		@RRD,
		NULL,
		@Total
		
		
-- =============================================
-- Return result
-- =============================================

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12
	FROM
		@ResultTable
	ORDER BY
		Result_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0018-11] TO HCVU
GO

