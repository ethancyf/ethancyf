IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHSAccountTSMP_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHSAccountTSMP_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:	Karl LAM
-- Modified date:	15 Aug 2013
-- Description:	Grant execute permission to HCVU
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 09 Oct 2009
-- Description:	Add EHSAccountTSMP while Adding Transaction
-- =============================================

CREATE PROCEDURE [proc_EHSAccountTSMP_add]
	@Doc_Code char(20),
	@identity varchar(20),
	@update_by varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Validation 
-- =============================================

	IF (SELECT Count(*) FROM [EHSAccountTSMP] WHERE [Doc_Code] = @Doc_Code AND Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) ) > 0
	BEGIN
		Raiserror('00011', 16, 1)
		RETURN @@error
	END	

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	INSERT [EHSAccountTSMP] ([Encrypt_Field1], [Doc_Code], [Update_Dtm], [Update_By])
	VALUES (EncryptByKey(KEY_GUID('sym_Key'), @identity), @Doc_Code, GetDate(), @update_by)
	
CLOSE SYMMETRIC KEY sym_Key
		
END
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EHSAccountTSMP_add] TO WSEXT
Go
