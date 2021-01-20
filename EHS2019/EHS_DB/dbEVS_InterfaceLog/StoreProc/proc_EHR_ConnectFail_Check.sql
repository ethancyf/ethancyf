IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHR_ConnectFail_Check]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHR_ConnectFail_Check]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		22 Feb 2017
-- CR No.:			CRE16-019
-- Description:		Check eHR connect fail
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHR_ConnectFail_Check]
	@Start_Dtm		datetime,
	@End_Dtm		datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Master table
-- =============================================
	
	DECLARE @EHR_Log TABLE (
		Function_Code		CHAR(6),
		Log_ID				CHAR(5),
		Site				CHAR(5)
	)

	DECLARE @Platform TABLE (
		PlatformCode		CHAR(2),
		Platform			CHAR(10)
	)
	
	INSERT INTO @Platform (PlatformCode, Platform) VALUES 	
	('01','VU'), ('02','SP')
	
	INSERT INTO @EHR_Log (Function_Code, Log_ID, Site) VALUES 
	('070302', '00101', 'PRI')  -- Application Connect Fail (Primary)
	,('070302', '00102', 'ALL')  -- Application Connect Fail (All Endpoints)


-- =============================================
-- Declaration 
-- =============================================

	-- Application Connect Fail (Primary)
	DECLARE @Result_PRI table (
		Description			NVARCHAR(MAX),
		Site				CHAR(5),	-- e.g. DC1
		PlatformCode		CHAR(2)		-- e.g. 01
	)

	-- Application Connect Fail (All Endpoints)
	DECLARE @Result_ALL table (
		E_Action_Key		VARBINARY(60)
	)


	EXEC [proc_SymmetricKey_open]


-- =============================================
-- Retrieve data
-- =============================================	
	INSERT INTO @Result_All (
		E_Action_Key
	)
	SELECT
		E_Action_Key
	FROM
		ViewAuditLogInterface v 
		JOIN @EHR_Log rl ON v.E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), rl.Function_Code) 
							AND v.E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), rl.Log_ID)
							AND rl.Site = 'ALL'
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
	
		
	INSERT INTO @Result_PRI (
		Description
	)
	SELECT
		CONVERT(nvarchar(MAX), DecryptByKey(v.E_Description)) AS [Description]
	FROM
		ViewAuditLogInterface v 
		JOIN @EHR_Log rl ON v.E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), rl.Function_Code) 
							AND v.E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), rl.Log_ID)
							AND rl.Site = 'PRI'
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
		AND NOT EXISTS (
						SELECT 1 
						FROM @Result_All RA
						WHERE RA.E_Action_Key = v.E_Action_Key
					) -- Exclude all end point fail for same action key
		
		
	EXEC [proc_SymmetricKey_close]


	-- Extract Data
	UPDATE @Result_PRI
	SET 
		Site = LTRIM(RTRIM(SUBSTRING( LEFT(Description, CHARINDEX('fail', Description) - 1), CHARINDEX('DC', Description), LEN(Description)))),
		PlatformCode = LTRIM(RTRIM(SUBSTRING( LEFT(Description, CHARINDEX('platform', Description) -1), CHARINDEX('in', Description) + LEN('in') + 1, LEN(Description))))

-- =============================================
-- Return result
-- =============================================
	-- Primary
	SELECT
		COUNT(1) AS [Error_Count],
		R.Site,
		ISNULL(P.Platform, R.PlatformCode) AS [Platform]
	FROM
		@Result_PRI R
	LEFT JOIN
		@Platform P ON R.PlatformCode = P.PlatformCode
	GROUP BY 
		R.Site, R.PlatformCode, P.Platform
	HAVING 
		COUNT(1) > 0
	ORDER BY
		R.Site,
		R.PlatformCode

	-- All Endpoint
	SELECT
		COUNT(1) AS [Error_Count]
	FROM
		@Result_All R
	HAVING 
		COUNT(1) > 0
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHR_ConnectFail_Check] TO WSINT
GO

