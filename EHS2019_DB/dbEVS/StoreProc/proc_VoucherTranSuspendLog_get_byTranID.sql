IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTranSuspendLog_get_byTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTranSuspendLog_get_byTranID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Get the VoucherTranSuspendLOG table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 September 2009
-- Description:		Retrieve [VoucherTranSuspendLog].[Original_Record_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTranSuspendLog_get_byTranID]
	@tran_id		 char(20)
AS BEGIN
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
		Update_By,
		Original_Record_Status,
		Record_Status,
		Remark 
		
	FROM
		VoucherTranSuspendLog 
		
	WHERE
		Transaction_ID = @tran_id
		
	ORDER BY
		System_Dtm

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTranSuspendLog_get_byTranID] TO HCVU
GO
