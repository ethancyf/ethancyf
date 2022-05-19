
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartProfileLog_upd]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartProfileLog_upd];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-011   
-- Author:  Nichole IP 
-- Create date:  12  Jul 2021   
-- Description:  Update IAMSmartProfileLog as call back from iAM Smart
-- =============================================   
CREATE PROCEDURE [dbo].[proc_IAMSmartProfileLog_upd] @BusinessID  NVARCHAR(255), 
                                                     @State       NVARCHAR(255), 
                                                     @Return_Code NVARCHAR(10), 
                                                     @Message     NVARCHAR(100), 
                                                     @Content     NVARCHAR(1500)
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_BusinessID NVARCHAR(255);
        DECLARE @l_State NVARCHAR(255);
        DECLARE @l_Return_Code NVARCHAR(10);
        DECLARE @l_Message NVARCHAR(100);
        DECLARE @l_Content NVARCHAR(500);

        -- =============================================  
        -- Initialization  
        -- ============================================= 
        SET @l_BusinessID = @BusinessID;
        SET @l_State = @State;
        SET @l_Return_Code = @Return_Code;
        SET @l_Message = @Message;
        SET @l_Content = @Content;

        -- =============================================
        -- Return results
        -- =============================================   
        EXEC [proc_SymmetricKey_open];
        UPDATE IAMSmartProfileLog
          SET 
              Return_Code = @l_Return_Code, 
              [Message] = @l_Message, 
              Encrypted_Content = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_Content),
			  Update_Dtm = GETDATE()
        WHERE BusinessID = @l_BusinessID
              AND State = @l_State;
        EXEC [proc_SymmetricKey_close];
    END;  
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartProfileLog_upd] TO HCSP;
GO

GRANT EXECUTE ON [dbo].[proc_IAMSmartProfileLog_upd] TO WSEXT;
GO

