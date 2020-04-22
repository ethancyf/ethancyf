IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_IVRSConnectionMapping_ins]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_IVRSConnectionMapping_ins]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Cheung
-- Create date:		25-11-2008
-- Description:		Add Unique Key to IVRSConnetionMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_IVRSConnectionMapping_ins]

@SP_ID char(8),
@CallUniqueID varchar(40)

AS

-- =============================================
-- Declaration
-- =============================================

-- =============================================
-- Initialization
-- =============================================
BEGIN
	INSERT INTO [dbo].[IVRSConnectionMapping]
	(
	SP_ID,
	CallUniqueID,
	Last_Action_Dtm
	)
	VALUES
	(
	@SP_ID,
	@CallUniqueID,
	getdate()
	)
END

GO

GRANT EXECUTE ON [dbo].[proc_IVRSConnectionMapping_ins] TO HCSP
GO
