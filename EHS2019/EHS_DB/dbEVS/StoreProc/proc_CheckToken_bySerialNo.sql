 IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_CheckToken_bySerialNo]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_CheckToken_bySerialNo];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================  
-- CRE20-018 Stop Token Sharing
-- Modified by:  Nichole Ip 
-- Modified date: 13/01/2021  
-- Description:  Check the token serial no on TokenEHR table
-- =============================================  
CREATE procedure [dbo].[proc_CheckToken_bySerialNo]  
@Serial_No  varchar(20)  
  
as  
BEGIN  
-- =============================================  
-- Return results  
-- =============================================  

IF EXISTS
(
    SELECT ISNULL(Token_serial_No, '') AS IsEHR
    FROM TokenEHR WITH (NOLOCK)
    WHERE Token_Serial_No = @Serial_No
)
    BEGIN
        SELECT 'Y' as IsEHR;
    END;
    ELSE
    BEGIN
        SELECT 'N' as IsEHR;
    END;


--SELECT   
--isnull(Token_serial_No, '')  as IsEHR
--FROM TokenEHR
--WHERE  
--Token_Serial_No = @Serial_No 


END
GO

GRANT EXECUTE ON [dbo].[proc_CheckToken_bySerialNo] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_CheckToken_bySerialNo] TO HCSP
GO
