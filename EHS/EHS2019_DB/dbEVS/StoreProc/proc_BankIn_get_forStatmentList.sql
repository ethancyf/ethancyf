IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankIn_get_forStatmentList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankIn_get_forStatmentList]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			INT19-0004
-- Modified by:		Koala CHENG
-- Modified date:	17 Apr 2019
-- Description:		Tune Performance
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No.:			I-CRE17-007
-- Modified by:		Chris YIM
-- Modified date:	14 February 2018
-- Description:		Tune Performance
-- =============================================  
-- =============================================
-- Modification History
-- CR No.:			INT17-0022
-- Modified by:		Koala CHENG
-- Modified date:	05 March 2018
-- Description:		Performance Tuning
--					1. Add WITH RECOMPILE
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE17-005
-- Modified by:		Koala CHENG
-- Modified date:	15 January 2018
-- Description:		Performance Tuning
--					1. Reorder table join
--					2. Remove WITH RECOMPILE
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	20 April 2011
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		31-07-2008
-- Description:		Get top 6 latest Reimbursement ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 August 2009
-- Description:		Change the INNER JOIN [ReimbursementAuthorisation] criteria:
--						(1) [Record_Status] from '2' to 'A'
--						(2) [Authorised_Status] to 'R'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	27 August 2009
-- Description:		Add inner join criteria [SchemeCode] on tables [ReimbursementAuthorisation] and [ReimbursementAuthTran]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	04 Oct 2009	
-- Description:		Change the no. of statement to be read by parameter
-- =============================================
CREATE PROCEDURE [dbo].[proc_BankIn_get_forStatmentList]
	@sp_id					CHAR(8),
	@practice_display_seq	SMALLINT
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Return results
-- =============================================

DECLARE @count INT

SELECT	@count = Parm_Value1
	FROM	dbo.SystemParameters
	WHERE	Parameter_name = 'ShowNoOfMthStatement' and
		Record_Status = 'A'
		
--SET ROWCOUNT @count

	SELECT
		DISTINCT TOP (@count)
		A.Reimburse_ID,
		R.CutOff_Date,
		B.Value_Date
	FROM
		(
		--SELECT 
		--	DISTINCT
		--	RA.Reimburse_ID,
		--	RA.Scheme_Code
		--FROM 	
		--	ReimbursementAuthTran RA WITH (NOLOCK)
		--WHERE
		--	EXISTS (SELECT 1 FROM VoucherTransaction T WITH (NOLOCK) WHERE Transaction_ID= RA.Transaction_ID AND SP_ID = @sp_id AND Practice_Display_Seq = @practice_display_seq)
			select Reimburse_ID, Scheme_Code from BankInDataFile WITH (NOLOCK)
			WHERE LEFT(Second_Party_Identifier,8) = @sp_id
			AND CAST(RIGHT(RTRIM(Second_Party_Identifier), LEN(Second_Party_Identifier)-8) AS INT) = @practice_display_seq
		) A

		LEFT JOIN ReimbursementAuthorisation R WITH (NOLOCK)
			ON R.Reimburse_ID = A.Reimburse_ID
				AND R.Scheme_Code = A.Scheme_Code
				AND R.Authorised_Status = 'R'
				AND R.Record_Status = 'A'

		INNER JOIN BankIn B WITH (NOLOCK)
			ON B.Reimburse_ID = A.Reimburse_ID
				AND B.Scheme_Code = A.Scheme_Code
				AND B.Transaction_File_Link = B.Reimburse_ID

	ORDER BY
		R.CutOff_Date DESC
		
END
GO

GRANT EXECUTE ON [dbo].[proc_BankIn_get_forStatmentList] TO HCSP
GO
