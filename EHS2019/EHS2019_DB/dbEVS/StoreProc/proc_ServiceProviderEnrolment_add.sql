IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	03 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Add [Join_PCD] column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	12 Feb 2016
-- CR No.			CRE15-019
-- Description:		Rename PPI-ePR to eHRSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 January 2016
-- CR No.			CRE15-008
-- Description:		Remove PPI-ePR enrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	18 August 2009
-- Description:		Add Batch_ID
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Service Provider to Table
--				ServiceProviderEnrolment
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderEnrolment_add]
	@enrolment_ref_no char(15), @sp_hkid char(9), @sp_eng_name varchar(40),
	@sp_chi_name nvarchar(6), @room nvarchar(5), @floor nvarchar(3), @block nvarchar(3),
	@building varchar(100), @building_chi nvarchar(100), @district char(4), 
	@address_code int, @phone_daytime varchar(20), @fax varchar(20),
	@email varchar(255), @already_joined_EHR char(1), @Join_PCD char(1),
	@batch_ID char(15)
	
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
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	INSERT INTO ServiceProviderEnrolment
				(Enrolment_Ref_No,				
				Enrolment_Dtm,
				Encrypt_Field1,
				Encrypt_Field2,
				Encrypt_Field3,
				Room,
				[Floor],
				Block,
				Building,
				Building_Chi,
				District,
				Address_Code,
				Phone_Daytime,
				Fax,
				Email,
				Already_Joined_EHR,
				Join_EHR,
				Join_PCD,
				Batch_ID)
	VALUES		(@enrolment_ref_no,				
				getdate(),
				EncryptByKey(KEY_GUID('sym_Key'), @sp_hkid),
				EncryptByKey(KEY_GUID('sym_Key'), @sp_eng_name),
				EncryptByKey(KEY_GUID('sym_Key'), @sp_chi_name),
				@room,
				@floor,
				@block,
				@building,
				@building_chi,
				@district,
				@address_code,
				@phone_daytime,
				@fax,
				@email,
				@already_joined_EHR,
				NULL,
				@Join_PCD,
				@batch_ID)

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolment_add] TO HCPUBLIC
GO
