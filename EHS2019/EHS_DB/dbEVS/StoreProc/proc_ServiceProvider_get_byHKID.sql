IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_byHKID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_byHKID]
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
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 October 2016
-- CR No.			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		(1) Grant to WSEXT
--					(2) Retrieve Record_Status
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Koala CHENG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 12 June 2008
-- Description:	Retrieve the SP ID and Name
--				from Table "ServiceProvider" by HKID
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_byHKID]
	@hkid	char(9)
AS
BEGIN
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

SELECT	SP_ID, 
		convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
		convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
		isNULL(convert(nvarchar, DecryptByKey(Encrypt_Field3)),'') as SP_Chi_Name,
		Record_Status
FROM	ServiceProvider
WHERE	EncryptByKey(KEY_GUID('sym_Key'), @hkid) = Encrypt_Field1

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byHKID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byHKID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byHKID] TO WSINT
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byHKID] TO WSEXT
GO
