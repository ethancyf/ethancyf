
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_TransactionAdditionalField_check_byAny]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_TransactionAdditionalField_check_byAny];
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
-- Author:		Martin Tang
-- CR No.:		CRE20-023
-- Create date: 20 May 2021
-- Description:	Check Any Transaction is used by TransactionAdditionalField
-- =============================================
CREATE PROCEDURE [dbo].[proc_TransactionAdditionalField_check_byAny] @AdditionalFieldID        VARCHAR(20) = NULL, 
                                                                     @AdditionalFieldValueCode VARCHAR(50) = NULL
AS
    BEGIN
        SET NOCOUNT ON;
        -- =============================================
        -- Declaration
        -- =============================================        
        -- =============================================
        -- Validation 
        -- =============================================
        -- =============================================
        -- Initialization
        -- =============================================
        -- =============================================
        -- Return results
        -- =============================================

        IF EXISTS
                 (
                     SELECT 1
                     FROM TransactionAdditionalField WITH(NOLOCK)
                     WHERE AdditionalFieldID = COALESCE(@AdditionalFieldID, AdditionalFieldID)
                           AND COALESCE(AdditionalFieldValueCode, '') = COALESCE(@AdditionalFieldValueCode, AdditionalFieldValueCode, '')
                  )
            BEGIN
                SELECT 1;
            END;
            ELSE
            BEGIN
                SELECT 0;
            END;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_TransactionAdditionalField_check_byAny] TO HCVU;

GO