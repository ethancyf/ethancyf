if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_HCSPUserACProfile_get_BySPID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_HCSPUserACProfile_get_BySPID]
GO

-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [SP_Password_Level],[SP_IVRS_Password_Level]}
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-011
-- Modified by:	Tommy TSE
-- Modified date: 27 Apr 2012
-- Description:	Select one more field Project
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Stanley CHAN
-- Modified date: 	9 Feb 2009
-- Description:	Add Print Option
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date:		26-06-2008
-- Description:		Get Profile of HCSPUserAC
-- =============================================


CREATE PROCEDURE [dbo].[proc_HCSPUserACProfile_get_BySPID]
	-- Add the parameters for the stored procedure here
	@SP_ID char(8),
	@Project char(10)
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
    Select 
		AC.SP_ID, 
		isNull(AC.Alias_Account, '') Alias_Account, 
		isNull(AC.SP_Password, '') SP_Password, 
		isNull(AC.SP_Password_Level, '') SP_Password_Level, 
		isNull(AC.SP_IVRS_Password, '') SP_IVRS_Password, 
		isNull(AC.SP_IVRS_Password_Level, '') SP_IVRS_Password_Level, 
		isNull(AC.Default_Language, '') 
		Default_Language, 
		AC.TSMP, 
		isNull(T.Token_Serial_No, '') Token_Serial_No , 
		isNull(AC.[ConsentPrintOption], '') ConsentPrintOption, 
		isNull(T.Project, '') Project
	from 
		HCSPUserAC AC, Token T
	where AC.SP_ID = @SP_ID 
		and AC.SP_ID = T.[User_ID]



END
GO

grant execute on [dbo].[proc_HCSPUserACProfile_get_BySPID] to HCSP
go







