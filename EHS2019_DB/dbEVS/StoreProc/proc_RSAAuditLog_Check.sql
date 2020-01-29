IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RSAAuditLog_Check]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RSAAuditLog_Check]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 March 2017
-- CR No.			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Add log 00042 IsTokenExistAndFreeToAssign fail
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	17 Mar 2016
-- Description:		1. Add new LogID 00040
--					2. Return column [Raise_Email_Alert] and [Raise_Pager_Alert]
-- ============================================= 
-- =============================================
-- Modification History
-- CR No.:			CRE15-001
-- Modified by:		Winnie SUEN
-- Modified date:	2 Dec 2015
-- Description:		Change LogID
-- ============================================= 
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		24 July 2014
-- CR No.:			CRE13-029
-- Description:		Check RSA error
-- =============================================

CREATE PROCEDURE [dbo].[proc_RSAAuditLog_Check]
	@Start_Dtm		datetime,
	@End_Dtm		datetime
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Master table
-- =============================================
	DECLARE @RSA_Function_Code_Main		char(6)
	DECLARE @RSA_Function_Code_Sub		char(6)
	
	DECLARE @RSA_Log table (
		Log_ID			char(5),
		Raise_Email_Alert	char(1),
		Raise_Pager_Alert	char(1)
	)

	SET @RSA_Function_Code_Main = '990101'
	SET @RSA_Function_Code_Sub  = '990102'
	
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00003', 'Y', 'N')  -- AuthRSAUser fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00006', 'Y', 'Y')  -- AddRSAUser fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00009', 'Y', 'Y')  -- DeleteRSAUser fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00012', 'Y', 'Y')  -- UpdateRSAUserToken fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00015', 'Y', 'Y')  -- ReplaceRSAUserToken fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00018', 'Y', 'Y')  -- EnableRSAUserToken fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00021', 'Y', 'Y')  -- DisableRSAUserToken fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00024', 'Y', 'Y')  -- ResetRSAUserToken fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00027', 'Y', 'Y')  -- ListRSAUserTokenByLoginID fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00030', 'Y', 'Y')  -- listRSAUserByTokenSerialNo fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00033', 'Y', 'Y')  -- IsTokenInNextTokenMode fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00036', 'Y', 'Y')  -- AuthWithNextTokenMode fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00039', 'Y', 'Y')  -- IsUserIDAndTokenAvailable fail 
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00040', 'Y', 'Y')  -- Raise pager alert for AuthRSAUser fail
	INSERT INTO @RSA_Log (Log_ID, Raise_Email_Alert, Raise_Pager_Alert) VALUES ('00042', 'Y', 'Y')  -- IsTokenExistAndFreeToAssign fail
	
	
-- =============================================
-- Declaration 
-- =============================================
	DECLARE @Result table (
		Platform		varchar(4),
		Main_Sub		varchar(5),
		Error_Count		int,
		Raise_Email_Alert	char(1),
		Raise_Pager_Alert	char(1)		
	)
	
	DECLARE @RSAAPIVersion table (
		RSA_Version		varchar(5),
		Main_Sub		varchar(5)
	)
	

-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @RSAAPIVersion (RSA_Version, Main_Sub)
	SELECT Parm_Value1, 'Main' FROM SystemParameters WHERE Parameter_Name = 'RSAAPIVersion' AND Scheme_Code = 'ALL'

	INSERT INTO @RSAAPIVersion (RSA_Version, Main_Sub)
	SELECT Parm_Value2, 'Sub' FROM SystemParameters WHERE Parameter_Name = 'RSAAPIVersion' AND Scheme_Code = 'ALL' AND Parm_Value2 IS NOT NULL


	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key


-- =============================================
-- Retrieve data
-- =============================================	
	INSERT INTO @Result (
		Platform,
		Main_Sub,
		Error_Count,
		Raise_Email_Alert,
		Raise_Pager_Alert
	)
	SELECT
		'SP',
		'Main',
		COUNT(Distinct E_Action_Key),
		ISNULL(MAX(rl.Raise_Email_Alert), 'N'),
		ISNULL(MAX(rl.Raise_Pager_Alert), 'N')
	FROM
		ViewAuditLogHCSP v 
		JOIN @RSA_Log rl ON v.E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), rl.Log_ID)
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
			AND E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @RSA_Function_Code_Main)
	
		
	INSERT INTO @Result (
		Platform,
		Main_Sub,
		Error_Count,
		Raise_Email_Alert,
		Raise_Pager_Alert			
	)
	SELECT
		'VU',
		'Main',
		COUNT(Distinct E_Action_Key),
		ISNULL(MAX(rl.Raise_Email_Alert), 'N'),
		ISNULL(MAX(rl.Raise_Pager_Alert), 'N')
	FROM
		ViewAuditLogHCVU v 
		JOIN @RSA_Log rl ON v.E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), rl.Log_ID)
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
			AND E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @RSA_Function_Code_Main)
		
				
	--
		
	INSERT INTO @Result (
		Platform,
		Main_Sub,
		Error_Count,
		Raise_Email_Alert,
		Raise_Pager_Alert		
	)
	SELECT
		'SP',
		'Sub',
		COUNT(Distinct E_Action_Key),
		ISNULL(MAX(rl.Raise_Email_Alert), 'N'),
		ISNULL(MAX(rl.Raise_Pager_Alert), 'N')
	FROM
		ViewAuditLogHCSP v 
		JOIN @RSA_Log rl ON v.E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), rl.Log_ID)
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
			AND E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @RSA_Function_Code_Sub)
			
			
	INSERT INTO @Result (
		Platform,
		Main_Sub,
		Error_Count,
		Raise_Email_Alert,
		Raise_Pager_Alert		
	)
	SELECT
		'VU',
		'Sub',
		COUNT(Distinct E_Action_Key),
		ISNULL(MAX(rl.Raise_Email_Alert), 'N'),
		ISNULL(MAX(rl.Raise_Pager_Alert), 'N')
	FROM
		ViewAuditLogHCVU v 
		JOIN @RSA_Log rl ON v.E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), rl.Log_ID)
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
			AND E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @RSA_Function_Code_Sub)
	

	CLOSE SYMMETRIC KEY sym_Key

	
-- =============================================
-- Return result
-- =============================================
	SELECT
		R.Platform,
		A.RSA_Version,
		R.Main_Sub,
		R.Error_Count,
		R.Raise_Email_Alert,
		R.Raise_Pager_Alert
	FROM
		@Result R
			LEFT JOIN @RSAAPIVersion A
				ON R.Main_Sub = A.Main_Sub
	WHERE
		R.Error_Count <> 0
	ORDER BY
		R.Platform,
		A.RSA_Version


END
GO

GRANT EXECUTE ON [dbo].[proc_RSAAuditLog_Check] TO HCVU
GO
