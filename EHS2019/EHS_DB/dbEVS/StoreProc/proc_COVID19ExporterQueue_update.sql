
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
-- CR No.:			CRE20-0023 
-- Modified by:		Martin Tang
-- Modified date:	21 May 2021
-- Description:		1. Fix bug
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023 
-- Modified by:		Martin Tang
-- Modified date:	05 May 2021
-- Description:		1. Performance turning 
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-00XX (Immu record)
-- Modified by:		Martin Tang
-- Modified date:	31 Dec 2020
-- Description:		update COVID19ExporterQueue
-- =============================================
CREATE PROCEDURE [dbo].[proc_COVID19ExporterQueue_update] @File_ID    VARCHAR(27) = NULL, 
                                                          @FromStatus CHAR(1)     = NULL, 
                                                          @ToStatus   CHAR(1)
AS
    BEGIN

        DECLARE @ln_FileID AS VARCHAR(27)= @File_ID;
        DECLARE @ln_FromStatus AS CHAR(1)= @fromStatus;
        DECLARE @ln_ToStatus AS CHAR(1)= @toStatus;
        DECLARE @SkipedStatus AS CHAR(1)= 'S';

        UPDATE COVID19ExporterQueue
          SET Record_Status = @ln_toStatus, 
              Update_Dtm = GETDATE()
        WHERE Record_Status = COALESCE(@ln_fromStatus, Record_Status)
              AND COALESCE(Record_File_ID, '') = COALESCE(@ln_FileID, Record_File_ID, '');
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19ExporterQueue_update] TO HCVU;
GO