
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19ExporterQueue_update]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19ExporterQueue_update];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-00XX (Immu record)
-- Modified by:		Martin Tang
-- Modified date:	31 Dec 2020
-- Description:		update COVID19ExporterQueue
-- =============================================
CREATE PROCEDURE [dbo].[proc_COVID19ExporterQueue_update] @File_ID        VARCHAR(27) = NULL, 
                                                          @Transaction_ID VARCHAR(20) = NULL, 
                                                          @FromStatus     CHAR(1)     = NULL, 
                                                          @ToStatus       CHAR(1)
AS
    BEGIN

        DECLARE @ln_TransactionID AS VARCHAR(20)= @Transaction_ID;
        DECLARE @ln_FileID AS VARCHAR(27)= @File_ID;
        DECLARE @ln_FromStatus AS CHAR(1)= @fromStatus;
        DECLARE @ln_ToStatus AS CHAR(1)= @toStatus;
        DECLARE @SkipedStatus AS CHAR(1)= 'S';

        UPDATE COVID19ExporterQueue
          SET Record_Status = @ln_toStatus, 
              Update_Dtm = GETDATE()
        WHERE(@ln_fromStatus IS NULL
              OR Record_Status = @ln_fromStatus)
             AND (@ln_FileID IS NULL
                  OR Record_File_ID = @ln_FileID)
             AND (@ln_TransactionID IS NULL
                  OR Transaction_ID = @ln_TransactionID);
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19ExporterQueue_update] TO HCVU;
GO