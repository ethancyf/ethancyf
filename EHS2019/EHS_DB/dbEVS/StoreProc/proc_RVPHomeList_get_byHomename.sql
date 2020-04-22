IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_get_byHomename]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RVPHomeList_get_byHomename]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
create procedure proc_RVPHomeList_get_byHomename
@homename nvarchar(255),
@RCH_Type char(5)
as 
begin
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	29 Aug 2016
-- CR No.			CRE16-002
-- Description:		Revamp VSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	13 Jul 2010
-- Description:		Grant right to HCVU
-- =============================================
-- =============================================
-- Author:		Timothy Leung
-- Create date: 4 September 2008
-- Description:	Search the RCHE List by homename
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	4 Nov 2009
-- Description:		Record Status with 'Y'
-- =============================================

	DECLARE @IN_HomeName NVARCHAR(255)

	SET @IN_HomeName = '%' + @homename + '%'

	SELECT 
		[RCH_code],
		[Type], 
		[District], 
		[Homename_Eng], 
		[Homename_Chi], 
		[Address_Eng], 
		[Address_Chi], 
		[Record_Status]
	FROM RVPHomeList
	WHERE 
		([Homename_Eng] like @IN_HomeName or [Homename_Chi] like @IN_HomeName)
		AND [Record_Status] = 'Y'
		AND ((@RCH_Type IS NOT NULL AND [Type] = @RCH_Type) OR @RCH_Type IS NULL)	
END
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_get_byHomename] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_get_byHomename] TO HCVU
GO