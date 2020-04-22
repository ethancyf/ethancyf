IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderEnrolment_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderEnrolment_get_byERN]
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
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Retrieve the Service Provider from Table
--				ServiceProviderEnrolment
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderEnrolment_get_byERN]
	@enrolment_ref_no	char(15)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)

DECLARE @tmp_SP table ( Enrolment_Ref_No char(15),
						Enrolment_Dtm datetime,
						--SP_HKID char(9),
						--SP_Eng_Name varchar(40),
						--SP_Chi_Name nvarchar(6),
						Encrypt_Field1 varbinary(100),
						Encrypt_Field2 varbinary(100),
						Encrypt_Field3 varbinary(100),
						Room nvarchar(5),
						[Floor] nvarchar(3),
						Block nvarchar(3),
						Building varchar(255),
						Building_Chi nvarchar(255) collate database_default,
						District char(4),
						Address_Code int,
						Phone_Daytime varchar(20),
						Fax varchar(20),
						Email varchar(255),
						Already_Joined_EHR char(1),
						Join_PCD char(1),
						Application_Printed char(1))

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO @tmp_SP ( Enrolment_Ref_No ,
						Enrolment_Dtm ,
						--SP_HKID,
						--SP_Eng_Name ,
						--SP_Chi_Name,
						Encrypt_Field1,
						Encrypt_Field2,
						Encrypt_Field3,
						Room,
						[Floor],
						Block ,
						Building,
						Building_Chi,
						District,
						Address_Code,
						Phone_Daytime,
						Fax,
						Email,
						Already_Joined_EHR,
						Join_PCD,
						Application_Printed)

SELECT Enrolment_Ref_No, Enrolment_Dtm, Encrypt_Field1, Encrypt_Field2, Encrypt_Field3,
			Room, [Floor], Block, Building, Building_Chi, District, Address_Code, Phone_Daytime,
			Fax, Email,	Already_Joined_EHR, Join_PCD, Application_Printed
FROM	ServiceProviderEnrolment
WHERE	Enrolment_Ref_No = @enrolment_ref_no

SELECT @record_id =  Address_Code
FROM	@tmp_SP
WHERE Enrolment_Ref_No = @enrolment_ref_no

if @record_id IS NOT NULL
BEGIN
	exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE @tmp_sp
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
END
-- =============================================
-- Return results
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT Enrolment_Ref_No, Enrolment_Dtm,
			convert(varchar, DecryptByKey(Encrypt_Field1)) as SP_HKID,
			convert(varchar(40), DecryptByKey(Encrypt_Field2)) as SP_Eng_Name,
			convert(nvarchar, DecryptByKey(Encrypt_Field3)) as SP_Chi_Name,
			Room, [Floor], Block, Building, Building_Chi, District, Address_Code, Phone_Daytime,
			Fax, Email,	Already_Joined_EHR, Join_PCD, Application_Printed
	FROM	@tmp_sp

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolment_get_byERN] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolment_get_byERN] TO HCVU
GO
