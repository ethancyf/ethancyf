IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AccountChangeMaintenance_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AccountChangeMaintenance_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Cheung
-- Create date:		17-06-2008
-- Description:		Update Account Change Maintenance Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 May 2009
-- Description:		Add @Scheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_AccountChangeMaintenance_upd]
	@Update_By					char(20),
	@SP_ID						char(8),
	@Upd_Type					varchar(2),
	@System_Dtm					datetime,
	@Record_Status				char(1),
	@TSMP						binary(8),
	@SP_Practice_Display_Seq	smallint,
	@Scheme_Code				char(10)
AS BEGIN

-- =============================================
-- Validation 
-- =============================================
	IF	(	
		SELECT	TSMP
		FROM	SPAccountMaintenance
		WHERE	SP_ID = @SP_ID 
					AND Scheme_Code = @Scheme_Code
					AND Upd_Type = @Upd_Type 
					AND System_Dtm = @System_Dtm
					AND (@SP_Practice_Display_Seq IS NULL 
							OR SP_Practice_Display_Seq = @SP_Practice_Display_Seq)
		) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

	UPDATE	SPAccountMaintenance
	SET		Record_Status = @Record_Status,
			Confirmed_By = @Update_by,
			Update_By = @Update_by,
			Confirm_Dtm = GETDATE()
	WHERE	SP_ID = @SP_ID
				AND Scheme_Code = @Scheme_Code
				AND Upd_Type = @Upd_Type
				AND System_Dtm = @System_Dtm
				AND (@SP_Practice_Display_Seq IS NULL
						OR SP_Practice_Display_Seq = @SP_Practice_Display_Seq)

END
GO

GRANT EXECUTE ON [dbo].[proc_AccountChangeMaintenance_upd] TO HCVU
GO
