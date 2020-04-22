IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderEnrolmentBOTH_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderEnrolmentBOTH_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 January 2016
-- Description:		Stored procedure is not used anymore
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Service Provider to Table
--				ServiceProviderEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_ServiceProviderEnrolmentBOTH_add]
	@enrolment_ref_no char(15), @sp_hkid char(9), @sp_eng_name varchar(40),
	@sp_chi_name nvarchar(6), @room varchar(5), @floor varchar(3), @block varchar(3),
	@building varchar(100), @building_chi nchar(100), @district char(4), 
	@address_code int, @phone_daytime varchar(20), @fax varchar(20),
	@email varchar(255), @already_joined_ha_ppi char(1), @join_ha_ppi char(1), @scheme char(5)
	
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
	if @scheme = 'IVSS'
	BEGIN
		exec dbIVSS..proc_ServiceProviderEnrolment_add @enrolment_ref_no, @sp_hkid, @sp_eng_name, @sp_chi_name,
											 @room, @floor, @block, @building, @building_chi, @district,
											 @address_code, @phone_daytime, @fax, @email, @already_joined_ha_ppi,
											 @join_ha_ppi
	END
	ELSE IF @scheme = 'EHCVS'
	BEGIN
		exec dbEVS..proc_ServiceProviderEnrolment_add @enrolment_ref_no, @sp_hkid, @sp_eng_name, @sp_chi_name,
											 @room, @floor, @block, @building, @building_chi, @district,
											 @address_code, @phone_daytime, @fax, @email, @already_joined_ha_ppi,
											 @join_ha_ppi
	END
	ELSE IF @scheme = 'BOTH'
	BEGIN
		exec dbIVSS..proc_ServiceProviderEnrolment_add @enrolment_ref_no, @sp_hkid, @sp_eng_name, @sp_chi_name,
											 @room, @floor, @block, @building, @building_chi, @district,
											 @address_code, @phone_daytime, @fax, @email, @already_joined_ha_ppi,
											 @join_ha_ppi
		exec dbEVS..proc_ServiceProviderEnrolment_add @enrolment_ref_no, @sp_hkid, @sp_eng_name, @sp_chi_name,
												 @room, @floor, @block, @building, @building_chi, @district,
												 @address_code, @phone_daytime, @fax, @email, @already_joined_ha_ppi,
												 @join_ha_ppi

	END

END

--GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolmentBOTH_add] TO HCPUBLIC
--GO
*/
