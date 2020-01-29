IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ERNProcessed_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ERNProcessed_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Author:			Kathy LEE
-- Create date:		02 Jun 2009
-- Description:		Get the enrolment list by SPID in Permament
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	03 July 2009
-- Description:		1. Remove suffix, mo_dispaly_seq, scheme_code, update_by, update_dtm
--					2. Add Sub_Enrolment_Ref_No
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE Procedure [dbo].[proc_ERNProcessed_get_bySPID]
	@sp_id char(8)
as
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

	Select  Enrolment_Ref_No,
			SP_ID,
			create_by,
			create_dtm,
			Sub_Enrolment_Ref_No
	from	ERNProcessed
	where	SP_ID = @sp_id
END
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessed_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessed_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessed_get_bySPID] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessed_get_bySPID] TO WSINT
GO