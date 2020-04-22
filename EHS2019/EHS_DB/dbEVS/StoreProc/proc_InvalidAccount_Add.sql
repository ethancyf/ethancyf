IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InvalidAccount_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InvalidAccount_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
	
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		25 March 2010
-- Description:		Add invalid account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_InvalidAccount_Add]
	@Invalid_Acc_ID		char(15),
	@Scheme_Code		char(10),
	@Record_Status		char(1),
	@Account_Purpose	char(1),
	@Original_Acc_ID	char(15),
	@Count_Benefit		char(1),
	@Create_Dtm			datetime,
	@Create_By			varchar(20),
	@Update_Dtm			datetime,
	@Update_By			varchar(20),
	@Original_Acc_Type	char(1)
AS BEGIN

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
-- Process data
-- =============================================
	INSERT INTO InvalidAccount (
		Invalid_Acc_ID,
		Scheme_Code,
		Record_Status,
		Account_Purpose,
		Original_Acc_ID,
		Count_Benefit,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Original_Acc_Type
	) VALUES (
		@Invalid_Acc_ID,
		@Scheme_Code,
		@Record_Status,
		@Account_Purpose,
		@Original_Acc_ID,
		@Count_Benefit,
		@Create_Dtm,
		@Create_By,
		@Update_Dtm,
		@Update_By,
		@Original_Acc_Type
	)
	

END
GO

GRANT EXECUTE ON [dbo].[proc_InvalidAccount_Add] TO HCVU
GO
