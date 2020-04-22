IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeItem_get_bySubsidizeCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeItem_get_bySubsidizeCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History      
-- CR No.:			CRE16-021 Transfer VSS category to PCD
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2016
-- Description:		Grant permission to WSINT and HCPUBLIC for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-019-02 Extend HCVS to China
-- Modified by:	    Chris YIM
-- Modified date:   12 Feb 2015
-- Description:	    Add three columns [Subsidize].[Legend_Desc],[Subsidize].[Legend_Desc_Chi], [Subsidize].[Legend_Desc_CN]
--					Remove two colums [SubsidizeItem].[subsidize_item_desc] and [SubsidizeItem].[subsidize_item_desc_chi]
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		04 Aug 2009
-- Description:		Get SubsidizeItem by SubsidizeCode
-- =============================================


CREATE PROCEDURE 	[dbo].[proc_SubsidizeItem_get_bySubsidizeCode]
						@subsidize_code char(10)
as
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
	s.Subsidize_code
	, s.subsidize_item_code
	, s.display_code
	, s.Display_Seq
	, s.record_status as Subsidize_Record_Status
--	, i.subsidize_item_desc
--	, i.subsidize_item_desc_chi
	, s.Legend_Desc as [subsidize_item_desc]
	, s.Legend_Desc_Chi as [subsidize_item_desc_chi]
	, s.Legend_Desc_CN as [subsidize_item_desc_cn]
	, i.subsidize_type
	, i.record_status as SubsidizeItem_Record_Status
	, s.Create_by as Subsidize_Create_by
	, s.Create_dtm as Subsidize_Create_dtm
	, s.Update_by as Subsidize_Update_By
	, s.Update_dtm as Subsidize_Update_Dtm
	, i.Create_by as SubsidizeItem_Create_by
	, i.Create_dtm as SubsidizeItem_Create_Dtm
	, i.Update_by as SubsidizeItem_Update_by
	, i.Update_dtm as SubsidizeItem_Update_dtm
	FROM	Subsidize s, SubsidizeItem i
	WHERE
		s.subsidize_item_code=i.subsidize_item_code
		and s.Subsidize_code=@subsidize_code		
	ORDER BY s.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItem_get_bySubsidizeCode] TO HCSP, HCVU, HCPUBLIC, WSINT
GO
