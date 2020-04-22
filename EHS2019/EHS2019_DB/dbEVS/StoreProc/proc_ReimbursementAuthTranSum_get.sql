IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTranSum_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthTranSum_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- Modified by:		Koala CHENG
-- Modified date:	15 January 2018
-- CR No.:			I-CRE17-005
-- Description:		Performance Tuning
-- 					1. Add WITH (NOLOCK)
-- ============================================= 
-- =============================================
-- Modification History
-- CR No.:			INT16-0016 (Fix Monthly Statement cannot view EVSS)
-- Modified by:		Lawrence TSANG
-- Modified date:	31 October 2016
-- Description:		Fix Monthly Statement cannot view EVSS
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-002 (Revamp VSS)
-- Modified by:		Lawrence TSANG
-- Modified date:	1 September 2016
-- Description:		Handle category
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004 (TIV and QIV)
-- Modified by:		Chris YIM
-- Modified date:	19 June 2015
-- Description:		Re-arrange the display order in Monthly Statement 
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	03 March 2015
-- Description:		Add Input Parameter "Available_HCSP_SubPlatform"
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:		Tommy LAM
-- Modified date:	27 Jan 2014
-- Description:		Add Column - [SubsidizeGroupClaim].[Mth_Statement_Unit_Visible]
-- =============================================


CREATE PROCEDURE [dbo].[proc_ReimbursementAuthTranSum_get]
	@sp_id					char(8),
	@practice_display_seq	smallint,
	@reimburse_id			varchar(20),
	@Available_HCSP_SubPlatform	char(2)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================

	DECLARE @In_SPID			char(8)
	DECLARE @In_Practice_Seq	smallint
	DECLARE @In_Reimburse_ID	varchar(20)
	DECLARE @In_Available_HCSP_SubPlatform	char(2)
	SET @In_SPID = @sp_id
	SET @In_Practice_Seq = @practice_display_seq
	SET @In_Reimburse_ID = @reimburse_id
	SET @In_Available_HCSP_SubPlatform = @Available_HCSP_SubPlatform

	DECLARE @Subsidy table (
		Scheme_Code					char(10),
		Category_Code				varchar(10),
		Subsidize_Code				char(10),
		Per_Unit_Value				money,
		Transaction_Count			int,
		Transaction_Amount			money,
		Scheme_Display_Seq			smallint,
		Scheme_Seq					smallint,
		Subsidize_Display_Seq		smallint,
		Mth_Statement_Desc			varchar(150),
		Mth_Statement_Desc_Chi		nvarchar(150),
		Mth_Statement_Desc_CN		nvarchar(150),
		Mth_Statement_Unit_Visible	char(1)
	)
	
	DECLARE @Scheme table (
		Scheme_Code					char(10),
		Scheme_Display_Seq			smallint,
		Mth_Statement_Unit_Visible	char(1),
		Transaction_Count			int,
		Transaction_Amount			money
	)
	
	DECLARE @Category table (
		Scheme_Code					char(10),
		Category_Code				varchar(10),
		Category_Display_Seq		int,
		Transaction_Amount			money
	)


-- =============================================
-- Retrieve data
-- =============================================
		
	INSERT INTO @Subsidy (
		Scheme_Code,
		Category_Code,
		Subsidize_Code,
		Per_Unit_Value,
		Transaction_Count,
		Transaction_Amount,
		Scheme_Display_Seq,
		Scheme_Seq,
		Subsidize_Display_Seq,
		Mth_Statement_Desc,
		Mth_Statement_Desc_Chi,
		Mth_Statement_Desc_CN,
		Mth_Statement_Unit_Visible
	)
	SELECT
		RAT.Scheme_Code,
		VT.Category_Code,
		TD.Subsidize_Code,
		TD.Per_Unit_Value,
		COUNT(1) AS [Transaction_Count],
		SUM(TD.Total_Amount) AS [Transaction_Amount],
		SC.Display_Seq AS [Scheme_Display_Seq],
		SGC.Scheme_Seq,
		SGC.Display_Seq AS [Subsidize_Display_Seq],
		SGC.Mth_Statement_Desc,
		SGC.Mth_Statement_Desc_Chi,
		SGC.Mth_Statement_Desc_CN,
		SGC.Mth_Statement_Unit_Visible
	FROM
		ReimbursementAuthTran RAT WITH (NOLOCK)
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
				ON RAT.Transaction_ID = VT.Transaction_ID
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON RAT.Transaction_ID = TD.Transaction_ID
			INNER JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
				ON TD.Scheme_Code = SGC.Scheme_Code
					AND	TD.Scheme_Seq = SGC.Scheme_Seq
					AND	TD.Subsidize_Code = SGC.Subsidize_Code
			INNER JOIN SchemeClaim SC WITH (NOLOCK)
				ON RAT.Scheme_Code = SC.Scheme_Code
	WHERE
		VT.SP_ID = @In_SPID
			AND VT.Practice_Display_Seq = @In_Practice_Seq
			AND RAT.Reimburse_ID = @In_Reimburse_ID
			AND (@In_Available_HCSP_SubPlatform IS NULL OR SC.Available_HCSP_SubPlatform = @In_Available_HCSP_SubPlatform)
	GROUP BY
		RAT.Scheme_Code,
		VT.Category_Code,
		TD.Subsidize_Code,
		TD.Per_Unit_Value,
		SC.Display_Seq,
		SGC.Scheme_Seq,
		SGC.Display_Seq,
		SGC.Mth_Statement_Desc,
		SGC.Mth_Statement_Desc_Chi,
		SGC.Mth_Statement_Desc_CN,
		SGC.Mth_Statement_Unit_Visible
	
	--
	
	INSERT INTO @Scheme (
		Scheme_Code,
		Scheme_Display_Seq,
		Mth_Statement_Unit_Visible,
		Transaction_Count,
		Transaction_Amount
	)
	SELECT
		S.Scheme_Code,
		S.Scheme_Display_Seq,
		MAX(S.Mth_Statement_Unit_Visible),	-- A trick to handle 'Y' and 'N', pick 'Y' whenever there is at least one row with 'Y'
		SUM(S.Transaction_Count),
		SUM(S.Transaction_Amount)
	FROM
		@Subsidy S
	GROUP BY
		S.Scheme_Code,
		S.Scheme_Display_Seq
	
	--
	
	INSERT INTO @Category (
		Scheme_Code,
		Category_Code,
		Category_Display_Seq,
		Transaction_Amount
	)
	SELECT DISTINCT
		S.Scheme_Code,
		S.Category_Code,
		CC.Display_Seq,
		SUM(S.Transaction_Amount)
	FROM
		@Subsidy S
			INNER JOIN ClaimCategory CC WITH (NOLOCK)
				ON S.Category_Code = CC.Category_Code
	GROUP BY
		S.Scheme_Code,
		S.Category_Code,
		CC.Display_Seq


-- =============================================
-- Return results
-- =============================================
	
	-- Table 1: Statement Header
	
	SELECT
		--RA.CutOff_Date AS [CutOff_Date],
		MIN(BI.Completion_Dtm) AS [Statement_Issue_Date],
		P.Practice_Name,
		P.Practice_Name_Chi,
		B.Bank_Account_No,
		B.Bank_Acc_Holder,
		(SELECT SUM(Transaction_Amount) FROM @Scheme) AS [Transaction_Amount]
	FROM
		Practice P WITH (NOLOCK)
			INNER JOIN BankAccount B WITH (NOLOCK)
				ON P.SP_ID = B.SP_ID
					AND B.SP_Practice_Display_Seq = @In_Practice_Seq
			INNER JOIN ReimbursementAuthorisation RA WITH (NOLOCK)
				 ON RA.Reimburse_ID = @In_Reimburse_ID
					AND RA.Authorised_Status = 'S'
					AND RA.Record_Status = 'A'
			INNER JOIN BankIn BI WITH (NOLOCK)
				ON BI.Reimburse_ID = @In_Reimburse_ID
	WHERE
		P.SP_ID = @In_SPID
			AND P.Display_Seq = @In_Practice_Seq
	GROUP BY
		P.Practice_Name,
		P.Practice_Name_Chi,
		B.Bank_Account_No,
		B.Bank_Acc_Holder,
		RA.CutOff_Date


	-- Table 2: Scheme
	
	SELECT
		S.Scheme_Code,
		S.Scheme_Display_Seq,
		S.Mth_Statement_Unit_Visible,
		PSI.Clinic_Type,
		S.Transaction_Count,
		S.Transaction_Amount
	FROM
		@Scheme S
			INNER JOIN (
				SELECT
					M.Scheme_Code_Claim AS [Scheme_Code],
					MAX(Clinic_Type) AS [Clinic_Type]
				FROM
					PracticeSchemeInfo PSI WITH (NOLOCK)
						INNER JOIN SchemeEnrolClaimMap M WITH (NOLOCK)
							ON PSI.Scheme_Code = M.Scheme_Code_Enrol
				WHERE
					SP_ID = @sp_id
						AND Practice_Display_Seq = @practice_display_seq
				GROUP BY
					M.Scheme_Code_Claim
			) PSI
				ON S.Scheme_Code = PSI.Scheme_Code
	
	
	-- Table 3: Category
	
	SELECT
		Scheme_Code,
		Category_Code,
		Category_Display_Seq,
		Transaction_Amount
	FROM
		@Category


	-- Table 4: Subsidy

	SELECT 
		Scheme_Code,
		Category_Code,
		Subsidize_Code,
		Per_Unit_Value,
		Transaction_Count,
		Transaction_Amount,
		Scheme_Seq,
		Subsidize_Display_Seq,
		Mth_Statement_Desc,
		Mth_Statement_Desc_Chi,
		Mth_Statement_Desc_CN
	FROM
		@Subsidy

		
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTranSum_get] TO HCSP
GO
