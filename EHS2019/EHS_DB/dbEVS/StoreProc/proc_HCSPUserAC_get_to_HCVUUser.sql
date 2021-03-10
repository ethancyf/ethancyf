IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_to_HCVUUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_to_HCVUUser]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	23 Feb 2021
-- CR No.			CRE20-022 (Immu Records)
-- Description:		Convert HCSP to HCVUUser model
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_get_to_HCVUUser]
	@User_ID	VARCHAR(20)
AS
-- =============================================
-- Declaration
-- =============================================
	--DECLARE @Last_Pwd_Change_Duration	INT
	--DECLARE @Last_Pwd_Change_Dtm		DATETIME
	DECLARE @SP_ID						CHAR(8)
	--DECLARE @SP_Record_Status			CHAR(1)
	DECLARE	@Token_Cnt					INT

-- =============================================
-- Initialization
-- =============================================
	SELECT 
		--@Last_Pwd_Change_Dtm = [Last_Pwd_Change_Dtm],
		@SP_ID = SP_ID
	FROM HCSPUserAC
	WHERE (SP_ID = @User_ID OR Alias_Account = @User_ID)

	--IF NOT @Last_Pwd_Change_Dtm IS NULL
	--BEGIN
	--	select @Last_Pwd_Change_Duration = DATEDIFF(dd, @Last_Pwd_Change_Dtm, GETDATE())
	--END
	--ELSE
	--BEGIN
	--	SELECT @Last_Pwd_Change_Duration = NULL
	--END

	SELECT @Token_Cnt = COUNT(1) FROM Token	WHERE [User_ID] = @SP_ID AND Record_Status = 'A'

-- =============================================
-- Return results
-- =============================================

	EXEC [proc_SymmetricKey_open]

	SELECT SPAC.[SP_ID] AS [User_ID]
		  ,[SP_Password] AS	User_Password
		  ,CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)) AS [User_Name]
		  ,[Last_Pwd_Change_Dtm]
		  ,[Last_Login_Dtm]
		  ,[Last_Unsuccess_Login_Dtm]
		  ,[Suspended] = CASE WHEN SP.Record_Status = 'S' THEN 'Y' ELSE NULL END
		  ,[Account_Locked] = CASE WHEN SPAC.Record_Status = 'S' THEN 'Y' ELSE 'N' END
		  ,[Last_Pwd_Change_Duration] = 0
		  ,[Effective_Date] ='2021-02-22 00:00:00.000'
		  ,[Expiry_Date] =NULL
		  ,@Token_Cnt AS Token_Cnt
		  ,SPAC.[TSMP]
		  ,[Force_Pwd_Change] = 'N'
		  ,[SP_Password_Level] AS Password_Level
	FROM 
		HCSPUserAC SPAC
			INNER JOIN ServiceProvider SP
				ON SPAC.[SP_ID] = SP.[SP_ID]
	WHERE 
		SPAC.[SP_ID] = @SP_ID

	EXEC [proc_SymmetricKey_close]

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_to_HCVUUser] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_to_HCVUUser] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_to_HCVUUser] TO WSEXT
GO

