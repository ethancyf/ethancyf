IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationPermanent_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationPermanent_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		26 May 2009
-- Description:		Update SchemeInformation (used in Account Change Confirmation)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	27 July 2009
-- Description:		Add timestamp checking
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationPermanent_upd_RecordStatus]
	@SP_ID			char(8),
	@Scheme_Code	char(10),
	@Record_Status	char(1),
	@Delist_Status	char(1),
	@Remark			nvarchar(255),
	@Update_By		varchar(20),
	@TSMP			timestamp
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF	(
		SELECT	TSMP 
		FROM	SchemeInformation
		WHERE	SP_ID = @SP_ID 
					AND Scheme_Code = @Scheme_Code
		) != @TSMP
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
	UPDATE	SchemeInformation
	SET		Record_Status = @Record_Status, 
			Remark = @Remark,
			Update_By = @Update_By, 
			Update_Dtm = GETDATE()
	WHERE	SP_ID = @SP_ID
				AND Scheme_Code = @Scheme_Code

	IF @Record_Status = 'D' BEGIN
		UPDATE	SchemeInformation
		SET		Delist_Status = @Delist_Status, 
				Delist_Dtm = GETDATE()
		WHERE	SP_ID = @SP_ID
					AND Scheme_Code = @Scheme_Code
	END

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationPermanent_upd_RecordStatus] TO HCVU
GO
