IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PopupNoticeLogBook_get_bySPIDDataEntryID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PopupNoticeLogBook_get_bySPIDDataEntryID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:
-- CR No.:
-- Description:	
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		06-09-2018
-- CR No.:			CRE18-005
-- Description:		Check Popup Notice Whether SP is acknowledged
-- =============================================

CREATE PROCEDURE [dbo].[proc_PopupNoticeLogBook_get_bySPIDDataEntryID]
	@SPID			CHAR(8),
	@DataEntryID	VARCHAR(20)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_SPID		CHAR(8)
	DECLARE @IN_DataEntryID	VARCHAR(20)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @IN_SPID = @SPID
	SET @IN_DataEntryID = ''

	IF @DataEntryID IS NOT NULL
		BEGIN
			SET @IN_DataEntryID = @DataEntryID
		END
	
-- =============================================
-- Return results
-- =============================================

	SELECT 
		[Popup_Name]
	FROM 
		[PopupNoticeLogBook] WITH (NOLOCK)
	WHERE
		[SP_ID] = @IN_SPID
		AND [Data_Entry_Account] = @IN_DataEntryID

END
GO

GRANT EXECUTE ON [dbo].[proc_PopupNoticeLogBook_get_bySPIDDataEntryID] TO HCSP
GO

