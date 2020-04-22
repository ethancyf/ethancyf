IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_get_ForExternalUse]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_get_ForExternalUse]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	07 Mar 2016
-- CR No.			CRE15-022
-- Description:		1. Include 'R' status for [External_Use]
--					2. Return [External_Use] column
--					3. Remove [Numeric_Only] column
-- =============================================
-- =============================================
-- Author:		Stanley Chan
-- Create date: 10 Sep 2008
-- Description:	Retrieve all Active System Parameters
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemParameters_get_ForExternalUse]
AS
BEGIN

	Select 
		[Parameter_Name],
		[Parm_Value1],
		[Parm_Value2],
		[Description],
		[Category],
		[Upper_Limit],
		[Lower_Limit],
		[Apply_Limit],
		[External_Use],
		[TSMP],
		[Scheme_Code]
	from 
		SystemParameters
	where 
		Record_Status = 'A'
		and [External_Use] IN ('Y','R')
	Order by [Category]

END
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_ForExternalUse] TO HCVU
GO
