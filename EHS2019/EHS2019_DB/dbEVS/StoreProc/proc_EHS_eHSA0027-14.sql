IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027-14]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027-14]
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027-14]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
Declare @strYear char(4)
	Declare @seq int
	set @strYear= cast(@Year as char(4))

	DECLARE @SP table (
		SP_ID					char(8),
		Service_Type			char(5),
		Scheme_Effective_Dtm	datetime,
		No_Of_Transaction		int,
		No_Of_Voucher			int,
		Record_Status_D			varchar(100)
	)
	
	DECLARE @ResultTable table(
		Result_Seq				smallint,
		Result_Value1			varchar(100) DEFAULT '',
		Result_Value2			varchar(40) DEFAULT '',
		Result_Value3			varchar(30) DEFAULT '',
		Result_Value4			varchar(30) DEFAULT '',
		Result_Value5			varchar(30) DEFAULT '',
		Result_Value6			varchar(30) DEFAULT ''
	)


-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Claimed SP
-- ---------------------------------------------

	INSERT INTO @SP (
		SP_ID,
		Service_Type,
		Scheme_Effective_Dtm,
		No_Of_Transaction,
		No_Of_Voucher,
		Record_Status_D
	)
	SELECT DISTINCT
		VT.SP_ID,
		VT.Service_Type,
		NULL AS [Scheme_Effective_Dtm],
		COUNT(1) AS [No_Of_Transaction],
		SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim) AS [No_Of_Voucher],
		SD.Status_Description AS [Record_Status_D]
	FROM
		VoucherTransaction VT
			INNER JOIN ServiceProvider SP	
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN StatusData SD
				ON SP.Record_Status = SD.Status_Value
					AND SD.Enum_Class = 'ServiceProviderStatus'
	WHERE
		VT.Scheme_Code = 'HCVS'
			AND YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Record_Status NOT IN ('I', 'D')
			AND ISNULL(VT.Invalidation, '') <> 'I'
	GROUP BY
		VT.SP_ID,
		VT.Service_Type,
		SD.Status_Description 


-- ---------------------------------------------
-- Patch the Scheme_Effective_Dtm
-- ---------------------------------------------

	UPDATE
		@SP
	SET
		Scheme_Effective_Dtm = SI.Effective_Dtm
	FROM
		@SP S
			INNER JOIN SchemeInformation SI
				ON S.SP_ID = SI.SP_ID
					AND SI.Scheme_Code = 'HCVS'


-- =============================================
-- Build frame
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0027-14: Statistics of transactions and vouchers of EHCP in '+@strYear)
	
	INSERT INTO @ResultTable (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	(10, 'SPID', 'HCVS enrolment effective date', 'Profession', 'Status', 'No. of transactions', 'No. of vouchers')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6)
	SELECT
		10 + ROW_NUMBER() OVER(ORDER BY SP_ID, Service_Type),
		SP_ID,
		CONVERT(varchar, Scheme_Effective_Dtm, 20) AS [Scheme_Effective_Dtm],
		Service_Type,
		Record_Status_D,
		No_Of_Transaction,
		No_Of_Voucher
	FROM
		@SP
	ORDER BY
		SP_ID,
		Service_Type


-- =============================================
-- Return result
-- =============================================

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6
	FROM
		@ResultTable
	ORDER BY
		Result_Seq




set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027-14] TO HCVU
GO

