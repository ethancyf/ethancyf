IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntry_Match]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntry_Match]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		14 June 2011
-- CR No.:			CRE11-007
-- Description:		Process DeathRecordEntry to match with current eHealth accounts
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntry_Match]
	@Death_Record_File_ID	char(15)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Process
-- =============================================

-- ---------------------------------------------
-- Match with validated account
-- ---------------------------------------------
	
	INSERT INTO DeathRecordMatchResult (
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		Encrypt_Field1,
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		Match_By,
		Death_Record_File_ID
	)
	SELECT
		VP.Voucher_Acc_ID AS [EHA_Acc_ID],
		VP.Doc_Code AS [EHA_Doc_Code],
		'V' AS [EHA_Acc_Type],
		DR.Encrypt_Field1,
		'N' AS [With_Claim],
		'N' AS [With_Suspicious_Claim],
		GETDATE() AS [Match_Dtm],
		'eHS' AS [Match_By],
		DR.Death_Record_File_ID
	FROM
		DeathRecordEntry DR
			INNER JOIN PersonalInformation VP
				ON DR.Encrypt_Field1 = VP.Encrypt_Field1
					AND DR.Death_Record_File_ID = @Death_Record_File_ID
					AND VP.Doc_Code IN (SELECT Doc_Code FROM DocType WHERE Death_Record_Available = 'Y')
			INNER JOIN VoucherAccount VA
				ON VP.Voucher_Acc_ID = VA.Voucher_Acc_ID
					AND VA.Record_Status NOT IN ('D')
	
		
-- ---------------------------------------------
-- Match with temporary account
-- ---------------------------------------------
	
	INSERT INTO DeathRecordMatchResult (
		EHA_Acc_ID,
		EHA_Doc_Code,
		EHA_Acc_Type,
		Encrypt_Field1,
		With_Claim,
		With_Suspicious_Claim,
		Match_Dtm,
		Match_By,
		Death_Record_File_ID
	)
	SELECT
		TP.Voucher_Acc_ID AS [EHA_Acc_ID],
		TP.Doc_Code AS [EHA_Doc_Code],
		'T' AS [EHA_Acc_Type],
		DR.Encrypt_Field1,
		'N' AS [With_Claim],
		'N' AS [With_Suspicious_Claim],
		GETDATE() AS [Match_Dtm],
		'eHS' AS [Match_By],
		DR.Death_Record_File_ID
	FROM
		DeathRecordEntry DR
			INNER JOIN TempPersonalInformation TP
				ON DR.Encrypt_Field1 = TP.Encrypt_Field1
					AND DR.Death_Record_File_ID = @Death_Record_File_ID
					AND TP.Doc_Code IN (SELECT Doc_Code FROM DocType WHERE Death_Record_Available = 'Y')
			INNER JOIN TempVoucherAccount TA
				ON TP.Voucher_Acc_ID = TA.Voucher_Acc_ID
					AND TA.Record_Status NOT IN ('V', 'D')
					AND TA.Account_Purpose IN ('C', 'V')


-- ---------------------------------------------
-- Update the [With_Claim] and [With_Suspicious_Claim]
-- ---------------------------------------------

-- =============================================
-- Declaration
-- =============================================
	DECLARE MyCursor CURSOR FOR
	SELECT
		R.EHA_Acc_ID,
		R.EHA_Doc_Code,
		R.EHA_Acc_Type,
		E.DOD
	FROM
		DeathRecordMatchResult R
			INNER JOIN DeathRecordEntry E
				ON R.Encrypt_Field1 = E.Encrypt_Field1
					AND R.Death_Record_File_ID = E.Death_Record_File_ID
	WHERE
		R.Death_Record_File_ID = @Death_Record_File_ID

--

	DECLARE @C_EHA_Acc_ID		char(15)
	DECLARE @C_EHA_Doc_Code		char(20)
	DECLARE @C_EHA_Acc_Type		char(1)
	DECLARE @C_DOD				datetime
	

-- =============================================
-- Process
-- =============================================
	OPEN MyCursor
	FETCH NEXT FROM MyCursor INTO @C_EHA_Acc_ID, @C_EHA_Doc_Code, @C_EHA_Acc_Type, @C_DOD
	
	WHILE @@FETCH_STATUS = 0 BEGIN
		EXEC [proc_DeathRecordMatchResult_Recheck] @C_EHA_Acc_ID, @C_EHA_Doc_Code, 'eHS', @C_EHA_Acc_Type, @C_DOD
		
		FETCH NEXT FROM MyCursor INTO @C_EHA_Acc_ID, @C_EHA_Doc_Code, @C_EHA_Acc_Type, @C_DOD
	
	END
	
	CLOSE MyCursor
	DEALLOCATE MyCursor

-- ---------------------------------------------
-- Update the DeathRecordFileHeader status to "Import Success" (S) and match time
-- ---------------------------------------------

	UPDATE
		DeathRecordFileHeader
	SET
		Record_Status = 'S',
		Match_Dtm = GETDATE()
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID


	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_Match] TO HCVU
GO
