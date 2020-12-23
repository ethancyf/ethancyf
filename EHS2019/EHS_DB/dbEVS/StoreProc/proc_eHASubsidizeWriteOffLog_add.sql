IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eHASubsidizeWriteOffLog_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_eHASubsidizeWriteOffLog_add]
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
-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:	CRE13-006
-- Create Date:	24 July 2013
-- Description:	Insert record into table - [proc_eHASubsidizeWriteOffLog_add]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_eHASubsidizeWriteOffLog_add]
	@action_key varchar(20),
	@doc_code char(20),
	@doc_id varchar(20),
	@dob datetime,
	@exact_dob char(1),
	@scheme_code char(10),
	@scheme_seq smallint,
	@subsidize_code char(10),
	@writeoff_unit int,
	@writeoff_per_unit_value money,
	@pvalue_ceiling int,
	@pvalue_total_entitlement int,
	@pvalue_season_entitlement int,
	@pvalue_total_used int,
	@pvalue_season_used int,
	@pvalue_available int,
	@create_dtm datetime,
	@create_reason varchar(2),
	@PValue_Total_Refund int,  
	@PValue_Season_Refund int
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================

	DECLARE @encrypt_field1 varbinary(100)

-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================

	EXEC [proc_SymmetricKey_open]

	SET @encrypt_field1 = EncryptByKey(KEY_GUID('sym_Key'), @doc_id)

	EXEC [proc_SymmetricKey_close]

-- ============================================================
-- Return results
-- ============================================================

    INSERT INTO eHASubsidizeWriteOffLog (
		[System_Dtm],
		[Action_Key],
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
		GETDATE(),
		@action_key,
		@doc_code,
		@encrypt_field1,
		@dob,
		@exact_dob,
		@scheme_code,
		@scheme_seq,
		@subsidize_code,
		@writeoff_unit,
		@writeoff_per_unit_value,
		@pvalue_ceiling,
		@pvalue_total_entitlement,
		@pvalue_season_entitlement,
		@pvalue_total_used,
		@pvalue_season_used,
		@pvalue_available,
		@create_dtm,
		@create_reason,
		@PValue_Total_Refund,
		@PValue_Season_Refund) 

END
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOffLog_add] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOffLog_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOffLog_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_eHASubsidizeWriteOffLog_add] TO WSEXT
Go
