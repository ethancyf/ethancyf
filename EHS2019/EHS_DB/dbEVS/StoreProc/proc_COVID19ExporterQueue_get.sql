
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19ExporterQueue_get]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19ExporterQueue_get];
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
-- Description:		 Assign file ID and get Records from COVID19ExporterQueue
-- =============================================
CREATE PROCEDURE [dbo].[proc_COVID19ExporterQueue_get] @File_ID VARCHAR(27)
AS
    BEGIN

        DECLARE @ln_file_id AS VARCHAR(27)= @File_ID;
        DECLARE @PendingStatus AS CHAR(1)= 'P';
        DECLARE @ProcessingStatus AS CHAR(1)= 'R';
        DECLARE @SkipedStatus AS CHAR(1)= 'S';
        DECLARE @intMaxRecods AS INT;

        SELECT @intMaxRecods = Parm_Value1
        FROM SystemParameters
        WHERE Parameter_Name = 'COVID19ExportFileBatchSize';

        EXEC [proc_SymmetricKey_open];

        UPDATE TOP (@intMaxRecods) COVID19ExporterQueue
          SET Record_File_ID = @ln_file_id, 
              Record_Status = @ProcessingStatus
        WHERE Record_Status = @PendingStatus;

        -------------------------------------------------------------
        --(Double checking)The transactions are skipped to send in COVID19ExporterException table 
        ------------------------------------------------------------
        UPDATE ceq
          SET Record_Status = @SkipedStatus
        FROM COVID19ExporterQueue ceq
             INNER JOIN COVID19ExporterException cee
             ON ceq.Transaction_ID = cee.Transaction_ID
        WHERE ceq.Record_Status = @ProcessingStatus;
        ------------------------------------------------------------
        SELECT LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(Encrypt_Field1)))), 
               ceq.Transaction_ID
        FROM COVID19ExporterQueue AS ceq
        WHERE ceq.Record_File_ID = @ln_file_id
              AND ceq.Record_Status = @ProcessingStatus;

        EXEC [proc_SymmetricKey_close];
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19ExporterQueue_get] TO HCVU;
GO