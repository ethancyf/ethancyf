IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOTransition_get_Active]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOTransition_get_Active]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 31 Aug 2009
-- Description:	Get the active records in
--				Table MOTransition
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_MOTransition_get_Active]	
	@hk_id	char(9)
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

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SELECT  SP_ID,
			Display_Seq, 			
			MO_Eng_Name, 
			MO_Chi_Name, 
			Room,
			[Floor],
			Block, 
			Building, 
			Building_Chi, 
			District, 
			Address_Code, 
			BR_Code, 
			Phone_Daytime, 
			Email, 
			Fax,
			Relationship,
			Relationship_Remark,			
			Record_Status
	FROM    MOTransition
	WHERE	Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)
			AND record_status<>'D'

CLOSE SYMMETRIC KEY sym_Key		

END

GO

GRANT EXECUTE ON [dbo].[proc_MOTransition_get_Active] TO HCVU
GO
