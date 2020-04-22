IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganization_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganization_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Lawrence TSANG
-- Create date: 3 Jun 2009
-- Description:	Update MedicalOrganization Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganization_upd_RecordStatus]
	@sp_id			char(8), 
	@display_seq	smallint, 
	@record_status	char(1), 
	@update_by		varchar(20),
	@tsmp			timestamp
	
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF	(	
		SELECT	TSMP 
		FROM	[MedicalOrganization]
		WHERE	SP_ID = @sp_id 
					AND Display_Seq = @display_seq
		) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE	[MedicalOrganization]
	SET		Record_Status = @record_status,
			Update_Dtm = GETDATE(),
			Update_By = @update_by
	WHERE	SP_ID = @sp_id 
				AND Display_Seq = @display_seq
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_upd_RecordStatus] TO HCVU
GO
