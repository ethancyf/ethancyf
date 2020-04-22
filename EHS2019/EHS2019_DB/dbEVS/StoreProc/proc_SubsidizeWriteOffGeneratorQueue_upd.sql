IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeWriteOffGeneratorQueue_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SubsidizeWriteOffGeneratorQueue_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   24 Nov 2017
-- Description:		Add [DOD], [Exact_DOD]
-- =============================================
-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:	CRE13-006
-- Create Date:	06 Nov 2013
-- Description:	Update record to table - [SubsidizeWriteOffGeneratorQueue] &
--										 [SubsidizeWriteOffGeneratorQueueLog]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SubsidizeWriteOffGeneratorQueue_upd]
	@row_id				int,
	@record_status		char(1),
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
	FROM [SubsidizeWriteOffGeneratorQueue]
	WHERE [Row_ID] = @row_id

	IF @tsmp_before <> @tsmp
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

	UPDATE [SubsidizeWriteOffGeneratorQueue]
	SET	[Record_Status] = @record_status,
		[Update_Dtm] = @current_dtm
	WHERE	[Row_ID] = @row_id
			AND [TSMP] = @tsmp

	IF @@rowcount = 1
		BEGIN

			INSERT INTO [SubsidizeWriteOffGeneratorQueueLog] (
				[System_Dtm],
				[Row_ID],
				[Doc_Code],
				[Encrypt_Field1],
				[DOB],
				[Exact_DOB],
				[Scheme_Code],
				[Subsidize_Code],
				[Create_Dtm],
				[Update_Dtm],
				[Record_Status],
				[DOD],
				[Exact_DOD]
			)
			SELECT
				@current_dtm,
				[Row_ID],
				[Doc_Code],
				[Encrypt_Field1],
				[DOB],
				[Exact_DOB],
				[Scheme_Code],
				[Subsidize_Code],
				[Create_Dtm],
				@current_dtm,
				@record_status,
				[DOD],
				[Exact_DOD]
			FROM
				[SubsidizeWriteOffGeneratorQueue]
			WHERE
				[Row_ID] = @row_id

		END

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeWriteOffGeneratorQueue_upd] TO HCVU
GO
