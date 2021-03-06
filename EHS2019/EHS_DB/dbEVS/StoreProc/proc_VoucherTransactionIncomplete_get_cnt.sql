IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionIncomplete_get_cnt]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_VoucherTransactionIncomplete_get_cnt]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
-- =============================================
-- Author:			Koala CHENG
-- CR No:			CRE11-024-02 HCVS Pilot Extension Part 2
-- Create date:		18-Aug-2011
-- Description:		Get the count of VoucherTansaction in [Incomplete] status
-- =============================================
Create Procedure [dbo].[proc_VoucherTransactionIncomplete_get_cnt]
@SP_ID		char(8),
@DataEntry  varchar(20)
as

-- =============================================
-- Declaration
-- =============================================
declare @Transaction_Dtm	datetime
declare @cnt1	int
declare @cnt2	int

-- =============================================
-- Initization
-- =============================================
select @Transaction_Dtm = getdate()
select @Transaction_Dtm = dateadd(day, 1, @Transaction_Dtm)


-- =============================================
-- Get Result
-- =============================================
select @cnt1 = count(1)
from VoucherTransaction v
, PersonalInformation p
, Practice pr
--, BankAccount b
where v.SP_ID = @SP_ID
and (@DataEntry is null or @DataEntry = V.DataEntry_By) 
--and (@Bank_Acc_Display_Seq is null or v.Bank_Acc_Display_Seq = @Bank_Acc_Display_Seq)
--and (@DataEntry_By is null or v.DataEntry_By = @DataEntry_By)
and v.SP_ID = pr.SP_ID
--and v.SP_ID = b.SP_ID
and v.Voucher_Acc_ID = p.Voucher_Acc_ID
and v.Practice_Display_Seq = pr.Display_Seq
--and v.Bank_Acc_Display_Seq = b.Display_Seq
and v.Transaction_Dtm < @Transaction_Dtm
--and v.Temp_Voucher_Acc_ID = ''
and v.Record_Status = 'U'

select @cnt2 = count(1)
from VoucherTransaction v
, TempPersonalInformation p
, Practice pr
--, BankAccount b
, TempVoucherAccount t
where v.SP_ID = @SP_ID
and (@DataEntry is null or @DataEntry = V.DataEntry_By)  
--and (@Bank_Acc_Display_Seq is null or v.Bank_Acc_Display_Seq = @Bank_Acc_Display_Seq)
--and (@DataEntry_By is null or v.DataEntry_By = @DataEntry_By)
and v.SP_ID = pr.SP_ID
--and v.SP_ID = b.SP_ID
and v.Temp_Voucher_Acc_ID = t.Voucher_Acc_ID
and v.Temp_Voucher_Acc_ID = p.Voucher_Acc_ID
and v.Practice_Display_Seq = pr.Display_Seq
--and v.Bank_Acc_Display_Seq = b.Display_Seq
and v.Transaction_Dtm < @Transaction_Dtm
and v.Voucher_Acc_ID = ''
and v.Record_Status = 'U'

-- =============================================
-- Return Result
-- =============================================
--return @cnt1 + @cnt2
select @cnt1 + @cnt2	cnt


GRANT EXECUTE ON proc_VoucherTransactionIncomplete_get_cnt TO HCVU
GO

GRANT EXECUTE ON proc_VoucherTransactionIncomplete_get_cnt TO HCSP
GO