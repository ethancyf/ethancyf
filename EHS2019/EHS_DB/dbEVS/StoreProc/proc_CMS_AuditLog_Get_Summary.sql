IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_CMS_AuditLog_Get_Summary]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_CMS_AuditLog_Get_Summary]
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
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:		Add input parm [Enquiry_System] to audit corresponding log
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	2 August 2017
-- CR No.:			I-CRE17-003 (Enhance eHA rectification on HCSP to check with eHS(S) and CMS vaccination)
-- Description:		Add function code "020401" for eHA Rectification
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 April 2016
-- CR No.:			I-CRE16-001
-- Description:		Revise the criteria of CMS log monitor
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		15 November 2010
-- Description:		Get HCSP audit log for checking CMS connection
-- =============================================

CREATE PROCEDURE [dbo].[proc_CMS_AuditLog_Get_Summary]
	@Enquiry_System	VARCHAR(10), -- CMS / CIMS
	@Minute_Before	int
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting 
-- =============================================
	DECLARE @Function_Code_Master table (
		Function_Code	char(6)
	)
	
	DECLARE @Log_ID_Master table (
		Log_ID			char(5)
	)

	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020201')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020202')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020401') -- HCSP eHA Rectification
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020801')

	IF @Enquiry_System = 'CMS' 
	BEGIN
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01009')	-- Get CMS Vaccination fail: Invalid parameter
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01010')	-- Get CMS Vaccination fail: Unknown error
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01011')	-- Get CMS Vaccination fail: Communication link error
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01012')	-- Get CMS Vaccination fail: EHS internal error
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01026')	-- Get CMS Vaccination fail: CMS result Message ID mismatch with EHS request Message ID
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01027')	-- Get CMS Vaccination fail: EAI Service Interruption
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01028')	-- Get CMS Vaccination fail: Returned health check result incorrect (Return Code: 100)
	END
	ELSE IF @Enquiry_System = 'CIMS' 
	BEGIN
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01109')	-- Get CIMS Vaccination fail: Invalid parameter
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01110')	-- Get CIMS Vaccination fail: Unknown error
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01111')	-- Get CIMS Vaccination fail: Communication link error
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01112')	-- Get CIMS Vaccination fail: EHS internal error
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01126')	-- Get CIMS Vaccination fail: CIMS result client mismatch with EHS request client
		INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01128')	-- Get CIMS Vaccination fail: Returned health check result incorrect (Return Code: 10001)
	END

	

-- =============================================
-- Declaration 
-- =============================================
	DECLARE @End_Dtm	datetime
	DECLARE @Start_Dtm	datetime
	
	SET @End_Dtm = GETDATE()
	SET @Start_Dtm = DATEADD(mi, -1 * @Minute_Before, @End_Dtm)

	DECLARE @Year		smallint	-- For identifying the audit log table


-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @AuditLog table (
		System_Dtm				datetime,
		Action_Dtm				datetime,
		End_Dtm					datetime,
		SP_ID					char(8),
		Data_Entry				varchar(20),
		Function_Code			char(6),
		Log_ID					char(5),
		Description				nvarchar(MAX),
		Session_ID				varchar(MAX)
	)


-- =============================================
-- Initialization
-- =============================================
	SET @Year = CONVERT(varchar(2), @Start_Dtm, 12)	-- Extract the Calendar Year: "12" gives the format YYMMDD

	EXEC [proc_SymmetricKey_open]


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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP10
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP11
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP12
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP13
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP14
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP15
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP16
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP17
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP18
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP19
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP20
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP21
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP22
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP23
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP24
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP25
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP26
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP27
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
			Session_ID
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
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP28
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND E_Function_Code IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Function_Code) FROM @Function_Code_Master)
				AND E_Log_ID IN (SELECT EncryptByKey(KEY_GUID('sym_Key'), Log_ID) FROM @Log_ID_Master)
				
	END

	EXEC [proc_SymmetricKey_close]


-- =============================================
-- Process data
-- =============================================

-----------------------------------------------------------------------------------------------
-- Delete the log retrieve data from session
-----------------------------------------------------------------------------------------------
	
	DELETE FROM
		@AuditLog
	WHERE
		Description LIKE '%<Return Data: Get from session>%'


-- =============================================
-- Return result
-- =============================================

	SELECT
		COUNT(1) AS [Connection_Fail_Count]
	FROM
		@AuditLog


END
GO

GRANT EXECUTE ON [dbo].[proc_CMS_AuditLog_Get_Summary] TO HCVU
GO
