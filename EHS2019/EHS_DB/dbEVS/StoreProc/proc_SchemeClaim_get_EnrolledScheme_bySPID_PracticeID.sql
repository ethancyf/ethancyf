 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeClaim_get_EnrolledScheme_bySPID_PracticeID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeClaim_get_EnrolledScheme_bySPID_PracticeID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 April 2021
-- CR No.:			CRE20-023
-- Description:		Add Column    - [SchemeClaim].[AllowTempAccBOClaim]
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		08 October 2018
-- CR No.:			CRE17-018-07
-- Description:		Retrieve the enrolled scheme for back-office claim
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeClaim_get_EnrolledScheme_bySPID_PracticeID]
	@userid			VARCHAR(20),
	@functin_code	CHAR(6),
	@sp_id			CHAR(8),
	@practice_id	SMALLINT
AS
BEGIN

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Initialization
-- =============================================
SELECT
	SC.[Scheme_Code],  
	SC.[Scheme_Seq],  
	SC.[Scheme_Desc],  
	SC.[Scheme_Desc_Chi],   
	SC.[Scheme_Desc_CN],   
	SC.[Display_Code],  
	SC.[Display_Seq],  
	SC.[BalanceEnquiry_Available],  
	SC.[IVRS_Available],  
	SC.[TextOnly_Available],  
	SC.[Claim_Period_From],  
	SC.[Claim_Period_To],  
	SC.[Create_By],  
	SC.[Create_Dtm],  
	SC.[Update_By],  
	SC.[Update_Dtm],  
	SC.[Record_Status],  
	SC.[Effective_dtm],  
	SC.[Expiry_dtm],  
	SC.[PreFill_Search_Available],  
	SC.[TSWCheckingEnable],
	SC.[Control_Type],
	SC.[Control_Setting],
	SC.[PCSClaim_Available],
	SC.[Confirmed_Transaction_Status],
	SC.[Reimbursement_Mode],
	SC.[Reimbursement_Currency],
	SC.[Available_HCSP_SubPlatform],
	SC.[ProperPractice_Avail],
	SC.[ProperPractice_SectionID],
	SC.[Readonly_HCSP],
	SC.[AllowTempAccBOClaim]
FROM
	SchemeClaim SC WITH (NOLOCK)
WHERE 
	[Scheme_Code] IN  
	(  
		SELECT DISTINCT 
			SECM.Scheme_Code_Claim  
		FROM 
			PracticeSchemeInfo PSI WITH (NOLOCK), SchemeEnrolClaimMap SECM WITH (NOLOCK)
		WHERE 
			PSI.[Scheme_Code] = SECM.[Scheme_Code_Enrol]  
			AND PSI.[SP_ID] = @sp_id  
			AND PSI.[Practice_Display_Seq] = @practice_id  
			AND EXISTS 
			(  
				SELECT 
					1
				FROM 
					UserRole UR WITH (NOLOCK), RoleSecurity RS WITH (NOLOCK) 
				WHERE 
					UR.[role_type] = RS.[Role_Type]  
					AND UR.[User_ID] = @userid  
					AND RS.[Function_Code] = @functin_code 
					AND UR.[Scheme_Code] = SECM.[Scheme_Code_Claim]
			)  
	)  
ORDER BY Display_Seq  
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaim_get_EnrolledScheme_bySPID_PracticeID] TO HCVU
GO

