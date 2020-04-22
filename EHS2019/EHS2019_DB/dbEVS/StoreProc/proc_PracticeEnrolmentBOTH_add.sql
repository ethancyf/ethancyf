IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeEnrolmentBOTH_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeEnrolmentBOTH_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Service Provider to Table
--				ServiceProviderEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeEnrolmentBOTH_add]
	@enrolment_ref_no char(15), @display_seq smallint, @practice_name nvarchar(100),
	@practice_type char(1), @room varchar(5), @floor varchar(3), @block varchar(3),
	@building varchar(100), @building_chi nchar(100), @district char(4), 
	@address_code int, @professional_seq smallint, @scheme char(5)
	
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
	if @scheme = 'IVSS'
	BEGIN
		exec dbIVSS..proc_PracticeEnrolment_add @enrolment_ref_no, @display_seq, @practice_name,
												@practice_type, @room, @floor, @block,
												@building, @building_chi, @district, 
												@address_code, @professional_seq
	END
	ELSE IF @scheme = 'EHCVS'
	BEGIN
		exec dbEVS..proc_PracticeEnrolment_add @enrolment_ref_no, @display_seq, @practice_name,
												@practice_type, @room, @floor, @block,
												@building, @building_chi, @district, 
												@address_code, @professional_seq
	END
	ELSE IF @scheme = 'BOTH'
	BEGIN
		exec dbIVSS..proc_PracticeEnrolment_add @enrolment_ref_no, @display_seq, @practice_name,
												@practice_type, @room, @floor, @block,
												@building, @building_chi, @district, 
												@address_code, @professional_seq

		exec dbEVS..proc_PracticeEnrolment_add @enrolment_ref_no, @display_seq, @practice_name,
												@practice_type, @room, @floor, @block,
												@building, @building_chi, @district, 
												@address_code, @professional_seq

	END

END

GO

GRANT EXECUTE ON [dbo].[proc_PracticeEnrolmentBOTH_add] TO HCPUBLIC
GO
