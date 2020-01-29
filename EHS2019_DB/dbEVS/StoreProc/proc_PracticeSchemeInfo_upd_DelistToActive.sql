IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfo_upd_DelistToActive]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfo_upd_DelistToActive]
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
-- Author:		Kathy LEE
-- Create date: 14 July 2009
-- Description:	Reactive the practice scheme in 
--				when those practice is enrolled and delisted
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 4 August 2009
-- Description:	Add "Subsidize_Code"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfo_upd_DelistToActive]
	@sp_id char(8), 
	@scheme_code char(10), 
	@practice_display_seq smallint, 
	@service_fee smallint,
	@update_by varchar(20),
	@record_status char(1),
	@tsmp timestamp,
	@provideservicefee char(1),
	@subsidize_code char(10),
	@Provide_Service char(1)
	
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
declare @exist_record_status char(1)
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM PracticeSchemeInfo
		WHERE SP_ID = @sp_id and
				Practice_Display_Seq = @practice_display_seq and
				Scheme_code = @scheme_code and
				subsidize_code = @subsidize_code) != @tsmp
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
			Update_By = @update_by,
			Update_Dtm = getdate(),
			record_status = @record_status,
			ProvideServiceFee = @provideservicefee,
			delist_status = null,
			delist_dtm = null,
			effective_dtm = getdate(),
			remark = null,
			Provide_Service = @Provide_Service
	WHERE	SP_ID = @sp_id and 
			Scheme_Code = @scheme_code and 
			Practice_Display_Seq=@practice_display_seq and
			subsidize_code = @subsidize_code

		
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfo_upd_DelistToActive] TO HCVU
GO
