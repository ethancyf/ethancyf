
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartStateLog_add]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartStateLog_add];
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
-- Created date:	21 Jul 2021
-- CR No.:			CRE20-0011
-- Description:		Add state from direct login 
-- =============================================
CREATE PROCEDURE [dbo].[proc_IAMSmartStateLog_add] @State   NVARCHAR(255) , 
                                                   @CookieKey NVARCHAR(255),
												   @ExpiresTime DATETIME
AS
    BEGIN
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_State NVARCHAR(255);
        DECLARE @l_CookieKey NVARCHAR(255);
		DECLARE @l_ExpiresTime DATETIME;
        -- =============================================  
        -- Initialization  
        -- =============================================
        SET @l_State = @State;
        SET @l_CookieKey = @CookieKey; 
		SET @l_ExpiresTime = @ExpiresTime;
        -- =============================================
        -- Return results
        -- =============================================   
        INSERT INTO [dbo].[IAMSmartStateLog]
        ([System_Dtm],
		 [State], 
         [CookieKey], 
		 [ExpiresTime],
         [Create_Dtm]
        )
        VALUES
        (GETDATE(),
		 @l_State, 
         @l_CookieKey, 
		 @l_ExpiresTime ,
         GETDATE()
        );
    END;
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartStateLog_add] TO HCSP;

GRANT EXECUTE ON [dbo].[proc_IAMSmartStateLog_add] TO WSEXT;

GO