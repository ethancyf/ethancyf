IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VaccineCentreVUMapping_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VaccineCentreVUMapping_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- Modified by:    
-- Modified date:   
-- CR No.:     
-- Description:    
-- =============================================   
-- =============================================  
-- Modification History  
-- Created by:  Raiman Chong  
-- Created date: 29 Mar 2021  
-- CR No.:   CRE20-0023  
-- Description: Add Vaccine Centre for HCVU User Maintenance Function
-- =============================================  

CREATE Procedure dbo.proc_VaccineCentreVUMapping_add
@User_ID		varchar(20)
,  @Centre_ID		varchar(10)
, @CreateBy	    varchar(20)
as
Begin
-- =============================================  
-- Declaration  
-- =============================================  
declare @Create_Dtm	as datetime
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- ============================================= 
select @Create_Dtm = getdate() 
-- =============================================  
-- Return results  
-- =============================================  

INSERT INTO [dbo].[VaccineCentreVUMapping]
           ([Centre_ID]
           ,[User_ID]
           ,[Create_By]
           ,[Create_Dtm]
           ,[Update_By]
           ,[Update_Dtm])
     VALUES
           (@Centre_ID
           ,@User_ID
           ,@CreateBy
           ,@Create_Dtm
           ,@CreateBy
           ,@Create_Dtm)

END;
GO

GRANT EXECUTE ON [dbo].[proc_VaccineCentreVUMapping_add] TO HCVU
GO
