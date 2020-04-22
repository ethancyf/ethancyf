IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationPermanent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationPermanent_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 21 July 2008
-- Description:	Insert the Scheme Information to Table
--				SchemeInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 8 May 2009
-- Description:	1. Remove "Service Fee From", "Service Fee To"
--				2. Add "Delist Status", "Delist Dtm", "Record Status"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationPermanent_add]
	@scheme_code	char(10), 
	@sp_id			char(8), 
	@record_status	char(1), 
	@create_by		varchar(20), 
	@update_by		varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO SchemeInformation	(	
									Scheme_Code,
									SP_ID,
									Record_Status,
									Effective_Dtm,						
									Create_Dtm,
									Create_By,
									Update_Dtm,
									Update_By
									)
									
	VALUES							(	
									@scheme_code,
									@sp_id,
									@record_status,				
									GETDATE(),				
									GETDATE(),
									@create_by,
									GETDATE(),
									@update_by
									)
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationPermanent_add] TO HCVU
GO
