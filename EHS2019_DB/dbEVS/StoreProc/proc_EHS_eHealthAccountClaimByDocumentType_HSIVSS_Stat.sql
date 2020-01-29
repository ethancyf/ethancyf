IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHealthAccountClaimByDocumentType_HSIVSS_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_HSIVSS_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		21 December 2009
-- Description:		Generate report for the Vaccination Claim Report (Scheme HSIVSS)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_HSIVSS_Stat] 
	@Cutoff_Dtm	datetime
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @HSIVSSTransaction table (
		Voucher_Acc_ID		char(15),
		Temp_Voucher_Acc_ID	char(15),
		Special_Acc_ID		char(15),
		Invalid_Acc_ID		char(15),
		Doc_Code			char(20)
	)
	
	DECLARE @Account table (
		Doc_Code			char(20),
		Encrypt_Field1		varbinary(100)
	)
	
	DECLARE @ResultTable table(
		Result_Seq			smallint,
		Result_Value1		varchar(100),
		Result_Value2		varchar(100),
		Result_Value3		varchar(100),
		Result_Value4		varchar(100),
		Result_Value5		varchar(100),
		Result_Value6		varchar(100),
		Result_Value7		varchar(100),
		Result_Value8		varchar(100),
		Result_Value9		varchar(100),
		Result_Value10		varchar(100),
		Result_Value11		varchar(100)
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

-- ---------------------------------------------
-- HSIVSS transactions
-- ---------------------------------------------

	INSERT INTO @HSIVSSTransaction (
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code
	)
	SELECT
		ISNULL(Voucher_Acc_ID, ''),
		ISNULL(Temp_Voucher_Acc_ID, ''),
		ISNULL(Special_Acc_ID, ''),
		ISNULL(Invalid_Acc_ID, ''),
		Doc_Code
	FROM
		VoucherTransaction
	WHERE
		Scheme_Code = 'HSIVSS'
			AND Transaction_Dtm <= @Cutoff_Dtm
			AND Record_Status <> 'I'


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Doc_Code,
		Encrypt_Field1
	)
	SELECT
		VP.Doc_Code,
		VP.Encrypt_Field1
	FROM
		@HSIVSSTransaction VT
			INNER JOIN PersonalInformation VP
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
					AND VT.Doc_Code = VP.Doc_Code
	WHERE
		VT.Voucher_Acc_ID <> ''


-- ---------------------------------------------
-- Temporary accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Doc_Code,
		Encrypt_Field1
	)
	SELECT
		TP.Doc_Code,
		TP.Encrypt_Field1
	FROM
		@HSIVSSTransaction VT
			INNER JOIN TempPersonalInformation TP
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Temp_Voucher_Acc_ID <> ''
			AND VT.Special_Acc_ID = ''


-- ---------------------------------------------
-- Special accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Doc_Code,
		Encrypt_Field1
	)
	SELECT
		SP.Doc_Code,
		SP.Encrypt_Field1
	FROM
		@HSIVSSTransaction VT
			INNER JOIN SpecialPersonalInformation SP
				ON VT.Special_Acc_ID = SP.Special_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Special_Acc_ID <> ''
			AND VT.Invalid_Acc_ID = ''

	
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Build format and result
-- ---------------------------------------------
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(0, 'eHealth Accounts with HSIVSS claim transactions (as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111) + ')', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(1, 'By document type', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(2, '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(3, 'HSIVSS', '', '', '', '', '', '', 'Total', '', 'HSIVSS**', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(10, 'HKIC/HKBC**', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', '', '', 'HKIC', 'HKBC')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(11, '', '', '', '', '', '', '', '', '', '', '')
	
	
	UPDATE
		@ResultTable
	SET
		Result_Value1 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'HKIC'
					OR Doc_Code = 'HKBC'
			)
	WHERE
		Result_Seq = 11
			
	UPDATE
		@ResultTable
	SET
		Result_Value2 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'Doc/I'
			)
	WHERE
		Result_Seq = 11

	UPDATE
		@ResultTable
	SET
		Result_Value3 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'REPMT'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value4 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'ID235B'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value5 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'VISA'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value6 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'ADOPC'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value7 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'EC'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value8 =
			(
			CONVERT(int, Result_Value1)
			+
			CONVERT(int, Result_Value2)
			+
			CONVERT(int, Result_Value3)
			+
			CONVERT(int, Result_Value4)
			+
			CONVERT(int, Result_Value5)
			+
			CONVERT(int, Result_Value6)
			+
			CONVERT(int, Result_Value7)
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value10 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'HKIC'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value11 =
			(
			SELECT
				COUNT(DISTINCT Encrypt_Field1)
			FROM
				@Account
			WHERE 
				Doc_Code = 'HKBC'
			)
	WHERE
		Result_Seq = 11
		

-- ---------------------------------------------
-- Insert to statistics table
-- ---------------------------------------------
	
	DELETE FROM _eHealthAccountByDocumentType_HSIVSS_Stat

	INSERT INTO _eHealthAccountByDocumentType_HSIVSS_Stat (
		Result_Seq,
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
		Result_Value11
	) 
	SELECT
		Result_Seq,
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
		Result_Value11
	FROM
		@ResultTable
	ORDER BY
		Result_Seq

		
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_HSIVSS_Stat] TO HCVU
GO
