IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_SerivceProviderStaging_ins')
	DROP TRIGGER [dbo].[tri_SerivceProviderStaging_ins] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 29 May 2008
-- Description:	Trigger a inset statment into ServiceProviderVerification
--				when a row insert into ServiceProviderStaging 
-- =============================================
CREATE TRIGGER [dbo].[tri_SerivceProviderStaging_ins]
   ON  dbo.ServiceProviderStaging
   AFTER INSERT
AS 
BEGIN

	SET NOCOUNT ON;

    INSERT INTO ServiceProviderVerification
	(Enrolment_Ref_No,
	 SP_ID,
	 Record_Status,
	 Update_By,
	 Update_Dtm)
	SELECT	SPS.Enrolment_Ref_No,
			SPS.SP_ID,
			'U',
			SPS.Create_By,
			SPS.Create_Dtm
	FROM	inserted SPS

END
GO
