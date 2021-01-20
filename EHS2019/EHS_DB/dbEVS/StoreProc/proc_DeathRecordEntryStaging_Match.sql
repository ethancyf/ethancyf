IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntryStaging_Match]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntryStaging_Match]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 May 2011
-- CR No.:			CRE11-007
-- Description:		Process DeathRecordEntryStaging:
--						1. Check the duplicate records
--						2. Copy the entry in DeathRecordEntryStaging to DeathRecordEntry
--						3. Match the entry in DeathRecordEntry with eHealth accounts (in another SProc)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	28 June 2011
-- CR No.:			CRE11-007
-- Description:		Fine tune check duplicate death record entry
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntryStaging_Match]
	@Death_Record_File_ID	char(15)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @C_Seq_No					int
	DECLARE @C_Encrypt_Field1			varbinary(100)
	DECLARE @C_DOD						datetime
	DECLARE @C_Exact_DOD				char(1)
	DECLARE @C_DOR						datetime
	DECLARE @C_Encrypt_Field2			varbinary(100)
	
	DECLARE @Duplicate_Record			int
	DECLARE @Fail_Record				int
	
	DECLARE @Encrypt_Field1_Exception	varbinary(100)


-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @Fail_Record = 0

	EXEC [proc_SymmetricKey_open]
	
	SET @Encrypt_Field1_Exception = EncryptByKey(KEY_GUID('sym_Key'), 'XXXXXXXXX')
	
	EXEC [proc_SymmetricKey_close]
	
	
-- =============================================
-- Process
-- =============================================

-- ---------------------------------------------
-- 1. Check duplicate records
-- ---------------------------------------------
	UPDATE DeathRecordEntryStaging
	SET Result = 'F',
		Fail_Type = 'D'
	WHERE Death_Record_File_ID = @Death_Record_File_ID 
			AND EXISTS 
				(SELECT S.Encrypt_Field1, COUNT(1) AS [Total]
					FROM
						(SELECT Encrypt_Field1 
						FROM DeathRecordEntryStaging T
						WHERE T.Death_Record_File_ID = @Death_Record_File_ID 
								AND T.Encrypt_Field1 <> @Encrypt_Field1_Exception
								AND T.Encrypt_Field1 = DeathRecordEntryStaging.Encrypt_Field1) S
						INNER JOIN
						(SELECT Encrypt_Field1 
						FROM DeathRecordEntry T
						WHERE T.Record_Status = 'A'
							  AND T.Encrypt_Field1 = DeathRecordEntryStaging.Encrypt_Field1) P
						ON S.Encrypt_Field1 = P.Encrypt_Field1
					GROUP BY S.Encrypt_Field1
					HAVING COUNT(1) > 0)


-- ---------------------------------------------
-- 2. If any one of the records are duplicate, update the DeathRecordFileHeader to status fail and exit
-- ---------------------------------------------
	
	IF @@ROWCOUNT <> 0 BEGIN
		UPDATE
			DeathRecordFileHeader
		SET
			Record_Status = 'F'
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
	
		RETURN -1
	END


-- ---------------------------------------------
-- 3. If all the records are okay, copy the records from DeathRecordEntryStaging to DeathRecordEntry (exclude the records without HKID)
-- ---------------------------------------------

	INSERT INTO DeathRecordEntry (
		Encrypt_Field1,
		Death_Record_File_ID,
		DOD,
		Exact_DOD,
		DOR,
		Encrypt_Field2,
		Record_Status,
		Effective_Dtm
	)
	SELECT
		Encrypt_Field1,
		Death_Record_File_ID,
		DOD,
		Exact_DOD,
		DOR,
		Encrypt_Field2,
		'A' AS [Record_Status],
		GETDATE() AS [Effective_Dtm]
	FROM
		DeathRecordEntryStaging
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID
			AND Encrypt_Field1 <> @Encrypt_Field1_Exception


-- ---------------------------------------------
-- 4. Update the DeathRecordFileHeader status to "Processing File" (M)
-- ---------------------------------------------
	
	UPDATE
		DeathRecordFileHeader
	SET
		Processing = 'E'
	WHERE
		Death_Record_File_ID = @Death_Record_File_ID

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntryStaging_Match] TO HCVU
GO
