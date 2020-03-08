IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntryStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntryStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 May 2011
-- CR No.:			CRE11-007
-- Description:		Add DeathRecordEntryStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntryStaging_add]
	@Death_Record_File_ID	char(15),
	@Seq_No					int,
	@HKID					varchar(40),
	@DOD					datetime,
	@Exact_DOD				char(1),
	@DOR					datetime,
	@English_Name			varchar(40)
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
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

-- =============================================
-- Return results
-- =============================================
	
	INSERT INTO DeathRecordEntryStaging (
		Death_Record_File_ID,
		Seq_No,
		Encrypt_Field1,
		DOD,
		Exact_DOD,
		DOR,
		Encrypt_Field2,
		Result,
		Fail_Type
	) VALUES (
		@Death_Record_File_ID,
		@Seq_No,
		EncryptByKey(KEY_GUID('sym_Key'), @HKID),
		@DOD,
		@Exact_DOD,
		@DOR,
		EncryptByKey(KEY_GUID('sym_Key'), @English_Name),
		'S',
		NULL
	)
	

-- =============================================
-- Finalizer
-- =============================================

	CLOSE SYMMETRIC KEY sym_Key


END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntryStaging_add] TO HCVU
GO