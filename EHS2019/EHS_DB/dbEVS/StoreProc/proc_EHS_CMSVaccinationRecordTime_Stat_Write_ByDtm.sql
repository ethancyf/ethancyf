IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_CMSVaccinationRecordTime_Stat_Write_ByDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordTime_Stat_Write_ByDtm]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	15 November 2010
-- Description:		Count time for enquiring CMS only, exclude overhead on XML
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 November 2010
-- Description:		Statistics for CMS Vaccination Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordTime_Stat_Write_ByDtm]
	@Start_Dtm	datetime,
	@End_Dtm	datetime
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
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020801')
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01003')	-- [A-End]		Get CMS Vaccination fail: CMS vaccination record unavailable for current Doc Code
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01004')	-- [A-End]		Get CMS Vaccination complete
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01005')	-- [A-End]		Get CMS Vaccination complete: No record found
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01007')	-- [A-End]		Get CMS Vaccination fail: Patient not found
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01008')	-- [A-End]		Get CMS Vaccination fail: Patient not match
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01011')	-- [A-End]		Get CMS Vaccination fail: Communication link error (Connection fail)
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01013')	-- [A-End]		Get CMS Vaccination fail: Vaccination Record service is turned off in EHS
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01014')	-- [B-Start]	Invoke CMS Web service: Start
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01015')	-- [B-End]		Invoke CMS Web service: End
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('01016')	-- [B-End]		Invoke CMS Web service: Exception

	
-- =============================================
-- Declaration 
-- =============================================
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

	DECLARE @TimeResult table (
		Function_Code			char(6),
		Start_Dtm				datetime,
		End_Dtm					datetime,
		Compare_Time			int,
		Session_ID				varchar(MAX)
	)
	
	DECLARE @ResultTable table (
		Result_Function			varchar(30),
		Result_Log_ID			char(5),
		Result_Title			varchar(30),
		Result_Value1			varchar(30),
		Result_Value2			varchar(30),
		Result_Value3			varchar(30)
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
		Log_ID IN ('01003', '01004', '01005', '01007', '01008', '01011', '01013')
			AND Description LIKE '%<Return Data: Get from session>%'


-----------------------------------------------------------------------------------------------
-- Clear up Audit Log (ensure the audit log comes with a pair)
-----------------------------------------------------------------------------------------------

	DECLARE @IsEnd char(1)

	DECLARE @SP_ID				char(8)
	DECLARE @Data_Entry			varchar(20)
	DECLARE @Log_ID				char(5)
	DECLARE @Session_ID			varchar(MAX)
	DECLARE @System_Dtm			datetime
	DECLARE @Function_Code		char(6)
	
	DECLARE @Temp_Function_Code	char(6)
	
	SELECT @IsEnd = 'N'

	DECLARE My_Cursor CURSOR FOR
		SELECT
			Function_Code,
			System_Dtm,
			SP_ID,
			Data_Entry,
			Log_ID,
			Session_ID
		FROM
			@AuditLog
		ORDER BY
			Function_Code,
			SP_ID,
			Data_Entry,
			Session_ID,
			System_Dtm DESC

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	
	WHILE @@Fetch_Status = 0 BEGIN

		IF @Temp_Function_Code <> @Function_Code BEGIN
			SELECT @Temp_Function_Code = NULL
		END
		
		IF @Log_ID IN ('01015', '01016') BEGIN
			SELECT @IsEnd = 'Y'
			SELECT @Temp_Function_Code = @Function_Code
			
		END ELSE IF @Log_ID IN ('01014') BEGIN
			IF @IsEnd = 'Y' BEGIN
				SELECT @IsEnd = 'N'
				
			END ELSE BEGIN
				DELETE FROM
					@AuditLog
				WHERE
					System_Dtm = @System_Dtm
						AND Log_ID = @Log_ID
						AND (Function_Code = @Temp_Function_Code OR @Temp_Function_Code IS NULL)
				
			END
		END		

		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	END

	CLOSE My_Cursor
	DEALLOCATE My_Cursor


-----------------------------------------------------------------------------------------------
-- Prepare time info
-----------------------------------------------------------------------------------------------

	DECLARE @Start_SP_ID			char(8)
	DECLARE @Start_Data_Entry		varchar(20)
	DECLARE @Start_Session_ID		varchar(MAX)
	DECLARE @Start_System_Dtm		datetime
	DECLARE @Start_Function_Code	char(6)
	
	SELECT @Start_SP_ID = NULL
	SELECT @Start_Data_Entry = NULL
	SELECT @Start_System_Dtm = NULL
	SELECT @Start_Session_ID = NULL
	SELECT @Start_Function_Code = NULL
	
	DECLARE My_Cursor CURSOR FOR
		SELECT
			Function_Code,
			System_Dtm,
			SP_ID,
			Data_Entry,
			Log_ID,
			Session_ID
		FROM
			@AuditLog
		ORDER BY
			Function_Code,
			SP_ID,
			Data_Entry,
			Session_ID,
			System_Dtm

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID IN ('01014') BEGIN
			SELECT @Start_SP_ID = @SP_ID
			SELECT @Start_Data_Entry = @Data_Entry
			SELECT @Start_System_Dtm = @System_Dtm
			SELECT @Start_Session_ID = @Session_ID
			SELECT @Start_Function_Code = @Function_Code
		
		END ELSE IF @Log_ID IN ('01015', '01016') BEGIN
			IF @Start_System_Dtm IS NOT NULL
				AND @Start_Session_ID IS NOT NULL AND @Start_Session_ID = @Session_ID
				AND @Start_Function_Code = @Function_Code
				AND @Start_SP_ID = @SP_ID
				AND ISNULL(@Start_Data_Entry, '') = ISNULL(@Data_Entry, '') BEGIN
				
				INSERT INTO @TimeResult (
					Function_Code,
					Start_Dtm,
					End_Dtm,
					Compare_Time,
					Session_ID
				)
				SELECT
					@Function_Code,
					@Start_System_Dtm,
					@System_Dtm, 
					DATEDIFF(ms, @Start_System_Dtm, @System_Dtm),
					@Session_ID
					
				SELECT @Start_System_Dtm = NULL
				
			END
		END
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	END

	CLOSE My_Cursor
	DEALLOCATE My_Cursor
	

-- =============================================
-- Retrieve data 
-- =============================================

-- ---------------------------------------------
-- Count Claim Enquire
-- ---------------------------------------------

-- 01003: DocumentNotAccept

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01003', 'DocumentNotAccept')

-- 01004: FullMatchWithRecord

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01004', 'FullMatchWithRecord')

-- 01005: FullMatchNoRecord

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01005', 'FullMatchNoRecord')

-- 01007: PatientNotFound

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01007', 'PatientNotFound')

-- 01008: DemographicsNotMatch

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01008', 'DemographicsNotMatch')

-- 01011: ConnectionFail

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01011', 'ConnectionFail')

-- 01013: Suspend

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('ClaimEnquire', '01013', 'Suspend')

-- Retrieve count

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
			SELECT
				COUNT(1)
			FROM
				@AuditLog
			WHERE
				Function_Code IN ('020201', '020202')
					AND Log_ID = Result_Log_ID
		)
	WHERE
		Result_Function = 'ClaimEnquire'

-- Retrieve total

	INSERT INTO @ResultTable (Result_Function, Result_Title, Result_Value1)
	SELECT
		'ClaimEnquire' AS [Result_Function],
		'All' AS [Result_Title],
		SUM(CONVERT(int, Result_Value1)) AS [Result_Value1]
	FROM
		@ResultTable
	WHERE
		Result_Function = 'ClaimEnquire'
		

-- ---------------------------------------------
-- Count VRE Enquire
-- ---------------------------------------------

-- 01003: DocumentNotAccept

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01003', 'DocumentNotAccept')

-- 01004: FullMatchWithRecord

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01004', 'FullMatchWithRecord')

-- 01005: FullMatchNoRecord

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01005', 'FullMatchNoRecord')

-- 01007: PatientNotFound

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01007', 'PatientNotFound')

-- 01008: DemographicsNotMatch

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01008', 'DemographicsNotMatch')

-- 01011: ConnectionFail

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01011', 'ConnectionFail')

-- 01013: Suspend

	INSERT INTO @ResultTable (Result_Function, Result_Log_ID, Result_Title) VALUES ('VREEnquire', '01013', 'Suspend')

-- Retrieve count

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
			SELECT
				COUNT(1)
			FROM
				@AuditLog
			WHERE
				Function_Code IN ('020801')
					AND Log_ID = Result_Log_ID
		)
	WHERE
		Result_Function = 'VREEnquire'

-- Retrieve total

	INSERT INTO @ResultTable (Result_Function, Result_Title, Result_Value1)
	SELECT
		'VREEnquire' AS [Result_Function],
		'All' AS [Result_Title],
		SUM(CONVERT(int, Result_Value1)) AS [Result_Value1]
	FROM
		@ResultTable
	WHERE
		Result_Function = 'VREEnquire'

-- ---------------------------------------------
-- Count Time
-- ---------------------------------------------

-- Prepare structure
	
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '0', '1')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '1', '2')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '2', '3')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '3', '4')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '4', '5')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '5', '6')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '6', '7')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '7', '8')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '8', '9')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '9', '10')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '10', '12')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '12', '14')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '14', '16')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '16', '18')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '18', '20')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '20', '25')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '25', '30')
	INSERT INTO @ResultTable (Result_Function, Result_Value2, Result_Value3) VALUES ('CountTime', '30', '99999')

-- Retrieve count

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
			SELECT
				COUNT(1)
			FROM
				@TimeResult
			WHERE
				Compare_Time >= CONVERT(int, Result_Value2) * 1000
					AND Compare_Time < CONVERT(int, Result_Value3) * 1000
		),
		Result_Title = Result_Value2
	WHERE
		Result_Function = 'CountTime'	

-- Retrieve Min, Max, Avg

	INSERT INTO @ResultTable (Result_Function, Result_Title, Result_Value1) 
	SELECT
		'CountTimeSumm',
		'Min',
		MIN(Compare_Time) / 1000.0
	FROM
		@TimeResult
	
	INSERT INTO @ResultTable (Result_Function, Result_Title, Result_Value1) 
	SELECT
		'CountTimeSumm',
		'Max',
		MAX(Compare_Time) / 1000.0
	FROM
		@TimeResult
	
	INSERT INTO @ResultTable (Result_Function, Result_Title, Result_Value1) 
	SELECT
		'CountTimeSumm',
		'Avg',
		AVG(Compare_Time) / 1000.0
	FROM
		@TimeResult


-- =============================================
-- Store into tables
-- =============================================

-- Clear today's record if any

	DELETE FROM
		_CMSVaccinationRecordStat
	WHERE
		Report_Dtm = CONVERT(varchar(10), @Start_Dtm, 20)  -- To yyyy-mm-dd
			AND Result_Function IN ('ClaimEnquire', 'VREEnquire', 'CountTime', 'CountTimeSumm')

-- Insert record

	INSERT INTO _CMSVaccinationRecordStat (
		System_Dtm,
		Report_Dtm,
		Result_Function,
		Result_Title,
		Result_Value1
	)
	SELECT
		GETDATE() AS [System_Dtm],
		CONVERT(varchar(10), @Start_Dtm, 20) AS [Report_Dtm],  -- To yyyy-mm-dd
		Result_Function,
		Result_Title,
		Result_Value1
	FROM
		@ResultTable


END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_CMSVaccinationRecordTime_Stat_Write_ByDtm] TO HCVU
GO
