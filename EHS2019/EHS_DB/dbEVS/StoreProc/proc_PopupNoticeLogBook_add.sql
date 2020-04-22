IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PopupNoticeLogBook_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PopupNoticeLogBook_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	10 Oct 2018
-- CR No.:			INT18-0012
-- Description:		Check duplicate record before insert to avoid error on concurrent browser action
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		06-09-2018
-- CR No.:			CRE18-005
-- Description:		Add Popup Notice When SP is acknowledged
-- =============================================

CREATE PROCEDURE [dbo].[proc_PopupNoticeLogBook_add]
	@PopupName		VARCHAR(20),
	@SPID			CHAR(8),
	@DataEntryID	VARCHAR(20),
	@ClosePopupDtm	DATETIME
	
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_PopupName		VARCHAR(20)
	DECLARE @IN_SPID			CHAR(8)
	DECLARE @IN_DataEntryID		VARCHAR(20)
	DECLARE @IN_ClosePopupDtm	DATETIME
	DECLARE @IsDuplicated		BIT

-- =============================================
-- Initialization
-- =============================================
	SET @IN_PopupName = @PopupName
	SET @IN_SPID = @SPID
	SET @IN_DataEntryID = ''
	SET @IN_ClosePopupDtm = @ClosePopupDtm
	SET @IsDuplicated = 0

	IF @DataEntryID IS NOT NULL
		BEGIN
			SET @IN_DataEntryID = @DataEntryID
		END

-- =============================================
-- Validation 
-- =============================================
	SET @IsDuplicated = (SELECT 
							COUNT(1)
						FROM 
							[PopupNoticeLogBook] WITH (NOLOCK)
						WHERE
							[SP_ID] = @IN_SPID
							AND [Data_Entry_Account] = @IN_DataEntryID)

-- =============================================
-- Return results
-- =============================================

	IF @IsDuplicated = 0
		BEGIN
			INSERT INTO [PopupNoticeLogBook](
				[Popup_Name],
				[SP_ID],
				[Data_Entry_Account],
				[Close_Popup_Dtm]
				)
			VALUES(
				@IN_PopupName,
				@IN_SPID,
				@IN_DataEntryID,
				@IN_ClosePopupDtm
				)
		END

END
GO

GRANT EXECUTE ON [dbo].[proc_PopupNoticeLogBook_add] TO HCSP
GO

