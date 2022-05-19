
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartProfileLog_get_byBusinessID]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartProfileLog_get_byBusinessID];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-011   
-- Author:  Nichole IP 
-- Create date:  14 Jul 2021   
-- Description:  Get the iAM Smart profile information by business ID
-- =============================================   
CREATE PROCEDURE [dbo].[proc_IAMSmartProfileLog_get_byBusinessID] @BusinessID NVARCHAR(255)
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_BusinessID NVARCHAR(255);

        -- =============================================  
        -- Initialization  
        -- ============================================= 
        SET @l_BusinessID = @BusinessID;

        -- =============================================
        -- Return results
        -- =============================================   
		EXEC [proc_SymmetricKey_open] 

        SELECT [BusinessID], 
			   [State],
               [Return_Code], 
               [Message], 
			   [Encrypted_Content] = CONVERT(NVARCHAR(MAX), DecryptByKey(Encrypted_Content))
        FROM IAMSmartProfileLog  WITH (NOLOCK)
        WHERE
			BusinessID = @l_BusinessID 
			AND Encrypted_Content IS NOT NULL

		EXEC [proc_SymmetricKey_close]  
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartProfileLog_get_byBusinessID] TO HCSP;
GO

