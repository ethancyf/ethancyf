   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_TempVoucherAccMatchLOG_get_byDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_TempVoucherAccMatchLOG_get_byDate]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 27 May 2010
-- Description:	Add New fields
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 2 Oct 2009
-- Description:	Add New fields
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Get Immd Return File Result From TempVoucherAccMatchLOG
-- =============================================

CREATE PROCEDURE [dbo].[proc_TempVoucherAccMatchLOG_get_byDate]
	@system_Dtm	datetime,
	@File_Name varchar(100)
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
		TVAML.Valid_HKID,
		TVAML.Voucher_Acc_ID,
		TVAML.Processed,
		TVASL.Record_Status,
		TVASL.Acc_Type
	FROM [dbo].[TempVoucherAccMatchLOG] TVAML
		INNER JOIN [dbo].[TempVoucherAccSubmissionLOG] TVASL ON 
			TVAML.Voucher_Acc_ID = TVASL.Voucher_Acc_ID AND
			TVAML.System_Dtm = TVASL.System_Dtm AND
			TVAML.File_Name = TVASL.File_Name
	WHERE
			TVAML.System_Dtm = @system_Dtm AND
			TVAML.File_Name = @File_Name
		--DATEPART(YEAR,TVASL.System_Dtm) = DATEPART(YEAR,@System_Dtm) AND DATEPART(MONTH,TVASL.System_Dtm) = DATEPART(MONTH,@System_Dtm) AND DATEPART(DAY,TVASL.System_Dtm) = DATEPART(DAY,@System_Dtm)
		

END

GO

Grant execute on [dbo].[proc_TempVoucherAccMatchLOG_get_byDate] to HCVU
GO 