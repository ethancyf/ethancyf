IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeAll_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeAll_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Get All Practice with SPID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip	
-- Modified date:	13 Aug 2009
-- Description:		Add Practice Chinese Name and display seq
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure proc_PracticeAll_get_bySPID
@SP_ID		char(8)
as

-- =============================================
-- Return results
-- =============================================
select p.SP_ID	SP_ID
, p.Display_Seq Display_Seq
, p.Practice_Name	Practice_Name
, p.Practice_Type	Practice_Type
, p.Practice_Name_Chi Practice_Chi_Name
, p.display_seq Practice_Display_Seq
from Practice p
where p.SP_ID = @SP_ID

GO

GRANT EXECUTE ON [dbo].[proc_PracticeAll_get_bySPID] TO HCSP
GO
