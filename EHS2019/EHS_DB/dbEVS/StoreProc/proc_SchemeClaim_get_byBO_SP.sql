 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeClaim_get_byBO_SP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeClaim_get_byBO_SP]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	10 Jan 2022
-- CR No.			CRE20-023-71 (COVID19 Exemption Record) 
-- Description:		Add Column    - [SchemeClaim].[AllowDataEntryClaim]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 April 2021
-- CR No.:			CRE20-023
-- Description:		Add Column    - [SchemeClaim].[AllowTempAccBOClaim]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	10 August 2018
-- CR No.:			CRE17-018
-- Description:		Add Column    - [SchemeClaim].[Readonly_HCSP]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
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
-- CR No:			CRE13-001 EHAPP
-- Modified by:		Tommy LAM, Koala CHENG
-- Modified date:	03 Apr 2013
-- Description:		Add Column - Reimbursement_Available, Confirmed_Transaction_Status
--					Add Column - PCSClaim_Available
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	27 Dec 2012
-- Description:		Add column - Control_Type, Control_Setting
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-01 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Koala CHENG
-- Modified date:	20 Jun 2012
-- Description:		Modify to return latest scheme seq information
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 7 July 2010
-- Description:	Retrieve the scheme claim by back office user's scheme
--				and sp's scheme
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeClaim_get_byBO_SP]
	-- Add the parameters for the stored procedure here
	@userid as varchar(20),
	@functin_code as char(6),
	@sp_id char(8),
	@practice_id as smallint,
	@service_dtm as datetime
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Initialization
-- =============================================
SELECT a.*,
		b.[Scheme_Desc],  
		b.[Scheme_Desc_Chi],   
		b.[Scheme_Desc_CN],   
		b.[Display_Code],  
		b.[Display_Seq],  
		b.[BalanceEnquiry_Available],  
		b.[IVRS_Available],  
		b.[TextOnly_Available],  
		b.[Claim_Period_From],  
		b.[Claim_Period_To],  
		b.[Create_By],  
		b.[Create_Dtm],  
		b.[Update_By],  
		b.[Update_Dtm],  
		b.[Record_Status],  
		b.[Effective_dtm],  
		b.[Expiry_dtm],  
		b.[PreFill_Search_Available],  
		b.[TSWCheckingEnable],
		b.[Control_Type],
		b.[Control_Setting],
		b.[PCSClaim_Available],
		b.[Confirmed_Transaction_Status],
		b.[Reimbursement_Mode],
		b.[Reimbursement_Currency],
		b.[Available_HCSP_SubPlatform],
		b.[ProperPractice_Avail],
		b.[ProperPractice_SectionID],
		b.[Readonly_HCSP],
		b.[AllowTempAccBOClaim],
		b.[AllowDataEntryClaim]
FROM
(select   
	 [Scheme_Code],  
	 MAX([Scheme_Seq]) AS [Scheme_Seq]  
	from schemeclaim   
	where scheme_code in  
	(  
		select distinct m.scheme_code_claim  
		from practiceschemeinfo p, SchemeEnrolClaimMap m  
		where p.scheme_code = m.scheme_code_enrol  
		and p.sp_id = @sp_id  
		and p.practice_display_seq = @practice_id  
		and m.scheme_code_claim in  
		(  
			select r.scheme_code from userrole r, RoleSecurity rs  
			where r.role_type = rs.role_type  
			and r.user_id = @userid  
			and rs.function_code =  @functin_code  
		)  
	)  
	and @service_dtm >= convert(datetime, convert(varchar, claim_period_from, 106))   
	and @service_dtm < convert(datetime, convert(varchar, claim_period_to, 106))  
	GROUP BY [Scheme_Code]
) AS a
LEFT JOIN SchemeClaim b
ON a.Scheme_Code=b.Scheme_Code AND a.Scheme_Seq=b.Scheme_Seq
order by display_seq  
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaim_get_byBO_SP] TO HCVU
GO

