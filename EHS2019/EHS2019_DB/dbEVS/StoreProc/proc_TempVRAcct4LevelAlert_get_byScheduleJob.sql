IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVRAcct4LevelAlert_get_byScheduleJob]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVRAcct4LevelAlert_get_byScheduleJob]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 April 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 June 2010
-- Description:		Include the case of amendment record created by Smart ID
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		20 Oct 2008
-- Description:		Get Temp VRAcct Count by different alert level
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	30 Sep 2009
-- Description:		Remove the scheme code to filter the temp voucher account
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_TempVRAcct4LevelAlert_get_byScheduleJob]	@level int								

as
BEGIN
-- =============================================
-- Declaration
-- =============================================
declare @day_level int

DECLARE @AvailableScheme table (
	Scheme_Code		char(10)
)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
if @level=1
begin
	select @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L1_OutstandingDay' AND [Scheme_Code] = 'ALL'	--0day
	
	INSERT INTO @AvailableScheme (Scheme_Code)
	SELECT Scheme_Code FROM SchemeClaim WHERE Available_HCSP_SubPlatform IN ('ALL', 'HK')
	
end

if @level=2
begin
	select @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L2_OutstandingDay' AND [Scheme_Code] = 'ALL'	--7day
	
	INSERT INTO @AvailableScheme (Scheme_Code)
	SELECT Scheme_Code FROM SchemeClaim WHERE Available_HCSP_SubPlatform IN ('ALL', 'HK')
	
end

if @level=3
begin
	select @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L3_OutstandingDay' AND [Scheme_Code] = 'ALL'	--14day
	
	INSERT INTO @AvailableScheme (Scheme_Code)
	SELECT Scheme_Code FROM SchemeClaim WHERE Available_HCSP_SubPlatform IN ('ALL', 'HK')
	
end

if @level=4
begin
	select @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L4_OutstandingDay' AND [Scheme_Code] = 'ALL'	--21day
	
	INSERT INTO @AvailableScheme (Scheme_Code)
	SELECT Scheme_Code FROM SchemeClaim WHERE Available_HCSP_SubPlatform IN ('ALL', 'HK')
	
end

if @level=5
begin
	select @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L5_OutstandingDay' AND [Scheme_Code] = 'ALL'	--28day
	
	INSERT INTO @AvailableScheme (Scheme_Code)
	SELECT Scheme_Code FROM SchemeClaim
	
end
-- =============================================
-- Return results
-- =============================================

select distinct c.sp_id
from TempVoucherAccPendingVerify t, VoucherAccountCreationLOG c, TempVoucherAccount tva
where 
t.Voucher_acc_id = c.Voucher_Acc_id and 
tva.voucher_acc_id = c.Voucher_acc_id and
tva.voucher_acc_id = t.Voucher_acc_id
and DATEDIFF(Day, t.First_Validate_Dtm, getdate()) >= @day_level	
and tva.Record_status='I'
and (tva.Account_Purpose='C' or tva.Account_Purpose='V' OR TVA.Account_Purpose = 'A')
AND TVA.Create_By_BO = 'N'
AND TVA.Scheme_Code IN (SELECT Scheme_Code FROM @AvailableScheme)

END
GO

GRANT EXECUTE ON [dbo].[proc_TempVRAcct4LevelAlert_get_byScheduleJob] TO HCVU
GO
