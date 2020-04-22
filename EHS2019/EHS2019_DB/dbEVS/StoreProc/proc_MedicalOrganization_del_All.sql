IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganization_del_All]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganization_del_All]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 07 Jul 2009
-- Description:	Delete All MedicalOrganization Record of the given ERN
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	29 Aug 2009
-- Description:		Filter out those Delisted MO will not be deleted when doing data migration
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganization_del_All]
	@sp_id char(8)
	
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

DELETE FROM [dbo].[MedicalOrganization]

WHERE 
	sp_id = @sp_id and Record_Status<>'D'
END

GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_del_All] TO HCVU
GO
