IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VouhcerUsed_get_byHKIDSchemeCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VouhcerUsed_get_byHKIDSchemeCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Koala CHENG
-- Modified date:	06 Jan 2021
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
--					Obsolete SProc
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 05 May 2008
-- Description:	Get the no. of vouhcer use for specific HKID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
--CREATE PROCEDURE [dbo].[proc_VouhcerUsed_get_byHKIDSchemeCode] 
--	@Scheme_Code char(10),
--	@HKID char(9)
--as
--BEGIN
--	SET NOCOUNT ON;
---- =============================================
---- Declaration
---- =============================================
---- =============================================
---- Validation 
---- =============================================
---- =============================================
---- Initialization
---- =============================================
---- =============================================
---- Return results
---- =============================================
--OPEN SYMMETRIC KEY sym_Key 
--	DECRYPTION BY ASYMMETRIC KEY asym_Key

--	select 
--		convert(varchar, DecryptByKey(P.Encrypt_Field1)) as HKID, 
--		VA.Voucher_Used, VA.TSMP as VATSMP, P.TSMP as PITSMP
--	from dbo.TempVoucherAccount VA, dbo.TempPersonalInformation P
--	where 
--		P.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID) and 
--		VA.Voucher_Acc_ID = P.Voucher_Acc_ID and 
--		VA.Scheme_Code = @Scheme_Code

--CLOSE SYMMETRIC KEY sym_Key
--END
--GO

--GRANT EXECUTE ON [dbo].[proc_VouhcerUsed_get_byHKIDSchemeCode] TO HCPUBLIC
--GO

--GRANT EXECUTE ON [dbo].[proc_VouhcerUsed_get_byHKIDSchemeCode] TO HCSP
--GO
