IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DHCSPMapping_get_byRegCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DHCSPMapping_get_byRegCode]
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
-- CR No.:		CRE19-006 (DHC)
-- Author:		Winnie SUEN
-- Create date: 20 Jun 2019
-- Description:	Check SP exist in DHCSPMapping by Profession and Reg No.
-- =============================================

CREATE PROCEDURE proc_DHCSPMapping_get_byRegCode
	@Service_Category_Code	char(5),
	@Registration_Code		varchar(15)
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
		COUNT(1) AS [rowcount] 
	FROM
		DHCSPMapping
	WHERE
		Service_Category_Code = @Service_Category_Code
		AND Registration_Code = @Registration_Code
	
	
END
GO

GRANT EXECUTE ON [dbo].[proc_DHCSPMapping_get_byRegCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_DHCSPMapping_get_byRegCode] TO HCVU
GO
