IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserACList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserACList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	9 Nov 2020
-- CR No.:			CRE20-015
-- Description:		For display HA Scheme only
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get HCVUUserAC
-- =============================================

CREATE PROCEDURE dbo.proc_HCVUUserACList_get
	@Inactive_Flag		BIT,
	@UserID				VARCHAR(20) = NULL
AS
BEGIN  
	-- =============================================  
	-- Declaration  
	-- =============================================  
	DECLARE @IsOnlySSSCMC CHAR(1)

	-- =============================================  
	-- Validation   
	-- =============================================  
	-- =============================================  
	-- Initialization  
	-- =============================================  

	IF @UserID IS NULL
	BEGIN
		SET @IsOnlySSSCMC = 'N'
	END
	ELSE
	BEGIN
		SELECT 
			@IsOnlySSSCMC = 
				CASE
					WHEN COUNT(Role_Count.[Role_Type]) = 0 THEN 'N'
					WHEN
						SUM(CASE 
								WHEN Role_Count.[Role_Type] <> 23 AND Role_Count.[Role_Type] <> 24  AND Role_Count.[Role_Type] <> 25 THEN 1 
								ELSE 0 
							END) > 0 THEN 'N'
					ELSE 'Y'
				END
			--COUNT(Role_Count.[Role_Type])
		FROM
			(SELECT 
				[User_ID], Role_Type 
			FROM 
				UserRole UR 
			WHERE 
				[User_ID] = @UserID
				AND EXISTS(SELECT 1 FROM UserRole WHERE [User_ID] = UR.[User_ID] AND Role_Type IN (23,24,25)) 
			GROUP BY 
				[User_ID], Role_Type
			) Role_Count
	END

	-- =============================================
	-- Return results
	-- =============================================
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	IF @Inactive_Flag = 0 
		BEGIN
			SELECT 
				AC.[User_ID]
				,[User_Name] = CONVERT(VARCHAR(40), DecryptByKey(AC.Encrypt_Field2))
			FROM
				HCVUUserAC AC
			WHERE
				([Expiry_Date] IS NULL OR DATEDIFF(dd, GETDATE(), [Expiry_Date]) > 0)
				AND Suspended IS NULL
				AND Account_Locked = 'N'
				AND (
						@IsOnlySSSCMC = 'N' 
						OR 
						EXISTS (SELECT 1 FROM UserRole WHERE Role_Type IN (23,24,25) AND [User_ID] = AC.[User_ID])
					)
			ORDER BY [User_ID]
		END
	ELSE
		BEGIN
			SELECT 
				AC.[User_ID]
				,[User_Name] = CONVERT(varchar(40), DecryptByKey(AC.Encrypt_Field2))
			FROM 
				HCVUUserAC AC
			WHERE 
				(
					(NOT [Expiry_Date] IS NULL AND DATEDIFF(dd,GETDATE(), [Expiry_Date]) <= 0)
					or Suspended = 'Y' 
					or Account_Locked = 'Y'
				)
				AND (
						@IsOnlySSSCMC = 'N' 
						OR 
						EXISTS (SELECT 1 FROM UserRole WHERE Role_Type IN (23,24,25) AND [User_ID] = AC.[User_ID])
					)
			ORDER BY AC.[User_ID]
		END

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserACList_get] TO HCVU
GO

