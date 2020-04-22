IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_upd_ServiceFee]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_ServiceFee]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 04 June 2008
-- Description:	Update the range of service fee in
--				Table "SchemeInformationStaging"
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_ServiceFee]
	@enrolment_ref_no char(15), @scheme_code char(10), @service_fee_from smallint,
	@service_fee_to smallint, @update_by varchar(20), @tsmp timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM SchemeInformationStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and Scheme_Code = @scheme_code) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	SchemeInformationStaging
	Set		Service_Fee_From = @service_fee_from,
			Service_Fee_To = @service_fee_to,
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and 
			Scheme_Code = @scheme_code
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_upd_ServiceFee] TO HCVU
GO
