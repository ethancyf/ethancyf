IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordEntry_upd_Status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordEntry_upd_Status]
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
-- Author:			Koala Cheng
-- Create date:		19 May 2011
-- CR No.:			CRE11-007
-- Description:		Update DeathRecordEntry status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordEntry_upd_Status]
	@Doc_No	VARCHAR(20),
	@Record_Status CHAR(1), 
	@Remove_By VARCHAR(20) = NULL
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
	EXEC [proc_SymmetricKey_open]

-- =============================================
-- Return results
-- =============================================

	IF  @Record_Status='A'
	BEGIN  
		RAISERROR('00011', 16, 1)  
		return @@error  
	END  

	UPDATE [dbo].[DeathRecordEntry]  
	SET  
		Record_Status = @Record_Status,
		Remove_dtm = GETDATE(),
		Remove_By = @Remove_By  
	WHERE   
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)
		AND Record_Status='A'

-- =============================================
-- Finialize
-- =============================================
	EXEC [proc_SymmetricKey_close]
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordEntry_upd_Status] TO HCVU
GO
