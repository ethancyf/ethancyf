IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_get_byCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RVPHomeList_get_byCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Timothy Leung
-- Create date: 4 September 2008
-- Description:	Search the RCHE List by Rch_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by: Clark YIP
-- Modified date:	8 Sept 2009
-- Description:		Grant right to HCVU
-- =============================================
-- =============================================
-- Modification History
-- Modified by: Twinsen Chan
-- CR No.: CRE11-013
-- Modified date:	18 Oct 2012
-- Description:		Add columns - Create_By, Create_Dtm, Update_By, Update_Dtm, TSMP
-- =============================================
CREATE procedure [dbo].[proc_RVPHomeList_get_byCode]
@rchcode varchar(10)
as
begin
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
	select [RCH_code],[Type], [District], [Homename_Eng], [Homename_Chi], 
			[Address_Eng], [Address_Chi], [Record_Status], [Create_By], [Create_Dtm],
			[Update_By], [Update_Dtm], [TSMP]
	from RVPHomeList
	where RCH_Code = @rchcode
end
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_get_byCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_get_byCode] TO HCVU
GO
