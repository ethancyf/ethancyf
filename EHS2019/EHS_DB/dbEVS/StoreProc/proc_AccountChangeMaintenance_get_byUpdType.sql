IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AccountChangeMaintenance_get_byUpdType]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AccountChangeMaintenance_get_byUpdType]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Chris YIM
-- Modified date:	11 Jul 2014
-- Description:		Add Column		- [SPAccountMaintenance].[Is_Share_Token]
--									  [SPAccountMaintenance].[Token_Serial_No_Replacement]
--								-     [SPAccountMaintenance].[Project_Replacement]
--								-     [SPAccountMaintenance].[Is_Share_Token_Replacement]
--					Change Column	- [Token].[Project] to [SPAccountMaintenance].[Project]


-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	19 Nov 2013
-- Description:		Add Column -	[SPAccountMaintenance].[Data_Input_By]
-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Modification History
-- CR No.:			CRE12-011
-- Modified by:	Tommy TSE
-- Modified date: 27 Apr 2012
-- Description:	Left join Token table to get the Project information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2009 May 26
-- Description:		Add Scheme_Code in Select
-- =============================================
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		03-06-2008
-- Description:		
-- =============================================

--proc_AccountChangeMaintenance_get_byUpdType null,1,1,0

CREATE PROCEDURE [dbo].[proc_AccountChangeMaintenance_get_byUpdType]
	@Action	varchar(2),
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @row_cnt_error varchar(max)
	DECLARE @rowcount int
		
-- =============================================    
-- Max Row Checking  
-- =============================================  


	BEGIN TRY       
		SELECT	@rowcount = COUNT(1)
		FROM	SPAccountMaintenance M
				LEFT JOIN ServiceProvider S
					ON M.SP_ID = S.SP_ID
		WHERE	M.Record_Status = 'A'
				AND (@Action IS NULL 
						OR M.Upd_Type = @Action)

		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		RETURN
	END CATCH 

-- =============================================
-- Select Table
-- =============================================
	EXEC [proc_SymmetricKey_open]

	SELECT	M.SP_ID SP_ID,
			CONVERT(varchar(40), DecryptByKey(s.Encrypt_Field2)) SP_Eng_Name,
			ISNULL(CONVERT(nvarchar,DecryptByKey(s.Encrypt_Field3)), '') SP_Chi_Name,
			M.Upd_Type Upd_Type,
			ISNULL(M.Scheme_Code, '') Scheme_Code,
			M.System_Dtm System_Dtm,
			ISNULL(M.Remark, '') Remark,
			ISNULL(M.Token_Serial_No, '') Token_Serial_No,
			ISNULL(M.Token_Remark, '') Token_Remark,
			ISNULL(M.SP_Practice_Display_Seq, 0) SP_Practice_Display_Seq,
			ISNULL(M.Delist_Status, '') Delist_Status,
			ISNULL(M.Update_by, '') Update_by,
			M.TSMP TSMP,
			ISNULL(M.Project, '') Project,
			M.Data_Input_By,
			ISNULL(M.Is_Share_Token,'') AS Is_Share_Token,
			ISNULL(M.Token_Serial_No_Replacement, '') Token_Serial_No_Replacement,
			ISNULL(M.Project_Replacement, '') Project_Replacement,
			ISNULL(M.Is_Share_Token_Replacement,'') AS Is_Share_Token_Replacement
			
	FROM	SPAccountMaintenance M
				LEFT JOIN ServiceProvider S
					ON M.SP_ID = S.SP_ID
					
	WHERE	M.Record_Status = 'A'
				AND (@Action IS NULL 
						OR M.Upd_Type = @Action)
						
	ORDER BY SP_ID ASC

	EXEC [proc_SymmetricKey_close]
END 
GO

GRANT EXECUTE ON [dbo].[proc_AccountChangeMaintenance_get_byUpdType] TO HCVU
GO
