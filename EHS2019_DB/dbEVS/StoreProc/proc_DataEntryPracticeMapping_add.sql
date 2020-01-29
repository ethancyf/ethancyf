IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryPracticeMapping_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryPracticeMapping_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 23 Jun 2008
-- Description:	Add Data Entry Account and Practice Mapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_DataEntryPracticeMapping_add]
	@SP_ID char(8),
	@Data_Entry_Account varchar(20),
	@SP_Practice_Display_Seq smallint,
	@SP_Bank_Acc_Display_Seq smallint
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
    insert into DataEntryPracticeMapping
	(
	SP_ID,
	Data_Entry_Account,
	SP_Practice_Display_Seq,
	SP_Bank_Acc_Display_Seq
	)
	values
	(
	@SP_ID,
	@Data_Entry_Account,
	@SP_Practice_Display_Seq,
	@SP_Bank_Acc_Display_Seq
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_DataEntryPracticeMapping_add] TO HCSP
GO
