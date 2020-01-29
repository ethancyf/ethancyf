IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_udp_ConsentPrinOption]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_udp_ConsentPrinOption]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Stanley Chan
-- Create date:		06 Feb 2008
-- Description:		Update ConsentPrintOption for Data Entry UserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_DataEntryUserAC_udp_ConsentPrinOption]
	@Data_Entry_Account varchar(20),
	@SP_ID char(8), 
	@Update_By varchar(20),
	@ConsentPrintOption char(1)
	--@TSMP timestamp
AS

BEGIN
	SET NOCOUNT ON;
	
	/*
	IF (SELECT TSMP FROM DataEntryUserAC
		WHERE SP_ID = @SP_ID and Data_Entry_Account = @Data_Entry_Account) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
	*/
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
		update DataEntryUserAC
		set 
		[ConsentPrintOption] = @ConsentPrintOption,
		[Update_by]=@Update_By		
		where SP_ID = @SP_ID and Data_Entry_Account = @Data_Entry_Account

END
GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_udp_ConsentPrinOption] TO HCSP
GO
