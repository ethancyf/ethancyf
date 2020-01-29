IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfo_upd_ServiceFee]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfo_upd_ServiceFee]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 18 May 2009
-- Description:	Update the service fee in
--				Table "PracticeSchemeInfo"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	10 July 2009
-- Description:		1. Add ProvideServiceFee
--					2. TSMP Checking
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	4 August 2009
-- Description:		Add Subsidize Code
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfo_upd_ServiceFee]
	@sp_id char(8), @scheme_code char(10), @practice_display_seq smallint, @service_fee smallint,
	@update_by varchar(20), @provideServiceFee char(1), @subsidize_code char(10), @tsmp timestamp,
	@Provide_Service char(1)
	
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM PracticeSchemeInfo
		WHERE SP_ID = @sp_id and 
			Scheme_Code = @scheme_code and 
			Practice_Display_Seq=@practice_display_seq and
			Subsidize_Code= @subsidize_code) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	PracticeSchemeInfo
	Set		Service_Fee = @service_fee,
			provideServiceFee = @provideServiceFee,
			Update_By = @update_by,
			Update_Dtm = getdate(),
			Provide_Service = @Provide_Service
	WHERE	SP_ID = @sp_id and 
			Scheme_Code = @scheme_code and 
			Practice_Display_Seq=@practice_display_seq and
			Subsidize_Code= @subsidize_code
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfo_upd_ServiceFee] TO HCVU
GO
