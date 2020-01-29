IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformation_get_bySPID_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformation_get_bySPID_all]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Lawrence TSANG
-- Modified date:	21 July 2015
-- Description:		SProc is replaced by [proc_SchemeInformation_get_bySPID]
-- =============================================
-- =============================================
-- Author:	PAK HO LEE
-- Create date: 21 August 2010
-- Description:	Retrieve the Scheme Information from Table SchemeInformation
--				Copy from [proc_SchemeInformation_get_bySPID_ServiceDate]
--              But do not check effective date and return all
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_SchemeInformation_get_bySPID_all]
	@sp_id			char(8)
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
DECLARE @EffectiveScheme table (
	Scheme_Code	char(10),
	Scheme_Seq	int
)

INSERT INTO @EffectiveScheme
SELECT
	Scheme_Code,
	MAX(Scheme_Seq)
FROM
	SchemeBackOffice
WHERE
	Record_Status = 'A'
		AND Effective_Dtm < GETDATE()
	GROUP BY Scheme_Code 

-- =============================================
-- Return results
-- =============================================

/*********************************************** 
	[Record_Status] reference:
		Active						A  
		Suspended					W  
		DelistedVoluntary			V  
		DelistedInvoluntary			I  
		ActivePendingSuspend		S  
		ActivePendingDelist			D  
		SuspendedPendingDelist		X  
		SuspendedPendingReactivate	Y  
 **********************************************/

	SELECT		S.SP_ID,
				S.Scheme_Code,
				CASE S.Record_Status
					WHEN 'A' THEN
						ISNULL(M.Upd_Type, 'A')
					WHEN 'S' THEN
						CASE ISNUll(M.Upd_Type, '')
							WHEN 'R' THEN
								'Y'
							WHEN 'D' THEN
								'X'
							ELSE 'W'
						END
					WHEN 'D' THEN
						ISNUll(S.Delist_Status, '')
				END [Record_Status],
				CASE S.Record_Status
					WHEN 'A' THEN 
						CASE ISNUll(M.Upd_Type, '')
							WHEN '' THEN
								''
							ELSE ISNUll(M.Remark, '')
						END
					WHEN 'S' THEN
						CASE ISNULL(M.Upd_Type, '')
							WHEN 'D' THEN
								ISNULL(M.Remark, '')
							ELSE ISNUll(S.Remark, '')
						END
					WHEN 'D' THEN 
						ISNUll(S.Remark, '')
				END [Remark],
				S.Delist_Status,
				S.Effective_Dtm,
				S.Delist_Dtm,
				S.Logo_Return_Dtm,
				S.Create_Dtm,
				S.Create_By,
				S.Update_Dtm,
				S.Update_By,
				S.TSMP,
				B.Display_Seq
				
	FROM		SchemeInformation S
					LEFT JOIN SPAccountMaintenance M
						ON S.SP_ID = M.SP_ID
							AND S.Scheme_Code = M.Scheme_Code
							AND M.Record_Status = 'A'
							AND M.SP_Practice_Display_Seq IS NULL
						 INNER JOIN @EffectiveScheme E
						ON S.Scheme_Code = E.Scheme_Code
					     INNER JOIN SchemeBackOffice B 
						ON S.Scheme_Code = B.Scheme_Code AND B.Scheme_Seq = E.Scheme_Seq
						 
					
	WHERE		S.SP_ID = @sp_id
	
	ORDER BY	M.System_Dtm DESC
	
END
--GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_get_bySPID_all] TO HCSP
--GO
*/
