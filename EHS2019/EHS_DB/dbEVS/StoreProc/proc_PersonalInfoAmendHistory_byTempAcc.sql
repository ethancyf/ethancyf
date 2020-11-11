IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PersonalInfoAmendHistory_byTempAcc]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_byTempAcc]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			INT20-037
-- Modified by:		Koala CHENG
-- Modified date:	30 Oct 2020
-- Description:		1. To avoid insert duplicate key (System_dtm,Voucher_Acc_ID) when user confirm multiple temp account in batch
--					Auto increment 1 to 109 milliseconds to system_dtm
--					2. Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-019 (To read new Smart HKIC in eHS(S))
-- Modified by:		Winnie SUEN
-- Modified date:	5 Dec 2018
-- Description:		Add [SmartID_Ver]
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 19 Feb 2010
-- Description:	Insert the personal information amendment
--				history from temp personal information
-- =============================================

CREATE PROCEDURE [dbo].[proc_PersonalInfoAmendHistory_byTempAcc]
	-- Add the parameters for the stored procedure here
	@Voucher_Acc_ID char(15),
	@doc_code char(10),
	@update_by varchar(20)
	                         
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

DECLARE @System_dtm as DATETIME 
SET @System_dtm = DATEADD(ms, ABS(CHECKSUM(NEWID()) % 100), GETDATE())

insert into personalinfoamendhistory
(
	System_Dtm,
	Voucher_Acc_ID,
	DOB,
	Exact_DOB,
	Sex,
	Date_of_Issue,
	Create_By_SmartID,
	Update_by,
	Record_Status,
	SubmitToVerify,
	Encrypt_Field1,
	Encrypt_Field2,
	Encrypt_Field3,
	Encrypt_Field4,
	Encrypt_Field5,
	Encrypt_Field6,
	Encrypt_Field7,
	Encrypt_Field8,
	Encrypt_Field9,
	Temp_Voucher_Acc_ID,
	Action_type,
	EC_Serial_No,
	EC_Reference_No,
	EC_Age,
	EC_Date_of_Registration,
	Doc_Code,
	Foreign_Passport_No,
	Other_Info,
	Permit_To_Remain_Until,
	Encrypt_Field10,
	Encrypt_Field11,
	SmartID_Ver
)
Select	(SELECT DATEADD(ms, 10 * COUNT(1), @System_dtm) 
			FROM PersonalInfoAmendHistory WITH (NOLOCK) 
			WHERE System_Dtm = @System_dtm AND Voucher_Acc_ID = t.Validated_Acc_ID) AS [System_Dtm],
		t.Validated_Acc_ID,
		tp.DOB,
		tp.Exact_DOB,
		tp.Sex,
		tp.Date_of_Issue,
		tp.Create_By_SmartID,
		@update_by,
		'V',
		'Y',
		tp.Encrypt_Field1,
		tp.Encrypt_Field2,
		tp.Encrypt_Field3,
		tp.Encrypt_Field4,
		tp.Encrypt_Field5,
		tp.Encrypt_Field6,
		tp.Encrypt_Field7,
		tp.Encrypt_Field8,
		tp.Encrypt_Field9,
		t.voucher_acc_id,
		'A',
		tp.EC_Serial_No,
		tp.EC_Reference_No,
		tp.EC_Age,
		tp.EC_Date_of_Registration,
		tp.Doc_Code,
		tp.Foreign_Passport_No,
		tp.Other_Info,
		tp.Permit_To_Remain_Until,
		tp.Encrypt_Field10,
		tp.Encrypt_Field11,
		tp.SmartID_Ver
		
from TempVoucherAccount t WITH (NOLOCK), TempPersonalInformation tp WITH (NOLOCK)
where t.voucher_acc_id = tp.voucher_acc_id
and tp.voucher_acc_id = @Voucher_Acc_ID
and tp.Doc_code = @doc_code

END
GO

GRANT EXECUTE ON [dbo].[proc_PersonalInfoAmendHistory_byTempAcc] TO HCSP
GO
