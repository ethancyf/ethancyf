
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartSPMapping_get_bySPID]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartSPMapping_get_bySPID];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-011   
-- Author:  Nichole IP 
-- Create date:  17 Jun 2021   
-- Description: Get the IAMSmart Tokenised Info by SP ID
-- =============================================   
CREATE PROCEDURE [dbo].[proc_IAMSmartSPMapping_get_bySPID] @SP_ID CHAR(8)  
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  
		DECLARE @l_SP_ID CHAR(8);
		 
        -- =============================================  
        -- Initialization  
        -- ============================================= 
		SET @l_SP_ID=@SP_ID
		 
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT SP_ID, 
               OpenID, 
               --Record_Status,
			   TSMP
        FROM IAMSmartSPMapping  with (nolock)
        WHERE SP_ID = @l_SP_ID  

    END;  
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartSPMapping_get_bySPID] TO HCSP;
GO

GRANT EXECUTE ON [dbo].[proc_IAMSmartSPMapping_get_bySPID] TO HCVU;
GO
