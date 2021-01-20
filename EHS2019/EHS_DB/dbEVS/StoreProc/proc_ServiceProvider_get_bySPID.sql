IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
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
-- Modified by:		Winnie SUEN
-- Modified date:	14 Nov 2016
-- CR No.			CRE16-018 (Display SP tentative email in HCVU)
-- Description:		Add Tentative_Email
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
-- CR No.:			CRE14-019
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
-- Description:		Insert into [SProcPerformance] to record sproc performance
-- =============================================  
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 10 May 2008
-- Description:	Retrieve Service Provider informaion by SPID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 7 May 2009
-- Description:	1. Remove "Delist Status", "Delist Dtm" and "Effective Dtm"
--              2. Add "Token Return Dtm"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 19 May 2009
-- Description:	1. if address code is not null, get address detail from address detail table
--					(same as what is done in proc_ServiceProvider_get_byERN)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	20 Nov 2013
-- Description:		Add Column -	[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_bySPID]
	@SP_ID	char(8)
AS
BEGIN
	SET NOCOUNT ON;
EXEC [proc_SymmetricKey_open]

-- =============================================
-- Declaration
-- =============================================
DECLARE @Performance_Start_Dtm datetime
SET @Performance_Start_Dtm = GETDATE()

DECLARE @In_SP_ID char(8)
SET @In_SP_ID = @SP_ID

DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)

DECLARE @tmp_SP table ( Enrolment_Ref_No char(15),
						Enrolment_Dtm datetime,
						SP_ID char(8),
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
						Fax	varchar(20),
						Email varchar(255),
						Tentative_Email varchar(255),
						Record_Status char(1),
						Remark nvarchar(255),
						Submission_Method char(1),
						Already_Joined_EHR char(1),
						UnderModification char(1),	
						Token_Return_Dtm datetime,		
						Create_Dtm datetime,
						Create_By varchar(20),
						Update_Dtm datetime,
						Update_By varchar(20),
						ConsentPrintOption char(1),
						Effective_Dtm datetime,
						Alias_Account varchar(20),
						Application_Printed char(1),
						Data_Input_By varchar(20),
						Data_Input_Effective_Dtm datetime
						)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO @tmp_SP (Enrolment_Ref_No,
					Enrolment_Dtm,
					SP_ID,
					Encrypt_Field1,
					Encrypt_Field2,
					Encrypt_Field3,
					Room,
					Floor,
					Block,
					Building,
					Building_Chi,
					District,
					Address_Code,
					Phone_Daytime,
					Fax,
					Email,
					Tentative_Email,
					Record_Status,
					Remark,
					Submission_Method,
					Already_Joined_EHR,
					UnderModification,
					Token_Return_Dtm,				
					Create_Dtm,
					Create_By,
					Update_Dtm,
					Update_By,
					ConsentPrintOption,
					Effective_Dtm,
					Alias_Account,
					Application_Printed,
					Data_Input_By,
					Data_Input_Effective_Dtm
					)

SELECT ServiceProvider.Enrolment_Ref_No, ServiceProvider.Enrolment_Dtm, ServiceProvider.SP_ID, ServiceProvider.Encrypt_Field1, ServiceProvider.Encrypt_Field2, ServiceProvider.Encrypt_Field3, ServiceProvider.Room, ServiceProvider.[Floor],
		ServiceProvider.Block, ServiceProvider.Building, ServiceProvider.Building_Chi, ServiceProvider.District, ServiceProvider.Address_Code, ServiceProvider.Phone_Daytime,
		ServiceProvider.Fax, ServiceProvider.Email, ServiceProvider.Tentative_Email, ServiceProvider.Record_Status, ServiceProvider.Remark, ServiceProvider.Submission_Method, ServiceProvider.Already_Joined_EHR,
		ServiceProvider.UnderModification, ServiceProvider.Token_Return_Dtm, ServiceProvider.Create_Dtm, ServiceProvider.Create_By, 
		ServiceProvider.Update_Dtm, ServiceProvider.Update_By, HCSPUserAC.ConsentPrintOption, ServiceProvider.Effective_Dtm, HCSPUserAC.Alias_Account,ServiceProvider.Application_Printed,
		ServiceProvider.Data_Input_By, ServiceProvider.Data_Input_Effective_Dtm
FROM	ServiceProvider, HCSPUserAC
WHERE	ServiceProvider.SP_ID = @In_SP_ID and ServiceProvider.SP_ID = HCSPUserAC.SP_ID

SELECT @record_id =  Address_Code
FROM	@tmp_SP
WHERE SP_ID = @In_SP_ID

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
	SELECT  T.Enrolment_Ref_No,T.Enrolment_Dtm, T.SP_ID, 
		convert(varchar, DecryptByKey(T.Encrypt_Field1)) as SP_HKID, 
		convert(varchar(40), DecryptByKey(T.Encrypt_Field2)) as SP_Eng_Name, 
		convert(nvarchar, DecryptByKey(T.Encrypt_Field3)) as SP_Chi_Name,
			T.Room, T.[Floor], T.Block, T.Building, T.Building_Chi, T.District, 
			T.Address_Code, T.Phone_Daytime, T.Fax, T.Email, T.Tentative_Email, T.Record_Status, 
			T.Remark, T.Submission_Method, T.Already_Joined_EHR,
			T.UnderModification, T.Application_Printed, T.Effective_Dtm, T.Token_Return_Dtm, T.Create_Dtm, T.Create_By, 
			T.Update_Dtm, T.Update_By, S.TSMP, T.Alias_Account, T.ConsentPrintOption,
			T.Data_Input_By, T.Data_Input_Effective_Dtm
	FROM	@tmp_sp T, ServiceProvider S
	WHERE	T.SP_ID = @In_SP_ID and T.SP_ID = S.SP_ID
	
	
EXEC [proc_SymmetricKey_close]

IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
	DECLARE @Performance_End_Dtm datetime
	SET @Performance_End_Dtm = GETDATE()
		
	EXEC proc_SProcPerformance_add 'proc_ServiceProvider_get_bySPID',
								   @In_SP_ID,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
END

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_bySPID] TO WSEXT
GO
