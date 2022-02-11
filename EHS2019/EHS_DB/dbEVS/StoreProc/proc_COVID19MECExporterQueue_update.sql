
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19MECExporterQueue_update]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19MECExporterQueue_update];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023-73 (Upload COVID19MEC to Central DB)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jan 2022
-- Description:		Update COVID19MECExporterQueue Status 
--					(Copied from "proc_COVID19ExporterQueue_update")
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19MECExporterQueue_update]	@File_ID	VARCHAR(27) = NULL, 
																@FromStatus CHAR(1)     = NULL, 
																@ToStatus   CHAR(1)
AS
    BEGIN

        DECLARE @ln_FileID AS VARCHAR(27)= @File_ID;
        DECLARE @ln_FromStatus AS CHAR(1)= @fromStatus;
        DECLARE @ln_ToStatus AS CHAR(1)= @toStatus;
        DECLARE @SkipedStatus AS CHAR(1)= 'S';

        UPDATE COVID19MECExporterQueue
          SET Record_Status = @ln_toStatus, 
              Update_Dtm = GETDATE()
        WHERE Record_Status = COALESCE(@ln_fromStatus, Record_Status)
              AND COALESCE(Record_File_ID, '') = COALESCE(@ln_FileID, Record_File_ID, '');
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19MECExporterQueue_update] TO HCVU;
GO