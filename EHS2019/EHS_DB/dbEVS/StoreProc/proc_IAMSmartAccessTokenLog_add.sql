
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartAccessTokenLog_add]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartAccessTokenLog_add];
    END;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================  
-- Modification History  
-- Modified by:   
-- Modified date:   
-- Description:   
-- =============================================  
-- =============================================
-- Modification History
-- Created by:		Nichole Ip
-- Created date:	11 Jul 2021
-- CR No.:			CRE20-0011
-- Description:		Add IAMSmartAccessTokenLog record from iamsmart 
-- =============================================
CREATE PROCEDURE [dbo].[proc_IAMSmartAccessTokenLog_add] @State     NVARCHAR(255), 
														 @TokenID     NVARCHAR(255),
                                                         @Return_Code NVARCHAR(10), 
                                                         @Message     NVARCHAR(100), 
                                                         @AccessToken NVARCHAR(255), 
                                                         @OpenID      NVARCHAR(255), 
                                                         @Content     NVARCHAR(500)
AS
    BEGIN
        -- =============================================  
        -- Declaration  
        -- =============================================  
		DECLARE @l_State NVARCHAR(255);
        DECLARE @l_TokenID NVARCHAR(255);
        DECLARE @l_Return_Code NVARCHAR(10);
        DECLARE @l_Message NVARCHAR(100);
        DECLARE @l_AccessToken NVARCHAR(255);
        DECLARE @l_OpenID NVARCHAR(255);
        DECLARE @l_Content NVARCHAR(500);
        -- =============================================  
        -- Initialization  
        -- =============================================
        SET @l_State = @State;
		SET @l_TokenID = @TokenID;
        SET @l_Return_Code = @Return_Code;
        SET @l_Message = @Message;
        SET @l_AccessToken = @AccessToken;
        SET @l_OpenID = @OpenID;
        SET @l_Content = @Content;

        -- =============================================
        -- Return results
        -- =============================================   
        INSERT INTO [IAMSmartAccessTokenLog]
        ([System_Dtm],
		 [State],
		 [TokenID], 
         [Return_Code], 
         [Message], 
         [AccessToken], 
         [OpenID], 
         [Content], 
         [Create_Dtm]
        )
        VALUES
        (GETDATE(),
		 @l_State,
		 @l_TokenID, 
         @l_Return_Code, 
         @l_Message, 
         @l_AccessToken, 
         @l_OpenID, 
         @l_Content, 
         GETDATE()
        );
    END;
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartAccessTokenLog_add] TO HCSP;
GO


GRANT EXECUTE ON [dbo].[proc_IAMSmartAccessTokenLog_add] TO WSEXT;
GO

