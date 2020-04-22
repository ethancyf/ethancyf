IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HKMTTVaccineMappingUndefined_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HKMTTVaccineMappingUndefined_add]
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
-- Author:			Koala CHENG
-- Create date:		09 Jun 2018
-- CR No.:			CRE18-004 (Sharing of vaccination records among eHS(S), HA CMS and DH CIMS)
-- Description:		Add Undefined DH CIMS vaccine (HKMTT)
-- =============================================
CREATE PROCEDURE [dbo].[proc_HKMTTVaccineMappingUndefined_add]
	@Vaccine_Type					VARCHAR(20),
	@Vaccine_Identifier_Type			VARCHAR(2),
	@L3_Vaccine_HKReqNo_Source		VARCHAR(30),
	@L3_Vaccine_ProductName_Source	VARCHAR(1000),
	@L2_Vaccine_Desc_Source			VARCHAR(200)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	-- Exit if the undefined vaccine is exist
	IF (SELECT Count(1) FROM [HKMTTVaccineMappingUndefined] WITH (NOLOCK)
		WHERE Vaccine_Type = @Vaccine_Type AND 
			Vaccine_Identifier_Type = @Vaccine_Identifier_Type AND 
			L3_Vaccine_HKReqNo_Source = @L3_Vaccine_HKReqNo_Source AND
			L2_Vaccine_Desc_Source = @L2_Vaccine_Desc_Source) > 0
	BEGIN
		RETURN
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Process
-- =============================================
	INSERT INTO [HKMTTVaccineMappingUndefined] (
		System_Dtm,
		Vaccine_Type,
		Vaccine_Identifier_Type,
		L3_Vaccine_HKReqNo_Source,
		L3_Vaccine_ProductName_Source,
		L2_Vaccine_Desc_Source
	) VALUES (
		GETDATE(),
		@Vaccine_Type,
		@Vaccine_Identifier_Type,
		@L3_Vaccine_HKReqNo_Source,
		@L3_Vaccine_ProductName_Source,
		@L2_Vaccine_Desc_Source
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_HKMTTVaccineMappingUndefined_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HKMTTVaccineMappingUndefined_add] TO HCVU
GO
