IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_ClaimDurationSmartIC_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_ClaimDurationSmartIC_Stat_Schedule]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	8 September 2011
-- CR No.:		INT11-0038 (Claim Duration Statistic report)
-- Description:		Fix measure of claim duration if user clicks VRE (Vaccination Record Enquiry) to claim
--						- Function_Code, Log_ID that are considered for start of a claim:
--								('020201','00018')
--								('020201','00048')
--								('020202','00018')
--								('020202','00048')
--								('020801','00048')  New
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 October 2010
-- Description:		Fix account creation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 June 2010
-- Description:		Include only the cases of successful search
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		4 June 2010
-- Description:		Statistics for getting claim duration relating to Smart IC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_ClaimDurationSmartIC_Stat_Schedule]
	@Start_Dtm	datetime,
	@End_Dtm	datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting 
-- =============================================
	/*
	DECLARE @Function_Code_Master table (
		Function_Code	char(6)
	)
	
	DECLARE @Log_ID_Master table (
		Log_ID			char(5)
	)
	*/
	DECLARE @Include_Master table (
		Function_Code	char(6),
		Log_ID			char(5)
	)	

	/*	
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020201')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020202')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020801')		-- consider Vaccination Record Enquiry

	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00048')	-- [A-Start]	Click 'Read and Search Card' and Token Request Complelet: <Scheme Code><...>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00018')	-- [A-Start]	Claim For Same Patient
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00017')	-- [A-End]		Complete Claim: <Transaction No><...>
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00024')	-- [B-Start]	Account Creation Page Loaded
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00025')	-- [B-Start]	Create Acccount Pressed
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00056')	-- [B-Start]	Confirm Modify Account: <Smart ID Read Status>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00058')	-- [B-End]		Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00059')	-- [B-End]		Confirm SmartID Detail Failed
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00040')	-- [C]			Procced to claim
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00023')	-- [D]			Form Printed
	*/

	-- Claim 020201
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00048')	-- [A-Start]	Click 'Read and Search Card' and Token Request Complelet: <Scheme Code><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00018')	-- [A-Start]	Claim For Same Patient
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00017')	-- [A-End]		Complete Claim: <Transaction No><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00024')	-- [B-Start]	Account Creation Page Loaded
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00025')	-- [B-Start]	Create Acccount Pressed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00056')	-- [B-Start]	Confirm Modify Account: <Smart ID Read Status>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00058')	-- [B-End]		Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00059')	-- [B-End]		Confirm SmartID Detail Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00040')	-- [C]			Proceed to claim
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00023')	-- [D]			Form Printed

	-- Claim (Text only version) 020202
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00048')	-- [A-Start]	Click 'Read and Search Card' and Token Request Complelet: <Scheme Code><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00018')	-- [A-Start]	Claim For Same Patient
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00017')	-- [A-End]		Complete Claim: <Transaction No><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00024')	-- [B-Start]	Account Creation Page Loaded
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00025')	-- [B-Start]	Create Acccount Pressed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00056')	-- [B-Start]	Confirm Modify Account: <Smart ID Read Status>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00058')	-- [B-End]		Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00059')	-- [B-End]		Confirm SmartID Detail Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00040')	-- [C]			Proceed to claim
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00023')	-- [D]			Form Printed

	-- Vaccination Record Enquiry 020801
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020801','00048')	-- [A-Start]	Click 'Read and Search Card' and Token Request Complelet: <Scheme Code><...>


-- =============================================
-- Declaration 
-- =============================================
	DECLARE @Year		smallint	-- For identifying the audit log table


-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @AuditLog table (
		System_Dtm					datetime,
		SP_ID						char(8),
		Data_Entry					varchar(20),
		Function_Code				char(6),
		Log_ID						char(5),
		Description					nvarchar(MAX),
		Session_ID					varchar(MAX)
	)
	
	DECLARE @ClaimResult table (
		Start_Dtm					datetime,
		End_Dtm						datetime,
		Second_Diff					int,
		Creation					char(1),
		Print_Consent				char(1),
		Print_Consent_Time			datetime,
		Print_Consent_Second_Diff	int,
		Session_ID					varchar(MAX)
	)

	DECLARE @AccountResult table (
		Start_Dtm					datetime,
		End_Dtm						datetime,
		Second_Diff					int,
		Session_ID					varchar(MAX)
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
	IF @Year = '09' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
			CONVERT(varchar(MAX), DecryptByKey(E_User_ID)),
			CONVERT(varchar(MAX), DecryptByKey(E_DataEntryAccount)),
			CONVERT(varchar(MAX), DecryptByKey(E_Function_Code)),
			CONVERT(varchar(MAX), DecryptByKey(E_Log_ID)),
			CONVERT(nvarchar(MAX), DecryptByKey(E_Description)),
			CONVERT(varchar(MAX), DecryptByKey(E_Session_ID))
		FROM
			AuditLogHCSP09
		WHERE
			System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP09.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP09.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
						
	END ELSE IF @Year = '10' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP10.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP10.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
				
	END ELSE IF @Year = '11' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP11.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP11.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
	
	END ELSE IF @Year = '12' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP12.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP12.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
	
	END ELSE IF @Year = '13' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP13.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP13.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
				
	END ELSE IF @Year = '14' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP14.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP14.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
	
	END ELSE IF @Year = '15' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP15.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP15.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
	
	END ELSE IF @Year = '16' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP16.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP16.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
				
	END ELSE IF @Year = '17' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP17.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP17.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))
				
	END ELSE IF @Year = '18' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP18.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP18.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '19' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP19.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP19.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '20' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP20.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP20.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '21' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP21.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP21.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '22' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP22.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP22.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '23' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP23.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP23.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '24' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP24.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP24.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '25' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP25.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP25.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '26' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP26.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP26.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '27' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP27.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP27.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END ELSE IF @Year = '28' BEGIN
		INSERT INTO @AuditLog (
			System_Dtm,
			SP_ID,
			Data_Entry,
			Function_Code,
			Log_ID,
			Description,
			Session_ID
		)
		SELECT
			System_Dtm,
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
				AND 0 < (SELECT Count(*) FROM @Include_Master WHERE 
					AuditLogHCSP28.E_Function_Code=EncryptByKey(KEY_GUID('sym_Key'), Function_Code)
					AND AuditLogHCSP28.E_Log_ID=EncryptByKey(KEY_GUID('sym_Key'), Log_ID))

	END

	CLOSE SYMMETRIC KEY sym_Key


-- =============================================
-- Process data
-- =============================================

-----------------------------------------------------------------------------------------------
-- Clear up the claim not relating to Smart IC
-----------------------------------------------------------------------------------------------

	DELETE FROM
		@AuditLog
	WHERE
		Log_ID = '00017'
			AND Description NOT LIKE '%<Is Read by Smart ID Case: True>%'

-----------------------------------------------------------------------------------------------
-- Clear up Claim's Audit Log (Ensure the audit log comes with a pair)
-----------------------------------------------------------------------------------------------

	DECLARE @IsEnd					char(1)
	
	DECLARE @System_Dtm				datetime
	DECLARE @SP_ID					char(8)
	DECLARE @Data_Entry				varchar(20)
	DECLARE @Function_Code			char(6)
	DECLARE @Log_ID					char(5)
	DECLARE @Description			nvarchar(MAX)
	DECLARE @Session_ID				varchar(MAX)
	
	DECLARE @Temp_System_Dtm		datetime
	DECLARE @Temp_Function_Code		char(6)
	DECLARE @Temp_Log_ID			char(5)
	DECLARE @Temp_Session_ID		varchar(MAX)
	
	SELECT @IsEnd = 'N'
	SELECT @Temp_Log_ID = NULL
	SELECT @Temp_System_Dtm = NULL
	SELECT @Temp_Session_ID = NULL
	SELECT @Temp_Function_Code = NULL

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
	--INT11-0038 commented out
	/*
			Function_Code,
			SP_ID,
			Data_Entry,
			Session_ID,
			System_Dtm DESC
	*/
	--INT11-0038 added
			Session_ID,
			System_Dtm DESC,
			Function_Code,
			SP_ID,
			Data_Entry
		
	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
		
	WHILE @@Fetch_Status = 0 BEGIN
		
		SELECT @Temp_Log_ID = @Log_ID
		SELECT @Temp_System_Dtm = @System_Dtm

		--INT11-0038 commented out			
		/*
		IF @Temp_Function_Code <> @Function_Code BEGIN
			SELECT @Temp_Function_Code = NULL
		END
		*/
		--INT11-0038 added
		IF 0 = (SELECT COUNT(*) FROM @Include_Master WHERE Function_Code=@Function_Code) AND (@Temp_Log_ID='00048' OR @Temp_Log_ID='00018') BEGIN
			SELECT @Temp_Function_Code = NULL
		END
		
		IF @Temp_Log_ID = '00017' BEGIN
			-- Complete Claim: Mark the Complete Flag
			SELECT @IsEnd = 'Y'
			SELECT @Temp_Session_ID = @Session_ID
			SELECT @Temp_Function_Code = @Function_Code
		
		END ELSE IF @Temp_Log_ID = '00048' OR @Temp_Log_ID = '00018' BEGIN
			-- Search EHSAccount Start
			IF @IsEnd = 'Y' BEGIN
				-- Matched a pair
				SELECT @IsEnd = 'N'
					
			END ELSE BEGIN
				-- Located a start log without a complete log --> Ignore the log
				DELETE FROM
					@AuditLog
				WHERE
					System_Dtm = @Temp_System_Dtm
						AND Log_ID = @Temp_Log_ID
						AND (Function_Code = @Temp_Function_Code OR @Temp_Function_Code IS NULL)
						
			END
		END
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
		
	END

	CLOSE My_Cursor
	
	
-----------------------------------------------------------------------------------------------
-- Clear up Account Creation's Audit Log (Ensure the audit log comes with a pair)
-----------------------------------------------------------------------------------------------
	
	SELECT @IsEnd = 'N'
	SELECT @Temp_Log_ID = NULL
	SELECT @Temp_System_Dtm = NULL
	SELECT @Temp_Session_ID = NULL
	SELECT @Temp_Function_Code = NULL

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	
	WHILE @@Fetch_Status = 0 BEGIN

		SELECT @Temp_Log_ID = @Log_ID
		SELECT @Temp_System_Dtm = @System_Dtm

		IF @Temp_Function_Code <> @Function_Code BEGIN
			SELECT @Temp_Function_Code = NULL
		END
		
		IF @Temp_Log_ID IN ('00058', '00059') BEGIN
			-- Create Account Complete
			SELECT @IsEnd = 'Y'
			SELECT @Temp_Session_ID = @Session_ID	
			SELECT @Temp_Function_Code = @Function_Code
		
		END ELSE IF @Temp_Log_ID IN ('00024', '00025', '00056') BEGIN
			-- Create Account Start
			IF @IsEnd = 'Y' BEGIN
				-- Matched a pair
				SELECT @IsEnd = 'N'
			
			END ELSE BEGIN
				-- Located a start log without a complete log --> Ignore the log
				DELETE FROM
					@AuditLog
				WHERE
					System_Dtm = @Temp_System_Dtm
						AND Log_ID = @Temp_Log_ID
						AND (Function_Code = @Temp_Function_Code OR @Temp_Function_Code IS NULL)
						
			END
			
		END

		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
		
	END

	CLOSE My_Cursor
	DEALLOCATE My_Cursor
	
	
-----------------------------------------------------------------------------------------------
-- Prepare time info for Claim (with A/C Creation + claim printing)
-----------------------------------------------------------------------------------------------
	
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
		--INT11-0038 commented out
		/*
			Function_Code,
			SP_ID,
			Data_Entry,
			Session_ID,
			System_Dtm
		*/
		--INT11-0038 added
			Session_ID,
			System_Dtm,
			Function_Code,
			SP_ID,
			Data_Entry
			
	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID = '00048' OR @Log_ID = '00018' BEGIN
			-- Start log
			SELECT @Temp_System_Dtm = @System_Dtm
			SELECT @Temp_Session_ID = @Session_ID
			SELECT @Temp_Function_Code = @Function_Code
		
		END ELSE IF @Log_ID = '00017' BEGIN 
			-- Complete Claim
			IF @Temp_System_Dtm IS NOT NULL
				AND @Temp_Session_ID IS NOT NULL AND @Temp_Session_ID = @Session_ID
				AND 0 < (SELECT COUNT(*) FROM @Include_Master WHERE Function_code=@Function_Code AND Log_ID='00017') BEGIN		--INT11-0038 added
				/* AND @Temp_Function_Code = @Function_Code BEGIN */	-- INT11-0038 commented out
				-- Located a pair: Put into the Result table
				INSERT INTO @ClaimResult (
					Start_Dtm,
					End_Dtm,
					Second_Diff,
					Session_ID
				)
				SELECT
					@Temp_System_Dtm,
					@System_Dtm, 
					DATEDIFF(ss, @Temp_System_Dtm, @System_Dtm),
					@Session_ID

				SELECT @Temp_System_Dtm = NULL
				
			END
			
		END
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
		
	END

	CLOSE My_Cursor


-----------------------------------------------------------------------------------------------
-- Prepare time information for Account Creation
-----------------------------------------------------------------------------------------------

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID IN ('00024', '00025', '00056') BEGIN
			-- Start log
			SELECT @Temp_System_Dtm = @System_Dtm
			SELECT @Temp_Function_Code = @Function_Code
			SELECT @Temp_Session_ID = @Session_ID
		
		END ELSE IF @Log_ID IN ('00058', '00059') BEGIN 
			-- Eng log
			IF @Temp_System_Dtm IS NOT NULL
				AND @Temp_Session_ID IS NOT NULL AND @Temp_Session_ID = @Session_ID
				AND @Temp_Function_Code = @Function_Code BEGIN
				-- Located a pair: Put into the Result table
				INSERT INTO @AccountResult (
					Start_Dtm,
					End_Dtm,
					Second_Diff,
					Session_ID
				)
				SELECT
					@Temp_System_Dtm,
					@System_Dtm, 
					DATEDIFF(ss, @Temp_System_Dtm, @System_Dtm),
					@Session_ID

				SELECT @Temp_System_Dtm = NULL
			END
		END
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	END

	CLOSE My_Cursor
	DEALLOCATE My_Cursor
	
	
-----------------------------------------------------------------------------------------------
-- Check which Claim log included Account Creation Process
-----------------------------------------------------------------------------------------------

	UPDATE
		@ClaimResult
	SET
		Creation = 'Y'
	FROM
		@ClaimResult R
			INNER JOIN (SELECT System_Dtm, Session_ID FROM @AuditLog WHERE Log_ID = '00040') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm
		
	UPDATE
		@ClaimResult
	SET
		Creation = 'N'
	WHERE
		Creation IS NULL
	
	
-----------------------------------------------------------------------------------------------
-- Check which Claim log included Print Consent form
-----------------------------------------------------------------------------------------------

	UPDATE
		@ClaimResult
	SET
		Print_Consent = 'Y',
		Print_Consent_Time = T.System_Dtm,
		Print_Consent_Second_Diff = DATEDIFF(ss, T.System_Dtm, R.End_Dtm)
	FROM
		@ClaimResult R
			INNER JOIN (SELECT Session_ID, System_Dtm FROM @AuditLog WHERE Log_ID = '00023') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm

	UPDATE
		@ClaimResult
	SET
		Print_Consent = 'N'
	WHERE
		Print_Consent_Second_Diff IS NULL

-- =============================================
-- Retrieve data and store into tables
-- =============================================

	-- All claim duration (without A/C Creation & - printing)
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	SELECT
		GETDATE(),
		@Start_Dtm,
		'SmartIC Claim (without A/C Creation & - claim printing)',
		MAX(Second_Diff - ISNULL(Print_Consent_Second_Diff, 0)), 
		MIN(Second_Diff - ISNULL(Print_Consent_Second_Diff, 0)), 
		AVG(Second_Diff - ISNULL(Print_Consent_Second_Diff, 0)),
		COUNT(1)
	FROM
		@ClaimResult
	WHERE
		Creation = 'N'
	
	-- All claim duration (without A/C Creation & + printing)
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	SELECT
		GETDATE(),
		@Start_Dtm,
		'SmartIC Claim (without A/C Creation & + claim printing)',
		MAX(Second_Diff),
		MIN(Second_Diff),
		AVG(Second_Diff),
		COUNT(1)
	FROM
		@ClaimResult
	WHERE
		Creation = 'N'
			AND Print_Consent = 'Y'
	
	-- All claim duration (with A/C Creation & - printing)
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	SELECT
		GETDATE(),
		@Start_Dtm,
		'SmartIC Claim (with A/C Creation & - claim printing)',
		MAX(Second_Diff - ISNULL(Print_Consent_Second_Diff, 0)), 
		MIN(Second_Diff - ISNULL(Print_Consent_Second_Diff, 0)), 
		AVG(Second_Diff - ISNULL(Print_Consent_Second_Diff, 0)),
		COUNT(1)
	FROM
		@ClaimResult 
	WHERE
		Creation = 'Y'

	-- All claim duration (with A/C Creation & + printing)
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value,
		no_of_transaction
	)
	SELECT
		GETDATE(),
		@Start_Dtm,
		'SmartIC Claim (with A/C Creation & + claim printing)',
		MAX(Second_Diff), 
		MIN(Second_Diff), 
		AVG(Second_Diff),
		COUNT(1)
	FROM
		@ClaimResult 
	WHERE
		Creation = 'Y'
			AND Print_Consent = 'Y'
	
	-- Printing Time
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	SELECT
		GETDATE(),
		@Start_Dtm,
		'SmartIC Claim Printing Time',
		MAX(Print_Consent_Second_Diff),
		MIN(Print_Consent_Second_Diff),
		AVG(Print_Consent_Second_Diff)
	FROM
		@ClaimResult
	WHERE
		Print_Consent = 'Y'

	-- A/C Creation
	INSERT INTO _ClaimDurationSummary (
		system_dtm,
		report_dtm,
		title,
		max_value,
		min_value,
		avg_value
	)
	SELECT
		GETDATE(),
		@Start_Dtm,
		'SmartIC A/C Creation (- Creation printing)',
		MAX(Second_Diff),
		MIN(Second_Diff),
		AVG(Second_Diff)
	FROM
		@AccountResult

END

GRANT EXECUTE ON [dbo].[proc_EHS_ClaimDurationSmartIC_Stat_Schedule] TO HCVU
GO