IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_IVRSConnectionMapping_check]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_IVRSConnectionMapping_check]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Cheung
-- Create date:		25-11-2008
-- Description:		Check if Unique Key is valid in IVRSConnetionMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

CREATE procedure [dbo].[proc_IVRSConnectionMapping_check]

@SP_ID char(8),
@CallUniqueID varchar(40)

AS

-- =============================================
-- Declaration
-- =============================================
Declare @time_gap smallint

--Set time gap to be 60 minutes
Select @time_gap = Parm_Value1 from SystemParameters where Parameter_Name ='IVRSValidTimeLimit' AND [Scheme_Code] = 'ALL'

-- =============================================
-- Initialization
-- =============================================
BEGIN
	Select count(1) as row_count
	From [dbo].[IVRSConnectionMapping]
	Where
	SP_ID=@SP_ID and
	CallUniqueID = @CallUniqueID and
	DATEDIFF(mi, Last_Action_Dtm, getdate()) <= @time_gap

END

GO

GRANT EXECUTE ON [dbo].[proc_IVRSConnectionMapping_check] TO HCSP
GO
