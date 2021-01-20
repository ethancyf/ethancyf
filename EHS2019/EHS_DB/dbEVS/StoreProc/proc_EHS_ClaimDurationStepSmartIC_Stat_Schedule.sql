IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_ClaimDurationStepSmartIC_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_ClaimDurationStepSmartIC_Stat_Schedule]
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
-- Modified by:		Tony FUNG
-- Modified date:	8 September 2011
-- CR No.:			INT11-0038 (Claim Duration Statistic report)
-- Description:		Fix to cater claims using Smart IC through VRE (Vaccination Record Enquiry)
--						Added Function Code: 020801
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 October 2010
-- Description:		(1) Fix the case of reading Smart ID Card with text-only version will have
--						an extra audit log Page Loaded
--					(2) Compare SP_ID and Data_Entry for adjacent records
--					(3) Include 00024 (Account Creation Page Loaded) for account creation
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

CREATE PROCEDURE [dbo].[proc_EHS_ClaimDurationStepSmartIC_Stat_Schedule]
	@Start_Dtm	datetime,
	@End_Dtm	datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting 
-- =============================================
	--INT11-0038 commented out
	/*
	DECLARE @Function_Code_Master table (
		Function_Code	char(6)
	)
	
	DECLARE @Log_ID_Master table (
		Log_ID			char(5)
	)
	*/

	--INT11-0038
	-- The @Include_Master table is built, in order to 
	-- include both Function_Code and Log_ID
	DECLARE @Include_Master table (
		Function_Code char(6),
		Log_ID			char(5)
	)

	--INT11-0038 commented out
	/*
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020201')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020202')
	INSERT INTO @Function_Code_Master (Function_Code) VALUES ('020801')		-- consider Vaccination Record Enquiry
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00042')	-- [A-Start]	Enter Claim Detail Page Loaded
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00010')	-- [A]			Enter Claim Detail Start: <...><Is Read by Smart ID Case>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00011')	-- [A-End]		Enter Claim Detail Complete
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00013')	-- [A-End]		Enter Claim Detail Failed
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00047')	-- [B-Start]	Click 'Read and Search Card' and Token Request: <Scheme Code>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00058')	-- [B]			Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00040')	-- [B]			Procced to claim
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00017')	-- [B-End]		Complete Claim: <...><Is Read by Smart ID Case>
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00024')	-- [C-Start]	Account Creation Page Loaded
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00025')	-- [C-Start]	Create Acccount Pressed
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00056')	-- [C-Start]	Confirm Modify Account: <Smart ID Read Status>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00058')	-- [C-End]		Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00059')	-- [C-End]		Confirm SmartID Detail Failed
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00001')	-- [D-Start]	Claim Page Loaded: <Practice Selected><...>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00052')	-- [D-Start]	Search & validate account with CFD Fail: <...>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00063')	-- [D-Start]	Get CFD Fail: <Artifact>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00005')	-- [D-Start]	Search Account Failed
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00008')	-- [D-Start]	Prefilled Search Failed
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00039')	-- [D-Start]	Create new account
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00044')	-- [D-Start]	Next Claim
	--INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00048')	-- [D-Mid]		Click 'Read and Search Card' and Token Request Complelet
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00051')	-- [D-End]		Search & validate account with CFD Complete: <...>
	
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00048')	-- [E-Start]	Click 'Read and Search Card' and Token Request Complelet
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00064')	-- [E-End]		Redirect FROM IDEAS after Token Request: <Scheme Code>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00062')	-- [F]			Get CFD Complete: <Artifact>
	INSERT INTO @Log_ID_Master (Log_ID) VALUES ('00063')	-- [F]			Get CFD Fail: <Artifact>
	*/
	
	--INT11--0038
	
	-- Claim 020201
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00042')	-- [A-Start]	Enter Claim Detail Page Loaded
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00010')	-- [A]			Enter Claim Detail Start: <...><Is Read by Smart ID Case>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00011')	-- [A-End]		Enter Claim Detail Complete
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00013')	-- [A-End]		Enter Claim Detail Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00047')	-- [B-Start]	Click 'Read and Search Card' and Token Request: <Scheme Code>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00058')	-- [B]			Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00040')	-- [B]			Proceed to claim
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00017')	-- [B-End]		Complete Claim: <...><Is Read by Smart ID Case>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00024')	-- [C-Start]	Account Creation Page Loaded
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00025')	-- [C-Start]	Create Acccount Pressed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00056')	-- [C-Start]	Confirm Modify Account: <Smart ID Read Status>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00058')	-- [C-End]		Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00059')	-- [C-End]		Confirm SmartID Detail Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00001')	-- [D-Start]	Claim Page Loaded: <Practice Selected><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00052')	-- [D-Start]	Search & validate account with CFD Fail: <...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00063')	-- [D-Start]	Get CFD Fail: <Artifact>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00005')	-- [D-Start]	Search Account Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00008')	-- [D-Start]	Prefilled Search Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00039')	-- [D-Start]	Create new account
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00044')	-- [D-Start]	Next Claim
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00051')	-- [D-Mid]		Click 'Read and Search Card' and Token Request Completet
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00048')	-- [E-Start]	Click 'Read and Search Card' and Token Request Complelet
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00064')	-- [E-End]		Redirect FROM IDEAS after Token Request: <Scheme Code>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00062')	-- [F]			Get CFD Complete: <Artifact>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020201','00063')	-- [F]			Get CFD Fail: <Artifact>

	-- Claim (Text Only Version) 020202
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00042')	-- [A-Start]	Enter Claim Detail Page Loaded
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00010')	-- [A]			Enter Claim Detail Start: <...><Is Read by Smart ID Case>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00011')	-- [A-End]		Enter Claim Detail Complete
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00013')	-- [A-End]		Enter Claim Detail Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00047')	-- [B-Start]	Click 'Read and Search Card' and Token Request: <Scheme Code>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00058')	-- [B]			Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00040')	-- [B]			Proceed to claim
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00017')	-- [B-End]		Complete Claim: <...><Is Read by Smart ID Case>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00024')	-- [C-Start]	Account Creation Page Loaded
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00025')	-- [C-Start]	Create Acccount Pressed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00056')	-- [C-Start]	Confirm Modify Account: <Smart ID Read Status>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00058')	-- [C-End]		Confirm Detail Complete: <Smart ID Read Status><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00059')	-- [C-End]		Confirm SmartID Detail Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00001')	-- [D-Start]	Claim Page Loaded: <Practice Selected><...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00052')	-- [D-Start]	Search & validate account with CFD Fail: <...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00063')	-- [D-Start]	Get CFD Fail: <Artifact>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00005')	-- [D-Start]	Search Account Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00008')	-- [D-Start]	Prefilled Search Failed
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00039')	-- [D-Start]	Create new account
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00044')	-- [D-Start]	Next Claim
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00051')	-- [D-End]		Search & validate account with CFD Complete: <...>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00048')	-- [E-Start]	Click 'Read and Search Card' and Token Request Complelet
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00064')	-- [E-End]		Redirect FROM IDEAS after Token Request: <Scheme Code>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00062')	-- [F]			Get CFD Complete: <Artifact>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020202','00063')	-- [F]			Get CFD Fail: <Artifact>

	-- Vaccination Record Enquiry 020801
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020801','00047')	-- [B-Start]	Click 'Read and Search Card' and Token Request: <Scheme Code>
	INSERT INTO @Include_Master (Function_Code,Log_ID) VALUES ('020801','00048')	-- [E-Start]	Click 'Read and Search Card' and Token Request Complelet
		
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

	-- Statistic Result (Enter Claim Detail)
	DECLARE @EnterClaimDetailResult table (
		Start_Dtm				datetime,
		End_Dtm					datetime,
		Compare_Time			int,
		Action_Taken_Time		int,
		Session_ID				varchar(MAX),
		Is_Smart_IC				char(1)
	)

	-- Statistic Result (Process To Claim) (Step from 'Search Account' to 'Proceed To Claim')
	DECLARE @ProceedToClaimResult table (
		Start_Dtm				datetime,
		End_Dtm					datetime,
		Compare_Time			int,
		Action_Taken_Time		int,
		Session_ID				varchar(MAX),
		Creation				char(1),
		Complete_Create_Temp	datetime,
		Proceed_To_Claim		datetime		
	)
	
	-- Statistic Result (Enter A/C Creation Details)
	DECLARE @AccountResult table (
		Start_Dtm				datetime,
		End_Dtm					datetime,
		Compare_Time			int,
		Action_Taken_Time		int,
		Session_ID				varchar(MAX)
	)
	
	-- Statistic Result (Search Account)
	DECLARE @SearchAccountResult table (
		Start_Dtm				datetime,
		End_Dtm					datetime,
		Compare_Time			int,
		Action_Taken_Time		int,
		Session_ID				varchar(MAX),
		Read_Card				char(1),
		Read_Card_Start_Dtm		datetime,
		Read_Card_End_Dtm		datetime,
		Get_CFD					char(1),
		Get_CFD_Time			int
	)


-- =============================================
-- Initialization
-- =============================================
	SET @Year = CONVERT(varchar(2), @Start_Dtm, 12)	-- Extract the Calendar Year: "12" gives the format YYMMDD

	EXEC [proc_SymmetricKey_open]


-- =============================================
-- Retrieve data
-- =============================================	
	IF @Year = '09' BEGIN
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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
			CONVERT(varchar(MAX), DecryptByKey(E_USer_ID)),
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

	EXEC [proc_SymmetricKey_close]


-- =============================================
-- Process data
-- =============================================

-----------------------------------------------------------------------------------------------
-- Delete the log not relating to Smart IC
-----------------------------------------------------------------------------------------------
	
	DELETE FROM
		@AuditLog
	WHERE
		Log_ID = '00010'
			AND Description NOT LIKE '%<Is Read by Smart ID Case: True>%'

	DELETE FROM
		@AuditLog
	WHERE
		Log_ID = '00017'
			AND Description NOT LIKE '%<Is Read by Smart ID Case: True>%'


-----------------------------------------------------------------------------------------------
-- Clear up Enter Claim Detail's Audit Log (ensure the audit log comes with a pair)
-----------------------------------------------------------------------------------------------

	DECLARE @IsEnd char(1)
	DECLARE @IsMid char(1)

	DECLARE @SP_ID				char(8)
	DECLARE @Data_Entry			varchar(20)
	DECLARE @Log_ID				char(5)
	DECLARE @Session_ID			varchar(MAX)
	DECLARE @System_Dtm			datetime
	DECLARE @Function_Code		char(6)
	
	DECLARE @Temp_SP_ID			char(8)
	DECLARE @Temp_Data_Entry	varchar(20)
	DECLARE @Temp_Log_ID		char(5)
	DECLARE @Temp_Session_ID	varchar(MAX)
	DECLARE @Temp_System_Dtm	datetime
	DECLARE @Temp_Function_Code	char(6)


	SELECT @IsEnd = 'N'
	SELECT @Temp_SP_ID = NULL
	SELECT @Temp_Data_Entry = NULL
	SELECT @Temp_Log_ID = NULL
	SELECT @Temp_Session_ID = NULL
	SELECT @Temp_System_Dtm = NULL
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
			Function_Code,
			SP_ID,
			Data_Entry,
			Session_ID,
			System_Dtm DESC

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID
	
	WHILE @@Fetch_Status = 0 BEGIN

		SELECT @Temp_Log_ID = @Log_ID
		SELECT @Temp_System_Dtm = @System_Dtm

		IF @Temp_Function_Code <> @Function_Code BEGIN
			SELECT @Temp_Function_Code = NULL
		END
		
		IF @Temp_Log_ID = '00011' OR @Temp_Log_ID = '00013' BEGIN
			SELECT @IsEnd = 'Y'
			SELECT @Temp_Session_ID = @Session_ID	
			SELECT @Temp_Function_Code = @Function_Code
			
		END ELSE IF @Temp_Log_ID = '00042' BEGIN
			IF @IsEnd = 'Y' BEGIN
				SELECT @IsEnd = 'N'
				
			END ELSE BEGIN
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
-- Clear up Process To Claim's Audit Log (ensure the audit log comes with a pair)
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

		--INT11-0038 commented out			
		/*
		IF @Temp_Function_Code <> @Function_Code BEGIN
			SELECT @Temp_Function_Code = NULL
		END
		*/

		--INT11-0038 added
		IF 0 = (SELECT COUNT(*) FROM @Include_Master WHERE Function_Code=@Function_Code) AND (@Temp_Log_ID='00047') BEGIN
			SELECT @Temp_Function_Code = NULL
		END

		IF @Temp_Log_ID = '00017' BEGIN
			SELECT @IsEnd = 'Y'	
			SELECT @Temp_Session_ID = @Session_ID	
			SELECT @Temp_Function_Code = @Function_Code
				
		END ELSE IF @Temp_Log_ID = '00047' BEGIN
			IF @IsEnd = 'Y' BEGIN
				SELECT @IsEnd = 'N'
					
			END ELSE BEGIN
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
-- Clear up Enter A/C Creation Detail's Audit Log (ensure the audit log comes with a pair)
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
		
		IF @Temp_Log_ID = '00058' OR @Temp_Log_ID = '00059' BEGIN
				SELECT @IsEnd = 'Y'	
				SELECT @Temp_Session_ID = @Session_ID	
				SELECT @Temp_Function_Code = @Function_Code
				
		END ELSE IF @Log_ID IN ('00024', '00025', '00056') BEGIN
			IF @IsEnd = 'Y' BEGIN
				SELECT @IsEnd = 'N'
						
			END ELSE BEGIN
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
-- Clear up Search Account's Audit Log (ensure the audit log comes with a pair)
-----------------------------------------------------------------------------------------------
-- This must be in a sequence:
--		00001 / 00052 / 00063 / 00005 / 00008 / 00039 / 00044
--	-->	00048
--	--> 00051
-----------------------------------------------------------------------------------------------

	SELECT @IsEnd = 'N'
	SELECT @IsMid = 'N'
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
		
		IF @Temp_Log_ID IN ('00051') BEGIN
			SELECT @IsEnd = 'Y'
			SELECT @IsMid = 'N'
			SELECT @Temp_Session_ID = @Session_ID	
			SELECT @Temp_Function_Code = @Function_Code
		
		END ELSE IF @Temp_Log_ID IN ('00048') BEGIN
			IF @IsEnd = 'Y' BEGIN
				SELECT @IsMid = 'Y'
			END
		
		END ELSE IF @Temp_Log_ID IN ('00001', '00052', '00063', '00005', '00008', '00039', '00044') BEGIN
			IF @IsEnd = 'Y' AND @IsMid = 'Y' BEGIN
				SELECT @IsEnd = 'N'
				SELECT @IsMid = 'N'
						
			END ELSE BEGIN
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
-- Prepare time info for Enter Claim Detail
-----------------------------------------------------------------------------------------------

	DECLARE @Action_Dtm datetime
	DECLARE @Action_End_Dtm datetime

	SELECT @Temp_SP_ID = NULL
	SELECT @Temp_Data_Entry = NULL
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
			Session_ID,
			Action_Dtm,
			End_Dtm
		FROM
			@AuditLog
		ORDER BY
			Function_Code,
			SP_ID,
			Data_Entry,
			Session_ID,
			System_Dtm

	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID = '00042' BEGIN
			SELECT @Temp_SP_ID = @SP_ID
			SELECT @Temp_Data_Entry = @Data_Entry
			SELECT @Temp_System_Dtm = @System_Dtm
			SELECT @Temp_Session_ID = @Session_ID
			SELECT @Temp_Function_Code = @Function_Code
		
		END ELSE IF @Log_ID = '00011' OR @Log_ID = '00013' BEGIN 
			IF @Temp_System_Dtm IS NOT NULL
				AND @Temp_Session_ID IS NOT NULL AND @Temp_Session_ID = @Session_ID
				AND @Temp_Function_Code = @Function_Code
				AND @Temp_SP_ID = @SP_ID
				AND (@Temp_Data_Entry = @Data_Entry OR (@Temp_Data_Entry IS NULL AND @Data_Entry IS NULL)) BEGIN
				
				INSERT INTO @EnterClaimDetailResult (
					Start_Dtm,
					End_Dtm,
					Compare_Time,
					Action_Taken_Time,
					Session_ID
				)
				SELECT
					@Temp_System_Dtm,
					@System_Dtm, 
					DATEDIFF(ss, @Temp_System_Dtm, @System_Dtm),
					DATEDIFF(ms, @Action_Dtm, @Action_End_Dtm) / 1000,
					@Session_ID
					
				SELECT @Temp_System_Dtm = NULL
				
			END
		END
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	END

	CLOSE My_Cursor
	

-- Delete the records not relating to Smart IC

	UPDATE
		@EnterClaimDetailResult
	SET
		Is_Smart_IC = 'Y'
	FROM
		@EnterClaimDetailResult R
			INNER JOIN (SELECT Session_ID, System_Dtm FROM @AuditLog WHERE Log_ID = '00010') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm
	
	DELETE FROM
		@EnterClaimDetailResult
	WHERE
		Is_Smart_IC IS NULL
		

-----------------------------------------------------------------------------------------------
-- Prepare time info for Proceed to Claim
-----------------------------------------------------------------------------------------------

	SELECT @Temp_SP_ID = NULL
	SELECT @Temp_Data_Entry = NULL
	SELECT @Temp_System_Dtm = NULL
	SELECT @Temp_Session_ID = NULL
	SELECT @Temp_Function_Code = NULL
	
	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID = '00047' BEGIN
			SELECT @Temp_SP_ID = @SP_ID
			SELECT @Temp_Data_Entry = @Data_Entry
			SELECT @Temp_System_Dtm = @System_Dtm
			SELECT @Temp_Session_ID = @Session_ID
			SELECT @Temp_Function_Code = @Function_Code
			
		END ELSE IF @Log_ID = '00017' BEGIN 
			IF @Temp_System_Dtm IS NOT NULL
				AND @Temp_Session_ID IS NOT NULL AND @Temp_Session_ID = @Session_ID
				AND 0 < (SELECT COUNT(*) FROM @Include_Master WHERE Function_code=@Function_Code AND Log_ID='00017')		--INT11-0038 added
				/* AND @Temp_Function_Code = @Function_Code */ --INT11-0038 commented out
				AND @Temp_SP_ID = @SP_ID
				AND (@Temp_Data_Entry = @Data_Entry OR (@Temp_Data_Entry IS NULL AND @Data_Entry IS NULL)) BEGIN
				
				INSERT INTO @ProceedToClaimResult (
					Start_Dtm,
					End_Dtm,
					Compare_Time,							
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
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	END

	CLOSE My_Cursor


-- Check which Claim log included Account Creation Process

	UPDATE
		@ProceedToClaimResult
	SET
		Creation = 'Y',
		Proceed_To_Claim = T.System_Dtm
	FROM
		@ProceedToClaimResult R
			INNER JOIN (SELECT Session_ID, System_Dtm FROM @AuditLog WHERE Log_ID = '00040') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm

	UPDATE
		@ProceedToClaimResult
	SET
		Complete_Create_Temp = T.System_Dtm,
		Action_Taken_Time = DATEDIFF(ms, T.Action_Dtm, T.End_Dtm) / 1000
	FROM
		@ProceedToClaimResult R
			INNER JOIN (SELECT Session_ID, System_Dtm, Action_Dtm, End_Dtm FROM @AuditLog WHERE Log_ID = '00058') AS T
				ON R.Session_ID = T.Session_ID
					AND T.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm
	WHERE
		R.Creation = 'Y'

	UPDATE
		@ProceedToClaimResult
	SET
		Compare_Time = DATEDIFF(ss, Complete_Create_Temp, Proceed_To_Claim)
	WHERE
		Creation = 'Y'


-----------------------------------------------------------------------------------------------
-- Prepare time info for Enter Account Creation Detail
-----------------------------------------------------------------------------------------------

	SELECT @Temp_SP_ID = NULL
	SELECT @Temp_Data_Entry = NULL
	SELECT @Temp_System_Dtm = NULL
	SELECT @Temp_Session_ID = NULL
	SELECT @Temp_Function_Code = NULL
	
	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID IN ('00024', '00025', '00056') BEGIN
			SELECT @Temp_SP_ID = @SP_ID
			SELECT @Temp_Data_Entry = @Data_Entry
			SELECT @Temp_System_Dtm = @System_Dtm
			SELECT @Temp_Session_ID = @Session_ID
			SELECT @Temp_Function_Code = @Function_Code
		
		END ELSE IF @Log_ID = '00058' OR @Log_ID = '00059' BEGIN 
			IF @Temp_System_Dtm IS NOT NULL
				AND @Temp_Session_ID IS NOT NULL AND @Temp_Session_ID = @Session_ID
				AND @Temp_Function_Code = @Function_Code
				AND @Temp_SP_ID = @SP_ID
				AND (@Temp_Data_Entry = @Data_Entry OR (@Temp_Data_Entry IS NULL AND @Data_Entry IS NULL)) BEGIN
				
				INSERT INTO @AccountResult (
					Start_Dtm,
					End_Dtm,
					Compare_Time,
					Action_Taken_Time,
					Session_ID
				)
				SELECT
					@Temp_System_Dtm,
					@System_Dtm, 
					DATEDIFF(ss, @Temp_System_Dtm, @System_Dtm),
					DATEDIFF(ms, @Action_Dtm, @Action_End_Dtm) / 1000,
					@Session_ID

				SELECT @Temp_System_Dtm = NULL
				
			END
		END
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	END

	CLOSE My_Cursor


-----------------------------------------------------------------------------------------------
-- Prepare time info for Search Account
-----------------------------------------------------------------------------------------------

	SELECT @Temp_SP_ID = NULL
	SELECT @Temp_Data_Entry = NULL
	SELECT @Temp_System_Dtm = NULL
	SELECT @Temp_Session_ID = NULL
	SELECT @Temp_Function_Code = NULL
	
	OPEN My_Cursor
	FETCH NEXT FROM My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	
	WHILE @@Fetch_Status = 0 BEGIN
		IF @Log_ID IN ('00001', '00052', '00063', '00005', '00008', '00039', '00044') BEGIN
			SELECT @Temp_SP_ID = @SP_ID
			SELECT @Temp_Data_Entry = @Data_Entry
			SELECT @Temp_System_Dtm = @System_Dtm
			SELECT @Temp_Session_ID = @Session_ID
			SELECT @Temp_Function_Code = @Function_Code
			
		END ELSE IF @Log_ID IN ('00051') BEGIN 
			IF @Temp_System_Dtm IS NOT NULL
				AND @Temp_Session_ID IS NOT NULL AND @Temp_Session_ID = @Session_ID
				AND @Temp_Function_Code = @Function_Code
				AND @Temp_SP_ID = @SP_ID
				AND (@Temp_Data_Entry = @Data_Entry OR (@Temp_Data_Entry IS NULL AND @Data_Entry IS NULL)) BEGIN
				
				INSERT INTO @SearchAccountResult (
					Start_Dtm,
					End_Dtm,
					Compare_Time,
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
		
		FETCH My_Cursor INTO @Function_Code, @System_Dtm, @SP_ID, @Data_Entry, @Log_ID, @Session_ID, @Action_Dtm, @Action_End_Dtm
	END

	CLOSE My_Cursor
	DEALLOCATE My_Cursor
	

-- Check whether the search account contains Read Card (in a success path it should be always yes)

	UPDATE
		@SearchAccountResult
	SET
		Read_Card = 'Y',
		Read_Card_Start_Dtm = A1.System_Dtm,
		Read_Card_End_Dtm = A2.System_Dtm
	FROM
		@SearchAccountResult R
			INNER JOIN (SELECT System_Dtm, Session_ID FROM @AuditLog WHERE Log_ID = '00048') A1
				ON R.Session_ID = A1.Session_ID
					AND A1.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm
			INNER JOIN (SELECT System_Dtm, Session_ID FROM @AuditLog WHERE Log_ID = '00064') A2
				ON R.Session_ID = A2.Session_ID
					AND A2.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm
			

-- Check whether the search account contains Get CFD (in a success path it should be always yes)

	UPDATE
		@SearchAccountResult
	SET
		Get_CFD = 'Y',
		Get_CFD_Time = DATEDIFF(ms, A.Action_Dtm, A.End_Dtm) / 1000
	FROM
		@SearchAccountResult R
			INNER JOIN (SELECT System_Dtm, Session_ID, Action_Dtm, End_Dtm FROM @AuditLog WHERE Log_ID IN ('00062', '00063')) A
				ON R.Session_ID = A.Session_ID
					AND A.System_Dtm BETWEEN R.Start_Dtm AND R.End_Dtm
			
	
-- Get the system time

	DECLARE @SearchAccount_Max	int
	DECLARE @SearchAccount_Min	int
	DECLARE @SearchAccount_Avg	int

	SELECT
		@SearchAccount_Max = MAX(DATEDIFF(ms, Action_Dtm, End_Dtm) / 1000)
	FROM
		@AuditLog
	WHERE
		Log_ID IN ('00051', '00052', '00063')
			AND Session_ID IN (SELECT Session_ID from @SearchAccountResult)

	SELECT
		@SearchAccount_Min = MIN(DATEDIFF(ms, Action_Dtm, End_Dtm) / 1000)
	FROM
		@AuditLog
	WHERE
		Log_ID IN ('00051', '00052', '00063')
			AND Session_ID IN (SELECT Session_ID from @SearchAccountResult)

	SELECT
		@SearchAccount_Avg = AVG(DATEDIFF(ms, Action_Dtm, End_Dtm) / 1000)
	FROM
		@AuditLog
	WHERE
		Log_ID IN ('00051', '00052', '00063')
			AND Session_ID IN (SELECT Session_ID from @SearchAccountResult)




-- =============================================
-- Retrieve data and store into tables
-- =============================================

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
		'SmartIC Search (User Time)',
		MAX(Compare_Time), 
		MIN(Compare_Time),
		AVG(Compare_Time),
		COUNT(1)
	FROM
		@SearchAccountResult

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
		'SmartIC Reading Card',
		MAX(DATEDIFF(ss, Read_Card_Start_Dtm, Read_Card_End_Dtm)), 
		MIN(DATEDIFF(ss, Read_Card_Start_Dtm, Read_Card_End_Dtm)),
		AVG(DATEDIFF(ss, Read_Card_Start_Dtm, Read_Card_End_Dtm))
	FROM
		@SearchAccountResult
	WHERE
		Read_Card = 'Y'

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
		'SmartIC Get CFD',
		MAX(Get_CFD_Time),
		MIN(Get_CFD_Time),
		AVG(Get_CFD_Time)
	FROM
		@SearchAccountResult
	WHERE
		Get_CFD = 'Y'
		
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
		'SmartIC Search (System Time)',
		@SearchAccount_Max,
		@SearchAccount_Min,
		@SearchAccount_Avg,
		COUNT(1)
	FROM
		@SearchAccountResult
		
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
		'SmartIC Enter Claim Details (User Time)',
		MAX(Compare_Time), 
		MIN(Compare_Time),
		AVG(Compare_Time),
		COUNT(1)
	FROM
		@EnterClaimDetailResult

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
		'SmartIC Enter Claim Details (System Time)',
		MAX(Action_Taken_Time),
		MIN(Action_Taken_Time),
		AVG(Action_Taken_Time),
		COUNT(1)
	FROM
		@EnterClaimDetailResult

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
		'SmartIC Enter A/C Creation Details (User Time)',
		MAX(Compare_Time),
		MIN(Compare_Time),
		AVG(Compare_Time),
		COUNT(1)
	FROM
		@AccountResult
	
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
		'SmartIC Enter A/C Creation Details (System Time)',
		MAX(Action_Taken_Time),
		MIN(Action_Taken_Time),
		AVG(Action_Taken_Time),
		COUNT(1)
	FROM
		@AccountResult
	
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
		'SmartIC Process To Claim (User Time)',
		MAX(Compare_Time),
		MIN(Compare_Time),
		AVG(Compare_Time),
		COUNT(1)
	FROM
		@ProceedToClaimResult
	WHERE
		Creation = 'Y'

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
		'SmartIC Process To Claim (System Time)',
		MAX(Action_Taken_Time),
		MIN(Action_Taken_Time),
		AVG(Action_Taken_Time),
		COUNT(1)
	FROM
		@ProceedToClaimResult
	WHERE
		Creation = 'Y'
	

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_ClaimDurationStepSmartIC_Stat_Schedule] TO HCVU
GO
