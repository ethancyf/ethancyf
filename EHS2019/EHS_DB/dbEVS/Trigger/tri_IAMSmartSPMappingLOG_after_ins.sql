
IF EXISTS
(
    SELECT *
    FROM sysobjects
    WHERE type = 'TR'
          AND name = 'tri_IAMSmartSPMappingLOG_after_ins'
)
    DROP TRIGGER [dbo].[tri_IAMSmartSPMappingLOG_after_ins];
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Author:		Nichole IP
-- CR No.		CRE20-011 (iAM Smart)
-- Create date: 23 Jul 2021
-- Description:	Add insert trigger for IAMSmartSPMappingLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE TRIGGER [dbo].[tri_IAMSmartSPMappingLOG_after_ins] ON [dbo].[IAMSmartSPMapping]
AFTER INSERT
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
                    'I', 
                    [SP_ID], 
                    [OpenID], 
                    [Create_Dtm]
             FROM inserted
         );
     END; 
GO