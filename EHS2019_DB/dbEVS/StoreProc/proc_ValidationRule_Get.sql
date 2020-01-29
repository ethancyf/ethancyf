IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ValidationRule_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ValidationRule_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================  
-- Modified By:  Derek LEUNG  
-- Modified date: 17 September 2010  
-- Description:  Add WarnIndicatorCode  
-- =============================================  
-- =============================================  
-- Author:  Derek LEUNG  
-- Create date: 28 July 2010  
-- Description: Get all validation rules  
-- =============================================  
CREATE PROCEDURE [dbo].[proc_ValidationRule_get]  
  
AS  
BEGIN  
   
 SET NOCOUNT ON;  
  
     SELECT  RuleSeq  
   ,RuleID  
   ,ClaimType  
   ,HandlingMethod  
   ,Scheme_Code  
   ,Scheme_Seq  
   ,Subsidize_Code  
   ,Rule_Group_ID1  
   ,Rule_Group_ID2  
   ,Function_Code  
   ,Severity_Code  
   ,Message_Code  
   ,WarnIndicatorCode   
 FROM ValidationRule  
  
END    
GO

GRANT EXECUTE ON [dbo].[proc_ValidationRule_Get] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ValidationRule_Get] TO WSEXT
GO