IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ThirdPartyAdditionalFieldEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ThirdPartyAdditionalFieldEnrolment_add]
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
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 12 Jan 2012
-- Description:	Insert the Type of Practice to Table
--				ThridPartyEnrolment
-- =============================================
CREATE PROCEDURE [dbo].[proc_ThirdPartyAdditionalFieldEnrolment_add] 
	@sys_code varchar(50),
	@enrolment_ref_no char(15), 
	@practice_display_seq smallint,
	@AdditionalFieldID varchar(50),
	@AdditionalFieldValueCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

   INSERT INTO [ThirdPartyAdditionalFieldEnrolment]
				(Sys_Code,
				Enrolment_Ref_No,				
				Practice_Display_Seq,
				AdditionalFieldID,
				AdditionalFieldValueCode,
				Create_dtm)
	VALUES		(@sys_code,				
				@enrolment_ref_no,
				@practice_display_seq,
				@AdditionalFieldID,
				@AdditionalFieldValueCode,
				GETDATE())
END
GO

GRANT EXECUTE ON [dbo].[proc_ThirdPartyAdditionalFieldEnrolment_add] TO HCPUBLIC
GO
