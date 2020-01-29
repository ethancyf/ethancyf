 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceSPPractice_get_bySPIDPractice]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceSPPractice_get_bySPIDPractice]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================    
-- Modification History    
-- Modified by:   Derek LEUNG   
-- Modified date: 23-Nov-2010  
-- Description:   Add system name to where clause  
-- =============================================    
-- =============================================    
-- Author:  Derek LEUNG  
-- Create date: 10 Nov 2010  
-- Description: Retrieve data from InterfaceSPPractice table    
-- =============================================    
  
CREATE PROCEDURE [dbo].[proc_InterfaceSPPractice_get_bySPIDPractice]    
 @SP_ID char(8),   
 @Practice_Display_Seq int  ,   
 @System_Name char(20)  
AS    
BEGIN    
 SET NOCOUNT ON;    
   
 SELECT * FROM InterfaceSPPractice   
 WHERE SP_ID = @SP_ID AND  
   Practice_Display_Seq = @Practice_Display_Seq AND  
   Interface_Sys = @System_Name  
  
END    

GO

GRANT EXECUTE ON [dbo].[proc_InterfaceSPPractice_get_bySPIDPractice] TO WSEXT
GO