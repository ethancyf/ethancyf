IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalPermanent_upd_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalPermanent_upd_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tommy Cheung
-- Create date: 18 Sept 2008
-- Description:	Update Professional Status 
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalPermanent_upd_status]
	@sp_id char(15),
	@professional_seq smallint,
	@record_status char(1)
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

	UPDATE	Professional
	SET		Record_Status = @record_status			
	WHERE	SP_ID = @sp_id and
			Professional_Seq = @Professional_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalPermanent_upd_status] TO HCVU
GO
