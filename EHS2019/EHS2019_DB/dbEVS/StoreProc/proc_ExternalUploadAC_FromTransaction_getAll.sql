IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExternalUploadAC_FromTransaction_getAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExternalUploadAC_FromTransaction_getAll]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Derek LEUNG
-- Create date: 05 October 2010
-- Description:	Retrieve External Upload System Names From Transactions Submitted
-- =============================================


CREATE PROCEDURE [dbo].[proc_ExternalUploadAC_FromTransaction_getAll]  
 @SP_ID char(8),   
 @SP_Practice_Display_Seq smallint  
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
  
 SELECT distinct dataentry_by   
 FROM vouchertransaction  
 WHERE IsUpload = 'Y'  
 AND sp_id = @SP_ID   
 and (@SP_Practice_Display_Seq is null or Practice_Display_Seq = @SP_Practice_Display_Seq)      
 order by dataentry_by      
    
END  

GO

GRANT EXECUTE ON [dbo].[proc_ExternalUploadAC_FromTransaction_getAll] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ExternalUploadAC_FromTransaction_getAll] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ExternalUploadAC_FromTransaction_getAll] TO WSEXT
GO
