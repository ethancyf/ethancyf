
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_IAMSmartProfileLog_add]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_IAMSmartProfileLog_add];
    END;
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================     
-- CR No.:  CRE20-011   
-- Author:  Nichole IP 
-- Create date:  21  Jul 2021   
-- Description:  Add iAMSmart Profile record
-- ============================================= 
CREATE PROCEDURE [dbo].[proc_IAMSmartProfileLog_add] @BusinessID  NVARCHAR(255), 
                                                     @State       NVARCHAR(255), 
                                                     @Return_Code NVARCHAR(10), 
                                                     @Message     NVARCHAR(100), 
                                                     @Content     NVARCHAR(500)
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

		IF @l_Content IS NULL 
		BEGIN
			INSERT INTO [dbo].[IAMSmartProfileLog]
			([System_Dtm],
			 [State],
			 [BusinessID], 
			 [Return_Code], 
			 [Message], 
			 [Encrypted_Content], 
			 [Create_Dtm]
			)
			VALUES
			(GETDATE(),
			 @l_State,
			 @l_BusinessID, 
			 @l_Return_Code, 
			 @l_Message, 
			 NULL, 
			 GETDATE()
			);
		END
		ELSE
		BEGIN
			INSERT INTO [dbo].[IAMSmartProfileLog]
			([BusinessID], 
			 [State], 
			 [Return_Code], 
			 [Message], 
			 [Encrypted_Content], 
			 [Create_Dtm]
			)
			VALUES
			(@l_BusinessID, 
			 @l_State, 
			 @l_Return_Code, 
			 @l_Message, 
			 ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_Content), 
			 GETDATE()
			);
		END

        EXEC [proc_SymmetricKey_close];
    END;
GO
GRANT EXECUTE ON [dbo].[proc_IAMSmartProfileLog_add] TO HCSP;
GO

