
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_PracticePhysicalMapping_get_bySPID_DisplaySeq]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_PracticePhysicalMapping_get_bySPID_DisplaySeq];
    END;
GO    
-- =============================================     
-- CR No.:  CRE20-022   
-- Author:  Nichole Ip 
-- Create date:  22 Mar 2021   
-- Description: Get the practive physical mapping list
-- =============================================  
--exec proc_PracticePhysicalMapping_get_bySPID_DisplaySeq '90017250','1'
CREATE PROCEDURE [dbo].[proc_PracticePhysicalMapping_get_bySPID_DisplaySeq] 
	@sp_id                CHAR(8), 
	@practice_display_seq SMALLINT
AS
    BEGIN
        SET NOCOUNT ON;    
        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @l_sp_id				CHAR(8);
        DECLARE @l_practice_display_seq SMALLINT;
        SET @l_sp_id = @sp_id;
        SET @l_practice_display_seq = @practice_display_seq;

        -- =============================================  
        -- Initialization  
        -- =============================================  
        -- =============================================
        -- Return results
        -- =============================================   
        SELECT *
        FROM ViewEqvPracticePhysicalMapping AS V1 WITH(NOLOCK)
        WHERE(@l_sp_id IS NULL
              OR @l_sp_id = V1.SP_ID)
             AND (@l_practice_display_seq IS NULL
                  OR @l_practice_display_seq = V1.Practice_Display_Seq)
        ORDER BY V1.SP_ID, 
                 V1.Practice_Display_Seq;
    END;  
GO

GRANT EXECUTE ON [dbo].[proc_PracticePhysicalMapping_get_bySPID_DisplaySeq] TO HCSP;
GO

GRANT EXECUTE ON [dbo].[proc_PracticePhysicalMapping_get_bySPID_DisplaySeq] TO HCVU;
GO