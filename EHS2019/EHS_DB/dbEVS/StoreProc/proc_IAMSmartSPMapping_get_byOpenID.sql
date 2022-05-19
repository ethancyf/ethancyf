
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartSPMapping_get_byOpenID]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartSPMapping_get_byOpenID];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-011   
-- Author:  Nichole IP 
-- Create date:  28 Jun 2021   
-- Description:  Get the iAM Smart mapping information
-- =============================================   
CREATE PROCEDURE [dbo].[proc_IAMSmartSPMapping_get_byOpenID] @OpenID NVARCHAR(255)
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_OpenID NVARCHAR(255);
        -- =============================================  
        -- Initialization  
        -- ============================================= 
        SET @l_OpenID = @OpenID; 
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT SP_ID, 
               OpenID, 
               --Record_Status, 
               TSMP
        FROM IAMSmartSPMapping WITH(NOLOCK)
        WHERE OpenID = @l_OpenID;
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartSPMapping_get_byOpenID] TO HCSP;
GO