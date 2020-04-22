IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserACList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserACList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get HCVUUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure dbo.proc_HCVUUserACList_get
@inactive_flag		bit
as

-- =============================================
-- Return results
-- =============================================
OPEN SYMMETRIC KEY sym_Key
DECRYPTION BY ASYMMETRIC KEY asym_Key

if @inactive_flag = 0 
begin

select User_ID
, convert(varchar(40), DecryptByKey(Encrypt_Field2))	User_Name 
from HCVUUserAC
where (Expiry_Date is null or datediff(dd,getdate(), Expiry_Date) > 0)
and Suspended is null and Account_Locked = 'N'
order by User_ID

end
else
begin

select User_ID
, convert(varchar(40), DecryptByKey(Encrypt_Field2))	User_Name 
from HCVUUserAC
where ((not Expiry_Date is null and datediff(dd,getdate(), Expiry_Date) <= 0)
or Suspended = 'Y' or Account_Locked = 'Y')
order by User_ID

end

CLOSE SYMMETRIC KEY sym_Key

GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserACList_get] TO HCVU
GO
