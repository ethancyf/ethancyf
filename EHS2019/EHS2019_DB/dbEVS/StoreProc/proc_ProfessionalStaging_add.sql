IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 22 May 2008
-- Description:	Insert the Professional to Table
--				ProfessionalStaging
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalStaging_add]
		@enrolment_ref_no char(15), @professional_seq smallint, @sp_id char(8),
		@service_category_code char(5), @registration_code varchar(15),
		@record_status char(1), @create_by varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
declare @rowcount int

SELECT	@rowcount = count(1)       
FROM	ProfessionalStaging
WHERE	Enrolment_Ref_No = @enrolment_ref_no and Professional_Seq  = @professional_seq
		
	IF @rowcount > 0
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@ERROR
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

   INSERT INTO ProfessionalStaging
				(Enrolment_Ref_No,
				 Professional_Seq,
				 SP_ID,
				 Service_Category_Code,
				 Registration_Code,
				 Record_Status,
				 Create_Dtm,
				 Create_By)
	VALUES		(@enrolment_ref_no,
				 @professional_seq,
				 @sp_id,
				 @service_category_code,
				 @registration_code,
				 @record_status,
				 getdate(),
				 @create_by)

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalStaging_add] TO HCVU
GO
