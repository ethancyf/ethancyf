IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR# : CRE13-019-02
-- Modified by:	Karl LAM
-- Modified date:	05 Jan 2015
-- Description:	Add @ExchangeRate_Value, @Total_Amount_RMB
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modified by:		Kathy LEE
-- Modified date:	8 Jul 2010
-- Description:		Grant Right to HCVU
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 31 Aug 2009
-- Description:	Insert Transaction Detail
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_TransactionDetail_add]
	@Transaction_ID			char(20),
	@Scheme_Code			char(10),
	@Scheme_Seq				smallint,
	@Subsidize_Code			char(10),
	@Subsidize_Item_Code	char(10),
	@Available_item_Code	char(20),
	@Unit					int,
	@Per_Unit_Value			money,
	--@Total_Amount			money,
	@Remark					nvarchar(255),
	@ExchangeRate_Value		Decimal(9,3),
	@Total_Amount_RMB		money
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

	INSERT INTO [TransactionDetail]
	(
		[Transaction_ID],
		[Scheme_Code],
		[Scheme_Seq],
		[Subsidize_Code],
		[Subsidize_Item_Code],
		[Available_item_Code],
		[Unit],
		[Per_Unit_Value],
		[Total_Amount],
		[Remark],
		[ExchangeRate_Value],
		[Total_Amount_RMB]
	)
	VALUES
	(
		@Transaction_ID,
		@Scheme_Code,
		@Scheme_Seq,
		@Subsidize_Code,
		@Subsidize_Item_Code,
		@Available_item_Code,
		@Unit,
		@Per_Unit_Value,
		@Unit * @Per_Unit_Value, --@Total_Amount
		@Remark,
		@ExchangeRate_Value,
		@Total_Amount_RMB
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_add] TO WSEXT
Go