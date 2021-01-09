IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherRefund_Get_byDocID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherRefund_Get_byDocID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Koala CHENG
-- Modified date:	22 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Chris YIM
-- Modified date:   20 Feb 2019
-- CR No.:			CRE20-005
-- Description:		Grant rights of WSEXT
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		30 Nov 2017
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Description:		Retrieve refunded record by Doc ID 
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherRefund_Get_byDocID]     
	@identity   varchar(20)
AS    
BEGIN    
    
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
						      
	-- Retrieve Voucher Refund By Doc No.
    
	SELECT DISTINCT
		VR.Refund_ID,
		VR.Voucher_Acc_ID,
		VR.Doc_Code,
		[Doc_No] = CONVERT(VARCHAR, DECRYPTBYKEY(VR.Encrypt_Field1)),
		VR.Refund_Amt,
		VR.Refund_Dtm,
		VR.Scheme_Seq,
		VR.Record_Status,
		VR.Create_Dtm,
		VR.Create_By,
		VR.Update_Dtm,
		VR.Update_By,
		VR.TSMP
	FROM    
		VoucherRefund VR
	WHERE    
		VR.Record_Status = 'R' -- Refunded Only
		AND VR.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @identity) 
	ORDER BY VR.Refund_Dtm    

	EXEC [proc_SymmetricKey_close]  

END  
GO

GRANT EXECUTE ON [dbo].[proc_VoucherRefund_Get_byDocID] TO HCSP, HCVU, HCPUBLIC, WSEXT
GO


