IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordMatchResult_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordMatchResult_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Koala Cheng
-- Create date:		13 June 2011
-- CR No.:			CRE11-007
-- Description:		Delete DeathRecordMatchResult
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordMatchResult_del]
	@Doc_No	VARCHAR(20),
	@Remove_By CHAR(1) = NULL
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

-- =============================================
-- Return results
-- =============================================

	-- Update remove detail for fire Trigger to log changes
	UPDATE [dbo].[DeathRecordMatchResult]  
	SET  
		Remove_dtm = GETDATE(),
		Remove_By = @Remove_By  
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)

	-- Physical delete record for better performance when user enquiry
	DELETE [dbo].[DeathRecordMatchResult] 
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)

-- =============================================
-- Finialize
-- =============================================
	CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordMatchResult_del] TO HCVU
GO
