IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [SP_Password_Level],{SP_IVRS_Password_Level]}
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 October 2016
-- CR No.			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Grant to WSEXT
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Tommy Cheung
-- Modified date: 10-11-2008
-- Description:	field [SP_IVRS_Password], [IVRS_Locked] is added 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Stanley CHAN
-- Modified date: 	6 Feb 2009
-- Description:	Add print option
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_get]
@User_ID			varchar(20)
as
-- =============================================
-- Declaration
-- =============================================
declare @Last_Pwd_Change_Duration	int
declare @Last_Pwd_Change_Dtm	datetime
declare @SP_ID					char(8)
declare @SP_Record_Status	char(1)
declare	@Token_Cnt			int

-- =============================================
-- Initialization
-- =============================================
select @Last_Pwd_Change_Dtm = [Last_Pwd_Change_Dtm]
, @SP_ID = SP_ID
FROM HCSPUserAC
where (SP_ID = @User_ID or Alias_Account = @User_ID)

if not @Last_Pwd_Change_Dtm is null
begin
	select @Last_Pwd_Change_Duration = datediff(dd, @Last_Pwd_Change_Dtm, getdate())
end
else
begin
	select @Last_Pwd_Change_Duration = null
end

select @SP_Record_Status = Record_Status
from ServiceProvider
where SP_ID = @SP_ID


select @Token_Cnt = count(1)
from Token
where User_ID = @SP_ID
and Record_Status = 'A'

-- =============================================
-- Return results
-- =============================================
SELECT [SP_ID]
      ,[SP_Password]	User_Password
      ,[Alias_Account]
      ,[Last_Login_Dtm]
      ,[Last_Unsuccess_Login_Dtm]
      ,[Last_Pwd_Change_Dtm]
      ,[Default_Language]
      ,[Record_Status]
      ,@Last_Pwd_Change_Duration	Last_Pwd_Change_Duration
      ,@SP_Record_Status	SP_Record_Status
      ,@Token_Cnt			Token_Cnt
      ,[TSMP]
      ,[SP_IVRS_Password]
      ,[ConsentPrintOption]
      , isNull([IVRS_Locked],'') IVRS_Locked
      ,[SP_Password_Level]	Password_Level
      ,[SP_IVRS_Password_Level]	IVRS_Password_Level
  FROM HCSPUserAC
where (SP_ID = @SP_ID)

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get] TO WSEXT
GO
