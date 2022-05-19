
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartSPMapping_del]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    DROP PROCEDURE [dbo].[proc_IAMSmartSPMapping_del];
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
-- CR No: CRE20-011
-- Author:		Nichole Ip
-- Create date: 23 Jul 2021
-- Description:	Delete IAMSmart Service Provider Mapping Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_IAMSmartSPMapping_del] @SP_ID CHAR(8), 
                                                    @tsmp  TIMESTAMP
AS
    BEGIN
        SET NOCOUNT ON;
        -- =============================================
        -- Declaration
        -- =============================================
        DECLARE @l_SP_ID CHAR(8);
        -- =============================================
        -- Initialization
        -- =============================================
        SET @l_SP_ID = @SP_ID;
        -- =============================================
        -- Validation 
        -- =============================================

        IF
        (
            SELECT TSMP
            FROM IAMSmartSPMapping
            WHERE SP_ID = @l_SP_ID
        ) != @tsmp
            BEGIN
                RAISERROR('00011', 16, 1);
                RETURN @@error;
            END;

        -- =============================================
        -- Return results
        -- =============================================

        DELETE FROM [dbo].[IAMSmartSPMapping]
        WHERE SP_ID = @l_SP_ID;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartSPMapping_del] TO HCSP;
GO