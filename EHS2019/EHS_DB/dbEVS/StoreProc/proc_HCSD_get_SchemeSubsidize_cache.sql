IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_SchemeSubsidize_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_SchemeSubsidize_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	22 Aug 2016
-- CR No.:			CRE16-002
-- Description:		Obsolete sproc
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Karl LAM
-- Modified date:	29 Jul 2014
-- CR No.:			INT14-0014
-- Description:		Fix SDIR scheme still displaying when all it is expired (Period_To_Dtm)
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		Tommy TSE
-- Modified date:	9 Sep 2011
-- CR No.:			CRE11-024-01 (Enhancement on HCVS Extension Part 1)
-- Description:		Profession related data is
--					retrieved from table [profession]
-- =============================================

-- =============================================
-- Author:		Mattie LO
-- Create date: 30 August 2009
-- Description:	 Retrieve the Scheme and Subsidize List
-- =============================================
/*
CREATE PROCEDURE [dbo].[proc_HCSD_get_SchemeSubsidize_cache] 
AS
BEGIN

	SELECT ss.[scheme_code_claim]
		  ,ss.[subsidize_code]
		  ,ss.[scheme_desc]
		  ,ss.[scheme_desc_chi]
		  ,ss.[scheme_display_seq_SD]
		  ,ss.[subsidize_display_seq_SD]
		  ,ss.[subsidize_item_desc_SD]
		  ,ss.[subsidize_item_desc_chi_SD]
		  ,[scheme].scheme_column
		  ,fee.fee_column
	  FROM [dbo].[SDSchemeSubsidize] ss
		, [dbo].[SDFeeColumnMapping] fee
		, [dbo].[SDSchemeColumnMapping] [scheme]
	  WHERE [scheme].scheme_code = ss.scheme_code_claim
		and fee.scheme_code = ss.scheme_code_claim
		and fee.subsidize_code = ss.subsidize_code
		and (ss.[Period_From] IS NULL OR DATEDIFF(Day, ss.[Period_From], GETDATE()) >= 0 )
		and (ss.[Period_To] IS NULL OR DATEDIFF(Day, GETDATE(), ss.[Period_To]) > 0 )

	UNION

	SELECT distinct ss.[scheme_code_claim]
		  ,'' as [subsidize_code]
		  ,ss.[scheme_desc]
		  ,ss.[scheme_desc_chi]
		  ,ss.[scheme_display_seq_SD]
		  ,0 as [subsidize_display_seq_SD]
		  ,'' as [subsidize_item_desc_SD]
		  ,'' as [subsidize_item_desc_chi_SD]
		  ,[scheme].scheme_column as scheme_column
		  ,'' as fee_column
	  FROM [dbo].[SDSchemeSubsidize] ss
		INNER JOIN [dbo].[SDSchemeColumnMapping] [scheme]	ON [scheme].scheme_code = ss.scheme_code_claim
	WHERE 	(ss.[Period_From] IS NULL OR DATEDIFF(Day, ss.[Period_From], GETDATE()) >= 0 )
		and (ss.[Period_To] IS NULL OR DATEDIFF(Day, GETDATE(), ss.[Period_To]) > 0 )

	 ORDER BY [scheme_display_seq_SD], [subsidize_display_seq_SD] 

--	select sc.scheme_code as scheme_code, 
--		sc.display_code as scheme_display_code,
--		sc.scheme_desc as scheme_desc, 
--		sc.scheme_desc_chi as scheme_desc_chi, 
--		sc.display_seq as scheme_display_seq,
--		sgp.subsidize_code as subsidize_code, 
--		sgp.display_seq as subsidize_display_seq,
--		ss.subsidize_item_code as subsidize_item_code,
--		sitem.subsidize_item_desc_SD as subsidize_item_desc_SD,
--		sitem.subsidize_item_desc_Chi_SD as subsidize_item_desc_Chi_SD,
--		sitem.subsidize_type as subsidize_type
--	from schemeclaim sc, subsidizegroupclaim sgp, subsidize ss, subsidizeitem sitem
--	where sgp.scheme_code = sc.scheme_code
--	and ss.subsidize_code = sgp.subsidize_code
--	and sitem.subsidize_item_code = ss.subsidize_item_code
--	and getdate() between sc.claim_period_from and sc.claim_period_to
--	and getdate() between sgp.claim_period_from and sgp.claim_period_to
--	and sc.record_status = 'A'
--	and sgp.record_status = 'A'
--	and ss.record_status = 'A'
--	and sitem.record_status = 'A'
--	and sc.scheme_code not in (select scheme_code from schemeexceptionlistSD)
--
--UNION
--
--	select distinct sc.scheme_code as scheme_code, 
--		sc.display_code as scheme_display_code,
--		sc.scheme_desc as scheme_desc, 
--		sc.scheme_desc_chi as scheme_desc_chi, 
--		sc.display_seq as scheme_display_seq,
--		'' as subsidize_code, 
--		'0' as subsidize_display_seq,
--		'' as subsidize_item_code,
--		'' as subsidize_item_desc_SD,
--		'' as subsidize_item_desc_Chi_SD,
--		sitem.subsidize_type as subsidize_type
--	from schemeclaim sc, subsidizegroupclaim sgp, subsidize ss, subsidizeitem sitem
--	where sgp.scheme_code = sc.scheme_code
--	and ss.subsidize_code = sgp.subsidize_code
--	and sitem.subsidize_item_code = ss.subsidize_item_code
--	and getdate() between sc.claim_period_from and sc.claim_period_to
--	and getdate() between sgp.claim_period_from and sgp.claim_period_to
--	and sc.record_status = 'A'
--	and sgp.record_status = 'A'
--	and ss.record_status = 'A'
--	and sitem.record_status = 'A'
--	and sc.scheme_code not in (select scheme_code from schemeexceptionlistSD)
--	order by sitem.subsidize_type, scheme_display_seq, subsidize_display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_SchemeSubsidize_cache] TO HCPUBLIC
GO
*/
