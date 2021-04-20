IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VaccineCentreVUMapping_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VaccineCentreVUMapping_del]
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
-- Description: Delete Vaccine Centre for HCVU User Maintenance Function
-- =============================================  

CREATE Procedure dbo.proc_VaccineCentreVUMapping_del
@User_ID	varchar(20)
, @Centre_ID		varchar(10)
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
Delete from [VaccineCentreVUMapping]
where User_ID = @User_ID
and (@Centre_ID is null or Centre_ID = @Centre_ID)

END;
GO

GRANT EXECUTE ON [dbo].[proc_VaccineCentreVUMapping_del] TO HCVU
GO
