IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherAccountRectifyListCnt_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherAccountRectifyListCnt_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	2 Dec 2019
-- CR No.:			INT19-0031 (Fix issue after upgraded .Net 4.8)
-- Description:		Exclude Special Account
-- =============================================    
-- =============================================
-- Modification History
-- Modified by:		Paul Yip
-- Modified date:	26 July 2010
-- Description:		Retrieve both fail temp acc (created by back office) and special acc
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 02 October 2008
-- Description:	Retrieve the number of fail validated Temp Voucher Account 
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherAccountRectifyListCnt_get]	
AS
BEGIN

-- =============================================
-- Initialization
-- =============================================
	SET NOCOUNT ON;

declare @record_num_validated  as integer
declare @record_num_special  as integer
declare @record_num_temp  as integer


select @record_num_validated = count(VA.Voucher_Acc_ID) 
	from VoucherAccount VA, PersonalInformation P
	where 
	VA.Voucher_Acc_ID = P.Voucher_Acc_ID 
	and VA.Voucher_Acc_ID in 
	(
	select TVA.Validated_Acc_ID
	from TempVoucherAccount TVA, TempPersonalInformation TP, PersonalInformation P
	where 
	P.Voucher_Acc_ID = TVA.Validated_Acc_ID 
	and P.Record_Status = 'U'
	and TVA.Record_Status = 'I'
	and	TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID
	and TVA.Account_Purpose = 'A' 
	and (TP.Validating is null or TP.Validating = 'N')
	) 

select @record_num_special = 0
/*
select @record_num_special = count(SA.Special_Acc_ID) 
	from SpecialAccount SA, SpecialPersonalInformation SP
	where 
	SP.Special_Acc_ID = SA.Special_Acc_ID 
	and SA.Record_Status = 'I'
	and SA.Account_Purpose = 'C' 
	and (SP.Validating is null or SP.Validating = 'N')
*/
	
Select @record_num_temp = count(TA.Voucher_Acc_ID)
 from TempVoucherAccount TA, TempPersonalInformation TP, VoucherAccountCreationLOG C    
 where     
 TA.Voucher_acc_id = TP.Voucher_acc_id     
 and TA.Record_Status ='I'
 and TA.Account_Purpose in ('V', 'C')    
 and TA.Create_By_BO = 'Y'
and (TP.Validating is null or TP.Validating = 'N')  
 and TA.Voucher_acc_id = C.Voucher_Acc_ID    
 and C.voucher_acc_type = 'T' 


select @record_num_validated + @record_num_special + @record_num_temp as RecordCount


END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherAccountRectifyListCnt_get] TO HCVU
GO
