IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027-07]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027-07]
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027-07]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	Declare @strYear char(4)
	Declare @cnt int
	set @strYear= cast(@Year as char(4))
	

	DECLARE @Transaction table (
		Transaction_Month		smallint,
		Service_Type			char(5),
		Reason_For_Visit_L1		varchar(50),
		Reason_For_Visit_L2		varchar(50),
		No_Of_Transaction		int,
		No_Of_Voucher			int
	)
	
	DECLARE	@ResultTable table (
		Result_Seq		smallint,
		Result_Value1	varchar(200) DEFAULT '',
		Result_Value2	varchar(30) DEFAULT '',
		Result_Value3	varchar(100) DEFAULT '',
		Result_Value4	varchar(100) DEFAULT '',
		Result_Value5	varchar(30) DEFAULT '',
		Result_Value6	varchar(30) DEFAULT ''
	)
		

-- =============================================
-- Retrieve data
-- =============================================

	INSERT INTO @Transaction (
		Transaction_Month,
		Service_Type,
		Reason_For_Visit_L1,
		Reason_For_Visit_L2,
		No_Of_Transaction,
		No_Of_Voucher
	)
	SELECT
		MONTH(VT.Transaction_Dtm) AS [Transaction_Month],
		VT.Service_Type,
		TAF1.AdditionalFieldValueCode AS [Reason_For_Visit_L1],
		TAF2.AdditionalFieldValueCode AS [Reason_For_Visit_L2],
		COUNT(1) AS [No_Of_Transaction],
		SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim) AS [No_Of_Voucher]
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionAdditionalField TAF1
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			INNER JOIN TransactionAdditionalField TAF2
				ON VT.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'Reason_for_Visit_L2'
	WHERE
		VT.Scheme_Code = 'HCVS'
			AND YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Record_Status NOT IN ('I', 'D')
			AND ISNULL(VT.Invalidation, '') <> 'I'
	GROUP BY
		MONTH(VT.Transaction_Dtm),
		VT.Service_Type,
		TAF1.AdditionalFieldValueCode,
		TAF2.AdditionalFieldValueCode


-- =============================================
-- Build frame
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0027-07: Monthly statistics of transactions and vouchers claimed in ' +@strYear+', broken down by professions and reasons of visit')
	
	INSERT INTO @ResultTable (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	(10, 'Period', 'Profession', 'Level 1 reason of visit', 'Level 2 reason of visit', 'No. of transactions', 'No. of vouchers')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6)
	SELECT
		10 + ROW_NUMBER() OVER(ORDER BY T.Transaction_Month, T.Service_Type, T.Reason_For_Visit_L1, T.Reason_For_Visit_L2),
		CONVERT(varchar, @Year) + '-' + RIGHT(CONVERT(varchar, '0') + CONVERT(varchar, T.Transaction_Month), 2) + '-01',
		T.Service_Type,
		RL1.Reason_L1,
		RL2.Reason_L2,
		T.No_Of_Transaction,
		T.No_Of_Voucher
	FROM
		@Transaction T
			INNER JOIN ReasonForVisitL1 RL1
				ON T.Service_Type = RL1.Professional_Code
					AND T.Reason_For_Visit_L1 = RL1.Reason_L1_Code
			INNER JOIN ReasonForVisitL2 RL2
				ON T.Service_Type = RL2.Professional_Code
					AND T.Reason_For_Visit_L1 = RL2.Reason_L1_Code
					AND T.Reason_For_Visit_L2 = RL2.Reason_L2_Code
	ORDER BY
		T.Transaction_Month,
		T.Service_Type,
		T.Reason_For_Visit_L1,
		T.Reason_For_Visit_L2


	set @cnt=0
	select @cnt=  max(Result_Seq) from @ResultTable
	set @cnt=@cnt +20
	INSERT INTO @ResultTable (Result_Seq) VALUES
	(@cnt)
	set @cnt=@cnt +1
	INSERT INTO @ResultTable (Result_Seq) VALUES
	(@cnt)
	set @cnt=@cnt +1
	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(@cnt, 'Note:')
	set @cnt=@cnt +1
	
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1)
	SELECT
		@cnt,
		'There are ' + CONVERT(varchar, COUNT(1)) +' transactions ( ' +convert(varchar,SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim)) +' vouchers) with defer input on reason for visit.' 

	FROM
		VoucherTransaction VT
			
	WHERE
		VT.Scheme_Code = 'HCVS'
			AND YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Record_Status='U'			
			AND ISNULL(VT.Invalidation, '') <> 'I'
-- =============================================
-- Return results
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

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027-07] TO HCVU
GO

