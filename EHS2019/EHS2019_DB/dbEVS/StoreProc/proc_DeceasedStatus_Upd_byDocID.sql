IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeceasedStatus_Upd_byDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeceasedStatus_Upd_byDocID]
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
-- Author:			Chris YIM
-- Create date:		30 Nov 2017
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Description:		Update Deceased Status by Doc ID 
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeceasedStatus_Upd_byDocID]     
	@IdentityNo	VARCHAR(20),
	@Deceased	CHAR(1),
	@DOD		DATETIME,
	@Exact_DOD	CHAR(1),
	@UpdateBy	VARCHAR(20)
AS    
BEGIN    
    
-- =============================================    
-- Declaration    
-- ============================================= 
	DECLARE @IN_Deceased	CHAR(1)
	DECLARE @IN_DOD			DATETIME
	DECLARE @IN_Exact_DOD	CHAR(1)
  
-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================      

	IF @Deceased = 'N'
		BEGIN
			SET @IN_Deceased = NULL
			SET @IN_DOD = NULL
			SET @IN_Exact_DOD = NULL
		END
	ELSE
		BEGIN
			SET @IN_Deceased = @Deceased
			SET @IN_DOD = @DOD
			SET @IN_Exact_DOD = @Exact_DOD
		END
  
-- =============================================    
-- Return results    
-- =============================================    

	OPEN SYMMETRIC KEY sym_Key     
		DECRYPTION BY ASYMMETRIC KEY asym_Key    

		-- ------------------------
		-- Validated Account 
		-- ------------------------

		UPDATE 
			PersonalInformation
		SET
			Deceased = @IN_Deceased,
			DOD = @IN_DOD,
			Exact_DOD = @IN_Exact_DOD,
			Update_By = @UpdateBy,
			Update_Dtm = GETDATE()
		WHERE 
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNo) 			


		UPDATE 
			VoucherAccount
		SET
			Deceased = @IN_Deceased,
			Update_By = @UpdateBy,
			Update_Dtm = GETDATE()
		FROM 
			VoucherAccount VA
			INNER JOIN PersonalInformation PInfo ON VA.Voucher_Acc_ID = PInfo.Voucher_Acc_ID
		WHERE 
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNo) 

		-- ------------------------
		-- Temporary Account 
		-- ------------------------

		UPDATE 
			TempPersonalInformation
		SET
			Deceased = @IN_Deceased,
			DOD = @IN_DOD,
			Exact_DOD = @IN_Exact_DOD,
			Update_By = @UpdateBy,
			Update_Dtm = GETDATE()
		WHERE 
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNo) 			


		UPDATE 
			TempVoucherAccount
		SET
			Deceased = @IN_Deceased,
			Update_By = @UpdateBy,
			Update_Dtm = GETDATE()
		FROM 
			TempVoucherAccount TVA
			INNER JOIN TempPersonalInformation TPInfo ON TVA.Voucher_Acc_ID = TPInfo.Voucher_Acc_ID
		WHERE 
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNo) 

		-- ------------------------
		-- Special Account 
		-- ------------------------

		UPDATE 
			SpecialPersonalInformation
		SET
			Deceased = @IN_Deceased,
			DOD = @IN_DOD,
			Exact_DOD = @IN_Exact_DOD,
			Update_By = @UpdateBy,
			Update_Dtm = GETDATE()
		WHERE 
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNo) 			


		UPDATE 
			SpecialAccount
		SET
			Deceased = @IN_Deceased,
			Update_By = @UpdateBy,
			Update_Dtm = GETDATE()
		FROM 
			SpecialAccount SA
			INNER JOIN SpecialPersonalInformation SPInfo ON SA.Special_Acc_ID = SPInfo.Special_Acc_ID
		WHERE 
			Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @IdentityNo) 
					       
	CLOSE SYMMETRIC KEY sym_Key    

END  
GO

GRANT EXECUTE ON [dbo].[proc_DeceasedStatus_Upd_byDocID] TO HCVU
GO


