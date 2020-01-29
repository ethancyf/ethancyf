IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalPermanent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalPermanent_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 23 June 2008
-- Description:	Insert the Professional to Table
--				Professional
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalPermanent_add]
		@professional_seq smallint, @sp_id char(8),
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
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

   INSERT INTO Professional
				(Professional_Seq,
				 SP_ID,
				 Service_Category_Code,
				 Registration_Code,
				 Record_Status,
				 Create_Dtm,
				 Create_By)
	VALUES		(@professional_seq,
				 @sp_id,
				 @service_category_code,
				 @registration_code,
				 @record_status,
				 getdate(),
				 @create_by)

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalPermanent_add] TO HCVU
GO
