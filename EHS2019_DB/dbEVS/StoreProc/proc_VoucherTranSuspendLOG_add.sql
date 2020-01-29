IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTranSuspendLOG_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTranSuspendLOG_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Clark Yip
-- Create date:		18 Jun 2008
-- Description:		Insert record in VoucherTranSuspendLOG table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 September 2009
-- Description:		Add @original_record_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTranSuspendLOG_add]
	@transaction_id				char(20),
	@system_dtm					datetime,
	@update_by					varchar(20),
	@original_record_status		char(1),
	@record_status				char(1),
	@remark						nvarchar(255)
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

	INSERT INTO VoucherTranSuspendLOG (
		[Transaction_ID],
		[System_Dtm],
		[Update_By],
		[Original_Record_Status],
		[Record_Status],
		[Remark]
	)
	VALUES (
		@transaction_id,
		@system_dtm,
		@update_by,
		@original_record_status,
		@record_status,
		@remark
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTranSuspendLOG_add] TO HCVU
GO
