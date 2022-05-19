
IF EXISTS
(
    SELECT *
    FROM sysobjects
    WHERE type = 'TR'
          AND name = 'tri_IAMSmartSPMappingLOG_after_del'
)
    DROP TRIGGER [dbo].[tri_IAMSmartSPMappingLOG_after_del];
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		Nichole IP
-- CR No.		CRE20-011 (iAM Smart)
-- Create date: 23 Jul 2021
-- Description:	Add delete trigger for IAMSmartSPMappingLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE TRIGGER [dbo].[tri_IAMSmartSPMappingLOG_after_del] ON [dbo].[IAMSmartSPMapping]
 AFTER	DELETE
AS
     BEGIN
         SET NOCOUNT ON;
         INSERT INTO [dbo].[IAMSmartSPMappingLOG]
         ([System_Dtm], 
          [Action], 
          [SP_ID], 
          [OpenID], 
          [Create_Dtm]
         )
         (
             SELECT GETDATE(), 
                    'D', 
                    [SP_ID], 
                    [OpenID], 
                    [Create_Dtm]
             FROM Deleted
         );
     END; 
GO