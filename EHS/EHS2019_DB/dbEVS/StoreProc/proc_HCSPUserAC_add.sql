IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Jun 2017
-- Description:		Add field "Activation_Code_Level"
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 25 June 2008
-- Description:	Insert the User account to Table HCSPUSerAC
--				Create Person = Update Person
--				Record Status = P (Pending)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho Lee
-- Modified date:	01 Mar 2008
-- Description:		Add ConsentPrintOption Default Value
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSPUserAC_add]
	@SP_ID char(8), 
	@Activation_Code varchar(100),	
	@Create_By varchar(20),
	@Activation_Code_Level INT
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


-- Default Value of ConsentPrintOption
Declare @printOption as char(1)

Select @printOption = ISNULL(parm_Value1,'') from SystemParameters
Where Parameter_Name = 'DefaultConsentPrintOption' And Record_Status = 'A' AND [Scheme_Code] = 'ALL'

IF RTRIM(LTRIM(@printOption)) = ''
begin
	SET @printOption = 'A'
end


	
	INSERT INTO HCSPUserAC
				(SP_ID,
				 Activation_Code,				 
				 Create_Dtm,
				 Create_By,
				 Update_Dtm,
				 Update_By,
				 Record_Status,
				 ConsentPrintOption,
				 Activation_Code_Level
				)
	VALUES		(@SP_ID,
				 @Activation_Code,
				 getdate(),
				 @Create_By,
				 getdate(),
				 @Create_By,
				 'P',
				 @printOption,
				 @Activation_Code_Level
				)

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_add] TO HCVU
GO
