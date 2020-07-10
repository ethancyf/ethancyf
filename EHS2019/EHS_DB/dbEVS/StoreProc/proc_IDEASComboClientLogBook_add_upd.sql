IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_IDEASComboClientLogBook_add_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_IDEASComboClientLogBook_add_upd]
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
-- Create date:		29-05-2020
-- CR No.:			CRE19-028
-- Description:		Check Ideas Combo Client when SP logins
-- =============================================

CREATE PROCEDURE [dbo].[proc_IDEASComboClientLogBook_add_upd]
	@SPID			CHAR(8),
	@DataEntryID	VARCHAR(20),
	@Installed		CHAR(1),
	@Version		VARCHAR(20),
	@LastUpdateDtm	DATETIME
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @IN_SPID			CHAR(8)
	DECLARE @IN_DataEntryID		VARCHAR(20)
	DECLARE @IN_Installed		CHAR(1)
	DECLARE @IN_Version			VARCHAR(20)
	DECLARE @IN_LastUpdateDtm	DATETIME
	DECLARE @IsExist			BIT

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @IN_SPID = @SPID
	SET @IN_DataEntryID = ''
	SET @IN_Installed = @Installed
	SET @IN_Version = ''
	SET @IN_LastUpdateDtm = @LastUpdateDtm
	SET @IsExist = 0

	IF @DataEntryID IS NOT NULL
		BEGIN
			SET @IN_DataEntryID = @DataEntryID
		END

	IF @IN_Version IS NOT NULL
		BEGIN
			SET @IN_Version = @Version
		END
	
-- =============================================
-- Return results
-- =============================================

	SET @IsExist = (SELECT 
						COUNT(1)
					FROM 
						[IDEASComboClientLogBook] WITH (NOLOCK)
					WHERE
						[SP_ID] = @IN_SPID
						AND [Data_Entry_Account] = @IN_DataEntryID)

	IF @IsExist = 0 
		BEGIN
			-- INSERT
			INSERT INTO [IDEASComboClientLogBook](
				[SP_ID],
				[Data_Entry_Account],
				[Installed],
				[Version],
				[Last_Update_Dtm]
				)
			VALUES(
				@IN_SPID,
				@IN_DataEntryID,
				@IN_Installed,
				@IN_Version,
				@IN_LastUpdateDtm
				)
		END
	ELSE
		BEGIN
			--UPDATE
			UPDATE 
				[IDEASComboClientLogBook]
			SET	
				[Installed] = @IN_Installed,
				[Version] = @IN_Version,
				[Last_Update_Dtm] = @IN_LastUpdateDtm

			WHERE
				[SP_ID] = @IN_SPID
				AND [Data_Entry_Account] = @IN_DataEntryID
		END

END
GO

GRANT EXECUTE ON [dbo].[proc_IDEASComboClientLogBook_add_upd] TO HCSP
GO

