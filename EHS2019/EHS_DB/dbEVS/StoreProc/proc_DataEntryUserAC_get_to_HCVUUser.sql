IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_get_to_HCVUUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_get_to_HCVUUser]
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
-- Modified date:	24 Feb 2021
-- CR No.			CRE20-022 (Immu Records)
-- Description:		Convert HCSP Data Entry to HCVUUser model
-- =============================================

CREATE procedure [dbo].[proc_DataEntryUserAC_get_to_HCVUUser]
	@SP_ID					VARCHAR(20),
	@Data_Entry_Account		VARCHAR(20),
	@HCSP_Sub_Platform		VARCHAR(20) = ''
AS

-- =============================================
-- Declaration
-- =============================================
	DECLARE @DE_SPID					CHAR(8)
	DECLARE @SP_Record_Status			CHAR(1)
	DECLARE @Token_Cnt					INT
	DECLARE @SP_Eng_Name				VARCHAR(40)

-- =============================================
-- Initialization
-- =============================================
	SELECT 
		@DE_SPID = s.SP_ID
	FROM 
		DataEntryUserAC d, HCSPUserAC s
	WHERE 
		d.Data_Entry_Account = @Data_Entry_Account
		AND d.SP_ID = s.SP_ID
		AND (s.SP_ID = @SP_ID OR s.Alias_Account = @SP_ID)

	EXEC [proc_SymmetricKey_open]

	SELECT 
		@SP_Record_Status = Record_Status
		,@SP_Eng_Name = CONVERT(VARCHAR(40), DecryptByKey(Encrypt_Field2))
	FROM ServiceProvider
	WHERE SP_ID = @DE_SPID

	EXEC [proc_SymmetricKey_close]

	select @Token_Cnt = count(1)
	from Token
	where User_ID = @DE_SPID

-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Table 1: Data Entry Account information
-- ---------------------------------------------

	SELECT 
		ED.SP_ID AS [User_ID]
		,ED.Data_Entry_Password AS [User_Password]
		,(ED.Data_Entry_Account + ' (for ' + @SP_Eng_Name + ')') AS [User_Name]
		,ED.Last_Pwd_Change_Dtm
		,ED.Last_Login_Dtm
		,ED.Last_Unsuccess_Login_Dtm
		,[Suspended] = CASE WHEN @SP_Record_Status = 'S' THEN 'Y' ELSE NULL END
		,ED.Account_Locked
		,[Last_Pwd_Change_Duration] = 0
		,[Effective_Date] ='2021-02-22 00:00:00.000'
		,[Expiry_Date] =NULL
		,@Token_Cnt	AS[Token_Cnt]
		,ED.TSMP
		,[Force_Pwd_Change] = 'N'
		,ED.[Data_Entry_Password_Level] AS Password_Level
	FROM 
		DataEntryUserAC ED
	WHERE 
		ED.Data_Entry_Account = @Data_Entry_Account
	AND ED.SP_ID = @DE_SPID

GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_get_to_HCVUUser] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_get_to_HCVUUser] TO HCVU
GO

