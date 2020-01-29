IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordMatching_Detail]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordMatching_Detail]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		09 May 2011
-- CR No.:			CRE11-007
-- Description:		View detail function for Death Record Matching
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordMatching_Detail]
	@Account_ID				char(15),
	@Doc_Code				char(20)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @DeathRecordMatchResult table (
		EHA_Acc_ID				char(15),
		EHA_Doc_Code			char(20),
		EHA_Acc_Type			char(1),
		Encrypt_Field1			varbinary(100),
		With_Claim				char(1),
		With_Suspicious_Claim	char(1),
		Match_Dtm				datetime,
		Death_Encrypt_Field2	varbinary(100),
		DOD						datetime,
		Exact_DOD				char(1),
		DOR						datetime
	)
	
	DECLARE @SuspiciousTransaction table (
		Transaction_ID			varchar(20),
		Scheme_Code				varchar(10),
		Record_Status			varchar(1)
	)
	
	DECLARE @EffectiveScheme table (
		Scheme_Code				varchar(10),
		Scheme_Seq				int
	)
	
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @EffectiveScheme (
		Scheme_Code,
		Scheme_Seq
	)
	SELECT
		Scheme_Code,
		MAX(Scheme_Seq)
	FROM
		SchemeClaim
	WHERE
		GETDATE() > Effective_Dtm
	GROUP BY
		Scheme_Code
		

-- =============================================
-- Retrieve data
-- =============================================

-- General Information

	INSERT INTO @DeathRecordMatchResult
	SELECT
		R.EHA_Acc_ID,
		R.EHA_Doc_Code,
		R.EHA_Acc_Type,
		R.Encrypt_Field1,
		R.With_Claim,
		R.With_Suspicious_Claim,
		R.Match_Dtm,
		E.Encrypt_Field2,
		E.DOD,
		E.Exact_DOD,
		E.DOR
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Death_Record_File_ID = E.Death_Record_File_ID
					AND R.Encrypt_Field1 = E.Encrypt_Field1
	WHERE
		R.EHA_Acc_ID = @Account_ID
			AND R.EHA_Doc_Code = @Doc_Code


-- Transaction and Suspicious Transaction

	IF (SELECT EHA_Acc_Type FROM @DeathRecordMatchResult) = 'V' BEGIN
		INSERT INTO @SuspiciousTransaction
		SELECT
			Transaction_ID,
			Scheme_Code,
			Record_Status
		FROM
			VoucherTransaction
		WHERE
			Voucher_Acc_ID = (SELECT EHA_Acc_ID FROM @DeathRecordMatchResult)
				AND Invalid_Acc_ID IS NULL
				AND Service_Receive_Dtm > (SELECT DOD FROM @DeathRecordMatchResult)
				AND Record_Status NOT IN ('I', 'D')
				AND ISNULL(Invalidation, '') <> 'I'
	
	END ELSE IF (SELECT EHA_Acc_Type FROM @DeathRecordMatchResult) = 'T' BEGIN 
		INSERT INTO @SuspiciousTransaction
		SELECT
			Transaction_ID,
			Scheme_Code,
			Record_Status
		FROM
			VoucherTransaction
		WHERE
			Temp_Voucher_Acc_ID = (SELECT EHA_Acc_ID FROM @DeathRecordMatchResult)
				AND Voucher_Acc_ID = ''
				AND Special_Acc_ID IS NULL
				AND Record_Status NOT IN ('I', 'D')
				AND ISNULL(Invalidation, '') <> 'I'
	
	END


-- =============================================
-- Return results
-- =============================================

	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	SELECT
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		CONVERT(varchar, DecryptByKey(Encrypt_Field1)) AS [Death_Document_No],
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		CONVERT(varchar, DecryptByKey(Death_Encrypt_Field2)) AS [Death_English_Name],
		DOD,
		Exact_DOD,
		DOR
	FROM
		@DeathRecordMatchResult

	CLOSE SYMMETRIC KEY sym_Key

--

	SELECT
		ST.Transaction_ID,
		SC.Display_Code,
		SC.Display_Seq,
		ST.Scheme_Code,
		ST.Record_Status
	FROM
		@SuspiciousTransaction ST
			INNER JOIN @EffectiveScheme ES
				ON ST.Scheme_Code = ES.Scheme_Code
			INNER JOIN SchemeClaim SC
				ON ES.Scheme_Code = SC.Scheme_Code
					AND ES.Scheme_Seq = SC.Scheme_Seq
	ORDER BY
		SC.Display_Seq,
		ST.Transaction_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordMatching_Detail] TO HCVU
GO
