IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_get]
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
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [Data_Entry_Password_Level]}
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	08 September 2015
-- CR No.:			INT15-0011 (Fix empty print option in HCSP Account)
-- Description:		Fix the direct join between PracticeSchemeInfo and SchemeClaim. Should add the mapping SchemeEnrolClaimMap between
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	22 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	29 July 2010
-- Description:		Get list of practices that the data entry account has
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		14-05-2008
-- Description:		Get DataEntryUserAC for login
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Stanley CHAN  
-- Modified date:   6 Feb 2009  
-- Description:  Add Print Option  
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	22 July 2009
-- Description:		If SP Chi Name: DBNull, return Empty string (work around)
-- =============================================

CREATE procedure [dbo].[proc_DataEntryUserAC_get]
@SP_ID					varchar(20),
@Data_Entry_Account		varchar(20),
@HCSP_Sub_Platform		varchar(20) = ''
as

-- =============================================
-- Declaration
-- =============================================
declare @Last_Pwd_Change_Duration	int
declare @Last_Pwd_Change_Dtm	datetime
declare @DE_SPID	char(8)
declare @SP_Record_Status	char(1)
declare @Default_Language	char(1)
declare @HCSPUserAC_Record_Status	char(1)
declare @Practice_Cnt	int
declare @Token_Cnt		int
declare @SP_Eng_Name	varchar(40)
declare @SP_Chi_Name		nvarchar(6)

-- =============================================
-- Initialization
-- =============================================
select @Last_Pwd_Change_Dtm = d.Last_Pwd_Change_Dtm
, @DE_SPID = s.SP_ID
, @Default_Language = s.Default_Language
, @HCSPUserAC_Record_Status = s.Record_Status
FROM DataEntryUserAC d, HCSPUserAC s
where d.Data_Entry_Account = @Data_Entry_Account
and d.SP_ID = s.SP_ID
and (s.SP_ID = @SP_ID or s.Alias_Account = @SP_ID)


EXEC [proc_SymmetricKey_open]

select @SP_Record_Status = Record_Status
, @SP_Eng_Name = convert(varchar(40), DecryptByKey(Encrypt_Field2))
, @SP_Chi_Name = convert(nvarchar, DecryptByKey(Encrypt_Field3))
from ServiceProvider
where SP_ID = @DE_SPID

EXEC [proc_SymmetricKey_close]

/*
select @Practice_Cnt = count(1)
from Practice
where SP_ID = @DE_SPID
and Record_Status = 'A'
*/

SELECT
	@Practice_Cnt = COUNT(DISTINCT P.Display_Seq) 
FROM
	Practice P
		INNER JOIN DataEntryPracticeMapping D
			ON P.SP_ID = D.SP_ID
				AND P.Display_Seq = D.SP_Practice_Display_Seq
				AND P.Record_Status = 'A'
		INNER JOIN PracticeSchemeInfo PS
			ON P.SP_ID = PS.SP_ID
				AND P.Display_Seq = PS.Practice_Display_Seq
				AND PS.Record_Status = 'A'
		INNER JOIN SchemeEnrolClaimMap M
			ON PS.Scheme_Code = M.Scheme_Code_Enrol
				AND M.Record_Status = 'A'
		INNER JOIN SchemeClaim SC
			ON M.Scheme_Code_Claim = SC.Scheme_Code
				AND SC.Record_Status = 'A'
WHERE
	D.SP_ID = @DE_SPID
		AND D.Data_Entry_Account = @Data_Entry_Account
		AND (SC.Available_HCSP_SubPlatform = 'ALL' OR SC.Available_HCSP_SubPlatform = @HCSP_Sub_Platform)

--

select @Token_Cnt = count(1)
from Token
where User_ID = @DE_SPID

if not @Last_Pwd_Change_Dtm is null
begin
	select @Last_Pwd_Change_Duration = datediff(dd, @Last_Pwd_Change_Dtm, getdate())
end
else
begin
	select @Last_Pwd_Change_Duration = null
end

-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Table 1: Data Entry Account information
-- ---------------------------------------------

SELECT d.SP_ID
      ,d.Data_Entry_Account
      ,d.Data_Entry_Password	User_Password
      ,d.Last_Login_Dtm
      ,d.Last_Unsuccess_Login_Dtm
      ,d.Last_Pwd_Change_Dtm
      , @SP_Eng_Name	SP_Eng_Name
	  , ISNULL(@SP_Chi_Name,'')	SP_Chi_Name
      ,@Default_Language	Default_Language
      ,@SP_Record_Status	SP_Record_Status
      ,@HCSPUserAC_Record_Status	HCSPUserAC_Record_Status
      ,d.Record_Status
      ,d.Account_Locked
      ,@Last_Pwd_Change_Duration	Last_Pwd_Change_Duration
      ,d.TSMP
      ,@Practice_Cnt	Practice_Cnt
      ,@Token_Cnt		Token_Cnt
      ,d.ConsentPrintOption
      ,d.Data_Entry_Password_Level Password_Level
FROM DataEntryUserAC d
where d.Data_Entry_Account = @Data_Entry_Account
and d.SP_ID = @DE_SPID

-- ---------------------------------------------
-- Table 2: List of practices
-- ---------------------------------------------
SELECT
	DISTINCT D.SP_Practice_Display_Seq
FROM
	Practice P
		INNER JOIN DataEntryPracticeMapping D
			ON P.SP_ID = D.SP_ID
				AND P.Display_Seq = D.SP_Practice_Display_Seq
				AND P.Record_Status = 'A'
		INNER JOIN PracticeSchemeInfo PS
			ON P.SP_ID = PS.SP_ID
				AND P.Display_Seq = PS.Practice_Display_Seq
				AND PS.Record_Status = 'A'
		INNER JOIN SchemeEnrolClaimMap M
			ON PS.Scheme_Code = M.Scheme_Code_Enrol
				AND M.Record_Status = 'A'
		INNER JOIN SchemeClaim SC
			ON M.Scheme_Code_Claim = SC.Scheme_Code
				AND SC.Record_Status = 'A'
WHERE
	D.SP_ID = @DE_SPID
		AND D.Data_Entry_Account = @Data_Entry_Account
		AND (SC.Available_HCSP_SubPlatform = 'ALL' OR SC.Available_HCSP_SubPlatform = @HCSP_Sub_Platform)


GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_get] TO HCSP
GO
