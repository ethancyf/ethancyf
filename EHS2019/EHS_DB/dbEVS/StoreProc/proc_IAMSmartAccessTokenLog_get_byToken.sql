
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartAccessTokenLog_get_byToken]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartAccessTokenLog_get_byToken];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-011   
-- Author:  Nichole IP 
-- Create date:  20 Jul 2021   
-- Description: Get access token information
-- =============================================   
CREATE PROCEDURE [dbo].[proc_IAMSmartAccessTokenLog_get_byToken] @TokenID NVARCHAR(255)  
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_TokenID NVARCHAR(255);
        -- =============================================  
        -- Initialization  
        -- ============================================= 
        SET @l_TokenID = @TokenID;
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT [TokenID], 
               [Return_code], 
               [Message], 
               [AccessToken], 
               [OpenID], 
               [Content]
        FROM IAMSmartAccessTokenLog  with (nolock)
        WHERE TokenID = @l_TokenID;
    END;  
GO

GRANT EXECUTE ON [dbo].[proc_IAMSmartAccessTokenLog_get_byToken] TO HCSP;
GO

