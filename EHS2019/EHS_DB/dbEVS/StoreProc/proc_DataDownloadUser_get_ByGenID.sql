IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataDownloadUser_get_ByGenID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataDownloadUser_get_ByGenID]
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
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		03 Jul 2008
-- Description:		Retrieve accessable user on this DataDownload
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 September 2009
-- Description:		Add criteria on Scheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	20 October 2009
-- Description:		Handle the cases that @Scheme_Code does not exist in the [FileGenerationQueue].[In_Parm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 October 2009
-- Description:		Only handle the logic on filtering scheme code when the [FileGenerationQueue].[File_ID] is BANK or SUPER
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	28 October 2009
-- Description:		Remove the checking of File_ID = Bank or Super
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	01 Dec 2009
-- Description:		Join with table "SchemeFileGeneration" to determine
--					the file belongs to which scheme
-- =============================================

CREATE PROCEDURE [dbo].[proc_DataDownloadUser_get_ByGenID]
	@Generation_ID	char(12)		
WITH RECOMPILE
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Scheme_Code	char(10)
	DECLARE @Target_Parm	varchar(100)
	DECLARE @Target_Value	varchar(255)
	DECLARE @File_ID		varchar(30)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SELECT @File_ID = [File_ID] FROM FileGenerationQueue WHERE Generation_ID = @Generation_ID
	
	-- Get the Scheme_Code from the [FileGenerationQueue].[In_Parm]
	IF LTRIM(RTRIM(ISNULL(CONVERT(varchar(MAX), (SELECT In_Parm FROM FileGenerationQueue WHERE Generation_ID = @Generation_ID)), ''))) <> '' BEGIN
	
		SELECT @Target_Parm = '@scheme_code'
		
		IF (SELECT PATINDEX('%' + @Target_Parm + '%', In_Parm) FROM FileGenerationQueue WHERE Generation_ID = @Generation_ID) <> 0 BEGIN

			SELECT 
				@Target_Value =
					RIGHT(CONVERT(varchar(MAX), In_Parm), LEN(CONVERT(varchar(MAX), In_Parm)) - (PATINDEX('%' + @Target_Parm + '%', In_Parm) - 1))
			FROM 
				FileGenerationQueue 
			WHERE
				Generation_ID = @Generation_ID

			IF PATINDEX('%|||%', @Target_Value) <> 0 BEGIN
				SELECT @Target_Value = LEFT(@Target_Value, PATINDEX('%|||%', @Target_Value) - 1)
			END

			SELECT @Target_Value = RIGHT(@Target_Value, PATINDEX('%;;;%', REVERSE(@Target_Value)) - 1)

			SELECT @Scheme_Code = @Target_Value
		
		END
		
	END

	EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	SELECT 
		DISTINCT
			FGQ.Generation_ID,
			UR.User_ID, 
			CONVERT(varchar(40), DecryptByKey(UAC.[Encrypt_Field2])) AS [User_Name]	
		
	FROM FileGenerationQueue FGQ
		INNER JOIN FileGeneration FG
			ON FGQ.File_ID = FG.File_ID
		INNER JOIN SchemeFileGeneration SFG
			on SFG.File_ID = FG.File_ID
		LEFT JOIN RoleTypeFileGeneration RTFG
			ON RTFG.File_ID = FG.File_ID
		LEFT JOIN UserRole UR
			ON UR.Role_Type = RTFG.Role_Type
		LEFT JOIN HCVUUserAC UAC
			ON UR.User_ID = UAC.User_ID

	WHERE 
		FGQ.Generation_ID = @Generation_ID 
			AND (	FG.Is_SelfAccess = 'N' 
					OR (FG.Is_SelfAccess = 'Y' AND UR.User_ID = FGQ.Request_By)
				)	
			AND (RTFG.Access_Type = 'A' OR RTFG.Access_Type = 'D') 
			AND UAC.User_ID IS NOT NULL
			AND	(@Scheme_Code IS NULL
					OR UR.Scheme_Code = @Scheme_Code)
			AND (SFG.Scheme_Code = 'ALL' or SFG.Scheme_Code = UR.Scheme_Code)
	
	EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_DataDownloadUser_get_ByGenID] TO HCVU
GO
