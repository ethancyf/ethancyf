
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartStateLog_get_ByState]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartStateLog_get_ByState];
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
-- Created date:	2 Aug 2021
-- CR No.:			CRE20-0011
-- Description:		Get state from iAMSmartStateLog table
-- =============================================

CREATE PROCEDURE [dbo].[proc_IAMSmartStateLog_get_ByState] 
	@State NVARCHAR(255)
AS
    BEGIN
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_State NVARCHAR(255);
        -- =============================================  
        -- Initialization  
        -- =============================================
        SET @l_State = @State; 
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT [State], 
               [CookieKey], 
               [ExpiresTime], 
               [Create_Dtm]
        FROM IAMSmartStateLog
        WHERE [State] = @l_State;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_IAMSmartStateLog_get_ByState] TO HCSP

GRANT EXECUTE ON [dbo].[proc_IAMSmartStateLog_get_ByState] TO WSEXT

GO

