IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeTransition_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeTransition_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 16 June 2009
-- Description:	Get the record in
--				Table PracticeTransition
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeTransition_get]	
	@hk_id	char(9),
	@display_seq smallint
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

EXEC [proc_SymmetricKey_open]

	SELECT 		 
		Display_Seq, 
		SP_ID, 
		Practice_Name, 
		Practice_Name_Chi,
		Room, 
		[Floor], 
		Block, 
		Building, 
		Building_Chi, 
		District, 
		Address_Code,		
		Phone_DayTime,
		MO_Display_Seq		
	FROM    PracticeTransition
	WHERE	Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HK_ID)  and Display_seq = @display_seq

EXEC [proc_SymmetricKey_close]
		
END

GO

GRANT EXECUTE ON [dbo].[proc_PracticeTransition_get] TO HCVU
GO
