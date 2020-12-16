IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HAServiceSubSpecialitiesMapping_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HAServiceSubSpecialitiesMapping_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO 

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- CR No.:		CRE20-015-10 (HA Scheme)
-- Author:		Winnie SUEN
-- Create date: 04 Dec 2020
-- Description:	get all recrod from [HAServiceSubSpecialitiesMapping]
-- =============================================

CREATE PROCEDURE proc_HAServiceSubSpecialitiesMapping_get
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

	SELECT
		[SubSpecialities_Code],
		[Practice_Display_Seq],
		[Display_Seq],
		[Name],
		[Name_CHI],
		[Name_CN],
		[Record_Status]
	FROM 
		[HAServiceSubSpecialitiesMapping]
	ORDER BY
		Practice_Display_Seq ASC, 
		Display_Seq ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_HAServiceSubSpecialitiesMapping_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HAServiceSubSpecialitiesMapping_get] TO HCVU
GO
