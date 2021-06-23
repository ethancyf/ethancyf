IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DHCSPMapping_delete]')
AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DHCSPMapping_delete]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			 
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-006 
-- Modified by:		Nichole Ip
-- Modified date:	16 JUL 2020
-- Description:		1. To remove eHS(S) service provider as a DHC NSP in eHS(S), it supports 
--                     to upload all district NSP or particular district NSP
-- =============================================

CREATE Procedure [dbo].[proc_DHCSPMapping_delete] 
	@UploadDistrictCode VARCHAR(3)
AS

	-- ============================================================
    -- Declaration
    -- ============================================================
	-- ============================================================
    -- Validation
    -- ============================================================
    -- ============================================================
    -- Initialization
    -- ============================================================ 
    -- ============================================================
    -- Return results
    -- ============================================================
	IF @UploadDistrictCode = 'ALL'
    BEGIN
        INSERT INTO DHCSPMappingLog 
					([System_Dtm],
					 [Service_Category_Code], 
                     [Registration_Code], 
                     [District_Code],
					 [Create_Dtm])
              SELECT [Create_Dtm],
					 [Service_Category_Code], 
                     [Registration_Code], 
                     [District_Code], 
                     GETDATE()
               FROM DHCSPMapping;

		DELETE FROM DHCSPMapping;
    END;
    ELSE
    BEGIN
        INSERT INTO DHCSPMappingLog
					([System_Dtm],
					 [Service_Category_Code], 
                     [Registration_Code], 
                     [District_Code],
					 [Create_Dtm])
              SELECT [Create_Dtm],
					 [Service_Category_Code], 
                     [Registration_Code], 
                     [District_Code], 
                     GETDATE()
               FROM DHCSPMapping
               WHERE District_Code = @UploadDistrictCode;

        DELETE FROM DHCSPMapping  WHERE [District_Code] = @UploadDistrictCode;
    END;
Go
 
GRANT EXECUTE ON [dbo].[proc_DHCSPMapping_delete] TO WSEXT 
GO

