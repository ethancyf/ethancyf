IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryPracticeMapping_delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryPracticeMapping_delete]
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
CREATE PROCEDURE [dbo].[proc_DataEntryPracticeMapping_delete]
	@SP_ID char(8),
	@Data_Entry_Account varchar(20)
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
    Delete from DataEntryPracticeMapping
	where 
	SP_ID = @SP_ID and
	Data_Entry_Account = @Data_Entry_Account 	
	and SP_ID + convert(varchar(3),SP_Practice_Display_Seq) in
	(select SP_ID + convert(varchar(3),Display_Seq)
	from Practice where SP_ID = @SP_ID and Record_status = 'A')
END
GO

GRANT EXECUTE ON [dbo].[proc_DataEntryPracticeMapping_delete] TO HCSP
GO
