IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceLog_GetTimeDiff_ByDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceLog_GetTimeDiff_ByDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:		Add input parm [Request_System]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Get Interface log for checking CMS connection
-- =============================================

CREATE PROCEDURE [dbo].[proc_InterfaceLog_GetTimeDiff_ByDtm]
	@Function_Code	char(6),
	@Log_ID			char(5),
	@Start_Dtm		datetime,
	@End_Dtm		datetime,
	@Request_System	VARCHAR(10)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Master table
-- =============================================
	DECLARE @Function_Code_Master table (
		Function_Code	char(6)
	)
	
	DECLARE @Log_ID_Master table (
		Log_ID			char(5)
	)

	INSERT INTO @Function_Code_Master (Function_Code) VALUES (@Function_Code)
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES (@Log_ID)
	
	
-- =============================================
-- Declaration 
-- =============================================
	DECLARE @Year		smallint	-- For identifying the audit log table
	

-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @AuditLog table (
		System_Dtm		datetime,
		Action_Dtm		datetime,
		End_Dtm			datetime,
		Time_Diff		int,
		SP_ID			char(8),
		Data_Entry		varchar(20),
		Function_Code	char(6),
		Log_ID			char(5),
		Description		nvarchar(MAX),
		Action_Key		varchar(MAX)
	)
	

-- =============================================
-- Initialization
-- =============================================
	SET @Year = CONVERT(varchar(2), @Start_Dtm, 12)	-- Extract the Calendar Year: "12" gives the format YYMMDD

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key


-- =============================================
-- Retrieve data
-- =============================================	
	IF @Year = '10' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface10
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '11' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface11
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
	
	END ELSE IF @Year = '12' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface12
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '13' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface13
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '14' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface14
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '15' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface15
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '16' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface16
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '17' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface17
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '18' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface18
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '19' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface19
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '20' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface20
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '21' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface21
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '22' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface22
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '23' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface23
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '24' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface24
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '25' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface25
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '26' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface26
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '27' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface27
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END ELSE IF @Year = '28' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			Action_Dtm,
			End_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Action_Key
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_End_Dtm)),
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Action_Key))
		FROM
			AuditLogInterface28
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END

	CLOSE SYMMETRIC KEY sym_Key


-- =============================================
-- Process data
-- =============================================
	
	UPDATE
		@AuditLog
	SET
		Time_Diff = DATEDIFF(ms, Action_Dtm, End_Dtm)


-- =============================================
-- Return result
-- =============================================

	SELECT
		System_Dtm,
		Action_Dtm,
		End_Dtm,
		Time_Diff,
		SP_ID,
		Data_Entry,
		Function_Code,
		Log_ID,
		Description,
		Action_Key
	FROM
		@AuditLog
	WHERE
		ISNULL(@Request_System,'') = '' 
		OR Description LIKE '%RequestSystem: ' + RTRIM(LTRIM(@Request_System)) + '%' 
	ORDER BY
		System_Dtm DESC


END
GO

GRANT EXECUTE ON [dbo].[proc_InterfaceLog_GetTimeDiff_ByDtm] TO WSINT
GO
