    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_TempVoucherAccSubHeader_get_byStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_TempVoucherAccSubHeader_get_byStatus]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- Modified by:		Dedrick Ng
-- Modified date:	24 Mar 2010
-- Description:		Add Doc_Code
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Get TempVoucherAccSubHeader By Record Status
--				P:Pend, E:Export, I:Import, C: Complete
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccSubHeader_get_byStatus]
	@Record_Status	char(1)
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

	SELECT 
		System_Dtm,
		[File_Name],
		[Record_Status],
		[Join_Doc_Code]		
	FROM [dbo].[TempVoucherAccSubHeader]
	WHERE
		[Record_Status] = @Record_Status
	ORDER BY System_Dtm DESC

END
GO

--Grant Access Right to user
--HCVU (for voucher unit platform)
--HCSP (for service provider platform)
--HCPUBLIC (for public access platform)
Grant execute on [dbo].[proc_TempVoucherAccSubHeader_get_byStatus] to HCVU
GO       