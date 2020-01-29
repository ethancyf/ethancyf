IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RVPHomeList_get_byRCHInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RVPHomeList_get_byRCHInfo]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

  
-- =============================================  
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- CR No.:		CRE11-013  
-- Author:		Twinsen Chan  
-- Create date: 17 Oct 2012  
-- Description: Search the RVP Home List By RCH Information  
-- =============================================  

--exec proc_RVPHomeList_get_byRCHInfo null,null, 's','s',null,1,1,0

CREATE PROCEDURE [dbo].[proc_RVPHomeList_get_byRCHInfo]  
 @rch_code  varchar(10),  
 @rch_type  varchar(5),  
 @rch_name  nvarchar(255),  
 @rch_addr  nvarchar(255),  
 @rch_stat  nvarchar(1),
 @result_limit_1st_enable BIT, 
 @result_limit_override_enable BIT,
 @override_result_limit BIT   

AS   
BEGIN  
  
	SET NOCOUNT ON;  
	  
	-- =============================================
	-- Declaration
	-- =============================================
	DECLARE @rowcount INT
	DECLARE @row_cnt_error varchar(max)

	-- =============================================
	-- Initialization
	-- =============================================
	SET @rch_name = '%' + @rch_name + '%'  
	SET @rch_addr = '%' + @rch_addr + '%'  

	SELECT   TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
	  [RCH_code], [Type], [District], [Homename_Eng], [Homename_Chi],   
	  [Address_Eng], [Address_Chi], [Record_Status], [Create_By], [Create_Dtm],  
	  [Update_By], [Update_Dtm], [TSMP]  
	INTO #tempHomeList
	FROM RVPHomeList  
	WHERE   
	  ([RCH_Code] = @rch_code OR @rch_code IS NULL)  
	  AND ([Type] = @rch_type OR @rch_type IS NULL)  
	  AND ([Homename_Eng] LIKE @rch_name OR [Homename_Chi] LIKE @rch_name)  
	  AND ([Address_Eng] LIKE @rch_addr OR [Address_Chi] LIKE @rch_addr)  
	  AND ([Record_Status] = @rch_stat OR @rch_stat IS NULL)  

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
		SELECT	@rowcount = COUNT(1) FROM #tempHomeList
		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    
		RAISERROR (@row_cnt_error,16,1)    
		RETURN
	END CATCH 

	-- =============================================  
	-- Return results  
	-- =============================================  
	SELECT  
	  [RCH_code], [Type], [District], [Homename_Eng], [Homename_Chi],   
	  [Address_Eng], [Address_Chi], [Record_Status], [Create_By], [Create_Dtm],  
	  [Update_By], [Update_Dtm], [TSMP]  
	FROM #tempHomeList

	DROP TABLE #tempHomeList  

END

GO

GRANT EXECUTE ON [dbo].[proc_RVPHomeList_get_byRCHInfo] TO HCVU
GO
  