IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfo_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfo_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	5 August 2016
-- Description:		Add Clinic_Type
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- Description:		Add field [Provide_Service],[Delist_Status],[Delist_Dtm],[Remark]
-- =============================================
-- =============================================
-- Author:			Clark YIP
-- Create date:		18-05-2009
-- Description:		Insert practice scheme information record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date:  02 Jul 2009
-- Description:	   Add the master scheme code, ProvideServiceFee, effective dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	04 Aug 2009
-- Description:		1. Rename MScheme_Code -> Scheme_Code
--					2. Rename Scheme_Code -> Subsidize_Code
-- =============================================
CREATE Procedure [dbo].[proc_PracticeSchemeInfo_add]
	@sp_id char(8), @scheme_code char(10), @practice_display_seq smallint, 
	@service_fee smallint, @record_status char(1), @create_by varchar(20), @update_by varchar(20),
	@subsidize_code char(10), @ProvideServiceFee char(1), @Provide_Service char(1),
	@Delist_Status char(1), @Delist_Dtm datetime, @Remark nvarchar(255),
	@Clinic_Type char(1)
AS BEGIN

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

	Insert into PracticeSchemeInfo
       ([SP_ID]
       ,[Subsidize_Code]
       ,[Practice_Display_Seq]
       ,[Service_Fee]
       ,[Delist_Status]
       ,[Delist_Dtm]
       ,[Effective_Dtm]
       ,[Record_Status]
       ,[Remark]
       ,[Create_Dtm]
       ,[Create_By]
       ,[Update_Dtm]
       ,[Update_By]
       ,[Scheme_Code]
       ,[ProvideServiceFee]
       ,[Provide_Service]
	   ,[Clinic_Type])
	VALUES		
		(@sp_id
		,@subsidize_code		
		,@practice_display_seq
		,@service_fee
		,@Delist_Status
		,@Delist_Dtm
		,getdate()
		,@record_status		
		,@Remark
		,getdate()	
		,@create_by	
		,getdate()
		,@update_by
		,@scheme_code
		,@ProvideServiceFee		
		,@Provide_Service
		,@Clinic_Type)
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfo_add] TO HCVU
GO
