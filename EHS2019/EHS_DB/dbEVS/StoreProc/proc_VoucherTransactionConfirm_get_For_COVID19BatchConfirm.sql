IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (Immu record)
-- Modified by:		Winnie SUEN
-- Modified date:	11 Mar 2021
-- Description:		Retrieve the list pending for COVID19BatchConfirm
-- =============================================


CREATE Procedure [dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm]  
	@Transaction_Dtm   datetime
AS BEGIN

-- =============================================  
-- Declaration  
-- =============================================  
DECLARE @Performance_Start_Dtm datetime
SET @Performance_Start_Dtm = GETDATE()

DECLARE @intMaxRecods AS INT
SELECT @intMaxRecods = Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'COVID19BatchConfirmSize'

DECLARE @In_Transaction_Dtm		DATETIME

SET @In_Transaction_Dtm = @Transaction_Dtm

-- 
create table #VoucherAccTransaction (  
  Transaction_ID  char(20) collate database_default,  
  Transaction_Dtm datetime,  
  SP_ID	VARCHAR(8),
  Voucher_Acc_ID char(15),  
  Temp_Voucher_Acc_ID char(15),  
  Record_Status char(1),  
  TSMP binary(8),  
  Voucher_Acc_TSMP binary(8),  
  Scheme_Code char(25) collate database_default,   
  Doc_Code char(20),  
  DOB    datetime,  
  Exact_DOB  char(1),  
  Date_Of_Issue datetime,  

  -- For Temp Account
  original_amend_acc_id char(15),  
  original_TSMP binary(8),  
  Validated_Acc_ID char(15),  
  original_DOI datetime,  
  original_DOB datetime,  
  original_Exact_DOB char(1),  
  Send_To_ImmD char(1),   
  Account_Purpose CHAR(1)

)  
  
CREATE INDEX IX_VAT on #VoucherAccTransaction (Transaction_ID)  


-- =============================================  
-- Initization  
-- =============================================  
  
  
select @In_Transaction_Dtm = dateadd(day, 1, @In_Transaction_Dtm)  
  

-- Validated Account
  
insert into #VoucherAccTransaction  
(  
	Transaction_ID,  
	Transaction_Dtm,  
	SP_ID,
	Voucher_Acc_ID,  
	Temp_Voucher_Acc_ID,   
	Record_Status,  
	TSMP,  
	Voucher_Acc_TSMP,   
	Scheme_Code,   
	Doc_Code
)  
  
SELECT
	v.Transaction_ID,  
	v.Transaction_Dtm,  
	v.SP_ID,
	v.Voucher_Acc_ID,  
	'',   
	v.Record_Status,  
	v.tsmp,  
	null,  
	v.scheme_code,  
	p.doc_code
FROM 
	VoucherTransaction v,
	PersonalInformation p
WHERE
	v.Scheme_Code = 'COVID19CVC'
AND v.Record_Status='P'	-- Pending For Confirmation
AND v.Voucher_Acc_ID = p.Voucher_Acc_ID  
AND v.doc_code = p.doc_code  
AND v.Transaction_Dtm < @In_Transaction_Dtm  



-- Temp Account
  
insert into #VoucherAccTransaction  
(  
	Transaction_ID,  
	Transaction_Dtm,
	SP_ID,
	Voucher_Acc_ID,  
	Temp_Voucher_Acc_ID,  
	Record_Status,  
	TSMP,  
	Voucher_Acc_TSMP,   
	Scheme_Code,   
	Doc_Code,  
	DOB,  
	Exact_DOB,  
	Date_Of_Issue,  
	original_amend_acc_id,  
	Validated_Acc_ID,   
	Account_Purpose
)  
select 
	v.Transaction_ID,  
	v.Transaction_Dtm,  
	v.SP_ID,
	'',  
	v.Temp_Voucher_Acc_ID,  
	v.Record_Status,  
	v.tsmp,  
	t.tsmp,  
	v.scheme_code,  
	p.doc_code,  
	p.DOB,  
	p.Exact_DOB,  
	p.Date_Of_Issue,  
	t.original_amend_acc_id,  
	t.Validated_Acc_ID,   
	t.Account_Purpose
FROM 
	VoucherTransaction v,TempPersonalInformation p, TempVoucherAccount t

WHERE
	v.Scheme_Code = 'COVID19CVC'
AND v.Record_Status='P'	-- Pending For Confirmation
AND v.Temp_Voucher_Acc_ID = t.Voucher_Acc_ID  
AND v.Temp_Voucher_Acc_ID = p.Voucher_Acc_ID  
AND v.Transaction_Dtm < @In_Transaction_Dtm  
AND v.Voucher_Acc_ID = ''  


--
  
  
update #VoucherAccTransaction  
set original_TSMP = t.TSMP,  
 original_DOI = p.date_of_issue,  
 original_DOB = p.DOB,  
 original_Exact_DOB = p.exact_dob  
from #VoucherAccTransaction vt, tempvoucheraccount t, temppersonalinformation p  
where vt.original_amend_acc_id = t.voucher_acc_id collate database_default  
and t.voucher_acc_id = p.voucher_acc_id  
and vt.original_amend_acc_id is not null  
and t.account_purpose = 'O'  
  
update #VoucherAccTransaction  
set Send_To_ImmD = 'X'  
where original_amend_acc_id is null  
  
update #VoucherAccTransaction  
set Send_To_ImmD = 'N'  
where Date_Of_Issue = Original_DOI  
and DOB = Original_DOB  
and Exact_DOB = original_Exact_DOB  
and Original_DOI is not null  
and Original_DOB is not null  
and original_Exact_DOB is not null  
and original_amend_acc_id is not null  
  
update #VoucherAccTransaction  
set Send_To_ImmD = 'Y'  
where Send_To_ImmD is null  
  
  
-- =============================================  
-- Return results  
-- =============================================  
SELECT TOP (@intMaxRecods)
	v.Transaction_ID,  
	v.Transaction_Dtm, 
	v.SP_ID,
	v.Voucher_Acc_ID,  
	v.Temp_Voucher_Acc_ID,  
	v.Record_Status,  
	v.TSMP,  
	v.Voucher_Acc_TSMP,   
	v.Doc_Code,  
	v.original_amend_acc_id,  
	v.original_TSMP,  
	v.Validated_Acc_ID,  
	v.Send_To_ImmD,
	v.Account_Purpose
FROM
	#VoucherAccTransaction v

ORDER BY 
	-- Priority: Amended Account > Other account
	CASE v.Account_Purpose WHEN 'A' THEN 1 ELSE 99 END ASC,
	v.Transaction_Dtm ASC


	--

IF (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'EnableSProcPerformCapture' AND Scheme_Code = 'ALL') = 'Y' BEGIN
	DECLARE @Performance_End_Dtm datetime
	SET @Performance_End_Dtm = GETDATE()
	DECLARE @Parameter varchar(255)
	SET @Parameter = ISNULL(CONVERT(varchar, @In_Transaction_Dtm, 120), '')
	
	EXEC proc_SProcPerformance_add 'proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm',
								   @Parameter,
								   @Performance_Start_Dtm,
								   @Performance_End_Dtm
END


END
GO  

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm] TO HCSP, HCVU, WSEXT
GO

