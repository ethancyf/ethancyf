IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHASubsidizeWriteOff_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_eHASubsidizeWriteOff_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   23 Nov 2017
-- Description:		Add [PValue_TotalRefund], [PValue_SeasonRefund]
-- =============================================
-- =============================================
-- Author:		Karl LAM
-- Create date: 23 Jul 2013
-- Description:	insert write off record
-- =============================================
CREATE PROCEDURE [dbo].[proc_eHASubsidizeWriteOff_add]  
 @Doc_Code	char(20),  
 @Doc_ID	varchar(20),  
 @DOB		datetime,  
 @Exact_DOB char(1),  
 @Scheme_Code char(10),  
 @Scheme_Seq smallint,  
 @Subsidize_Code char(10),  
 @Writeoff_Unit int,  
 @WriteOff_Per_Unit_Value money,  
 @PValue_Ceiling int,  
 @PValue_Total_Entitlement int,  
 @PValue_Season_Entitlement int,  
 @PValue_Total_Used int,  
 @PValue_Season_Used int,  
 @PValue_Available int,  
 @Create_dtm datetime,   
 @Create_Reason varchar(2),
 @PValue_Total_Refund int,  
 @PValue_Season_Refund int
AS BEGIN  
-- ============================================================  
-- Declaration  
-- ============================================================  
  
 DECLARE @Encrypt_Field1 varbinary(100)  
  
-- ============================================================  
-- Validation  
-- ============================================================  
-- ============================================================  
-- Initialization  
-- ============================================================  
 EXEC [proc_SymmetricKey_open]
  
 SET @Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @Doc_ID)  
  
 EXEC [proc_SymmetricKey_close]
-- ============================================================  
-- Return results  
-- ============================================================  
  
INSERT INTO eHASubsidizeWriteOff (  
  [Doc_Code],  
  [Encrypt_Field1],  
  [DOB],  
  [Exact_DOB],  
  [Scheme_Code],  
  [Scheme_Seq],  
  [Subsidize_Code],  
  [WriteOff_Unit],  
  [WriteOff_Per_Unit_Value],  
  [PValue_Ceiling],  
  [PValue_TotalEntitlement],  
  [PValue_SeasonEntitlement],  
  [PValue_TotalUsed],  
  [PValue_SeasonUsed],  
  [PValue_Available],  
  [Create_Dtm],  
  [Create_Reason],
  [PValue_TotalRefund],
  [PValue_SeasonRefund])  
 VALUES (  
 @Doc_Code,  
 @Encrypt_Field1,  
 @DOB,  
 @Exact_DOB,  
 @Scheme_Code,  
 @Scheme_Seq,  
 @Subsidize_Code,  
 @Writeoff_Unit,  
 @WriteOff_Per_Unit_Value,  
 @PValue_Ceiling,  
 @PValue_Total_Entitlement,  
 @PValue_Season_Entitlement,  
 @PValue_Total_Used,  
 @PValue_Season_Used,  
 @PValue_Available,   
 @Create_dtm,
 @Create_Reason,
 @PValue_Total_Refund,
 @PValue_Season_Refund)  
  
END
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_add] TO HCPUBLIC
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_add] TO HCSP
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_add] TO HCVU
GO
GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOff_add] TO WSEXT
GO



