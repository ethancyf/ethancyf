IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_getActive_byCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RVPHomeList_getActive_byCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	05 Sep 2016
-- CR No.			CRE16-002
-- Description:		Revamp VSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	13 Jul 2010
-- Description:		Grant right to HCVU
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 4 Nov 2009
-- Description:	Search Active RVP Home List by Code (Record_Status = 'Y')
-- =============================================
-- =============================================
-- Modification History
-- Modified by: 
-- Modified date:
-- Description:
-- =============================================
CREATE Procedure [proc_RVPHomeList_getActive_byCode]
@rchcode varchar(10),
@RCH_Type char(5)
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
	select 
		[RCH_code],
		[Type], 
		[District], 
		[Homename_Eng], 
		[Homename_Chi], 
		[Address_Eng], 
		[Address_Chi], 
		[Record_Status]
	from 
		RVPHomeList
	where 
		RCH_Code = @rchcode AND [Record_Status] = 'Y'
		AND ((@RCH_Type IS NOT NULL AND [Type] = @RCH_Type) OR @RCH_Type IS NULL)	
END
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_getActive_byCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_getActive_byCode] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_getActive_byCode] TO WSEXT
GO