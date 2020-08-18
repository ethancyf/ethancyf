
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_HCVUUserACInfo_get_byUserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_HCVUUserACInfo_get_byUserID]
GO

-- =============================================  
-- Modification History  
-- Modified by:  Golden Yang  
-- Modified date: 12 Jun 2020  
-- CR No.    CRE19-022 - Inspection  
-- Description:  Add Gender,ChineseName,ContactNo field  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Dickson Law  
-- Modified date: 22 Jun 2017  
-- CR No.   I-CRE16-007-02 (Refine system from CheckMarx findings)  
-- Description:  Add [User_Password_Level] field  
-- =============================================  
  
-- =============================================  
-- Author:   Billy Lam  
-- Create date:  09-06-2008  
-- Description:  Get HCVUUserAC Info by User_ID  
--     for user account main  
-- =============================================  
  
CREATE Procedure [dbo].[proc_HCVUUserACInfo_get_byUserID]  
@User_ID   char(20)  
as  
  
-- =============================================  
-- Return results  
-- =============================================  
  
OPEN SYMMETRIC KEY sym_Key  
DECRYPTION BY ASYMMETRIC KEY asym_Key  
  
select User_ID  
, convert(varchar(40), DecryptByKey(Encrypt_Field2)) User_Name  
, convert(varchar, DecryptByKey(Encrypt_Field1)) HKID  
, convert(nvarchar(100), DecryptByKey(Encrypt_Field3)) Chinese_Name  
, Sex AS [Gender]
, Contact_No  
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