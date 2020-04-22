IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_getAll_BySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_getAll_BySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 20 Jun 2008
-- Description:	Retrieve all Data Entry Account by SPID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_DataEntryUserAC_getAll_BySPID]
	@SP_ID char(8)
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
    select SP_ID, Data_Entry_Account, Record_Status, TSMP
	from DataEntryUserAC
	where SP_ID = @SP_ID
	Order by Data_Entry_Account

END

GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_getAll_BySPID] TO HCSP
GO
