IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'ProfessionalSubmissionResult_after_upd')
	DROP TRIGGER [dbo].[ProfessionalSubmissionResult_after_upd] 

GO	

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Unknown 
-- Create date: Unknown (missing in Visual Source Safe, add back on 20-Oct-2014
-- Description:	when a row is updated / inserted to ProfessionalSubmissionResult
--				Trigger an insert into ProfessionalSubmissionResultLOG				
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:  
-- Description:	   
-- =============================================  
  
CREATE Trigger [dbo].[ProfessionalSubmissionResult_after_upd]  
 ON [ProfessionalSubmissionResult]  
 AFTER INSERT, UPDATE  
   
  
 AS  
BEGIN  
 SET NOCOUNT ON;  
  
-- deleted: old values  
-- inserted: new values  
  
INSERT INTO [dbo].[ProfessionalSubmissionResultLOG]  
(  
   
 [System_Dtm],  
 [File_Name],  
 [Reference_No],  
 [Result],  
 [Remark]   
)  
(  
 SELECT  
  GetDate(),  
  [File_Name],  
  [Reference_No],  
  [Result],  
  [Remark]    
 FROM inserted  
)  
  
END   

GO
  