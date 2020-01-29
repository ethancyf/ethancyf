IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_getAll_BySPIDPracticeID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_getAll_BySPIDPracticeID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 30 Sep 2009
-- Description:	Retrieve all Data Entry Account by SPID and Practice ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_DataEntryUserAC_getAll_BySPIDPracticeID]
	@SP_ID char(8),
	@SP_Practice_Display_Seq smallint
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

select distinct d.SP_ID, d.Data_Entry_Account  
from DataEntryUserAC d, DataEntryPracticeMapping p  
where d.SP_ID = @SP_ID  
and d.SP_ID = p.SP_ID  
and d.Data_Entry_Account = p.Data_Entry_Account  
and (@SP_Practice_Display_Seq is null or p.SP_Practice_Display_Seq = @SP_Practice_Display_Seq)  
order by d.Data_Entry_Account  

END
GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_getAll_BySPIDPracticeID] TO HCSP
GO
