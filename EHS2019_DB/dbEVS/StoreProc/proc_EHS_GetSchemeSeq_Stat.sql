IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_GetSchemeSeq_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_GetSchemeSeq_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

  
-- =============================================  
-- Modification History  
-- CR No:		  INT13-0017 Fix statisic report problem on get current scheme season
-- Modified by:	  Koala CHENG
-- Modified date: 08 Aug 2013  
-- Description:	  Rectify the logic to retrieve the latest effective season (Even claim period ended)
-- =============================================
-- =============================================  
-- Modification History  
-- CR No:		 CRE12-008-01 Allowing different subsidy level for each scheme at different date period
-- Modified by:  Koala CHENG
-- Modified date: 11 Sep 2012  
-- Description:	  Modify to get the max Scheme Seq
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:  Eric Tse  
-- Modified date: 27 October 2010  
-- Description:	  Use to handle get scheme seq for vaccination statistic
-- =============================================    
CREATE PROCEDURE  [dbo].[proc_EHS_GetSchemeSeq_Stat]    
	@scheme_code char(10),
	@cutoff_dtm datetime
as  
BEGIN  
  
--***********************************************/  
--* Declaration                                 */                          
--***********************************************/  
--***********************************************/  
--*   Validation                                */  
--***********************************************/  
--***********************************************/  
--*   Initialization                            */  
--***********************************************/  
--***********************************************/  
--*   Return results                            */  
--***********************************************/  
Declare @Scheme_Seq int 

SELECT @Scheme_Seq = MAX(scheme_seq) FROM SubsidizeGroupClaim 
where scheme_code = @scheme_code AND Record_Status = 'A' and Claim_Period_From <= @Cutoff_Dtm 
HAVING MAX(scheme_seq) IS NOT NULL 

--SELECT TOP 1 @Scheme_Seq = Scheme_seq FROm (
--SELECT MAX(scheme_seq) AS [scheme_seq], '1' [level] FROM SubsidizeGroupClaim where scheme_code = @scheme_code and Claim_Period_From <= @Cutoff_Dtm and @Cutoff_Dtm < Claim_Period_To AND Record_Status = 'A' HAVING MAX(scheme_seq) IS NOT NULL 
--UNION ALL
--SELECT top 1  scheme_seq, '2' [level] FROM SubsidizeGroupClaim where scheme_code = @scheme_code AND Record_Status = 'A' and @Cutoff_Dtm >= Claim_Period_To and Claim_Period_From < @Cutoff_Dtm ORDER BY Claim_Period_To DESC 
--) SubsidizeGroupClaimForSeq ORDER BY [LEVEL]

RETURN @Scheme_Seq
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_GetSchemeSeq_Stat] TO HCVU
GO
