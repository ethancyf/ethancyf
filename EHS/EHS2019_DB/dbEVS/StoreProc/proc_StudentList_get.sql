IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:	
-- Description:	
-- =============================================
-- =============================================
-- Author:		Marco CHOI
-- Create date: 21 Aug 2018
-- Description:	Retrieve Student File List
--  			proc_StudentList_get  'SF2018081700003'		
-- =============================================


CREATE PROCEDURE [dbo].[proc_StudentList_get] 
	@Student_File_ID	varchar(15)
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

	--


	SELECT 
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Class_No,
		Contact_No,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Temp_Acc_Record_Status,
		Temp_Acc_Validate_Dtm,
		Acc_Validation_Result,
		Validated_Acc_Found,
		Validated_Acc_Unmatch_Result,
		convert(VARCHAR(MAX), DecryptByKey(Encrypt_Field3)) AS Chi_Name,
		convert(VARCHAR(MAX), DecryptByKey(Encrypt_Field2_1)) AS Eng_Surname,
		convert(VARCHAR(MAX), DecryptByKey(Encrypt_Field2_2)) AS Eng_GivenName,
		Sex,
		DOB,
		Doc_Code,
		convert(VARCHAR(MAX), DecryptByKey(Encrypt_Field1)) AS DocNumber,
		Date_of_Issue,
		Permit_To_Remain_Until,
		Foreign_Passport_No,
		EC_Serial_No,
		EC_Reference_No
	from StudentFileEntryStaging
	where Student_File_ID = 'SF2018081700003'
	ORDER BY
		Student_Seq
	
	--
	
	CLOSE SYMMETRIC KEY sym_Key

END
GO


GRANT EXECUTE ON [dbo].[proc_StudentList_get] TO HCVU
GO


