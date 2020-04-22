IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_by_UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_by_UserLogin]
GO


-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 January 2016
-- Description:		Stored procedure is not used anymore
-- =============================================



SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
--GO

/*

CREATE Procedure [dbo].[proc_ServiceProvider_get_by_UserLogin]
@user_login	char(20)
, @user_password	varchar(20)
as

--RAISERROR ('02000101', 16, 1, '')

select u.System_ID
, u.User_Type
, u.user_Description
, u.Alias_Account
, u.Last_Login_Dtm
, u.Last_Unsuccess_Login_Dtm
, s.SP_ID
, s.Enrolment_Ref_No
, s.SP_HKID
, s.SP_Eng_Name
, s.SP_Chi_Name
, s.Fax
, s.Email
, s.Record_Status
, s.Remark
, s.Join_HA_PPI
, s.UnderModification
from UserAC u, ServiceProvider s
where (u.System_ID = @user_login or u.Alias_Account = @user_login)
and u.User_Password = @user_password
and u.System_ID = s.SP_ID

--GO
*/
