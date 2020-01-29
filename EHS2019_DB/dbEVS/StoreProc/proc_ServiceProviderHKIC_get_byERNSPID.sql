IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderHKIC_get_byERNSPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderHKIC_get_byERNSPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Kathy LEE
-- Create date: 16 June 2009
-- Description:	Retrieve Service Provider HKIC 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:
-- Modified date:
-- Description:
-- =============================================


CREATE PROCEDURE [dbo].[proc_ServiceProviderHKIC_get_byERNSPID]
	@enrolment_ref_no char(15), @sp_id char (8)
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================]
declare @hkid as char(9)
declare @ern as char(15)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT	@hkid = convert(varchar, DecryptByKey(Encrypt_Field1)), @ern = enrolment_ref_no
	FROM	ServiceProviderEnrolment
	WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = Enrolment_Ref_No)
	and		@sp_id is null
	
	if @hkid is null
	begin
	
		SELECT	@hkid = convert(varchar, DecryptByKey(Encrypt_Field1)), @ern = enrolment_ref_no
		FROM	ServiceProviderStaging
		WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = Enrolment_Ref_No)
		and		(@sp_id is null or @sp_id = sp_id)
		
		if @hkid is null
		begin		
			SELECT	@hkid = convert(varchar, DecryptByKey(Encrypt_Field1)), @ern = enrolment_ref_no
			FROM	ServiceProvider
			WHERE	(@enrolment_ref_no is null or @enrolment_ref_no = Enrolment_Ref_No)
			and		(@sp_id is null or @sp_id = sp_id)		
		end	
	
	end	
	
	--Final Result
	select @ern as enrolment_ref_no, @hkid as HKID

CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderHKIC_get_byERNSPID] TO HCVU
GO
