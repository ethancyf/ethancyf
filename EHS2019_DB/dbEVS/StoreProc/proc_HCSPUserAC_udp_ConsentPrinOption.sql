IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_udp_ConsentPrinOption]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_udp_ConsentPrinOption]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Stanley Chan
-- Create date:		06 Feb 2008
-- Description:		Update ConsentPrintOption for HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSPUserAC_udp_ConsentPrinOption]
	@SP_ID char(8), 
	@ConsentPrintOption char(1)
--@TSMP timestamp
AS
BEGIN
	SET NOCOUNT ON;
	
	/*
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @SP_ID) != @TSMP
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
		update HCSPUserAC
		set 
		[ConsentPrintOption] = @ConsentPrintOption,
		[Update_by]=@SP_ID		
		where SP_ID = @SP_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_udp_ConsentPrinOption] TO HCSP
GO
