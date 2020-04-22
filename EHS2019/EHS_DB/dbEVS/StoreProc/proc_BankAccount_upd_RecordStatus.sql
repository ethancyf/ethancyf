IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankAccount_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankAccount_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 03 July 2008
-- Description:	Update BankAccount Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	27 May 2009
-- Description:		Remove Delist_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankAccount_upd_RecordStatus]
	@SP_ID						char(8),
	@SP_Practice_Display_Seq	smallint,
	@Record_Status				char(1),
	@Update_By					varchar(20),
	@TSMP						timestamp
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT TSMP FROM [dbo].[BankAccount]
		WHERE SP_ID = @SP_ID AND SP_Practice_Display_Seq = @SP_Practice_Display_Seq
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
	UPDATE	[dbo].[BankAccount]	
	SET		Update_By = @Update_By,
			Update_Dtm = GetDate(),
			Record_Status = @Record_Status
	WHERE	SP_ID = @SP_ID
				AND SP_Practice_Display_Seq = @SP_Practice_Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_BankAccount_upd_RecordStatus] TO HCVU
GO
