IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionDetail_get_byTranID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TransactionDetail_get_byTranID]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		25 Aug 2009
-- Description:		Get the TransactionDetail by TranID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:	30 Aug 2009
-- Description:	    Get Addition Field
--					[Available_item_Code], [Available_Item_Desc], [Available_Item_Desc_Chi]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 September 2009
-- Description:	    Adding ordering [SubsidizeGroupClaim].[Display_Seq] (ascending)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:	    
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_TransactionDetail_get_byTranID] 
	@tran_id		 varchar(20)
AS
BEGIN
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
		TD.[Transaction_ID],
		TD.[Scheme_Code],
		TD.[Scheme_Seq],
		TD.[Subsidize_Code],
		TD.[Subsidize_Item_Code],
		TD.[Available_item_Code],
		TD.[Unit],
		TD.[Per_Unit_Value],
		TD.[Total_Amount],
		TD.[Remark],
		TD.[ExchangeRate_Value],
        TD.[Total_Amount_RMB],
		SID.[Available_Item_Desc],
		SID.[Available_Item_Desc_Chi],
		SID.[Available_Item_Desc_CN]

	FROM 
		[TransactionDetail] TD
			INNER JOIN [SubsidizeItemDetails] SID
				ON TD.[Available_item_Code] = SID.[Available_item_Code]
					AND TD.[Subsidize_Item_Code] = SID.[Subsidize_Item_Code]
			INNER JOIN [SubsidizeGroupClaim] SGC
				ON TD.[Scheme_Code] = SGC.[Scheme_Code]
					AND TD.[Scheme_Seq] = SGC.[Scheme_Seq]
					AND TD.[Subsidize_Code] = SGC.[Subsidize_Code]	
					
	WHERE 
		TD.[Transaction_ID] = @tran_id
		
	ORDER BY
		SGC.[Display_Seq]
	
END
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byTranID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TransactionDetail_get_byTranID] TO HCVU
GO
