 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUser_get_byRoleType]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUser_get_byRoleType]
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
-- Author:		Paul YIP
-- Create date: 13 Oct 2010
-- Description:	Retrieve accessable user by role type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Golden Yang
-- Modified date:	11 Jun 2019
-- Description:		Add User Name, Chinese Name ,Contact No, Gender
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCVUUser_get_byRoleType]
	@intRoleType as integer
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
-- =============================================
-- Return results
-- =============================================

EXEC [proc_SymmetricKey_open]

 SELECT distinct ur.[User_ID]
 , convert(varchar(40), DecryptByKey(Encrypt_Field2)) AS [User_Name]
  , convert(varchar(40), DecryptByKey(Encrypt_Field3)) AS [ChineseName]
  , Sex AS [Gender]
  , Contact_No
  , ISNULL(SD.Data_Value,'') AS [Gender_Title]
  --, Contact_No ContactNo
  --, ur.[User_ID] + ' - ' + 
  --case when Sex is null then 'Mr.' else 
  --case when Sex ='M' then 'Mr.' 
  --when Sex ='F' then 'Ms.' 
  --else 'Mr.' end end + ' '+ convert(varchar(40), DecryptByKey(Encrypt_Field2))  as MergeField_Text
  --,ur.[User_ID] + '-' + Contact_No  as MergeField_Value
 FROM [UserRole] ur
 inner join HCVUUserAC usr on ur.[User_ID] = usr.[User_ID]
 LEFT JOIN StaticData SD
		ON Column_Name = 'GenderTitle' AND ISNULL(usr.Sex,'') = SD.Item_No

 WHERE[Role_Type] = @intRoleType

 EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_HCVUUser_get_byRoleType] TO HCVU
GO
