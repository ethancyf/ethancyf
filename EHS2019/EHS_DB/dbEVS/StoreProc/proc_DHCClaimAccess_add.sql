IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DHCClaimAccess_add]')
AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DHCClaimAccess_add]
GO
 
SET ANSI_NULLS ON
GO
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
-- Modification History
-- CR No.:			CRE20-006 
-- Modified by:		Nichole Ip
-- Modified date:	16 JUL 2020
-- Description:		1. Get the claim information from DH IT system and insert into the DHCClaimAccess table for preparing the login
--					 
-- =============================================

CREATE PROCEDURE proc_DHCClaimAccess_add
	 @Artifact VARCHAR(100),
	 @SPID CHAR(8),
	 @ProfCode CHAR(5),
	 @RegNo VARCHAR(15),
	 @HKID VARCHAR(50),
	 @DocType CHAR(20),
	 @HKICSymbol CHAR(1),
	 @DOBFormat VARCHAR(20),
	 @DOB VARCHAR(50),
	 @ClaimAmount  FLOAT,
	 @DistrictCode  VARCHAR(50) = NULL,
	 @EHSLoginURL VARCHAR(500),
	 @ActivationCode VARCHAR(100)
AS
BEGIN
	-- ============================================================
    -- Declaration
    -- ============================================================
	DECLARE @E_HKID VARBINARY(100);
	-- ============================================================
    -- Validation
    -- ============================================================
    -- ============================================================
    -- Initialization
    -- ============================================================ 
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	SELECT @E_HKID = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @HKID);
	CLOSE SYMMETRIC KEY sym_Key
    -- ============================================================
    -- Return results
    -- ============================================================
  	INSERT INTO DHCClaimAccess
	(	
		artifact, 
		SP_ID, 
		Service_Category_Code, 
		Registration_Code, 
		HKID, 
		Doc_Code, 
		HKIC_Symbol, 
		DOBFormat, 
		DOB, 
		DHCDistrictCode,
		Claim_Amount, 
		EHSLoginURL, 
		Activation_Code, 
		Encrypt_Field1, 
		Create_Dtm
	)
	VALUES
	(
		@Artifact, 
		@SPID, 
		@ProfCode, 
		@RegNo, 
		NULL, 
		@DocType, 
		@HKICSymbol, 
		@DOBFormat, 
		@DOB, 
		@DistrictCode ,
		@ClaimAmount, 
		@EHSLoginURL, 
		@ActivationCode, 
		@E_HKID, 
		GETDATE()
	);
END
GO


GRANT EXECUTE ON [dbo].[proc_DHCClaimAccess_add] TO WSEXT
GO
