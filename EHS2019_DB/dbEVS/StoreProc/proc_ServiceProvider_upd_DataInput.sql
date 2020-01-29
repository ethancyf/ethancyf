IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_DataInput]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_DataInput]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		2 February 2017
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Grant EXECUTE to WSEXT
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 November 2014
-- CR No.:			CRE13-029
-- Description:		RSA Server Upgrade - Grand EXECUTE to HCSP
-- =============================================
-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:	INT13-0028 - SP Amendment Report
-- Create Date:	19 Nov 2013
-- Description:	Update record - [Data_Input_By], [Data_Input_Effective_Dtm]
--				to Table - [ServiceProvider]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_DataInput]
	@sp_id				char(8),
	@data_input_by		varchar(20),
	@update_by			varchar(20),
	@tsmp				timestamp
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================

	DECLARE @current_dtm AS datetime
	DECLARE @tsmp_before AS timestamp

-- ============================================================
-- Validation
-- ============================================================

	SELECT @tsmp_before = [TSMP]
	FROM [ServiceProvider]
	WHERE [SP_ID] = @sp_id

	IF NOT (@tsmp_before = @tsmp)
		BEGIN
			RAISERROR('00011', 16, 1)
			RETURN @@ERROR
		END

-- ============================================================
-- Initialization
-- ============================================================

	SET @current_dtm = GETDATE()

-- ============================================================
-- Return results
-- ============================================================

	UPDATE [ServiceProvider]
	SET	[Data_Input_By] = @data_input_by,
		[Data_Input_Effective_Dtm] = @current_dtm,
		[Update_Dtm] = @current_dtm,
		[Update_By] = @update_by
	WHERE	[SP_ID] = @sp_id
			AND [TSMP] = @tsmp

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_DataInput] TO HCVU, HCSP, WSEXT
GO
