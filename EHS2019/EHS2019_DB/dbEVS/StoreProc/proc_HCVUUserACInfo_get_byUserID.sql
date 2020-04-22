
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_HCVUUserACInfo_get_byUserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_HCVUUserACInfo_get_byUserID]
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	22 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Add [User_Password_Level] field
-- =============================================

-- =============================================
-- Author:			Billy Lam
-- Create date:		09-06-2008
-- Description:		Get HCVUUserAC Info by User_ID
--					for user account main
-- =============================================

CREATE Procedure [dbo].[proc_HCVUUserACInfo_get_byUserID]
@User_ID			char(20)
as

-- =============================================
-- Return results
-- =============================================

OPEN SYMMETRIC KEY sym_Key
DECRYPTION BY ASYMMETRIC KEY asym_Key

select User_ID
, convert(varchar(40), DecryptByKey(Encrypt_Field2))	User_Name
, convert(varchar, DecryptByKey(Encrypt_Field1))	HKID
, Effective_Date
, Expiry_Date
, Suspended
, Account_Locked 
, User_Password_Level
, tsmp
from HCVUUserAC
where User_ID = @User_ID
order by User_ID

CLOSE SYMMETRIC KEY sym_Key


GO

GRANT EXECUTE ON [proc_HCVUUserACInfo_get_byUserID] TO HCVU
GO