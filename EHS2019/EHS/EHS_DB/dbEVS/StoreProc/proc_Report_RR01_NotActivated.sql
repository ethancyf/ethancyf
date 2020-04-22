IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Report_RR01_NotActivated]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Report_RR01_NotActivated]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026 (Change email for locked SP)
-- Modified by:		Winnie SUEN
-- Modified date:	12 Sep 2017
-- Description:		Check not activated by whether password is null regardless of the status
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0026
-- Modified by:		Koala CHENG
-- Modified date:	3 Dec 2013
-- Description:		Fix incorrect recipient demographics retrieval, specify varchar(40) when decrypt
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE11-014
-- Modified by:		Koala CHENG
-- Modified date:	12 July 2011
-- Description:		Change Parameter name for new parameter from generic report submit control
--					Not use 'Between' for Date condition, use >=, <
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:   05 Mar 2009
-- Description:	    Add Report Parameters Summary
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Oct 2008
-- Description:	Statistics for getting voucher account and claim
--				1. Get related data to temp table (__VoucherClaimByVoucherAcc, _TransactionByVoucherRemainZero)
-- =============================================
CREATE PROCEDURE [dbo].[proc_Report_RR01_NotActivated]
	@request_time		datetime,
	@From_Date			varchar(255),
	@To_Date			varchar(255)
AS
BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
declare @start_dtm as datetime
declare @end_dtm as datetime
-- =============================================
-- Validation 
-- =============================================

-- =============================================
-- Initialization
-- =============================================
SET @start_dtm = @From_Date
SET @end_dtm = @To_Date
-- =============================================
-- Return results
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

select '''' + s.sp_id as [SP ID],
	   convert(varchar(40), DecryptByKey(s.[Encrypt_Field2])) as [English Name],
	   isnull(convert(nvarchar, DecryptByKey(s.[Encrypt_Field3])),'') as [Chinese Name],
	   '''' + s.phone_daytime as [Phone No],
	   s.email as [Email],
	   s.effective_dtm as [Effective Time],
	   DATEADD(day, 21, s.effective_dtm)  as [Effective Time (+21 Days)],
	   s.record_status as [Record Status]
from serviceprovider s, hcspuserac a
where s.sp_id  = a.sp_id
-- and a.record_status = 'P' 
and isnull(a.[sp_password],'') = ''
and (s.effective_dtm >= @start_dtm and s.effective_dtm <  @end_dtm )
order by s.effective_dtm


CLOSE SYMMETRIC KEY sym_Key


-- Report Parameters Summary

DECLARE @tempSummary Table 
(
	Parameter varchar(100),
	Value varchar(50)	
)

INSERT INTO @tempSummary (Parameter, Value) Values ('','')
INSERT INTO @tempSummary (Parameter, Value) Values ('Request Date',CONVERT(VARCHAR(11), @request_time, 106) + ' ' + CONVERT(VARCHAR(8), @request_time, 108))
INSERT INTO @tempSummary (Parameter, Value) Values ('Generation Date',CONVERT(VARCHAR(11), GETDATE(), 106) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108))
INSERT INTO @tempSummary (Parameter, Value) Values ('','')
INSERT INTO @tempSummary (Parameter, Value) Values ('EHCP Effective Time From',CONVERT(VARCHAR(11), @start_dtm, 106) + ' ' + CONVERT(VARCHAR(5), @start_dtm, 108))
INSERT INTO @tempSummary (Parameter, Value) Values ('EHCP Effective Time To',CONVERT(VARCHAR(11), @end_dtm, 106) + ' ' + CONVERT(VARCHAR(5), @end_dtm, 108))


SELECT 
	Parameter as [Summary], Value as [ ]
FROM @tempSummary

END 
GO

GRANT EXECUTE ON [dbo].[proc_Report_RR01_NotActivated] TO HCVU
GO
