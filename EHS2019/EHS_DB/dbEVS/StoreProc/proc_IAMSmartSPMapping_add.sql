
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartSPMapping_add]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartSPMapping_add];
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
-- Description:		Add iAM Smart Service Provider Mapping record
-- =============================================
CREATE PROCEDURE [dbo].[proc_IAMSmartSPMapping_add] @SP_ID   CHAR(8) , 
                                                    @OpenID NVARCHAR(255)
AS
    BEGIN
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_SP_ID CHAR(8);
        DECLARE @l_OpenID NVARCHAR(255);
        -- =============================================  
        -- Initialization  
        -- =============================================
        SET @l_SP_ID = @SP_ID;
        SET @l_OpenID = @OpenID; 
        -- =============================================
        -- Return results
        -- =============================================   
        INSERT INTO [dbo].[IAMSmartSPMapping]
        ([SP_ID], 
         [OpenID], 
         [Create_Dtm]
        )
        VALUES
        (@l_SP_ID, 
         @l_OpenID, 
         GETDATE()
        );
    END;
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartSPMapping_add] TO HCSP;
GO